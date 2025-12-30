using System.Net;
using System.Text.Json;
using Backend.Features.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Backend.Features.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IOptions<Microsoft.AspNetCore.Mvc.JsonOptions> jsonOptions)
        {
            _next = next;
            _logger = logger;
            _jsonOptions = jsonOptions.Value.JsonSerializerOptions;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                await HandleExceptionAsync(context, HttpStatusCode.Unauthorized, ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                await HandleExceptionAsync(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, 
                    "Ocurrió un error interno");
            }
        }

        private Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            
            var response = ApiResponse<ResultError>.Error(message);
            var json = JsonSerializer.Serialize(response, _jsonOptions);

            return context.Response.WriteAsync(json);
        }
    }
}

/*
    Caso                    Excepción                     HTTP
    ********************************************************************
    No autorizado            UnauthorizedAccessException   401
    Datos inválidos          ArgumentException             400
    Regla de negocio         InvalidOperationException     400
    Recurso no existe        KeyNotFoundException          404
    Error interno            Exception                     500

*/