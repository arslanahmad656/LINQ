using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EfDAL.Models;

namespace LinqPractice
{
    class JoindreExemples
    {
        public static void Début()
        {
            //JoinVsSelectMany();
            //LimitingInnerResult();
            //MultiVarJoin();
            //LimitingDeepInnerResult();
            //DemoGroupJoin();
            //DemoGroupJoinFiltered();
            DemoGroupJoinFlattened();
        }

        static void JoinVsSelectMany()
        {
            // SelectMany is efficient for L2S
            // Join is efficient for local quries
            var entities = new Entities();

            // db sources converted to local sources
            var studentsArr = entities.AspNetStudents.ToArray();
            var enrollmentsArr = entities.AspNetStudent_Enrollments.ToArray();

            // The results should be the same for both demos
            Console.WriteLine("Using Join:");
            DemoJoin(); // this will perform innerjoin

            Console.WriteLine();
            Console.WriteLine("Using SelectMany:");
            DemoSelectMany();

            void DemoJoin()
            {
                var result = from s in studentsArr
                             join e in enrollmentsArr
                             on s.Id equals e.StudentId
                             select $"{s.Name} is enrolled in {e.AspNetCours.Name}";
                result.ToList().ForEach(r => Console.WriteLine(r));
            }

            void DemoSelectMany()
            {
                var result = from s in studentsArr
                             from e in enrollmentsArr
                             where s.Id == e.StudentId
                             select $"{s.Name} is enrolled in {e.AspNetCours.Name}";
                result.ToList().ForEach(r => Console.WriteLine(r));
            }
        }

        static void MultiVarJoin()
        {
            var entities = new Entities();
            var studentsArr = entities.AspNetStudents.ToArray();
            var enrollmentsArr = entities.AspNetStudent_Enrollments.ToArray();
            var classCoursesArr = entities.AspNetClass_Courses.ToArray();

            // Results should be the same
            Console.WriteLine("Using Query Syntax:");
            UsingQuery();
            Console.WriteLine();
            Console.WriteLine("Using Fluent Syntax:");
            UsingFluent();

            void UsingQuery()
            {
                var result = from s in studentsArr
                             join e in enrollmentsArr
                             on s.Id equals e.StudentId
                             join c in classCoursesArr
                             on e.CourseId equals c.CourseId
                             select $"{s.Name} enrolled in {e.AspNetCours.Name} in {c.AspNetClass.Name} class";
                result.ToList().ForEach(r => Console.WriteLine(r));
            }

            void UsingFluent()
            {
                var result = studentsArr
                            .Join(enrollmentsArr,
                                s => s.Id,
                                e => e.StudentId,
                                (s, e) => new { Student = s, Enrollment = e })
                            .Join(classCoursesArr, 
                                se => se.Enrollment.CourseId, 
                                se => se.CourseId, 
                                (se, c) => $"{se.Student.Name} enrolled in {se.Enrollment.AspNetCours.Name} in {c.AspNetClass.Name}");
                result.ToList().ForEach(r => Console.WriteLine(r));
            }
        }

        static void LimitingDeepInnerResult()
        {
            var entities = new Entities();
            var studentsArr = entities.AspNetStudents.ToArray();
            var enrollmentsArr = entities.AspNetStudent_Enrollments.ToArray();
            var classCoursesArr = entities.AspNetClass_Courses.ToArray();

            List<string> result = new List<string>();

            studentsArr
                .Take(2)
                .ToList()
                .ForEach(s => enrollmentsArr
                                .Where(e => e.StudentId == s.Id)
                                .Take(3)
                                .ToList()
                                .ForEach(e => classCoursesArr
                                                .Where(c => c.CourseId == e.CourseId)
                                                .Take(4)
                                                .ToList()
                                                .ForEach(c => result.Add($"{s.Name} enrolled in {e.AspNetCours.Name} in {c.AspNetClass.Name}"))));
            result.ToList().ForEach(r => Console.WriteLine(r));
        }

        static void LimitingInnerResult()
        {
            var entities = new Entities();
            var studentsArr = entities.AspNetStudents.ToArray();
            var enrollmentsArr = entities.AspNetStudent_Enrollments.ToArray();

            List<string> result = new List<string>();

            studentsArr
                .Take(2)
                .ToList()
                .ForEach(s => enrollmentsArr
                                .Where(e => e.StudentId == s.Id)
                                .Take(3)
                                .ToList()
                                .ForEach(e => result.Add($"{s.Name} is enrolled in {e.AspNetCours.Name}")));

            result.ForEach(r => Console.WriteLine(r));
        }

        static void DemoGroupJoin()
        {
            // this will perform the outer join without any filtering.
            // That means that students without any enrollments will also be listed
            var entities = new Entities();
            var studentsArr = entities.AspNetStudents.ToArray();
            var enrollmentsArr = entities.AspNetStudent_Enrollments.ToArray();

            var result = from s in studentsArr
                         join e in enrollmentsArr
                         on s.Id equals e.StudentId
                         into studentCourses
                         select new
                         {
                             Student = s.Name,
                             Courses = studentCourses.Select(c => c.AspNetCours.Name)
                         };
            foreach (var seq in result)
            {
                Console.WriteLine();
                Console.WriteLine($"{seq.Student}:");
                Console.Write("\t");
                foreach (var item in seq.Courses)
                {
                    Console.Write($"  -{item}");
                }
            }
        }

        static void DemoGroupJoinFiltered()
        {
            // Here the students without the courses will filtered out
            var entities = new Entities();
            var studentsArr = entities.AspNetStudents.ToArray();
            var enrollmentsArr = entities.AspNetStudent_Enrollments.ToArray();

            var result = from s in studentsArr
                         join e in enrollmentsArr
                         on s.Id equals e.StudentId
                         into studentCourses
                         where studentCourses.Any()
                         select new
                         {
                             Student = s.Name,
                             Courses = studentCourses.Select(c => c.AspNetCours.Name)
                         };
            foreach (var seq in result)
            {
                Console.WriteLine();
                Console.WriteLine($"{seq.Student}:");
                Console.Write("\t");
                foreach (var item in seq.Courses)
                {
                    Console.Write($"  -{item}");
                }
            }
        }

        static void DemoGroupJoinFlattened()
        {
            // All the students, irrespective of whether they have any course enrolled, will be enlisted

            var entities = new Entities();
            var studentsArr = entities.AspNetStudents.ToArray();
            var enrollmentsArr = entities.AspNetStudent_Enrollments.ToArray();

            var result = from s in studentsArr
                         join e in enrollmentsArr
                         on s.Id equals e.StudentId
                         into studentCourses
                         from sc in studentCourses.DefaultIfEmpty()
                         select (s.Name, sc?.AspNetCours?.Name);

            Console.WriteLine("Display Format: (Student Name, Course Name)");
            foreach (var item in result)
            {
                if (item.Item2 == null)
                {
                    Console.WriteLine($"{item.Item1} not enrolled.");
                }
                else
                {
                    Console.WriteLine($"{item.Item1} enrolled in {item.Item2}");
                }
            }
        }
    }
}
