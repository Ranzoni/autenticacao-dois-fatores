using AutenticacaoDoisFatores.Core.Extensoes;
using AutenticacaoDoisFatores.Servico.Excecoes;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AutenticacaoDoisFatores.Servico
{
    internal static class Token
    {
        private const string EMAIL_REENVIO_CHAVE = "EMAIL";

        private static string GerarToken(string sub, Claim claim)
        {
            var chaveToken = Environment.GetEnvironmentVariable("TOKEN_KEY") ?? "";
            if (chaveToken.IsNullOrEmptyOrWhiteSpaces())
                TokenException.ChaveNaoEncontrada();

            var chaveSecreta = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveToken));

            var credenciais = new SigningCredentials(chaveSecreta, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, sub),
                claim
            };

            var tokeOptions = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credenciais);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            return tokenString;
        }

        internal static string GerarTokenReenvioChave(string email)
        {
            var claim = new Claim(ClaimTypes.Email, email);
            var token = GerarToken(EMAIL_REENVIO_CHAVE, claim);

            return token;
        }

        private static ClaimsPrincipal ValidarToken(string token)
        {
            var chaveToken = Environment.GetEnvironmentVariable("TOKEN_KEY") ?? "";
            if (chaveToken.IsNullOrEmptyOrWhiteSpaces())
                TokenException.ChaveNaoEncontrada();

            var segurancaToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(chaveToken);

            var parametrosValidacao = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
            var principal = segurancaToken.ValidateToken(token, parametrosValidacao, out _);

            return principal;
        }

        internal static string? RetornarEmail(string token)
        {
            var principal = ValidarToken(token);
            var subjectClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (subjectClaim?.Value != EMAIL_REENVIO_CHAVE)
                return null;

            var emailClaim = principal.FindFirst(ClaimTypes.Email);
            return emailClaim?.Value;
        }
    }
}
