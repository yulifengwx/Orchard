using Owin;

namespace Orchard.Owin {
    public static class HelloOrchardMiddleware {
        public static IAppBuilder UseHelloOrchard(this IAppBuilder app) {
            app.UseHandler((request, response, next) => {
                response.AddHeader("Hello", "Orchard");
                return next();
            });

            return app;
        }
    }
}
