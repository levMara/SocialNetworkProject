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
    public class ProfileController : ControllerBase
    {

        
        public async Task<ActionResult> UserProfile(string id)
        {
            if (!(await Authorized()))
                return AccountLogin();

            var profileResult = await SocialManager.GetUserProfile(UserToken, id);
            if (profileResult.Success)
            {
                
                return View(profileResult.Profile);
            }
            else
                return ErrorView(profileResult.UserErrorMessage);

        }
    }
}