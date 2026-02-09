document.addEventListener('DOMContentLoaded', () => {
    const CLASSES = {
        default: ['border-gray-400', 'bg-white', 'focus:ring-blue-500'],
        error: ['border-red-700', 'bg-gray-100', 'text-gray-900'],
    };

    const form = document.getElementById('regForm');

    const emailInput = document.getElementById('emailInput');
    const passInput = document.getElementById('passwordInput');
    const confirmInput = document.getElementById('confirmInput');

    const emailError = document.getElementById('emailError');
    const passError = document.getElementById('passwordError');
    const confirmError = document.getElementById('confirmError');
    
    function setInputState(input, isValid, errorEl, errorMsg = null) {
        input.classList.remove(...CLASSES.error);

        if (isValid) {
            if (errorEl) errorEl.classList.add('hidden');
        } else {
            input.classList.add(...CLASSES.error);
            if (errorEl) {
                errorEl.classList.remove('hidden');
                if (errorMsg) errorEl.innerText = errorMsg;
            }
        }
    }

    const validateEmail = () => {
        const val = emailInput.value.trim();
        const isValid = /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(val);
        setInputState(emailInput, isValid, emailError);
        return isValid;
    };
    
    const validatePass = () => {
        const p = passInput.value || '';
        let msg = null;

        if (!p) {
            msg = "Пароль обов'язковий.";
        }
        else if (p.length < 12) {
            msg = "Мінімум 12 символів.";
        }
        else if (p.length > 128) {
            msg = "Максимум 128 символів.";
        }
        else if (!/[A-Z]/.test(p)) {
            msg = "Потрібна хоча б одна велика літера.";
        }
        else if (!/[a-z]/.test(p)) {
            msg = "Потрібна хоча б одна мала літера.";
        }
        else if (!/\d/.test(p)) {
            msg = "Потрібна хоча б одна цифра.";
        }
        else if (!/[^a-zA-Z0-9_\s]/.test(p)) {
            msg = "Потрібен хоча б один спецсимвол.";
        }
        
        setInputState(passInput, msg === null, passError, msg);
        
        if (confirmInput.value) validateConfirm();

        return msg === null;
    };

    const validateConfirm = () => {
        const isValid = confirmInput.value === passInput.value && confirmInput.value !== "";
        setInputState(confirmInput, isValid, confirmError);
        return isValid;
    };

    emailInput.addEventListener('blur', validateEmail);
    passInput.addEventListener('blur', validatePass);
    confirmInput.addEventListener('blur', validateConfirm);

    emailInput.addEventListener('input', () => {
        if (emailInput.classList.contains('bg-red-50')) validateEmail();
    });

    form.addEventListener('submit', (e) => {
        const v2 = validateEmail();
        const v3 = validatePass();
        const v4 = validateConfirm();

        if (!v2 || !v3 || !v4) {
            e.preventDefault();
        }
    });
});