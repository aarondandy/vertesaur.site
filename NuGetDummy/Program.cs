using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NuGetDummy
{
    class Program
    {
        static void Main(string[] args) {

            Console.WriteLine("This exists because so far it is the easiest way to get NuGet packages without referencing them in the web project.");

            // Neat little code bit to get the nuget package data from the server
            /*if (!Directory.Exists(NugetPackageFolder))
                Directory.CreateDirectory(NugetPackageFolder);

            var nugetPackageRepository = PackageRepositoryFactory.Default.CreateRepository("https://nuget.org/api/v2/");
            var latestVertesaurCore = nugetPackageRepository.GetPackages().Where(x => x.Id == "Vertesaur.Core").OrderByDescending(x => x.Version).FirstOrDefault();
            var latestVertesaurGen = nugetPackageRepository.GetPackages().Where(x => x.Id == "Vertesaur.Generation").OrderByDescending(x => x.Version).FirstOrDefault();*/

        }
    }
}
