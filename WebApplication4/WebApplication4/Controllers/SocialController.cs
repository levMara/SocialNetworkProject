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
               
        [HttpPost]
        public async Task<ActionResult> FollowUser(string otherUserId)
        {
            var followResult = await SocialManager.FollowUser(UserToken, otherUserId);
            return Json(followResult);
        }

        [HttpPost]
        public async Task<ActionResult> UnfollowUser(string otherUserId)
        {
            var unfollowResult = await SocialManager.UnfollowUser(UserToken, otherUserId);
            return Json(unfollowResult);
        }

        public async Task<ActionResult> MyFollows()
        {
            if (!(await Authorized()))
                return AccountLogin();

            var result = await SocialManager.GetUserFollows(UserToken);
            if (result.Success)
            {
                await RefreshToken();
                return View(result.UserFollows);
            }
            else
            {
                return ErrorView(result.UserErrorMessage);
            }

        }

        [HttpPost]
        public async Task<ActionResult> MyFollows(MyFollowsViewModel model)
        {
            if (!(await Authorized()))
                return AccountLogin();

            if (model.UnfollowUserId == null)
            {
                AddError("No user was selected.");
                return View(model);
            }

            var result = await SocialManager.UnfollowUser(UserToken, model.UnfollowUserId);

            if (result.Success)
                return SuccessView("User Unfollowed successfully");
            else
                return ErrorView();

        }

        public async Task<ActionResult> MyFollowers()
        {
            if (!(await Authorized()))
                return AccountLogin();
            

            var result = await SocialManager.GetUserFollowers(UserToken);
            if (result.Success)
            {
                await RefreshToken();
                return View(result.UserFollowers);
            }
            else
            {
                return ErrorView(result.UserErrorMessage);
            }
        }

        [HttpPost]
        public async Task<ActionResult> MyFollowers(MyFollowersViewModel model)
        {
            if (!(await Authorized()))
                return AccountLogin();
            
            if (model.FollowingAction == MyFollowersAction.Block)
            {
                if (model.BlockUserId == null)
                {
                    AddError("No user was selected.");
                    return View(model);
                }
                var result = await SocialManager.BlockUser(UserToken, model.BlockUserId);

                if (result)
                    return SuccessView("User blocked successfully");
                else
                    return ErrorView();
            }
            else if (model.FollowingAction == MyFollowersAction.FollowBack)
            {
                if (model.FollowBackUserId == null)
                {
                    AddError("No user was selected.");
                    return View(model);
                }
                var result = await SocialManager.FollowUser(UserToken, model.FollowBackUserId);
                if (result.Success) {
                    return SuccessView("User followed successfully");
                }
                else
                    return ErrorView();
            }
            else if (model.FollowingAction == MyFollowersAction.Unfollow)
            {
                if (model.UnfollowUserId == null)
                {
                    AddError("No user was selected.");
                    return View(model);
                }
                var result = await SocialManager.UnfollowUser(UserToken, model.UnfollowUserId);
                if (result.Success)
                    return SuccessView("User Unfollowed successfully");
                else
                    return ErrorView();
            }
            else
            {
                AddError("No action was selected.");
                return View(model);
            }

        }


        public async Task<ActionResult> BlockedUsers()
        {

            if (!(await Authorized()))
                return AccountLogin();

            var result = await SocialManager.GetBlockedUsers(UserToken);
            if (result.Success)
            {
                await RefreshToken();
                return View(result.BlockedUsers);
            }
            else
            {
                return ErrorView(result.UserErrorMessage);
            }
        }

        [HttpPost]
        public async Task<ActionResult> BlockedUsers(BlockedUsersViewModel model)
        {
            if (!(await Authorized()))
                return AccountLogin();
            
            if (model.UnblockUserId == null)
            {
                AddError("No user was selected.");
                return View(model);
            }
            else
            {
                var result = await SocialManager.UnblockUser(UserToken, model.UnblockUserId);
                if (result)
                    return SuccessView("User Unblocked successfully");
                else
                    return ErrorView();
            }
        }
        
    }

}