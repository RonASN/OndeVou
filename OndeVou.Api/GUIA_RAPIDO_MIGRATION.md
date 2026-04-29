# 🚀 GUIA RÁPIDO - Aplicar Migration

## ⚠️ IMPORTANTE: Execute ANTES de rodar a API

---

## 📋 Pré-requisitos

- PostgreSQL rodando
- Banco de dados `onde_vou_db` criado
- Connection string configurada no appsettings.json

---

## 🎯 Método 1: Via EF Core CLI (Recomendado)

### Passo 1: Abrir Terminal
```bash
cd C:\Users\ronna\Desktop\Projetos\OndeVouApi\OndeVou\OndeVou.Infrastructure
```

### Passo 2: Verificar Conexão
```bash
dotnet ef database update --dry-run --startup-project ..\OndeVou.Api --context AppDbContext
```

### Passo 3: Aplicar Migration
```bash
dotnet ef database update --startup-project ..\OndeVou.Api --context AppDbContext
```

### ✅ Resultado Esperado
```
Build started...
Build succeeded.
Applying migration '20260429053926_AddTipoUsuarioERelacao'.
Done.
```

---

## 🎯 Método 2: Via SQL Manual (Se CLI não funcionar)

### Passo 1: Abrir pgAdmin ou psql

### Passo 2: Conectar ao Banco
```sql
\c onde_vou_db
```

### Passo 3: Executar o Script
```sql
-- Copie todo o conteúdo de:
-- OndeVou.Infrastructure/Migrations/AddTipoUsuarioERelacao.sql

BEGIN;

ALTER TABLE usuarios 
ADD COLUMN "TipoUsuario" INTEGER NOT NULL DEFAULT 1;

ALTER TABLE estabelecimentos 
ADD COLUMN "UsuarioId" INTEGER NOT NULL DEFAULT 0;

CREATE INDEX "IX_estabelecimentos_UsuarioId" 
ON estabelecimentos ("UsuarioId");

ALTER TABLE estabelecimentos 
ADD CONSTRAINT "FK_estabelecimentos_usuarios_UsuarioId" 
FOREIGN KEY ("UsuarioId") 
REFERENCES usuarios ("Id") 
ON DELETE RESTRICT;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260429053926_AddTipoUsuarioERelacao', '8.0.26');

COMMIT;
```

### ✅ Verificar
```sql
-- Verificar coluna TipoUsuario
SELECT column_name, data_type, is_nullable 
FROM information_schema.columns 
WHERE table_name = 'usuarios' AND column_name = 'TipoUsuario';

-- Verificar coluna UsuarioId
SELECT column_name, data_type, is_nullable 
FROM information_schema.columns 
WHERE table_name = 'estabelecimentos' AND column_name = 'UsuarioId';

-- Verificar foreign key
SELECT conname 
FROM pg_constraint 
WHERE conname = 'FK_estabelecimentos_usuarios_UsuarioId';
```

---

## 🔍 Verificação Pós-Migration

### 1. Verificar Tabelas
```sql
-- Estrutura de usuarios
\d usuarios

-- Deve mostrar:
-- TipoUsuario | integer | not null | 1
```

```sql
-- Estrutura de estabelecimentos
\d estabelecimentos

-- Deve mostrar:
-- UsuarioId | integer | not null
-- Foreign Key: FK_estabelecimentos_usuarios_UsuarioId
```

### 2. Verificar Migrations History
```sql
SELECT * FROM "__EFMigrationsHistory" 
ORDER BY "MigrationId" DESC;

-- Deve aparecer: 20260429053926_AddTipoUsuarioERelacao
```

---

## 🛠️ Resolver Problemas Comuns

### ❌ Erro: "Foreign key violation"
**Causa**: Você tem estabelecimentos sem UsuarioId válido

**Solução**:
```sql
-- Antes de aplicar a FK, atualizar estabelecimentos órfãos
UPDATE estabelecimentos 
SET "UsuarioId" = (SELECT "Id" FROM usuarios LIMIT 1) 
WHERE "UsuarioId" = 0;
```

---

### ❌ Erro: "Column already exists"
**Causa**: Migration já foi aplicada parcialmente

**Solução**:
```sql
-- Verificar o que existe
SELECT column_name FROM information_schema.columns 
WHERE table_name = 'usuarios' AND column_name = 'TipoUsuario';

-- Se já existe, apenas adicionar no history:
INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260429053926_AddTipoUsuarioERelacao', '8.0.26');
```

---

### ❌ Erro: "Could not load assembly"
**Causa**: Política de segurança do Windows

**Solução**: Use o Método 2 (SQL Manual)

---

## 📊 Dados de Teste

### Criar Usuários de Teste
```sql
-- Usuário Comum
INSERT INTO usuarios ("Nome", "Email", "SenhaHash", "TipoUsuario", "DataCriacao")
VALUES (
  'Usuário Comum Teste',
  'comum@test.com',
  '$2a$11$hash...', -- Use o hash BCrypt real
  1,
  NOW()
);

-- Usuário Empresa
INSERT INTO usuarios ("Nome", "Email", "SenhaHash", "TipoUsuario", "DataCriacao")
VALUES (
  'Empresa Teste Ltda',
  'empresa@test.com',
  '$2a$11$hash...', -- Use o hash BCrypt real
  2,
  NOW()
);
```

### Atualizar Usuários Existentes
```sql
-- Definir usuários existentes como Empresa
UPDATE usuarios 
SET "TipoUsuario" = 2 
WHERE "Email" IN (
  'empresa1@example.com',
  'empresa2@example.com'
);

-- Resto como Comum (padrão já é 1)
```

---

## ✅ Checklist Final

- [ ] Migration aplicada com sucesso
- [ ] Coluna `TipoUsuario` existe em `usuarios`
- [ ] Coluna `UsuarioId` existe em `estabelecimentos`
- [ ] Foreign Key criada
- [ ] Índice criado
- [ ] Entry adicionada em `__EFMigrationsHistory`
- [ ] Usuários existentes atualizados (se necessário)
- [ ] Estabelecimentos órfãos vinculados (se necessário)

---

## 🚀 Próximo Passo

### Rodar a API
```bash
cd ..\OndeVou.Api
dotnet run
```

### Testar
```bash
# Abrir navegador em:
https://localhost:7000/swagger

# Ou executar testes:
python GUIA_TESTES_TIPOS_USUARIO.py
```

---

## 📞 Em Caso de Problemas

1. **Verificar logs do PostgreSQL**
   ```bash
   # Linux/Mac
   tail -f /var/log/postgresql/postgresql-*.log

   # Windows
   # Verificar Event Viewer
   ```

2. **Verificar connection string**
   ```json
   // appsettings.Development.json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Port=5432;Database=onde_vou_db;Username=postgres;Password=postgres"
   }
   ```

3. **Reverter migration (se necessário)**
   ```bash
   # Via CLI
   dotnet ef database update PreviousMigrationName --startup-project ..\OndeVou.Api

   # Ou SQL manual (ver comentário no script)
   ```

---

## 🎉 Pronto!

Após aplicar a migration com sucesso, sua API está pronta para:
- ✅ Diferenciar usuários Comum e Empresa
- ✅ Permitir apenas empresas cadastrarem estabelecimentos
- ✅ Vincular estabelecimentos aos donos

**Boa sorte! 🚀**
