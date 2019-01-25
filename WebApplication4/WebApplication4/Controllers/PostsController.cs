using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication4.BL;
using WebApplication4.Models;
using static WebApplication4.BL.PostsManager;

namespace WebApplication4.Controllers
{
    public class PostsController : ControllerBase
    {

        public async Task<ActionResult> Upload()
        {
            var getMentionsResult = await AccountManager.GetUserIdsAndNames();
            if (getMentionsResult.Success)
                ViewBag.Mentions = getMentionsResult.UserIdsAndNames ?? new List<UserIdAndName>();
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


            string uploadedImageUrl = null;

            if (model.ImageFile != null)
            {
                if (model.ImageFile.ContentLength > 1024 * 1024 * 4)
                {
                    ViewBag.UploadPostFormVisible = true;
                    AddError("Image file size must not exceed 4MB");

                    return View(model);
                }
                else
                {
                    AmazonBucketClient client = new AmazonBucketClient();
                    uploadedImageUrl = client.UploadFile(model.ImageFile.InputStream, RandomString());
                }
            }

            var mentionedUserIds = System.Web.Helpers.Json.Decode<string[]>(model.JsonMentions);

            var result=await PostsManager.UploadPost(UserToken, new PostModel {Content=model.PostContent,ImageUrl= uploadedImageUrl,Mentions=mentionedUserIds,Permission=model.PostPermission });
            if (result)
            {
                return RedirectToAction("Index","Home");
            }
            else
                return ErrorView("Post was not uploaded");
        }

        public async Task<ActionResult> UploadComment(string postId)
        {
            if (!(await Authorized()))
                return AccountLogin();

            var getMentionsResult = await AccountManager.GetUserIdsAndNames();
            if (getMentionsResult.Success)
                ViewBag.Mentions = getMentionsResult.UserIdsAndNames ?? new List<UserIdAndName>();
            else
                ViewBag.Mentions = new List<UserIdAndName>();

            ViewBag.Post = PostsManager.GetPostById(UserToken, postId);
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UploadComment(UploadCommentViewModel model)
        {
            if (!(await Authorized()))
                return RedirectToAction("Login", "Account");
            if (!ModelState.IsValid)
                return View(model);

            string uploadedImageUrl = null;
            if (model.ImageFile != null)
            {
                if (model.ImageFile.ContentLength > 1024 * 1024 * 4)
                {
                    ViewBag.UploadPostFormVisible = true;
                    AddError("Image file size must not exceed 4MB");

                    return View(model);
                }
                else
                {
                    AmazonBucketClient client = new AmazonBucketClient();
                    uploadedImageUrl = client.UploadFile(model.ImageFile.InputStream, RandomString());
                }
            }
            var mentionedUserIds = System.Web.Helpers.Json.Decode<string[]>(model.JsonMentions);


            var result = await PostsManager.UploadComment(UserToken, new CommentModel { Content = model.CommentContent, ImageUrl = uploadedImageUrl, Mentions = mentionedUserIds },model.PostId);
            if (result)
            {
                return RedirectToAction("Index", "Home");
            }
            else
                return ErrorView("Comment was not uploaded");
        }


        [HttpPost]
        public async Task<ActionResult> Like(string postId)
        {
            var likeResult= await PostsManager.LikePost(UserToken, postId);
            return Json(likeResult);
        }

        [HttpPost]
        public async Task<ActionResult> LikeComment(string commentId)
        {
            var likeResult = await PostsManager.LikeComment(UserToken, commentId);
            return Json(likeResult);
        }

        #region Helpers

        private string RandomString(int length = 50)
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