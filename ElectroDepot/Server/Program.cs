using Microsoft.EntityFrameworkCore;
using Server.Context;
using System.Threading.Tasks;

namespace Server
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });
            // Swagger/OpenAPI configuration
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configure DbContext with connection string
            builder.Services.AddDbContext<DatabaseContext>(x =>
                x.UseSqlServer(builder.Configuration.GetConnectionString("DevConn")));

            var app = builder.Build();

            if(args.Length > 0 && args[0] == "seed")
            {
                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        var context = services.GetRequiredService<DatabaseContext>();
                        await SingleUserTestingDataSeeder.SeedDataAsync(context);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error seeding data: {ex.Message}");
                    }
                }
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
            await app.RunAsync();
        }
    }
}
