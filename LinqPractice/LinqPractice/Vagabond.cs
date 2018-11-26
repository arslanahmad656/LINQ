using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqPractice
{
    static class Vagabond
    {
        public static void Début()
        {
            //LookupDemo();
            LookupDemo2();
        }

        static void LookupDemo()
        {
            var types = new[] { typeof(string), typeof(System.IO.BinaryReader), typeof(List<>), typeof(Enumerable) };
            var allTypes = types.Select(t => t.Assembly).SelectMany(a => a.GetTypes());
            Console.WriteLine($"Tous les types comptent: {allTypes.Count()}");
            var groupedByNamespace = allTypes.ToLookup(t => t.Namespace);
            Console.WriteLine($"Namespace clés comptent: {groupedByNamespace.Count}");
            string namespaceToLookIn = "System.IO";
            Console.WriteLine($"Les types dans namespace {namespaceToLookIn}:");
            int count = 0;
            foreach (var type in groupedByNamespace[namespaceToLookIn])
            {
                Console.WriteLine(type.Name);
                count++;
                if (count == 10)
                {
                    break;
                }
            }
            if(count != groupedByNamespace[namespaceToLookIn].Count())
            {
                Console.WriteLine($"et {groupedByNamespace.Count() - count} plus objets...");
            }
        }

        static void LookupDemo2()
        {
            List<Package> packages = new List<Package> { new Package { Company = "Coho Vineyard", Weight = 25.2, TrackingNumber = 89453312L },
                                                 new Package { Company = "Lucerne Publishing", Weight = 18.7, TrackingNumber = 89112755L },
                                                 new Package { Company = "Wingtip Toys", Weight = 6.0, TrackingNumber = 299456122L },
                                                 new Package { Company = "Contoso Pharmaceuticals", Weight = 9.3, TrackingNumber = 670053128L },
                                                 new Package { Company = "Wide World Importers", Weight = 33.8, TrackingNumber = 4665518773L } };

            ILookup<char, string> lookup = packages.ToLookup(p => p.Company[0], p => $"{p.Company} {p.TrackingNumber}");
            Console.WriteLine($"Tout les clés: {lookup.Count}");
            Console.WriteLine("Lister au dessus de Lookup:");
            foreach (IGrouping<char, string> item in lookup)
            {
                Console.WriteLine($"Les items dans le clé {item.Key}:");
                foreach (string val in item)
                {
                    Console.WriteLine($"\t{val}");
                }
            }
        }

        private class Package
        {
            public string Company;
            public double Weight;
            public long TrackingNumber;
        }
    }
}
