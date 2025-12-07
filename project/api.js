const API_BASE = "https://localhost:7117/api";

/**
 * @param {Object} param0
 * @param {string} param0.email
 * @param {string} param0.password
 * @returns {Promise<LoginResponseDTO>}
 */
export async function loginUser({ email, password }) {
    const res = await fetch(`${API_BASE}/User/login`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password })
    });
    if (!res.ok) {
        const err = await res.json();
        throw new Error(err.message || "Hiba a bejelentkezés során!");
    }
    return res.json();
}

/**
 * @param {Object} userData
 * @returns {Promise<UserDTO>}
 */

export async function registerUser(userData) {
    const res = await fetch(`${API_BASE}/User/register`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(userData)
    });

    console.log(userData);

    if (!res.ok) {
        const err = await res.json();
        throw new Error(err.message || "Hiba a regisztráció során!");
    }
    return res.json();
}


/**
 * @param {string} email
 * @returns {Promise<UserDTO|null>}
 */
export async function checkEmail(email) {
    const res = await fetch(`${API_BASE}/User/by-email?email=${encodeURIComponent(email)}`)
    if (res.status === 404) return null;
    if (!res.ok) throw new Error("Hiba az email ellenőrzésekor!");

    const text = await res.text();  
    if (!text) return null;          

    const data = JSON.parse(text);   
    if (data.userId === 0) return null;

    return data;
}

/**
 * @returns {Promise<ProductDTO[]>}
 */

export async function fetchProducts() {
    const res = await fetch(`${API_BASE}/Product`,{
        method: "GET",
        headers: {"Content-Type": "application/json"}
    });
    if(!res.ok){
        const err = await res.json();
        throw new Error(err.message || "Hiba a termékek lekérése során!");
    }
    return res.json();
}

export async function fetchCart(userId, token) {
    if (!token) throw new Error("Nem található token a kosár betöltéséhez!");

    const res = await fetch(`${API_BASE}/Cart/${userId}`, {
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`
        }
    });

    if (res.status === 404) {
        return { items: [] };
    }

    if (!res.ok) {
        const errorBody = await res.text();
        console.error("Backend hiba:", errorBody);
        throw new Error("Nem sikerült betölteni a kosarat.");
    }

    return await res.json();
}

export async function addCartItem(userId, productId, quantity = 1, token) {
    if (!token) throw new Error("Nem található token, jelentkezz be!");

    const res = await fetch(`${API_BASE}/Cart?userId=${userId}`, {
        method: "POST",
        headers: { 
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`
        },
        body: JSON.stringify({ productId, quantity })
    });

    if (!res.ok) {
        const errorBody = await res.text();
        console.error("Backend hiba:", errorBody);
        throw new Error("Nem sikerült a kosárba helyezni a terméket.");
    }

    return await res.json();
}

export async function updateCartItem(userId, productId, quantity, token) {
    if (!token) throw new Error("Nem található token a kosár frissítéséhez!");

    const res = await fetch(`${API_BASE}/Cart/${userId}/items`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`
        },
        body: JSON.stringify({ productId, quantity })
    });

    if (!res.ok) {
        const errorBody = await res.text();
        console.error("Backend hiba:", errorBody);
        throw new Error("Nem sikerült frissíteni a kosár elemet.");
    }

    return await res.json();
}

export async function removeCartItem(userId, productId, token) {
    if (!token) throw new Error("Nem található token a kosár törléséhez!");

    const res = await fetch(`${API_BASE}/Cart/${userId}/items/${productId}`, {
        method: "DELETE",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`
        }
    });

    if (!res.ok) {
        const errorBody = await res.text();
        console.error("Backend hiba:", errorBody);
        throw new Error("Nem sikerült törölni a kosár elemet.");
    }

    return await res.json();
}

export async function getUserProfile(userId, token) {
    const res = await fetch(`${API_BASE}/User/${userId}`, {
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`
        }
    });

    if (!res.ok) {
        throw new Error("Hiba a profiladatok lekérésekor!");
    }

    return res.json();
}

export async function updateUserProfile(userId, token, data) {
    const formatted = {
        user: {
            FirstName: data.firstName,
            LastName: data.lastName,
            City: data.city,
            PostalCode: data.postalCode,
            PhoneNumber: data.phoneNumber,
            AddressDetails: data.addressDetails,
            AcceptedTerms: data.acceptedTerms
        }
    };

     const res = await fetch(`${API_BASE}/User/${userId}`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`
        },
        body: JSON.stringify(data)
    });

    if (!res.ok) {
        const txt = await res.text();
        throw new Error(txt);
    }

    const text = await res.text();
    return text ? JSON.parse(text) : true;
}

export async function clearCart(userId, token) {
    if (!token) throw new Error("Nem található token a kosár törléséhez!");

    const res = await fetch(`${API_BASE}/Cart/${userId}`, {
        method: "DELETE",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`
        }
    });

    if (!res.ok) {
        const errorBody = await res.text();
        throw new Error(errorBody || "Nem sikerült törölni a kosarat.");
    }

    return await res.json();
}

export async function placeOrder(userId, orderData, token) {
    if (!token) throw new Error("Nem található token a rendelés leadásához!");

    const res = await fetch(`${API_BASE}/Order/${userId}`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify(orderData)
        });

    if (!res.ok) {
        const errorBody = await res.text();
        throw new Error(errorBody || "Hiba a rendelés leadása során!");
    }

    return await res.json();
}