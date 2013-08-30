using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;

namespace Contrib.HelloOwin {
    public class Routes : IRouteProvider {

        public void GetRoutes(ICollection<RouteDescriptor> routes) {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes() {
            return new[] {
                             new RouteDescriptor {
                                                     Route = new Route(
                                                         "ExternalLogin",
                                                         new RouteValueDictionary {
                                                                                      {"area", "Contrib.HelloOwin"},
                                                                                      {"controller", "Account"},
                                                                                      {"action", "ExternalLogin"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "Contrib.HelloOwin"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "ExternalLogins",
                                                         new RouteValueDictionary {
                                                                                      {"area", "Contrib.HelloOwin"},
                                                                                      {"controller", "Account"},
                                                                                      {"action", "ExternalLogins"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "Contrib.HelloOwin"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                            new RouteDescriptor {
                                                     Route = new Route(
                                                         "Auth",
                                                         new RouteValueDictionary {
                                                                                      {"area", "Contrib.HelloOwin"},
                                                                                      {"controller", "Account"},
                                                                                      {"action", "Index"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "Contrib.HelloOwin"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                                                  new RouteDescriptor {
                                                     Route = new Route(
                                                         "signing-google",
                                                         new RouteValueDictionary {
                                                                                      {"area", "Contrib.HelloOwin"},
                                                                                      {"controller", "Account"},
                                                                                      {"action", "SignInGoogle"}
                                                                                  },
                                                         new RouteValueDictionary(),
                                                         new RouteValueDictionary {
                                                                                      {"area", "Contrib.HelloOwin"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 }
                         };
        }
    }
}