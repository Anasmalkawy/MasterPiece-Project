//// JavaScript لصفحة MasterPiece الرئيسية
//$(document).ready(function () {
//    // تهيئة AOS للتأثيرات الحركية
//    AOS.init({
//        duration: 800,
//        easing: 'ease-in-out',
//        once: true
//    });

//    // تحميل الهيرو مع تأثير
//    setTimeout(function () {
//        $('.hero-container').addClass('loaded');
//    }, 500);

//    // إضافة شارات العروض بشكل عشوائي
//    $('.product-card').each(function (index) {
//        // إضافة شارة العرض لبعض المنتجات
//        if (index % 3 === 0) {
//            let discountValue = Math.floor(Math.random() * 30) + 10;
//            $(this).append(`<div class="discount-badge">خصم ${discountValue}%</div>`);
//        }

//        // تأخير ظهور البطاقات للتأثير الحركي
//        let delay = index * 100;
//        $(this).css('animation-delay', delay + 'ms');
//    });

//    // تأثير التمرير السلس للأقسام
//    $('a[href*="#"]').on('click', function (e) {
//        e.preventDefault();
//        $('html, body').animate({
//            scrollTop: $($(this).attr('href')).offset().top - 100
//        }, 500, 'linear');
//    });

//    // محاكاة تحميل الصور بتأثير ناعم
//    $('.product-img-container img').each(function () {
//        const originalSrc = $(this).attr('src');
//        $(this).attr('src', ''); // تفريغ المصدر مؤقتًا

//        // إضافة تأثير التحميل
//        $(this).addClass('loading-img');

//        // إعادة تحميل الصورة مع تأثير
//        setTimeout(() => {
//            $(this).attr('src', originalSrc);
//            $(this).removeClass('loading-img');
//            $(this).addClass('loaded-img');
//        }, Math.random() * 1000);
//    });

//    // تأثير التكبير عند التمرير
//    $(window).scroll(function () {
//        const scrollTop = $(this).scrollTop();
//        $('.product-section').each(function () {
//            const sectionOffset = $(this).offset().top;
//            const sectionHeight = $(this).outerHeight();

//            if (scrollTop > sectionOffset - window.innerHeight + sectionHeight / 3) {
//                $(this).addClass('section-in-view');
//            }
//        });
//    });

//    // تأثير تكبير وتصغير بطاقات المنتجات عند التمرير فوقها
//    $('.product-card').hover(
//        function () {
//            $(this).find('.card-body').addClass('card-body-hover');
//        },
//        function () {
//            $(this).find('.card-body').removeClass('card-body-hover');
//        }
//    );

//    // عرض عدد المنتجات في السلة (افتراضي)
//    let cartCount = localStorage.getItem('cartCount') || 0;
//    updateCartCount(cartCount);

//    // تحديث: استخدام مفوض الأحداث للتعامل مع جميع روابط إضافة المنتج للسلة
//    // بغض النظر عن تغييرات CSS أو تعليقات في الكود
//    $(document).on('click', 'a[asp-controller="product"][asp-action="addCart"]', function (e) {
//        // منع النقر المباشر والتوجيه
//        e.preventDefault();

//        // تأثير النقر
//        const btn = $(this);
//        btn.addClass('btn-clicked');
//        setTimeout(() => {
//            btn.removeClass('btn-clicked');
//        }, 300);

//        // تحديث عداد السلة
//        cartCount++;
//        localStorage.setItem('cartCount', cartCount);
//        updateCartCount(cartCount);

//        // عرض رسالة تم الإضافة بنجاح
//        showToast('تمت إضافة المنتج إلى السلة بنجاح');

//        // الحصول على معرف المنتج من سمة المسار
//        const productId = $(this).attr('asp-route-id');

//        // إنشاء URL استنادًا إلى سمات ASP.NET
//        const url = `/product/addCart/${productId}`;

//        // إرسال طلب AJAX لإضافة المنتج (محاكاة)
//        simulateAddToCart(url);
//    });

//    // إضافة شريط الترويج المتحرك
//    initPromoBanner();

//    // إضافة قسم العلامات التجارية
//    initBrandsSlider();
//});

//// وظيفة لتحديث عدد عناصر السلة
//function updateCartCount(count) {
//    // تحديث العدد في الأيقونة في الهيدر (يفترض وجودها)
//    $('.cart-count').text(count);

//    // تأثير النبض إذا كان العدد أكبر من 0
//    if (count > 0) {
//        $('.cart-count').addClass('pulse-animation');
//        setTimeout(() => {
//            $('.cart-count').removeClass('pulse-animation');
//        }, 1000);
//    }
//}

//// وظيفة لعرض رسالة نجاح
//function showToast(message) {
//    // إنشاء عنصر التنبيه
//    if (!$('#toast-container').length) {
//        $('body').append('<div id="toast-container" class="position-fixed top-0 end-0 p-3" style="z-index: 1080;"></div>');
//    }

//    // إنشاء رسالة التنبيه
//    const toastId = 'toast-' + Date.now();
//    const toast = `
//        <div id="${toastId}" class="toast align-items-center text-white bg-success border-0" role="alert" aria-live="assertive" aria-atomic="true">
//            <div class="d-flex">
//                <div class="toast-body">
//                    ${message}
//                </div>
//                <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="إغلاق"></button>
//            </div>
//        </div>
//    `;

//    // إضافة التنبيه إلى الحاوية
//    $('#toast-container').append(toast);

//    // عرض التنبيه
//    $(`#${toastId}`).toast({
//        delay: 3000,
//        animation: true
//    }).toast('show');

//    // إزالة التنبيه بعد الإغلاق
//    $(`#${toastId}`).on('hidden.bs.toast', function () {
//        $(this).remove();
//    });
//}

//// محاكاة إضافة منتج إلى السلة بـ AJAX
//function simulateAddToCart(url) {
//    // هذه وظيفة محاكاة فقط - في التطبيق الحقيقي ستستخدم AJAX
//    console.log('إضافة المنتج من الرابط: ' + url);

//    // يمكنك استبدال هذا بطلب AJAX حقيقي
//    /*
//    $.ajax({
//        url: url,
//        type: 'POST',
//        success: function(response) {
//            // التعامل مع الاستجابة
//        },
//        error: function(error) {
//            // التعامل مع الخطأ
//        }
//    });
//    */
//}

//// إضافة شريط الترويج المتحرك
//function initPromoBanner() {
//    const promoSection = `
//        <section class="promo-section" data-aos="fade-up">
//            <div class="container">
//                <div class="row">
//                    <div class="col-md-6 mb-4 mb-md-0">
//                        <div class="promo-card">
//                            <img src="/img/promo-laptop.jpg" alt="عروض اللابتوبات" class="promo-image">
//                            <div class="promo-content">
//                                <h3 class="promo-title">أحدث اللابتوبات</h3>
//                                <p class="promo-subtitle">تصفح تشكيلتنا من أحدث اللابتوبات بأفضل الأسعار</p>
//                                <a href="#" class="promo-button">تسوق الآن</a>
//                            </div>
//                        </div>
//                    </div>
//                    <div class="col-md-6">
//                        <div class="promo-card">
//                            <img src="/img/promo-phone.jpg" alt="عروض الجوالات" class="promo-image">
//                            <div class="promo-content">
//                                <h3 class="promo-title">جوالات ذكية</h3>
//                                <p class="promo-subtitle">اكتشف أحدث الجوالات بمواصفات متطورة</p>
//                                <a href="#" class="promo-button">استكشف المزيد</a>
//                            </div>
//                        </div>
//                    </div>
//                </div>
//            </div>
//        </section>
//    `;

//    // إضافة قسم الترويج بين قسمي المنتجات
//    $('.product-section').first().after(promoSection);
//}

//// إضافة شريط العلامات التجارية
//function initBrandsSlider() {
//    // شعارات العلامات التجارية - استبدلها بالصور الحقيقية
//    const brands = [
//        'apple.png', 'samsung.png', 'dell.png', 'hp.png',
//        'asus.png', 'lenovo.png', 'huawei.png', 'sony.png'
//    ];

//    let brandItems = '';
//    brands.forEach(brand => {
//        brandItems += `
//            <div class="brand-item">
//                <img src="/img/brands/${brand}" alt="علامة تجارية">
//            </div>
//        `;
//    });

//    const brandsSection = `
//        <section class="brands-section" data-aos="fade-up">
//            <div class="container">
//                <div class="brands-title">
//                    <h3 class="fw-bold">علاماتنا التجارية</h3>
//                </div>
//                <div class="brands-container overflow-hidden">
//                    <div class="brands-slider">
//                        ${brandItems}
//                        ${brandItems}
//                    </div>
//                </div>
//            </div>
//        </section>
//    `;

//    // إضافة قسم العلامات التجارية قبل نهاية الصفحة
//    $('.product-section').last().after(brandsSection);
//}

//// إضافة تأثير الظهور عند التمرير
//document.addEventListener('DOMContentLoaded', function () {
//    const observer = new IntersectionObserver((entries) => {
//        entries.forEach(entry => {
//            if (entry.isIntersecting) {
//                entry.target.classList.add('in-view');
//                observer.unobserve(entry.target);
//            }
//        });
//    }, {
//        threshold: 0.1
//    });

//    document.querySelectorAll('.product-card, .section-title, .promo-card').forEach(element => {
//        observer.observe(element);
//    });
//});