# Aequitas: Real-Time Portfolio Engine

> Simulando um Sistema de Back-Office e Gestão de Risco em .NET

Aequitas é um motor de processamento financeiro de back-end desenvolvido em ASP.NET Core Web API (NET 8) que simula o básico de um sistema de gestão de posições e cálculo de performance em tempo real, comummente usado em corretoras ou gestoras de ativos.

---

## Funcionalidades Chave

### 1. Sistema de Agendamento Assíncrono (Cron Job)
* **Tecnologia:** **Hangfire**.
* **Descrição:** Utilização de um *background job processor* robusto para agendar tarefas recorrentes. O sistema executa o `PriceUpdateJob` a cada **5 minutos** para simular a atualização constante de preços do mercado, crucial em um ambiente de *trading*. 

### 2. Integração de API Externa e Preços ao Vivo
* **Tecnologia:** `HttpClient` e **Alpha Vantage API**.
* **Descrição:** Integração e tratamento de dados externos para obter cotações de ativos em tempo real, com gestão eficiente da desserialização JSON (incluindo tratamento de culturas numéricas).

### 3. Engine de Cálculo Financeiro
* **Métricas Calculadas:** P&L (Profit & Loss), Valor de Mercado, Exposição no Portfólio e Performance Diária.
* **Diferencial:** Demonstra conhecimento de lógica financeira de back-office e uso de tipos de dados seguros (`decimal`) para cálculos monetários.

### 4. Apresentação Interativa
* **Tecnologia:** **Blazor Server**.
* **Descrição:** Um dashboard web interativo construído em C# para visualizar os dados e as métricas calculadas pelo *engine* em tempo real.

---

## 🛠️ Tecnologias Utilizadas

| Categoria | Tecnologia | Justificativa |
| :--- | :--- | :--- |
| **Framework** | .NET 8 (C#) | Versão LTS mais recente, performance e estabilidade. |
| **Persistência** | Entity Framework Core (SQLite) | Mapeamento Objeto-Relacional e fácil portabilidade para testes. |
| **Agendamento** | Hangfire | Solução robusta e com Dashboard web para monitorização de jobs. |
| **Comunicação** | Alpha Vantage API | Provedor de dados financeiros para preços ao vivo. |
| **Front-end** | Blazor Server | Demonstra full-stack C# e interatividade web. |

---

## ⚙️ Como Configurar e Executar

### Pré-requisitos
1.  .NET 8 SDK
2.  Chave de API da Alpha Vantage

### Passos
1.  **Clone o Repositório:**
    ```bash
    git clone 
    cd AequitasTracker
    ```
2.  **Configuração da API Key:**
    * Edite `appsettings.json` e insira sua chave:
        ```json
        "AlphaVantage": {
            "ApiKey": "SUA_CHAVE_AQUI" 
        }
        ```
3.  **Restaurar e Construir:**
    ```bash
    dotnet restore
    dotnet build
    ```
4.  **Criar o Banco de Dados:**
    ```bash
    dotnet ef database update
    ```
5.  **Executar a Aplicação:**
    ```bash
    dotnet run
    ```
    
    * A aplicação estará acessível em `https://localhost:PORTA`.
    * O Dashboard Blazor estará na raiz (`/`).
    * O Dashboard do Hangfire (Back-Office) estará em `/hangfire`.

### Endpoints da API (Swagger)

Você pode usar o Swagger para registrar as posições: `/swagger`

* `POST api/Positions`: Para adicionar um novo ativo ao portfólio.
* `GET api/Positions/performance`: Para obter os resultados do motor de cálculo.
