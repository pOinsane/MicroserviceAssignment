window.onload = () => {
    fetch('/api/users/me', {
        method: 'GET',
        credentials: 'include' // Ensures cookie is sent
    })
        .then(response => {
            if (!response.ok) throw new Error("Unauthorized");
            return response.json();
        })
        .then(user => {
            document.getElementById('welcomeMessage').innerText = `Hello, ${user.userName}!`;
        })
        .catch(() => {
            window.location.href = '/index.html'; // Redirect to login if not authenticated
        });
};

function logout() {
    document.cookie = "jwt=; Max-Age=0; path=/;";
    window.location.href = '/index.html';
}
