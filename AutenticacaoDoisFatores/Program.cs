using AutenticacaoDoisFatores.Core.Repositorios;
using AutenticacaoDoisFatores.Core.Servicos;
using AutenticacaoDoisFatores.Core.Servicos.Interfaces;
using AutenticacaoDoisFatores.Infra.Contexto;
using AutenticacaoDoisFatores.Infra.Repositorios;
using AutenticacaoDoisFatores.Servico;
using AutenticacaoDoisFatores.Servico.Interfaces;
using AutenticacaoDoisFatores.Servico.Mapeadores;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AutenticacaoDoisFatoresContexto>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("ADFConnection"))
);

builder.Services.AddTransient<IEntidadeAcessoRepositorio, EntidadeAcessoRepositorio>();

builder.Services.AddTransient<IEntidadeAcessoDominio, EntidadeAcessoDominio>();

builder.Services.AddTransient<IEntidadeAcessoServico, EntidadeAcessoServico>();

var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<EntidadeAcessoMapeamento>();
});
var mapeamentos = config.CreateMapper();
builder.Services.AddSingleton(mapeamentos);

builder.Services.AddScoped<INotificador, Notificador>();

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
