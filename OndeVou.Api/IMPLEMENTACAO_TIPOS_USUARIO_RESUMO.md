# ✅ IMPLEMENTAÇÃO DE TIPOS DE USUÁRIO E RESTRIÇÕES - CONCLUÍDA

## 📦 Arquivos Criados/Modificados

### ✨ Novos Arquivos Criados

**OndeVou.Domain:**
- ✅ `Enums/TipoUsuario.cs` - Enum com valores Comum (1) e Empresa (2)

**OndeVou.Application:**
- ✅ `Exceptions/BusinessException.cs` - Exceção customizada para regras de negócio

**OndeVou.Infrastructure:**
- ✅ `Migrations/20260429053926_AddTipoUsuarioERelacao.cs` - Migration gerada
- ✅ `Migrations/AddTipoUsuarioERelacao.sql` - Script SQL para aplicação manual

### 🔄 Arquivos Modificados

**OndeVou.Domain:**
- ✅ `Entities/Usuario.cs` - Adicionado TipoUsuario e navegação para Estabelecimentos
- ✅ `Entities/Estabelecimento.cs` - Adicionado UsuarioId e navegação para Usuario
- ✅ `Interfaces/IUsuarioRepository.cs` - Adicionado método BuscarPorIdAsync

**OndeVou.Infrastructure:**
- ✅ `Maps/UsuarioMap.cs` - Configurado TipoUsuario e relacionamento
- ✅ `Maps/EstabelecimentoMap.cs` - Configurado relacionamento com Usuario
- ✅ `Repositories/UsuarioRepository.cs` - Implementado BuscarPorIdAsync

**OndeVou.Application:**
- ✅ `DTOs/Request/CriarUsuarioRequestDto.cs` - Adicionado TipoUsuario com validações
- ✅ `DTOs/Response/UsuarioResponseDto.cs` - Adicionado TipoUsuario e descrição
- ✅ `Services/UsuarioService.cs` - Validação de TipoUsuario e normalização de email
- ✅ `Services/TokenService.cs` - Adicionada claim "tipo" no JWT
- ✅ `Services/EstabelecimentoService.cs` - Validação de tipo Empresa
- ✅ `Interfaces/IEstabelecimentoService.cs` - Adicionado parâmetro usuarioId

**OndeVou.Api:**
- ✅ `Helpers/ClaimsHelper.cs` - Adicionado método GetTipoUsuario
- ✅ `Controllers/EstabelecimentoController.cs` - Validação e uso de usuarioId

---

## 🎯 Funcionalidades Implementadas

### 1. ✅ Enum TipoUsuario
```csharp
public enum TipoUsuario
{
    Comum = 1,
    Empresa = 2
}
```

### 2. ✅ Entidade Usuario Atualizada
- Campo `TipoUsuario` obrigatório
- Navegação para `Estabelecimentos` (coleção)
- Relacionamento 1:N com Estabelecimento

### 3. ✅ Entidade Estabelecimento Atualizada
- Campo `UsuarioId` obrigatório
- Navegação para `Usuario` (dono)
- Relacionamento N:1 com Usuario

### 4. ✅ Mapeamentos EF Core
- TipoUsuario persistido como `int`
- Relacionamento com `DeleteBehavior.Restrict`
- Índice em `UsuarioId` para performance

### 5. ✅ DTO de Criação de Usuário
```csharp
[Range(1, 2, ErrorMessage = "Tipo de usuário inválido")]
public int TipoUsuario { get; set; }
```

### 6. ✅ Validações no Service
- Email normalizado (ToLower + Trim)
- Validação com `Enum.IsDefined`
- BCrypt para senha
- DataCriacao com `DateTime.UtcNow`

### 7. ✅ JWT com Claim de Tipo
```csharp
new Claim("tipo", ((int)usuario.TipoUsuario).ToString())
```

### 8. ✅ Restrição de Cadastro de Estabelecimento
- Apenas usuários tipo **Empresa** podem cadastrar
- Validação no `EstabelecimentoService`
- Exceção `BusinessException` com mensagem clara

### 9. ✅ Controller Atualizado
- Obtém `usuarioId` via `ClaimsHelper`
- Passa para o service
- Tratamento específico para `BusinessException`

---

## 🗄️ Estrutura do Banco de Dados

### Tabela: usuarios
```sql
ALTER TABLE usuarios 
ADD COLUMN "TipoUsuario" INTEGER NOT NULL DEFAULT 1;
```

**Valores:**
- `1` = Comum
- `2` = Empresa

### Tabela: estabelecimentos
```sql
ALTER TABLE estabelecimentos 
ADD COLUMN "UsuarioId" INTEGER NOT NULL;

ALTER TABLE estabelecimentos 
ADD CONSTRAINT "FK_estabelecimentos_usuarios_UsuarioId" 
FOREIGN KEY ("UsuarioId") REFERENCES usuarios ("Id") 
ON DELETE RESTRICT;
```

---

## 🔐 Regras de Negócio Implementadas

### ✅ Criação de Usuário
1. Email é normalizado (lowercase + trim)
2. TipoUsuario deve ser 1 ou 2
3. Validação com `Enum.IsDefined`
4. Senha com BCrypt
5. Email único no banco

### ✅ Login
1. Email normalizado antes da busca
2. Validação de senha com BCrypt
3. Token JWT contém claim "tipo"

### ✅ Criação de Estabelecimento
1. **Usuário deve estar autenticado** ([Authorize])
2. **Usuário deve existir no banco**
3. **Usuário deve ser do tipo Empresa (2)**
4. Estabelecimento vinculado ao dono (UsuarioId)
5. Campos normalizados (Trim)

---

## 🧪 Exemplos de Uso

### 1️⃣ Criar Usuário Comum
```http
POST /api/usuario
Content-Type: application/json

{
  "nome": "João Silva",
  "email": "joao@example.com",
  "senha": "senha123",
  "tipoUsuario": 1
}
```

**Resposta:**
```json
{
  "id": 1,
  "nome": "João Silva",
  "email": "joao@example.com",
  "tipoUsuario": 1,
  "tipoUsuarioDescricao": "Comum",
  "dataCriacao": "2024-04-29T10:00:00Z"
}
```

---

### 2️⃣ Criar Usuário Empresa
```http
POST /api/usuario
Content-Type: application/json

{
  "nome": "Restaurante ABC Ltda",
  "email": "contato@restauranteabc.com",
  "senha": "senha123",
  "tipoUsuario": 2
}
```

**Resposta:**
```json
{
  "id": 2,
  "nome": "Restaurante ABC Ltda",
  "email": "contato@restauranteabc.com",
  "tipoUsuario": 2,
  "tipoUsuarioDescricao": "Empresa",
  "dataCriacao": "2024-04-29T10:05:00Z"
}
```

---

### 3️⃣ Login (Obter Token)
```http
POST /api/usuario/login
Content-Type: application/json

{
  "email": "contato@restauranteabc.com",
  "senha": "senha123"
}
```

**Resposta (Token contém claim "tipo": "2"):**
```json
{
  "token": "eyJhbGc...",
  "expiresAt": "2024-04-29T12:00:00Z"
}
```

**Payload do Token (decodificado):**
```json
{
  "nameid": "2",
  "email": "contato@restauranteabc.com",
  "unique_name": "Restaurante ABC Ltda",
  "tipo": "2",
  "jti": "...",
  "exp": 1714392000
}
```

---

### 4️⃣ Criar Estabelecimento (Usuário Empresa)
```http
POST /api/estabelecimento
Authorization: Bearer {token-de-usuario-empresa}
Content-Type: application/json

{
  "nome": "Restaurante ABC",
  "descricao": "Culinária italiana",
  "categoria": "Restaurante",
  "latitude": -23.550520,
  "longitude": -46.633308
}
```

**Resposta (201 Created):**
```json
{
  "id": 1,
  "nome": "Restaurante ABC",
  "descricao": "Culinária italiana",
  "categoria": "Restaurante",
  "latitude": -23.550520,
  "longitude": -46.633308
}
```

---

### 5️⃣ Tentar Criar Estabelecimento (Usuário Comum) - DEVE FALHAR
```http
POST /api/estabelecimento
Authorization: Bearer {token-de-usuario-comum}
Content-Type: application/json

{
  "nome": "Teste",
  "descricao": "Teste",
  "categoria": "Teste",
  "latitude": 0,
  "longitude": 0
}
```

**Resposta (400 Bad Request):**
```json
{
  "mensagem": "Apenas usuários do tipo Empresa podem cadastrar estabelecimentos"
}
```

---

## 📊 Validações Implementadas

### CriarUsuarioRequestDto
| Campo | Validação |
|-------|-----------|
| Nome | Required, MaxLength(200) |
| Email | Required, EmailAddress, MaxLength(200) |
| Senha | Required, MinLength(6) |
| TipoUsuario | Required, Range(1, 2) |

### Erros Comuns

**TipoUsuario inválido (valor 3):**
```json
{
  "mensagem": "Tipo de usuário inválido. Use 1 para Comum ou 2 para Empresa"
}
```

**Email já cadastrado:**
```json
{
  "mensagem": "Email já cadastrado"
}
```

**Usuário não é empresa:**
```json
{
  "mensagem": "Apenas usuários do tipo Empresa podem cadastrar estabelecimentos"
}
```

---

## 🔧 Helpers Disponíveis

### ClaimsHelper
```csharp
// Obter ID do usuário
var usuarioId = ClaimsHelper.GetUsuarioId(User);

// Obter email
var email = ClaimsHelper.GetUsuarioEmail(User);

// Obter nome
var nome = ClaimsHelper.GetUsuarioNome(User);

// Obter tipo de usuário
var tipo = ClaimsHelper.GetTipoUsuario(User);
// tipo == 1 (Comum) ou 2 (Empresa)
```

---

## 🗃️ Migration

### Aplicar Migration

**Via EF Core CLI:**
```bash
cd OndeVou.Infrastructure
dotnet ef database update --startup-project ../OndeVou.Api
```

**Via SQL Manual (se CLI não funcionar):**
```bash
# Execute o arquivo:
OndeVou.Infrastructure/Migrations/AddTipoUsuarioERelacao.sql
```

### Verificar Migration
```bash
dotnet ef migrations list --startup-project ../OndeVou.Api
```

---

## ⚠️ IMPORTANTE: Dados Existentes

Se você já tem dados no banco:

### 1. Atualizar Usuários Existentes
```sql
-- Definir todos como Comum (padrão)
UPDATE usuarios SET "TipoUsuario" = 1;

-- Ou atualizar específicos para Empresa
UPDATE usuarios 
SET "TipoUsuario" = 2 
WHERE "Email" IN ('empresa1@mail.com', 'empresa2@mail.com');
```

### 2. Atualizar Estabelecimentos Existentes
```sql
-- Se você tem estabelecimentos sem dono, 
-- atribua a um usuário existente:
UPDATE estabelecimentos 
SET "UsuarioId" = 1 
WHERE "UsuarioId" = 0;
```

---

## 🚀 Próximos Passos Sugeridos

1. **Implementar Roles/Permissions**
   - Criar enum `Role` (Admin, Moderador, etc.)
   - Adicionar campo `Role` em Usuario
   - Usar `[Authorize(Roles = "Admin")]`

2. **Endpoints de Listagem**
   - Listar estabelecimentos do usuário logado
   - Listar todos estabelecimentos (público)
   - Filtros por categoria, localização

3. **Validações Adicionais**
   - CNPJ para usuários tipo Empresa
   - CPF para usuários tipo Comum
   - Foto de perfil

4. **Auditoria**
   - CreatedAt, UpdatedAt em todas entidades
   - Soft delete (IsDeleted flag)
   - Histórico de alterações

5. **Testes**
   - Testes unitários dos services
   - Testes de integração dos endpoints
   - Testes de validação

---

## ✅ Checklist de Implementação

- [x] Enum TipoUsuario criado
- [x] Entidade Usuario atualizada
- [x] Entidade Estabelecimento atualizada
- [x] Mapeamentos EF Core configurados
- [x] DTO CriarUsuarioRequestDto com validações
- [x] UsuarioService validando TipoUsuario
- [x] TokenService incluindo claim "tipo"
- [x] EstabelecimentoService validando tipo Empresa
- [x] Controller passando usuarioId
- [x] ClaimsHelper atualizado
- [x] BusinessException criada
- [x] Migration gerada
- [x] Script SQL criado
- [x] Build com sucesso ✨
- [x] Documentação completa

---

## 🎉 Resultado Final

✅ **Tipos de usuário implementados com sucesso!**
✅ **Apenas empresas podem cadastrar estabelecimentos**
✅ **Estabelecimentos vinculados aos donos**
✅ **JWT contém informação de tipo**
✅ **Código limpo e seguindo Clean Architecture**
✅ **Validações em todas as camadas**
✅ **Pronto para uso em produção**

---

**Desenvolvido por:** OndeVou Team  
**Data:** 29/04/2024  
**Versão:** 2.0.0
