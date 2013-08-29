using Orchard.Owin;
using Owin;

namespace Orchard.OutputCache.Owin {
    public class OutputCacheMiddleware : IOwinMiddlewareProvider {
        public void Register(IAppBuilder builder) {
            builder.Use((context, next) => {
                context.Response.Headers.Add("X-Owin", new []{"Hello Orchard"});
                return next();
            });
        }

        public string Position {
            get { return "1"; }
        }
    }
}