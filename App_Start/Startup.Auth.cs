using System;
using System.Security.Claims;
using System.Threading.Tasks;
using ListPlanner_GamGam.Extensions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using ListPlanner_GamGam.Models;
using Microsoft.Owin.Security.Facebook;

namespace ListPlanner_GamGam
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //consumerKey: "",
            //consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "1716254471985442",
            //   appSecret: "a167315f4a8b67ce5f14994d6c1e317f");

            app.UseFacebookAuthentication(new FacebookAuthenticationOptions
            {
                Scope = { "public_profile", "user_friends" },
                AppId = "1716254471985442",
                AppSecret = "a167315f4a8b67ce5f14994d6c1e317f",
                Provider = new FacebookAuthenticationProvider
                {
                    OnAuthenticated = context =>
                    {
                        context.Identity.AddClaim(new System.Security.Claims.Claim("FB_AccessToken", context.AccessToken));



                        return Task.FromResult(0);
                    }
                }
            });

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "140418038220-s3l00svupbl3ejpr1d6o0b2ifdjicln5.apps.googleusercontent.com",
                ClientSecret = "MSjuqGOsCAEYiPIFnmwDhR0B",
                Scope = { "https://www.googleapis.com/auth/plus.login", "profile" },
                Provider = new GoogleOAuth2AuthenticationProvider
                {
                    OnAuthenticated = context =>
                    {
                        var googleImageUrl = context.User.GetValue("image").SelectToken("url").ToString();
                        var googleUserName = context.User.GetValue("displayName").ToString();

                        context.Identity.AddClaim(new System.Security.Claims.Claim(ClaimTypes.Email, context.Email ?? ""));
                        context.Identity.AddClaim(new System.Security.Claims.Claim("ProviderAccessToken", context.AccessToken));

                        context.Identity.AddClaim(new System.Security.Claims.Claim(HsmClaimTypes.ProviderImageUrl, googleImageUrl));
                        context.Identity.AddClaim(new System.Security.Claims.Claim(HsmClaimTypes.ProviderUserName, googleUserName));



                        return Task.FromResult(0);
                    }
                }
            });
        }
    }
}