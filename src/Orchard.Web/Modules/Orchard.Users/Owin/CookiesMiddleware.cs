using System.Collections.Generic;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using Orchard.Owin;
using Owin;
using AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode;
using OwinMiddleware = Orchard.Owin.OwinMiddleware;

namespace Orchard.Users.Owin {
    public class CookiesMiddleware : IOwinMiddlewareProvider {
        public IEnumerable<OwinMiddleware> GetOwinMiddlewares() {

            yield return new OwinMiddleware {
                Configure = app => app.UseCookieAuthentication(new CookieAuthenticationOptions {
                    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                    LoginPath = new PathString("/Users/Account/AccessDenied"),
                }),
                Priority = "1"
            };


            yield return new OwinMiddleware {
                Configure = app => app.UseCookieAuthentication(new CookieAuthenticationOptions {
                    AuthenticationType = DefaultAuthenticationTypes.ExternalCookie,
                    AuthenticationMode = AuthenticationMode.Passive,
                }),
                Priority = "1.1"
            };
        }
    }
}