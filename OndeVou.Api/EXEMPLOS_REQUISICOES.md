# Exemplos de Requisições HTTP - OndeVou API

## 🔧 Usando REST Client (VS Code) ou similar

### 1. Criar Usuário
```http
POST https://localhost:7000/api/usuario
Content-Type: application/json

{
  "nome": "João Silva",
  "email": "joao@example.com",
  "senha": "senha123456"
}
```

**Resposta Esperada (201 Created):**
```json
{
  "id": 1,
  "nome": "João Silva",
  "email": "joao@example.com",
  "dataCriacao": "2024-01-20T10:30:00Z"
}
```

---

### 2. Login (Obter Token)
```http
POST https://localhost:7000/api/usuario/login
Content-Type: application/json

{
  "email": "joao@example.com",
  "senha": "senha123456"
}
```

**Resposta Esperada (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwiZW1haWwiOiJqb2FvQGV4YW1wbGUuY29tIiwidW5pcXVlX25hbWUiOiJKb8OjbyBTaWx2YSIsImp0aSI6IjEyMzQ1Njc4LTEyMzQtMTIzNC0xMjM0LTEyMzQ1Njc4OTAxMiIsImV4cCI6MTcwNTc1MjYwMCwiaXNzIjoiT25kZVZvdUFQSSIsImF1ZCI6Ik9uZGVWb3VBUEkifQ.signature",
  "expiresAt": "2024-01-20T12:30:00Z"
}
```

**Resposta em Caso de Erro (401 Unauthorized):**
```json
{
  "mensagem": "Email ou senha inválidos"
}
```

---

### 3. Criar Estabelecimento (COM Token)
```http
POST https://localhost:7000/api/estabelecimento
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "nome": "Restaurante Bom Sabor",
  "descricao": "Culinária italiana autêntica",
  "categoria": "Restaurante",
  "latitude": -23.550520,
  "longitude": -46.633308
}
```

**Resposta Esperada (201 Created):**
```json
{
  "id": 1,
  "nome": "Restaurante Bom Sabor",
  "descricao": "Culinária italiana autêntica",
  "categoria": "Restaurante",
  "latitude": -23.550520,
  "longitude": -46.633308
}
```

---

### 4. Tentar Criar Estabelecimento SEM Token (Deve Falhar)
```http
POST https://localhost:7000/api/estabelecimento
Content-Type: application/json

{
  "nome": "Teste",
  "descricao": "Teste",
  "categoria": "Teste",
  "latitude": 0,
  "longitude": 0
}
```

**Resposta Esperada (401 Unauthorized):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7235#section-3.1",
  "title": "Unauthorized",
  "status": 401
}
```

---

## 🧪 Sequência de Testes Completa

### Arquivo: requests.http (para VS Code REST Client)

```http
### Variáveis
@baseUrl = https://localhost:7000
@token = {{login.response.body.token}}

### 1. Criar Usuário
POST {{baseUrl}}/api/usuario
Content-Type: application/json

{
  "nome": "Maria Santos",
  "email": "maria@example.com",
  "senha": "senha123456"
}

### 2. Login e Salvar Token
# @name login
POST {{baseUrl}}/api/usuario/login
Content-Type: application/json

{
  "email": "maria@example.com",
  "senha": "senha123456"
}

### 3. Criar Estabelecimento (usando token da requisição anterior)
POST {{baseUrl}}/api/estabelecimento
Authorization: Bearer {{token}}
Content-Type: application/json

{
  "nome": "Café da Manhã",
  "descricao": "Melhor café da cidade",
  "categoria": "Cafeteria",
  "latitude": -23.550520,
  "longitude": -46.633308
}

### 4. Testar Login Inválido
POST {{baseUrl}}/api/usuario/login
Content-Type: application/json

{
  "email": "maria@example.com",
  "senha": "senhaErrada"
}

### 5. Testar Acesso Sem Token
POST {{baseUrl}}/api/estabelecimento
Content-Type: application/json

{
  "nome": "Teste Sem Token",
  "descricao": "Deve falhar",
  "categoria": "Teste",
  "latitude": 0,
  "longitude": 0
}
```

---

## 🐍 Usando Python

```python
import requests
import json

BASE_URL = "https://localhost:7000"

# 1. Criar usuário
def criar_usuario():
    url = f"{BASE_URL}/api/usuario"
    data = {
        "nome": "Python User",
        "email": "python@example.com",
        "senha": "senha123456"
    }
    response = requests.post(url, json=data, verify=False)
    print(f"Criar Usuário: {response.status_code}")
    return response.json()

# 2. Login
def fazer_login():
    url = f"{BASE_URL}/api/usuario/login"
    data = {
        "email": "python@example.com",
        "senha": "senha123456"
    }
    response = requests.post(url, json=data, verify=False)
    print(f"Login: {response.status_code}")
    if response.status_code == 200:
        return response.json()["token"]
    return None

# 3. Criar estabelecimento
def criar_estabelecimento(token):
    url = f"{BASE_URL}/api/estabelecimento"
    headers = {
        "Authorization": f"Bearer {token}",
        "Content-Type": "application/json"
    }
    data = {
        "nome": "Python Restaurant",
        "descricao": "Criado via Python",
        "categoria": "Restaurante",
        "latitude": -23.550520,
        "longitude": -46.633308
    }
    response = requests.post(url, json=data, headers=headers, verify=False)
    print(f"Criar Estabelecimento: {response.status_code}")
    return response.json()

# Executar testes
if __name__ == "__main__":
    print("1. Criando usuário...")
    criar_usuario()

    print("\n2. Fazendo login...")
    token = fazer_login()

    if token:
        print(f"\nToken obtido: {token[:50]}...")

        print("\n3. Criando estabelecimento...")
        resultado = criar_estabelecimento(token)
        print(json.dumps(resultado, indent=2))
```

---

## 🟢 Usando Node.js (Axios)

```javascript
const axios = require('axios');

const BASE_URL = 'https://localhost:7000';

// Ignorar SSL em desenvolvimento
process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0';

async function criarUsuario() {
  try {
    const response = await axios.post(`${BASE_URL}/api/usuario`, {
      nome: 'Node User',
      email: 'node@example.com',
      senha: 'senha123456'
    });
    console.log('Usuário criado:', response.data);
  } catch (error) {
    console.error('Erro ao criar usuário:', error.response?.data);
  }
}

async function fazerLogin() {
  try {
    const response = await axios.post(`${BASE_URL}/api/usuario/login`, {
      email: 'node@example.com',
      senha: 'senha123456'
    });
    console.log('Login bem-sucedido!');
    return response.data.token;
  } catch (error) {
    console.error('Erro no login:', error.response?.data);
    return null;
  }
}

async function criarEstabelecimento(token) {
  try {
    const response = await axios.post(
      `${BASE_URL}/api/estabelecimento`,
      {
        nome: 'Node Restaurant',
        descricao: 'Criado via Node.js',
        categoria: 'Restaurante',
        latitude: -23.550520,
        longitude: -46.633308
      },
      {
        headers: {
          Authorization: `Bearer ${token}`
        }
      }
    );
    console.log('Estabelecimento criado:', response.data);
  } catch (error) {
    console.error('Erro ao criar estabelecimento:', error.response?.data);
  }
}

async function executarTestes() {
  console.log('1. Criando usuário...');
  await criarUsuario();

  console.log('\n2. Fazendo login...');
  const token = await fazerLogin();

  if (token) {
    console.log(`\nToken obtido: ${token.substring(0, 50)}...`);

    console.log('\n3. Criando estabelecimento...');
    await criarEstabelecimento(token);
  }
}

executarTestes();
```

---

## 🔍 Validações Implementadas

### LoginRequestDto
- ✅ Email obrigatório
- ✅ Email válido (formato)
- ✅ Senha obrigatória
- ✅ Senha mínima de 6 caracteres

### Exemplos de Validação

**Email inválido:**
```http
POST /api/usuario/login
{
  "email": "email-invalido",
  "senha": "123456"
}
```
Retorna: 400 Bad Request com mensagem de validação

**Senha muito curta:**
```http
POST /api/usuario
{
  "nome": "Teste",
  "email": "teste@mail.com",
  "senha": "123"
}
```
Retorna: 400 Bad Request - "A senha deve ter no mínimo 6 caracteres"

---

## 📊 Decodificando o Token JWT

Você pode decodificar o token em: https://jwt.io

**Exemplo de payload decodificado:**
```json
{
  "nameid": "1",
  "email": "joao@example.com",
  "unique_name": "João Silva",
  "jti": "12345678-1234-1234-1234-123456789012",
  "exp": 1705752600,
  "iss": "OndeVouAPI",
  "aud": "OndeVouAPI"
}
```

---

## ⚠️ Códigos de Status HTTP

| Código | Significado | Quando Ocorre |
|--------|-------------|---------------|
| 200 | OK | Login bem-sucedido |
| 201 | Created | Usuário ou estabelecimento criado |
| 400 | Bad Request | Dados inválidos ou faltando |
| 401 | Unauthorized | Credenciais inválidas ou token ausente/expirado |
| 409 | Conflict | Email já cadastrado |

---

**Pronto para testar!** 🚀
