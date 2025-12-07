window.addEventListener("DOMContentLoaded", () => {
    const profileLink = document.getElementById("profileLink");
    const token = localStorage.getItem("token");

    if (token) {
        profileLink.href = "profadatok.html";
    } else {
        profileLink.href = "profile.html";
    }
});

import { fetchProducts, addCartItem } from './api.js'; 

const menu = window.document.getElementById("menu");
const leftside = window.document.getElementById("leftside");

menu.addEventListener("click", function(){
    leftside.classList.toggle("leftside");
})

const babak = window.document.getElementById("babak");
const kiegeszitok = window.document.getElementById("kiegeszitok");
const osszes = window.document.getElementById("osszes");

const contentsDiv = document.querySelector('.contents');

babak.addEventListener("click", () => {
    filterProducts("Barbie");
});

kiegeszitok.addEventListener("click", () => {
    filterProducts("Accessory");
});

osszes.addEventListener("click", () => {
    filterProducts(); 
});

/**
 * @param {string} categoryName 
 */
function filterProducts(categoryName) {
    const allProducts = contentsDiv.querySelectorAll("div.Barbie, div.Accessory");
    allProducts.forEach(product => {
        if (!categoryName) {
            product.style.display = "flex";
        } else if (product.classList.contains(categoryName)) {
            product.style.display = "flex"; 
        } else {
            product.style.display = "none"; 
        }
    });
}

window.addEventListener("DOMContentLoaded", renderProducts);

const categoryMap = {
    0: "Barbie",
    1: "Accessory"
};

async function renderProducts() {
    const contentsDiv = document.querySelector('.contents');
    contentsDiv.innerHTML = "";

    try {
        const products = await fetchProducts();

        products.forEach(product => {
            const categoryName = categoryMap[product.category];
        const productHTML = `
            <div class="${categoryName}" id="${product.id}">
                <div class="pic">
                    <img src="${product.imageUrl}" alt="${product.productName}" id="ph">
                </div>
                <div>
                    <div class="prodname">${product.productName}</div>
                    <div class="prodprize">${product.price} Ft</div>
                </div>
                <div class="cart">
                    <img src="cart.png" class="putcart" data-id="${product.productId}" alt="shopping cart">
                </div>
            </div>
        `;
            contentsDiv.innerHTML += productHTML;
        });

    } catch (err) {
        contentsDiv.innerHTML = `<p style="color:red;">Hiba történt: ${err.message}</p>`;
    }
}

document.addEventListener("click", async (event) => {
    if (event.target.classList.contains("putcart")) {
        const userId = Number(localStorage.getItem("userId"));
        const token = localStorage.getItem("token");

        if (!token || !userId) {
            alert("Jelentkezz be, ha kosárba akarsz tenni terméket!");
            return;
        }

        const productId = Number(event.target.dataset.id);

        try {
            await addCartItem(userId, productId, 1, token);
            alert("Kosárhoz adva!");
        } catch (err) {
            console.error("Hiba a kosárba tétel során:", err);
            alert("Hiba történt a kosárba tétel során!");
        }
    }
});
