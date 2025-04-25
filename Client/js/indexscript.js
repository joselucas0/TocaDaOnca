// Função para abrir as abas
function openTab(evt, tabName) {
    // Esconde todos os conteúdos de tab
    var tabContents = document.getElementsByClassName("tab-content");
    for (var i = 0; i < tabContents.length; i++) {
        tabContents[i].classList.remove("active");
    }

    // Remove a classe active de todas as tabs
    var tabLinks = document.getElementsByClassName("client-tab");
    for (var i = 0; i < tabLinks.length; i++) {
        tabLinks[i].classList.remove("active");
    }

    // Mostra a tab atual e adiciona a classe active ao botão
    document.getElementById(tabName).classList.add("active");
    evt.currentTarget.classList.add("active");
}

// Função para carregar os quiosques da API
async function loadKiosks() {
    try {
        const response = await fetch("http://localhost:5050/api/Kiosk");
        if (!response.ok) {
            throw new Error("Erro ao carregar quiosques");
        }
        const kiosks = await response.json();
        displayKiosks(kiosks);
    } catch (error) {
        console.error("Erro ao carregar quiosques:", error);
    }
}

// Função para exibir os quiosques na interface
function displayKiosks(kiosks) {
    const container = document.querySelector(".kiosks-grid");
    if (!kiosks || kiosks.length === 0) {
        container.innerHTML = "<p>Nenhum quiosque disponível no momento.</p>";
        return;
    }

    container.innerHTML = "";
    kiosks.forEach((kiosk) => {
        // Usar imagens placeholder baseadas no id do quiosque (para manter as imagens atuais)
        const imageUrl = getKioskImage(kiosk.id);
        const tags = kiosk.description.split(",").map((tag) => tag.trim());
        const tagElements = tags.map((tag) => `<span>${tag}</span>`).join("");

        console.table(kiosk);

        const kioskCard = `
        <div class="kiosk-card">
            <div class="kiosk-image">
                <img src="${imageUrl}" alt="${kiosk.title}">
            </div>
            <div class="kiosk-info">
                <h3>${kiosk.title}</h3>
                <p style='font-size: 1rem'>Comporta até ${kiosk.maxPeople} pessoas</p>
                <div class="kiosk-price">R$ ${kiosk.value} / dia</div>
                <div class="kiosk-features">
                    ${tagElements}
                </div>
                <button class="reserve-btn" onclick="openModal('${kiosk.title}', 150, ${kiosk.id})">Reservar</button>
            </div>
        </div>`;

        container.innerHTML += kioskCard;
    });
}

// Função para obter imagem de acordo com o ID do quiosque (manter as imagens atuais do sistema)
function getKioskImage(id) {
    const images = [
        "https://images.unsplash.com/photo-1564501049412-61c2a3083791?ixlib=rb-1.2.1&auto=format&fit=crop&w=1350&q=80",
        "https://images.unsplash.com/photo-1582719478250-c89cae4dc85b?ixlib=rb-1.2.1&auto=format&fit=crop&w=1350&q=80",
        "https://images.unsplash.com/photo-1566073771259-6a8506099945?ixlib=rb-1.2.1&auto=format&fit=crop&w=1350&q=80",
    ];
    return images[id % images.length];
}

// Funções para o modal
function openModal(kioskName, kioskPrice, kioskId) {
    // See if the user is logged
    const user = JSON.parse(localStorage.getItem("userId"));
    if (!user) {
        alert("Faça login para realizar reservas!");
        window.location.href = "./loginUsuario.html";
        return;
    }

    const token = localStorage.getItem("token");
    const userData = parseJwt(token);

    if (userData) {
        document.getElementById("inputNome").value = userData.unique_name || "";
        document.getElementById("inputEmail").value = userData.email || "";
        document.getElementById("inputCpf").value = userData.CPF.trim() || "";
        document.getElementById("inputPhone").value = userData.Phone || "";
    } else {
        console.log("Usuário não está logado ou token inválido.");
    }

    document.getElementById("modalKioskName").textContent = kioskName;
    document.getElementById("kiosk-type").value = kioskName;
    document.getElementById("kiosk-id").value = kioskId; // Adicionando o ID do quiosque
    document.getElementById("kiosk-price").value = kioskPrice;
    document.getElementById("reservation-summary-kiosk").textContent =
        kioskName;
    document.getElementById("reservation-summary-price").textContent = "R$ " +
        kioskPrice.toFixed(2);

    // Configurar a data mínima como hoje
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, "0");
    var mm = String(today.getMonth() + 1).padStart(2, "0");
    var yyyy = today.getFullYear();
    today = yyyy + "-" + mm + "-" + dd;
    document.getElementById("reservation-date").min = today;

    document.getElementById("reservationModal").style.display = "flex";
}

function closeModal() {
    document.getElementById("reservationModal").style.display = "none";
}

// Fechar modal ao clicar fora do conteúdo
window.onclick = function (event) {
    var modal = document.getElementById("reservationModal");
    if (event.target == modal) {
        closeModal();
    }
};

// Função para verificar se o usuário está autenticado
function isAuthenticated() {
    return localStorage.getItem("token") !== null;
}

// Função para obter o ID do usuário do token
function getUserId() {
    const userId = localStorage.getItem("userId");
    return userId ? parseInt(userId) : null;
}

// Formulário de reserva
document.getElementById("reservation-form").addEventListener(
    "submit",
    async function (e) {
        e.preventDefault();

        if (!isAuthenticated()) {
            alert("Você precisa estar logado para fazer uma reserva.");
            window.location.href = "/Client/loginUsuario.html";
            return;
        }

        const userId = getUserId();
        if (!userId) {
            alert(
                "Erro ao identificar o usuário. Por favor, faça login novamente.",
            );
            window.location.href = "/Client/loginUsuario.html";
            return;
        }

        const kioskId = document.getElementById("kiosk-id").value;
        const reservationDate =
            document.getElementById("reservation-date").value;
        const numPeople = document.getElementById("reservation-people").value;

        if (!reservationDate) {
            alert("Por favor, selecione uma data para a reserva.");
            return;
        }

        // Cria objeto de data para a reserva
        const selectedDate = new Date(reservationDate);
        // Configura para às 08:00
        selectedDate.setHours(8, 0, 0, 0);
        // Configura data final para às 18:00 do mesmo dia
        const endDate = new Date(selectedDate);
        endDate.setHours(18, 0, 0, 0);

        try {
            // Criar a reserva
            const reservationData = {
                userId: userId,
                kioskId: parseInt(kioskId),
                startTime: selectedDate.toISOString(),
                endTime: endDate.toISOString(),
            };

            const reservationResponse = await fetch(
                "http://localhost:5050/api/Reservation",
                {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                        "Authorization": "Bearer " +
                            localStorage.getItem("token"),
                    },
                    body: JSON.stringify(reservationData),
                },
            );

            if (!reservationResponse.ok) {
                throw new Error("Erro ao criar reserva");
            }

            const reservationResult = await reservationResponse.json();

            alert(
                "Sua solicitação de reserva foi enviada com sucesso! Entraremos em contato para confirmação.",
            );
            this.reset();
            closeModal();
        } catch (error) {
            console.error("Erro ao processar reserva:", error);
            alert("Erro ao processar reserva: " + error.message);
        }
    },
);

// Carregar quiosques quando a página carregar
document.addEventListener("DOMContentLoaded", function () {
    loadKiosks();
});

// Funcção para decodificar o JWT
function parseJwt(token) {
    try {
        const base64Url = token.split(".")[1];
        const base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
        const jsonPayload = decodeURIComponent(
            atob(base64).split("").map(function (c) {
                return "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2);
            }).join(""),
        );
        return JSON.parse(jsonPayload);
    } catch (e) {
        return null;
    }
}
