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
            @ViewBag.Parts = ZincParseController.GetPartData(ZincParseController.GetParts("Motherboard"));
            //ZincParseController.SaveGPUsToDB();

            //List<JObject> testlist = new List<JObject>();
            //testlist = ZincParseController.GetPartData(ZincParseController.GetParts("PCI+Card"));
            //int[] datatosend;
            //foreach (var x in testlist)
            //{
            //   // GetMaxScreenResolution(x["product_description"]().ToArray);
            //}

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
        
        public static int[] GetMaxScreenResolution(string[] Data)
        {
            int Index = -1;

            for (int i = 0; i <= Data.Length; i++ )
            {
                if (Data[i].ToLower().Contains("res"))
                {
                    Index = i;
                }
            }

            int MaxResX;
            int MaxResY;

            if (Index < 0)
                return null;

            string ScreenResolution = Regex.Match(Data[Index], @"\d+ x \d+").ToString();
            string[] SplitScreenRes = ScreenResolution.Split(' ');

            MaxResX = int.Parse(SplitScreenRes[0]);
            MaxResY = int.Parse(SplitScreenRes[2]);

            int[] MaxScreenResolution = { MaxResX, MaxResY };
            return MaxScreenResolution;
                       
        }
    }
}