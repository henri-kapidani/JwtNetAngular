using JwtNetAngular.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace JwtNetAngular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AuthController : ControllerBase
    {
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _key;

        public AuthController(IConfiguration configuration) {
            _issuer = configuration["Jwt:Issuer"]!;
            _audience = configuration["Jwt:Audience"]!;
            _key = configuration["Jwt:Key"]!;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginModel model)
        {
            // recuperare dal db l'utente con l'email passata
            // se lo troviamo verificare la password
            // se tutto è verificato generiamo il JWT
            // e rispondiamo con il JWT

            if (model.Email == "asdf@asdf.asdf" && model.Password=="asdf")
            {
                // aggiungiamo i claims al principal dell'utente loggato
                List<Claim> claims = [
                    // new Claim(JwtRegisteredClaimNames.NameId, model.Id)
                    new Claim(JwtRegisteredClaimNames.Email, model.Email)
                ];

                // generare il JWT
                var key = new SymmetricSecurityKey(
                    System.Text.Encoding.UTF8.GetBytes(_key)
                );
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expiration = DateTime.Now.AddSeconds(30); // mettere una scadenza sufficientemente lunga

                var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                    issuer: _issuer,
                    audience: _audience,
                    claims: claims,
                    expires: expiration,
                    signingCredentials: creds
                );

                return Ok(
                    new LoginResponseModel
                    {
                        // Expiration = expiration.ToUniversalTime().ToString(),
                        AccessToken = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token),
                        User = model
                    }
                );
            }
            return Unauthorized();
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterModel model)
        {
            // salvate i dati nel db

            // se volete fare il login automatico dovete generare il jwt come fatto nel controller del Login

            return Ok(new RegisterResponseModel
            {
                // IMPORTANTE passare l'id dell'utente appena creato al frontend + gli altri dati
            });
        }
    }   
}
