const API_URL = "http://localhost:5299/api/user";

// Ekranda Tab (Sekme) değiştirme mantığı
function switchTab(tab) {
    document.getElementById('authError').style.display = 'none';
    document.getElementById('authSuccess').style.display = 'none';

    if (tab === 'login') {
        document.getElementById('loginForm').classList.remove('hidden');
        document.getElementById('registerForm').classList.add('hidden');
        document.getElementById('tabLogin').classList.add('active');
        document.getElementById('tabRegister').classList.remove('active');
    } else {
        document.getElementById('loginForm').classList.add('hidden');
        document.getElementById('registerForm').classList.remove('hidden');
        document.getElementById('tabLogin').classList.remove('active');
        document.getElementById('tabRegister').classList.add('active');
    }
}

// Hata/Başarı Mesajı Gösterme
function showError(msg, areaId = 'authError') {
    const el = document.getElementById(areaId);
    el.innerText = msg;
    el.style.display = 'block';
}

function showSuccess(msg, areaId = 'authSuccess') {
    const el = document.getElementById(areaId);
    el.innerText = msg;
    el.style.display = 'block';
}

// KAYIT OL (REGISTER)
document.getElementById('registerForm').addEventListener('submit', async (e) => {
    e.preventDefault(); // Sayfanın yenilenmesini engelle
    
    const name = document.getElementById('registerName').value;
    const email = document.getElementById('registerEmail').value;
    const password = document.getElementById('registerPassword').value;

    try {
        const response = await fetch(`${API_URL}`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ Name: name, Email: email, RawPassword: password })
        });

        // Eğer .NET (FluentValidation veya Middleware) bize hata döndüyse:
        if (!response.ok) {
            const errData = await response.json();
            
            // Eğer FluentValidation 400 Bad Request döndüyse hataları ayıklayıp göster
            if (errData.errors) {
                let errorMessages = Object.values(errData.errors).flat().join('\n');
                showError("Geçersiz Veri:\n" + errorMessages);
            } 
            // Eğer bizim GlobalExceptionMiddleware hata döndüyse (Aynı email falan)
            else if (errData.errorMessage) {
                showError(errData.errorMessage);
            } 
            else {
                showError("Bilinmeyen bir hata oluştu.");
            }
            return;
        }

        // Başarılı
        showSuccess("Harika! Kayıt başarılı. Şimdi giriş yapabilirsiniz.");
        document.getElementById('authError').style.display = 'none';
        
    } catch (err) {
        showError("Sunucuya bağlanılamadı. API açık mı?");
    }
});

// GİRİŞ YAP (LOGIN)
document.getElementById('loginForm').addEventListener('submit', async (e) => {
    e.preventDefault();
    
    const email = document.getElementById('loginEmail').value;
    const password = document.getElementById('loginPassword').value;

    try {
        const response = await fetch(`${API_URL}/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ Email: email, RawPassword: password })
        });

        if (!response.ok) {
            // Eğer FluentValidation 400 Bad Request döndüyse:
            if (response.status === 400) {
                const errData = await response.json();
                if(errData.errors) {
                    showError(Object.values(errData.errors).flat().join('\n'));
                } else if(errData.errorMessage) {
                    showError(errData.errorMessage);
                }
            } else if (response.status === 401) {
                showError("E-posta veya şifre hatalı!");
            }
            return;
        }

        // Başarılı (Token geldi!)
        const data = await response.json();
        
        // Gelen JWT biletini tarayıcının yerel hafızasına (Local Storage) kaydediyoruz.
        localStorage.setItem("myToken", data.token);

        // Ekranları değiştir (Uygulamanın içine gir)
        document.getElementById('authArea').classList.add('hidden');
        document.getElementById('dashboardArea').classList.remove('hidden');

    } catch (err) {
        showError("Sunucuya bağlanılamadı.");
    }
});

// BİLGİLERİ GETİR (AUTHORIZE)
async function fetchUsers() {
    // Tarayıcının hafızasındaki JWT'yi al
    const token = localStorage.getItem("myToken");

    if (!token) {
        showError("VIP Kartınız yok! Önce giriş yapın.", "dashboardError");
        return;
    }

    try {
        const response = await fetch(`${API_URL}`, {
            method: 'GET',
            headers: { 
                'Content-Type': 'application/json',
                // ZARFIN İÇİNE BİLETİ KOYUYORUZ (Bearer [Bilet])
                'Authorization': `Bearer ${token}` 
            }
        });

        if (response.status === 401) {
            showError("Yetkiniz yok veya Token süresi dolmuş. Lütfen tekrar giriş yapın.", "dashboardError");
            return;
        }

        const users = await response.json();
        
        const listEl = document.getElementById('usersList');
        listEl.innerHTML = users.map(u => `
            <div class="user-card">
                <strong>İsim:</strong> ${u.name} <br>
                <strong>E-Posta:</strong> ${u.email}
            </div>
        `).join('');

        document.getElementById('dashboardError').style.display = 'none';

    } catch (err) {
        showError("Bağlantı hatası.", "dashboardError");
    }
}

// ÇIKIŞ YAP
function logout() {
    // Hafızadaki JWT'yi sil
    localStorage.removeItem("myToken");
    
    // Auth ekranına geri dön
    document.getElementById('authArea').classList.remove('hidden');
    document.getElementById('dashboardArea').classList.add('hidden');
    document.getElementById('usersList').innerHTML = ""; // listeyi temizle
}
