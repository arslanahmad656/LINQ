using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EfModel = EfDAL.Models;
using EfEntities = EfDAL.Models.Entities;

namespace LinqPractice
{

    public static class BaseDeDonnéesDémonstrateur
    {
        public static void Début()
        {
            GettingQueryableObjects();
        }

        static void GettingQueryableObjects()
        {
            DemoWithEf();

            void DemoWithEf()
            {
                var efContext = new EfEntities();
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
