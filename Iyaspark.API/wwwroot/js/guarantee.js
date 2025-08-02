const API_BASE = window.location.hostname.includes("localhost")
    ? "https://localhost:7237/api"
    : "https://iyasparkyonetim.com/api";

const token = localStorage.getItem("token");
if (!token) {
    Swal.fire("Oturum süresi doldu!", "Yeniden giriş yapınız.", "warning").then(() => {
        window.location.href = "/login.html";
    });
}

const typeOptions = [
    "Nakit", "Çek", "Senet", "Banka Teminat Mektubu",
    "İpotek", "Akreditif", "Garanti Bonosu", "Altın"
];

const currencyOptions = ["TRY", "USD", "EUR", "GBP"];
const currencySymbols = {
    0: "₺",
    1: "$",
    2: "€",
    3: "£"
};

function populateDropdown(id, options) {
    const select = document.getElementById(id);
    select.innerHTML = "<option value=''>Seçiniz</option>";
    options.forEach((opt, index) => {
        select.innerHTML += `<option value="${index}">${opt}</option>`;
    });
}

$(document).ready(() => {
    populateDropdown("type", typeOptions);
    populateDropdown("currencyType", currencyOptions);
    fetchGuarantees();
    fetchTenants();

    $("#guaranteeForm").on("submit", function (e) {
        e.preventDefault();
        saveGuarantee();
    });
});

function fetchGuarantees() {
    fetch(`${API_BASE}/Guarantee`, {
        headers: {
            Authorization: "Bearer " + token
        }
    })
        .then(res => res.json())
        .then(data => {
            const tbody = $("#guaranteeTable tbody");
            tbody.empty();
            data.forEach(item => {
                const row = `
                    <tr>
                        <td>${item.companyName || "-"}</td>
                        <td>${typeOptions[item.type]}</td>
                        <td>${item.amount.toLocaleString("tr-TR")} ${currencySymbols[item.currencyType]}</td>
                        <td>${currencyOptions[item.currencyType]}</td>
                        <td>${item.receivedDate?.split("T")[0]}</td>
                        <td>${item.isReturned ? "Evet" : "Hayır"}</td>
                        <td>${item.description || "-"}</td>
                        <td>
                            <button class="btn btn-sm btn-warning" onclick='editGuarantee(${JSON.stringify(item)})'>Güncelle</button>
                            <button class="btn btn-sm btn-danger" onclick="deleteGuarantee('${item.id}')">Sil</button>
                        </td>
                    </tr>
                `;
                tbody.append(row);
            });
        });
}

function fetchTenants() {
    fetch(`${API_BASE}/Tenant`, {
        headers: {
            Authorization: "Bearer " + token
        }
    })
        .then(res => res.json())
        .then(data => {
            const select = $("#tenantId");
            select.empty();
            data.forEach(t => {
                select.append(`<option value="${t.id}">${t.companyName}</option>`);
            });
        });
}

function openCreateModal() {
    $("#guaranteeForm")[0].reset();
    $("#guaranteeId").val("");
    $("#modalTitle").text("Yeni Teminat");
    $("#guaranteeModal").modal("show");
}

function editGuarantee(item) {
    $("#guaranteeId").val(item.id);
    $("#tenantId").val(item.tenantId);
    $("#type").val(item.type);
    $("#amount").val(item.amount);
    $("#currencyType").val(item.currencyType);
    $("#receivedDate").val(item.receivedDate?.split("T")[0]);
    $("#description").val(item.description);
    $("#isReturned").prop("checked", item.isReturned);
    $("#modalTitle").text("Teminat Güncelle");
    $("#guaranteeModal").modal("show");
}

function saveGuarantee() {
    const id = $("#guaranteeId").val();
    const dto = {
        tenantId: $("#tenantId").val(),
        type: parseInt($("#type").val()),
        amount: parseFloat($("#amount").val()),
        currencyType: parseInt($("#currencyType").val()),
        receivedDate: $("#receivedDate").val(),
        isReturned: $("#isReturned").is(":checked"),
        description: $("#description").val()
    };

    const method = id ? "PUT" : "POST";
    const url = id ? `${API_BASE}/Guarantee/${id}` : `${API_BASE}/Guarantee`;

    fetch(url, {
        method: method,
        headers: {
            "Content-Type": "application/json",
            Authorization: "Bearer " + token
        },
        body: JSON.stringify(dto)
    })
        .then(res => {
            if (!res.ok) throw new Error("İşlem başarısız");
            return res.status === 204 ? null : res.json();
        })
        .then(() => {
            $("#guaranteeModal").modal("hide");
            fetchGuarantees();
            Swal.fire("Başarılı", "İşlem tamamlandı", "success");
        })
        .catch(() => {
            Swal.fire("Hata", "Kayıt sırasında hata oluştu", "error");
        });
}

function deleteGuarantee(id) {
    Swal.fire({
        title: "Silmek istediğinize emin misiniz?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Evet, sil",
        cancelButtonText: "Vazgeç"
    }).then(result => {
        if (result.isConfirmed) {
            fetch(`${API_BASE}/Guarantee/${id}`, {
                method: "DELETE",
                headers: {
                    Authorization: "Bearer " + token
                }
            })
                .then(() => {
                    fetchGuarantees();
                    Swal.fire("Silindi", "Teminat silindi", "success");
                })
                .catch(() => {
                    Swal.fire("Hata", "Silme işlemi başarısız", "error");
                });
        }
    });
}
