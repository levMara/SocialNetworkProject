using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication4.BL;

namespace WebApplication4.Controllers
{
    public abstract class IdentityControllerBase : Controller
    {
        private const string usertoken_sessionkey = "usertoken";

        protected string UserToken
        {
            get
            {
                if (Session[usertoken_sessionkey]!=null && Session[usertoken_sessionkey].GetType() == typeof(string))
                    return (string)Session[usertoken_sessionkey];
                else
                    return null;
            }
            set
            {
                if (value == null)
                    Session.Remove(usertoken_sessionkey);
                else
                    Session[usertoken_sessionkey] = value;
            }
        }

        protected bool Authorized
        {
            get => (AccountManager.IsUserAuthorized(UserToken));
        }

        public void RefreshToken()
        {
            UserToken= AccountManager.RefreshToken(UserToken);
        }

        protected ActionResult AccountLogin()
        {
            return RedirectToAction("Login", "Account");
        }
        
        protected void AddError(string errorMessage)
        {
            ModelState.AddModelError("", errorMessage);
        }
        protected ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}