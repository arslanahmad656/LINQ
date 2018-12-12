using EfDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqPractice
{
    static class OpérateurSet
    {
        public static readonly Entities _entities;
        public static readonly AspNetStudent[] _students;
        public static readonly AspNetStudent_Enrollments[] _enrollments;
        public static readonly AspNetClass_Courses[] _classCourses;

        static OpérateurSet()
        {
            _entities = new Entities();
            _students = _entities.AspNetStudents.ToArray();
            _enrollments = _entities.AspNetStudent_Enrollments.ToArray();
            _classCourses = _entities.AspNetClass_Courses.ToArray();
        }

        public static void Début()
        {
            //GetStudentsIn6thAnd9th();
            //GetStudentsInMathsOrGeography();
            GetStudentsInMathsAndGeography();
        }

        static void GetStudentsIn6thAnd9th()
        {
            var groupsByCourseAndClass = 
                         from s in _students
                         join e in _enrollments
                         on s.Id equals e.StudentId
                         join c in _classCourses
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

            var classesToGet = new[] { "6th", "7th" };

            var requiredGroups = from @group in groupsByCourseAndClass
                                 where classesToGet.Contains(@group.Key.Class.ToLower())
                                 select @group;
            var result = new List<string>();
            foreach (var group in requiredGroups)
            {
                var union = result.Union(group.Select(g => g.Student)).ToList();
                var newElements = union.Except(result);
                result.AddRange(newElements);
            }

            Console.WriteLine("Students in 6th and 7th class:");
            result.ForEach(r => Console.WriteLine(r));
        }

        static void GetStudentsInMathsOrGeography()
        {
            var groupsByCourseAndClass =
                         from s in _students
                         join e in _enrollments
                         on s.Id equals e.StudentId
                         join c in _classCourses
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
            var coursesToGet = new[] { /*"mathematics", "urdu"*/ "geography", "mathematics" };

            var requiredGroups = from @group in groupsByCourseAndClass
                                 where coursesToGet.Contains(@group.Key.Course.ToLower())
                                 select @group;
            var result = new List<string>();
            foreach (var group in requiredGroups)
            {
                var union = result.Union(group.Select(g => g.Student)).ToList();
                var newElements = union.Except(result);
                result.AddRange(newElements);
            }

            Console.WriteLine("Students in maths or geography courses:");
            result.ForEach(r => Console.WriteLine(r));
        }

        static void GetStudentsInMathsAndGeography()
        {
            var groupsByCourseAndClass =
                         from s in _students
                         join e in _enrollments
                         on s.Id equals e.StudentId
                         join c in _classCourses
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

            var mathGroups = from @group in groupsByCourseAndClass
                             where @group.Key.Course.ToLower().Equals("mathematics")
                             select @group;
            var geoGroups = from @group in groupsByCourseAndClass
                             where @group.Key.Course.ToLower().Equals("geography")
                             select @group;

            var mathGroupStudents = new List<string>();
            var geoGroupStudents = new List<string>();
            foreach (var group in mathGroups)
            {
                var union = mathGroupStudents.Union(group.Select(g => g.Student)).ToList();
                var newElements = union.Except(mathGroupStudents);
                mathGroupStudents.AddRange(newElements);
            }
            foreach (var group in geoGroups)
            {
                var union = geoGroupStudents.Union(group.Select(g => g.Student)).ToList();
                var newElements = union.Except(geoGroupStudents);
                geoGroupStudents.AddRange(newElements);
            }

            var result = mathGroupStudents.Intersect(geoGroupStudents).ToList();

            Console.WriteLine("Students in maths or geography courses:");
            result.ForEach(r => Console.WriteLine(r));
        }
    }
}
