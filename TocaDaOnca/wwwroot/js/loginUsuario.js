/**
 * Script para gerenciar o login de usuários comuns
 */

document.addEventListener('DOMContentLoaded', function () {
    const loginForm = document.querySelector('.login-form');

    if (loginForm) {
        loginForm.addEventListener('submit', handleLogin);
    }
});

/**
 * Função para lidar com o envio do formulário de login
 */
async function handleLogin(event) {
    event.preventDefault();

    const email = event.target[0].value;
    const password = event.target[1].value;

    try {
        const response = await fetch('api/Auth/user', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                email: email,
                password: password
            })
        });

        if (!response.ok) {
            const errorData = await response.json();
            alert(errorData.message || 'Erro ao fazer login');
            return;
        }

        const data = await response.json();
        // Salva o token no localStorage para uso em requisições autenticadas
        localStorage.setItem('token', data.token);

        // Extrai informações do token (opcional)
        const tokenInfo = parseJwt(data.token);
        localStorage.setItem('userId', tokenInfo.nameid || '');
        localStorage.setItem('userEmail', tokenInfo.email || '');

        alert('Login realizado com sucesso!');
        window.location.href = '/index.html'; // Redireciona para a página inicial
    } catch (error) {
        console.error('Erro ao fazer login:', error);
        alert('Erro ao fazer login. Tente novamente mais tarde.');
    }
}

/**
 * Função auxiliar para decodificar tokens JWT
 */
function parseJwt(token) {
    try {
        const base64Url = token.split('.')[1];
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        const jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));

        return JSON.parse(jsonPayload);
    } catch (e) {
        console.error('Erro ao decodificar token:', e);
        return {};
    }
}