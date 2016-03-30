using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ListPlanner_GamGam.Extensions;
using ListPlanner_GamGam.Models;
using Microsoft.AspNet.Identity;

namespace ListPlanner_GamGam.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
                //{
                //    UserId = User.Identity.GetUserId(),
                //    Name = User.Identity.GetName(),
                //    UserName = User.Identity.GetUserName(),
                //    ImageUrl = User.Identity.GetImageUrl(),
                //    Email = User.Identity.GetEmail()

                //}

               // );
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}