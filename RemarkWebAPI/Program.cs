using Microsoft.EntityFrameworkCore;
using StudyWebAPI.Application.Interfaces;
using StudyWebAPI.Infrastructure.Data;
using StudyWebAPI.Infrastructure.Services;

namespace RemarkWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args); //настройка приложения до его сборки

            builder.Services.AddDbContext<RemarkDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("Default"))); //регистрация контекста данных и настройка подключения к БД

            builder.Services.AddControllers(); //регистрация контроллеров для обработки HTTP-запросов

            builder.Services.AddScoped<IRemarkService, EfRemarkService>(); //регистрация сервиса, который будет использоваться в контроллерах

            builder.Services.AddEndpointsApiExplorer(); // Добавляется инфраструктура для обнаружения API-endpoints
            builder.Services.AddSwaggerGen(); // Добавляется генерация Swagger-документации для API, что позволяет легко тестировать и документировать API-интерфейсы


            var app = builder.Build(); //строительство приложения на основе настроек, добавленных в builder. На этом этапе приложение готово к запуску, и можно настроить конвейер обработки HTTP-запросов.

            if (app.Environment.IsDevelopment()) // проверка, находится ли приложение в режиме разработки. Если да, то включается Swagger UI для удобного тестирования API.
            {
                app.UseSwagger(); // Включение middleware для генерации Swagger-документации
                app.UseSwaggerUI(); // Включение middleware для отображения Swagger UI, который позволяет визуально исследовать и тестировать API
            }

            app.UseHttpsRedirection(); // Включение middleware для перенаправления HTTP-запросов на HTTPS, обеспечивая безопасность передачи данных

            app.MapControllers(); // Настройка маршрутизации для контроллеров, что позволяет обрабатывать входящие HTTP-запросы и направлять их к соответствующим методам в контроллерах

            app.Run(); // Запуск приложения, что позволяет ему начать прослушивание входящих HTTP-запросов и обрабатывать их в соответствии с настроенной маршрутизацией и логикой контроллеров.
        }
    }
}
