using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmartMeter.Services.TariffServices;
using SmartMeter.Services.UserServices;
using SmartMeter.Services;
using System.Text;
using SmartMeter.Data;
using SmartMeter.Services.TodRuleServices;
using SmartMeter.Services.TariffSlabServices;
using SmartMeter.Services.RabbitMqService.Utils;
using static SmartMeter.Services.RabbitMqService.RabbitMqProcessService;
using SmartMeter.Services.RabbitMqService;
using Microsoft.Extensions.DependencyInjection;

namespace SmartMeter
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

            // json to date converter
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new SmartMeter.Helpers.DateOnlyJsonConverter());
            });


            //b
            //  uilder.Services.AddSingleton(new DatabaseService("Host=localhost;Port=5433;Username=postgres;Password=Admin;Database=SmartMeterDatabase"));

            builder.Services.AddHostedService<RabbitMqConsumerService>();
            //builder.Services.AddHostedService<DatabaseService>();
            

            // In Program.cs (for .NET 6+ minimal APIs) or Startup.ConfigureServices (for older versions)
            //builder.Services.AddScoped<IDatabaseService, DatabaseService>(provider =>
            //{
            //    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            //    return new DatabaseService(connectionString);
            //});



            builder.Services.AddDbContext<SmartMeterDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ITariffServices, TariffServices>();
            builder.Services.AddScoped<IUserServices, UserServices>();
            builder.Services.AddScoped<ITodRuleServices, TodRuleServices>();
            builder.Services.AddScoped<ITariffSlabServices, TariffSlabServices>();
            //builder.Services.AddScoped<IDatabaseService, DatabaseService>();
            //builder.Services.AddScoped<IConsumptionService, ConsumptionService>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["AppSettings:Issuer"],
                ValidAudience = builder.Configuration["AppSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!))

            });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();



            app.MapControllers();

            app.Run();
        }
    }
}
