using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace CalculatorService.MiddleWare
{
    public class AuthorizationHandler:AuthenticationHandler<AuthenticationSchemeOptions>
    {
        IConfiguration config;
        public AuthorizationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,ISystemClock clock, IConfiguration _config) :base(options, logger, encoder, clock)
        {
            config = _config;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                if (Request.Headers.ContainsKey("Authorization"))
                {
                    Request.Headers.TryGetValue("Authorization",out var authorizationVal);
                    var configuredValue = config.GetValue<string>("AuthorizationKey");
                    if (authorizationVal.Contains<string>(configuredValue))
                    {
                        return AuthenticateResult.Success(new AuthenticationTicket(new System.Security.Claims.ClaimsPrincipal(), "Authorization"));
                    }

                    return AuthenticateResult.Fail("Un Authorized access");
                }
                return AuthenticateResult.Fail("Un Authorized access");
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
