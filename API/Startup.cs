using Biblioteca.API.ConfiguracoesDeInicializacao;
using Biblioteca.API.Middlewares;
using Biblioteca.Infra.Configuracoes.Orm.Permissionamento;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.AspNetCore.Identity;
using NHibernate.Cfg;

namespace Biblioteca.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var cfg = new Configuration();

            ConfiguracaoDoMvc.Configurar(services);
            ConfiguracaoDeAutenticacao.Configurar(services, Configuration);
            ConfiguracaoDoApplicationInsights.Configurar(services, Configuration);
            ConfiguracaoDeInjecaoDeDependencia.Configurar(services, Configuration);
            ConfiguracaoDeSessaoDoBanco.Configurar(services, Configuration);

            cfg.AddIdentityMappingsForSqlServer();

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredUniqueChars = 1;
                })
                .AddHibernateStores();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            TelemetryConfiguration.Active.InstrumentationKey = Configuration["ApplicationInsights"];
            app.UsePathBase(Configuration["RotaBase"]);
            app.UseAuthentication();
            app.UseRequestLocalization();
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseMiddlewareDeTratamentoDeErro();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}