﻿using Biblioteca.API.ConfiguracoesDeInicializacao;
using Biblioteca.API.Middlewares;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            ConfiguracaoDoMvc.Configurar(services);
            ConfiguracaoDeAutenticacao.Configurar(services, Configuration);
            ConfiguracaoDoApplicationInsights.Configurar(services, Configuration);
            ConfiguracaoDeInjecaoDeDependencia.Configurar(services, Configuration);
            ConfiguracaoDeSessaoDoBanco.Configurar(services, Configuration);
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