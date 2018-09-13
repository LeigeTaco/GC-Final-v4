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

        public ActionResult Edit(string buildName, string motherboard, string gpu, string cpu, string psu, string casename, string ram)
        {
            BuildDetails temp = new BuildDetails();
            temp.Name = buildName;
            PCEntities ORM = new PCEntities();
            temp.MB = new MotherBoardDetails(ORM.Motherboards.Where(x => x.title.ToLower() == motherboard.ToLower()).ToArray()[0]);
            temp.GPU.Add(new GPUDetails(ORM.GPUs.Where(x => x.title.ToLower() == gpu.ToLower()).ToArray()[0]));
            temp.CPU = new CPUDetails(ORM.CPUs.Where(x => x.title.ToLower() == cpu.ToLower()).ToArray()[0]);
            temp.PSU = new PSUDetails(ORM.PSUs.Where(x => x.title.ToLower() == psu.ToLower()).ToArray()[0]);
            temp.Case = new CaseDetails(ORM.Cases.Where(x => x.title.ToLower() == casename.ToLower()).ToArray()[0]);
            temp.RAM.Add(new RAMDetails(ORM.RAMs.Where(x => x.title.ToLower() == ram.ToLower()).ToArray()[0]));
            Build UserBuild = new Build(temp);
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