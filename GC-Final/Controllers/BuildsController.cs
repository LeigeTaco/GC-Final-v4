using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using GC_Final.Models;

namespace GC_Final.Controllers
{
    [Authorize]
    public class BuildsController : Controller
    {
        // GET: Builds
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(Build UserBuild)
        {
            PCEntities ORM = new PCEntities();
            ORM.Builds.Add(UserBuild);
            ORM.SaveChanges();
            ViewBag.UserBuild = UserBuild;

            return RedirectToAction("_Edit", UserBuild.BuildID);
        }

        //edit when given a buildID
        private ActionResult _Edit(string BuildID)
        {
            PCEntities ORM = new PCEntities();

            ViewBag.UserBuild = ORM.Builds.Where(x => x.BuildID == BuildID).ToList()[0];
            
            return View("Edit");
        }

        public ActionResult Edit(string id)
        {
            PCEntities ORM = new PCEntities();

            if(ORM.Builds.Where(x => x.BuildID == id).ToArray()[0].OwnerID == User.Identity.GetUserId())
            {
                return RedirectToAction("_Edit", id);
            }
            return RedirectToAction("Display", id);
        }

        [AllowAnonymous]
        public ActionResult Display()
        {
            return View();
        }
    }
}