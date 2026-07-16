using Microsoft.EntityFrameworkCore;
using MovieManagement.DataAccess.Data;
using MovieManagement.Domain.Repositories;
using MovieManagement.DataAccess.Repositories;
using System.Text.Json.Serialization;

namespace MovieManagement.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    // Ignore cycles to prevent serialization exceptions due to bidirectional navigation properties
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

            // Register DbContext with SQLite provider
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register UnitOfWork (Transient or Scoped)
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Ensure SQLite database is created and seeded on startup
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.EnsureCreated();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            // Redirect root path to Swagger UI
            app.MapGet("/", (HttpContext context) =>
            {
                context.Response.Redirect("/swagger");
                return Task.CompletedTask;
            });

            app.Run();
        }
    }
}

