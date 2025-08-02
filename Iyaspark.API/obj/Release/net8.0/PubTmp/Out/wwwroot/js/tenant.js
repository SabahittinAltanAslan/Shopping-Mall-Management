const API_URL = window.location.hostname.includes("localhost")
    ? "https://localhost:7237/api/Tenant"
    : "https://iyasparkyonetim.com/api/Tenant";

const token = localStorage.getItem("token");
if (!token) {
    Swal.fire("Giriş yapınız!", "", "warning").then(() => {
        window.location.href = "/login.html";
    });
}

const SectorEnum = ["Giyim", "Ayakkabı", "Yemeİçme", "Teknoloji", "Aksesuar", "Hizmet", "Spor", "KitapOyuncak", "Market", "Kozmetik", "EvYasam", "Eczane", "Kuyumcu", "Eğlence", "Cafe", "Diğer"];
const FacadeEnum = ["Kuzey", "Güney", "Doğu", "Batı", "Köşe", "DışCephe"];
const TenantTypeEnum = ["Mağaza", "Kiosk", "Ofis"];

$(document).ready(function () {
    loadEnums();
    loadTenants();

    $('#openCreateModal').click(function () {
        clearForm();
        $('#tenantModal').modal('show');
    });

    $('#tenantForm').submit(function (e) {
        e.preventDefault();

        const dto = {
            id: $('#tenantId').val(),
            companyName: $('#companyName').val(),
            taxNumber: $('#taxNumber').val(),
            squareMeter: parseInt($('#squareMeter').val()),
            floorCode: $('#floorCode').val(),
            floorLabel: $('#floorLabel').val(),
            sector: parseInt($('#sector').val()),
            facadeDirection: parseInt($('#facadeDirection').val()),
            tenantType: parseInt($('#tenantType').val()),
            hasExtraStorage: $('#hasExtraStorage').is(':checked')
        };

        const isUpdate = !!dto.id;
        const url = isUpdate ? `${API_URL}?id=${dto.id}` : API_URL;
        const method = isUpdate ? 'PUT' : 'POST';

        fetch(url, {
            method: method,
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + token
            },
            body: JSON.stringify(dto)
        })
            .then(res => {
                if (res.status === 204 || res.status === 200) return null;
                return res.text().then(text => text ? JSON.parse(text) : null);
            })
            .then(() => {
                $('#tenantModal').modal('hide');
                clearForm();
                loadTenants();
                Swal.fire("Başarılı!", isUpdate ? "Kiracı güncellendi." : "Kiracı eklendi.", "success");
            })
            .catch(err => {
                console.error(err);
                Swal.fire("Hata!", "İşlem başarısız.", "error");
            });
    });
});

function loadEnums() {
    fillSelect('#sector', SectorEnum);
    fillSelect('#facadeDirection', FacadeEnum);
    fillSelect('#tenantType', TenantTypeEnum);
}

function fillSelect(id, list) {
    $(id).empty();
    list.forEach((val, index) => {
        $(id).append(`<option value="${index}">${val}</option>`);
    });
}

function loadTenants() {
    fetch(API_URL, {
        headers: { 'Authorization': 'Bearer ' + token }
    })
        .then(res => res.json())
        .then(data => {
            const table = $('#tenantTable').DataTable();
            table.clear().destroy();
            $('#tenantTable tbody').empty();

            data.forEach(item => {
                const row = `
                    <tr>
                        <td>${item.companyName}</td>
                        <td>${item.taxNumber}</td>
                        <td>${item.squareMeter}</td>
                        <td>${item.floorLabel} (${item.floorCode})</td>
                        <td>${SectorEnum[item.sector]}</td>
                        <td>${FacadeEnum[item.facadeDirection]}</td>
                        <td>${TenantTypeEnum[item.tenantType]}</td>
                        <td>${item.hasExtraStorage ? "Evet" : "Hayır"}</td>
                        <td>
                            <button class="btn btn-sm btn-primary me-1" onclick='editTenant(${JSON.stringify(item)})'>Güncelle</button>
                            <button class="btn btn-sm btn-danger" onclick='deleteTenant("${item.id}")'>Sil</button>
                        </td>
                    </tr>
                `;
                $('#tenantTable tbody').append(row);
            });

            $('#tenantTable').DataTable();
        });
}

function editTenant(item) {
    $('#tenantId').val(item.id);
    $('#companyName').val(item.companyName);
    $('#taxNumber').val(item.taxNumber);
    $('#squareMeter').val(item.squareMeter);
    $('#floorCode').val(item.floorCode);
    $('#floorLabel').val(item.floorLabel);
    $('#sector').val(item.sector);
    $('#facadeDirection').val(item.facadeDirection);
    $('#tenantType').val(item.tenantType);
    $('#hasExtraStorage').prop('checked', item.hasExtraStorage);
    $('#tenantModal').modal('show');
}

function deleteTenant(id) {
    Swal.fire({
        title: 'Emin misiniz?',
        text: "Bu işlem geri alınamaz!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Evet, sil!',
        cancelButtonText: 'İptal'
    }).then(result => {
        if (result.isConfirmed) {
            fetch(`${API_URL}/${id}`, {
                method: 'DELETE',
                headers: { 'Authorization': 'Bearer ' + token }
            })
                .then(res => {
                    if (!res.ok) throw new Error("Silinemedi");
                    loadTenants();
                    Swal.fire("Silindi!", "Kiracı silindi.", "success");
                })
                .catch(err => {
                    console.error(err);
                    Swal.fire("Hata!", "Silme işlemi başarısız.", "error");
                });
        }
    });
}

function clearForm() {
    $('#tenantForm')[0].reset();
    $('#tenantId').val('');
}
