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
        private Build _Algorithm(string bName, string bID, string oID, Motherboard _MB, CPU _CPU, GPU _GPU, PSU _PSU, RAM _RAM, PCCase _CASE, double? maxPrice)
        {
            //Strings: bName, bID, oID
            //Parts: MB, CPU, GPU, PSU, RAM, PCCase
            //Double: Price

            Entities ORM = new Entities();
            Build _out = new Build();
            _out.BuildName = bName;
            _out.BuildID = bID;
            _out.OwnerID = oID;
            bool isCompatible = false;

            do
            {

                if (maxPrice != null)
                {
                    double mPrice = (double)maxPrice;

                    if (_MB == null)
                    {
                        _MB = ORM.Motherboards.Where(x => x.GetPrice() <= mPrice).First();
                    }

                }


            } while (!isCompatible);

            return _out;

        }

        // GET: Builds
        public ActionResult Create()
        {
            Entities ORM = new Entities();

            ViewBag.GPUs = ORM.GPUs;
            ViewBag.CPUs = ORM.CPUs;
            ViewBag.Motherboards = ORM.Motherboards;
            ViewBag.PSUs = ORM.PSUs;
            ViewBag.RAMs = ORM.RAMs;
            ViewBag.Monitors = ORM.Monitors;
            ViewBag.PCCases = ORM.PCCases;
            ViewBag.HardDrives = ORM.HardDrives;
            ViewBag.OpticalDrivers = ORM.OpticalDrivers;
            ViewBag.PCICards = ORM.PCICards;
            return View();
        }

        public ActionResult Create(newPart)
        {
            Entities ORM = new Entities();

            ViewBag.GPUs = ORM.GPUs;
            ViewBag.CPUs = ORM.CPUs;
            ViewBag.Motherboards = ORM.Motherboards;
            ViewBag.PSUs = ORM.PSUs;
            ViewBag.RAMs = ORM.RAMs;
            ViewBag.Monitors = ORM.Monitors;
            ViewBag.PCCases = ORM.PCCases;
            ViewBag.HardDrives = ORM.HardDrives;
            ViewBag.OpticalDrivers = ORM.OpticalDrivers;
            ViewBag.PCICards = ORM.PCICards;
            return View();
        }

        [RequireParameter("autoComplete")]
        public ActionResult Edit(string buildName, string motherboard, string gpu, string cpu, string psu, string casename, string ram, double? price, string preferredBrands, bool autoComplete)
        {

            Entities ORM = new Entities();
            Build UserBuild = new Build();
            UserBuild.BuildName = buildName;
            UserBuild.BuildID = Guid.NewGuid().ToString("D");
            UserBuild.OwnerID = User.Identity.GetUserId();

            Motherboard _MB = new Motherboard();
            CPU _CPU = new CPU();
            RAM _RAM = new RAM();
            PSU _PSU = new PSU();
            GPU _GPU = new GPU();
            PCCase _CASE = new PCCase();
            if (motherboard != null)
            {
                _MB = ORM.Motherboards.Where(x => x.Name == motherboard).First();
            }
            if (cpu != null)
            {
                _CPU = ORM.CPUs.Where(x => x.Name == cpu).First();
            }
            if (ram != null)
            {
                _RAM = ORM.RAMs.Where(x => x.Name == ram).First();
            }
            if (psu != null)
            {
                _PSU = ORM.PSUs.Where(x => x.Name == psu).First();
            }
            if (gpu != null)
            {
                _GPU = ORM.GPUs.Where(x => x.Name == gpu).First();
            }
            if (casename != null)
            {
                _CASE = ORM.PCCases.Where(x => x.Name == casename).First();
            }

            UserBuild.Glorp(_Algorithm(UserBuild.BuildName, UserBuild.BuildID, UserBuild.OwnerID, _MB, _CPU, _GPU, _PSU, _RAM, _CASE, price));
            ORM.Builds.Add(UserBuild);
            ORM.SaveChanges();

            return _Edit(UserBuild.BuildID, User.Identity.GetUserId());
        }
        
        [RequireParameter("buildName")]
        public ActionResult Edit(string buildName, string motherboard, string gpu, string cpu, string psu, string casename, string ram)
        {
            Entities ORM = new Entities();
            Build UserBuild = new Build();
            BuildsRAM userRam = new BuildsRAM();

            UserBuild.BuildID = Guid.NewGuid().ToString("D");
            UserBuild.OwnerID = User.Identity.GetUserId().ToString();
            UserBuild.BuildName = buildName;
            UserBuild.MBID = ORM.Motherboards.Where(x => x.Name == motherboard).Select(x => x.MotherboardID).ToArray()[0];
            UserBuild.CPUID = ORM.CPUs.Where(x => x.Name == cpu).Select(x => x.CPUID).ToArray()[0];
            UserBuild.GPUID = ORM.GPUs.Where(x => x.Name == gpu).Select(x => x.GPUID).ToArray()[0];
            UserBuild.GPUCount = 1;
            userRam.BuildID = UserBuild.BuildID;
            userRam.RAMID = ORM.RAMs.Where(x => x.Name == ram).Select(x => x.RAMID).ToArray()[0];
            UserBuild.PSUID = ORM.PSUs.Where(x => x.Name == psu).Select(x => x.PSUID).ToArray()[0];
            UserBuild.CaseID = ORM.PCCases.Where(x => x.Name == casename).Select(x => x.CaseID).ToArray()[0];
            ORM.BuildsRAMs.Add(userRam);
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