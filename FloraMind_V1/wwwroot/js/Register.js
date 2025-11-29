document.addEventListener('DOMContentLoaded', function () {
    const registerContainer = document.getElementById('registerContainer');
    const registerForm = document.getElementById('registerForm');
    const goToLoginBtn = document.getElementById('goToLogin');
    const goToLayoutBtn = document.getElementById('goToLayout');

    if (goToLoginBtn) {
        goToLoginBtn.addEventListener('click', function (e) {
            e.preventDefault();
            registerContainer.classList.add('fade-out');
            setTimeout(function () {
                window.location.href = '/Account/Login';
            }, 300);
        });
    }

    if (goToLayoutBtn) {
        goToLayoutBtn.addEventListener('click', function (e) {
            e.preventDefault();
            registerContainer.classList.add('fade-out');
            setTimeout(function () {
                window.location.href = '/';
            }, 300);
        });
    }

    registerForm.addEventListener('submit', function (e) {
        e.preventDefault();

        const nameInput = document.getElementById('name');
        const emailInput = document.getElementById('email');
        const passwordInput = document.getElementById('password');
        let isValid = true;

        // İsim kontrolü
        const nameVal = nameInput.value.trim();
        if (nameVal === '') {
            showError(nameInput, 'İsim boş olamaz');
            isValid = false;
        } else if (nameVal.length < 3) {
            showError(nameInput, 'İsim en az 3 karakter olmalı');
            isValid = false;
        } else {
            clearError(nameInput);
        }

        // Email kontrolü
        const emailVal = emailInput.value.trim();
        if (emailVal === '') {
            showError(emailInput, 'Email boş olamaz');
            isValid = false;
        } else if (!validateEmail(emailVal)) {
            showError(emailInput, 'Geçerli bir email giriniz');
            isValid = false;
        } else {
            clearError(emailInput);
        }

        // Şifre kontrolü
        const pwd = passwordInput.value;
        if (pwd === '') {
            showError(passwordInput, 'Şifre boş olamaz');
            isValid = false;
        } else if (pwd.length < 6) {
            showError(passwordInput, 'Şifre en az 6 karakter olmalı');
            isValid = false;
        } else {
            clearError(passwordInput);
        }

        if (!isValid) return;

        registerContainer.classList.add('fade-out');
        setTimeout(function () {
            registerForm.submit();
        }, 300);
    });
});
//doğru email formatı yazılması için (regex) aşağıdaki fonksiyon
function validateEmail(email) {
    const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(email);
}

function showError(input, message) {
    input.style.borderColor = '#e74c3c';
    const parent = input.parentElement;
    const oldError = parent.querySelector('small.error-message');
    if (oldError) oldError.remove();

    const error = document.createElement('small');
    error.className = 'error-message';
    error.textContent = message;
    error.style.color = '#e74c3c';
    error.style.fontSize = '12px';
    error.style.display = 'block';
    error.style.marginTop = '5px';
    error.style.textAlign = 'left';
    parent.appendChild(error);
}

function clearError(input) {
    input.style.borderColor = '#ddd';
    const parent = input.parentElement;
    const oldError = parent.querySelector('small.error-message');
    if (oldError) oldError.remove();
}