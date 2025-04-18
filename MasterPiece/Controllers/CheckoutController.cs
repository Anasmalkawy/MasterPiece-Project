using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MasterPiece.Models;
using MasterPiece.ViewModels;

namespace MasterPiece.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly MyDbContext _context;

        public CheckoutController(MyDbContext context)
        {
            _context = context;
        }

        // عرض سلة التسوق
        public IActionResult Cart()
        {
            var userId = HttpContext.Session.GetInt32("id");
            if (userId == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var cartItems = _context.Carts
                .Where(c => c.UserId == userId)
                .Include(c => c.Product)
                .ToList();

            // حساب المجموع
            decimal subtotal = 0;
            foreach (var item in cartItems)
            {
                if (item.Product != null && item.Product.Price != null && item.Quantity != null)
                {
                    subtotal += (decimal)item.Product.Price * (int)item.Quantity;
                }
            }

            ViewBag.Subtotal = subtotal;
            ViewBag.Total = subtotal;

            return View(cartItems);
        }

        // تحديث كمية المنتج في السلة
        [HttpPost]
        public IActionResult UpdateCartQuantity(int cartId, int quantity)
        {
            var userId = HttpContext.Session.GetInt32("id");
            if (userId == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var cartItem = _context.Carts.FirstOrDefault(c => c.Id == cartId && c.UserId == userId);
            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                _context.SaveChanges();
            }

            return RedirectToAction("Cart");
        }

        // حذف منتج من السلة
        [HttpPost]
        public IActionResult RemoveFromCart(int cartId)
        {
            var userId = HttpContext.Session.GetInt32("id");
            if (userId == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var cartItem = _context.Carts.FirstOrDefault(c => c.Id == cartId && c.UserId == userId);
            if (cartItem != null)
            {
                _context.Carts.Remove(cartItem);
                _context.SaveChanges();
            }

            return RedirectToAction("Cart");
        }

        // عرض صفحة الخروج - الخطوة الأولى (معلومات الشحن)
        public IActionResult Checkout()
        {
            var userId = HttpContext.Session.GetInt32("id");
            if (userId == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var cartItems = _context.Carts
                .Where(c => c.UserId == userId)
                .Include(c => c.Product)
                .ToList();

            if (cartItems.Count == 0)
            {
                return RedirectToAction("Cart");
            }

            // حساب المجموع
            decimal subtotal = 0;
            decimal discount = 10.00m; // يمكن جعل هذا ديناميكيًا

            foreach (var item in cartItems)
            {
                if (item.Product != null && item.Product.Price != null && item.Quantity != null)
                {
                    subtotal += (decimal)item.Product.Price * (int)item.Quantity;
                }
            }

            decimal total = subtotal - discount;

            var viewModel = new CheckoutViewModel
            {
                CartItems = cartItems,
                Subtotal = subtotal,
                Discount = discount,
                Total = total,
                Status = "Pending",
                PaymentMethod = "COD",
                ShippingDate = DateTime.Now.AddDays(2),
                DeliveryDate = DateTime.Now.AddDays(5)
            };

            return View(viewModel);
        }

        // معالجة معلومات الشحن - طريقة مبسطة
        [HttpPost]
        public IActionResult ProcessShipping(CheckoutViewModel model)
        {
            var userId = HttpContext.Session.GetInt32("id");
            if (userId == null)
            {
                return RedirectToAction("Login", "Home");
            }

            // تجاهل بعض التحقق من الصحة للتبسيط
            ModelState.Clear();

            // تجهيز عناصر السلة ومعلومات الأسعار
            var cartItems = _context.Carts
                .Where(c => c.UserId == userId)
                .Include(c => c.Product)
                .ToList();

            decimal subtotal = 0;
            foreach (var item in cartItems)
            {
                if (item.Product?.Price != null && item.Quantity != null)
                {
                    subtotal += (decimal)item.Product.Price * (int)item.Quantity;
                }
            }

            decimal discount = 10.00m;
            decimal total = subtotal - discount;

            // حفظ المعلومات في الجلسة
            HttpContext.Session.SetString("CustomerName", model.CustomerName ?? "");
            HttpContext.Session.SetString("PhoneNumber", model.PhoneNumber ?? "");
            HttpContext.Session.SetString("Address", model.Address ?? "");
            HttpContext.Session.SetString("City", model.City ?? "");
            HttpContext.Session.SetString("Country", model.Country ?? "");
            HttpContext.Session.SetString("PostalCode", model.PostalCode ?? "");
            HttpContext.Session.SetString("Total", total.ToString());
            HttpContext.Session.SetString("Status", "Pending");
            HttpContext.Session.SetString("PaymentMethod", "COD");

            // توجيه مباشر للخطوة التالية
            return RedirectToAction("CheckoutStep2");
        }

        // عرض صفحة الخروج - الخطوة الثانية (طريقة الدفع)
        public IActionResult CheckoutStep2()
        {
            var userId = HttpContext.Session.GetInt32("id");
            if (userId == null)
            {
                return RedirectToAction("Login", "Home");
            }

            // التحقق من وجود معلومات الشحن في الجلسة
            var customerName = HttpContext.Session.GetString("CustomerName");
            if (string.IsNullOrEmpty(customerName))
            {
                return RedirectToAction("Checkout");
            }

            var cartItems = _context.Carts
                .Where(c => c.UserId == userId)
                .Include(c => c.Product)
                .ToList();

            // حساب المجموع
            decimal subtotal = 0;
            decimal discount = 10.00m;

            foreach (var item in cartItems)
            {
                if (item.Product != null && item.Product.Price != null && item.Quantity != null)
                {
                    subtotal += (decimal)item.Product.Price * (int)item.Quantity;
                }
            }

            decimal total = subtotal - discount;

            var viewModel = new CheckoutViewModel
            {
                CartItems = cartItems,
                Subtotal = subtotal,
                Discount = discount,
                Total = total,
                CustomerName = customerName,
                PhoneNumber = HttpContext.Session.GetString("PhoneNumber"),
                Address = HttpContext.Session.GetString("Address"),
                City = HttpContext.Session.GetString("City"),
                Country = HttpContext.Session.GetString("Country"),
                PostalCode = HttpContext.Session.GetString("PostalCode"),
                Status = "Pending",
                PaymentMethod = "COD"
            };

            return View(viewModel);
        }

        // تأكيد الطلب
        [HttpPost]
        public IActionResult ConfirmOrder(string paymentMethod)
        {
            var userId = HttpContext.Session.GetInt32("id");
            if (userId == null)
            {
                return RedirectToAction("Login", "Home");
            }

            // التحقق من وجود معلومات الشحن
            var customerName = HttpContext.Session.GetString("CustomerName");
            if (string.IsNullOrEmpty(customerName))
            {
                return RedirectToAction("Checkout");
            }

            var cartItems = _context.Carts
                .Where(c => c.UserId == userId)
                .Include(c => c.Product)
                .ToList();

            if (cartItems.Count == 0)
            {
                return RedirectToAction("Cart");
            }

            // حساب المجموع
            decimal total = 0;
            foreach (var item in cartItems)
            {
                if (item.Product != null && item.Product.Price != null && item.Quantity != null)
                {
                    total += (decimal)item.Product.Price * (int)item.Quantity;
                }
            }

            // 1. إنشاء سجل الطلب الرئيسي
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateOnly.FromDateTime(DateTime.Now),
                Status = "Pending"
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            // 2. إنشاء سجلات تفاصيل الطلب
            foreach (var item in cartItems)
            {
                if (item.Product != null && item.Product.Price != null && item.Quantity != null)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Product.Price
                    };

                    _context.OrderDetails.Add(orderDetail);
                }
            }

            // 3. إنشاء سجل الشحن
            var shipping = new Shipping
            { 
                OrderId = order.Id,
                Address = HttpContext.Session.GetString("Address"),
                City = HttpContext.Session.GetString("City"),
                Country = HttpContext.Session.GetString("Country"),
                PostalCode = HttpContext.Session.GetString("PostalCode"),
                ShippingDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
                DeliveryDate = DateOnly.FromDateTime(DateTime.Now.AddDays(5)),
                Status = "Pending"
            };

            _context.Shippings.Add(shipping);

            // 4. إنشاء سجل الدفع
            var payment = new Payment
            {
                OrderId = order.Id,
                PaymentMethod = paymentMethod,
                Amount = (int)total,
                Status = paymentMethod == "COD" ? "Pending" : "Paid"
            };

            _context.Payments.Add(payment);

            // 5. حذف المنتجات من سلة التسوق
            _context.Carts.RemoveRange(cartItems);

            _context.SaveChanges();

            // حفظ معلومات الطلب للعرض في صفحة النجاح
            HttpContext.Session.SetInt32("OrderId", order.Id);
            HttpContext.Session.SetString("OrderTotal", total.ToString());

            return RedirectToAction("OrderSuccess");
        }

        // عرض صفحة نجاح الطلب
        public IActionResult OrderSuccess()
        {
            var orderId = HttpContext.Session.GetInt32("OrderId");
            if (orderId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var total = HttpContext.Session.GetString("OrderTotal");

            ViewBag.OrderId = orderId;
            ViewBag.Total = total;

            return View();
        }

        // إضافة منتج إلى السلة (واجهة برمجة التطبيقات)
        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            var userId = HttpContext.Session.GetInt32("id");
            if (userId == null)
            {
                return RedirectToAction("Login", "Home");
            }

            // التحقق من وجود المنتج
            var product = _context.Products.Find(productId);
            if (product == null)
            {
                return NotFound();
            }

            // التحقق من وجود المنتج في السلة بالفعل
            var existingCartItem = _context.Carts
                .FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);

            if (existingCartItem != null)
            {
                // تحديث الكمية إذا كان المنتج موجوداً بالفعل
                existingCartItem.Quantity += quantity;
            }
            else
            {
                // إضافة المنتج إلى السلة إذا لم يكن موجوداً
                var cartItem = new Cart
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity,
                    DateAdded = DateOnly.FromDateTime(DateTime.Now)
                };
                _context.Carts.Add(cartItem);
            }

            _context.SaveChanges();

            // إعادة عدد العناصر في السلة لتحديث العداد في واجهة المستخدم
            var cartCount = _context.Carts.Where(c => c.UserId == userId).Sum(c => c.Quantity);

            return Json(new { success = true, cartCount });
        }
    }
}