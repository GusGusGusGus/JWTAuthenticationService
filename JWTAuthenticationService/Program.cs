using JWTAuthenticationService.Managers;
using JWTAuthenticationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace JWTAuthenticationService
{
    public class Program
    {
        static void Main(string[] args)
        {
            IAuthContainerModel model = GetJWTContainerModel("Dirty Gus", "gus@gus.com");
            IAuthService authService = new JWTService(model.SecretKey);

            string token = authService.GenerateToken(model);

            if (!authService.IsTokenValid(token))
                throw new UnauthorizedAccessException();
            else
            {
                List<Claim> claims = authService.GetTokenClaims(token).ToList();

                WriteLine( claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value );
                WriteLine(claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Email)).Value);
                Read();
            }
        }

        private static JWTContainerModel GetJWTContainerModel(string name, string email)
        {
            return new JWTContainerModel()
            {
                Claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name, name),
                    new Claim(ClaimTypes.Email, email)
                }
            };
        }
    }
}
