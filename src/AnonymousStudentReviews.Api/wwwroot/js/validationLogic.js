const {createApp, ref, computed, watch} = Vue;

createApp({
    setup() {
        const selectedRole = ref(null);
        const email = ref("");
        const password = ref("");
        const confirmationPassword = ref("");

        const currentError = ref(null);
        const roleError = computed(() => currentError.value === 'role');
        const emailError = computed(() => currentError.value === 'email');
        const differentPasswordsError = computed(() => currentError.value === 'different-passwords');

        const roles = ['Студент', 'Абітурієнт', 'Інше'];

        const selectRole = (role) => {
            if (selectedRole.value === role) {
                selectedRole.value = null;
                return;
            }
            selectedRole.value = role;
        }

        watch(selectedRole, (newRole) => {
            if (newRole != null && roleError.value) {
                currentError.value = null;
            }
        });

        watch(email, (newEmail, oldEmail) => {
            if (newEmail !== oldEmail && emailError.value) {
                currentError.value = null;
            }
        });

        watch(password, (newPassword, oldPassword) => {
            if (newPassword !== oldPassword && differentPasswordsError.value) {
                currentError.value = null;
            }
        });

        watch(confirmationPassword, (newPassword, oldPassword) => {
            if (newPassword !== oldPassword && differentPasswordsError.value) {
                currentError.value = null;
            }
        });

        const checkRole = () => {
            return selectedRole.value != null;
        }

        const checkEmail = () => {
            const allowedDomain = "knu.ua";

            if (!email.value.includes("@")) return false;
            return email.value.split("@")[1] === allowedDomain;
        }

        const checkPasswordsSimilarity = () => {
            return password.value === confirmationPassword.value;
        }

        const handleSubmit = () => {
            currentError.value = null;

            if (!checkRole()) {
                currentError.value = 'role';
                return;
            }

            if (!checkEmail()) {
                currentError.value = 'email';
                return;
            }

            if (!checkPasswordsSimilarity()) {
                currentError.value = 'different-passwords';
                return;
            }

            alert(`Success! Selected role: ${selectedRole.value} ${email.value} Match: ${password.value === confirmationPassword.value}`);
        }

        return {
            roles,
            selectedRole,
            email,
            password,
            confirmationPassword,
            roleError,
            emailError,
            differentPasswordsError,
            selectRole,
            handleSubmit
        };
    }
}).mount('#app');