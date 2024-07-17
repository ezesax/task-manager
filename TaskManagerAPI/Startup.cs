using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security;
using Owin;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Web.Http;
using System;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using TaskManagerAPI.Services;
using TaskManagerAPI.Services.Jobs;
using Microsoft.Extensions.Hosting;

[assembly: OwinStartup(typeof(TaskManagerAPI.Startup))]

namespace TaskManagerAPI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Crear un nuevo ServiceCollection
            var services = new ServiceCollection();

            // Llamar a ConfigureServices para agregar los servicios a la colección
            ConfigureServices(services);

            var issuer = "https://localhost:44351";
            var audience = "TaskManagerAPI";
            var secret = "dGhpcyBzaGFyZCBpcyBsb25nIGVuZCBjYW5ub3QgdGVsbCBjb3N0";
            var key = Encoding.UTF8.GetBytes(secret);

            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                }
            });

            app.Use(async (context, next) =>
            {
                // Log de la solicitud
                Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
                await next.Invoke();
            });

            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseWebApi(config);

            // Agrega un log para confirmar la configuración de los servicios
            Console.WriteLine("Startup: Servicios configurados.");

            // Construir el proveedor de servicios
            var serviceProvider = services.BuildServiceProvider();

            // Iniciar el servicio hospedado
            var hostedService = serviceProvider.GetService<IHostedService>() as TaskManagerAPI.Services.Jobs.QuartzHostedService;
            if (hostedService != null)
            {
                hostedService.StartAsync(new System.Threading.CancellationToken()).Wait();
                Console.WriteLine("Startup: Servicio hospedado Quartz iniciado.");
            }
            else
            {
                Console.WriteLine("Startup: No se pudo iniciar el servicio hospedado Quartz.");
            }
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            services.AddSingleton<CurrencyJob>();
            services.AddSingleton(new JobSchedule(
                jobType: typeof(CurrencyJob),
                cronExpression: "0 */1 * * * ?"));

            services.AddHostedService<TaskManagerAPI.Services.Jobs.QuartzHostedService>();

            // Log para confirmar la configuración de los servicios
            Console.WriteLine("ConfigureServices: Servicios configurados.");
        }
    }
}
