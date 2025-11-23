using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using Npgsql;
using System.Text.Json;


namespace webshop_barbie.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        // a pipeline következő eleme
        private readonly RequestDelegate _next;
        //loggoláshoz
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        //konstruktor: a pipeline következő elemét és a loggert várja
        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        //invoke metódus: minden request ide fut be
        public async Task Invoke(HttpContext context)
        {
            try
            {
                // Meghívjuk a pipeline következő elemét (controller vagy más middleware)
                await _next(context);
            }
            catch (Exception ex)
            {
                // Ha bármilyen exception történik, ide fut
                await HandleExceptionAsync(context, ex);
            }
        }

        //hibakezelés metódus
        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            //Logolás
            _logger.LogError(ex, "Unhandled exception occurred.");

            //HTTP státuszkód és üzenet alapértelmezett értékei
            int statusCode = StatusCodes.Status500InternalServerError;
            string message = "Ismeretlen hiba történt.";

            //Különböző exception típusok kezelése
            switch (ex)
            {
                case ArgumentNullException argNullEx:
                case ArgumentOutOfRangeException outOfRangeEx:
                case ArgumentException argEx:
                    statusCode = StatusCodes.Status400BadRequest;
                    message = ex.Message;
                    break;

                case InvalidOperationException invalidOpEx:
                    statusCode = StatusCodes.Status409Conflict;
                    message = invalidOpEx.Message;
                    break;

                case KeyNotFoundException keyNotFoundEx:
                    statusCode = StatusCodes.Status404NotFound;
                    message = keyNotFoundEx.Message;
                    break;

                case UnauthorizedAccessException unauthorizedEx:
                    statusCode = StatusCodes.Status401Unauthorized;
                    message = unauthorizedEx.Message;
                    break;

                case DbUpdateConcurrencyException dbConcurrencyEx:
                case DbUpdateException dbEx:
                    statusCode = StatusCodes.Status500InternalServerError;
                    message = "Adatbázis hiba történt.";
                    break;

                case NpgsqlException npgsqlEx:
                    statusCode = StatusCodes.Status500InternalServerError;
                    message = npgsqlEx.Message;
                    break;

                case TaskCanceledException taskCanceledEx:
                case TimeoutException timeoutEx:
                    statusCode = StatusCodes.Status499ClientClosedRequest;
                    message = ex.Message;
                    break;

                case FileNotFoundException fileNotFoundEx:
                    statusCode = StatusCodes.Status404NotFound;
                    message = fileNotFoundEx.Message;
                    break;

                case Exception e: // minden más exception
                    statusCode = StatusCodes.Status500InternalServerError;
                    message = e.Message;
                    break;
            }

            //válasz JSON formátumban
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var result = System.Text.Json.JsonSerializer.Serialize(new
            {
                status = statusCode,
                error = ex.GetType().Name,
                message
            });

            return context.Response.WriteAsync(result);
        }
    }
}

