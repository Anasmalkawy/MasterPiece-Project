// تصميم عالمي احترافي لصفحات الشراء - checkout.js
document.addEventListener('DOMContentLoaded', function () {
    // التحقق من وجود عناصر في الصفحة قبل تنفيذ الدوال
    initCartQuantityControls();
    initDiscountCode();
    initPaymentOptions();
    initFormValidation();
    initAnimations();
});

// التحكم في الكمية في سلة المشتريات
function initCartQuantityControls() {
    // أزرار زيادة ونقصان الكمية
    const incrementButtons = document.querySelectorAll('.quantity-button.plus');
    const decrementButtons = document.querySelectorAll('.quantity-button.minus');

    if (incrementButtons.length > 0) {
        incrementButtons.forEach(button => {
            button.addEventListener('click', function () {
                const cartId = this.getAttribute('onclick').match(/\d+/)[0];
                incrementQuantity(cartId);
                updateItemTotal(cartId);
            });
        });
    }

    if (decrementButtons.length > 0) {
        decrementButtons.forEach(button => {
            button.addEventListener('click', function () {
                const cartId = this.getAttribute('onclick').match(/\d+/)[0];
                decrementQuantity(cartId);
                updateItemTotal(cartId);
            });
        });
    }

    // تحديث إجمالي المنتج عند تغيير الكمية
    const quantityInputs = document.querySelectorAll('.quantity-input');
    if (quantityInputs.length > 0) {
        quantityInputs.forEach(input => {
            input.addEventListener('change', function () {
                const cartId = this.id.replace('quantity-', '');
                updateItemTotal(cartId);
            });
        });
    }
}

// زيادة الكمية
function incrementQuantity(cartId) {
    const inputElement = document.getElementById('quantity-' + cartId);
    if (inputElement) {
        inputElement.value = parseInt(inputElement.value) + 1;
    }
}

// إنقاص الكمية
function decrementQuantity(cartId) {
    const inputElement = document.getElementById('quantity-' + cartId);
    if (inputElement && parseInt(inputElement.value) > 1) {
        inputElement.value = parseInt(inputElement.value) - 1;
    }
}

// تحديث إجمالي سعر المنتج بناءً على الكمية
function updateItemTotal(cartId) {
    const quantityInput = document.getElementById('quantity-' + cartId);
    const priceElement = quantityInput.closest('.table-row').querySelector('.cart-price');
    const totalElement = quantityInput.closest('.table-row').querySelector('.cart-total');

    if (quantityInput && priceElement && totalElement) {
        const quantity = parseInt(quantityInput.value);
        const price = parseFloat(priceElement.textContent.replace('$', ''));
        const total = (quantity * price).toFixed(2);

        totalElement.textContent = '$' + total;

        // تحديث الإجمالي الكلي للسلة
        updateCartTotal();
    }
}

// تحديث إجمالي السلة
function updateCartTotal() {
    const totalElements = document.querySelectorAll('.cart-total');
    let subtotal = 0;

    totalElements.forEach(element => {
        subtotal += parseFloat(element.textContent.replace('$', ''));
    });

    // تحديث المجموع الفرعي والإجمالي في القسم الجانبي
    const subtotalElement = document.querySelector('.order-subtotal p');
    const totalElement = document.querySelector('.order-total p');

    if (subtotalElement && totalElement) {
        subtotalElement.textContent = '$' + subtotal.toFixed(2);
        totalElement.textContent = '$' + subtotal.toFixed(2); // يمكن تعديله لإضافة الضرائب أو الشحن
    }
}

// التعامل مع كود الخصم
function initDiscountCode() {
    const discountButtons = document.querySelectorAll('.apply-discount');

    if (discountButtons.length > 0) {
        discountButtons.forEach(button => {
            button.addEventListener('click', function (e) {
                e.preventDefault();

                const discountInput = this.previousElementSibling;
                const discountCode = discountInput.value.trim();

                if (discountCode) {
                    // هنا يمكن إضافة رمز للتحقق من الكود عبر AJAX
                    applyDiscount(discountCode);
                } else {
                    showNotification('الرجاء إدخال كود الخصم', 'error');
                }
            });
        });
    }
}

// تطبيق الخصم (نموذج بسيط)
function applyDiscount(code) {
    // في التطبيق الحقيقي، يجب التحقق من الكود عبر طلب AJAX للخادم
    if (code === 'WELCOME10') {
        // تطبيق خصم 10%
        const subtotalElement = document.querySelector('.order-subtotal p');
        const totalElement = document.querySelector('.order-total p');

        if (subtotalElement && totalElement) {
            const subtotal = parseFloat(subtotalElement.textContent.replace('$', ''));
            const discount = subtotal * 0.1;
            const newTotal = subtotal - discount;

            totalElement.textContent = '$' + newTotal.toFixed(2);

            // إضافة عرض للخصم المطبق (يجب إضافة العنصر إلى HTML)
            const discountRow = document.createElement('div');
            discountRow.classList.add('order-discount');
            discountRow.innerHTML = `
                <span>خصم (10%)</span>
                <p>-$${discount.toFixed(2)}</p>
            `;

            // إضافة الخصم قبل المجموع النهائي
            const orderSummary = document.querySelector('.order-summary');
            orderSummary.insertBefore(discountRow, document.querySelector('.order-total'));

            showNotification('تم تطبيق الخصم بنجاح', 'success');
        }
    } else {
        showNotification('كود الخصم غير صالح', 'error');
    }
}

// خيارات الدفع
function initPaymentOptions() {
    const paymentOptions = document.querySelectorAll('.payment-option');

    if (paymentOptions.length > 0) {
        paymentOptions.forEach(option => {
            option.addEventListener('click', function () {
                // تحديد الخيار المختار
                const radio = this.querySelector('input[type="radio"]');
                radio.checked = true;

                // إضافة فئة مختارة لتمييز الخيار المحدد
                paymentOptions.forEach(opt => opt.classList.remove('selected'));
                this.classList.add('selected');
            });
        });
    }
}

// التحقق من صحة نموذج الدفع
function initFormValidation() {
    const checkoutForm = document.querySelector('.checkout-form');

    if (checkoutForm) {
        checkoutForm.addEventListener('submit', function (e) {
            const requiredFields = this.querySelectorAll('[required]');
            let isValid = true;

            requiredFields.forEach(field => {
                if (!field.value.trim()) {
                    isValid = false;
                    field.classList.add('error');
                } else {
                    field.classList.remove('error');
                }
            });

            if (!isValid) {
                e.preventDefault();
                showNotification('الرجاء ملء جميع الحقول المطلوبة', 'error');
            }
        });
    }
}

// تأثيرات الحركة
function initAnimations() {
    // تأثير للصورة في صفحة نجاح الطلب
    const successImage = document.querySelector('.success-img img');
    if (successImage) {
        // تم تنفيذ التأثير في CSS باستخدام @keyframes
    }

    // تأثير لأزرار الكمية
    const quantityButtons = document.querySelectorAll('.quantity-button');
    if (quantityButtons.length > 0) {
        quantityButtons.forEach(button => {
            button.addEventListener('mousedown', function () {
                this.style.transform = 'scale(0.95)';
            });

            button.addEventListener('mouseup', function () {
                this.style.transform = 'scale(1)';
            });

            button.addEventListener('mouseleave', function () {
                this.style.transform = 'scale(1)';
            });
        });
    }

    // تأثير للأزرار الرئيسية
    const mainButtons = document.querySelectorAll('.continue-button, .checkout-button a, .continue-button1 a');
    if (mainButtons.length > 0) {
        mainButtons.forEach(button => {
            button.addEventListener('mouseover', function () {
                this.style.transform = 'translateY(-2px)';
                this.style.boxShadow = '0 4px 8px rgba(0, 0, 0, 0.1)';
            });

            button.addEventListener('mouseout', function () {
                this.style.transform = 'translateY(0)';
                this.style.boxShadow = '0 2px 5px rgba(0, 0, 0, 0.05)';
            });
        });
    }
}

// إظهار إشعارات للمستخدم
function showNotification(message, type = 'info') {
    // إنشاء عنصر الإشعار
    const notification = document.createElement('div');
    notification.className = `notification ${type}`;
    notification.innerHTML = `
        <div class="notification-content">
            <i class='bx ${type === 'success' ? 'bx-check-circle' : type === 'error' ? 'bx-x-circle' : 'bx-info-circle'}'></i>
            <span>${message}</span>
        </div>
        <button class="notification-close"><i class='bx bx-x'></i></button>
    `;

    // إضافة الإشعار إلى الصفحة
    document.body.appendChild(notification);

    // إظهار الإشعار بتأثير حركي
    setTimeout(() => {
        notification.classList.add('show');
    }, 10);

    // زر إغلاق الإشعار
    const closeButton = notification.querySelector('.notification-close');
    closeButton.addEventListener('click', () => {
        closeNotification(notification);
    });

    // إغلاق الإشعار تلقائياً بعد 5 ثوانٍ
    setTimeout(() => {
        closeNotification(notification);
    }, 5000);
}

// إغلاق الإشعار بتأثير حركي
function closeNotification(notification) {
    notification.classList.remove('show');

    // إزالة العنصر بعد انتهاء التأثير
    setTimeout(() => {
        notification.remove();
    }, 300);
}

// التحقق من أن المستعرض قد قام بتحميل كل شيء
document.addEventListener('load', function () {
    // إزالة فئة التحميل إذا كانت موجودة
    document.body.classList.remove('loading');
});