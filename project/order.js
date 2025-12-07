var c1 = window.document.getElementById("c1");
var c2 = window.document.getElementById("c2");
var c3 = window.document.getElementById("c3");
var c4 = window.document.getElementById("c4");

c1.addEventListener("change", function(){
    c2.checked = false;
})
c2.addEventListener("change", function(){
    c1.checked = false;
})

c3.addEventListener("change", function(){
    c4.checked = false;
})
c4.addEventListener("change", function(){
    c3.checked = false;
})

var sub = window.document.getElementById("sub");

import { placeOrder, getUserProfile, fetchCart, clearCart } from "./api.js";

window.addEventListener("DOMContentLoaded", async () => {
    const token = localStorage.getItem("token");
    const userData = JSON.parse(localStorage.getItem("user"));

    if (!token || !userData) {
        console.log("Nincs bejelentkezve.");
        return;
    }

    try {
        const userData = JSON.parse(localStorage.getItem("user"));
        if (!userData) return;

        const user = await getUserProfile(userData.userId, token);

        document.getElementById("email").value = user.email || "";
        document.getElementById("lastName").value = user.lastName || "";
        document.getElementById("firstName").value = user.firstName || "";
        document.getElementById("tel").value = user.phoneNumber || "";
        document.getElementById("postnum").value = user.postalCode || "";
        document.getElementById("city").value = user.city || "";
        document.getElementById("address").value = user.addressDetails || "";

    } catch (error) {
        console.error("Hiba a rendelési adatok betöltésekor:", error);
    }
});

sub.addEventListener("click", async (event) => {
    event.preventDefault();

    const userId = Number(localStorage.getItem("userId"));
    const token = localStorage.getItem("token");

    if (!userId || !token) {
        alert("Jelentkezz be a rendelés leadásához!");
        return;
    }

    const email = document.getElementById("email").value.trim();
    if (!email) {
        alert("Minden adatot adj meg!");
        return;
    }

    const paymentMethod = c1.checked ? 0 : c2.checked ? 1 : null;
    const shippingMethod = c3.checked ? 0 : c4.checked ? 1 : null;

    if (paymentMethod === null || shippingMethod === null) {
        alert("Válassz fizetési és szállítási módot!");
        return;
    }

    try {
        const cart = await fetchCart(userId, token);

        if (!cart.items || cart.items.length === 0) {
            alert("A kosár üres! Nem lehet rendelést leadni.");
            renderCart({ items: [] });
            return;
        }

        await placeOrder(userId, { shippingMethod, paymentMethod }, token);
        alert("Rendelés sikeres!");
        window.location.href = "index.html";

        try {
            await clearCart(userId, token);
        } catch (err) {
            if (err.message.includes("404")) {
                console.log("Kosár már üres, nincs mit törölni.");
            } else {
                console.error("Kosár törlése sikertelen:", err.message);
            }
        }

    } catch (err) {
        console.error("Hiba a rendelés leadásakor:", err);
        alert("Hiba történt a rendelés leadása során!");
    }
});
