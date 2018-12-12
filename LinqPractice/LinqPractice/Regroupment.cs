using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using EfDAL.Models;

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
            //FindMode();
            //GroupByL2S();
            //GroupBySubgroupBy();  // this one needs correction. There are too many duplicate groups
            //GetTotalStudentsSubjectWise();
            //GetTotalStudentsSubjectWiseInEachClass();
            //GroupByMultiple();
            GroupByMultiple2();
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

        static void GroupByL2S()
        {
            // Group students and their enrolled subjects with respect to the classes

            var entities = new Entities();
            var studentsArr = entities.AspNetStudents.ToArray();
            var enrollmentsArr = entities.AspNetStudent_Enrollments.ToArray();
            var classCoursesArr = entities.AspNetClass_Courses.ToArray();

            var result = from s in studentsArr
                         join e in enrollmentsArr
                         on s.Id equals e.StudentId
                         join c in classCoursesArr
                         on e.CourseId equals c.CourseId
                         group (s.Name, e.AspNetCours.Name, c.AspNetClass.Name) by c.AspNetClass.Name;

            foreach (var group in result)
            {
                Console.WriteLine();
                Console.WriteLine($"Class {group.Key}");
                Console.WriteLine("----------------------------------------------------------------");
                int lineCounter = 0;
                foreach (var item in group)
                {
                    if (lineCounter++ % 3 == 0)
                    {
                        Console.WriteLine();
                    }
                    Console.Write("{0,-35}", $"({item.Item1}, {item.Item2})");
                }
                Console.WriteLine();
            }
        }

        static void GroupBySubgroupBy()
        {
            // Group students and thier enrolled subjects with respect to their classes. Further group the students in each class with respect to the subjects.

            var entities = new Entities();
            var studentsArr = entities.AspNetStudents.ToArray();
            var enrollmentsArr = entities.AspNetStudent_Enrollments.ToArray();
            var classCoursesArr = entities.AspNetClass_Courses.ToArray();

            var result = from s in studentsArr
                         join e in enrollmentsArr
                         on s.Id equals e.StudentId
                         group new DTO.Student
                         {
                             StudentDto = s,
                             CourseDto = e.AspNetCours
                         } by e.AspNetCours
                         into ogroup
                         from o in ogroup
                         join c in classCoursesArr
                         on o.CourseDto.Id equals c.CourseId
                         group new
                         {
                             Class = c.AspNetClass,
                             Subgroups = ogroup
                         } by c.AspNetClass.Name;

            File.AppendAllText("output.txt", $"{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}-------------------------------------------------------------------{DateTime.Now}Subgroups:{Environment.NewLine}{Environment.NewLine}");
            int count = 0;
            foreach (var item in result)
            { 
                File.AppendAllText("output.txt", $"{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}");
                var k1 = item.Key;
                File.AppendAllText("output.txt", $"{Environment.NewLine}Class (k1): {k1}");
                foreach (var item2 in item)
                {
                    var k2 = item2.Class;
                    File.AppendAllText("output.txt", $"{Environment.NewLine}Course (k2): {k2.Name}");
                    foreach (var item3 in item2.Subgroups)
                    {
                        File.AppendAllText("output.txt", $"{Environment.NewLine}item 3 params: ({ item3.StudentDto.Name}, { item3.CourseDto.Name})");
                        if(count++ % 20 == 0)
                        {
                            Console.WriteLine($"Writing entry {count}");
                        }
                    }
                }
            }

            Console.WriteLine("Output written to output.txt");
        }

        static void GetTotalStudentsSubjectWise()
        {
            // Method to Get total number of students in each subject

            var entities = new Entities();
            var studentsArr = entities.AspNetStudents.ToArray();
            var enrollmentsArr = entities.AspNetStudent_Enrollments.ToArray();
            var classCoursesArr = entities.AspNetClass_Courses.ToArray();

            var groups = from s in studentsArr
                         join e in enrollmentsArr
                         on s.Id equals e.StudentId
                         join c in classCoursesArr
                         on e.CourseId equals c.CourseId
                         group (s.Name, c.AspNetClass.Name, e.AspNetCours.Name) by e.AspNetCours.Name
                         into studentClassGroups
                         select new
                         {
                             Course = studentClassGroups.Key,
                             NumberOfStudents = studentClassGroups.Count()
                         }
                         into studentCounts
                         orderby studentCounts.NumberOfStudents
                         select studentCounts;
            Console.WriteLine();
            Console.WriteLine("  |------------------------------------------------|");
            Console.WriteLine("  | {0,-25} | {1, -16} |", "COURSE", "NUMBER OF STUDENTS");
            Console.WriteLine("  |------------------------------------------------|");
            foreach (var item in groups)
            {
                Console.WriteLine($"  | {item.Course,-25} | {item.NumberOfStudents, -18} |");
            }
            Console.WriteLine("  |------------------------------------------------|");
        }

        static void GetTotalStudentsSubjectWiseInEachClass()
        {
            // Method to get the total number of students grouped by subjects in each class
            var entities = new Entities();
            var studentsArr = entities.AspNetStudents.ToArray();
            var enrollmentsArr = entities.AspNetStudent_Enrollments.ToArray();
            var classCoursesArr = entities.AspNetClass_Courses.ToArray();

            Console.WriteLine();
            var classes = new[] { "5th", "6th", "7th", "8th", "9th", "10th", "11th", "12th", };
            foreach (var @class in /*entities.AspNetClasses.OrderBy(c => c.Id)*/classes)
            {
                Console.WriteLine("  |------------------------------------------------|");
                Console.WriteLine($"  |                     {@class,4}                       |");
                Console.WriteLine("  |------------------------------------------------|");
                Console.WriteLine("  | {0,-25} | {1, -16} |", "COURSE", "NUMBER OF STUDENTS");
                Console.WriteLine("  |------------------------------------------------|");
                var groups = from s in studentsArr
                             join e in enrollmentsArr
                             on s.Id equals e.StudentId
                             join c in classCoursesArr
                             on e.CourseId equals c.CourseId
                             where c.AspNetClass.Name.Equals(@class, StringComparison.OrdinalIgnoreCase)
                             group (s.Name, c.AspNetClass.Name, e.AspNetCours.Name) by e.AspNetCours.Name
                             into studentClassGroups
                             select new
                             {
                                 Course = studentClassGroups.Key,
                                 NumberOfStudents = studentClassGroups.Count()
                             }
                             into studentCounts
                             orderby studentCounts.NumberOfStudents
                             select studentCounts;
                foreach (var item in groups)
                {
                    Console.WriteLine($"  | {item.Course,-25} | {item.NumberOfStudents,-18} |");
                }
                Console.WriteLine("  |------------------------------------------------|");
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        static void GroupByMultiple()
        {
            var names = new[] { "Arslan Ahmad", "Asim Kabir", "Usman Kabir" };

            var result = from n in names
                         group n by new { FirstLetter = n[0], Length = n.Length };
            foreach (var item in result)
            {
                Console.WriteLine("Key: " + item.Key);
                foreach (var item2 in item)
                {
                    Console.WriteLine(item2);
                }
            }
        }

        static void GroupByMultiple2()
        {
            var entities = new Entities();
            var studentsArr = entities.AspNetStudents.ToArray();
            var enrollmentsArr = entities.AspNetStudent_Enrollments.ToArray();
            var classCoursesArr = entities.AspNetClass_Courses.ToArray();

            var result = from s in studentsArr
                         join e in enrollmentsArr
                         on s.Id equals e.StudentId
                         join c in classCoursesArr
                         on e.CourseId equals c.CourseId
                         select new
                         {
                             Student = s.Name,
                             Course = c.AspNetCours.Name,
                             Class = c.AspNetClass.Name
                         } into scc
                         group scc by new
                         {
                             scc.Course,
                             scc.Class
                         }
                         into groups
                         orderby groups.Key.Course
                         select groups;
            foreach (var group in result)
            {
                Console.WriteLine(group.Key);
                foreach (var member in group)
                {
                    Console.WriteLine(member);
                }
                Console.WriteLine();
                Console.WriteLine();
            }
        }
    }
}
