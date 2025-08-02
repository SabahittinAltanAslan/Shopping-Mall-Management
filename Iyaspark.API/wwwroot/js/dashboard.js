const API_BASE = window.location.hostname.includes("localhost")
    ? "https://localhost:7237/api/dashboard"
    : "https://iyasparkyonetim.com/api/dashboard";

const token = localStorage.getItem("token");
if (!token) {
    alert("Giriş yapınız!");
    window.location.href = "/login.html";
}

loadSummary();
loadFloorProfitChart();
loadFacadeTrendChart();
loadSectorComparisonChart();


function loadSummary() {
    fetch(`${API_BASE}/summary`, {
        headers: { Authorization: "Bearer " + token }
    })
        .then(res => res.json())
        .then(data => {
            document.getElementById("cardTotalProfit").textContent = `${data.totalProfit.toLocaleString("tr-TR")} ₺`;
            document.getElementById("cardTopFloor").textContent = data.topFloor;
            document.getElementById("cardTopFacade").textContent = data.topFacade;
            document.getElementById("cardTopTenant").textContent = data.topTenant;
        });
}

function loadFloorProfitChart() {
    fetch(`${API_BASE}/floor-profit`, {
        headers: { Authorization: "Bearer " + token }
    })
        .then(res => res.json())
        .then(data => {
            const labels = data.map(d => d.floorLabel);
            const profits = data.map(d => d.profit);

            new Chart(document.getElementById("floorProfitChart"), {
                type: "bar",
                data: {
                    labels,
                    datasets: [{
                        label: "Toplam Kâr (₺)",
                        data: profits,
                        backgroundColor: "rgba(40, 167, 69, 0.7)"
                    }]
                },
                options: {
                    responsive: true,
                    plugins: { legend: { display: false } }
                }
            });
        });
}

function loadFacadeTrendChart() {
    fetch(`${API_BASE}/facade-profit-trend`, {
        headers: { Authorization: "Bearer " + token }
    })
        .then(res => res.json())
        .then(data => {
            const labels = data.map(d => d.facadeDirection);
            const changes = data.map(d => d.changePercentage);

            new Chart(document.getElementById("facadeTrendChart"), {
                type: "bar",
                data: {
                    labels,
                    datasets: [{
                        label: "Kârlılık Değişimi (%)",
                        data: changes,
                        backgroundColor: changes.map(val => val >= 0 ? "rgba(0, 123, 255, 0.6)" : "rgba(220, 53, 69, 0.6)")
                    }]
                },
                options: {
                    responsive: true,
                    plugins: { legend: { display: false } },
                    scales: {
                        y: { ticks: { callback: val => `${val}%` } }
                    }
                }
            });
        });
}

function loadSectorComparisonChart() {
    fetch(`${API_BASE}/sector-comparison`, {
        headers: { Authorization: "Bearer " + token }
    })
        .then(res => res.json())
        .then(data => {
            const labels = data.map(d => d.sector);
            const revenues = data.map(d => d.totalRevenue);
            const rents = data.map(d => d.totalRent);
            const profits = rents; // same as profit

            new Chart(document.getElementById("sectorComparisonChart"), {
                type: "bar",
                data: {
                    labels,
                    datasets: [
                        {
                            label: "Ciro",
                            data: revenues,
                            backgroundColor: "rgba(0, 123, 255, 0.6)"
                        },
                        {
                            label: "Kira",
                            data: rents,
                            backgroundColor: "rgba(255, 193, 7, 0.6)"
                        },
                        {
                            label: "Kâr",
                            data: profits,
                            backgroundColor: "rgba(40, 167, 69, 0.6)"
                        }
                    ]
                },
                options: {
                    responsive: true,
                    plugins: { legend: { position: "top" } },
                    scales: {
                        y: {
                            ticks: {
                                callback: value => value.toLocaleString("tr-TR")
                            }
                        }
                    }
                }
            });
        });
}
