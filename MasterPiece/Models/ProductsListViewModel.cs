using System;
using System.Collections.Generic;
using MasterPiece.Models;

namespace MasterPiece.ViewModels
{
    public class ProductsListViewModel
    {
        // قائمة المنتجات المصفاة
        public List<Product> Products { get; set; } = new List<Product>();

        // قائمة الفئات للفلتر
        public List<Category> Categories { get; set; } = new List<Category>();

        // الفئة المحددة
        public int? SelectedCategoryId { get; set; }

        // السعر الأدنى المحدد
        public int? MinPrice { get; set; }

        // السعر الأعلى المحدد
        public int? MaxPrice { get; set; }

        // نوع الترتيب المحدد
        public string? SortOrder { get; set; }

        // نطاق الأسعار الحالي في المنتجات
        public PriceRangeViewModel PriceRange { get; set; } = new PriceRangeViewModel();
    }

    public class PriceRangeViewModel
    {
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
    }
}