using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Core6.Controllers
{
    [AllowAnonymous]
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
                Id = ProductId.New(),
                Name = "T1",
                Price = 100,
                Code = Guid.NewGuid()
            });
            _context.Product.Add(new Product
            {
                Id = ProductId.New(),
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
        [HttpPost("Update")]
        public string Update([FromForm] ProductId Id)
        {
            //三個都會更新
            var item = _context.Product.Where(x => x.Id == Id).First();
            //item.Name = $"MT{id}";
            item.Price = 0;
            item.Code = null;
            _context.Product.Update(item);
            _context.SaveChanges();
            return "OK";
        }
        [HttpGet("Update2")]
        public string Update2([FromQuery] ProductId id)
        {
            //這樣會更新 全部欄位
            //其他沒有設定到的都會更新為初始值
            //所以不能是null 的欄位 會塞null進去 然後會出例外
            var item = new Product { Id = id };
            //item.Name = $"MT{id}";
            //item.Price = 0;
            //item.Code = null;
            _context.Product.Update(item);
            _context.SaveChanges();
            return "OK";
        }
        [HttpPut("AttachUpdate")]
        public string AttachUpdate([FromBody] ProductDto dto)
        {
            //Price 跟 Code 不會更新
            var item = new Product { Id = dto.Id };
            _context.Product.Attach(item);
            //item.Name = $"MT{id}";
            //item.Price = 0;
            //item.Code = null;
            _context.SaveChanges();
            return "OK";
        }
        [HttpGet("AttachUpdate2")]
        public string AttachUpdate2(ProductId id)
        {
            //有不同 所以三個都會更新
            var item = new Product { Id = id, Price = int.MinValue, Code = Guid.Empty };
             _context.Product.Attach(item);
            item.Name = $"AUT{id}";
            item.Price = 0;
            item.Code = null;
          
            _context.SaveChanges();
            return "OK";
        }
        [HttpGet("AttachUpdate3")]
        public string AttachUpdate3(ProductId id)
        {
            //三個都會更新
            var item = new Product { Id = id };
            var entityEntry = _context.Product.Attach(item);
            item.Name = $"MT{id}";
            item.Price = 0;
            item.Code = null;
            entityEntry.Property(x => x.Name).IsModified = true;
            entityEntry.Property(x => x.Price).IsModified = true;
            entityEntry.Property(x => x.Code).IsModified = true;
            _context.SaveChanges();
            return "OK";
        }

        [HttpPost("Delete")]
        public string Delete([FromForm]Product id)
        {
            //可以刪除
            var item = _context.Product.Where(x => x.Id == id.Id).First();
            _context.Product.Remove(item);
            _context.SaveChanges();
            return "OK";
        }
        [HttpGet("Delete2")]
        public string Delete2(ProductId id)
        {
            //可以刪除
            var item = new Product { Id = id };
            _context.Product.Remove(item);
            _context.SaveChanges();
            return "OK";
        }

        [HttpGet("AttachDelete")]
        public string AttachDelete(ProductId id)
        {
            //可以刪除
            var item = new Product { Id = id };
            var entityEntry = _context.Product.Attach(item);
            entityEntry.State = EntityState.Deleted;
            _context.SaveChanges();
            return "OK";
        }
    }
}