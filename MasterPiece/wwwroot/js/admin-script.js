//// ملف JavaScript للوحة تحكم المدير
//$(document).ready(function() {
//    // إظهار وإخفاء القوائم المنسدلة
//    $('.notification-btn').click(function(e) {
//        e.stopPropagation();
//        $('.notification-dropdown').toggleClass('show');
//        $('.user-dropdown').removeClass('show');
//    });
    
//    $('.user-menu-btn').click(function(e) {
//        e.stopPropagation();
//        $('.user-dropdown').toggleClass('show');
//        $('.notification-dropdown').removeClass('show');
//    });
    
//    // إغلاق القوائم المنسدلة عند النقر خارجها
//    $(document).click(function() {
//        $('.notification-dropdown, .user-dropdown').removeClass('show');
//    });
    
//    // إظهار وإخفاء القائمة في الشاشات الصغيرة
//    $('.sidebar-toggle').click(function() {
//        $('.admin-sidebar').toggleClass('show');
//    });
    
//    // حالة الطلب
//    $('.order-status-update form').submit(function() {
//        // إظهار مؤشر التحميل
//        $(this).find('button').append(' <i class="fas fa-spinner fa-spin"></i>');
//    });
    
//    // تأكيد الحذف
//    $('.delete-btn').click(function() {
//        return confirm('هل أنت متأكد من الحذف؟');
//    });
    
//    // تفعيل التبويبات
//    $('.tab-btn').click(function() {
//        const target = $(this).data('target');
//        $('.tab-btn').removeClass('active');
//        $(this).addClass('active');
//        $('.tab-content').removeClass('active');
//        $(target).addClass('active');
//    });
    
//    // معاينة الصورة قبل الرفع
//    $('input[type="file"]').change(function() {
//        const file = this.files[0];
//        if (file) {
//            const reader = new FileReader();
//            reader.onload = function(e) {
//                $('#preview-image').attr('src', e.target.result);
//                $('.image-preview').removeClass('d-none');
//            }
//            reader.readAsDataURL(file);
//        }
//    });
    
//    // إغلاق الإشعارات
//    $('.alert .close').click(function() {
//        $(this).parent().fadeOut();
//    });
    
//    // التحقق من صحة النموذج
//    $('form').submit(function(e) {
//        const requiredFields = $(this).find('[required]');
//        let isValid = true;
        
//        requiredFields.each(function() {
//            if (!$(this).val()) {
//                isValid = false;
//                $(this).addClass('is-invalid');
//            } else {
//                $(this).removeClass('is-invalid');
//            }
//        });
        
//        if (!isValid) {
//            e.preventDefault();
//            alert('الرجاء ملء جميع الحقول المطلوبة');
//        }
//    });
    
//    // إزالة فئة الخطأ عند الكتابة
//    $('input, textarea, select').keyup(function() {
//        if ($(this).val()) {
//            $(this).removeClass('is-invalid');
//        }
//    });
    
//    // تفعيل tooltips
//    $('[data-toggle="tooltip"]').tooltip();
    
//    // تغيير حالة تبديل
//    $('.toggle-switch').click(function() {
//        $(this).toggleClass('active');
        
//        const isActive = $(this).hasClass('active');
//        const statusField = $(this).data('field');
//        const itemId = $(this).data('id');
        
//        // يمكن إضافة AJAX هنا لتحديث الحالة في قاعدة البيانات
//        console.log(`تم تغيير حالة ${statusField} للعنصر رقم ${itemId} إلى ${isActive}`);
//    });
    
//    // إظهار وإخفاء كلمة المرور
//    $('.password-toggle').click(function() {
//        const passwordField = $(this).siblings('input');
//        const fieldType = passwordField.attr('type');
        
//        if (fieldType === 'password') {
//            passwordField.attr('type', 'text');
//            $(this).html('<i class="fas fa-eye-slash"></i>');
//        } else {
//            passwordField.attr('type', 'password');
//            $(this).html('<i class="fas fa-eye"></i>');
//        }
//    });
    
//    // تحديث الرسومات البيانية عند تغيير الفترة الزمنية
//    $('.time-period').change(function() {
//        const period = $(this).val();
//        // يمكن إضافة AJAX هنا لتحديث بيانات الرسومات البيانية
//        console.log(`تم تغيير الفترة الزمنية إلى ${period}`);
//    });
    
//    // تحديث الجدول عند تغيير عدد العناصر لكل صفحة
//    $('.items-per-page').change(function() {
//        const itemsCount = $(this).val();
//        // يمكن إضافة AJAX هنا لتحديث الجدول
//        console.log(`تم تغيير عدد العناصر إلى ${itemsCount}`);
//    });
//});

//$(document).ready(function () {
//    // تصفية حسب حالة الطلب
//    $(".filter-btn").click(function () {
//        $(".filter-btn").removeClass("active");
//        $(this).addClass("active");

//        var status = $(this).data("status");
//        if (status === "all") {
//            $(".orders-table tbody tr").show();
//        } else {
//            $(".orders-table tbody tr").hide();
//            $(".orders-table tbody tr[data-status='" + status + "']").show();
//        }
//    });

//    // بحث في الطلبات
//    $("#orderSearch").on("keyup", function () {
//        var value = $(this).val().toLowerCase();
//        $(".orders-table tbody tr").filter(function () {
//            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
//        });
//    });

//    // تصفية حسب التاريخ
//    $("#applyDateFilter").click(function () {
//        var startDate = $("#startDate").val();
//        var endDate = $("#endDate").val();

//        if (startDate && endDate) {
//            $(".orders-table tbody tr").hide();
//            $(".orders-table tbody tr").each(function () {
//                var orderDate = $(this).data("order-date");
//                if (orderDate >= startDate && orderDate <= endDate) {
//                    $(this).show();
//                }
//            });

//            // إعادة تطبيق تصفية الحالة إذا كانت نشطة
//            var activeStatus = $(".filter-btn.active").data("status");
//            if (activeStatus !== "all") {
//                $(".orders-table tbody tr").hide();
//                $(".orders-table tbody tr[data-status='" + activeStatus + "']").show();
//            }
//        } else {
//            alert("الرجاء تحديد تاريخ البداية والنهاية");
//        }
//    });

//    // إعادة ضبط تصفية التاريخ
//    $("#resetDateFilter").click(function () {
//        $("#startDate").val("");
//        $("#endDate").val("");

//        // إعادة تطبيق تصفية الحالة فقط
//        var activeStatus = $(".filter-btn.active").data("status");
//        if (activeStatus === "all") {
//            $(".orders-table tbody tr").show();
//        } else {
//            $(".orders-table tbody tr").hide();
//            $(".orders-table tbody tr[data-status='" + activeStatus + "']").show();
//        }
//    });

//    // عرض وإخفاء القائمة المنسدلة لتغيير الحالة
//    $(".dropdown-btn").click(function (e) {
//        e.stopPropagation();
//        // أغلق جميع القوائم المنسدلة الأخرى أولاً
//        $(".dropdown-content").not($(this).siblings(".dropdown-content")).removeClass("show");
//        $(this).siblings(".dropdown-content").toggleClass("show");
//    });

//    // إغلاق القائمة المنسدلة عند النقر خارجها
//    $(document).click(function () {
//        $(".dropdown-content").removeClass("show");
//    });

//    // منع إغلاق القائمة المنسدلة عند النقر داخلها
//    $(".dropdown-content").click(function (e) {
//        e.stopPropagation();
//    });
//});


//// إضافة هذا الكود في ملف JavaScript الخاص بك
//$(document).ready(function () {
//    // التأكد من إزالة أي قوائم منسدلة مفتوحة عند تحميل الصفحة
//    $(".dropdown-content").removeClass("show");

//    // عرض وإخفاء القائمة المنسدلة لتغيير الحالة
//    $(document).on("click", ".dropdown-btn", function (e) {
//        e.preventDefault();
//        e.stopPropagation();

//        // إغلاق جميع القوائم المنسدلة المفتوحة الأخرى
//        $(".dropdown-content").not($(this).siblings(".dropdown-content")).removeClass("show");

//        // تبديل حالة القائمة المنسدلة الحالية
//        var dropdownContent = $(this).siblings(".dropdown-content");
//        dropdownContent.toggleClass("show");

//        // للتحقق من فتح القائمة
//        console.log("تم النقر على الزر", dropdownContent.hasClass("show") ? "القائمة مفتوحة" : "القائمة مغلقة");
//    });

//    // منع إغلاق القائمة المنسدلة عند النقر داخلها
//    $(document).on("click", ".dropdown-content", function (e) {
//        e.stopPropagation();
//    });

//    // إغلاق جميع القوائم المنسدلة عند النقر في أي مكان آخر في الصفحة
//    $(document).on("click", function () {
//        $(".dropdown-content").removeClass("show");
//    });
//});