using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;

[assembly: OwinStartup(typeof(WebAPIOauth.App_Start.Startup1))]

namespace WebAPIOauth.App_Start
{
    public class Startup1
    {
        public static string PublicClientId { get; private set; }
        //public static OAuthAuthorizationServerOptions OAuthOptions;
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            WebApiConfig.Register(config);
           
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888

            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            PublicClientId = "self";
            OAuthAuthorizationServerOptions OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new AppOAuthProvider(PublicClientId),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                // Note: Remove the following line before you deploy to production:
                AllowInsecureHttp = true
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthAuthorizationServer(OAuthOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

           
            app.UseWebApi(config);

        }
    }
}
