import { loginUser } from './api.js';

const pw = document.getElementById("pw");
const eye = document.getElementById("eye");

eye.addEventListener("click", () => {
    pw.type = pw.type === "password" ? "text" : "password";
    eye.src = pw.type === "text" ? "eyeclosed.png" : "eye.png";
});

document.querySelector("button").addEventListener("click", async () => {
    const email = document.querySelector("input[type='email']").value.trim();
    const password = pw.value;

    if (!email || !password) {
        alert("Add meg az emailt és jelszót!");
        return;
    }

    try {
        const res = await loginUser({ email, password });
        localStorage.setItem("token", res.token);
        localStorage.setItem("user", JSON.stringify(res));
        localStorage.setItem("userId", res.userId);

        try {
            await fetch(`https://localhost:7117/api/Cart/${res.userId}`, {
                method: "POST",
                headers: { "Authorization": `Bearer ${res.token}` }
            });
            console.log("Kosár inicializálva a felhasználónak.");
        } catch (error) {
            console.log("Kosár inicializálás sikertelen vagy már létezik:", error.message);
        }

        alert("Sikeres bejelentkezés!");
        window.location.href = "index.html";

    } catch (error) {
        alert("Hiba történt: " + error.message);
    }
});