using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqPractice.DTO
{
    class Student
    {
        public EfDAL.Models.AspNetStudent StudentDto { get; set; }
        public EfDAL.Models.AspNetCours CourseDto { get; set; }
    }
}
