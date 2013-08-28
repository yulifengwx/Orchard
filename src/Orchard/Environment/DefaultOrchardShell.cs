using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.OwnedInstances;
using Orchard.Logging;
using Orchard.Mvc.ModelBinders;
using Orchard.Mvc.Routes;
using Orchard.Owin;
using Orchard.Tasks;
using Orchard.WebApi.Routes;
using Owin;
using Owin.Builder;
using IModelBinderProvider = Orchard.Mvc.ModelBinders.IModelBinderProvider;

namespace Orchard.Environment {
    public class DefaultOrchardShell : IOrchardShell {
        private readonly Func<Owned<IOrchardShellEvents>> _eventsFactory;
        private readonly IEnumerable<IRouteProvider> _routeProviders;
        private readonly IEnumerable<IHttpRouteProvider> _httpRouteProviders;
        private readonly IRoutePublisher _routePublisher;
        private readonly IEnumerable<IModelBinderProvider> _modelBinderProviders;
        private readonly IModelBinderPublisher _modelBinderPublisher;
        private readonly ISweepGenerator _sweepGenerator;

        public DefaultOrchardShell(
            Func<Owned<IOrchardShellEvents>> eventsFactory,
            IEnumerable<IRouteProvider> routeProviders,
            IEnumerable<IHttpRouteProvider> httpRouteProviders,
            IRoutePublisher routePublisher,
            IEnumerable<IModelBinderProvider> modelBinderProviders,
            IModelBinderPublisher modelBinderPublisher,
            ISweepGenerator sweepGenerator) {
            _eventsFactory = eventsFactory;
            _routeProviders = routeProviders;
            _httpRouteProviders = httpRouteProviders;
            _routePublisher = routePublisher;
            _modelBinderProviders = modelBinderProviders;
            _modelBinderPublisher = modelBinderPublisher;
            _sweepGenerator = sweepGenerator;

            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        public void Activate() {
            IAppBuilder app = new AppBuilder(new Dictionary<Tuple<Type, Type>, Delegate>(), new Dictionary<string, object>());

            // todo: use a dedicated event to register middlewares, right now, hard code some of them
            app.UseHelloOrchard();

            Func<IDictionary<string, object>, Task> env = app.Build();

            var allRoutes = new List<RouteDescriptor>();
            allRoutes.AddRange(_routeProviders.SelectMany(provider => provider.GetRoutes()));
            allRoutes.AddRange(_httpRouteProviders.SelectMany(provider => provider.GetRoutes()));

            _routePublisher.Publish(allRoutes, env);
            _modelBinderPublisher.Publish(_modelBinderProviders.SelectMany(provider => provider.GetModelBinders()));

            using (var events = _eventsFactory()) {
                events.Value.Activated();
            }


            _sweepGenerator.Activate();
        }

        public void Terminate() {
            SafelyTerminate(() => {
                       using (var events = _eventsFactory()) {
                           SafelyTerminate(() => events.Value.Terminating());
                       }
                   });

            SafelyTerminate(() => _sweepGenerator.Terminate());
        }


        private void SafelyTerminate(Action action) {
            try {
                action();
            }
            catch(Exception e) {
                Logger.Error(e, "An unexcepted error occured while terminating the Shell");
            }
        }
    }
}
