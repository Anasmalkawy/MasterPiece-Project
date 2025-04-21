using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MasterPiece.Models;
using MasterPiece.ViewModels;

namespace MasterPiece.Controllers
{
    public class ProductsListController : Controller
    {
        private readonly MyDbContext _context;

        public ProductsListController(MyDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? categoryId = null, int? minPrice = null, int? maxPrice = null, string? sortOrder = null)
        {
            // 1. جلب جميع المنتجات
            var productsQuery = _context.Products
                .Include(p => p.Category)
                .AsQueryable();

            // 2. التصفية حسب الفئة إذا تم تحديدها
            if (categoryId.HasValue && categoryId > 0)
            {
                productsQuery = productsQuery.Where(p => p.CategoryId == categoryId);
            }

            // 3. التصفية حسب السعر الأدنى إذا تم تحديده
            if (minPrice.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Price >= minPrice);
            }

            // 4. التصفية حسب السعر الأعلى إذا تم تحديده
            if (maxPrice.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Price <= maxPrice);
            }

            // 5. الترتيب حسب الاختيار
            switch (sortOrder)
            {
                case "price_asc":
                    productsQuery = productsQuery.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    productsQuery = productsQuery.OrderByDescending(p => p.Price);
                    break;
                case "name_asc":
                    productsQuery = productsQuery.OrderBy(p => p.Name);
                    break;
                case "name_desc":
                    productsQuery = productsQuery.OrderByDescending(p => p.Name);
                    break;
                default:
                    productsQuery = productsQuery.OrderBy(p => p.Id);
                    break;
            }

            // 6. تنفيذ الاستعلام واسترجاع المنتجات
            var products = productsQuery.ToList();

            // 7. جلب جميع الفئات للفلتر
            var categories = _context.Categories.ToList();

            // 8. تحديد قيم السعر الأدنى والأعلى في المنتجات
            int minProductPrice = 0;
            int maxProductPrice = 1000;

            var minDbPrice = _context.Products.Min(p => p.Price);
            var maxDbPrice = _context.Products.Max(p => p.Price);

            if (minDbPrice.HasValue) minProductPrice = minDbPrice.Value;
            if (maxDbPrice.HasValue) maxProductPrice = maxDbPrice.Value;

            // 9. إنشاء نموذج العرض
            var viewModel = new ProductsListViewModel
            {
                Products = products,
                Categories = categories,
                SelectedCategoryId = categoryId,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                SortOrder = sortOrder,
                PriceRange = new PriceRangeViewModel
                {
                    MinPrice = minPrice ?? minProductPrice,
                    MaxPrice = maxPrice ?? maxProductPrice
                }
            };

            // 10. إرسال البيانات إلى العرض
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Filter(int? categoryId, int? minPrice, int? maxPrice, string? sortOrder)
        {
            return RedirectToAction("Index", new
            {
                categoryId,
                minPrice,
                maxPrice,
                sortOrder
            });
        }

        // إضافة إلى السلة
        [HttpPost]
        public IActionResult AddToCart(int productId)
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
                existingCartItem.Quantity = (existingCartItem.Quantity ?? 0) + 1;
            }
            else
            {
                // إضافة المنتج إلى السلة إذا لم يكن موجوداً
                var cartItem = new Cart
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = 1,
                    DateAdded = DateOnly.FromDateTime(DateTime.Now)
                };
                _context.Carts.Add(cartItem);
            }

            _context.SaveChanges();

            // إعادة توجيه المستخدم إلى نفس صفحة المنتجات مع الاحتفاظ بالفلاتر
            return RedirectToAction("Index", new
            {
                categoryId = Request.Query["categoryId"],
                minPrice = Request.Query["minPrice"],
                maxPrice = Request.Query["maxPrice"],
                sortOrder = Request.Query["sortOrder"]
            });
        }
    }
}