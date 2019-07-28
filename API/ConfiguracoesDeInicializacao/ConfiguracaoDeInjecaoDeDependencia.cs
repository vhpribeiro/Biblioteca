using Biblioteca.Aplicacao.Autores;
using Biblioteca.Aplicacao.Livros;
using Biblioteca.Aplicacao.Livros.Comando;
using Biblioteca.Aplicacao.Livros.Consulta;
using Biblioteca.Infra.AcessoADados.Repositorio;
using Biblioteca.Infra.Log.AlteracaoDeEntidade;
using Biblioteca.Infra.Log.LogsGerais;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;

namespace Biblioteca.API.ConfiguracoesDeInicializacao
{
    public class ConfiguracaoDeInjecaoDeDependencia
    {
        public static void Configurar(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IRegistrosDeLog>(
                provider => new RegistrosDeLogs(configuration["Data:DefaultConnection:ConnectionString"]));
            services.AddScoped<LogsDeAlteracaoDeEntidade>();
            services.AddScoped<LogadorDeAlteracaoDeEntidade>();
            services.AddScoped<SigningConfigurations>();
            services.AddScoped<TokenConfigurations>();
            services.AddScoped<IInterceptor, LogInterceptor>();
            services.AddScoped<ILivroRepositorio, LivroRepositorio>();
            services.AddScoped<IAutorRepositorio, AutorRepositorio>();
            services.AddScoped<IConsultaDeLivros, ConsultaDeLivros>();
            services.AddScoped<IControleDeQuantidadeDeLivros, ControleDeQuantidadeDeLivros>();
        }
    }
}