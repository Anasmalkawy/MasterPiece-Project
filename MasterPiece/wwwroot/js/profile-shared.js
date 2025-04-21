//// profile-shared.js
//document.addEventListener('DOMContentLoaded', function () {
//    // Handle validation messages display
//    const validationMessages = document.querySelectorAll('.text-danger');
//    validationMessages.forEach(message => {
//        if (message.textContent.trim() !== '') {
//            message.style.display = 'block';
//        } else {
//            message.style.display = 'none';
//        }
//    });

//    // Show success messages with fade effect
//    const successMessages = document.querySelectorAll('.alert-success');
//    successMessages.forEach(message => {
//        message.style.opacity = '1';
//        setTimeout(() => {
//            message.style.transition = 'opacity 1s ease';
//            message.style.opacity = '0';
//            setTimeout(() => {
//                message.style.display = 'none';
//            }, 1000);
//        }, 3000);
//    });

//    // Toggle order details visibility on row click
//    const orderRows = document.querySelectorAll('.order-row');
//    orderRows.forEach(row => {
//        row.addEventListener('click', function () {
//            const detailsRow = this.nextElementSibling;

//            if (detailsRow && detailsRow.classList.contains('order-details-row')) {
//                if (detailsRow.style.display === 'flex') {
//                    detailsRow.style.display = 'none';
//                } else {
//                    detailsRow.style.display = 'flex';
//                }
//            }
//        });
//    });

//    // Cancel Order Form Submit Handler
//    const cancelForms = document.querySelectorAll('.cancel-form');
//    cancelForms.forEach(form => {
//        const button = form.querySelector('.cancel-order-btn');
//        if (button) {
//            button.addEventListener('click', function (e) {
//                e.preventDefault();
//                if (confirm('هل أنت متأكد من إلغاء هذا الطلب؟')) {
//                    form.submit();
//                }
//            });
//        }
//    });

//    // Password strength validation for ChangePassword page
//    const newPasswordInput = document.querySelector('input[name="NewPassword"]');
//    const passwordStrengthIndicator = document.getElementById('password-strength');

//    if (newPasswordInput && passwordStrengthIndicator) {
//        newPasswordInput.addEventListener('input', function () {
//            const password = this.value;
//            let strength = 0;
//            let feedback = '';

//            if (password.length >= 8) strength += 1;
//            if (password.match(/[A-Z]/)) strength += 1;
//            if (password.match(/[0-9]/)) strength += 1;
//            if (password.match(/[^A-Za-z0-9]/)) strength += 1;

//            switch (strength) {
//                case 0:
//                    feedback = 'ضعيفة جداً';
//                    passwordStrengthIndicator.className = 'password-strength-weak';
//                    break;
//                case 1:
//                    feedback = 'ضعيفة';
//                    passwordStrengthIndicator.className = 'password-strength-weak';
//                    break;
//                case 2:
//                    feedback = 'متوسطة';
//                    passwordStrengthIndicator.className = 'password-strength-medium';
//                    break;
//                case 3:
//                    feedback = 'جيدة';
//                    passwordStrengthIndicator.className = 'password-strength-good';
//                    break;
//                case 4:
//                    feedback = 'قوية';
//                    passwordStrengthIndicator.className = 'password-strength-strong';
//                    break;
//            }

//            passwordStrengthIndicator.textContent = `قوة كلمة المرور: ${feedback}`;
//            passwordStrengthIndicator.style.display = password ? 'block' : 'none';
//        });
//    }

//    // Form validation for EditProfile
//    const editProfileForm = document.querySelector('form[action="/profile/EditProfile"]');
//    if (editProfileForm) {
//        editProfileForm.addEventListener('submit', function (e) {
//            let isValid = true;

//            // Email validation
//            const emailInput = document.querySelector('input[name="Email"]');
//            if (emailInput && !validateEmail(emailInput.value)) {
//                const emailError = emailInput.nextElementSibling;
//                emailError.textContent = 'الرجاء إدخال بريد إلكتروني صحيح';
//                emailError.style.display = 'block';
//                isValid = false;
//            }

//            // Phone validation
//            const phoneInput = document.querySelector('input[name="Phone"]');
//            if (phoneInput && !validatePhone(phoneInput.value)) {
//                const phoneError = phoneInput.nextElementSibling;
//                phoneError.textContent = 'الرجاء إدخال رقم هاتف صحيح';
//                phoneError.style.display = 'block';
//                isValid = false;
//            }

//            if (!isValid) {
//                e.preventDefault();
//            }
//        });
//    }

//    // Validate email format
//    function validateEmail(email) {
//        const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
//        return re.test(email);
//    }

//    // Validate phone number
//    function validatePhone(phone) {
//        return /^\d+$/.test(phone);
//    }
//});جراء عند النقر على زر التأكيد
//const confirmBtn = confirmModal.querySelector('.confirm-btn-confirm');
//confirmBtn.onclick = function () {
//    // إنشاء نموذج وإرساله
//    const form = document.createElement('form');
//    form.method = 'POST';
//    form.action = '/profile/CancelOrder/' + orderId;

//    // إضافة token منع التزييف CSRF إذا كان موجودًا
//    const csrfToken = document.querySelector('input[name="__RequestVerificationToken"]');
//    if (csrfToken) {
//        const tokenInput = document.createElement('input');
//        tokenInput.type = 'hidden';
//        tokenInput.name = '__RequestVerificationToken';
//        tokenInput.value = csrfToken.value;
//        form.appendChild(tokenInput);
//    }

//    document.body.appendChild(form);
//    form.submit();
//};

//// إظهار النافذة
//confirmModal.style.display = 'flex';
//}

//document.addEventListener('DOMContentLoaded', function () {
//    // Handle validation messages display
//    const validationMessages = document.querySelectorAll('.text-danger');
//    validationMessages.forEach(message => {
//        if (message.textContent.trim() !== '') {
//            message.style.display = 'block';
//        } else {
//            message.style.display = 'none';
//        }
//    });

//    // Show success messages with fade effect
//    const successMessages = document.querySelectorAll('.alert-success');
//    successMessages.forEach(message => {
//        message.style.opacity = '1';
//        setTimeout(() => {
//            message.style.transition = 'opacity 1s ease';
//            message.style.opacity = '0';
//            setTimeout(() => {
//                message.style.display = 'none';
//            }, 1000);
//        }, 3000);
//    });

//    // Toggle order details visibility
//    const viewDetailsBtns = document.querySelectorAll('.view-details-btn');
//    viewDetailsBtns.forEach(btn => {
//        btn.addEventListener('click', function () {
//            const orderRow = this.closest('.order-row');
//            const detailsRow = orderRow.nextElementSibling;

//            if (detailsRow && detailsRow.classList.contains('order-details-row')) {
//                if (detailsRow.style.display === 'flex') {
//                    detailsRow.style.display = 'none';
//                    this.querySelector('i').classList.remove('rotate-icon');
//                } else {
//                    detailsRow.style.display = 'flex';
//                    this.querySelector('i').classList.add('rotate-icon');
//                }
//            }
//        });
//    });

//    // Password strength validation for ChangePassword page
//    const newPasswordInput = document.querySelector('input[name="NewPassword"]');
//    const passwordStrengthIndicator = document.getElementById('password-strength');

//    if (newPasswordInput && passwordStrengthIndicator) {
//        newPasswordInput.addEventListener('input', function () {
//            const password = this.value;
//            let strength = 0;
//            let feedback = '';

//            if (password.length >= 8) strength += 1;
//            if (password.match(/[A-Z]/)) strength += 1;
//            if (password.match(/[0-9]/)) strength += 1;
//            if (password.match(/[^A-Za-z0-9]/)) strength += 1;

//            switch (strength) {
//                case 0:
//                    feedback = 'ضعيفة جداً';
//                    passwordStrengthIndicator.className = 'password-strength-weak';
//                    break;
//                case 1:
//                    feedback = 'ضعيفة';
//                    passwordStrengthIndicator.className = 'password-strength-weak';
//                    break;
//                case 2:
//                    feedback = 'متوسطة';
//                    passwordStrengthIndicator.className = 'password-strength-medium';
//                    break;
//                case 3:
//                    feedback = 'جيدة';
//                    passwordStrengthIndicator.className = 'password-strength-good';
//                    break;
//                case 4:
//                    feedback = 'قوية';
//                    passwordStrengthIndicator.className = 'password-strength-strong';
//                    break;
//            }

//            passwordStrengthIndicator.textContent = `قوة كلمة المرور: ${feedback}`;
//            passwordStrengthIndicator.style.display = password ? 'block' : 'none';
//        });
//    }

//    // Form validation for EditProfile
//    const editProfileForm = document.querySelector('form[action="/profile/EditProfile"]');
//    if (editProfileForm) {
//        editProfileForm.addEventListener('submit', function (e) {
//            let isValid = true;

//            // Email validation
//            const emailInput = document.querySelector('input[name="Email"]');
//            if (emailInput && !validateEmail(emailInput.value)) {
//                const emailError = emailInput.nextElementSibling;
//                emailError.textContent = 'الرجاء إدخال بريد إلكتروني صحيح';
//                emailError.style.display = 'block';
//                isValid = false;
//            }

//            // Phone validation
//            const phoneInput = document.querySelector('input[name="Phone"]');
//            if (phoneInput && !validatePhone(phoneInput.value)) {
//                const phoneError = phoneInput.nextElementSibling;
//                phoneError.textContent = 'الرجاء إدخال رقم هاتف صحيح';
//                phoneError.style.display = 'block';
//                isValid = false;
//            }

//            if (!isValid) {
//                e.preventDefault();
//            }
//        });
//    }

//    // Validate email format
//    function validateEmail(email) {
//        const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
//        return re.test(email);
//    }

//    // Validate phone number
//    function validatePhone(phone) {
//        return /^\d+$/.test(phone);
//    }
//});