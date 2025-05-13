using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace WebApiCore.Security
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            this._next = next;
            this._configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/hub", StringComparison.OrdinalIgnoreCase) &&
                context.Request.Query.TryGetValue("access_token", out var accessToken))
            {
                context.Request.Headers.Add("Authorization", $"Bearer {accessToken}");
            }

            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                attachUserToContext(context, token);

            await _next(context);
        }

        private void attachUserToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var secretKey = _configuration.GetSection("JWT:SECRET_KEY").Value;
                var audienceToken = _configuration.GetSection("JWT:AUDIENCE_TOKEN").Value;
                var issuerToken = _configuration.GetSection("JWT:ISSUER_TOKEN").Value;
                var expireTime = _configuration.GetSection("JWT:EXPIRE_MINUTES").Value;
                var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(secretKey));

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidAudience = audienceToken,
                    ValidIssuer = issuerToken,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    LifetimeValidator = this.LifetimeValidator,
                    IssuerSigningKey = securityKey
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                context.Items["idUsuario"] = jwtToken.Payload.GetValueOrDefault("unique_name");
            }
            catch (Exception ex) //MOBILE
            {
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var secretKey = _configuration.GetSection("JWT_MOBILE:SECRET_KEY").Value;
                    var audienceToken = _configuration.GetSection("JWT_MOBILE:AUDIENCE_TOKEN").Value;
                    var issuerToken = _configuration.GetSection("JWT_MOBILE:ISSUER_TOKEN").Value;
                    var expireTime = _configuration.GetSection("JWT_MOBILE:EXPIRE_MINUTES").Value;
                    var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(secretKey));

                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidAudience = audienceToken,
                        ValidIssuer = issuerToken,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        LifetimeValidator = this.LifetimeValidator,
                        IssuerSigningKey = securityKey
                    }, out SecurityToken validatedToken);

                    var jwtToken = (JwtSecurityToken)validatedToken;

                    context.Items["idUsuario"] = jwtToken.Payload.GetValueOrDefault("unique_name");
                }
                catch
                {

                }
            }
            var idUsuario = context?.Items?["idUsuario"] ?? null;
        }

        public bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (expires != null)
            {
                if (DateTime.UtcNow < expires) return true;
            }
            return false;
        }
    }
}