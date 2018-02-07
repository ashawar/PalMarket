#region Imports

using System;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Owin;
using PalMarket.API.OAuthProviders;

#endregion

namespace PalMarket.API
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the application for OAuth based flow
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/api/Token"),
                Provider = new CustomOAuthProvider(),
                //If the AccessTokenExpireTimeSpan is changed, also change the ExpiresUtc in the RefreshTokenProvider.cs.
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(30),
                AllowInsecureHttp = true,
                AuthenticationMode = AuthenticationMode.Active,
                RefreshTokenProvider = new CustomRefreshTokenProvider(),
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);
        }
    }
}