# 👤 FCG Users API

Microsserviço de **usuários** da FIAP Cloud Games (Tech Challenge — Fase 2).

## 📌 Finalidade

- Cadastro de usuários
- Login com geração de token **JWT**
- Autorização por perfil (`Usuario` e `Administrador`)
- Publica o evento **`UserCreatedEvent`** no RabbitMQ sempre que um usuário é cadastrado

Este serviço veio da separação do monólito da Fase 1 (`AuthController`, `UsuariosController` e Identity).

## 🔗 Endpoints

| Método | Rota | Acesso |
|---|---|---|
| POST | `/api/auth/register` | Público |
| POST | `/api/auth/login` | Público |
| GET | `/api/auth/perfil` | Autenticado |
| GET | `/api/usuarios` | Administrador |

Administrador criado automaticamente: `admin@fcg.com` / `Admin@123`.

## 📨 Eventos

| Evento | Tipo | Campos |
|---|---|---|
| `UserCreatedEvent` | Publica | `UserId`, `Nome`, `Email` |

## ⚙️ Variáveis de ambiente

| Variável | Descrição | Exemplo |
|---|---|---|
| `ConnectionStrings__DefaultConnection` | Banco SQLite | `Data Source=/app/data/users.db` |
| `Jwt__Key` | Chave do token JWT (Base64, 32 bytes) | — |
| `Jwt__Issuer` | Emissor do token | `Fcg.Users.Api` |
| `Jwt__Audience` | Público do token | `Fcg` |
| `RabbitMq__Host` | Endereço do RabbitMQ | `rabbitmq` |
| `RabbitMq__Username` | Usuário do RabbitMQ | `fcg` |
| `RabbitMq__Password` | Senha do RabbitMQ | — |

No Kubernetes, os valores não sensíveis estão no **ConfigMap** (`k8s/configmap.yaml`) e os sensíveis no **Secret** (`k8s/secret.yaml`).

## 🛠️ Tecnologias

.NET 8, ASP.NET Core, Entity Framework Core + SQLite, ASP.NET Identity, JWT, MassTransit + RabbitMQ, Swagger, xUnit, Docker e Kubernetes.

## ▶️ Como executar

A forma recomendada é pelo repositório de orquestração (`fcg-orchestration`), que sobe todos os serviços com `docker compose up`.

Para gerar só a imagem deste serviço:

```bash
docker build -t fcg-users-api:latest .
```

Para rodar pelo Visual Studio, o RabbitMQ precisa estar rodando em `localhost`. O banco é criado automaticamente ao iniciar.

## 🧪 Testes

```bash
dotnet test
```

## ☸️ Kubernetes

```bash
kubectl apply -f k8s/
```

Arquivos: `deployment.yaml`, `service.yaml`, `configmap.yaml` e `secret.yaml`. Dentro do cluster o serviço é acessado por `http://users-api:80`.

---

Feito por **Milena Torres**
