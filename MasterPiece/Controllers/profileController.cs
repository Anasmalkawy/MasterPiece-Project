using MasterPiece.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MasterPiece.Controllers
{
    public class profileController : Controller
    {
        private readonly MyDbContext _context;

        public profileController(MyDbContext context)
        {
            _context = context;
        }

        public IActionResult profile()
        {
            var userId = HttpContext.Session.GetInt32("id");
            if (userId == null) return RedirectToAction("login", "Home");

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null) return RedirectToAction("login", "Home");

            // Get recent orders for dashboard
            var recentOrders = _context.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .Take(3)
                .ToList();

            ViewBag.User = user;
            ViewBag.RecentOrders = recentOrders;

            return View();
        }

        public IActionResult Orders()
        {
            int? userId = HttpContext.Session.GetInt32("id");
            if (userId == null) return RedirectToAction("login", "Home");

            // Include all related data to show in the enhanced orders view
            var orders = _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Include(o => o.Shippings)
                .Include(o => o.Payments)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            // Pass db context to view for any additional queries
            ViewBag.DbContext = _context;

            return View(orders);
        }

        public IActionResult ChangePassword()
        {
            int? userId = HttpContext.Session.GetInt32("id");
            if (userId == null) return RedirectToAction("login", "Home");

            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(string Password, string NewPassword, string ConfirmPassword)
        {
            int? userId = HttpContext.Session.GetInt32("id");
            if (userId == null) return RedirectToAction("login", "Home");

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null || user.Password != Password)
            {
                ViewBag.Error = "كلمة المرور القديمة غير صحيحة.";
                return View();
            }

            if (NewPassword != ConfirmPassword)
            {
                ViewBag.Error = "كلمة المرور الجديدة وتأكيدها غير متطابقتين.";
                return View();
            }

            // Password validation
            if (NewPassword.Length < 6)
            {
                ViewBag.Error = "كلمة المرور يجب أن تكون 6 أحرف على الأقل.";
                return View();
            }

            user.Password = NewPassword;
            _context.SaveChanges();

            ViewBag.Message = "تم تغيير كلمة المرور بنجاح.";
            return View();
        }

        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("login", "Home");
        }

        [HttpGet]
        public IActionResult EditProfile()
        {
            int? userId = HttpContext.Session.GetInt32("id");
            if (userId == null) return RedirectToAction("login", "Home");

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost]
        public IActionResult EditProfile(User updatedUser)
        {
            int? userId = HttpContext.Session.GetInt32("id");
            if (userId == null) return RedirectToAction("login", "Home");

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null) return NotFound();

            // Email validation
            if (string.IsNullOrEmpty(updatedUser.Email) || !updatedUser.Email.Contains("@"))
            {
                TempData["Error"] = "الرجاء إدخال بريد إلكتروني صحيح.";
                return View(updatedUser);
            }

            // Check if email is already used by another user
            var existingUserWithEmail = _context.Users.FirstOrDefault(u =>
                u.Email == updatedUser.Email && u.Id != userId);

            if (existingUserWithEmail != null)
            {
                TempData["Error"] = "البريد الإلكتروني مستخدم بالفعل.";
                return View(updatedUser);
            }

            // Phone validation
            if (updatedUser.Phone == null || updatedUser.Phone.ToString().Length < 9)
            {
                TempData["Error"] = "الرجاء إدخال رقم هاتف صحيح.";
                return View(updatedUser);
            }

            // Check if phone is already used by another user
            var existingUserWithPhone = _context.Users.FirstOrDefault(u =>
                u.Phone == updatedUser.Phone && u.Id != userId);

            if (existingUserWithPhone != null)
            {
                TempData["Error"] = "رقم الهاتف مستخدم بالفعل.";
                return View(updatedUser);
            }

            // Update user data
            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            user.Phone = updatedUser.Phone;

            _context.SaveChanges();

            TempData["Success"] = "تم تحديث البيانات بنجاح!";
            return RedirectToAction("profile");
        }

        // Get order details for AJAX call
        [HttpGet]
        public IActionResult OrderDetails(int id)
        {
            int? userId = HttpContext.Session.GetInt32("id");
            if (userId == null) return Unauthorized();

            var order = _context.Orders
                .Where(o => o.Id == id && o.UserId == userId)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Include(o => o.Shippings)
                .Include(o => o.Payments)
                .FirstOrDefault();

            if (order == null) return NotFound();

            return PartialView("_OrderDetailsPartial", order);
        }

        // إلغاء الطلب
        [HttpPost]
        public IActionResult CancelOrder(int id)
        {
            int? userId = HttpContext.Session.GetInt32("id");
            if (userId == null) return Unauthorized();

            var order = _context.Orders
                .FirstOrDefault(o => o.Id == id && o.UserId == userId);

            if (order == null) return NotFound();

            // التحقق من أن الطلب في حالة "قيد الانتظار"
            string orderStatus = order.Status?.ToLower() ?? "";
            if (orderStatus != "pending" && orderStatus != "قيد الانتظار")
            {
                TempData["Error"] = "لا يمكن إلغاء الطلب لأنه ليس في حالة الانتظار.";
                return RedirectToAction("Orders");
            }

            // تغيير حالة الطلب إلى "ملغي"
            order.Status = "ملغي";
            _context.SaveChanges();

            TempData["Success"] = "تم إلغاء الطلب بنجاح.";
            return RedirectToAction("Orders");
        }
    }
}