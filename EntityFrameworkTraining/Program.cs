using EntityFrameworkTraining.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkTraining
{
    class Program
    {
        static void Main(string[] args)
        {
            UpdateData();
            Console.ReadLine();
        }
        static void AddData()
        {
            DataContext context = new DataContext();
            context.Database.Log = (log) => Console.WriteLine(log);
            Customer c1 = new Customer() { Name = "AAA" };
            context.Customer.Add(c1);
            Customer c2 = new Customer() { Name = "BBB" };
            context.Customer.Add(c2);
            Customer c3 = new Customer() { Name = "CCC" };
            context.Customer.Add(c3);
            context.SaveChanges();
        }
        static void UpdateData()
        {
            DataContext context = new DataContext();
            context.Database.Log = (log) => Console.WriteLine(log);

            var item = context.Customer.FirstOrDefault(a => a.ID == 3);
            item.Name = "ZZZ";
            context.SaveChanges();
        }
      
    }
}
