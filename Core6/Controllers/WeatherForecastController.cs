using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;

namespace Core6.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly MyContext _context;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            MyContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("AddTestData")]
        public string AddTestData()
        {
            _context.Product.Add(new Product
            {
                Name = "T1",
                Price = 100,
                Code = Guid.NewGuid()
            });
            _context.Product.Add(new Product
            {
                Name = "T2",
                Price = 200,
                Code = Guid.NewGuid()
            });
            _context.SaveChanges();
            return "OK";
        }
        [HttpGet("List")]
        public IActionResult List()
        {
            return Ok(_context.Product);
        }
        [HttpGet("Update")]
        public string Update(int id)
        {
            var item = _context.Product.Where(x => x.Id == id).First();
            //item.Name = $"MT{id}";
            item.Price = 0;
            item.Code = null;
            _context.Product.Update(item);
            _context.SaveChanges();
            return "OK";
        }

        [HttpGet("AttachUpdate")]
        public string AttachUpdate(int id)
        {
            var item = new Product { Id = id };
            _context.Product.Attach(item);
            item.Name = $"MT{id}";
            item.Price = 0;
            item.Code = null;
            _context.SaveChanges();
            return "OK";
        }
        [HttpGet("AttachUpdate2")]
        public string AttachUpdate2(int id)
        {
            var item = new Product { Id = id, Price = int.MinValue, Code = Guid.Empty };
            _context.Product.Attach(item);
            item.Name = $"AUT{id}";
            item.Price = 0;
            item.Code = null;
            _context.SaveChanges();
            return "OK";
        }

        [HttpGet("Delete")]
        public string Delete(int id)
        {
            var item = _context.Product.Where(x => x.Id == id).First();
            _context.Product.Remove(item);
            _context.SaveChanges();
            return "OK";
        }

        [HttpGet("AttachDelete")]
        public string AttachDelete(int id)
        {
            var item = new Product { Id = id };
            var entityEntry = _context.Product.Attach(item);
            entityEntry.State = EntityState.Deleted;
            _context.SaveChanges();
            return "OK";
        }
    }
}