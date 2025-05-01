const apiBase = "https://localhost:7298/api";

// Get JWT from cookies
function getCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
}

// Unified fetch helper that adds Authorization header
const fetchWithAuth = (url, options = {}) => {
    const jwt = localStorage.getItem("jwt");

    return fetch(url, {
        ...options,
        credentials: "include", // optional now
        headers: {
            ...options.headers,
            Authorization: `Bearer ${jwt}`
        }
    });
};



document.addEventListener("DOMContentLoaded", () => {
    loadRoles();
    loadSkills();
    loadUsers();

    document.getElementById("role-form").addEventListener("submit", createRole);
    document.getElementById("skill-form").addEventListener("submit", createSkill);
    document.getElementById("user-form").addEventListener("submit", createUser);
});

function createRole(e) {
    e.preventDefault();
    const roleName = document.getElementById("role-name").value;
    fetchWithAuth(`${apiBase}/Roles`, {
        method: "POST",
        body: JSON.stringify({ roleName })
    }).then(() => {
        document.getElementById("role-name").value = "";
        loadRoles();
    });
}

function createSkill(e) {
    e.preventDefault();
    const skillName = document.getElementById("skill-name").value;
    fetchWithAuth(`${apiBase}/Skills`, {
        method: "POST",
        body: JSON.stringify({ skillName })
    }).then(() => {
        document.getElementById("skill-name").value = "";
        loadSkills();
    });
}

function createUser(e) {
    e.preventDefault();
    const username = document.getElementById("username").value;
    const password = document.getElementById("password").value;
    const roleId = document.getElementById("role-select").value;
    const skillCheckboxes = document.querySelectorAll("#skill-checkboxes input:checked");
    const skillIds = Array.from(skillCheckboxes).map(cb => parseInt(cb.value));

    fetchWithAuth(`${apiBase}/Users`, {
        method: "POST",
        body: JSON.stringify({ userName: username, password, roleId, skillIds })
    }).then(() => {
        document.getElementById("user-form").reset();
        loadUsers();
    });
}

function loadRoles() {
    fetchWithAuth(`${apiBase}/Roles`)
        .then(res => res.json())
        .then(roles => {
            const roleSelect = document.getElementById("role-select");
            roleSelect.innerHTML = `<option value="">Select Role</option>`;
            const tableBody = document.querySelector("#roles-table tbody");
            tableBody.innerHTML = "";

            roles.forEach(role => {
                roleSelect.innerHTML += `<option value="${role.id}">${role.roleName}</option>`;
                tableBody.innerHTML += `
                    <tr><td>${role.id}</td><td>${role.roleName}</td><td><!-- actions here --></td></tr>
                `;
            });
        });
}

function loadSkills() {
    fetchWithAuth(`${apiBase}/Skills`)
        .then(res => res.json())
        .then(skills => {
            const checkboxes = document.getElementById("skill-checkboxes");
            const tableBody = document.querySelector("#skills-table tbody");
            checkboxes.innerHTML = "";
            tableBody.innerHTML = "";

            skills.forEach(skill => {
                checkboxes.innerHTML += `
                    <label><input type="checkbox" value="${skill.id}"> ${skill.skillName}</label>
                `;
                tableBody.innerHTML += `
                    <tr><td>${skill.id}</td><td>${skill.skillName}</td><td><!-- actions --></td></tr>
                `;
            });
        });
}

function loadUsers() {
    fetchWithAuth(`${apiBase}/Users`)
        .then(res => res.json())
        .then(users => {
            const tableBody = document.querySelector("#users-table tbody");
            tableBody.innerHTML = "";

            users.forEach(user => {
                const skills = (user.skillIds || []).join(", ");
                tableBody.innerHTML += `
                    <tr>
                        <td>${user.id}</td>
                        <td>${user.userName}</td>
                        <td>${user.roleId}</td>
                        <td>${skills}</td>
                        <td><!-- actions here --></td>
                    </tr>
                `;
            });
        });
}
