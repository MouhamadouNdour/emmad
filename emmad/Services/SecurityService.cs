using emmad.Entity;
using emmad.Settings;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace emmad.Services
{
    public class SecurityService
    {
        public static void CreatePasswordHash(string password, out byte[] passwordHashed, out byte[] passwordSalted)
        {
            if (password == null)
            {
                throw new ArgumentNullException("Mot de passe (password)");
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Le mot de passe ne peut pas être vide.", "Mot de passe (password)");
            }

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalted = hmac.Key;
                passwordHashed = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }

        public static bool VerifyPasswordHash(byte[] storedHashedPassword, byte[] storedSaltedPassword, string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException("Mot de passe (password)");
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Le mot de passe ne peut pas être vide.", "Mot de passe (password)");
            }
            if (storedHashedPassword.Length != 64)
            {
                throw new ArgumentException("La taille du mot de passe n'est pas valide", "storedHashedPassword");
            }
            if (storedSaltedPassword.Length != 128)
            {
                throw new ArgumentException("La taille du mot de passe salt n'est pas valide", "storedSaltedPassword");
            }

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSaltedPassword))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHashedPassword[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static string GenerateJwtToken(Administrateur administrateur, AppSettings settings)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(settings.JWTSecretCode);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", administrateur.id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static string GenerateRandomToken()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // Conversion en string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }
    }
}
