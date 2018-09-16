using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using GC_Final.Models;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Configuration;
using System.Reflection;

namespace GC_Final.Controllers
{
    public class RequireParameterAttribute : ActionMethodSelectorAttribute
    {
        public string ValueName { set; get; }
        public RequireParameterAttribute(string valueName)
        {
            ValueName = valueName;
        }
        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            return (controllerContext.HttpContext.Request[ValueName] != null);
        }
    }

    [Authorize]
    public class BuildsController : Controller
    {
        // GET: Builds
        public ActionResult Create()
        {
            Entities ORM = new Entities();

            ViewBag.GPUs = ORM.GPUs;
            ViewBag.CPUs = ORM.CPUs;
            ViewBag.Motherboards = ORM.Motherboards;
            ViewBag.PSUs = ORM.PSUs;
            ViewBag.RAMs = ORM.RAMs;
            ViewBag.Cases = ORM.PCCases;
            return View();
        }

        
        [RequireParameter("buildName")]
        public ActionResult Edit(string buildName, string motherboard, string gpu, string cpu, string psu, string casename, string ram)
        {
            Entities ORM = new Entities();
            Build UserBuild = new Build();
            //UserBuild.OwnerID = User.Identity.GetUserId().ToString();
            Motherboard tempMB = new Motherboard(motherboard);
            ORM.Motherboards.Add(tempMB);
            UserBuild.Motherboard = tempMB;
            GPU tempGPU = new GPU(gpu);
            ORM.GPUs.Add(tempGPU);
            UserBuild.GPU = tempGPU;
            UserBuild.GPUCount = 1;
            CPU tempCPU = new CPU(cpu);
            ORM.CPUs.Add(tempCPU);
            UserBuild.CPU = tempCPU;
            PSU tempPSU = new PSU(psu);
            ORM.PSUs.Add(tempPSU);
            UserBuild.PSU = tempPSU;
            PCCase tempCase = new PCCase(casename);
            ORM.PCCases.Add(tempCase);
            UserBuild.PCCase = tempCase;
            RAM tempRAM = new RAM(ram);
            ORM.RAMs.Add(tempRAM);
            ORM.SaveChanges();
            UserBuild.MBID = tempMB.MotherboardID;
            UserBuild.GPUID = tempGPU.GPUID;
            UserBuild.CPUID = tempCPU.CPUID;
            UserBuild.PSUID = tempPSU.PSUID;
            UserBuild.CaseID = tempCase.CaseID;
            UserBuild.BuildID = Guid.NewGuid().ToString("D");
            UserBuild.OwnerID = User.Identity.GetUserId().ToString();
            ORM.Builds.Add(UserBuild);
            ORM.SaveChanges();

            return _Edit(UserBuild.BuildID, User.Identity.GetUserId());
        }

        ////edit when given a buildID
        private ActionResult _Edit(string BuildID, string UserID)
        {
            Entities ORM = new Entities();
            Build temp = ORM.Builds.Find(BuildID);

            if (temp.OwnerID == UserID)
            {
                //ViewBag Stuff here
                return View("Edit");
            }
            else
            {
                return View("Display", "Builds", BuildID);
            }
        }

        public ActionResult Edit(string id)
        {
            return _Edit(id, User.Identity.GetUserId());
        }

        [AllowAnonymous]
        public ActionResult Display(string id)
        {
            Entities ORM = new Entities();
            Build temp = ORM.Builds.Find(id);

            if (temp == null)
            {
                ViewBag.Message = "The build you were searching for could not be found!";
                return View("Error");
            }

            return View();
        }


    }
}