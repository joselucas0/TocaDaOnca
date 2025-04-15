/**
 * Script para gerenciar o registro de usuários
 */

const apiUrl = 'http://localhost:5050/api/User'; // URL da API para registro de usuários

document.addEventListener('DOMContentLoaded', function () {
    console.log("DOM carregado, inicializando formulário de registro");
    const registerForm = document.querySelector('.register-form');

    if (registerForm) {
        console.log("Formulário de registro encontrado, adicionando event listener");
        registerForm.addEventListener('submit', handleRegister);
    } else {
        console.error("Formulário de registro não encontrado");
    }

    // Inicializa as máscaras de entrada
    initializeMasks();
});

/**
 * Função para lidar com o envio do formulário de registro
 */
async function handleRegister(event) {
    event.preventDefault();
    console.log("Formulário de registro submetido");

    // Obter os valores do formulário
    const name = document.getElementById('name').value;
    const cpf = document.getElementById('cpf').value.replace(/[^\d]/g, ''); // Remove pontos e traços
    const birthDate = document.getElementById('birth_date').value;
    const email = document.getElementById('email').value;
    const phone = document.getElementById('phone').value.replace(/\D/g, ''); // Remover caracteres não numéricos
    const premium = document.getElementById('plan').value === 'premium'; // Convertendo para booleano
    const password = document.getElementById('password').value;

    console.log("Valores do formulário coletados:", { name, cpf, birthDate, email, phone, premium });

    // Validar os campos - vamos temporariamente pular a validação de idade para testar
    // se o problema está aí
    if (!validateFormExceptAge(name, cpf, birthDate, email, phone, password)) {
        console.log("Validação do formulário falhou");
        return;
    }

    try {
        console.log("Enviando requisição para a API...");
        
        // Construindo o objeto para envio conforme o formato que funcionou no Swagger
        const userData = {
            fullName: name,
            cpf: cpf,
            birthDate: new Date(birthDate).toISOString(), // Convertendo para o formato ISO
            email: email,
            phone: phone,
            premium: premium,
            password: password,
            reservations: []
        };
        
        console.log("Dados a serem enviados:", userData);
        
        const response = await fetch(apiUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(userData)
        });

        console.log("Status da resposta:", response.status);
        
        // Se houve erro
        if (!response.ok) {
            let errorText = "";
            try {
                const errorData = await response.json();
                console.error("Erro da API:", errorData);
                errorText = errorData.message || 'Erro ao criar conta';
            } catch (jsonError) {
                console.error("Erro ao processar resposta JSON:", jsonError);
                errorText = await response.text();
                console.error("Texto da resposta de erro:", errorText);
            }
            alert('Erro ao criar conta: ' + errorText);
            return;
        }

        const data = await response.json();
        console.log("Cadastro realizado com sucesso:", data);
        alert('Conta criada com sucesso!');

        // Redireciona para a página de login após o cadastro
        window.location.href = '/Client/loginUsuario.html';
    } catch (error) {
        console.error('Erro ao registrar usuário:', error);
        alert('Erro ao criar conta. Tente novamente mais tarde. Detalhes: ' + error.message);
    }
}

/**
 * Função para validar os campos do formulário sem a verificação de idade
 */
function validateFormExceptAge(name, cpf, birthDate, email, phone, password) {
    // Validação do CPF (algoritmo simplificado)
    if (cpf.length !== 11) {
        alert('Por favor, insira um CPF válido');
        return false;
    }

    // Validação básica de data de nascimento
    if (!birthDate) {
        alert('Por favor, insira uma data de nascimento');
        return false;
    }

    // Validação de senha (mínimo 6 caracteres)
    if (password.length < 6) {
        alert('A senha deve ter pelo menos 6 caracteres');
        return false;
    }

    return true;
}

/**
 * Função para validar os campos do formulário (original com validação de idade)
 */
function validateForm(name, cpf, birthDate, email, phone, password) {
    // Validação do CPF (algoritmo simplificado)
    if (cpf.length !== 11) {
        alert('Por favor, insira um CPF válido');
        return false;
    }

    // Validação da data de nascimento (deve ser maior de 18 anos)
    const today = new Date();
    const birth = new Date(birthDate);
    const age = today.getFullYear() - birth.getFullYear();
    const m = today.getMonth() - birth.getMonth();

    if (m < 0 || (m === 0 && today.getDate() < birth.getDate())) {
        age--;
    }

    if (age < 18) {
        alert('Você deve ter pelo menos 18 anos para se cadastrar');
        return false;
    }

    // Validação de senha (mínimo 6 caracteres)
    if (password.length < 6) {
        alert('A senha deve ter pelo menos 6 caracteres');
        return false;
    }

    return true;
}

/**
 * Função para inicializar as máscaras de entrada
 */
function initializeMasks() {
    // Máscara para CPF
    document.getElementById('cpf').addEventListener('input', function (e) {
        let value = e.target.value.replace(/\D/g, '');

        if (value.length > 3) {
            value = value.substring(0, 3) + '.' + value.substring(3);
        }
        if (value.length > 7) {
            value = value.substring(0, 7) + '.' + value.substring(7);
        }
        if (value.length > 11) {
            value = value.substring(0, 11) + '-' + value.substring(11, 13);
        }

        e.target.value = value;
    });

    // Máscara para telefone
    document.getElementById('phone').addEventListener('input', function (e) {
        let value = e.target.value.replace(/\D/g, '');

        if (value.length > 0) {
            value = '(' + value;
        }
        if (value.length > 3) {
            value = value.substring(0, 3) + ') ' + value.substring(3);
        }
        if (value.length > 10) {
            value = value.substring(0, 10) + '-' + value.substring(10);
        }

        e.target.value = value;
    });
}