#region Imports

using System;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Autofac;
using Autofac.Integration.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using PalMarket.Domain.Contracts.Services;
using PalMarket.Model;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

#endregion

namespace PalMarket.API.OAuthProviders
{
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var lifetimeScope = context.OwinContext.GetAutofacLifetimeScope();
            var userService = lifetimeScope.Resolve<IUserService>();

            try
            {
                var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin") ?? "*";
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

                if (string.IsNullOrWhiteSpace(context.UserName) || string.IsNullOrWhiteSpace(context.Password))
                {
                    context.SetError(((int)ErrorCode.ValidationError).ToString(), "The username or password is not passed.");
                    return;
                }

                User user = userService.Authenticate(context.UserName, context.Password);
                if (user == null)
                {
                    context.SetError(((int)ErrorCode.ValidationError).ToString(), "Invalid username or password.");
                    return;
                }

                ClaimsIdentity identity = new ClaimsIdentity(new OAuthAuthorizationServerOptions().AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Email, user.Username));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.FirstName));
                identity.AddClaim(new Claim(ClaimTypes.Surname, user.LastName));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()));
                identity.AddClaim(new Claim("BranchID", user.BranchID.ToString()));

                var properties = CreateProperties(user, context);
                var ticket = new AuthenticationTicket(identity, properties);
                context.Validated(ticket);
            }
            catch (Exception ex)
            {
                context.SetError(((int)ErrorCode.GeneralError).ToString(), ex.Message);
            }
        }

        public static AuthenticationProperties CreateProperties(User user, OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userID = user.UserID;
            var username = user.Username;
            var firstName = user.FirstName;
            var lastName = user.LastName;
            var branchID = user.BranchID;

            IDictionary<string, string> data = new Dictionary<string, string>
            {
                {
                    "as:client_id", context.ClientId ?? string.Empty
                },
                {
                    "userId", userID.ToString()
                },
                {
                    "email", username
                },
                {
                    "firstName", firstName
                },
                {
                    "lastName", lastName
                },
                {
                    "branchId", branchID.ToString()
                }
            };
            return new AuthenticationProperties(data);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (var property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.OwinContext.Set("as:clientAllowedOrigin", "*");
            context.OwinContext.Set("as:clientRefreshTokenLifeTime", "7200");
            context.Validated();
        }

        //public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        //{
        //    var lifetimeScope = context.OwinContext.GetAutofacLifetimeScope();
        //    var logger = lifetimeScope.Resolve<ILogger>();

        //    var current = TimeSpan.FromTicks(DateTime.UtcNow.Ticks);
        //    logger.Debug("GrantRefreshToken: Entry");


        //    var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
        //    var currentClient = context.ClientId;

        //    if (originalClient != currentClient)
        //    {
        //        context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
        //        logger.DebugFormat("GrantRefreshToken: Exit -> {0}",
        //            TimeSpan.FromTicks(DateTime.UtcNow.Ticks).Subtract(current).TotalMilliseconds);
        //        return Task.FromResult<object>(null);
        //    }

        //    // Change auth ticket for refresh token requests
        //    var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
        //    //  newIdentity.AddClaim(new Claim("newClaim", "newValue"));

        //    var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
        //    context.Validated(newTicket);

        //    var now = TimeSpan.FromTicks(DateTime.UtcNow.Ticks);
        //    logger.DebugFormat("GrantRefreshToken: Exit -> {0}", now.Subtract(current).TotalMilliseconds);

        //    return Task.FromResult<object>(null);
        //}
    }
}