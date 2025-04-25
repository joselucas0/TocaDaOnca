/**
 * Script para gerenciar o login de funcionários
 */

const apiUrl = 'http://localhost:5050/api/Auth/staff'; // URL da API para login de funcionários

document.addEventListener('DOMContentLoaded', function () {
    const loginForm = document.querySelector('.login-form');

    if (loginForm) {
        loginForm.addEventListener('submit', handleEmployeeLogin);
    }
});

/**
 * Função para lidar com o envio do formulário de login
 */
async function handleEmployeeLogin(event) {
    localStorage.clear();
    event.preventDefault();

    const email = event.target[0].value;
    const password = event.target[1].value;

    try {
        const response = await fetch(apiUrl, {
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
        localStorage.setItem('employeeToken', data.token);
        localStorage.setItem('employeeName', data.name);
        localStorage.setItem('employeeId', data.id);
        localStorage.setItem('employeeRole', data.role);
        localStorage.setItem('employeeIsManager', data.isManager);

        alert('Login realizado com sucesso!');
        window.location.href = './funcionario.html'; // Redireciona para a área do funcionário
    } catch (error) {
        console.error('Erro ao fazer login:', error);
        alert('Erro ao fazer login. Tente novamente mais tarde.');
    }
}

/**
 * Função auxiliar para verificar se o usuário está autenticado como funcionário
 */
function isEmployeeAuthenticated() {
    return localStorage.getItem('employeeToken') !== null;
}

/**
 * Função para fazer logout
 */
function employeeLogout() {
    localStorage.removeItem('employeeToken');
    localStorage.removeItem('employeeName');
    localStorage.removeItem('employeeId');
    localStorage.removeItem('employeeRole');
    localStorage.removeItem('employeeIsManager');
    window.location.href = './loginFuncionario.html';
}