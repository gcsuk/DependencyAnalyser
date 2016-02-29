using System;
using System.Linq;
using System.Threading.Tasks;
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

        private async void Execute(Args args)
        {
            try
            {
                var component = _componentsService.GetItem(args.BuildId);

                var packages = await Task.Run(() => _analysisService.Analyse(args.BuildRoot));

                component.Packages = packages.ToList();

                _analysisService.Upload(component).GetAwaiter().GetResult();

                Console.WriteLine("Packages processed successfully.");
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