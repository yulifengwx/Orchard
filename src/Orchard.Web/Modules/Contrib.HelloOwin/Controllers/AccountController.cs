using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Orchard.Themes;

namespace Contrib.HelloOwin.Controllers {
    [Themed]
    public class AccountController : Controller {

        public ActionResult Index() {
            return View(HttpContext.GetOwinContext().Authentication.GetExternalAuthenticationTypes());
        }

        [ActionName("Challenge")]
        public ActionResult ChallengeGet(string provider) {
            var result = HttpContext.GetOwinContext().Authentication.AuthenticateAsync(DefaultAuthenticationTypes.ExternalCookie).Result;

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult Challenge(string provider) {
            HttpContext.GetOwinContext().Authentication.Challenge(provider);

            return new EmptyResult();
        }
    }
}