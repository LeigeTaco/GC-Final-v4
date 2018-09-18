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
    public class PartsController : Controller
    {
        // GET: Parts
        public ActionResult SavePart(string chosenPartID, string partType)
        {
            if (chosenPartID == "GPU")
            { ZincParseController.GetSaveGPUToDB(chosenPartID); }
            if (chosenPartID == "CPU")
            { ZincParseController.SaveCPUToDB(chosenPartID); }
            if (chosenPartID == "Motherboard")
            { ZincParseController.SaveMotherboardToDB(chosenPartID); }
            if (chosenPartID == "PCCase")
            { ZincParseController.SavePCCaseToDB(chosenPartID); }
            if (chosenPartID == "PSU")
            { ZincParseController.SavePSUToDB(chosenPartID); }
            if (chosenPartID == "RAM")
            { ZincParseController.SaveRAMToDB(chosenPartID); }
            if (chosenPartID == "OpticalDrive")
            { ZincParseController.SaveOpticalDriverToDB(chosenPartID); }
            if (chosenPartID == "HardDrive")
            { ZincParseController.SaveHardDriveToDB(chosenPartID); }
            if (chosenPartID == "Monitor")
            { ZincParseController.SaveMonitorToDB(chosenPartID); }

            //return RedirectToAction("Create?newPart=" + chosenPartID, new { Controller = "Builds" });     //From Ash <3
            return RedirectToAction("Create", "Builds" , new { newPart = chosenPartID });
        }

        public ActionResult PartDetails(string partid)
        {
            return View("SavePart");
        }

        public ActionResult MoreParts(string partType)
        {

            ViewBag.PartSearch = ZincParseController.GetParts(partType);

            //ViewBag.PartSearch= ZincParseController.GetParts(partType);

            return View();
        }
        public ActionResult Create(string newPart)
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
    }
}