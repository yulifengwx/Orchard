using System.Collections.Generic;
using Orchard.Owin;
using Owin;

namespace Contrib.HelloOwin.Owin {
    public class HelloOwinMiddleware : IOwinMiddlewareProvider {

        public IEnumerable<OwinMiddleware> GetOwinMiddlewares() {
            yield return new OwinMiddleware {
                Configure = app => app.Use((context, next) => {
                    context.Response.Headers.Append("Owin", "Hello Orchard");
                    return next();
                }),
                Priority = "10"
            };
        }
    }
}