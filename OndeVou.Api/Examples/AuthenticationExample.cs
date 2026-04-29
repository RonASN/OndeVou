// Exemplo de classe para testes de autenticação JWT
// Este é um exemplo de como você poderia estruturar testes para a API

using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace OndeVou.Api.Examples;

/// <summary>
/// Exemplo de cliente para testar a autenticação JWT
/// </summary>
public class AuthenticationExample
{
    private readonly HttpClient _httpClient;
    private string? _token;

    public AuthenticationExample(string baseUrl)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseUrl)
        };
    }

    /// <summary>
    /// Exemplo 1: Criar um novo usuário
    /// </summary>
    public async Task<bool> CriarUsuarioExemploAsync()
    {
        var request = new
        {
            nome = "João Silva",
            email = "joao@example.com",
            senha = "senha123"
        };

        var response = await _httpClient.PostAsJsonAsync("/api/usuario", request);
        return response.IsSuccessStatusCode;
    }

    /// <summary>
    /// Exemplo 2: Fazer login e obter token
    /// </summary>
    public async Task<string?> LoginExemploAsync()
    {
        var request = new
        {
            email = "joao@example.com",
            senha = "senha123"
        };

        var response = await _httpClient.PostAsJsonAsync("/api/usuario/login", request);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var loginResponse = JsonSerializer.Deserialize<LoginResponse>(
                content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            _token = loginResponse?.Token;
            return _token;
        }

        return null;
    }

    /// <summary>
    /// Exemplo 3: Usar token para acessar endpoint protegido
    /// </summary>
    public async Task<bool> CriarEstabelecimentoProtegidoAsync()
    {
        if (string.IsNullOrEmpty(_token))
        {
            throw new InvalidOperationException("Faça login primeiro para obter o token!");
        }

        // Adicionar token no header Authorization
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _token);

        var request = new
        {
            nome = "Restaurante Exemplo",
            descricao = "Melhor comida da cidade",
            categoria = "Restaurante",
            latitude = -23.550520,
            longitude = -46.633308
        };

        var response = await _httpClient.PostAsJsonAsync("/api/estabelecimento", request);
        return response.IsSuccessStatusCode;
    }

    /// <summary>
    /// Exemplo 4: Tentar acessar endpoint protegido sem token (deve falhar)
    /// </summary>
    public async Task<HttpStatusCode> TentarAcessoSemTokenAsync()
    {
        // Remover token do header
        _httpClient.DefaultRequestHeaders.Authorization = null;

        var request = new
        {
            nome = "Teste",
            descricao = "Teste",
            categoria = "Teste",
            latitude = 0.0,
            longitude = 0.0
        };

        var response = await _httpClient.PostAsJsonAsync("/api/estabelecimento", request);
        return response.StatusCode; // Deve retornar 401 Unauthorized
    }

    /// <summary>
    /// Exemplo 5: Tentar login com credenciais inválidas
    /// </summary>
    public async Task<HttpStatusCode> TentarLoginInvalidoAsync()
    {
        var request = new
        {
            email = "joao@example.com",
            senha = "senhaErrada123"
        };

        var response = await _httpClient.PostAsJsonAsync("/api/usuario/login", request);
        return response.StatusCode; // Deve retornar 401 Unauthorized
    }

    private class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}

/// <summary>
/// Exemplo de uso completo
/// </summary>
public class ExemploUsoCompleto
{
    public static async Task ExecutarExemploCompletoAsync()
    {
        var client = new AuthenticationExample("https://localhost:7000");

        // 1. Criar usuário
        Console.WriteLine("1. Criando usuário...");
        var usuarioCriado = await client.CriarUsuarioExemploAsync();
        Console.WriteLine($"   Usuário criado: {usuarioCriado}");

        // 2. Fazer login
        Console.WriteLine("\n2. Fazendo login...");
        var token = await client.LoginExemploAsync();
        Console.WriteLine($"   Token obtido: {token?[..20]}...");

        // 3. Acessar endpoint protegido COM token
        Console.WriteLine("\n3. Criando estabelecimento (com token)...");
        var estabelecimentoCriado = await client.CriarEstabelecimentoProtegidoAsync();
        Console.WriteLine($"   Estabelecimento criado: {estabelecimentoCriado}");

        // 4. Tentar acessar SEM token
        Console.WriteLine("\n4. Tentando acessar sem token...");
        var statusSemToken = await client.TentarAcessoSemTokenAsync();
        Console.WriteLine($"   Status retornado: {statusSemToken} (esperado: 401 Unauthorized)");

        // 5. Tentar login inválido
        Console.WriteLine("\n5. Tentando login com senha errada...");
        var statusLoginInvalido = await client.TentarLoginInvalidoAsync();
        Console.WriteLine($"   Status retornado: {statusLoginInvalido} (esperado: 401 Unauthorized)");
    }
}
