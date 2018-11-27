using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LinqPractice.DTO
{
    [Table(Name = "[dbo].[AspNetStudents]")]
    public class AspNetStudent
    {
        [Column(IsPrimaryKey = true)]
        public int Id { get; set; }

        [Column]
        public string Name { get; set; }

        [Column]
        public string RollNo { get; set; }

        [Column]
        public string UserId { get; set; }

        [Column]
        public int BranchId { get; set; }

        [Column]
        public string Address { get; set; }

        [Column]
        public string File { get; set; }

        [Column]
        public int? ParentId { get; set; }
    }
}
