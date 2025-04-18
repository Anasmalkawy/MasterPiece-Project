using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MasterPiece.Models;

namespace MasterPiece.ViewModels
{
    public class CheckoutViewModel
    {
        // معلومات العميل
        [Required(ErrorMessage = "الرجاء إدخال الاسم")]
        [Display(Name = "الاسم")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال رقم الهاتف")]
        [Display(Name = "رقم الهاتف")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "الرجاء إدخال أرقام فقط")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال العنوان")]
        [Display(Name = "العنوان")]
        public string Address { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال المدينة")]
        [Display(Name = "المدينة")]
        public string City { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال الدولة")]
        [Display(Name = "الدولة")]
        public string Country { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال الرمز البريدي")]
        [Display(Name = "الرمز البريدي")]
        public string PostalCode { get; set; }

        // بيانات الطلب
        public DateTime ShippingDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Status { get; set; }

        // طريقة الدفع
        public string PaymentMethod { get; set; }

        // عناصر السلة
        public List<Cart> CartItems { get; set; }

        // المجاميع
        [Display(Name = "المجموع الفرعي")]
        public decimal Subtotal { get; set; }

        [Display(Name = "الخصم")]
        public decimal Discount { get; set; }

        [Display(Name = "المجموع الكلي")]
        public decimal Total { get; set; }
    }
}