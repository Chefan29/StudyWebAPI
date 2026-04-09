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
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<RemarkDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
            
            builder.Services.AddControllers();

            builder.Services.AddScoped<IRemarkService, EfRemarkService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddHttpsRedirection(options =>
            {
                options.HttpsPort = 7264;
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.MapControllers();

            app.Run();
        }
    }
}
