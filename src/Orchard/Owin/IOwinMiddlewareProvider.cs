using Owin;

namespace Orchard.Owin {
    public interface IOwinMiddlewareProvider : IDependency {
        void Register(IAppBuilder builder);
        string Position { get; }
    }
}
