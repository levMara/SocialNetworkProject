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
    public class HomeController : ControllerBase
    {
        public async Task<ActionResult> Index()
        {
            if (!(await Authorized()))
                return AccountLogin();

            var userDetails = await AccountManager.GetUserInfoAsync(UserToken);
            var postsResult = await PostsManager.GetUserFeed(UserToken);
            var profileVisitableUsers = await SocialManager.GetProfileVisitableUsers(UserToken);
            if (postsResult.Success && profileVisitableUsers.Success && userDetails!=null)
            {
                await RefreshToken();
                return View(new IndexViewModel(postsResult.Feed, profileVisitableUsers.Users, userDetails));
            }
            else
            {
                return ErrorView (postsResult.UserErrorMessage);
            }
        }

        public async Task<ActionResult> About()
        {
            ViewBag.Message = "Your application description page.";
            await RefreshToken();
            return View();
        }

        public async Task<ActionResult> Contact()
        {
            ViewBag.Message = "Your contact page.";
            await RefreshToken();
            return View();
        }
    }
}