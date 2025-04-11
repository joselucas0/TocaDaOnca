# TocaDaOnca

# 🐆 TOCA DA ONÇA - Sistema de Gestão para Balneário  

## 🚀 Introdução  
O **Toca da Onça** é um sistema de gestão completo para balneários, desenvolvido para modernizar operações como reservas de espaços, controle de vendas e gestão de visitantes. Nosso objetivo é unir **natureza e tecnologia**, oferecendo uma experiência digital intuitiva para clientes e eficiência operacional para gestores.

---

## 🛠️ Arquitetura do Projeto  
O sistema é dividido em **Frontend** (interface do usuário) e **Backend** (API e lógica de negócio), seguindo o padrão RESTful:  

| Componente       | Tecnologias                  | Responsabilidade                              |  
|------------------|------------------------------|-----------------------------------------------|  
| **Frontend**     | HTML, CSS, JavaScript        | Interface web para clientes e funcionários.   |  
| **Backend**      | ASP.NET Core Web API         | API para processar dados e regras de negócio. |  
| **Banco de Dados** | PostgreSQL + Entity Framework | Armazenamento e gestão de dados.              |  

---

## 🌿 Política de Branches  
Cada funcionalidade/tela deve ser desenvolvida em uma **branch específica**, seguindo o padrão:  
```bash
feature/<tipo>/<nome-da-funcionalidade>  # Ex: feature/front/tela-login
```

### Fluxo de Desenvolvimento:  
1. **Crie sua branch** a partir de `developer`:  
   ```bash
   git checkout developer
   git pull origin developer
   git checkout -b feature/front/tela-login  # Exemplo para frontend
   ```  
2. **Desenvolva e teste** localmente.  
3. **Suba para o repositório**:  
   ```bash
   git push origin feature/front/tela-login
   ```  
4. **Abra um PR** para `developer` e aguarde revisão.  
5. **Após aprovação**, o código será mesclado em `developer` e posteriormente em `main`.  

### Tipos de Branches:  
| Tipo          | Exemplo                      | Descrição                                  |  
|---------------|------------------------------|--------------------------------------------|  
| **front**     | `feature/front/tela-login`   | Desenvolvimento de interfaces (HTML/CSS/JS). |  
| **api**       | `feature/api/reservas`       | Criação/modificação de endpoints da API.   |  
| **db**        | `feature/db/migracao-estoque`| Alterações no banco de dados (migrations). |  

---

## ✨ Padrões de Commit  
Adotamos **Conventional Commits** para rastreabilidade clara:  

| Prefixo       | Exemplo                          | Escopo Válido               |  
|---------------|----------------------------------|------------------------------|  
| **`feat:`**   | `feat(front): adiciona tela de login` | `front`, `api`, `db`         |  
| **`fix:`**    | `fix(api): corrige status code 500`   | `front`, `api`, `db`         |  
| **`refactor:`**| `refactor(db): otimiza query de reservas` | `front`, `api`, `db` |  
| **`style:`**  | `style(front): ajusta padding do menu` | `front`                     |  
| **`docs:`**   | `docs: atualiza guia de instalação`    | (sem escopo)               |  

---

## 📥 Instalação e Configuração  

### Pré-requisitos:  
- [.NET 6 SDK](https://dotnet.microsoft.com/download)  
- [PostgreSQL 14+](https://www.postgresql.org/download/)  
- Navegador moderno (Chrome, Firefox)  

### Passos para Executar o Projeto:  
1. **Clone o repositório**:  
   ```bash
   git clone https://github.com/seu-usuario/toca-da-onca.git
   cd toca-da-onca/backend
   ```  

2. **Configure o banco de dados**:  
   - Crie um arquivo `appsettings.json` com:  
     ```json
      {
      "ConnectionStrings": {
         "DefaultConnection": "Host=localhost;Database=TocaDaOnca;Username=postgres;Password=olszewski"
      },
      "Jwt": {
         "Key": "tocadaonca_supersecret_key_for_jwt_auth_2025",
         "Issuer": "TocaDaOnca.API",
         "Audience": "TocaDaOnca.Client"
      },
      "Logging": {
         "LogLevel": {
         "Default": "Information",
         "Microsoft.AspNetCore": "Warning"
         }
      },
      "AllowedHosts": "*"
   }
     ```  

3. **Restaure as dependências**:  
   ```bash
   dotnet restore
   ```  

4. **Execute as migrations**:  
   ```bash
   dotnet ef database update
   ```  

5. **Inicie o servidor backend**:  
   ```bash
   dotnet run
   ```  

6. **Acesse o frontend**:  
   - Abra `frontend/index.html` em um navegador.  

---

## 🧪 Como Testar  
### Testando a API (Backend):  
Use ferramentas como [Postman](https://www.postman.com/) ou `curl`:  
```bash
# Exemplo: Listar quiosques
curl -X GET https://localhost:5001/api/quiosques
```  

### Testando o Frontend:  
- Abra o console do navegador (**F12**) para ver erros JS.  
- Simule reservas através da interface.  

---

## 🤝 Como Contribuir  
1. **Sincronize-se com `developer`** antes de codificar:  
   ```bash
   git fetch origin
   git rebase origin/developer
   ```  
2. **Siga os padrões de código**:  
   - Backend: Use injeção de dependência e async/await.  
   - Frontend: Mantenha o CSS modularizado.  
3. **Documente alterações complexas** no PR.  

---

## 🔗 Links do Projeto  
- [Quadro no Trello](https://trello.com/invite/b/67f6b300afe50dd31f552fff/ATTI61c4b60fd6926267b41f52e5a44c7fc6E8F94865/toca-da-onca)  
- [Documentação do ASP.NET Core](https://docs.microsoft.com/pt-br/aspnet/core/)  
- [Guia do Entity Framework](https://docs.microsoft.com/pt-br/ef/core/)
- [Figma do projeto em alto nível](https://www.figma.com/design/0JhUqmtc8EYWbJABtmBDch/Untitled?node-id=0-1&p=f)

---

## 📄 Licença  
Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para detalhes.  

**"Organizando a selva digital! 🐾"**  
``` 

### Exemplo de Estrutura de Pastas:  
```plaintext
toca-da-onca/
├── backend/                 # ASP.NET Core Web API
│   ├── Controllers/         # Endpoints (ReservaController.cs)
│   ├── Models/              # Entidades (Quiosque.cs, Visitante.cs)
│   ├── Migrations/          # Scripts do Entity Framework
│   └── appsettings.json     # Configurações
│
├── frontend/                # Interface estática
│   ├── css/                 # Estilos
│   ├── js/                  # Scripts (login.js, reservas.js)
│   └── index.html           # Página inicial
│
└── README.md                # Você está aqui!
```
