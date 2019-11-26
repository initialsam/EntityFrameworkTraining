using EntityFrameworkTraining.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkTraining
{
    public class DataContext : DbContext
    {
        public DataContext()
            : base("name=DataContext")
        {
        }
        public virtual IDbSet<Customer> Customer { get; set; }
    }
}
