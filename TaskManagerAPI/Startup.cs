using Microsoft.Owin;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security;
using Owin;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Web.Http;
using System;

[assembly: OwinStartup(typeof(TaskManagerAPI.Startup))]

namespace TaskManagerAPI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configura CORS si es necesario
            //app.UseCors(CorsOptions.AllowAll);

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
        }
    }
}
