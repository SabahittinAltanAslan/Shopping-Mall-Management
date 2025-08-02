const API_BASE = window.location.hostname.includes("localhost")
    ? "https://localhost:7237/api"
    : "https://iyasparkyonetim.com/api";

$(document).ready(function () {
    $("#loginForm").submit(async function (e) {
        e.preventDefault();

        const email = $("#email").val();
        const password = $("#password").val();

        try {
            const response = await fetch(`${API_BASE}/User/login`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ email, password })
            });

            if (!response.ok) {
                throw new Error("Giriş başarısız");
            }

            const data = await response.json();
            const token = data.token;
            const decoded = JSON.parse(atob(token.split('.')[1]));
            const role = decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

            localStorage.setItem("token", token);
            localStorage.setItem("role", role);

            Swal.fire({
                icon: "success",
                title: "Giriş başarılı",
                showConfirmButton: false,
                timer: 1500
            }).then(() => {
                window.location.href = "/views/dashboard.html";
            });

        } catch (error) {
            Swal.fire({
                icon: "error",
                title: "Hatalı giriş",
                text: "Lütfen bilgilerinizi kontrol ediniz."
            });
        }
    });
});
