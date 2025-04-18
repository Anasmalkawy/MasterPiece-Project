using System.Runtime.Intrinsics.X86;
using MasterPiece.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MasterPiece.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyDbContext _context;

        public HomeController(MyDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var product = _context.Products.ToList();
            return View(product);
        }

        public IActionResult login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult login(User user)
        {
            var data = _context.Users.FirstOrDefault(x => x.Email == user.Email && x.Password == user.Password);
            if (data.Role == "admin")
            {
                HttpContext.Session.SetInt32("id", data.Id);
                HttpContext.Session.SetString("role", data.Role);
                return RedirectToAction("index", "Admin");


            }
            if (data != null)
            {
                HttpContext.Session.SetInt32("id", data.Id);
                HttpContext.Session.SetString("name", data.Name);
                HttpContext.Session.SetString("mail", data.Email);
                return RedirectToAction("index", "Home");
            }
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return RedirectToAction("login", "Home");
        }


        public IActionResult about()
        {
            return View();
        }

        public IActionResult contact()
        {
            return View();
        }

        



    }
}
