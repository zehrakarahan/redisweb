using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RedisWeb.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RedisWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private RedisCacheOptions options;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public  IActionResult Deneme()
        {
            options = new RedisCacheOptions
            {
                Configuration = "127.0.0.1:6379",
                InstanceName = ""
            };
            List<Product> model = new List<Product>();
          
            for (int i = 0; i < 100; i++)
            {
                Product product = new Product();
                product.Id = i;
                product.ProductName = "denem";
                product.Aciklama = "deneme yapiliyor";
                product.Fiyat = "25";
                model.Add(product);
            }
            Set($"deneme",model,60) ;

            return View();
        }
        public void Set(string key, object valueObject, int expiration)
        {
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expiration)
            };

            using (var redisCache = new RedisCache(options))
            {
                var valueString = JsonConvert.SerializeObject(valueObject);
                redisCache.SetString(key, valueString, cacheOptions);
            }
        }
    }
}
