using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqPractice
{
    class ConstruireQuestions
    {
        /**
         * Query: Remove all the vowels from a list of names, and then present in alphabetical order those whose length is still more than more than two characters.
         * 
        */

        static string[] _names = { "Tom", "Dick", "Harry", "Marry", "Jay" };

        public static void Début()
        {
            //BuildingProgressivelyFleuently();
            //BuildingProgressivelyQuerySyntaxCorrect();
            //BuildingProgressivelyQuerySyntaxWrong();
            //BuildingByWrapping();
            BuildingUsingInto();
        }

        static void BuildingProgressivelyFleuently()
        {
            var result = _names.Select(n => n.Replace("a", "").Replace("e", "").Replace("i", "").Replace("o", "").Replace("u", ""))
                .Where(n => n.Length > 2)
                .OrderBy(n => n);
            Console.WriteLine("Rêsultat de la requête:");
            result.All(n =>
            {
                Console.WriteLine(n);
                return true;
            });
        }

        static void BuildingProgressivelyQuerySyntaxCorrect()
        {
            var inter = from n in _names
                        select n.Replace("a", "").Replace("e", "").Replace("i", "").Replace("o", "").Replace("u", "");
            var final = from n in inter
                        where n.Length > 2
                        orderby n
                        select n;
            Console.WriteLine("Rêsultat de la requête:");
            final.All(n =>
            {
                Console.WriteLine(n);
                return true;
            });
        }

        static void BuildingProgressivelyQuerySyntaxWrong()
        {
            var result = from n in _names
                         where n.Length > 2
                         orderby n
                         select n.Replace("a", "").Replace("e", "").Replace("i", "").Replace("o", "").Replace("u", "");
            Console.WriteLine("Rêsultat de la requête:");
            result.All(n =>
            {
                Console.WriteLine(n);
                return true;
            });
        }

        static void BuildingByWrapping()
        {
            var result = from n in
                             (from n1 in _names
                              select n1.Replace("a", "").Replace("e", "").Replace("i", "").Replace("o", "").Replace("u", ""))
                         where n.Length > 2
                         orderby n
                         select n;
            Console.WriteLine("Rêsultat de la requête:");
            result.All(n =>
            {
                Console.WriteLine(n);
                return true;
            });
        }

        static void BuildingUsingInto()
        {
            var result = from n in _names
                         select n.Replace("a", "").Replace("e", "").Replace("i", "").Replace("o", "").Replace("u", "")
                         into nWithoutVowels
                         where nWithoutVowels.Length > 2
                         orderby nWithoutVowels
                         select nWithoutVowels;
            Console.WriteLine("Rêsultat de la requête:");
            result.All(n =>
            {
                Console.WriteLine(n);
                return true;
            });
        }
    }
}
