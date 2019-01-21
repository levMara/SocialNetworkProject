using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using WebApplication4.Models;
using WebApplication4.BL;

namespace WebApplication4.Controllers
{
    public class SocialController : ControllerBase
    {

        public async Task<ActionResult> MyFollows(MyFollowsMessageId? message)
        {
            if (!(await Authorized()))
                return AccountLogin();

            ViewBag.StatusMessage =
            message == MyFollowsMessageId.UserUnfollowSuccess ? "User unfollowed successfully."
            : message == MyFollowsMessageId.Error ? "An error has occurred."
            : "";

            if (message != null && message == MyFollowsMessageId.Error)
                return View();

            var result = await SocialManager.GetUserFollows(UserToken);
            if (result.Success)
            {
                await RefreshToken();
                return View(result.UserFollows);
            }
            else
            {
                return View(MyFollowsMessageId.Error);
            }

        }

        [HttpPost]
        public async Task<ActionResult> MyFollows(MyFollowsViewModel model)
        {
            if (!(await Authorized()))
                return AccountLogin();

            if (!ModelState.IsValid)
                return View();

            if (model.UnfollowUserId == null)
            {
                AddError("No user was selected.");
                return View();
            }

            var result = await SocialManager.UnfollowUser(UserToken, model.UnfollowUserId);
            if (result)
                return View(MyFollowsMessageId.UserUnfollowSuccess);
            else
            {
                return View(new { Message = MyFollowsMessageId.Error });
            }
        }

        public async Task<ActionResult> MyFollowers(MyFollowersMessageId? message)
        {
            if (!(await Authorized()))
                return AccountLogin();

            ViewBag.StatusMessage =
            message == MyFollowersMessageId.FollowBackUserSuccess ? "User followed back successfully."
            : message == MyFollowersMessageId.UnfollowUserSuccess ? "User unfollowed successfully."
            : message == MyFollowersMessageId.BlockUserSuccess ? "User blocked successfully."
            : message == MyFollowersMessageId.Error ? "An error has occurred."
            : "";

            if (message != null && message == MyFollowersMessageId.Error)
                return View();

            var result = await SocialManager.GetUserFollowers(UserToken);
            if (result.Success)
            {
                await RefreshToken();
                return View(result.UserFollowers);
            }
            else
            {
                return View(MyFollowersMessageId.Error);
            }
        }

        [HttpPost]
        public async Task<ActionResult> MyFollowers(MyFollowersViewModel model)
        {
            if (!(await Authorized()))
                return AccountLogin();

            if (!ModelState.IsValid)
                return View();

            if (model.FollowingAction == MyFollowersAction.Block)
            {
                if (model.BlockUserId == null)
                {
                    AddError("No user was selected.");
                    return View();
                }
                var result = await SocialManager.BlockUser(UserToken, model.BlockUserId);
                if (result)
                    return View(MyFollowersMessageId.BlockUserSuccess);
                else
                    return View(MyFollowersMessageId.Error);
            }
            else if (model.FollowingAction == MyFollowersAction.FollowBack)
            {
                if (model.FollowBackUserId == null)
                {
                    AddError("No user was selected.");
                    return View();
                }
                var result = await SocialManager.FollowUser(UserToken, model.BlockUserId);
                if (result)
                    return View(MyFollowersMessageId.FollowBackUserSuccess);
                else
                    return View(MyFollowersMessageId.Error);
            }
            else if (model.FollowingAction == MyFollowersAction.Unfollow)
            {
                if (model.UnfollowUserId == null)
                {
                    AddError("No user was selected.");
                    return View();
                }
                var result = await SocialManager.UnfollowUser(UserToken, model.UnfollowUserId);
                if (result)
                    return View(MyFollowersMessageId.UnfollowUserSuccess);
                else
                    return View(MyFollowersMessageId.Error);
            }
            else
            {
                AddError("No action was selected.");
                return View();
            }

        }


        public async Task<ActionResult> BlockedUsers(BlockedUsersMessageId? message)
        {

            if (!(await Authorized()))
                return AccountLogin();

            ViewBag.StatusMessage =
            message == BlockedUsersMessageId.UnblockUserSuccess ? "User unblocked successfully."
            : message == BlockedUsersMessageId.Error ? "An error has occurred."
            : "";

            if (message != null && message == BlockedUsersMessageId.Error)
                return View();

            var result = await SocialManager.GetBlockedUsers(UserToken);
            if (result.Success)
            {
                await RefreshToken();
                return View(result.BlockedUsers);
            }
            else
            {
                AddError(result.UserErrorMessage);
                return View(BlockedUsersMessageId.Error);
            }
        }


        [HttpPost]
        public async Task<ActionResult> BlockedUsers(BlockedUsersViewModel model)
        {
            if (!(await Authorized()))
                return AccountLogin();

            if (!ModelState.IsValid)
                return View();

            if (model.UnblockUserId == null)
            {
                AddError("No user was selected.");
                return View();
            }
            else
            {
                var result = await SocialManager.UnblockUser(UserToken, model.UnblockUserId);
                if (result)
                    return View(BlockedUsersMessageId.UnblockUserSuccess);
                else
                    return View(new { Message = BlockedUsersMessageId.Error });
            }
        }


        #region Helpers

        public enum MyFollowsMessageId
        {
            UserUnfollowSuccess,
            Error
        }

        public enum MyFollowersMessageId
        {
            FollowBackUserSuccess,
            UnfollowUserSuccess,
            BlockUserSuccess,
            Error
        }

        public enum BlockedUsersMessageId
        {
            UnblockUserSuccess,
            Error
        }

        #endregion
    }

}