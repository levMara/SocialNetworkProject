using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class PostsController : ControllerBase
    {
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Upload(UploadPostViewModel model)
        {
            ViewBag.UploadPostFormVisible = false;
            if (!(await Authorized()))
                return RedirectToAction("Login", "Account");
            if (!ModelState.IsValid)
                return View(model);

            if (model.ImageFile != null)
            {
                if (model.ImageFile.ContentLength > 1024*1024*4)
                {
                    ViewBag.UploadPostFormVisible = true;
                    AddError("Image file must not exceed 4MB too large");
                    return View(model);
                }
            }
            return RedirectToAction("Index", "Home");
        }


    }
}