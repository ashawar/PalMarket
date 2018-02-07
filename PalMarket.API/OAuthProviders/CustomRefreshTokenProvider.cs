#region Imports

using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Autofac;
using Autofac.Integration.Owin;
using Microsoft.Owin.Security.Infrastructure;

#endregion

namespace PalMarket.API.OAuthProviders
{
    public class CustomRefreshTokenProvider : IAuthenticationTokenProvider
    {
        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            //var lifetimeScope = context.OwinContext.GetAutofacLifetimeScope();
            //var authRepository = lifetimeScope.Resolve<IAuthRepository>();
            //var logger = lifetimeScope.Resolve<ILogger>();

            //var current = TimeSpan.FromTicks(DateTime.UtcNow.Ticks);
            //logger.Debug("CreateAsync: Entry");

            //var clientid = context.Ticket.Properties.Dictionary["as:client_id"];

            //if (string.IsNullOrEmpty(clientid))
            //{
            //    logger.DebugFormat("CreateAsync: Exit -> {0}",
            //        TimeSpan.FromTicks(DateTime.UtcNow.Ticks).Subtract(current).TotalMilliseconds);
            //    return;
            //}

            //var refreshTokenId = NewId.NextGuid().ToString("N");
            //var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");

            //var token = new RefreshToken
            //{
            //    Id = refreshTokenId.GetHash<SHA256CryptoServiceProvider>(),
            //    ClientId = clientid,
            //    Subject = context.Ticket.Identity.Name,
            //    IssuedUtc = DateTime.UtcNow,
            //    ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))
            //};

            //context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
            //context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

            //token.ProtectedTicket = context.SerializeTicket();

            //await authRepository.AddRefreshToken(token);
            //context.SetToken(refreshTokenId);
            //var now = TimeSpan.FromTicks(DateTime.UtcNow.Ticks);
            //logger.DebugFormat("CreateAsync: Exit -> {0}", now.Subtract(current).TotalMilliseconds);
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            //var lifetimeScope = context.OwinContext.GetAutofacLifetimeScope();
            //var authRepository = lifetimeScope.Resolve<IAuthRepository>();
            //var logger = lifetimeScope.Resolve<ILogger>();

            //var current = TimeSpan.FromTicks(DateTime.UtcNow.Ticks);
            //logger.Debug("ReceiveAsync: Entry");

            //var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] {allowedOrigin});

            //var hashedTokenId = context.Token.GetHash<SHA256CryptoServiceProvider>();

            //var refreshToken = await authRepository.FindRefreshToken(hashedTokenId);

            //if (refreshToken != null)
            //{
            //    //Get protectedTicket from refreshToken class
            //    context.DeserializeTicket(refreshToken.ProtectedTicket);
            //    await authRepository.RemoveRefreshToken(hashedTokenId);
            //}
            //var now = TimeSpan.FromTicks(DateTime.UtcNow.Ticks);
            //logger.DebugFormat("ReceiveAsync: Exit -> {0}", now.Subtract(current).TotalMilliseconds);
        }
    }
}