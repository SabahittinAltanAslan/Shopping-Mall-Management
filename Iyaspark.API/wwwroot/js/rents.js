document.getElementById("btnFilter").addEventListener("click", loadMonthlyRents);

async function loadMonthlyRents() {
    const year = document.getElementById("yearInput").value;
    const month = document.getElementById("monthInput").value;
    const token = localStorage.getItem("token");

    if (!token) {
        alert("Giriş yapmanız gerekiyor.");
        return;
    }

    try {
        const response = await fetch(`/api/report/monthly?year=${year}&month=${month}`, {
            headers: {
                "Authorization": `Bearer ${token}`
            }
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const data = await response.json();
        renderTable(data);
    } catch (error) {
        console.error("Veri çekme hatası:", error);
        alert("Kira verileri yüklenirken bir hata oluştu.");
    }
}

function renderTable(data) {
    const tbody = document.querySelector("#monthlyRentTable tbody");
    tbody.innerHTML = "";

    data.forEach(rent => {
        const row = `
            <tr>
                <td>${rent.companyName}</td>
                <td>${rent.taxNumber}</td>
                <td>${rent.floorLabel}</td>
                <td>${rent.facadeDirection}</td>
                <td>${rent.rentType}</td>
                <td>${rent.month}</td>
                <td>${rent.year}</td>
                <td>${rent.rentAmount.toFixed(2)} ₺</td>
            </tr>
        `;
        tbody.insertAdjacentHTML("beforeend", row);
    });
}
