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
        public ActionResult SavePart(string partDetails)//, string chosenPartID)
        {
            string[] variables = partDetails.Split('+');
            string partType = variables[0];
            string chosenPartID = variables[1];
            if (partType == "GPU")
            { ZincParseController.GetSaveGPUToDB(chosenPartID); }
            if (partType == "CPU")
            { ZincParseController.SaveCPUToDB(chosenPartID); }
            if (partType == "Motherboard")
            { ZincParseController.SaveMotherboardToDB(chosenPartID); }
            if (partType == "PCCase")
            { ZincParseController.SavePCCaseToDB(chosenPartID); }
            if (partType == "PSU")
            { ZincParseController.SavePSUToDB(chosenPartID); }
            if (partType == "RAM")
            { ZincParseController.SaveRAMToDB(chosenPartID); }
            if (partType == "Optical+Drive")
            { ZincParseController.SaveOpticalDriverToDB(chosenPartID); }
            if (partType == "HardDrive")
            { ZincParseController.SaveHardDriveToDB(chosenPartID); }
            if (partType == "Monitor")
            { ZincParseController.SaveMonitorToDB(chosenPartID); }
            if (partType == "PCICard")
            { ZincParseController.SavePCICardToDB(chosenPartID); }

            return RedirectToAction("Create", "Builds");

           // return RedirectToAction("Create", "Builds" , new { newPart = chosenPartID });
        }

        public ActionResult MoreDetails(string chosenPartID)
        {               
            JObject x = ZincParseController.GetPartData(chosenPartID);
            //List<string> images = new List<string>();
            ViewBag.Part = x;
            ViewBag.PartDetails = ZincParseController.ParseToArray(x["feature_bullets"]);
            ViewBag.PartImages = ZincParseController.ParseToArray(x["images"]);

            //for (int i=0; i <5; i++)
            //{
            //    //if (x["images"][i].ToString() == null)5
            //    //{
            //    //    i = 11;
            //    //}
            //    images.Add(x["images"][i].ToString());
            //}
            //foreach (string y in x["images"])
            //{
            //    images.Add(y.ToString());
            //}
            //ViewBag.PartDetails = images;

            return View();
        }

        public ActionResult MoreParts(string partType)
        {
            if (partType == "PCCase") { partType = "Computer+Case"; }
            if (partType == "OpticalDriver") { partType = "Optical+Drive"; }
            if (partType == "HardDrive") { partType = "Internal+Hard+Drive"; }
            if (partType == "PCICard") { partType = "PCI+Card"; }
            if (partType == "Monitor") { partType = "Monitor"; }
            ViewBag.PartSearch = ZincParseController.GetParts(partType);

            ViewBag.PartType = partType;
            ViewBag.page = 1;

            return View();
        }
       
    }
}