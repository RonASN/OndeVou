using Microsoft.EntityFrameworkCore;
using OndeVou.Application.Interfaces;
using OndeVou.Application.Services;
using OndeVou.Domain.Interfaces;
using OndeVou.Infrastructure.Data;
using OndeVou.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        x => x.UseNetTopologySuite()
    )
);

// Repositories
builder.Services.AddScoped<IEstabelecimentoRepository, EstabelecimentoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// Services
builder.Services.AddScoped<IEstabelecimentoService, EstabelecimentoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.CanConnect();
}

app.Run();
