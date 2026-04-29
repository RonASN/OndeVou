# 🧪 Guia de Testes - Tipos de Usuário

## 📋 Cenários de Teste

### ✅ Cenário 1: Criar Usuário Comum

**Request:**
```http
POST https://localhost:7000/api/usuario
Content-Type: application/json

{
  "nome": "Maria Santos",
  "email": "maria@example.com",
  "senha": "senha123",
  "tipoUsuario": 1
}
```

**Response Esperada (201 Created):**
```json
{
  "id": 1,
  "nome": "Maria Santos",
  "email": "maria@example.com",
  "tipoUsuario": 1,
  "tipoUsuarioDescricao": "Comum",
  "dataCriacao": "2024-04-29T10:00:00Z"
}
```

---

### ✅ Cenário 2: Criar Usuário Empresa

**Request:**
```http
POST https://localhost:7000/api/usuario
Content-Type: application/json

{
  "nome": "Pizzaria Bella Italia",
  "email": "contato@bellaitalia.com",
  "senha": "senha123",
  "tipoUsuario": 2
}
```

**Response Esperada (201 Created):**
```json
{
  "id": 2,
  "nome": "Pizzaria Bella Italia",
  "email": "contato@bellaitalia.com",
  "tipoUsuario": 2,
  "tipoUsuarioDescricao": "Empresa",
  "dataCriacao": "2024-04-29T10:05:00Z"
}
```

---

### ❌ Cenário 3: Tentar Criar com TipoUsuario Inválido

**Request:**
```http
POST https://localhost:7000/api/usuario
Content-Type: application/json

{
  "nome": "Teste",
  "email": "teste@example.com",
  "senha": "senha123",
  "tipoUsuario": 5
}
```

**Response Esperada (400 Bad Request):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "TipoUsuario": [
      "Tipo de usuário inválido. Use 1 para Comum ou 2 para Empresa"
    ]
  }
}
```

---

### ✅ Cenário 4: Login com Usuário Empresa

**Request:**
```http
POST https://localhost:7000/api/usuario/login
Content-Type: application/json

{
  "email": "contato@bellaitalia.com",
  "senha": "senha123"
}
```

**Response Esperada (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIyIiwiZW1haWwiOiJjb250YXRvQGJlbGxhaXRhbGlhLmNvbSIsInVuaXF1ZV9uYW1lIjoiUGl6emFyaWEgQmVsbGEgSXRhbGlhIiwidGlwbyI6IjIiLCJqdGkiOiIxMjM0NTY3OC0xMjM0LTEyMzQtMTIzNC0xMjM0NTY3ODkwMTIiLCJleHAiOjE3MTQzOTIwMDAsImlzcyI6Ik9uZGVWb3VBUEkiLCJhdWQiOiJPbmRlVm91QVBJIn0.signature",
  "expiresAt": "2024-04-29T12:00:00Z"
}
```

**Token Decodificado (em jwt.io):**
```json
{
  "nameid": "2",
  "email": "contato@bellaitalia.com",
  "unique_name": "Pizzaria Bella Italia",
  "tipo": "2",
  "jti": "12345678-1234-1234-1234-123456789012",
  "exp": 1714392000,
  "iss": "OndeVouAPI",
  "aud": "OndeVouAPI"
}
```

---

### ✅ Cenário 5: Empresa Cria Estabelecimento (Sucesso)

**Request:**
```http
POST https://localhost:7000/api/estabelecimento
Authorization: Bearer {token-do-usuario-empresa}
Content-Type: application/json

{
  "nome": "Pizzaria Bella Italia - Centro",
  "descricao": "A melhor pizza da cidade!",
  "categoria": "Restaurante",
  "latitude": -23.550520,
  "longitude": -46.633308
}
```

**Response Esperada (201 Created):**
```json
{
  "id": 1,
  "nome": "Pizzaria Bella Italia - Centro",
  "descricao": "A melhor pizza da cidade!",
  "categoria": "Restaurante",
  "latitude": -23.550520,
  "longitude": -46.633308
}
```

---

### ❌ Cenário 6: Usuário Comum Tenta Criar Estabelecimento (Falha)

**Passo 1: Login com Usuário Comum**
```http
POST https://localhost:7000/api/usuario/login
Content-Type: application/json

{
  "email": "maria@example.com",
  "senha": "senha123"
}
```

**Passo 2: Tentar Criar Estabelecimento**
```http
POST https://localhost:7000/api/estabelecimento
Authorization: Bearer {token-do-usuario-comum}
Content-Type: application/json

{
  "nome": "Teste",
  "descricao": "Não deve funcionar",
  "categoria": "Teste",
  "latitude": 0,
  "longitude": 0
}
```

**Response Esperada (400 Bad Request):**
```json
{
  "mensagem": "Apenas usuários do tipo Empresa podem cadastrar estabelecimentos"
}
```

---

### ❌ Cenário 7: Tentar Criar Sem Autenticação (Falha)

**Request:**
```http
POST https://localhost:7000/api/estabelecimento
Content-Type: application/json

{
  "nome": "Teste",
  "descricao": "Sem token",
  "categoria": "Teste",
  "latitude": 0,
  "longitude": 0
}
```

**Response Esperada (401 Unauthorized):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7235#section-3.1",
  "title": "Unauthorized",
  "status": 401
}
```

---

## 🐍 Script Python para Testes Automatizados

```python
import requests
import json

BASE_URL = "https://localhost:7000"

# Ignorar SSL warnings em desenvolvimento
requests.packages.urllib3.disable_warnings()

class TesteTiposUsuario:
    def __init__(self):
        self.token_comum = None
        self.token_empresa = None

    def test_1_criar_usuario_comum(self):
        print("\n1️⃣ Testando: Criar Usuário Comum")
        url = f"{BASE_URL}/api/usuario"
        data = {
            "nome": "Teste Comum",
            "email": "comum@test.com",
            "senha": "senha123",
            "tipoUsuario": 1
        }
        response = requests.post(url, json=data, verify=False)
        print(f"Status: {response.status_code}")
        print(f"Response: {json.dumps(response.json(), indent=2)}")
        assert response.status_code == 201
        assert response.json()["tipoUsuario"] == 1
        print("✅ Sucesso!")

    def test_2_criar_usuario_empresa(self):
        print("\n2️⃣ Testando: Criar Usuário Empresa")
        url = f"{BASE_URL}/api/usuario"
        data = {
            "nome": "Teste Empresa Ltda",
            "email": "empresa@test.com",
            "senha": "senha123",
            "tipoUsuario": 2
        }
        response = requests.post(url, json=data, verify=False)
        print(f"Status: {response.status_code}")
        print(f"Response: {json.dumps(response.json(), indent=2)}")
        assert response.status_code == 201
        assert response.json()["tipoUsuario"] == 2
        print("✅ Sucesso!")

    def test_3_tipo_invalido(self):
        print("\n3️⃣ Testando: TipoUsuario Inválido")
        url = f"{BASE_URL}/api/usuario"
        data = {
            "nome": "Teste",
            "email": "invalido@test.com",
            "senha": "senha123",
            "tipoUsuario": 99
        }
        response = requests.post(url, json=data, verify=False)
        print(f"Status: {response.status_code}")
        print(f"Response: {json.dumps(response.json(), indent=2)}")
        assert response.status_code == 400
        print("✅ Validação funcionou!")

    def test_4_login_comum(self):
        print("\n4️⃣ Testando: Login Usuário Comum")
        url = f"{BASE_URL}/api/usuario/login"
        data = {
            "email": "comum@test.com",
            "senha": "senha123"
        }
        response = requests.post(url, json=data, verify=False)
        print(f"Status: {response.status_code}")
        if response.status_code == 200:
            self.token_comum = response.json()["token"]
            print(f"Token obtido: {self.token_comum[:50]}...")
            print("✅ Sucesso!")
        else:
            print(f"Response: {response.json()}")

    def test_5_login_empresa(self):
        print("\n5️⃣ Testando: Login Usuário Empresa")
        url = f"{BASE_URL}/api/usuario/login"
        data = {
            "email": "empresa@test.com",
            "senha": "senha123"
        }
        response = requests.post(url, json=data, verify=False)
        print(f"Status: {response.status_code}")
        if response.status_code == 200:
            self.token_empresa = response.json()["token"]
            print(f"Token obtido: {self.token_empresa[:50]}...")
            print("✅ Sucesso!")
        else:
            print(f"Response: {response.json()}")

    def test_6_empresa_cria_estabelecimento(self):
        print("\n6️⃣ Testando: Empresa Cria Estabelecimento")
        if not self.token_empresa:
            print("❌ Faça login primeiro!")
            return

        url = f"{BASE_URL}/api/estabelecimento"
        headers = {"Authorization": f"Bearer {self.token_empresa}"}
        data = {
            "nome": "Estabelecimento Teste",
            "descricao": "Descrição teste",
            "categoria": "Restaurante",
            "latitude": -23.5505,
            "longitude": -46.6333
        }
        response = requests.post(url, json=data, headers=headers, verify=False)
        print(f"Status: {response.status_code}")
        print(f"Response: {json.dumps(response.json(), indent=2)}")
        assert response.status_code == 201
        print("✅ Sucesso!")

    def test_7_comum_nao_pode_criar(self):
        print("\n7️⃣ Testando: Usuário Comum NÃO Pode Criar")
        if not self.token_comum:
            print("❌ Faça login primeiro!")
            return

        url = f"{BASE_URL}/api/estabelecimento"
        headers = {"Authorization": f"Bearer {self.token_comum}"}
        data = {
            "nome": "Não Deve Funcionar",
            "descricao": "Teste",
            "categoria": "Teste",
            "latitude": 0,
            "longitude": 0
        }
        response = requests.post(url, json=data, headers=headers, verify=False)
        print(f"Status: {response.status_code}")
        print(f"Response: {json.dumps(response.json(), indent=2)}")
        assert response.status_code == 400
        assert "Apenas usuários do tipo Empresa" in response.json()["mensagem"]
        print("✅ Validação funcionou corretamente!")

    def test_8_sem_autenticacao(self):
        print("\n8️⃣ Testando: Sem Autenticação")
        url = f"{BASE_URL}/api/estabelecimento"
        data = {
            "nome": "Sem Token",
            "descricao": "Teste",
            "categoria": "Teste",
            "latitude": 0,
            "longitude": 0
        }
        response = requests.post(url, json=data, verify=False)
        print(f"Status: {response.status_code}")
        assert response.status_code == 401
        print("✅ Autenticação obrigatória!")

    def executar_todos(self):
        print("=" * 60)
        print("🧪 INICIANDO TESTES DE TIPOS DE USUÁRIO")
        print("=" * 60)

        try:
            self.test_1_criar_usuario_comum()
            self.test_2_criar_usuario_empresa()
            self.test_3_tipo_invalido()
            self.test_4_login_comum()
            self.test_5_login_empresa()
            self.test_6_empresa_cria_estabelecimento()
            self.test_7_comum_nao_pode_criar()
            self.test_8_sem_autenticacao()

            print("\n" + "=" * 60)
            print("✅ TODOS OS TESTES PASSARAM!")
            print("=" * 60)
        except AssertionError as e:
            print(f"\n❌ TESTE FALHOU: {e}")
        except Exception as e:
            print(f"\n❌ ERRO: {e}")

if __name__ == "__main__":
    teste = TesteTiposUsuario()
    teste.executar_todos()
```

---

## 🧪 Executando os Testes

### Via Python
```bash
python test_tipos_usuario.py
```

### Via Postman/Insomnia
1. Importe a collection
2. Execute os requests na ordem
3. Verifique os status codes esperados

### Via Swagger
1. Execute a API
2. Acesse `/swagger`
3. Teste cada endpoint manualmente

---

## ✅ Checklist de Validação

- [ ] Usuário Comum criado com sucesso
- [ ] Usuário Empresa criado com sucesso
- [ ] TipoUsuario inválido é rejeitado
- [ ] Login retorna token com claim "tipo"
- [ ] Empresa pode criar estabelecimento
- [ ] Usuário Comum NÃO pode criar estabelecimento
- [ ] Endpoint protegido exige autenticação
- [ ] Banco de dados atualizado corretamente

---

## 📊 Resultados Esperados

| Teste | Endpoint | Tipo Usuário | Status | Resultado |
|-------|----------|--------------|--------|-----------|
| 1 | POST /usuario | Comum | 201 | ✅ Criado |
| 2 | POST /usuario | Empresa | 201 | ✅ Criado |
| 3 | POST /usuario | Inválido (99) | 400 | ✅ Rejeitado |
| 4 | POST /login | Comum | 200 | ✅ Token com tipo=1 |
| 5 | POST /login | Empresa | 200 | ✅ Token com tipo=2 |
| 6 | POST /estabelecimento | Empresa | 201 | ✅ Criado |
| 7 | POST /estabelecimento | Comum | 400 | ✅ Rejeitado |
| 8 | POST /estabelecimento | Sem Auth | 401 | ✅ Não autorizado |

---

**Pronto para testar! 🚀**
