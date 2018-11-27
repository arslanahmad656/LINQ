using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqPractice.EFModel;
//using LinqPractice.L2SModel;
using Ef = LinqPractice.EFModel;
//using Ls = LinqPractice.L2SModel;

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
                var context = new EfEntities();
                var students = context.Set<Ef.AspNetStudent>();
                var students_ = context.AspNetStudents;
                Console.WriteLine($"References equal: {students == students_}");
                int count = 0;
                int total = students.Count();
                foreach (var student in students)
                {
                    if (count < 5) 
                    {
                        break;
                    }
                    Console.WriteLine($"{student.Name}, {student.AspNetUser?.Email ?? student.AspNetUser.UserName}");
                }
                if (count < total)
                {
                    Console.WriteLine($"and {total - count} others...");
                }
            }
        }
    }
}
