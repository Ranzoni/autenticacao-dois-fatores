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
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var root = Directory.GetCurrentDirectory();
var dotenv = Path.Combine(root, ".env");
EnvConfig.Carregar(dotenv);

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

builder.Services.AddTransient<IEntidadeAcessoDominio, EntidadeAcessoDominio>();

builder.Services.AddTransient<IEmailServico, EmailServico>();

builder.Services.AddTransient<IEntidadeAcessoServico, EntidadeAcessoServico>();
builder.Services.AddTransient<EntidadeAcessoServicoValidacao>();

var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<EntidadeAcessoMapeamento>();
});
var mapeamentos = config.CreateMapper();
builder.Services.AddSingleton(mapeamentos);

builder.Services.AddScoped<INotificadorServico, NotificadorServico>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
