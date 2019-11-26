using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkTraining.Repository.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        DataContext DataContext { get; }

        void Save();

        Task SaveAsync();
    }
}
