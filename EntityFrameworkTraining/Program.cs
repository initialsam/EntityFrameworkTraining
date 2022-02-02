using EntityFrameworkTraining.Entities;
using EntityFrameworkTraining.Repository;
using EntityFrameworkTraining.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkTraining
{
    class Program
    {
        static void Main(string[] args)
        {
            test();
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
        static void ReloadData()
        {
            DataContext context1 = new DataContext();
            DataContext context2 = new DataContext();
            context1.Database.Log = (log) => Console.WriteLine($"context1 : {log}");
            context2.Database.Log = (log) => Console.WriteLine($"context2 : {log}");

            var item1 = context1.Customer.FirstOrDefault(a => a.ID == 1);
            item1.Name = "edit by context1";
            context1.SaveChanges();

            var item2 = context2.Customer.FirstOrDefault(a => a.ID == 1);
            item2.Name = "edit by context2";
            context2.SaveChanges();

            //還是edit by context1 因為EF會快取資料
            var item3 = context1.Customer.FirstOrDefault(a => a.ID == 1);
            Console.WriteLine($"item3 ID:{item3.ID}, NAME:{item3.Name}");

            Console.WriteLine($"item1 ID:{item1.ID}, NAME:{item1.Name}");
            //用Reload可以重新讀資料
            context1.Entry(item1).Reload();
            Console.WriteLine($"Reload item1 ID:{item1.ID}, NAME:{item1.Name}");
        }

        static void test()
        {
            IUnitOfWork unitOfWork = new EFUnitOfWork();
            IRepository<Customer> customerRepo = new Repository<Customer>(unitOfWork);

            var entity = customerRepo.Create(new Customer()
            {
                Name = "SS"
            });
            customerRepo.Save();
            entity.Name = "TT"+ entity.ID;
            customerRepo.Update(entity);
            customerRepo.Save();

        }

        private static void QueryAndUpdateNinjaDisconnected()
        {
            Customer ninja;
            using (var context = new DataContext())
            {
                context.Database.Log = Console.WriteLine;
                ninja = context.Customer.FirstOrDefault();
            }

            ninja.Name = (ninja.Name+"UUUUUUUUUUUU");

            using (var context = new DataContext())
            {
                context.Database.Log = Console.WriteLine;
                context.Customer.Attach(ninja);
                context.Entry(ninja).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        private static void RetrieveDataWithFind()
        {
            var keyval = 4;
            using (var context = new DataContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = context.Customer.Find(keyval);
                Console.WriteLine("After Find#1:" + ninja.Name);

                var someNinja = context.Customer.Find(keyval);
                Console.WriteLine("After Find#2:" + someNinja.Name);
                ninja = null;
            }
        }

        private static void DeleteNinja()
        {
            Customer ninja;
            using (var context = new DataContext())
            {
                context.Database.Log = Console.WriteLine;
                ninja = context.Customer.FirstOrDefault();
                //context.Ninjas.Remove(ninja);
                //context.SaveChanges();
            }
            using (var context = new DataContext())
            {
                context.Database.Log = Console.WriteLine;
                //context.Ninjas.Attach(ninja);
                //context.Ninjas.Remove(ninja);
                context.Entry(ninja).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        private static void DeleteNinjaWithKeyValue()
        {
            var keyval = 1;
            using (var context = new DataContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = context.Customer.Find(keyval);
                context.Customer.Remove(ninja);
                context.SaveChanges();
            }
        }

    }
}
