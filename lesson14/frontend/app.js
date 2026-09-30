const API_URL = "http://localhost:5050/api";

// Uygulama aÃ§Ä±ldÄ±ÄŸÄ±nda Token var mÄ± kontrol et
document.addEventListener("DOMContentLoaded", () => {
    const token = localStorage.getItem("taskify_token");
    if (token) {
        showDashboard();
    }
});

// Sekme DeÄŸiÅŸtirme (Login / Register)
function switchTab(tab) {
    document.querySelectorAll('.tab-btn').forEach(btn => btn.classList.remove('active'));
    document.getElementById('loginForm').classList.add('hidden');
    document.getElementById('registerForm').classList.add('hidden');
    document.getElementById('authError').classList.add('hidden');

    if (tab === 'login') {
        document.querySelectorAll('.tab-btn')[0].classList.add('active');
        document.getElementById('loginForm').classList.remove('hidden');
    } else {
        document.querySelectorAll('.tab-btn')[1].classList.add('active');
        document.getElementById('registerForm').classList.remove('hidden');
    }
}

function showError(msg) {
    const err = document.getElementById('authError');
    err.innerText = msg;
    err.classList.remove('hidden');
}

// KAYIT OL (REGISTER)
document.getElementById("registerForm").addEventListener("submit", async (e) => {
    e.preventDefault();
    const name = document.getElementById("registerName").value;
    const email = document.getElementById("registerEmail").value;
    const password = document.getElementById("registerPassword").value;

    try {
        const response = await fetch(`${API_URL}/auth/register`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ name, email, password })
        });

        const data = await response.json();
        
        if (response.ok) {
            alert("KayÄ±t baÅŸarÄ±lÄ±! LÃ¼tfen giriÅŸ yapÄ±n.");
            switchTab('login');
        } else {
            // FluentValidation veya API'den gelen hata
            showError(data.title || data.message || "Bir hata oluÅŸtu");
        }
    } catch (err) {
        showError("Sunucuya baÄŸlanÄ±lamadÄ±. Docker Ã§alÄ±ÅŸÄ±yor mu?");
    }
});

// GÄ°RÄ°Å YAP (LOGIN)
document.getElementById("loginForm").addEventListener("submit", async (e) => {
    e.preventDefault();
    const email = document.getElementById("loginEmail").value;
    const password = document.getElementById("loginPassword").value;

    try {
        const response = await fetch(`${API_URL}/auth/login`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ email, password })
        });

        const data = await response.json();

        if (response.ok) {
            // Token'Ä± tarayÄ±cÄ±ya kaydet!
            localStorage.setItem("taskify_token", data.token);
            showDashboard();
        } else {
            showError(data.message || "GiriÅŸ baÅŸarÄ±sÄ±z.");
        }
    } catch (err) {
        showError("Sunucuya baÄŸlanÄ±lamadÄ±.");
    }
});

// DASHBOARD (GÃ¶revler) EKRANINA GEÃ‡
function showDashboard() {
    document.getElementById("authPanel").classList.add("hidden");
    document.getElementById("dashboardPanel").classList.remove("hidden");
    fetchTasks();
}

// Ã‡IKIÅ YAP
function logout() {
    localStorage.removeItem("taskify_token");
    document.getElementById("authPanel").classList.remove("hidden");
    document.getElementById("dashboardPanel").classList.add("hidden");
}

// GÃ–REVLERÄ° GETÄ°R (Sadece Bana Ait Olanlar)
async function fetchTasks() {
    const token = localStorage.getItem("taskify_token");
    const response = await fetch(`${API_URL}/task`, {
        headers: { "Authorization": `Bearer ${token}` }
    });

    if (response.status === 401) {
        alert("Oturumunuz sÃ¼resi dolmuÅŸ. LÃ¼tfen tekrar girin.");
        logout();
        return;
    }

    const tasks = await response.json();
    const taskList = document.getElementById("taskList");
    taskList.innerHTML = "";

    tasks.forEach(task => {
        const li = document.createElement("li");
        li.className = "task-item";
        const textStyle = task.isCompleted ? "text-decoration: line-through; color: #888;" : "";
        const btnText = task.isCompleted ? "Geri Al" : "Tamamla";
        li.innerHTML = `
            <span style="${textStyle}" onclick="toggleTask(${task.id})" class="task-title-clickable">${task.title}</span>
            <div>
                <button class="toggle-btn" onclick="toggleTask(${task.id})">${btnText}</button>
                <button class="delete-btn" onclick="deleteTask(${task.id})">Sil</button>
            </div>
        `;
        taskList.appendChild(li);
    });
}

// YENÄ° GÃ–REV EKLE
async function addTask() {
    const titleInput = document.getElementById("newTaskTitle");
    const title = titleInput.value.trim();
    if (!title) return;

    const token = localStorage.getItem("taskify_token");
    const response = await fetch(`${API_URL}/task`, {
        method: "POST",
        headers: { 
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}` 
        },
        body: JSON.stringify({ title, description: "Otomatik eklendi" })
    });

    if (response.ok) {
        titleInput.value = "";
        fetchTasks(); // Listeyi yenile
    }
}

// GÃ–REV SÄ°L
async function deleteTask(id) {
    const token = localStorage.getItem("taskify_token");
    const response = await fetch(`${API_URL}/task/${id}`, {
        method: "DELETE",
        headers: { "Authorization": `Bearer ${token}` }
    });

    if (response.ok) {
        fetchTasks();
    } else {
        const data = await response.json();
        alert(data.message || "Silinemedi");
    }
}

// GÖREV DURUMUNU DEĞİŞTİR (TAMAMLANDI/TAMAMLANMADI)
async function toggleTask(id) {
    const token = localStorage.getItem("taskify_token");
    const response = await fetch(`${API_URL}/task/${id}/toggle`, {
        method: "PATCH",
        headers: { "Authorization": `Bearer ${token}` }
    });

    if (response.ok) {
        fetchTasks();
    } else {
        const data = await response.json();
        alert(data.message || "Güncellenemedi");
    }
}

