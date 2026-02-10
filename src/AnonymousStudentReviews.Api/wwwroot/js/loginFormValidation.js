document.addEventListener('DOMContentLoaded', () => {
    const form = document.getElementById('loginForm');
    const emailInput = document.getElementById('email');
    const passwordInput = document.getElementById('password');
    const emailError = document.getElementById('emailError');
    const passwordError = document.getElementById('passwordError');

    const CLASSES = {
        errorInput: ['border-red-700', 'bg-gray-100', 'text-gray-900'],
        errorLabel: ['text-red-700', 'dark:text-red-400'],
        normalInput: ['border-gray-300', 'bg-gray-50', 'text-gray-900', 'focus:ring-blue-500'],
        normalLabel: ['text-gray-600', 'dark:text-gray-400']
    };

    const setInputError = (input, errorSpan, isError, message = '') => {
        const label = input.closest('div').querySelector('label');

        if (isError) {
            // Remove normal classes
            input.classList.remove('border-gray-300', 'bg-gray-50', 'focus:ring-blue-500');
            label.classList.remove(...CLASSES.normalLabel);

            // Add error classes
            input.classList.add(...CLASSES.errorInput);
            label.classList.add(...CLASSES.errorLabel);

            // Show error message
            if (errorSpan) {
                errorSpan.textContent = message;
                errorSpan.classList.remove('hidden');
            }
        } else {
            // Remove error classes
            input.classList.remove(...CLASSES.errorInput);
            label.classList.remove(...CLASSES.errorLabel);

            // Add normal classes
            input.classList.add('border-gray-300', 'bg-gray-50', 'focus:ring-blue-500');
            label.classList.add(...CLASSES.normalLabel);

            // Hide error message
            if (errorSpan) {
                errorSpan.classList.add('hidden');
            }
        }
    };

    const validateEmail = () => {
        const value = emailInput.value.trim();
        if (!value) {
            setInputError(emailInput, emailError, true, "Введіть електронну пошту");
            return false;
        }
        const isValid = /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value);
        if (!isValid) {
            setInputError(emailInput, emailError, true, "Невірний формат електронної пошти");
            return false;
        }
        setInputError(emailInput, emailError, false);
        return true;
    };

    const validatePassword = () => {
        const value = passwordInput.value;
        if (!value) {
            setInputError(passwordInput, passwordError, true, "Введіть пароль");
            return false;
        }
        setInputError(passwordInput, passwordError, false);
        return true;
    };
    
    emailInput.addEventListener('input', () => {
        if (emailInput.classList.contains('border-red-700')) validateEmail();
    });
    passwordInput.addEventListener('input', () => {
        if (passwordInput.classList.contains('border-red-700')) validatePassword();
    });
    
    emailInput.addEventListener('blur', validateEmail);
    passwordInput.addEventListener('blur', validatePassword);
    
    form.addEventListener('submit', (e) => {
        const isEmailValid = validateEmail();
        const isPasswordValid = validatePassword();

        if (!isEmailValid || !isPasswordValid) {
            e.preventDefault();
        }
    });
});