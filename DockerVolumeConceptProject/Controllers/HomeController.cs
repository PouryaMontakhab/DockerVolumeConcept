using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerVolumeConceptProject.Controllers
{
    //source 
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env,
            IConfiguration configuration)
        {
            _logger = logger;
            _env = env;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Save(string title, string body)
        {
            //if you want to create automatically Storage folder
            if (!Directory.Exists($"{_env.ContentRootPath}\\Storage\\"))
                Directory.CreateDirectory($"{_env.ContentRootPath}\\Storage\\");
            //if you want to create automatically Temp folder
            if (!Directory.Exists($"{_env.ContentRootPath}\\Temp\\"))
                Directory.CreateDirectory($"{_env.ContentRootPath}\\Temp\\");

            //temporary data
            var tempPath = $"{_env.ContentRootPath}\\Temp\\{title}.txt";
            using (FileStream fs = System.IO.File.Create(tempPath))
            {
                byte[] content = new UTF8Encoding(true).GetBytes($"{body}");

                fs.Write(content, 0, content.Length);
            }

            //persistant data
            var storagePath = $"{_env.ContentRootPath}\\Storage\\{title}.txt";
            if (IsExistFile(storagePath))
            {
                System.IO.File.Delete(tempPath);
                return Content("This file already exist");
            }

            System.IO.File.Move(tempPath, storagePath);
            return RedirectToAction("Index");
        }
        public bool IsExistFile(string path)
            => System.IO.File.Exists(path);

    }
}
