using AutenticacaoDoisFatores.Core.Extensoes;
using AutenticacaoDoisFatores.Servico.Excecoes;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AutenticacaoDoisFatores.Servico.Utilitarios
{
    public static class Token
    {
        private const string EMAIL_ENVIO_CHAVE = "ENVIO_EMAIL_CHAVE";
        private const string EMAIL_REENVIO_CHAVE = "REENVIO_EMAIL_CHAVE";
        private const string EMAIL_ALTERACAO_NOME_ENTIDADE_ACESSO = "EMAIL_ALTERACAO_NOME_ENTIDADE_ACESSO";
        private const string EMAIL_ALTERACAO_EMAIL_ENTIDADE_ACESSO = "EMAIL_ALTERACAO_EMAIL_ENTIDADE_ACESSO";
        private const string EMAIL_CONFIRMACAO_CADASTRO_USUARIO = "EMAIL_CONFIRMACAO_CADASTRO_USUARIO";
        private const string EMAIL_ALTERACAO_EMAIL_USUARIO = "EMAIL_ALTERACAO_EMAIL_USUARIO";
        private const string EMAIL_ALTERACAO_SENHA_USUARIO = "EMAIL_ALTERACAO_SENHA_USUARIO";
        private const string AUTENTICAO_USUARIO = "AUTENTICAO_USUARIO";

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

        private static string GerarToken(string sub, List<Claim> claims)
        {
            var chaveToken = Environment.GetEnvironmentVariable("TOKEN_KEY") ?? "";
            if (chaveToken.IsNullOrEmptyOrWhiteSpaces())
                TokenException.ChaveNaoEncontrada();

            var chaveSecreta = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveToken));

            var credenciais = new SigningCredentials(chaveSecreta, SecurityAlgorithms.HmacSha256);

            var claimNameIdentifier = new Claim(ClaimTypes.NameIdentifier, sub);
            claims.Add(claimNameIdentifier);

            var tokeOptions = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credenciais);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            return tokenString;
        }

        public static string GerarTokenReenvioChave(int id)
        {
            var claim = new Claim(ClaimTypes.Hash, id.ToString());
            var token = GerarToken(EMAIL_REENVIO_CHAVE, claim);

            return token;
        }

        public static string GerarTokenEnvioChaveAcesso(string email)
        {
            var claim = new Claim(ClaimTypes.Email, email);
            var token = GerarToken(EMAIL_ENVIO_CHAVE, claim);

            return token;
        }

        private static ClaimsPrincipal ValidarToken(string token)
        {
            var chaveToken = Environment.GetEnvironmentVariable("TOKEN_KEY") ?? "";
            if (chaveToken.IsNullOrEmptyOrWhiteSpaces())
                TokenException.ChaveNaoEncontrada();

            var segurancaToken = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(chaveToken);

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

        public static int? RetornarIdReenvioChave(string token)
        {
            var principal = ValidarToken(token);
            var subjectClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (subjectClaim?.Value != EMAIL_REENVIO_CHAVE)
                return null;

            var idClaim = principal.FindFirst(ClaimTypes.Hash);
            int? id = idClaim is not null ? int.Parse(idClaim.Value) : null;
            return id;
        }

        public static string? RetornarEmailConfirmacaoCadastroEntidadeAcesso(string token)
        {
            var principal = ValidarToken(token);
            var subjectClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (subjectClaim?.Value != EMAIL_ENVIO_CHAVE)
                return null;

            var emailClaim = principal.FindFirst(ClaimTypes.Email);
            return emailClaim?.Value;
        }

        public static string GerarTokenAlterarNomeEntidadeAcesso(int id, string nome)
        {
            var claims = new List<Claim>()
            {
                new(ClaimTypes.Hash, id.ToString()),
                new(ClaimTypes.Name, nome)
            };
            var token = GerarToken(EMAIL_ALTERACAO_NOME_ENTIDADE_ACESSO, claims);

            return token;
        }

        public static (int? id, string? nome) RetornarNomeAlteracaoEntidadeAcesso(string token)
        {
            var principal = ValidarToken(token);
            var subjectClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (subjectClaim?.Value != EMAIL_ALTERACAO_NOME_ENTIDADE_ACESSO)
                return (null, null);

            var idClaim = principal.FindFirst(ClaimTypes.Hash);
            int? id = idClaim is not null ? int.Parse(idClaim.Value) : null;

            var nomeClaim = principal.FindFirst(ClaimTypes.Name);
            var nome = nomeClaim?.Value;

            return (id, nome);
        }

        public static string GerarTokenAlterarEmailEntidadeAcesso(int id, string email)
        {
            var claims = new List<Claim>()
            {
                new(ClaimTypes.Hash, id.ToString()),
                new(ClaimTypes.Email, email)
            };
            var token = GerarToken(EMAIL_ALTERACAO_EMAIL_ENTIDADE_ACESSO, claims);

            return token;
        }

        public static (int? id, string? email) RetornarIdEmailAlteracaoEmailEntidadeAcesso(string token)
        {
            var principal = ValidarToken(token);
            var subjectClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (subjectClaim?.Value != EMAIL_ALTERACAO_EMAIL_ENTIDADE_ACESSO)
                return (null, null);

            var idClaim = principal.FindFirst(ClaimTypes.Hash);
            var idString = idClaim?.Value;
            int? id = idString is not null ? int.Parse(idString) : null;

            var emailClaim = principal.FindFirst(ClaimTypes.Email);
            var email = emailClaim?.Value;

            return (id, email);
        }
        
        public static string GerarTokenConfirmacaoCadastro(int id, Guid chave)
        {
            var listaClaims = new List<Claim>();
            var idClaim = new Claim(ClaimTypes.Hash, id.ToString());
            listaClaims.Add(idClaim);
            var chaveClaim = new Claim(ClaimTypes.Authentication, chave.ToString());
            listaClaims.Add(chaveClaim);

            var token = GerarToken(EMAIL_CONFIRMACAO_CADASTRO_USUARIO, listaClaims);

            return token;
        }

        public static (int? id, Guid? chave) RetornarIdEChaveConfirmacaoCadastro(string token)
        {
            var principal = ValidarToken(token);
            var subjectClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (subjectClaim?.Value != EMAIL_CONFIRMACAO_CADASTRO_USUARIO)
                return (null, null);

            var idClaim = principal.FindFirst(ClaimTypes.Hash);
            var idString = idClaim?.Value;
            int? id = idString is not null ? int.Parse(idString) : null;

            var chaveClaim = principal.FindFirst(ClaimTypes.Authentication);
            var chaveString = chaveClaim?.Value;
            Guid? chave = chaveString is not null ? Guid.Parse(chaveString) : null;

            return (id, chave);
        }

        public static string GerarTokenAlterarEmailUsuario(int id, string email, Guid chave)
        {
            var claims = new List<Claim>()
            {
                new(ClaimTypes.Hash, id.ToString()),
                new(ClaimTypes.Email, email),
                new(ClaimTypes.Authentication, chave.ToString())
            };
            var token = GerarToken(EMAIL_ALTERACAO_EMAIL_USUARIO, claims);

            return token;
        }

        public static (int? id, string? email, Guid? chave) RetornarIdEmailAlteracaoEmailUsuario(string token)
        {
            var principal = ValidarToken(token);
            var subjectClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (subjectClaim?.Value != EMAIL_ALTERACAO_EMAIL_USUARIO)
                return (null, null, null);

            var idClaim = principal.FindFirst(ClaimTypes.Hash);
            var idString = idClaim?.Value;
            int? id = idString is not null ? int.Parse(idString) : null;

            var emailClaim = principal.FindFirst(ClaimTypes.Email);
            var email = emailClaim?.Value;

            var chaveClaim = principal.FindFirst(ClaimTypes.Authentication);
            var chaveString = chaveClaim?.Value;
            Guid? chave = chaveString is not null ? Guid.Parse(chaveString) : null;

            return (id, email, chave);
        }

        public static string GerarTokenAlterarSenhaUsuario(int id, string senha, Guid chave)
        {
            var claims = new List<Claim>()
            {
                new(ClaimTypes.Hash, id.ToString()),
                new(ClaimTypes.AuthorizationDecision, senha),
                new(ClaimTypes.Authentication, chave.ToString())
            };
            var token = GerarToken(EMAIL_ALTERACAO_SENHA_USUARIO, claims);

            return token;
        }

        public static (int? id, string? senha, Guid? chave) RetornarIdSenhaAlteracaoSenhaUsuario(string token)
        {
            var principal = ValidarToken(token);
            var subjectClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (subjectClaim?.Value != EMAIL_ALTERACAO_SENHA_USUARIO)
                return (null, null, null);

            var idClaim = principal.FindFirst(ClaimTypes.Hash);
            var idString = idClaim?.Value;
            int? id = idString is not null ? int.Parse(idString) : null;

            var senhaClaim = principal.FindFirst(ClaimTypes.AuthorizationDecision);
            var senha = senhaClaim?.Value;

            var chaveClaim = principal.FindFirst(ClaimTypes.Authentication);
            var chaveString = chaveClaim?.Value;
            Guid? chave = chaveString is not null ? Guid.Parse(chaveString) : null;

            return (id, senha, chave);
        }

        public static string GerarTokenAutenticacaoUsuario(int id, Guid chave)
        {
            var claims = new List<Claim>()
            {
                new(ClaimTypes.Hash, id.ToString()),
                new(ClaimTypes.Authentication, chave.ToString())
            };
            var token = GerarToken(AUTENTICAO_USUARIO, claims);

            return token;
        }
    }
}
