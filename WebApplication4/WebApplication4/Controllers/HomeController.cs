using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class HomeController : ControllerBase
    {
        public async Task<ActionResult> Index()
        {
            await RefreshToken();
            PostModel[] posts=new PostModel[] { new PostModel { Id = "234565434567", Content = "rgdfgdfg\nsdfsdfdsf\nsfsdfsdfdf", Date = new DateTime(2016, 5, 6), Likes = 5, ImageUrl = "https://cdn.houseplans.com/product/q5qkhirat4bcjrr4rpg9fk3q94/w1024.jpg?v=8" } ,
            new PostModel { Id="5455454545445454", Content ="AAAAAAA\nAAAA\nAAAAAA",Date=new DateTime(2017,5,6),Likes=10} };
            return View(posts);
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