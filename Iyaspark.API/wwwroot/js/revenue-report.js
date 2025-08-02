const API_BASE = window.location.hostname.includes("localhost")
    ? "https://localhost:7237/api/Report"
    : "https://iyasparkyonetim.com/api/Report";

const token = localStorage.getItem("token");
if (!token) {
    Swal.fire("Giriş yapınız!", "", "warning").then(() => {
        window.location.href = "/login.html";
    });
}

// ENUM seçenekleri
const sectorOptions = [
    "Giyim", "Ayakkabı", "Yemeİçme", "Teknoloji", "Aksesuar",
    "Hizmet", "Spor", "KitapOyuncak", "Market", "Kozmetik",
    "EvYaşam", "Eczane", "Kuyumcu", "Eğlence", "Cafe", "Diğer"
];
const tenantTypeOptions = ["Mağaza", "Kiosk", "Ofis"];
const facadeDirectionOptions = ["Kuzey", "Güney", "Doğu", "Batı", "Köşe", "DışCephe"];

// Dropdown'ları doldur
function populateDropdown(id, options) {
    const select = document.getElementById(id);
    select.innerHTML = '<option value="">Tümü</option>';
    options.forEach((opt, index) => {
        select.innerHTML += `<option value="${index}">${opt}</option>`;
    });
}

populateDropdown("sector", sectorOptions);
populateDropdown("tenantType", tenantTypeOptions);
populateDropdown("facadeDirection", facadeDirectionOptions);

// Floor değerini string'e çevir
function mapFloorNumberToLabel(value) {
    switch (parseInt(value)) {
        case -1: return "-1";
        case 0: return "Zemin";
        case 1: return "1";
        case 2: return "2";
        default: return null;
    }
}

// Tarih zorunlu kontrolü
function validateDateRange(start, end) {
    if (!start || !end) {
        Swal.fire("Uyarı", "Lütfen başlangıç ve bitiş tarihi seçiniz.", "warning");
        return false;
    }
    return true;
}

function getReport() {
    const startDate = $('#startDate').val();
    const endDate = $('#endDate').val();
    if (!validateDateRange(startDate, endDate)) return;

    const floorRaw = $('#floorNumber').val();
    const floorLabel = floorRaw ? mapFloorNumberToLabel(floorRaw) : null;

    const filters = {
        startDate,
        endDate,
        tenantType: $('#tenantType').val() === "" ? null : $('#tenantType').val(),
        sector: $('#sector').val() === "" ? null : $('#sector').val(),
        facadeDirection: $('#facadeDirection').val() === "" ? null : $('#facadeDirection').val(),
        floorLabel: floorLabel,
        minSquareMeter: $('#minSquareMeter').val() || null,
        maxSquareMeter: $('#maxSquareMeter').val() || null
    };

    fetch(`${API_BASE}/revenue`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + token
        },
        body: JSON.stringify(filters)
    })
        .then(res => {
            if (!res.ok) throw res;
            return res.json();
        })
        .then(data => {
            $('#reportTable tbody').empty();
            if (data.length === 0) {
                $('#reportTable tbody').append(`<tr><td colspan="8" class="text-center">Kayıt bulunamadı</td></tr>`);
                return;
            }

            data.forEach(item => {
                const row = `
                <tr>
                    <td>${item.companyName}</td>
                    <td>${item.taxNumber}</td>
                    <td>${item.sector || "-"}</td>
                    <td>${item.tenantType || "-"}</td>
                    <td>${item.floorLabel || "-"}</td>
                    <td>${item.facadeDirection || "-"}</td>
                    <td>${item.squareMeter}</td>
                    <td>${item.totalRevenue.toLocaleString('tr-TR')} ₺</td>
                </tr>
            `;
                $('#reportTable tbody').append(row);
            });
        })
        .catch(async err => {
            let msg = "Rapor getirilirken bir hata oluştu.";
            try {
                const errJson = await err.json();
                if (errJson?.errors) {
                    msg = Object.values(errJson.errors).flat().join("<br>");
                }
            } catch { }
            Swal.fire("Hata!", msg, "error");
        });
}

function exportToExcel() {
    const startDate = $('#startDate').val();
    const endDate = $('#endDate').val();
    if (!validateDateRange(startDate, endDate)) return;

    const floorRaw = $('#floorNumber').val();
    const floorLabel = floorRaw ? mapFloorNumberToLabel(floorRaw) : null;

    const filters = {
        startDate,
        endDate,
        tenantType: $('#tenantType').val() === "" ? null : $('#tenantType').val(),
        sector: $('#sector').val() === "" ? null : $('#sector').val(),
        facadeDirection: $('#facadeDirection').val() === "" ? null : $('#facadeDirection').val(),
        floorLabel: floorLabel,
        minSquareMeter: $('#minSquareMeter').val() || null,
        maxSquareMeter: $('#maxSquareMeter').val() || null
    };

    fetch(`${API_BASE}/revenue/excel`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + token
        },
        body: JSON.stringify(filters)
    })
        .then(response => {
            if (!response.ok) throw new Error("Excel indirilemedi");
            return response.blob();
        })
        .then(blob => {
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement("a");
            a.href = url;
            a.download = "ciro-raporu.xlsx";
            document.body.appendChild(a);
            a.click();
            a.remove();
        })
        .catch(err => {
            console.error(err);
            Swal.fire("Hata!", "Excel oluşturulamadı.", "error");
        });
}
