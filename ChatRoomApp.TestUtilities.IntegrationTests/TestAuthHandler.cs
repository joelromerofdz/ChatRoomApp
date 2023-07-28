﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace ChatRoomApp.TestUtilities.IntegrationTests
{
    //Authentication
    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
           
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, UserSettings.User),
                new Claim(ClaimTypes.NameIdentifier, UserSettings.UserId)
            };
            var identity = new ClaimsIdentity(claims, UserSettings.User);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, UserSettings.User);

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }
}