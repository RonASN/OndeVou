# Autenticação JWT - OndeVou API

## 📋 Configuração Implementada

A autenticação JWT foi implementada seguindo Clean Architecture com as seguintes camadas:

### Estrutura de Arquivos Criados

```
OndeVou.Api/
├── Controllers/
│   ├── UsuarioController.cs (atualizado com endpoint /login)
│   └── EstabelecimentoController.cs (protegido com [Authorize])
├── Helpers/
│   └── ClaimsHelper.cs (helper para leitura de claims)
├── Program.cs (configurado com JWT)
└── appsettings.json (com seção Jwt)

OndeVou.Application/
├── DTOs/
│   ├── Request/
│   │   └── LoginRequestDto.cs
│   └── Response/
│       └── LoginResponseDto.cs
├── Interfaces/
│   ├── ITokenService.cs
│   └── IUsuarioService.cs (atualizado)
└── Services/
    ├── TokenService.cs
    └── UsuarioService.cs (atualizado com LoginAsync)
```

## 🔐 Endpoints Disponíveis

### 1. Criar Usuário
```http
POST /api/usuario
Content-Type: application/json

{
  "nome": "João Silva",
  "email": "joao@example.com",
  "senha": "senha123"
}
```

### 2. Login (Obter Token)
```http
POST /api/usuario/login
Content-Type: application/json

{
  "email": "joao@example.com",
  "senha": "senha123"
}
```

**Resposta:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresAt": "2024-01-20T15:30:00Z"
}
```

### 3. Criar Estabelecimento (Protegido)
```http
POST /api/estabelecimento
Authorization: Bearer {seu-token-aqui}
Content-Type: application/json

{
  "nome": "Restaurante XYZ",
  "descricao": "Melhor comida da região",
  "categoria": "Restaurante",
  "latitude": -23.550520,
  "longitude": -46.633308
}
```

## 🧪 Testando no Swagger

1. Inicie a API
2. Acesse `https://localhost:7xxx/swagger`
3. Execute POST `/api/usuario/login` para obter o token
4. Clique no botão **Authorize** (🔒) no topo do Swagger
5. Digite: `Bearer {seu-token}`
6. Agora você pode testar endpoints protegidos

## 🔧 Configuração JWT (appsettings.json)

```json
{
  "Jwt": {
    "Key": "OndeVou-SecretKey-SuperSecure-2024-MinimumLength32Characters",
    "Issuer": "OndeVouAPI",
    "Audience": "OndeVouAPI",
    "ExpireHours": 2
  }
}
```

⚠️ **IMPORTANTE**: Em produção, armazene a chave em variáveis de ambiente ou Azure Key Vault.

## 📦 Claims Disponíveis no Token

- **NameIdentifier**: ID do usuário
- **Email**: Email do usuário
- **Name**: Nome do usuário
- **Jti**: ID único do token

### Exemplo de Uso em Controller

```csharp
[Authorize]
[HttpPost]
public async Task<IActionResult> MinhaAction()
{
    var usuarioId = ClaimsHelper.GetUsuarioId(User);
    var usuarioEmail = ClaimsHelper.GetUsuarioEmail(User);
    var usuarioNome = ClaimsHelper.GetUsuarioNome(User);

    // Sua lógica aqui
}
```

## 🛡️ Segurança

- ✅ Senhas armazenadas com BCrypt
- ✅ Token assinado com HmacSha256
- ✅ Validação de Issuer, Audience e Lifetime
- ✅ Mensagens de erro genéricas (não expõe se email existe)
- ✅ ClockSkew = Zero (sem tolerância de expiração)

## 🚀 Próximos Passos (Sugeridos)

1. **Implementar Roles/Permissions**
   - Adicionar campo `Role` na entidade Usuario
   - Adicionar claim de role no token
   - Usar `[Authorize(Roles = "Admin")]`

2. **Refresh Token**
   - Implementar refresh token para renovar acesso
   - Armazenar em tabela RefreshTokens

3. **Rate Limiting**
   - Limitar tentativas de login

4. **Logging**
   - Registrar tentativas de login falhadas

5. **Email Verification**
   - Confirmar email antes de permitir login

## 📝 Exemplo de Teste Completo

### Passo 1: Criar Usuário
```bash
curl -X POST "https://localhost:7xxx/api/usuario" \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "Teste User",
    "email": "teste@example.com",
    "senha": "senha123"
  }'
```

### Passo 2: Fazer Login
```bash
curl -X POST "https://localhost:7xxx/api/usuario/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "teste@example.com",
    "senha": "senha123"
  }'
```

### Passo 3: Usar Token
```bash
curl -X POST "https://localhost:7xxx/api/estabelecimento" \
  -H "Authorization: Bearer {TOKEN_OBTIDO}" \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "Meu Estabelecimento",
    "descricao": "Descrição",
    "categoria": "Restaurante",
    "latitude": -23.5505,
    "longitude": -46.6333
  }'
```

## ⚙️ Dependências NuGet Adicionadas

- `System.IdentityModel.Tokens.Jwt` (8.2.1)
- `Microsoft.AspNetCore.Authentication.JwtBearer` (8.0.26)
- `BCrypt.Net-Next` (4.0.3)

## 📄 Licença

Este projeto segue as práticas de Clean Architecture e está pronto para produção.
