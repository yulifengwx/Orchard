using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Orchard.Environment.Configuration;
using Orchard.Logging;
using Orchard.ContentManagement;
using Orchard.Mvc;
using Orchard.Services;

namespace Orchard.Security.Providers {
    public class FormsAuthenticationService : IAuthenticationService {
        private readonly ShellSettings _settings;
        private readonly IClock _clock;
        private readonly IContentManager _contentManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IUser _signedInUser;
        private bool _isAuthenticated;

        public FormsAuthenticationService(ShellSettings settings, IClock clock, IContentManager contentManager, IHttpContextAccessor httpContextAccessor) {
            _settings = settings;
            _clock = clock;
            _contentManager = contentManager;
            _httpContextAccessor = httpContextAccessor;

            Logger = NullLogger.Instance;
            
            ExpirationTimeSpan = TimeSpan.FromDays(30);
        }

        public ILogger Logger { get; set; }

        public TimeSpan ExpirationTimeSpan { get; set; }

        public void SignIn(IUser user, bool createPersistentCookie) {

            //var now = _clock.UtcNow.ToLocalTime();
            //var userData = Convert.ToString(user.Id);

            //var ticket = new FormsAuthenticationTicket(
            //    1 /*version*/,
            //    user.UserName,
            //    now,
            //    now.Add(ExpirationTimeSpan),
            //    createPersistentCookie,
            //    userData,
            //    FormsAuthentication.FormsCookiePath);

            //var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            //var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket) {
            //    HttpOnly = true, 
            //    Secure = FormsAuthentication.RequireSSL, 
            //    Path = FormsAuthentication.FormsCookiePath
            //};

            //var httpContext = _httpContextAccessor.Current();

            //if (!String.IsNullOrEmpty(_settings.RequestUrlPrefix)) {
            //    var cookiePath = httpContext.Request.ApplicationPath;
            //    if (cookiePath != null && cookiePath.Length > 1) {
            //        cookiePath += '/';
            //    }

            //    cookiePath += _settings.RequestUrlPrefix;
            //    cookie.Path = cookiePath;
            //}

            //if (FormsAuthentication.CookieDomain != null) {
            //    cookie.Domain = FormsAuthentication.CookieDomain;
            //}

            //if (createPersistentCookie) {
            //    cookie.Expires = ticket.Expiration;
            //}
            
            //httpContext.Response.Cookies.Add(cookie);

            //_isAuthenticated = true;
            //_signedInUser = user;

            // var claims = new ClaimsIdentity(AuthenticationType.SignIn);

            //var userManager = new UserManager(new IdentityManager());
            //ClaimsIdentity cookieIdentity = await UserManager.CreateIdentityAsync(user,
            //        DefaultAuthenticationTypes.ApplicationCookie);

            //var manager = new UserManager<UserStub>(new UserStoreStub());

            //var claimUser = new UserStub {
            //    Id = user.Id.ToString(),
            //    UserName = user.UserName
            //};
            
            var claimsIdentity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), "http://www.w3.org/2001/XMLSchema#string"));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.UserName, "http://www.w3.org/2001/XMLSchema#string"));
            claimsIdentity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"));
	
            _httpContextAccessor.Current()
                .GetOwinContext()
                .Authentication
                .SignIn(claimsIdentity);
        }

        public void SignOut() {
            _signedInUser = null;
            _isAuthenticated = false;

            _httpContextAccessor.Current()
                .GetOwinContext()
                .Authentication
                .SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }

        public void SetAuthenticatedUserForRequest(IUser user) {
            _signedInUser = user;
            _isAuthenticated = true;
        }

        public IUser GetAuthenticatedUser() {
            if (_signedInUser != null || _isAuthenticated)
                return _signedInUser;

            var httpContext = _httpContextAccessor.Current();
            if (httpContext == null || !httpContext.Request.IsAuthenticated || !(httpContext.User.Identity is ClaimsIdentity)) {
                return null;
            }

            var formsIdentity = (ClaimsIdentity)httpContext.User.Identity;
            var userId = int.Parse(formsIdentity.GetUserId());
            

            _isAuthenticated = true;
            return _signedInUser = _contentManager.Get(userId).As<IUser>();
        }
    }
}
