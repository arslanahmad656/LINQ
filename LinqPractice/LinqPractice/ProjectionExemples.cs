﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqPractice
{
    static class ProjectionExemples
    {
        public static void Début()
        {
            ProductUsingSelectMany();
        }

        static void ProductUsingSelectMany()
        {
            string[] source1 = new[] { "a", "b", "c" },
                source2 = new[] { "i", "ii", "iii" },
                source3 = new[] { "1", "2", "3" };

            UsingQuerySyntax().ForEach(p => Console.WriteLine(p));

            List<string> UsingQuerySyntax()
            {
                return
                    (from s1 in source1
                     from s2 in source2
                     from s3 in source3
                     select $"({s1}, {s2}, {s3})").ToList();
                // How to do this simple operation using the fluent syntax?
            }
        }

        static void QuerySyntaxVsFluent()
        {
            var source = new[] { "Harry James Potter", "Severus Snivilleus Snape", "Albus Wulfric Perceival Brian Dumbledore", "Dolores Janes Umbrdige", "Minevera McGonagall" };
            // try printing the single names alongwith their full names sorted first according the fullname and then by name
            // using query syntax is easier since many range variables can be in scope; this is not the case, however, with the fluent syntax.

            var listUsingQuerySyntax = UsingQuerySyntax();
            var listUsingFluentSyntax = UsingFluentSyntax();
            Console.WriteLine("Printing list obtained using the query syntax: ");
            PrintNames(listUsingQuerySyntax);
            Console.WriteLine();
            Console.WriteLine("Printing list obtained using the fluent syntax: ");
            PrintNames(listUsingFluentSyntax);

            List<(string name, string fullName)> UsingQuerySyntax()
            {
                return
                    (from fullname in source
                     from name in fullname.Split(' ')
                     orderby fullname, name
                     select (name, fullname)).ToList();
                // That easy since both fullname and name are in scope
            }

            List<(string name, string fullName)> UsingFluentSyntax()
            {
                return source
                        .SelectMany(fn => fn.Split(' ')
                            .Select(n => new { Name = n, FullName = fn }))  // we had to this rola to overcome the scope limitation of fn
                        .OrderBy(n => n.FullName)
                        .ThenBy(n => n.Name)
                        .Select(n => (n.Name, n.FullName))
                        .ToList();
                // See? So much rola to handle the scope limitation
            }

            void PrintNames(List<(string name, string fullname)> list)
            {
                foreach (var (name, fullname) in list)
                {
                    Console.WriteLine($"{name} came from {fullname}");
                }
            }
        }

        static void SelectVsSelectMany()
        {
            var source = new[] { "Harry James Potter", "Severus Snivilleus Snape", "Albus Wulfric Perceival Brian Dumbledore", "Dolores Janes Umbrdige", "Minevera McGonagall" };
            var selected = DemoSelect();
            Console.WriteLine($"SELECT:");
            Console.WriteLine($"  Type: {selected.GetType().Name}");
            Console.WriteLine($"  Count: {(selected as IEnumerable<object>).Count()}");
            Console.WriteLine($"  Type of a member: {(selected as IEnumerable<object>).First().GetType().Name}");

            var selectedMany = DemoSelectMany();
            Console.WriteLine($"SELECT MANY:");
            Console.WriteLine($"  Type: {selectedMany.GetType().Name}");
            Console.WriteLine($"  Count: {(selectedMany as IEnumerable<object>).Count()}");
            Console.WriteLine($"  Type of a member: {(selectedMany as IEnumerable<object>).First().GetType().Name}");

            object DemoSelect()
            {
                return source
                    .Select(s => s.Split(' '))
                    .ToArray();
            }

            object DemoSelectMany()
            {
                return source
                    .SelectMany(s => s.Split(' '))
                    .ToArray();
            }
        }
    }
}
