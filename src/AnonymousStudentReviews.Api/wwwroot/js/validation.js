document.addEventListener("DOMContentLoaded", function () {
    const form = document.querySelector("form");

    const fields = {
        Email: {
            input: document.querySelector("input[name='Email']"),
            error: document.querySelector("span[data-valmsg-for='Email']"),
            validate: value => {
                if (!value.trim()) return "The email can't be empty";
                if (value.length > 70) return "The email's length must not exceed 70 characters";
                if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value)) return "The email must be in the email format";
                return "";
            }
        },
        Password: {
            input: document.querySelector("input[name='Password']"),
            error: document.querySelector("span[data-valmsg-for='Password']"),
            validate: value => {
                if (!value) return "Password is required.";
                if (value.length < 12) return "Password must be at least 12 characters long.";
                if (value.length > 128) return "Password must not exceed 128 characters.";
                if (!/[A-Z]/.test(value)) return "Password must contain at least one uppercase letter.";
                if (!/[a-z]/.test(value)) return "Password must contain at least one lowercase letter.";
                if (!/\d/.test(value)) return "Password must contain at least one number.";
                if (!/[^\w\d\s]/.test(value)) return "Password must contain at least one special character.";
                return "";
            }
        },
        ConfirmPassword: {
            input: document.querySelector("input[name='ConfirmPassword']"),
            error: document.querySelector("span[data-valmsg-for='ConfirmPassword']"),
            validate: value => {
                const password = fields.Password.input.value;
                if (!value) return "Password is required.";
                if (value !== password) return "Passwords do not match";
                return "";
            }
        }
    };

    function validateField(field) {
        const message = field.validate(field.input.value);
        field.error.textContent = message;
        return message === "";
    }

    Object.values(fields).forEach(field => {
        field.input.addEventListener("input", () => validateField(field));
        field.input.addEventListener("blur", () => validateField(field));
    });


    form.addEventListener("submit", function (e) {
        let allValid = true;

        Object.values(fields).forEach(field => {
            if (!validateField(field)) {
                allValid = false;
            }
        });

        if (!allValid) {
            e.preventDefault();
        }
    });
});
