# 🎮 FGC – FIAP Cloud Games

Repositório oficial do **FIAP Cloud Games (FGC)**, API backend desenvolvida em **.NET 8** como parte do **Tech Challenge FIAP – Fase 1** da FIAP.

---

## 📚 Índice

- [📦 Visão Geral](#-visão-geral)
- [⚙️ Tecnologias Utilizadas](#-tecnologias-utilizadas)
- [🚀 Como Executar o Projeto](#-como-executar-o-projeto)
- [🛠️ Configuração do Banco de Dados](#-configuração-do-banco-de-dados)
- [🌱 Dados Iniciais (Seed com EF Core)](#-dados-iniciais-seed-com-ef-core)
- [🔐 Credenciais de Acesso](#-credenciais-de-acesso)
- [🔗 Referências e Documentações](#-referências-e-documentações)

---

## 📦 Visão Geral

O **FGC** simula uma **loja virtual de jogos digitais** com recursos completos de autenticação, catálogo, promoções e bibliotecas de jogos por usuário.

### Funcionalidades:

- 🔐 Login e autenticação com **JWT**
- 🎮 Cadastro e listagem de jogos
- 💸 Aplicação de **promoções**
- 📚 Biblioteca personalizada para cada usuário
- 📊 Precificação com histórico de compra

---

## ⚙️ Tecnologias Utilizadas

- [.NET 8 (C#)](https://learn.microsoft.com/en-us/dotnet/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/)
- [JWT Bearer Authentication](https://jwt.io/)

---

## 🚀 Como Executar o Projeto

### 1️⃣ Clonar o Repositório

```bash
git clone https://github.com/VitorDietrich-Coder/FGC-Fiap.git

cd FGC-Challenge

dotnet restore
```
# 🌐 Configuração de Host
- 👤 Autenticação no SQL Server
  
SQLSERVER_USER=seu_usuario
SQLSERVER_PASSWORD=sua_senha_segura

- 🗃️ Banco de Dados
SQLSERVER_DB=FGC_Games
SQLSERVER_HOST=localhost
SQLSERVER_PORT=1433

Alterar no Arquivo AppsettingsDevelop.json

"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=FGC_Games;User Id=seu_usuario;Password=sua_senha_segura;TrustServerCertificate=True;"
}

dotnet run --project ./src/FGC.Api

## Dados inseridos:
👤 Usuários
adminnew@fiapgames.com (Admin)
usernew@fiapgames.com (Usuário comum)

🎮 Jogos
4 títulos com nome, categoria e preço

🛍️ Promoções
3 promoções com datas de validade

📚 Bibliotecas
Uma biblioteca para cada usuário

🧾 LibraryGames
Registros de jogos comprados, com preço pago e data de compra

🔐 Credenciais de Acesso

👤 Usuário Comum
Email: usernew@fiapgames.com
Senha: 1GamesTeste@

👑 Usuário Administrador
Email: adminnew@fiapgames.com
Senha: 1GamesAdmin@

## 📄 Documentação
🛠️ Documentação Técnica (em breve)
