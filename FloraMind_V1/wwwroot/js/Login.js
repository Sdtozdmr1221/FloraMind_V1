document.addEventListener('DOMContentLoaded', function () {
    const loginContainer = document.getElementById('loginContainer');
    const loginForm = document.getElementById('loginForm');
    const goToRegisterBtn = document.getElementById('goToRegister');
    const goToLayoutBtn = document.getElementById('goToLayout');

    if (goToRegisterBtn) {
        goToRegisterBtn.addEventListener('click', function (e) {
            e.preventDefault();
            loginContainer.classList.add('fade-out');
            setTimeout(function () {
                window.location.href = '/Account/Register';
            }, 300);
        });
    }

    if (goToLayoutBtn) {
        goToLayoutBtn.addEventListener('click', function (e) {
            e.preventDefault();
            loginContainer.classList.add('fade-out');
            setTimeout(function () {
                window.location.href = '/';
            }, 300);
        });
    }

    loginForm.addEventListener('submit', function (e) {
        e.preventDefault();

        const emailInput = loginForm.querySelector('input[type="email"]');
        const passwordInput = loginForm.querySelector('input[type="password"]');
        let isValid = true;

        // Email kontrolü
        const emailVal = emailInput.value.trim();
        if (emailVal === "") {
            showError(emailInput, "Email boş olamaz");
            isValid = false;
        } else if (!validateEmail(emailVal)) {
            showError(emailInput, "Geçerli bir email giriniz");
            isValid = false;
        } else {
            clearError(emailInput);
        }

        // Şifre kontrolü
        const pwd = passwordInput.value;
        if (pwd === "") {
            showError(passwordInput, "Şifre boş olamaz");
            isValid = false;
        } else if (pwd.length < 6) {
            showError(passwordInput, "Şifre en az 6 karakter olmalı");
            isValid = false;
        } else {
            clearError(passwordInput);
        }

        if (!isValid) return;

        loginContainer.classList.add('fade-out');
        setTimeout(function () {
            loginForm.submit();
        }, 300);
    });
});

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