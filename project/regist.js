import { registerUser, checkEmail } from './api.js';

const pw = document.getElementById("pw");
const pw2 = document.getElementById("pw2");
const eye = document.getElementById("eye");
const eye2 = document.getElementById("eye2");

eye.addEventListener("click", () => {
    pw.type = pw.type === "password" ? "text" : "password";
    eye.src = pw.type === "text" ? "eyeclosed.png" : "eye.png";
});

eye2.addEventListener("click", () => {
    pw2.type = pw2.type === "password" ? "text" : "password";
    eye2.src = pw2.type === "text" ? "eyeclosed.png" : "eye.png";
});

document.getElementById("submitBtn").addEventListener("click", async () => {
    const email = document.getElementById("email").value.trim();
    const firstName = document.getElementById("firstName").value.trim();
    const lastName = document.getElementById("lastName").value.trim();
    const password = pw.value;
    const password2 = pw2.value;
    const acceptedTerms = document.getElementById("c1").checked;
    const subscribedToNewsletter = document.getElementById("c2").checked;

    if (!email || !firstName || !lastName || !password) {
        alert("Minden mező kötelező!");
        return;
    }
    if (password !== password2) {
        alert("A két jelszó nem egyezik!");
        return;
    }
    if (!acceptedTerms) {
        alert("El kell fogadnod az ÁSZF-et!");
        return;
    }

    try {
        const existingUser = await checkEmail(email);
        if (existingUser) {
            alert("Ez az email már regisztrálva van!");
            return;
        }

        const newUser = await registerUser({ email, password, firstName, lastName, acceptedTerms, subscribedToNewsletter });
        localStorage.setItem("user", JSON.stringify(newUser));
        alert("Sikeres regisztráció!");
        window.location.href = "login.html";

    } catch (error) {
        alert("Hiba történt: " + error.message);
    }
});