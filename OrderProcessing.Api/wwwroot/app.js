let orders = [];
let selectedId = null;

const ordersList = document.getElementById("ordersList");
const orderDetails = document.getElementById("orderDetails");
const stateDiagram = document.getElementById("stateDiagram");
const historyTimeline = document.getElementById("historyTimeline");

const modal = document.getElementById("modal");
const newOrderButton = document.getElementById("newOrderButton");
const closeModalButton = document.getElementById("closeModalButton");
const createOrderForm = document.getElementById("createOrderForm");
const itemsContainer = document.getElementById("itemsContainer");
const addItemButton = document.getElementById("addItemButton");

addItemButton.addEventListener("click", () => {
    const div = document.createElement("div");
    div.className = "order-item";

    div.innerHTML = `
    <select class="productSelect">
        <option value="P001">Laptop</option>
        <option value="P002">Telefon</option>
        <option value="P003">Tableta</option>
        <option value="P999 Monitor">Monitor</option>
    </select>

    <input
        type="number"
        class="quantityInput"
        placeholder="Cantitate"
        value="1"
        min="1"
        required>

    <button
        type="button"
        class="removeItemButton">
        Sterge
    </button>
`;
    itemsContainer.appendChild(div);

    div
        .querySelector(".removeItemButton")
        .addEventListener("click", () => {
            div.remove();
        });
});

newOrderButton.addEventListener("click", () => {
    modal.classList.remove("hidden");
});

closeModalButton.addEventListener("click", () => {
    modal.classList.add("hidden");
});

createOrderForm.addEventListener("submit", async (event) => {
    event.preventDefault();

    const data = {
        customer: {
            id: crypto.randomUUID(),
            name: document.getElementById("customerName").value,
            email: document.getElementById("customerEmail").value,
            age: Number(document.getElementById("customerAge").value),
            isTrusted: document.getElementById("isTrusted").checked
        },
        shippingAddress: {
            street: "Str. Exemplu 12",
            city: "Bucuresti",
            postalCode: "010101",
            country: "Romania"
        },
        items: Array.from(
            document.querySelectorAll(".order-item"))
            .map(item => {

                const productId =
                    item.querySelector(".productSelect").value;

                const quantity =
                    Number(
                        item.querySelector(".quantityInput").value);

                return {
                    productId: productId,
                    productName: getProductName(productId),
                    quantity: quantity,

                    unitPrice: {
                        amount: getProductPrice(productId),
                        currency: "RON"
                    },

                    hasAgeRestriction: true
                };
            })
    };

    await createOrder(data);
});

async function fetchOrders() {
    const response = await fetch("/orders");
    orders = await response.json();

    renderOrders();
}

async function fetchOrder(id) {
    const response = await fetch(`/orders/${id}`);

    if (!response.ok) {
        showToast("Comanda nu a fost găsită", "error");
        return;
    }

    const order = await response.json();
    selectedId = order.id;

    renderDetails(order);
    renderStateDiagram(order.status);
    renderActions(order.status);
    renderHistory(order.history);
}

async function createOrder(data) {
    try {
        const response = await fetch("/orders", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(data)
        });

        const result = await response.json();

        if (!response.ok) {
            showToast(result.errors[0], "error");
            return;
        }

        modal.classList.add("hidden");
        createOrderForm.reset();

        showToast(`Comandă creată cu succes #${result.id}`, "success");

        await fetchOrders();
        await fetchOrder(result.id);
    } catch {
        showToast("Eroare la crearea comenzii", "error");
    }
}

async function triggerTransition(action) {
    if (!selectedId) {
        showToast("Selectează o comandă", "error");
        return;
    }

    try {
        const response = await fetch(`/orders/${selectedId}/${action}`, {
            method: "POST"
        });

        const result = await response.json();

        if (!response.ok) {
            showToast(result.error, "error");
            return;
        }

        showToast(`Tranziție realizată: ${result.status}`, "success");

        await fetchOrders();
        await fetchOrder(result.id);
    } catch {
        showToast("Eroare la tranziție", "error");
    }
}

function renderOrders() {
    ordersList.innerHTML = "";

    orders.forEach(order => {
        const card = document.createElement("div");
        card.className = "order-card";
        card.innerHTML = `
            <strong>#${order.id.substring(0, 8)}</strong>
            <span class="status-badge status-${order.status.toLowerCase()}">
                ${order.status}
            </span>
        `;

        card.addEventListener("click", () => fetchOrder(order.id));

        ordersList.appendChild(card);
    });
}

function renderDetails(order) {
    orderDetails.innerHTML = `
        <p><strong>ID:</strong> ${order.id}</p>
        <p><strong>Status:</strong> ${order.status}</p>
        <p><strong>Customer:</strong> ${order.customer.name} (${order.customer.age} ani)</p>
        <p><strong>Email:</strong> ${order.customer.email}</p>
        <p><strong>Address:</strong> ${order.address.street}, ${order.address.city}</p>
        <p><strong>Total:</strong> ${order.total.amount} ${order.total.currency}</p>
        <p><strong>Items:</strong></p>
        <ul>
            ${order.items.map(item => `
                <li>
                    ${item.productName} —
                    ${item.quantity} x
                    ${item.unitPrice.amount} ${item.unitPrice.currency}
                </li>
            `).join("")}
        </ul>
    `;
}

function renderStateDiagram(currentStatus) {
    const states = ["Pending", "Confirmed", "Processing", "Shipped", "Delivered", "Cancelled"];

    stateDiagram.innerHTML = states.map(state => {
        const activeClass = state === currentStatus ? "status-badge status-" + state.toLowerCase() : "status-badge";

        return `<span class="${activeClass}">${state}</span>`;
    }).join(" → ");
}

function renderActions(status) {
    setButton("payButton", status === "Pending", "pay");
    setButton("processButton", status === "Confirmed", "process");
    setButton("shipButton", status === "Processing", "ship");
    setButton("deliverButton", status === "Shipped", "deliver");
    setButton("cancelButton",
        status === "Pending" ||
        status === "Confirmed" ||
        status === "Processing",
        "cancel");
}

function setButton(id, enabled, action) {
    const button = document.getElementById(id);

    button.disabled = !enabled;
    button.style.opacity = enabled ? "1" : "0.4";

    button.onclick = enabled
        ? () => triggerTransition(action)
        : null;
}

function renderHistory(history) {
    historyTimeline.innerHTML = "";

    if (!history || history.length === 0) {
        historyTimeline.innerHTML = "<p>Nu există tranziții încă.</p>";
        return;
    }

    history.forEach(item => {
        const div = document.createElement("div");
        div.className = "history-item";

        div.innerHTML = `
            <strong>${item.fromState} → ${item.toState}</strong>
            <span>${new Date(item.at).toLocaleTimeString()}</span>
        `;

        historyTimeline.appendChild(div);
    });
}

function showToast(message, type) {
    const toast = document.createElement("div");

    toast.className = `toast toast-${type}`;
    toast.innerText = message;

    document.getElementById("toastContainer").appendChild(toast);

    setTimeout(() => {
        toast.remove();
    }, 4000);
}

function getProductName(productId) {
    if (productId === "P001") {
        return "Laptop";
    }

    if (productId === "P002") {
        return "Telefon";
    }

    if (productId === "P003") {
        return "Tableta";
    }

    return "Monitor";
} 

function getProductPrice(productId) {

    if (productId === "P001") {
        return 2500;
    }

    if (productId === "P002") {
        return 1800;
    }

    if (productId === "P003") {
        return 1200;
    }

    return 3000;
}
fetchOrders();