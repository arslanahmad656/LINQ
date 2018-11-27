using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EfModel = EfDAL.Models;
using LsModel = LsDAL.Models;
using EfEntities = EfDAL.Models.Entities;
using LsEntities = LsDAL.Models.ModelDataContext;
using DtoStudent = LinqPractice.DTO.AspNetStudent;
using System.Data.Linq;
using System.Data.Entity;

namespace LinqPractice
{

    public static class BaseDeDonnéesDémonstrateur
    {
        public static void Début()
        {
            GettingQueryableObjects_TypedContexts();
            //GettingQueryableObjects_Manual();
        }

        static void GettingQueryableObjects_Manual()
        {
            Console.WriteLine("Using Untyped:");
            DemoWithLs_Untyped();
            Console.WriteLine("Using Typed:");
            DemoWithLs_Typed();

            void DemoWithLs_Untyped()
            {
                var context = new DataContext("Data Source=172.168.9.105;Initial Catalog=aspnet-SeaVersion_2_20170818051512;Persist Security Info=True;User ID=sa;Password=webdir123R");
                var students = context.GetTable<DtoStudent>();
                int count = 0;
                int total = students.Count();
                foreach (var student in students)
                {
                    if (count == 5)
                    {
                        break;
                    }
                    Console.WriteLine($"Type: {student.GetType().FullName}");
                    Console.WriteLine($"{student.Name}, {student.RollNo}, {student.Address}");
                    count++;
                }
                if (count < total)
                {
                    Console.WriteLine($"and {total - count} others...");
                }
            }

            void DemoWithLs_Typed()
            {
                var context = new DTO.ManualDataContext();
                var students = context.AspNetStudents;
                int count = 0;
                int total = students.Count();
                foreach (var student in students)
                {
                    if (count == 5)
                    {
                        break;
                    }
                    Console.WriteLine($"Type: {student.GetType().FullName}");
                    Console.WriteLine($"{student.Name}, {student.RollNo}, {student.Address}");
                    count++;
                }
                if (count < total)
                {
                    Console.WriteLine($"and {total - count} others...");
                }
            }
        }

        static void GettingQueryableObjects_TypedContexts()
        {
            Console.WriteLine("Using EF:");
            DemoWithEf();
            Console.WriteLine("Using L2S:");
            DemoWithLs();

            void DemoWithLs()
            {
                using (var lsContext = new LsEntities())
                {
                    var students = lsContext.GetTable<LsModel.AspNetStudent>();
                    var students_ = lsContext.AspNetStudents;
                    Console.WriteLine($"References equal: {students == students_}");
                    int count = 0;
                    int total = students.Count();
                    foreach (var student in students)
                    {
                        if (count == 5)
                        {
                            break;
                        }
                        Console.WriteLine($"Type: {student.GetType().FullName}");
                        Console.WriteLine($"{student.Name}, {student.AspNetUser?.Email ?? student.AspNetUser.UserName}");
                        count++;
                    }
                    if (count < total)
                    {
                        Console.WriteLine($"and {total - count} others...");
                    }
                }
            }

            void DemoWithEf()
            {
                using (var efContext = new EfEntities())
                {
                    var students = efContext.Set<EfModel.AspNetStudent>();
                    var students_ = efContext.AspNetStudents;
                    Console.WriteLine($"References equal: {students == students_}");
                    int count = 0;
                    int total = students.Count();
                    foreach (var student in students)
                    {
                        if (count == 5)
                        {
                            break;
                        }
                        Console.WriteLine($"Type: {student.GetType().FullName}");
                        Console.WriteLine($"{student.Name}, {student.AspNetUser?.Email ?? student.AspNetUser.UserName}");
                        count++;
                    }
                    if (count < total)
                    {
                        Console.WriteLine($"and {total - count} others...");
                    }
                }
            }
        }
    }
}
