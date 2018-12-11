using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LinqPractice
{
    static class Regroupment
    {
        private static readonly string[] _source;

        static Regroupment()
        {

            _source = Directory.GetFiles(Path.GetTempPath()).Select(f =>
            {
                var indexOfDot = f.LastIndexOf('.');
                var indexOfSlash = f.LastIndexOf('\\');
                var length = indexOfDot - indexOfSlash - 1;
                string selectedText;
                if (length > 0)
                {
                    selectedText = f.Substring(indexOfSlash + 1, length);
                }
                else
                {
                    selectedText = f.Substring(indexOfSlash);
                }
                return selectedText;
            }).ToArray();
        }

        public static void Début()
        {
            //DemoGroupBy();
            //GroupBySorted();
            //GroupBySortedAndFiltered();
            FindMode();
        }

        static void DemoGroupBy()
        {
            Console.WriteLine($"Using Query Syntax:");
            UsingQuery();
            Console.WriteLine();
            Console.WriteLine("Using Fluent Syntax:");
            UsingFluent();

            void UsingQuery()
            {
                var result = from f in _source
                             group f by Path.GetExtension(f);
                PrintResult(result);
            }

            void UsingFluent()
            {
                var result = _source.GroupBy(f => Path.GetExtension(f));
                PrintResult(result);
            }
        }

        static void PrintResult(IEnumerable<IGrouping<string, string>> sequence)
        {
            foreach (var group in sequence)
            {
                Console.WriteLine($"Extension {group.Key}");
                foreach (var item in group)
                {
                    Console.WriteLine($"   - {item}");
                }
                Console.WriteLine();
            }
        }

        static void GroupBySorted()
        {
            var result = from f in _source
                         group f by Path.GetExtension(f)
                         into groups
                         orderby groups.Key
                         select groups; // The result is not IEnumerable<,>
            Console.WriteLine("Groups sorted by group key:");
            PrintResult(result);
        }

        static void GroupBySortedAndFiltered()
        {
            var result = from f in _source
                         group f by Path.GetExtension(f)
                         into groups
                         where groups.Count() < 10
                         orderby groups.Key
                         select groups;
            Console.WriteLine("Groups having files lesser than 10 and sorted by extension:");
            PrintResult(result);
        }

        static void FindMode()
        {
            var data = new[] { 2, 3, 4, 1, 1, 2, 4, 1, 1 };

            var numGroups = from i in data
                       group i by i
                       into groups
                       orderby groups.Count() descending
                       select groups.Key;
            Console.WriteLine($"Most frequently appearing integer: {numGroups.First()}");
        }
    }
}
