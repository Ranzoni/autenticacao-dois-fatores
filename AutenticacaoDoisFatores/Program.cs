using AutenticacaoDoisFatores.Core.Repositorios;
using AutenticacaoDoisFatores.Core.Servicos;
using AutenticacaoDoisFatores.Core.Servicos.Interfaces;
using AutenticacaoDoisFatores.Infra;
using AutenticacaoDoisFatores.Infra.Contexto;
using AutenticacaoDoisFatores.Infra.Repositorios;
using AutenticacaoDoisFatores.Servico.Mapeadores;
using AutenticacaoDoisFatores.Servico.Servicos;
using AutenticacaoDoisFatores.Servico.Servicos.Interfaces;
using AutenticacaoDoisFatores.Servico.Validacoes;
using AutoMapper;
using Mensageiro;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


var root = Directory.GetCurrentDirectory();
var dotenv = Path.Combine(root, ".env");
EnvConfig.Carregar(dotenv);

var chaveSecreta = Environment.GetEnvironmentVariable("TOKEN_KEY") ?? "";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveSecreta))
        };
    });

var host = Environment.GetEnvironmentVariable("ADF_HOST");
var database = Environment.GetEnvironmentVariable("ADF_DATABASE");
var username = Environment.GetEnvironmentVariable("ADF_USERNAME") + ";";
var password = Environment.GetEnvironmentVariable("ADF_PASSWORD") + ";";
var sslMode = Environment.GetEnvironmentVariable("ADF_SSLMODE") + ";";
var trustServerCertificate = Environment.GetEnvironmentVariable("ADF_TRUST_SERVER_CERTIFICATE");

var connectionString = $"Host={host};Database={database};Username={username};Password={password};SSL Mode={sslMode};Trust Server Certificate={trustServerCertificate}";

builder.Services.AddDbContext<AutenticacaoDoisFatoresContexto>(opt =>
    opt.UseNpgsql(connectionString)
);

builder.Services.AddTransient<IEntidadeAcessoRepositorio, EntidadeAcessoRepositorio>();
builder.Services.AddTransient<IUsuarioRepositorio, UsuarioRepositorio>();

builder.Services.AddTransient<IEntidadeAcessoDominio, EntidadeAcessoDominio>();
builder.Services.AddTransient<IUsuarioDominio, UsuarioDominio>();

builder.Services.AddTransient<IEmailServico, EmailServico>();

builder.Services.AddTransient<IEntidadeAcessoServico, EntidadeAcessoServico>();
builder.Services.AddTransient<EntidadeAcessoServicoValidacao>();
builder.Services.AddTransient<IUsuarioServico, UsuarioServico>();
builder.Services.AddTransient<UsuarioServicoValidacao>();
builder.Services.AddTransient<IAutenticacaoServico, AutenticacaoServico>();
builder.Services.AddTransient<AutenticacaoServicoValidacao>();

var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<EntidadeAcessoMapeamento>();
    cfg.AddProfile<UsuarioMapeamento>();
});
var mapeamentos = config.CreateMapper();
builder.Services.AddSingleton(mapeamentos);

builder.Services.AddScoped<INotificador, Notificador>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
