using ElectroDepotClassLibrary.Services;
using ElectroDepotClassLibrary.Stores;
using Microsoft.EntityFrameworkCore;
using Server.Context;
using Server.Services;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            ServerConfigService configService = ServerConfigService.Create();
            ImageStorageService imageService = ImageStorageService.CreateService();
            try
            {
                configService.LoadConfigFile();
                imageService.Initialize(configService.StoragePath);

                // Images folder was just created and there are no images.
                //configService.ShouldSeed = imageService.RequiresSeeding;

                if (configService.ShouldSeed == true)
                {
                    imageService.DeleteAllImages();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return;
            }

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });
            // Swagger/OpenAPI configuration
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            string localConnStr = "Server=DESKTOP-ODHTUSS;Database=ElectronDepot;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True";

            // Get Environment vars and create connection string
            //string serverName = Environment.GetEnvironmentVariable("SERVER_NAME") ?? "DESKTOP-ODHTUSS";
            string serverName = Environment.GetEnvironmentVariable("SERVER_NAME") ?? "localhost";
            string dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "ElectroDepotDB";
            //string dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "1433";
            string dbUser= Environment.GetEnvironmentVariable("DB_USER") ?? "SA";
            string dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "Password123";

            StringBuilder sb = new StringBuilder();
            sb.Append("Server=");
            sb.Append(serverName);
            sb.Append(";Database=");
            sb.Append(dbName);
            sb.Append(";User ID =");
            sb.Append(dbUser);
            sb.Append(";Password =");
            sb.Append(dbPassword);
            //sb.Append(";");
            //sb.Append(";Trusted_Connection=True;");
            sb.Append(";TrustServerCertificate=True;MultipleActiveResultSets=True");

            
            string connectionString = sb.ToString();

            if(Environment.GetEnvironmentVariable("SERVER_NAME") == null)
            {
                connectionString = localConnStr;
            }

            Console.WriteLine($"Connection string: '{connectionString}'");

            // Configure DbContext with connection string
            builder.Services.AddDbContext<DatabaseContext>(x =>
                x.UseSqlServer(connectionString));

            var app = builder.Build();

            if(configService.ShouldSeed == true)
            {
                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        var context = services.GetRequiredService<DatabaseContext>();
                        await DefaultDataSeeder.SeedDataAsync(context, configService);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error seeding data: {ex.Message}");
                    }
                }
                configService.ShouldSeed = false;
                configService.SaveImageServiceConfig();
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
