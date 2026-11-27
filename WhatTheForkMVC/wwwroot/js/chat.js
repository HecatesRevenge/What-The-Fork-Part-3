const chatWindow = document.getElementById("chatWindow");
const chatMessages = document.getElementById("chatMessages");
const chatInput = document.getElementById("chatInput");
const chatSend = document.getElementById("chatSend");

function escapeHtml(text) {
    const div = document.createElement("div");
    div.textContent = text;
    return div.innerHTML;
}

function linkRestaurants(text) {
    text = text.replace(/\*\*/g, "");
    return escapeHtml(text).replace(/\(#(\d+)\)/g, '(<a href="/Restaurant/Profile/$1">view</a>)');
}

function addBubble(text, role) {
    const bubble = document.createElement("div");
    bubble.className = "chat-bubble " + role;
    bubble.innerHTML = linkRestaurants(text);
    chatMessages.appendChild(bubble);
    chatMessages.scrollTop = chatMessages.scrollHeight;
    return bubble;
}

async function sendMessage() {
    const message = chatInput.value.trim();
    if (message === "") return;

    addBubble(message, "user");
    chatInput.value = "";
    chatSend.disabled = true;
    const replyBubble = addBubble("Thinking...", "assistant thinking");

    const form = new FormData();
    form.append("message", message);
    form.append("filters", document.getElementById("chatFilters").value);

    try {
        const response = await fetch("/Chat/Send", { method: "POST", body: form });
        const data = await response.json();
        replyBubble.innerHTML = linkRestaurants(data.reply);
    } catch {
        replyBubble.textContent = "Something went wrong. Please try again.";
    }
    replyBubble.classList.remove("thinking");
    chatSend.disabled = false;
    chatMessages.scrollTop = chatMessages.scrollHeight;
}

chatSend.addEventListener("click", sendMessage);
chatInput.addEventListener("keydown", function (e) {
    if (e.key === "Enter") sendMessage();
});

document.getElementById("chatToggle").addEventListener("click", function () {
    chatWindow.classList.toggle("chat-hidden");
    chatMessages.scrollTop = chatMessages.scrollHeight;
});

document.getElementById("chatClear").addEventListener("click", async function () {
    await fetch("/Chat/Clear", { method: "POST" });
    chatMessages.innerHTML = "";
    addBubble("Starting fresh. What are you in the mood for?", "assistant");
});

document.querySelectorAll("#chatMessages .chat-bubble.assistant").forEach(function (bubble) {
    bubble.innerHTML = linkRestaurants(bubble.textContent);
});