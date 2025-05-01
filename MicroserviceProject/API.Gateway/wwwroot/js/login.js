async function login() {
    const username = document.getElementById("username").value.trim();
    const password = document.getElementById("password").value.trim();

    if (!username || !password) {
        alert("Please enter both username and password.");
        return;
    }

    const response = await fetch("https://localhost:7298/api/Auth/login", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        credentials: "include",
        body: JSON.stringify({ userName: username, password: password })
    });

    const result = await response.json();

    if (response.ok) {
        console.log(result);
        const token = result.token;
        console.log("Received token:", token);

        localStorage.setItem("jwt", token);
        alert("Login successful!");
        window.location.href = "/pages/dashboard.html";
    } else {
        alert(result.message || "Login failed");
    }
}
