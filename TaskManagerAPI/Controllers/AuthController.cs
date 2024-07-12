using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
using BCrypt.Net;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Controllers
{
    public class AuthController : ApiController
    {
        private DBContext _context = new DBContext();

        [HttpGet]
        [Route("api/auth/test")]
        public IHttpActionResult Test()
        {
            return Ok("Test endpoint reached");
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("api/auth/login")]
        public IHttpActionResult Login([FromBody] LoginModel login)
        {
            if (login == null)
                return BadRequest("Invalid client request");

            var user = _context.Users.SingleOrDefault(u => u.UserName == login.UserName);

            if (user == null || !VerifyPasswordHash(login.Password, user.PasswordHash))
                return Unauthorized();

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("dGhpcyBzaGFyZCBpcyBsb25nIGVuZCBjYW5ub3QgdGVsbCBjb3N0")); // Cambia esto por tu secret key
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

            var tokenOptions = new JwtSecurityToken(
                issuer: "https://localhost:44351", // Cambia esto por tu issuer
                audience: "TaskManagerAPI", // Cambia esto por tu audience
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return Ok(new { Token = tokenString });
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            // Implementa tu lógica de verificación de hash de contraseña
            // Por ejemplo, si usas bcrypt:
            // return BCrypt.Net.BCrypt.Verify(password, storedHash);
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
    }

    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}