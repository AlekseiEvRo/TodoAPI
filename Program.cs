
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TodoAPI.DAL;
using TodoAPI.Middleware;

namespace TodoAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var xmlFile = Path.Combine(AppContext.BaseDirectory, "TodoAPIDocumentation.xml");
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TODO API", Version = "v1" });
                c.IncludeXmlComments(xmlFile); // Подключение XML комментариев
            });

            builder.Services.AddDbContext<TodoContext>(options =>
                options.UseSqlite("Data Source=todo.db"));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<RequestLoggingMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}