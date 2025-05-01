async function register() {
    const username = document.getElementById("username").value.trim();
    const password = document.getElementById("password").value.trim();
    const name = document.getElementById("name").value.trim();
    const surname = document.getElementById("surname").value.trim();

    if (!username || !password || !name || !surname) {
        alert("Please fill out all fields.");
        return;
    }

    const response = await fetch("https://localhost:7298/api/Auth/register", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            userName: username,
            password: password,
            name: name,
            surname: surname
        })
    });

    const result = await response.json();

    if (response.ok) {
        alert("Registration successful! Redirecting to login...");
        window.location.href = "/pages/index.html";
    } else {
        alert(result.message || "Registration failed.");
    }
}
