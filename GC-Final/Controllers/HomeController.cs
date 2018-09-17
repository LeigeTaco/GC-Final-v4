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
            @ViewBag.Parts = ZincParseController.GetPartData(ZincParseController.GetParts("PCI+Card"));
            ZincParseController.SaveGPUsToDB();
        
            return View();
        }

        [Authorize(Roles ="Admin")]
        public ActionResult Admin()
        {
            ZincParseController.SaveMotherBoardsToDB();

            return View();
        }


        public ActionResult SavePart (JObject part)
        {


            return RedirectToAction("About");
        }

        public ActionResult MoreParts(string partType)
        {
            ViewBag.PartSearch = ZincParseController.GetParts(partType);

            return View();
        }

    }
}