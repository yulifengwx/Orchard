using Owin;

namespace Orchard.Owin {
    public class Startup {
        public void Configuration(IAppBuilder app) {
            app.Use((context, next) => {
                context.Response.Headers.Append("Owin", "Global Hello");
                return next();
            });
        }
    }
}