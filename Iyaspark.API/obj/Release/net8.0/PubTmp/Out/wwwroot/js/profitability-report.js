const API_BASE = window.location.hostname.includes("localhost")
    ? "https://localhost:7237/api/Report"
    : "https://iyasparkyonetim.com/api/Report";

const token = localStorage.getItem("token");
if (!token) {
    alert("Giriş yapınız!");
    window.location.href = "/login.html";
}

// ENUM sabitleri
const rentTypeOptions = ["Sabit", "Ciro", "Sabit&Ciro", "Kademeli"];
const tenantTypeOptions = ["Mağaza", "Kiosk", "Ofis"];
const sectorOptions = ["Giyim", "Ayakkabı", "Yemeİçme", "Teknoloji", "Aksesuar", "Hizmet", "Spor", "KitapOyuncak", "Market", "Kozmetik", "EvYaşam", "Eczane", "Kuyumcu", "Eğlence", "Cafe", "Diğer"];
const facadeDirectionOptions = ["Kuzey", "Güney", "Doğu", "Batı", "Köşe", "DışCephe"];

// Dropdownları doldur
function populateDropdown(id, options) {
    const select = document.getElementById(id);
    select.innerHTML = '<option value="">Tümü</option>';
    options.forEach((opt, index) => {
        select.innerHTML += `<option value="${index}">${opt}</option>`;
    });
}

populateDropdown("rentType", rentTypeOptions);
populateDropdown("tenantType", tenantTypeOptions);
populateDropdown("sector", sectorOptions);
populateDropdown("facadeDirection", facadeDirectionOptions);

// Kat numarası label
function mapFloorNumberToLabel(val) {
    switch (parseInt(val)) {
        case -1: return "-1";
        case 0: return "Zemin";
        case 1: return "1";
        case 2: return "2";
        default: return null;
    }
}

// Tarih kontrolü
function validateDateRange(start, end) {
    if (!start || !end) {
        Swal.fire({
            icon: "warning",
            title: "Tarih Eksik!",
            text: "Lütfen başlangıç ve bitiş tarihi giriniz.",
            confirmButtonColor: "#3085d6",
            confirmButtonText: "Tamam"
        });
        return false;
    }
    return true;
}


function getFilters() {
    const startDate = $('#startDate').val();
    const endDate = $('#endDate').val();
    if (!validateDateRange(startDate, endDate)) return null;

    const floorRaw = $('#floorNumber').val();
    const floorLabel = floorRaw ? mapFloorNumberToLabel(floorRaw) : null;

    return {
        startDate,
        endDate,
        rentType: $('#rentType').val() || null,
        tenantType: $('#tenantType').val() || null,
        sector: $('#sector').val() || null,
        floorLabel,
        facadeDirection: $('#facadeDirection').val() || null,
        hasExtraStorage: $('#hasExtraStorage').val() || null,
        minSquareMeter: $('#minSquareMeter').val() || null,
        maxSquareMeter: $('#maxSquareMeter').val() || null
    };
}

function getProfitabilityReport() {
    const filters = getFilters();
    if (!filters) return;

    fetch(`${API_BASE}/profitability`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + token
        },
        body: JSON.stringify(filters)
    })
        .then(res => res.json())
        .then(data => {
            const tbody = $('#reportTable tbody');
            tbody.empty();

            if (data.length === 0) {
                tbody.append(`<tr><td colspan="15" class="text-center">Kayıt bulunamadı</td></tr>`);
                return;
            }

            data.forEach(item => {
                const row = `
    <tr>
        <td>${item.companyName}</td>
        <td>${item.taxNumber}</td>
        <td>${item.sector ?? "-"}</td>
        <td>${item.facadeDirection ?? "-"}</td>
        <td>${item.tenantType ?? "-"}</td>
        <td>${item.floorCode ?? "-"}</td>
        <td>${item.floorLabel ?? "-"}</td>
        <td>${item.squareMeter ?? "-"}</td>
        <td>${item.rentType ?? "-"}</td>
<td>${item.fixedRentAmount > 0 ? item.fixedRentAmount : "-"}</td>
<td>${item.revenueBasedRent > 0 ? item.revenueBasedRent : "-"}</td>
<td>${item.extraStorageRent > 0 ? item.extraStorageRent : "-"}</td>
        <td>${item.totalRevenue ?? "-"}</td>
        <td>${item.totalRent ?? "-"}</td>
        <td>${item.profit ?? "-"}</td>
    </tr>
`;

                tbody.append(row);
            });
        })
        .catch(err => {
            console.error("Rapor getirilemedi", err);
            alert("Rapor getirilemedi.");
        });
}

function exportProfitabilityExcel() {
    const filters = getFilters();
    if (!filters) return;

    fetch(`${API_BASE}/profitability/excel`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + token
        },
        body: JSON.stringify(filters)
    })
        .then(res => {
            if (!res.ok) throw new Error("Excel indirilemedi");
            return res.blob();
        })
        .then(blob => {
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement("a");
            a.href = url;
            a.download = "karlilik-raporu.xlsx";
            document.body.appendChild(a);
            a.click();
            a.remove();
        })
        .catch(err => {
            console.error(err);
            alert("Excel oluşturulamadı.");
        });
}
