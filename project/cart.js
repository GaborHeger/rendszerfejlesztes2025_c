import { fetchCart, updateCartItem, removeCartItem } from './api.js';

window.addEventListener("DOMContentLoaded", loadCartFromAPI);

async function loadCartFromAPI() {
    const userId = Number(localStorage.getItem("userId"));
    const token = localStorage.getItem("token");
    if (!userId || !token) return;

    try {
        const cart = await fetchCart(userId, token);
        renderCart(cart);
    } catch (err) {
        console.error("Kosár betöltési hiba:", err);
        document.querySelector('.products').innerHTML = "<p>Nem sikerült betölteni a kosarat.</p>";
    }
}

function renderCart(cart) {
    const productsDiv = document.querySelector('.products');
    productsDiv.innerHTML = "";

    if (!cart.items || cart.items.length === 0) {
        document.getElementById("money").textContent = "0 Ft";
        return;
    }

    cart.items.forEach(product => {
        const productHTML = `
            <div class="product" data-productid="${product.productId}">
                <div class="pic">
                    <img src="${product.imageUrl}" alt="${product.productName}" class="p">
                </div>
                <div>
                    <div class="prodname">${product.productName}</div>
                    <div class="prodprize">${product.price} Ft</div>
                    <div class="numDiv">
                        <input type="number" class="num" value="${product.quantity}" min="1" max="${product.stock}">
                    </div>
                </div>
                <div class="remove" data-productid="${product.productId}">
                    <img src="cancel.png" class="cancel" alt="remove from cart">
                </div>
            </div>
        `;
        productsDiv.innerHTML += productHTML;
    });

    updateSum(cart);
}

function updateSum(cart) {
    const total = cart.items.reduce((sum, item) => sum + item.price * item.quantity, 0);
    document.getElementById("money").textContent = total + " Ft";
}


document.addEventListener("input", async (event) => {
    if (event.target.classList.contains("num")) {
        const userId = Number(localStorage.getItem("userId"));
        const token = localStorage.getItem("token");
        if (!userId || !token) {
            alert("Jelentkezz be a kosár módosításához!");
            return;
        }

        const productDiv = event.target.closest(".product");
        const productId = Number(productDiv.dataset.productid);
        const quantity = Math.max(1, Number(event.target.value));

        try {
            const updatedCart = await updateCartItem(userId, productId, quantity, token);
            renderCart(updatedCart);
        } catch (err) {
            console.error(err);
            alert("Nem sikerült frissíteni a mennyiséget!");
        }
    }
});

document.addEventListener("click", async (event) => {
    if (event.target.classList.contains("cancel")) {
        const userId = Number(localStorage.getItem("userId"));
        const token = localStorage.getItem("token"); 
        if (!userId || !token) {
            alert("Jelentkezz be a kosár módosításához!");
            return;
        }

        const productDiv = event.target.closest(".remove");
        const productId = Number(productDiv.dataset.productid);

        try {
            const updatedCart = await removeCartItem(userId, productId, token);
            renderCart(updatedCart);
        } catch (error) {
            console.error(error);
            alert("Nem sikerült törölni a terméket!");
        }
    }
});

window.addEventListener("DOMContentLoaded", async () => {
    const token = localStorage.getItem("token");
    const userId = Number(localStorage.getItem("userId"));
    const orderBtn = document.getElementById("orderb");

    let disableOrder = false;

    if (!token || !userId) {
        disableOrder = true;
        orderBtn.title = "Jelentkezz be a rendelés leadásához";
    } else {
        try {
            const cart = await fetchCart(userId, token);
            if (!cart.items || cart.items.length === 0) {
                disableOrder = true;
                orderBtn.title = "A kosár üres, nem lehet rendelést leadni";
            }
        } catch (err) {
            console.error("Kosár ellenőrzés hiba:", err);
            disableOrder = true;
            orderBtn.title = "Hiba a kosár betöltésekor, nem lehet rendelést leadni";
        }
    }

    if (disableOrder) {
        orderBtn.disabled = true;
        orderBtn.style.cursor = "not-allowed";

        const linkParent = orderBtn.closest("a");
        if (linkParent) {
            linkParent.addEventListener("click", (e) => {
                e.preventDefault();
                alert(orderBtn.title);
            });
        }
    }
});