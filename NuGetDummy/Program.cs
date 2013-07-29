using System;
using Vertesaur;
using Vertesaur.PolygonOperation;

namespace NuGetDummy
{
    class Program
    {
        static void Main(string[] args) {

            Console.WriteLine("This exists because so far it is the easiest way to get NuGet packages without referencing them in the web project.");

            var square = new Polygon2(new [] {
                new Point2(0,0),
                new Point2(1,0),
                new Point2(1,1),
                new Point2(0,1)
            }, hole: false);

            var triangle = new Polygon2(new[] {
                new Point2(0.5, -0.5),
                new Point2(1,2),
                new Point2(0,2)
            }, hole: false);

            // subtract the triangle shape from the square shape
            var result = (Polygon2)new PolygonDifferenceOperation().Difference(square, triangle);
            for (int ringIndex = 0; ringIndex < result.Count; ringIndex++) {
                var ring = result[ringIndex];
                Console.WriteLine("ring: {0} hole: {1}", ringIndex, ring.Hole);
                foreach (var point in ring)
                    Console.WriteLine("\t{0}", point);
            }

            Console.ReadKey();

        }

    }
}


// Neat little code bit to get the nuget package data from the server
/*if (!Directory.Exists(NugetPackageFolder))
    Directory.CreateDirectory(NugetPackageFolder);

var nugetPackageRepository = PackageRepositoryFactory.Default.CreateRepository("https://nuget.org/api/v2/");
var latestVertesaurCore = nugetPackageRepository.GetPackages().Where(x => x.Id == "Vertesaur.Core").OrderByDescending(x => x.Version).FirstOrDefault();
var latestVertesaurGen = nugetPackageRepository.GetPackages().Where(x => x.Id == "Vertesaur.Generation").OrderByDescending(x => x.Version).FirstOrDefault();*/
