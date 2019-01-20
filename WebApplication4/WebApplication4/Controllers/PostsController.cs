using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication4.BL;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class PostsController : ControllerBase
    {
        
        public async Task<ActionResult> Upload()
        {
            var getMentionsResult= await AccountManager.GetUserIdsAndNames();
            if (getMentionsResult.Success)
                ViewBag.Mentions = getMentionsResult.UserIdsAndNames?? new List<UserIdAndName>();
            else
                ViewBag.Mentions = new List<UserIdAndName>();

            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Upload(UploadPostViewModel model)
        {
            if (!(await Authorized()))
                return RedirectToAction("Login", "Account");
            if (!ModelState.IsValid)
                return View(model);

            if (model.ImageFile != null)
            {
                if (model.ImageFile.ContentLength > 1024*1024*4)
                {
                    ViewBag.UploadPostFormVisible = true;
                    AddError("Image file size must not exceed 4MB");
                    
                    return View(model);
                }
                else
                {
                    AmazonBucketClient client= new AmazonBucketClient();
                    var x=client.UploadFile(model.ImageFile.InputStream, RandomString());
                }
            }
            var mentionedUserIds = System.Web.Helpers.Json.Decode<string[]>(model.JsonMentions);
          
            return RedirectToAction("Index", "Home");
        }

        #region Helpers

        private string RandomString(int length=50)
        {
            Random rnd = new Random((int)DateTime.Now.ToBinary());
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] arr = new char[length];
            for (int i = 0; i < length; i++)
                arr[i] = chars[rnd.Next(0, chars.Length)];
            return new string(arr);
        }
        #endregion
    }
}