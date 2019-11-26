using EntityFrameworkTraining.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkTraining.Repository
{
    public class EFUnitOfWork : IUnitOfWork
    {
        public DataContext DataContext { get; set; }


        public EFUnitOfWork()
        {
            DataContext = new DataContext();
        }

        public void Save()
        {
            DataContext.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await DataContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            DataContext.Dispose();
        }
    }
}
