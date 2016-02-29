using System;
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
            if (args.Length < 3)
            {
                Console.WriteLine("Invalid number of arguments. Please supply BuildId, ProjectName and BuildRoot");
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
                    BuildRoot = args[2]
                };

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
                // Get the component to update
                var component = _componentsService.GetItem(args.BuildId);

                // Get the existing package list for the component
                var packages = _analysisService.Analyse(args.BuildRoot);

                // Enumerate the list and assign to the retrieved component
                component.Packages = packages.ToList();

                // Get all of the packages with multiple versions
                var duplicatedPackages = component.Packages.GroupBy(p => p.Name).Where(grp => grp.Count() > 1).Select(dp => new { Name = dp.Key }).ToList();

                // If there are duplicates. Consolidation is needed so list the packages to the console and exit
                if (duplicatedPackages.Any())
                {
                    foreach (var package in duplicatedPackages)
                    {
                        Console.WriteLine($"NuGet consolidation issue: {package.Name}");

                        foreach (var pack in component.Packages.Where(p => p.Name == package.Name))
                        {
                            Console.WriteLine($"\t{pack.Version} ({pack.TargetFramework})");

                            foreach (var project in pack.Projects)
                            {
                                Console.WriteLine($"\t\tProject: {project.Name}");
                            }
                        }
                    }
                }
                else
                {
                    // All packages are distinct so update the database.
                    _analysisService.Upload(component).GetAwaiter().GetResult();

                    Console.WriteLine("Packages processed successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
#if !DEBUG
                Environment.Exit(1);
#endif
            }
        }
    }
}