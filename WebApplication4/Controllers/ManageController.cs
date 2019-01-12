using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebApplication4.Models;
using WebApplication4.BL;
using System.Collections.Generic;

namespace WebApplication4.Controllers
{
    public class ManageController : IdentityControllerBase
    {
        //private AccountManager _accountManager;

        public ManageController()
        {
            //_accountManager = new AccountManager();
        }

        //
        // GET: /Manage/Index

        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            if (!Authorized)
                return AccountLogin();

            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.ChangeDetailsSuccess ? "Your details has been changed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            RefreshToken();
            return View();

        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            if (!Authorized)
                return AccountLogin();

            RefreshToken();
            return View();
        }

        //
        // GET: /Manage/ChangeDetails
        public async Task<ActionResult> ChangeDetails()
        {
            if (!Authorized)
                return AccountLogin();

            UserModel userModel = await AccountManager.GetUserInfoAsync(UserToken);
            RefreshToken();
            if (userModel != null)
                return View(new ChangeDetailsViewModel { FullName = userModel.FullName, BirthDate = userModel.BirthDate, City = userModel.City, WorkPlace = userModel.WorkPlace });
            else
                return View();

        }

        [HttpPost]
        public async Task<ActionResult> ChangeDetails(ChangeDetailsViewModel model)
        {
            if (!Authorized)
                return AccountLogin();

            if (!ModelState.IsValid)
                return View(model);

            var result = await AccountManager.ChangeUserDetails(UserToken, new UserModel(model.FullName, model.BirthDate, model.City, model.WorkPlace));

            if (result.Success)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangeDetailsSuccess });
            }
            else
            {
                AddError(result.UserErrorMessage);
                return View(new { Message = ManageMessageId.Error });
            }
        }


        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!Authorized)
                return AccountLogin();

            if (!ModelState.IsValid)
                return View(model);
            
            var result = await AccountManager.ChangePasswordAsync(UserToken, model.OldPassword, model.NewPassword);
            if (result.Success)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            else
            {
                AddError(result.UserErrorMessage);
                return View();
            }

        }

        #region Helpers
        /*
    // Used for XSRF protection when adding external logins
    private const string XsrfKey = "XsrfId";

    private IAuthenticationManager AuthenticationManager
    {
        get
        {
            return HttpContext.GetOwinContext().Authentication;
        }
    }
    */

        public enum ManageMessageId
        {
            ChangeDetailsSuccess,
            ChangePasswordSuccess,
            Error
        }

#endregion
    }
}