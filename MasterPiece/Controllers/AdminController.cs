using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MasterPiece.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace MasterPiece.Controllers
{
    public class AdminController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(MyDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // التحقق من صلاحيات المدير
        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("role");
            return role == "admin";
        }

        // الصفحة الرئيسية للوحة التحكم
        public IActionResult Index()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Home");
            }

            // إحصائيات للوحة القيادة
            ViewBag.TotalUsers = _context.Users.Count();
            ViewBag.TotalProducts = _context.Products.Count();
            ViewBag.TotalOrders = _context.Orders.Count();
            ViewBag.TotalRevenue = _context.Payments.Where(p => p.Status == "Paid").Sum(p => p.Amount ?? 0);

            // الطلبات الأخيرة
            var recentOrders = _context.Orders
                .Include(o => o.User)
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .ToList();

            // المستخدمين الجدد
            var newUsers = _context.Users
                .OrderByDescending(u => u.Id)
                .Take(5)
                .ToList();

            ViewBag.RecentOrders = recentOrders;
            ViewBag.NewUsers = newUsers;

            return View();
        }

        #region إدارة المنتجات

        // عرض قائمة المنتجات
        public IActionResult Products()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Home");
            }

            var products = _context.Products
                .Include(p => p.Category)
                .ToList();

            return View(products);
        }

        // إضافة منتج جديد - عرض النموذج
        public IActionResult AddProduct()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Home");
            }

            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        // إضافة منتج جديد - معالجة النموذج
        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product, IFormFile imageFile)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid)
            {
                // معالجة تحميل الصورة
                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(imageFile.FileName);
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "img", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    product.Image = fileName;
                }

                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Products");
            }

            ViewBag.Categories = _context.Categories.ToList();
            return View(product);
        }

        // تعديل منتج - عرض النموذج
        public IActionResult EditProduct(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Home");
            }

            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = _context.Categories.ToList();
            return View(product);
        }

        // تعديل منتج - معالجة النموذج
        [HttpPost]
        public async Task<IActionResult> EditProduct(Product product, IFormFile imageFile)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid)
            {
                // معالجة تحميل الصورة الجديدة
                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(imageFile.FileName);
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "img", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    product.Image = fileName;
                }
                else
                {
                    // الاحتفاظ بالصورة القديمة إذا لم يتم تحميل صورة جديدة
                    var existingProduct = _context.Products.AsNoTracking().FirstOrDefault(p => p.Id == product.Id);
                    if (existingProduct != null)
                    {
                        product.Image = existingProduct.Image;
                    }
                }

                _context.Update(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Products");
            }

            ViewBag.Categories = _context.Categories.ToList();
            return View(product);
        }

        // حذف منتج
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Home");
            }

            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            // التحقق من وجود ارتباطات بالمنتج وحذفها
            var cartItems = _context.Carts.Where(c => c.ProductId == id).ToList();
            var wishlistItems = _context.Wishlists.Where(w => w.ProductId == id).ToList();
            var orderDetails = _context.OrderDetails.Where(od => od.ProductId == id).ToList();

            _context.Carts.RemoveRange(cartItems);
            _context.Wishlists.RemoveRange(wishlistItems);
            _context.OrderDetails.RemoveRange(orderDetails);
            _context.Products.Remove(product);

            await _context.SaveChangesAsync();
            return RedirectToAction("Products");
        }

        #endregion

        #region إدارة الفئات

        // عرض قائمة الفئات
        public IActionResult Categories()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Home");
            }

            var categories = _context.Categories.ToList();
            return View(categories);
        }

        // إضافة فئة جديدة - عرض النموذج
        public IActionResult AddCategory()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Home");
            }

            return View();
        }

        // إضافة فئة جديدة - معالجة النموذج
        [HttpPost]
        public async Task<IActionResult> AddCategory(Category category)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction("Categories");
            }

            return View(category);
        }

        // تعديل فئة - عرض النموذج
        public IActionResult EditCategory(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Home");
            }

            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // تعديل فئة - معالجة النموذج
        [HttpPost]
        public async Task<IActionResult> EditCategory(Category category)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid)
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
                return RedirectToAction("Categories");
            }

            return View(category);
        }

        // حذف فئة
        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Home");
            }

            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            // التحقق من وجود منتجات مرتبطة بالفئة
            var products = _context.Products.Where(p => p.CategoryId == id).ToList();
            if (products.Any())
            {
                TempData["Error"] = "لا يمكن حذف هذه الفئة لأنها تحتوي على منتجات. قم بحذف المنتجات أو نقلها لفئة أخرى أولاً.";
                return RedirectToAction("Categories");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Categories");
        }

        #endregion

        #region إدارة الطلبات

        // عرض قائمة الطلبات
        public IActionResult Orders()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Home");
            }

            var orders = _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .Include(o => o.Payments)
                .Include(o => o.Shippings)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }

        // عرض تفاصيل الطلب
        public IActionResult OrderDetails(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Home");
            }

            var order = _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Include(o => o.Payments)
                .Include(o => o.Shippings)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // تحديث حالة الطلب
        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(int id, string status)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Home");
            }

            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            order.Status = status;
            await _context.SaveChangesAsync();

            // تحديث حالة الشحن أيضًا
            var shipping = _context.Shippings.FirstOrDefault(s => s.OrderId == id);
            if (shipping != null)
            {
                shipping.Status = status;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("OrderDetails", new { id });
        }

        #endregion

        #region إدارة المستخدمين

        // عرض قائمة المستخدمين
        public IActionResult Users()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Home");
            }

            var users = _context.Users.ToList();
            return View(users);
        }

        // تعديل مستخدم - عرض النموذج
        public IActionResult EditUser(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Home");
            }

            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // تعديل مستخدم - معالجة النموذج
        [HttpPost]
        public async Task<IActionResult> EditUser(User user)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid)
            {
                // التحقق من عدم وجود تضارب في البريد الإلكتروني
                var existingUser = _context.Users
                    .AsNoTracking()
                    .FirstOrDefault(u => u.Email == user.Email && u.Id != user.Id);

                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "هذا البريد الإلكتروني مستخدم بالفعل.");
                    return View(user);
                }

                // التحقق من عدم وجود تضارب في رقم الهاتف
                existingUser = _context.Users
                    .AsNoTracking()
                    .FirstOrDefault(u => u.Phone == user.Phone && u.Id != user.Id);

                if (existingUser != null)
                {
                    ModelState.AddModelError("Phone", "رقم الهاتف هذا مستخدم بالفعل.");
                    return View(user);
                }

                // الاحتفاظ بكلمة المرور القديمة إذا لم يتم تحديثها
                if (string.IsNullOrEmpty(user.Password))
                {
                    var oldUser = _context.Users.AsNoTracking().FirstOrDefault(u => u.Id == user.Id);
                    if (oldUser != null)
                    {
                        user.Password = oldUser.Password;
                    }
                }
                else
                {
                    // يمكن إضافة تشفير لكلمة المرور هنا
                }

                _context.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Users");
            }

            return View(user);
        }

        // حذف مستخدم
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Home");
            }

            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            // التحقق من عدم وجود ارتباطات
            var orders = _context.Orders.Where(o => o.UserId == id).ToList();
            var cartItems = _context.Carts.Where(c => c.UserId == id).ToList();
            var wishlistItems = _context.Wishlists.Where(w => w.UserId == id).ToList();

            if (orders.Any())
            {
                TempData["Error"] = "لا يمكن حذف هذا المستخدم لأنه لديه طلبات سابقة.";
                return RedirectToAction("Users");
            }

            // حذف العناصر المرتبطة
            _context.Carts.RemoveRange(cartItems);
            _context.Wishlists.RemoveRange(wishlistItems);
            _context.Users.Remove(user);

            await _context.SaveChangesAsync();
            return RedirectToAction("Users");
        }

        #endregion

        #region التقارير

        // تقرير المبيعات
        public IActionResult SalesReport()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Home");
            }

            ViewBag.TotalSales = _context.Payments.Where(p => p.Status == "Paid").Sum(p => p.Amount ?? 0);
            ViewBag.TotalOrders = _context.Orders.Count();
            ViewBag.PendingOrders = _context.Orders.Count(o => o.Status == "Pending");
            ViewBag.ShippedOrders = _context.Orders.Count(o => o.Status == "Shipped");
            ViewBag.DeliveredOrders = _context.Orders.Count(o => o.Status == "Delivered");

            // المبيعات حسب الفئة
            var categorySales = _context.OrderDetails
                .Include(od => od.Product)
                .ThenInclude(p => p.Category)
                .Where(od => od.Product.Category != null)
                .GroupBy(od => od.Product.Category.Name)
                .Select(g => new
                {
                    CategoryName = g.Key,
                    TotalSales = g.Sum(od => (od.Price ?? 0) * (od.Quantity ?? 0))
                })
                .ToList();

            ViewBag.CategorySales = categorySales;

            // المنتجات الأكثر مبيعاً
            var topProducts = _context.OrderDetails
                .Include(od => od.Product)
                .GroupBy(od => new { od.ProductId, ProductName = od.Product.Name })
                .Select(g => new
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName,
                    TotalQuantity = g.Sum(od => od.Quantity ?? 0)
                })
                .OrderByDescending(x => x.TotalQuantity)
                .Take(5)
                .ToList();

            ViewBag.TopProducts = topProducts;

            return View();
        }

        #endregion
    }
}