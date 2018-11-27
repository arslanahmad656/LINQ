using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqPractice.DTO
{
    class ManualDataContext : System.Data.Linq.DataContext
    {
        public ManualDataContext()
            : base(/*Properties.Settings.Default.DbConnection*/"Data Source=172.168.9.105;Initial Catalog=aspnet-SeaVersion_2_20170818051512;Persist Security Info=True;User ID=sa;Password=webdir123R")
        {
            
        }

        public System.Data.Linq.Table<AspNetStudent> AspNetStudents => this.GetTable<AspNetStudent>();
    }
}
