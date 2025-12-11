function loadCart() {
    try {
        const cart = localStorage.getItem("cart");
        return cart ? JSON.parse(cart) : [];
    } catch {
        return [];
    }
}

function formatPrice(n) {
    return "$" + Number(n).toFixed(2);
}

function renderSummary() {
    const itemsList = document.getElementById("itemsList");
    const totalPriceEl = document.getElementById("totalPrice");

    const cart = loadCart();
    itemsList.innerHTML = "";

    if (cart.length === 0) {
        itemsList.innerHTML = "<p>Your cart is empty.</p>";
        totalPriceEl.textContent = formatPrice(0);
        return;
    }

    let total = 0;
    cart.forEach(item => {
        const itemTotal = item.price * item.qty;
        total += itemTotal;

        const row = document.createElement("div");
        row.className = "item-row";
        row.innerHTML = `
            <img src="${item.image || 'https://via.placeholder.com/56'}">
            <div style="flex:1">
                <div><b>${item.title}</b></div>
                <div style="font-size:13px;color:#666">Qty: ${item.qty}</div>
            </div>
            <div>${formatPrice(itemTotal)}</div>
        `;
        itemsList.appendChild(row);
    });

    totalPriceEl.textContent = formatPrice(total);
}

document.addEventListener("DOMContentLoaded", () => {
    renderSummary();

    const form = document.getElementById("checkoutForm");
    form.addEventListener("submit", async (e) => {
        e.preventDefault();

        const name = document.getElementById("name").value;
        const address = document.getElementById("address").value;
        const phone = document.getElementById("phone").value;

        const cart = loadCart();
        if (cart.length === 0) {
            alert("Your cart is empty!");
            return;
        }

        const payload = {
            FullName: name,
            Address: address,
            Phone: phone,
            Items: cart,
            Total: cart.reduce((s, i) => s + i.price * i.qty, 0)
        };

        try {
            const res = await fetch("/Orders/PlaceOrder", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(payload)
            });

            if (res.ok) {
                alert("Order placed successfully!");
                localStorage.removeItem("cart");
                window.location.href = "/Orders/Checkout"; // ممكن تغيري للـ History page
            } else {
                alert("Failed to place order.");
            }
        } catch (err) {
            console.error(err);
            alert("Error placing order.");
        }
    });
});
