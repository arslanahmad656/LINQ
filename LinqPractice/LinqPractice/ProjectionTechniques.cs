using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqPractice
{
    static class ProjectionTechniques
    {
        static string[] _names = { "Tom", "Dick", "Harry", "Marry", "Jay" };

        public static void Début()
        {
            //ProjectingUsingObjectInitializer();
            //ProjectingUsingAnonymousObjects();
            ProjectingUsingLetKeyWord();
        }

        static void ProjectingUsingObjectInitializer()
        {
            var originalAndTransformed = (from n in _names
                                          select new ProjectionItem
                                          {
                                              original = n,
                                              withoutVowel = n.Replace("a", "").Replace("e", "").Replace("i", "").Replace("o", "").Replace("u", "")
                                          }
                                         into transformed
                                          where transformed.withoutVowel.Length > 2
                                          select transformed).ToList();
            Console.WriteLine("Aprês transformation");
            Console.WriteLine("Original, transformed:");
            originalAndTransformed.ForEach(n => Console.WriteLine($"{n.original}, {n.withoutVowel}"));
        }

        static void ProjectingUsingAnonymousObjects()
        {
            var originalAndTransformed = (from n in _names
                                          select new
                                          {
                                              original = n,
                                              withoutVowel = n.Replace("a", "").Replace("e", "").Replace("i", "").Replace("o", "").Replace("u", "")
                                          }
                                         into transformed
                                          where transformed.withoutVowel.Length > 2
                                          select transformed).ToList();
            Console.WriteLine("Aprês transformation");
            Console.WriteLine("Original, transformed:");
            originalAndTransformed.ForEach(n => Console.WriteLine($"{n.original}, {n.withoutVowel}"));
        }

        static void ProjectingUsingLetKeyWord()
        {
            var originalAndTransformed = from n in _names
                                         let withoutVowel = n.Replace("a", "").Replace("e", "").Replace("i", "").Replace("o", "").Replace("u", "")
                                         where withoutVowel.Length > 2
                                         select (n, withoutVowel);
            Console.WriteLine("Aprês transformation");
            Console.WriteLine("Original, transformed:");
            originalAndTransformed.ToList().ForEach(n => Console.WriteLine($"{n.n}, {n.withoutVowel}"));
        }

        private class ProjectionItem
        {
            public string original;
            public string withoutVowel;
        }
    }
    
}
