using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqPractice
{
    static class MéthodesConversion
    {
        public static void Début()
        {
            //DemoSuccessfulConversions();
            //DemoUnsuccessfulConversions();
            //Subtlety();
            CastUsingQuery();
        }

        static void DemoSuccessfulConversions()
        {
            var ngList = new ArrayList(new [] { 1, 2, 3, 4 });
            Console.WriteLine($"Type of {nameof(ngList)}: {ngList.GetType().Name}");

            var convertedWithCast = ngList.Cast<int>();
            var convertedWithOfType = ngList.OfType<int>();

            Console.WriteLine();
            Console.WriteLine($"Type of {nameof(convertedWithCast)}: {convertedWithCast.GetType().Name}");
            Console.WriteLine($"Elements:");
            convertedWithCast.ToList().ForEach(i => Console.Write($"{i}   "));

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"Type of {nameof(convertedWithOfType)}: {convertedWithOfType.GetType().Name}");
            Console.WriteLine($"Elements:");
            convertedWithOfType.ToList().ForEach(i => Console.Write($"{i}   "));
        }

        static void DemoUnsuccessfulConversions()
        {
            var ngList = new ArrayList()
            {
                1, 2, "A string", 3, 4, DateTime.Now, 5
            };
            Console.WriteLine($"Type of {nameof(ngList)}: {ngList.GetType().Name}");

            var convertedWithOfType = ngList.OfType<int>();
            Console.WriteLine();
            Console.WriteLine($"Type of {nameof(convertedWithOfType)}: {convertedWithOfType.GetType().Name}");
            Console.WriteLine($"Elements:");
            convertedWithOfType.ToList().ForEach(i => Console.Write($"{i}   "));

            Console.WriteLine();
            try
            {
                var convertedWithCast = ngList.Cast<int>();
                Console.WriteLine();
                Console.WriteLine($"Type of {nameof(convertedWithCast)}: {convertedWithCast.GetType().Name}");
                Console.WriteLine($"Elements:");
                convertedWithCast.ToList().ForEach(i => Console.Write($"{i}   "));
            }
            catch (InvalidCastException ex)
            {
                Console.WriteLine("Could not convert using Cast. Details: " + ex.Message);
            }
        }

        static void Subtlety()
        {
            // Trying to cast IEnumerable<int> to IEnumerable<long> fails. Why? HINT: object --> long conversion (unboxing): is it possible?
            var ints = new[] { 1, 2, 3, 4, 5 };
            Console.WriteLine("Original ints count: " + ints.Count());

            IEnumerable<long> longWithOfType = ints.OfType<long>();
            Console.WriteLine("After converting to long with OfType, count: " + longWithOfType.Count());

            try
            {
                IEnumerable<long> longWithCast = ints.Cast<long>(); // THIS LINE WON'T THROW AN EXCEPTION!
                Console.WriteLine("After converting to long with Cast, count: " + longWithCast.Count());    // THIS LINE WILL DO BECAUSE ENUMERATION IS DONE HERE
            }
            catch(InvalidCastException ex)
            {
                Console.WriteLine("Could not convert using Cast. Exeption details: " + ex.Message);
            }

            IEnumerable<long> longWithProjection = ints.Select(i => (long)i);
            Console.WriteLine("Projected (not converted/cast) to long, count: " + longWithProjection.Count());
        }

        static void CastUsingQuery()
        {
            // OfType doesn't have query equivalent
            var ngList = new ArrayList(new[] { 1, 2, 3, 4 });

            var result = from int i in ngList
                         select i;

            foreach (var item in result)
            {
                Console.WriteLine(item.GetType().Name + ", " + item);
            }
        }
    }
}
