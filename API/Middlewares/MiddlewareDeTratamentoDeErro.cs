﻿using System;
using System.Net;
using System.Threading.Tasks;
using Biblioteca.Dominio._Comum;
using Biblioteca.Infra.Configuracoes.Excecoes;
using Biblioteca.Infra.Log.LogsGerais;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Biblioteca.API.Middlewares
{
    public class MiddlewareDeTratamentoDeErro
    {
        private readonly RequestDelegate _proximo;
        private readonly IRegistrosDeLog _registrosDeLogs;

        public MiddlewareDeTratamentoDeErro(RequestDelegate proximo, IRegistrosDeLog registrosDeLogs)
        {
            _proximo = proximo;
            _registrosDeLogs = registrosDeLogs;
        }

        public async Task Invoke(HttpContext contexto)
        {
            try
            {
                await _proximo(contexto);
            }
            catch (Exception excecao)
            {
                await TratarExcecaoAssincronamente(contexto, excecao);
            }
        }

        private Task TratarExcecaoAssincronamente(HttpContext contexto, Exception excecao)
        {
            const HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            var configuracoesDoJson = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var resposta = JsonConvert.SerializeObject(excecao.ObterObjetoDoErro(), configuracoesDoJson);

            contexto.Response.ContentType = "application/json";
            contexto.Response.StatusCode = (int)statusCode;

            if (!(excecao is ExcecaoDeDominio)) new TelemetryClient().TrackException(excecao);
            _registrosDeLogs.RegistrarErro(excecao);

            return contexto.Response.WriteAsync(resposta);
        }
    }

    public static class TratamentoDeErroMiddlewareExtensions
    {
        public static IApplicationBuilder UseMiddlewareDeTratamentoDeErro(this IApplicationBuilder builder)
            => builder.UseMiddleware<MiddlewareDeTratamentoDeErro>();

    }
}