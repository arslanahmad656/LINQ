using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LsDAL.Models;

namespace LinqPractice
{
    class DiverExemples
    {
        public static void Début()
        {
            //GetDirectoryItemsQuery();
            //GetDirectoryItemsFluent();
            //EnrollmentCount();
            //SelectVsSelectMany();
            //QuerySyntaxVsFluent();
        }

        

        static void EnrollmentCount()
        {
            /**
             *  Gets all the students including their enrollments only if a student has at least 5 three enrollments.
             */

            var context = new ModelDataContext();
            var students = from std in context.AspNetStudents
                              where std.AspNetStudent_Enrollments.Count > 5
                              select new
                              {
                                  StudentId = std.Id,
                                  StudentName = std.Name,
                                  Enrollments = from enroll in std.AspNetStudent_Enrollments
                                                select new
                                                {
                                                    CourseName = enroll.AspNetCourse.Name
                                                }
                              };
            Console.WriteLine("Students with more than 3 enrollments:");
            int outerCount = 0;
            foreach (var student in students)
            {
                Console.WriteLine($" {++outerCount}- {student.StudentName} ({student.StudentId})");
                int innerCount = 0;
                foreach (var enrollment in student.Enrollments)
                {
                    Console.WriteLine($"     {outerCount}.{++innerCount}- {enrollment.CourseName}");
                }
            }
        }

        static void GetDirectoryItemsFluent()
        {
            var directoryInfos = new System.IO.DirectoryInfo(@"C:\Users\administrator.PUCIT\Documents\Dev").GetDirectories();

            var heirarchy = directoryInfos
                            .Where(di => (di.Attributes & System.IO.FileAttributes.Hidden & System.IO.FileAttributes.System) == 0)
                            .Select(di => new
                            {
                                DirectoryName = di.Name,
                                Files = di.GetFiles()
                                        .Where(f => (f.Attributes & System.IO.FileAttributes.Hidden & System.IO.FileAttributes.System) == 0)
                                        .Select(f => new
                                        {
                                            FileName = f.Name,
                                            Size = f.Length
                                        })
                            });
            var heirarchyList = heirarchy.ToList();
            Console.WriteLine($"Displaying the files in {heirarchyList.Count} directories:\n");
            foreach (var dir in heirarchyList)
            {
                Console.WriteLine($" - {dir.DirectoryName}");
                foreach (var file in dir.Files)
                {
                    Console.WriteLine($"     - {file.FileName} ({file.Size} bytes)");
                }
            }
        }

        static void GetDirectoryItemsQuery()
        {
            var directoryInfos = new System.IO.DirectoryInfo(@"C:\Users\administrator.PUCIT\Documents\Dev").GetDirectories();

            var heirarchy = from dir in directoryInfos
                            where (dir.Attributes & System.IO.FileAttributes.Hidden & System.IO.FileAttributes.System) == 0
                            select new
                            {
                                DirectoryName = dir.Name,
                                Files = from file in dir.GetFiles()
                                            where (file.Attributes & System.IO.FileAttributes.System & System.IO.FileAttributes.Hidden) == 0
                                            select new
                                            {
                                                FileName = file.Name,
                                                Size = file.Length
                                            }
                            };
            var heirarchyList = heirarchy.ToList();
            Console.WriteLine($"Displaying the files in {heirarchyList.Count} directories:\n");
            foreach (var dir in heirarchyList)
            {
                Console.WriteLine($" - {dir.DirectoryName}");
                foreach (var file in dir.Files)
                {
                    Console.WriteLine($"     - {file.FileName} ({file.Size} bytes)");
                }
            }
        }
    }
}
