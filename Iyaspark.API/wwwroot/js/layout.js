$(document).ready(function () {
    const token = localStorage.getItem("token");

    if (!token) {
        window.location.href = "/login.html";
        return;
    }

    $("#logoutBtn").click(function () {
        Swal.fire({
            title: "Çıkış yapmak istiyor musunuz?",
            icon: "question",
            showCancelButton: true,
            confirmButtonText: "Evet",
            cancelButtonText: "Hayır"
        }).then(result => {
            if (result.isConfirmed) {
                localStorage.removeItem("token");
                window.location.href = "/login.html";
            }
        });
    });
});
