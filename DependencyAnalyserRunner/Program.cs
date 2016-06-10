using System;
using System.Collections.Generic;
using System.Linq;
using DependencyAnalyser.Models;

namespace DependencyAnalyser
{
    class Program
    {
        private readonly Services.IAnalysisService _analysisService;
        private readonly Services.IComponentsService _componentsService;

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("The application requires 5 parameters in order to function:");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Build Configuration ID: This is the build type in Team City, which can be found on the General Settings tab in your build job configuration. In Team City, pass %system.teamcity.buildType.id%");
                Console.WriteLine();
                Console.WriteLine("Project Name: This is the name of the component you are building. In Team City, pass %system.teamcity.projectName%");
                Console.WriteLine();
                Console.WriteLine("Build Root: This is the location of the directory to be scanned. In Team City, pass %teamcity.build.checkoutDir%");
                Console.WriteLine();
                Console.WriteLine("Consolidation Level: This is the amount of detail with regard to package consolidation that you wish to log to the Console (or in Team City, the Build Log)");
                Console.WriteLine("\t0 = None");
                Console.WriteLine("\t1 = Versions Only");
                Console.WriteLine("\t2 = .NET Frameworks Only");
                Console.WriteLine("\t3 = Both Versions and Frameworks");
                Console.WriteLine();
                Console.WriteLine("Consolidation Enforcement: Set to true if you want Team City to fail a build if packages are not consolidated. This is ignored if Consolidation Level is set to None.");
                Console.WriteLine();
                Console.WriteLine("Upload Results: Set to true if you want to upload package data to Dependency Summary API.");
                Console.WriteLine();
                Console.WriteLine("Press any key to close...");
                Console.ReadKey();

                Environment.Exit(0);
            }

            if (args.Length < 6)
            {
                Console.WriteLine("Invalid number of arguments. Please supply Build ID, Project Name, Build Root, Consolidation Level, Consolidation Enforced flag and Upload Results flag.");
#if !DEBUG
                Environment.Exit(1);
#endif
            }
            else
            {
                var arguments = new Args
                {
                    BuildId = args[0],
                    ProjectName = args[1],
                    BuildRoot = args[2],
                    ConsolidationLevel = (ConsolidationLevel) Convert.ToInt32(args[3]),
                    ConsolidationEnforced = Convert.ToBoolean(args[4]),
                    UploadResults = Convert.ToBoolean(args[5])
                };

                // Hidden feature that if an extra true is passed then exceptions will not be thrown
                if (args.Length > 6)
                {
                    arguments.MaskExceptions = Convert.ToBoolean(args[6]);
                }

                Console.WriteLine($"Arguments: {arguments}");

                new Program().Execute(arguments);
            }

#if DEBUG
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
#endif
            Environment.Exit(0);
        }

        public Program()
        {
            _analysisService = new Services.AnalysisService(Properties.Settings.Default.ApiUrl);
            _componentsService = new Services.ComponentsService(Properties.Settings.Default.ApiUrl);
        }

        private void Execute(Args args)
        {
            try
            {
                Console.Write($"Getting component details for {args.BuildId}...");

                // Get the component to update
                var component = _componentsService.GetItem(args.BuildId);

                if (component == null)
                {
                    Console.WriteLine("The specified component was not found. Data will not be uploaded but a summary will still be created for the build log.");
                    component = new Component {Name = args.ProjectName};
                }

                Console.WriteLine("Done");
                Console.Write($"Getting package details for {args.BuildRoot}...");

                // Get the existing package list for the component and assign to the retrieved component
                component.Packages = _analysisService.Analyse(args.BuildRoot).ToList();

                Console.WriteLine("Done");

                var isConsolidated = true;

                if (args.ConsolidationLevel != ConsolidationLevel.None)
                {
                    isConsolidated = ProcessConsolidation(component.Packages, args.ConsolidationLevel);
                }
                
                if (isConsolidated)
                {
                    if (args.UploadResults && component.Id > 0)
                    {
                        _analysisService.Upload(component).GetAwaiter().GetResult();

                        Console.WriteLine("Packages processed and updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Packages processed successfully.");
                    }
                }
                else if (args.ConsolidationEnforced)
                {
                    Environment.Exit(1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                if (ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.Message);
                }
#if !DEBUG
                Environment.Exit(args.MaskExceptions ? 0 : 1);
#endif
            }
        }

        private bool ProcessConsolidation(IEnumerable<Package> packages, ConsolidationLevel consolidationLevel)
        {
            Console.WriteLine($"Processing consolidation data.");

            var duplicateItems =
                packages.GroupBy(p => p.Name)
                    .Where(grp => grp.Count() > 1)
                    .Select(dp => new {dp.Key})
                    .ToList();

            Console.WriteLine($"{duplicateItems.Count} NuGet packages found with multiple versions/frameworks.");

            // If there are duplicates. Consolidation is needed so list the packages to the console and exit
            if (duplicateItems.Any())
            {
                switch (consolidationLevel)
                {
                    case ConsolidationLevel.VersionOnly:

                        foreach (var duplicate in duplicateItems)
                        {
                            var uniqueVersions =
                                packages.Where(p => p.Name == duplicate.Key.ToString())
                                    .Select(v => v.Version)
                                    .Distinct()
                                    .ToList();

                            if (uniqueVersions.Count() > 1)
                            {
                                Console.WriteLine($"NuGet consolidation issue: {duplicate.Key}");

                                foreach (var uniqueVersion in uniqueVersions)
                                {
                                    Console.WriteLine($"\tv{uniqueVersion}");

                                    foreach (
                                        var package in
                                            packages.Where(
                                                p =>
                                                    p.Name == duplicate.Key.ToString() &&
                                                    p.Version == uniqueVersion))
                                    {
                                        foreach (var project in package.Projects)
                                        {
                                            Console.WriteLine($"\t\tProject: {project.Name} (Path: {project.Path})");
                                        }
                                    }
                                }
                            }
                        }

                        return false;

                    case ConsolidationLevel.FrameworkOnly:

                        foreach (var duplicate in duplicateItems)
                        {
                            var uniqueFrameworks =
                                packages.Where(p => p.Name == duplicate.Key.ToString())
                                    .Select(v => v.TargetFramework)
                                    .Distinct()
                                    .ToList();

                            if (uniqueFrameworks.Count() > 1)
                            {
                                Console.WriteLine($"NuGet consolidation issue: {duplicate.Key}");

                                foreach (var uniqueFramework in uniqueFrameworks)
                                {
                                    Console.WriteLine($"\tFramework {uniqueFramework}");

                                    foreach (
                                        var package in
                                            packages.Where(
                                                p =>
                                                    p.Name == duplicate.Key.ToString() &&
                                                    p.TargetFramework == uniqueFramework))
                                    {
                                        foreach (var project in package.Projects)
                                        {
                                            Console.WriteLine($"\t\tProject: {project.Name} (Path: {project.Path})");
                                        }
                                    }
                                }
                            }
                        }

                        return false;

                    case ConsolidationLevel.VersionAndFramework:

                        foreach (var duplicate in duplicateItems)
                        {
                            Console.WriteLine($"NuGet consolidation issue: {duplicate.Key}");

                            foreach (var package in packages.Where(p => p.Name == duplicate.Key.ToString()))
                            {
                                Console.WriteLine($"\tv{package.Version} (Framework {package.TargetFramework})");

                                foreach (var project in package.Projects)
                                {
                                    Console.WriteLine($"\t\tProject: {project.Name} (Path: {project.Path})");
                                }
                            }
                        }

                        return false;
                }
            }

            return true;
        }
    }
}