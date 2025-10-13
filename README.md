# 🎮 FGC – FIAP Cloud Games

Repositório oficial do **FIAP Cloud Games (FGC)**, API backend desenvolvida em **.NET 8** como parte do **Tech Challenge FIAP – Fase 1** da FIAP.

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
- [Deals](https://gg.deals/) Reference of shop of games
---

## 🚀 Como Executar o Projeto

### 1️⃣ Clonar o Repositório

```bash
git clone https://github.com/VitorDietrich-Coder/FGC-Fiap.git

cd ../FGC-Challenge/src/fgc.api

dotnet restore
```
## 🌐 Configuração de Host

#### 👤 Autenticação no SQL Server

Alterar no Arquivo AppsettingsDevelop.json

"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=FGC_Games;User Id=seu_usuario;Password=sua_senha_segura;TrustServerCertificate=True;"
}

Para rodar o projeto execute:
dotnet run --project ./src/FGC.Api

## Dados inseridos:

#### 👤 Usuários

adminnew@fiapgames.com (Admin)

usernew@fiapgames.com (Usuário comum)

####  🎮 Jogos

4 títulos com nome, categoria e preço

####  🛍️ Promoções

3 promoções com datas de validade

####  📚 Bibliotecas

Uma biblioteca para cada usuário

####  🧾 LibraryGames

Registros de jogos comprados, com preço pago e data de compra

## 🔐 Credenciais de Acesso

####   👤 Usuário Comum

Email: usernew@fiapgames.com

Senha: 1GamesTeste@

####  👑 Usuário Administrador

Email: adminnew@fiapgames.com

Senha: 1GamesAdmin@

##  📄 Documentação

🛠️ Event Storming: https://miro.com/app/board/uXjVJXr1M14=/

##  🐳 Rodando a Aplicação com docker

cd FGC-Challenge
docker compose up -d

Com esse comando irá subir a API juntamente com o grafana e o prometheus, 
tendo em vista que deixei um docker compose para empacotar e subir mais facil localmente.

docker-compose.yml

Após isso teremos os seguintes serviços:

API: http://localhost:8080

Grafana: http://localhost:3000
(usuário/padrão: admin / admin)

Prometheus: http://localhost:9090/query



