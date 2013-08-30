using System.Web;
using System.Web.Mvc;

namespace Contrib.HelloOwin.Results {
    public class ChallengeResult : HttpUnauthorizedResult {
        public ChallengeResult(string loginProvider, Controller controller) {
            LoginProvider = loginProvider;
            Request = controller.Request;
        }

        public string LoginProvider { get; set; }
        public HttpRequestBase Request { get; set; }

        public override void ExecuteResult(ControllerContext context) {
            Request.GetOwinContext().Authentication.Challenge(LoginProvider);
        }
    }
}
