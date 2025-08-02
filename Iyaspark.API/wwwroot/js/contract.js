const apiUrl = "https://localhost:7237/api/Contract";
const tenantApiUrl = "https://localhost:7237/api/Tenant";
const token = localStorage.getItem("token");
const role = localStorage.getItem("role");

let table;

$(document).ready(function () {
    if (!token || role !== "Admin") {
        window.location.href = "/login.html";
        return;
    }

    loadTenants();
    loadContracts();

    $("#btnOpenModal").click(() => openContractModal());
    $("#contractForm").submit(e => handleSubmit(e));

    $("#rentType, #hasExtraStorage").change(toggleFields);
});

function loadTenants() {
    $.ajax({
        url: tenantApiUrl,
        method: "GET",
        headers: { Authorization: `Bearer ${token}` },
        success: function (data) {
            const tenantSelect = $("#tenantId");
            tenantSelect.empty();
            tenantSelect.append(`<option value="">Seçiniz</option>`);
            data.forEach(t => {
                tenantSelect.append(`<option value="${t.id}">${t.companyName}</option>`);
            });
        }
    });
}

function loadContracts() {
    $.ajax({
        url: apiUrl,
        method: "GET",
        headers: { Authorization: `Bearer ${token}` },
        success: function (data) {
            if ($.fn.DataTable.isDataTable("#contractTable")) {
                table.clear().rows.add(data).draw();
            } else {
                table = $("#contractTable").DataTable({
                    data: data,
                    columns: [
                        { data: "companyName", defaultContent: "" },
                        {
                            data: "startDate",
                            render: date => date ? date.split("T")[0] : ""
                        },
                        {
                            data: "endDate",
                            render: date => date ? date.split("T")[0] : ""
                        },
                        {
                            data: null,
                            render: (data) => {
                                const rt = data.rentType;
                                let result = getRentTypeText(rt);

                                switch (rt) {
                                    case 0: // Sabit
                                        if (data.fixedRentAmount)
                                            result += ` (${data.fixedRentAmount.toLocaleString("tr-TR")} ₺)`;
                                        break;
                                    case 1: // Ciro
                                        if (data.revenuePercentage)
                                            result += ` (%${data.revenuePercentage})`;
                                        break;
                                    case 2: // Sabit + Ciro
                                        const fixed = data.fixedRentAmount ? `${data.fixedRentAmount.toLocaleString("tr-TR")} ₺` : "";
                                        const percent = data.revenuePercentage ? `%${data.revenuePercentage}` : "";
                                        result += ` (${fixed} + ${percent})`;
                                        break;
                                    case 3: // Kademeli
                                        const above = data.aboveTargetPercentage ? `%${data.aboveTargetPercentage}` : "";
                                        const below = data.belowTargetPercentage ? `%${data.belowTargetPercentage}` : "";
                                        result += ` (Üst: ${above}, Alt: ${below})`;
                                        break;
                                }

                                if (data.extraStorageRent) {
                                    result += `<br><span class="badge bg-secondary">Ekstra Depo: ${data.extraStorageRent.toLocaleString("tr-TR")} ₺</span>`;
                                }

                                return result;
                            }
                        },
                        {
                            data: "pdfFilePath",
                            render: path =>
                                path
                                    ? `<a href="/${path}" class="btn btn-sm btn-secondary" target="_blank">PDF</a>`
                                    : "-"
                        },
                        {
                            data: null,
                            render: (data) => `
                                <button class="btn btn-sm btn-primary" onclick='editContract(${JSON.stringify(
                                data
                            )})'>Güncelle</button>
                                <button class="btn btn-sm btn-danger" onclick='deleteContract("${data.id}")'>Sil</button>
                            `
                        }
                    ]
                });
            }
        }
    });
}


function getRentTypeText(type) {
    return ["Sabit", "Ciro", "Sabit + Ciro", "Kademeli"][type] || "-";
}

function openContractModal() {
    $("#contractForm")[0].reset();
    $("#contractId").val("");
    $("#contractModal").modal("show");
    toggleFields();
}

function editContract(data) {
    $("#contractId").val(data.id);
    $("#tenantId").val(data.tenantId);
    $("#startDate").val(data.startDate.split("T")[0]);
    $("#endDate").val(data.endDate.split("T")[0]);
    $("#rentType").val(data.rentType);
    $("#fixedRentAmount").val(data.fixedRentAmount || "");
    $("#revenuePercentage").val(data.revenuePercentage || "");
    $("#revenueTarget").val(data.revenueTarget || "");
    $("#aboveTargetPercentage").val(data.aboveTargetPercentage || "");
    $("#belowTargetPercentage").val(data.belowTargetPercentage || "");
    $("#hasExtraStorage").prop("checked", data.extraStorageRent > 0);
    $("#extraStorageRent").val(data.extraStorageRent || "");

    // Contract PDF yolunu saklıyoruz (güncelleme için)
    $("#contractFilePath").val(data.pdfFilePath || "");

    toggleFields();
    $("#contractModal").modal("show");
}


function deleteContract(id) {
    Swal.fire({
        title: "Emin misiniz?",
        text: "Bu işlem geri alınamaz!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Evet, sil",
        cancelButtonText: "İptal"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `${apiUrl}/${id}`,
                method: "DELETE",
                headers: { Authorization: `Bearer ${token}` },
                success: loadContracts
            });
        }
    });
}

async function handleSubmit(e) {
    e.preventDefault();
    // 0. Sayısal alanlardaki virgülleri noktaya çevir
    [
        "#fixedRentAmount",
        "#revenuePercentage",
        "#revenueTarget",
        "#aboveTargetPercentage",
        "#belowTargetPercentage",
        "#extraStorageRent"
    ].forEach(selector => {
        const el = $(selector);
        if (el.length && el.val().includes(',')) {
            el.val(el.val().replace(',', '.'));
        }
    });

    const id = $("#contractId").val();
    const method = id ? "PUT" : "POST";
    let filePath = "";

    // 1. PDF dosyası varsa önce upload işlemi
    const file = $("#contractFile")[0].files[0];
    if (file) {
        const uploadForm = new FormData();
        uploadForm.append("file", file);

        await $.ajax({
            url: apiUrl + "/upload",
            method: "POST",
            data: uploadForm,
            contentType: false,
            processData: false,
            headers: { Authorization: `Bearer ${token}` },
            success: res => {
                filePath = res.filePath; // contracts/xyz.pdf
            },
            error: err => {
                alert("PDF yükleme başarısız!");
                return;
            }
        });
    }

    // 2. DTO verisi
    const formData = new FormData();
    if (id) formData.append("Id", id);
    formData.append("TenantId", $("#tenantId").val());
    formData.append("StartDate", $("#startDate").val());
    formData.append("EndDate", $("#endDate").val());
    formData.append("RentType", $("#rentType").val());
    formData.append("FixedRentAmount", $("#fixedRentAmount").val());
    formData.append("RevenuePercentage", $("#revenuePercentage").val());
    formData.append("RevenueTarget", $("#revenueTarget").val());
    formData.append("AboveTargetPercentage", $("#aboveTargetPercentage").val());
    formData.append("BelowTargetPercentage", $("#belowTargetPercentage").val());
    formData.append("HasExtraStorage", $("#hasExtraStorage").is(":checked"));
    formData.append("ExtraStorageRent", $("#extraStorageRent").val());

    // 3. 📎 Dosya yolu ekle
    if (filePath) formData.append("ContractFilePath", filePath);

    // 4. API'ye gönder
    $.ajax({
        url: apiUrl,
        method: method,
        headers: { Authorization: `Bearer ${token}` },
        contentType: false,
        processData: false,
        data: formData,
        success: () => {
            $("#contractModal").modal("hide");
            loadContracts();
        },
        error: (xhr) => {
            if (xhr.status === 400) {
                const errors = xhr.responseJSON?.errors;
                Swal.fire({
                    icon: "error",
                    title: "Geçersiz Veri",
                    html: errors ? errors.join("<br>") : "Hatalı veri girdiniz."
                });
            } else {
                Swal.fire("Hata", "Sunucu hatası oluştu", "error");
            }
        }
    });

}



function submitFormData(method, formData) {
    $.ajax({
        url: apiUrl,
        method: method,
        headers: { Authorization: `Bearer ${token}` },
        contentType: false,
        processData: false,
        data: formData,
        success: () => {
            $("#contractModal").modal("hide");
            loadContracts();
        },
        error: (xhr) => {
            console.error("Hata:", xhr.responseText);
            alert("Bir hata oluştu.");
        }
    });
}


function toggleFields() {
    const rentType = $("#rentType").val();
    const hasExtraStorage = $("#hasExtraStorage").is(":checked");

    // Tüm alanları gizle
    $(".container-fixedRentAmount").hide();
    $(".container-revenuePercentage").hide();
    $(".container-revenueTarget").hide();
    $(".container-aboveTargetPercentage").hide();
    $(".container-belowTargetPercentage").hide();
    $(".container-extraStorageRent").hide();

    // Kira tipi alanlarını göster
    if (rentType === "0") {
        $(".container-fixedRentAmount").show(); // Sabit
    } else if (rentType === "1") {
        $(".container-revenuePercentage").show(); // Ciro
    } else if (rentType === "2") {
        $(".container-fixedRentAmount").show();
        $(".container-revenuePercentage").show(); // Sabit + Ciro
    } else if (rentType === "3") {
        $(".container-revenueTarget").show();
        $(".container-aboveTargetPercentage").show();
        $(".container-belowTargetPercentage").show(); // Kademeli
    }

    // Ekstra depo alanı
    if (hasExtraStorage) {
        $(".container-extraStorageRent").show();
    } else {
        $(".container-extraStorageRent").hide();
        $("#extraStorageRent").val(""); // gereksiz veri gitmesin
    }
}

