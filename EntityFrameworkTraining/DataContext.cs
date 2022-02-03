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
        public override int SaveChanges()
        {
            var historys = this.ChangeTracker.Entries()
                .Where(e => e.Entity is IModificationHistory 
                            && (e.State == EntityState.Added ||e.State == EntityState.Modified))
                .Select(e => e.Entity as IModificationHistory);


            foreach (var history in historys)
            {
                history.DateModified = DateTime.Now;
                if (history.DateCreated == DateTime.MinValue)
                {
                    history.DateCreated = DateTime.Now;
                }
            }
            int result = base.SaveChanges();
          
            return result;
        }
    }
}
