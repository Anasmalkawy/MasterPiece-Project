using System.Data;
using MasterPiece.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MasterPiece.Controllers
{
    public class productController : Controller
    {
        private readonly MyDbContext _context;

        public productController(MyDbContext context)
        {
            _context = context;
        }

        public IActionResult singleproduct(int id)
        {
            var SingleProduct = _context.Products.Find(id);
            return View(SingleProduct);
        }
        public IActionResult addCart(int id)
        {
            var cart = _context.Products.Find(id);
            var item = new Cart
            {
                UserId = HttpContext.Session.GetInt32("id"),
                ProductId=id,
                DateAdded= DateOnly.FromDateTime(DateTime.Now),
                Quantity=1
            };
            _context.Carts.Add(item);
            _context.SaveChanges(); 
            return RedirectToAction("index", "Home");
        }

    }
}
