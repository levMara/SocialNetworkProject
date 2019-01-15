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
    public class ManageController : ControllerBase
    {

        //
        // GET: /Manage/Index

        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            if (!(await Authorized()))
                return AccountLogin();

            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.ChangeDetailsSuccess ? "Your details has been changed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            await RefreshToken();
            return View();

        }

        //
        // GET: /Manage/ChangePassword
        public async Task<ActionResult> ChangePassword()
        {
            if (!(await Authorized()))
                return AccountLogin();

            await RefreshToken();
            return View();
        }

        //
        // GET: /Manage/ChangeDetails
        public async Task<ActionResult> ChangeDetails()
        {
            if (!(await Authorized()))
                return AccountLogin();

            FullUser userModel = await AccountManager.GetUserInfoAsync(UserToken);
            await RefreshToken();
            if (userModel != null)
            {
                return View(new ChangeDetailsViewModel { FullName = userModel.FullName, BirthDate = userModel.BirthDate, City = `.City, WorkPlace = userModel.WorkPlace });
            }
            else
                return View();

        }

        [HttpPost]
        public async Task<ActionResult> ChangeDetails(ChangeDetailsViewModel model)
        {
            if (!(await Authorized()))
                return AccountLogin();

            if (!ModelState.IsValid)
                return View(model);

            var result = await AccountManager.ChangeUserDetailsAsync(UserToken, new FullUser(model.FullName, model.BirthDate, model.City, model.WorkPlace));

            if (result.Success)
            {
                DisplayUserName = model.FullName;
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
            if (!(await Authorized()))
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

        public enum ManageMessageId
        {
            ChangeDetailsSuccess,
            ChangePasswordSuccess,
            Error
        }

#endregion
    }
}