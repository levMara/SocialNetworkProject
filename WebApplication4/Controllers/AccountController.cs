using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebApplication4.Models;
using WebApplication4.BL;

namespace WebApplication4.Controllers
{
    //[Authorize]
    public class AccountController : ControllerBase
    {
        //private static AccountManager _accountManager=new AccountManager();

        public AccountController()
        {
            //_accountManager = new AccountManager();
        }

        //
        // GET: /Account/Login
        //[AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        
        //
        // POST: /Account/Login
        [HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, change to shouldLockout: true
                var result = await AccountManager.PasswordSignInAsync(model.Username, model.Password);
                if (result.Success)
                {
                    UserToken = result.Token;
                    DisplayUserName = (await AccountManager.GetUserInfoAsync(result.Token))?.FullName;
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    AddError(result.UserErrorMessage);
                }
            }
            return View(model);
        }


        [HttpPost]
        public async Task<ActionResult> FacebookLogin(FacebookLoginModel model)
        {
            var result = await AccountManager.FacebookLogin(model.AccessToken, new FullUser(model.FullName, model.BirthDay, model.City, model.WorkPlace));
            if (result.Success)
            {
                UserToken = result.Token;
                DisplayUserName = (await AccountManager.GetUserInfoAsync(result.Token))?.FullName;

                return RedirectToAction("Index", "Home");
            }
            else
                return View("FacebookLoginError"); 
        }

        //
        // GET: /Account/Register
        //[AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await AccountManager.RegisterAsync(model.Username,model.Password,new FullUser( model.FullName,model.BirthDate,model.City,model.WorkPlace));
            if (result.Success)
            {
                UserToken = result.Token;
                DisplayUserName = (await AccountManager.GetUserInfoAsync(result.Token))?.FullName;

                return RedirectToAction("Index", "Home");
            }
            else
            {
                AddError(result.UserErrorMessage);
                return View(model);
            }

            // If we got this far, something failed, redisplay form
        }


        
        //
        // POST: /Account/LogOff
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            // _accountManager.SignOut();
            UserToken = null;
            DisplayUserName = null;

            return RedirectToAction("Index", "Home");
        }

    }
}