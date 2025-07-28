# POCTMB - Projeto de Simulação e Processamento de Pedidos

## Requisitos
- Docker
- Docker Compose

## Serviços incluídos
- PostgreSQL
- Azure Storage Emulator (Azurite)
- API backend (.NET 8)
- Worker de processamento (consome mensagens do Service Bus)
- Frontend (React)

## Configuração e execução

### 1. Clonar o repositório

```bash
git clone <URL_DO_REPOSITORIO>
cd poc-tmb
```

### 2. Construir e subir os containers

```bash
docker-compose up --build
```

Isso irá:
- Construir as imagens para backend, worker e frontend
- Subir todos os containers: banco, azurite, backend, worker e frontend

### 3. Aguardar

O backend executará migrations automaticamente no startup e ficará disponível na porta 5000.

O frontend ficará disponível na porta 4200.

### 4. Acessar a aplicação

Abra o navegador em: http://localhost:4200

### 5. APIs disponíveis

- API REST de pedidos: `http://localhost:5000/api/orders`
- Swagger UI: `http://localhost:5000/swagger`

### 6. WebSocket (SignalR)

O frontend se conecta automaticamente ao Hub SignalR para receber notificações de mudanças no status dos pedidos.

---

## Problemas comuns

- **Conexão com PostgreSQL falha?**  
  Verifique se a porta 5432 está livre e o container do banco está rodando.

- **Service Bus não conecta?**  
  Verifique a string de conexão no arquivo `appsettings.json` e no `docker-compose.yml`.  
  Assegure que a fila `orders-queue` existe no Azure Service Bus.

- **Erro de CORS**  
  O backend está configurado para permitir todas as origens na política `AllowAll`.

---

## Comandos úteis

- Subir containers (em background):  
  `docker-compose up -d`

- Ver logs do backend:  
  `docker logs -f poc-orderapi`

- Parar containers:  
  `docker-compose down`

---

## Estrutura de pastas

- `backend/OrderApi`: Código backend API
- `backend/OrderWorker`: Worker de processamento
- `frontend/order-frontend`: Código frontend React
- `docker-compose.yml`: Orquestração dos containers

---

## Contato

Matheus Carvalho  
E-mail: mtw.nda@hotmail.com  
