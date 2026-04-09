using System.Diagnostics;

namespace RemarkWebAPI.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;
        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation("Входящий запрос: {Method} {Path}", context.Request.Method, context.Request.Path);
            var stopwatch = Stopwatch.StartNew(); // Запуск таймера для измерения времени обработки запроса

            await _next(context); // Передача управления следующему middleware в конвейере

            stopwatch.Stop(); // Остановка таймера после обработки запроса
            _logger.LogInformation("Исходящий ответ: метод - {Method}, путь - {Path} код - {StatusCode} за {ElapsedMilliseconds} мс", 
                context.Request.Method, context.Request.Path, context.Response.StatusCode, stopwatch.ElapsedMilliseconds);
        }
    }
}
