using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using GC_Final.Models;
using GC_Final.Controllers;
using System.Text.RegularExpressions;

namespace GC_Final.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            //ViewBag.Parts = GetPartData()

            //@ViewBag.Parts = ZincParseController.GetPartData(ZincParseController.GetParts("Motherboard"));

            ZincParseController.SaveRAMsToDB();


            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Admin()
        {
            ZincParseController.SaveHardDrivesToDB();
            ZincParseController.SaveOpticalDriversToDB();
            ZincParseController.SaveGPUsToDB();
            ZincParseController.SavePCCasesToDB();
            ZincParseController.SavePSUsToDB();
            ZincParseController.SaveRAMsToDB();
            ZincParseController.SaveMotherBoardsToDB();


            return View();
        }

        public ActionResult Testing()
        {
            return View();
        }
      
    }
}

/*
PCI 3.0 x 16 slots - string/string[] and return a byte.
MultiGPU Limit - string/string[] and return byte. (May need one for Crossfire and one for SLI)

    Hard Drive:
Type of Drive(HDD, SSHD, SSD) - string/string[] and return string.
Size(2.5", 3.5", mSATA) - string/string[] and return string.
Read/Write Speed - string/string[] and return int. (Maybe separate Read and Write speeds into to methods and take the lower for HD)

    Optical Drive:
Type of Drive(CD, DVD, Blu-Ray, Floppy) - string/string[] and return string.
Read/Write Speed - Definitely need two methods for this one, re-use the HD ones.
Writable Drive - string/string[] and return bool.
*/