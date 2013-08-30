using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Google;
using Orchard.Owin;
using Owin;
using OwinMiddleware = Orchard.Owin.OwinMiddleware;

namespace Contrib.HelloOwin.Google {
    public class GoogleAuthMiddleware : IOwinMiddlewareProvider {
        public IEnumerable<OwinMiddleware> GetOwinMiddlewares() {

            yield return new OwinMiddleware {
                Configure = app => app.UseGoogleAuthentication(new GoogleAuthenticationOptions {
                    SignInAsAuthenticationType = DefaultAuthenticationTypes.ExternalCookie,
                    CallbackPath = new PathString("/signin-google") // todo: tenant based 
                }),
                Priority = "1.2"
            };
        }
    }
}