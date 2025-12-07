const pen1 = window.document.getElementById("pen1");
const pen2 = window.document.getElementById("pen2");
const pen3 = window.document.getElementById("pen3");
const pen4 = window.document.getElementById("pen4");
const tel = window.document.getElementById("tel");
const postnum =window.document.getElementById("postnum");
const city = window.document.getElementById("city");
const address = window.document.getElementById("address");

pen1.addEventListener("click", function(){
    tel.removeAttribute("readonly");
})

pen2.addEventListener("click", function(){
    postnum.removeAttribute("readonly");
})

pen3.addEventListener("click", function(){
    city.removeAttribute("readonly");
})

pen4.addEventListener("click", function(){
    address.removeAttribute("readonly");
})

const l = window.document.getElementById("l");

l.addEventListener("click", function(){
    localStorage.removeItem("token");
    localStorage.removeItem("user");

    window.location.href = "index.html";
})

import { getUserProfile, updateUserProfile } from './api.js';

window.addEventListener("DOMContentLoaded", async () => {
    const token = localStorage.getItem("token");
    const userData = JSON.parse(localStorage.getItem("user"));

    const saveBtn = document.querySelector(".adatok button");

    if (!token || !userData) {
        alert("Nincs bejelentkezve!");
        window.location.href = "login.html";
        return;
    }

    try {
        const user = await getUserProfile(userData.userId, token);
        console.log("User object:", user);

        document.getElementById("name").textContent = `Üdvözöllek ${user.firstName || ""}!`;
        document.getElementById("tel").value = user.phoneNumber || "";      
        document.getElementById("postnum").value = user.postalCode || "";  
        document.getElementById("city").value = user.city || "";  
        document.getElementById("address").value = user.addressDetails || ""; 

    } catch (error) {
        console.error(error);
        alert("Hiba a profiladatok betöltésekor!");
    }

    saveBtn.addEventListener("click", async () => {

        const token = localStorage.getItem("token");
        const user = await getUserProfile(userData.userId, token);

        const updatedUser = {
            id: user.userId,
            firstName: user.firstName,
            lastName: user.lastName,
            phoneNumber: tel.value,
            postalCode: postnum.value,
            city: city.value,
            addressDetails: address.value,
            subscribedToNewsletter: false,
            acceptedTerms: true
        };

        try {
            await updateUserProfile(userData.userId, token, updatedUser);
            alert("Adatok frissítve!");
        } catch (error) {
            console.error(error);
            alert("Hiba mentéskor: " + error);
        }
    });

});



