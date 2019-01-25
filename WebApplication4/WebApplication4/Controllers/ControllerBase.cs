using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication4.BL;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public abstract class ControllerBase : Controller
    {
        private const string usertoken_sessionkey = "usertoken";
        private const string displayuser_sessionkey = "displayuser";

        protected string UserToken
        {
            get
            {
                if (Session[usertoken_sessionkey] != null && Session[usertoken_sessionkey].GetType() == typeof(string))
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
        protected FullUser DisplayUserInfo
        {
            get
            {
                if (Session[displayuser_sessionkey] != null && Session[displayuser_sessionkey].GetType() == typeof(string))
                    return (FullUser)Session[displayuser_sessionkey];
                else
                    return null;
            }
            set
            {
                if (value == null)
                    Session.Remove(displayuser_sessionkey);
                else
                    Session[displayuser_sessionkey] = value;
            }
        }

        protected async Task<bool> Authorized()
        {
           return await AccountManager.IsUserAuthorizedAsync(UserToken);
        }

        public async Task RefreshToken()
        {
            UserToken= await AccountManager.RefreshTokenAsync(UserToken);
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


        protected ActionResult ErrorView(string message = null)
        {
            Session["ErrorMessage"] = message;
            return View("Error");
        }

        protected ActionResult SuccessView(string message = null)
        {
            Session["SuccessMessage"] = message;
            return View("Success");
        }
    }
}