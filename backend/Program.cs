using System.Text.Json.Serialization;
using backend.Data;
using backend.Hubs;
using backend.Repositories.Implementations;
using backend.Repositories.Interfaces;
using backend.Services.Implementations;
using backend.Services.Implementations.Util;
using backend.Services.Interfaces;
using backend.Services.Interfaces.Util;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            ReferenceHandler.IgnoreCycles;
    });;
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:4200",
                "https://localhost:4200"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");
var serverVersion = ServerVersion.AutoDetect(connectionString);

builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseMySql(connectionString, serverVersion));

// Services e Repositories
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDesafioRepository, DesafioRepository>();
builder.Services.AddScoped<IDesafioService, DesafioService>();
builder.Services.AddScoped<ISenhaService, SenhaService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

//NotificationService e SignalR
builder.Services.AddSingleton<INotificationService, NotificationService>();

builder.Services.AddSignalR();

// DesafiosBackupService
builder.Services.AddScoped<IDesafiosBackupService, DesafiosBackupService>();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseCors("Frontend");
app.MapControllers();

app.MapHub<NotificationHub>("/notifications");

app.Run();

