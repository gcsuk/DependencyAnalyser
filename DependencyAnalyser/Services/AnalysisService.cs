using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DependencyAnalyser.Services
{
    public class AnalysisService
    {
        private readonly List<string> _fileList;

        public AnalysisService()
        {
            _fileList = new List<string>();
        }

        public IEnumerable<Models.Package> Analyse(string targetDirectory)
        {
            var packages = new List<Models.Package>();

            DirectorySearch(targetDirectory, "*packages.config");

            foreach (var packageFile  in _fileList)
            {
                foreach (var assemblyPackage in AddToCatalog(packageFile))
                {
                    var existingPackage = packages.SingleOrDefault(f => f.ToString() == assemblyPackage.ToString());

                    var projectPathDirectories = Path.GetDirectoryName(packageFile).Split(new[] {Path.DirectorySeparatorChar}, StringSplitOptions.None);

                    var projectName = projectPathDirectories[projectPathDirectories.Length - 1];

                    if (existingPackage == null)
                    {
                        assemblyPackage.Projects.Add(new Models.Project {Name = projectName});
                        packages.Add(assemblyPackage);
                    }
                    else
                    {
                        existingPackage.Projects.Add(new Models.Project { Name = projectName });
                    }
                }
            }

            return packages.OrderBy(p => p.Name).ThenBy(v => v.Version);
        }

        private static IEnumerable<Models.Package> AddToCatalog(string packageFile)
        {
            var file = XDocument.Load(packageFile);

            if (file.Root != null)
            {
                var packages = file.Root.Elements("package");

                foreach (var package in packages)
                {
                    yield return new Models.Package
                    {
                        Name = package.Attribute("id").Value,
                        Version = package.Attribute("version").Value,
                        TargetFramework = package.Attribute("targetFramework").Value
                    };
                }
            }
        }

        private void DirectorySearch(string root, string pattern)
        {
            foreach (var directory in Directory.GetDirectories(root))
            {
                DirectorySearch(directory, pattern);
            }

            _fileList.AddRange(Directory.GetFiles(root, pattern).ToList());
        }

        public async Task<bool> Upload(Models.Component component)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:59561");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PostAsync("api/Packages", component, new JsonMediaTypeFormatter());

                return response.IsSuccessStatusCode;
            }
        }
    }
}
