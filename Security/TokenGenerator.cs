using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApiCore.Security
{
    internal class TokenGenerator
    {
        private readonly IConfiguration _configuration;

        public TokenGenerator(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public string GenerateTokenJwt(int idUsuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = _configuration.GetSection("JWT:SECRET_KEY").Value;
            var audienceToken = _configuration.GetSection("JWT:AUDIENCE_TOKEN").Value;
            var issuerToken = _configuration.GetSection("JWT:ISSUER_TOKEN").Value;
            var expireTime = _configuration.GetSection("JWT:EXPIRE_MINUTES").Value;
            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, idUsuario.ToString())
            });

            var jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(
                audience: audienceToken,
                issuer: issuerToken,
                subject: claimsIdentity,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(expireTime)),
                signingCredentials: signingCredentials);

            var jwtTokenString = tokenHandler.WriteToken(jwtSecurityToken);
            return jwtTokenString;
        }

        public string GenerateTokenJwt_mobile(int idUsuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = _configuration.GetSection("JWT_MOBILE:SECRET_KEY").Value;
            var audienceToken = _configuration.GetSection("JWT_MOBILE:AUDIENCE_TOKEN").Value;
            var issuerToken = _configuration.GetSection("JWT_MOBILE:ISSUER_TOKEN").Value;
            var expireTime = _configuration.GetSection("JWT_MOBILE:EXPIRE_MINUTES").Value;
            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, idUsuario.ToString())
            });

            var jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(
                audience: audienceToken,
                issuer: issuerToken,
                subject: claimsIdentity,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(expireTime)),
                signingCredentials: signingCredentials);

            var jwtTokenString = tokenHandler.WriteToken(jwtSecurityToken);
            return jwtTokenString;
        }
    }
}
