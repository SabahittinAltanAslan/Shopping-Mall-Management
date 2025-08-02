const API_BASE = window.location.hostname.includes("localhost")
    ? "https://localhost:7237/api/MonthlyRevenue"
    : "https://iyasparkyonetim.com/api/MonthlyRevenue";

const token = localStorage.getItem("token");
if (!token) {
    Swal.fire("Giriş yapınız!", "", "warning").then(() => {
        window.location.href = "/login.html";
    });
}

const CurrencyEnum = { 1: "TL", 2: "USD", 3: "EUR" };

window.loadTenants = function () {
    const tenantApi = window.location.hostname.includes("localhost")
        ? "https://localhost:7237/api/Tenant"
        : "https://iyasparkyonetim.com/api/Tenant";

    fetch(tenantApi, {
        headers: { 'Authorization': 'Bearer ' + token }
    })
        .then(res => res.json())
        .then(data => {
            $('#tenantId').empty();
            data.forEach(t => {
                $('#tenantId').append(`<option value="${t.id}">${t.companyName} (${t.taxNumber})</option>`);
            });
        });
};

window.loadRevenues = function () {
    fetch(API_BASE, {
        headers: { 'Authorization': 'Bearer ' + token }
    })
        .then(res => res.json())
        .then(data => {
            const table = $('#revenueTable').DataTable();
            table.clear().destroy();
            $('#revenueTable tbody').empty();

            data.forEach(item => {
                const dateFormatted = `${item.month.toString().padStart(2, '0')}/${item.year}`;
                const row = `
                    <tr>
                        <td>${item.companyName}</td>
                        <td>${item.taxNumber}</td>
                        <td>${dateFormatted}</td>
                        <td>${item.revenueAmount}</td>
                        <td>${CurrencyEnum[item.currencyType]}</td>
                        <td>
                            <button class="btn btn-sm btn-primary me-1" onclick='editRevenue(${JSON.stringify(item)})'>Güncelle</button>
                            <button class="btn btn-sm btn-danger" onclick='deleteRevenue("${item.id}")'>Sil</button>
                        </td>
                    </tr>
                `;
                $('#revenueTable tbody').append(row);
            });

            $('#revenueTable').DataTable();
        });
};

$(document).ready(() => {
    $('#openCreateModal').click(() => {
        clearForm();
        $('#revenueModal').modal('show');
    });

    $('#revenueForm').submit(async function (e) {
        e.preventDefault();

        const [year, month] = $('#revenueDate').val().split("-");
        const isUpdate = !!$('#revenueId').val();
        const url = isUpdate ? `${API_BASE}/${$('#revenueId').val()}` : API_BASE;
        const method = isUpdate ? 'PUT' : 'POST';

        const dto = {
            tenantId: $('#tenantId').val(),
            year: parseInt(year),
            month: parseInt(month),
            revenueAmount: parseFloat($('#revenueAmount').val().replace(',', '.')),
            currencyType: parseInt($('#currencyType').val())
        };

        const body = JSON.stringify(dto);

        try {
            const res = await fetch(url, {
                method,
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + token
                },
                body: body
            });

            const responseText = await res.text();

            if (res.ok) {
                $('#revenueModal').modal('hide');
                loadRevenues();
                Swal.fire("Başarılı", isUpdate ? "Ciro güncellendi." : "Ciro eklendi.", "success");
            } else {
                try {
                    const json = JSON.parse(responseText);
                    if (json.errors && Array.isArray(json.errors)) {
                        Swal.fire("Hata", json.errors.join("<br>"), "error");
                    } else if (responseText.includes("Aynı")) {
                        Swal.fire("Uyarı", "Aynı kiracı için aynı ayda ciro girişi yapılamaz.", "warning");
                    } else {
                        Swal.fire("Hata", json.error || "İşlem sırasında beklenmeyen bir hata oluştu.", "error");
                    }
                } catch (err) {
                    Swal.fire("Hata", "Sunucu hatası oluştu.", "error");
                    console.error("Parse Hatası:", responseText);
                }
            }
        } catch (err) {
            Swal.fire("Hata", "Sunucuya ulaşılamadı.", "error");
            console.error("Network Hatası:", err);
        }
    });
});

function editRevenue(item) {
    $('#revenueId').val(item.id);
    $('#tenantId').val(item.tenantId);
    $('#revenueDate').val(`${item.year}-${item.month.toString().padStart(2, '0')}`);
    $('#revenueAmount').val(item.revenueAmount);
    $('#currencyType').val(item.currencyType);
    $('#revenueModal').modal('show');
}

function deleteRevenue(id) {
    Swal.fire({
        title: 'Emin misiniz?',
        text: "Bu işlem geri alınamaz!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Evet, sil!',
        cancelButtonText: 'İptal'
    }).then(result => {
        if (result.isConfirmed) {
            fetch(`${API_BASE}/${id}`, {
                method: 'DELETE',
                headers: { 'Authorization': 'Bearer ' + token }
            })
                .then(res => {
                    if (!res.ok) throw new Error("Silinemedi");
                    loadRevenues();
                    Swal.fire("Silindi!", "Ciro silindi.", "success");
                })
                .catch(() => Swal.fire("Hata!", "Silme işlemi başarısız.", "error"));
        }
    });
}

function clearForm() {
    $('#revenueForm')[0].reset();
    $('#revenueId').val('');
}
