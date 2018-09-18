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

            @ViewBag.Parts = ZincParseController.GetPartData(ZincParseController.GetParts("PSU"));

            ZincParseController.GetSaveGPUToDB("B01MA62JSZ");


            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Admin()
        {
            ZincParseController.SaveMotherBoardsToDB();

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

            string ScreenResolution = Regex.Match(Data[Index], @"\d+ x \d+").Value;
            string[] SplitScreenRes = ScreenResolution.Split(' ');

            MaxResX = int.Parse(SplitScreenRes[0]);
            MaxResY = int.Parse(SplitScreenRes[2]);

            int[] MaxScreenResolution = { MaxResX, MaxResY };
            return MaxScreenResolution;
                       
        }

        public static string GetSocketType(string Data)
        {
            string[] _dataArray = { Data };
            return GetSocketType(_dataArray);
        }

        public static string GetSocketType(string[] Data)
        {

            for (int i = 0; i <= Data.Length; i++)
            {
                if (Data[i].ToLower().Contains("socket"))
                {
                    return Regex.Match(Data[i], @"^[lpLP][Gg][Aa] \d+$").Value;
                }
            }

            return null;

        }

        public static string GetFormFactor(string Data)
        {
            string[] _dataArray = { Data };
            return GetFormFactor(_dataArray);
        }

        public static string GetFormFactor(string[] Data)
        {
            for (int i = 0; i <= Data.Length; i++)
            {
                if (Data[i].ToLower().Contains("form factor"))
                {
                    string _FFDetails = "";
                    
                    if (Data[i].ToLower().Contains("at"))
                    {
                        _FFDetails += "AT,";
                    }

                    if (Data[i].ToLower().Contains("baby at"))
                    {
                        _FFDetails += "Baby AT,";
                    }

                    if (Data[i].ToLower().Contains("atx"))
                    {
                        _FFDetails += "ATX,";
                    }

                    if (Data[i].ToLower().Contains("mini atx"))
                    {
                        _FFDetails += "Mini ATX,";
                    }

                    if (Data[i].ToLower().Contains("micro atx"))
                    {
                        _FFDetails += "Micro ATX,";
                    }

                    if (Data[i].ToLower().Contains("flex atx"))
                    {
                        _FFDetails += "Flex ATX,";
                    }

                    if (Data[i].ToLower().Contains("lpx"))
                    {
                        _FFDetails += "LPX,";
                    }

                    if (Data[i].ToLower().Contains("nlx"))
                    {
                        _FFDetails += "NLX,";
                    }

                    return _FFDetails;

                }

            }

            return null;

        }

        public static string GetChipset(string Data)
        {
            string[] _dataArray = { Data };
            return GetChipset(_dataArray);
        }

        public static string GetChipset(string[] Data)
        {
            for (int i = 0; i <= Data.Length; i++)
            {
                if (Data[i].ToLower().Contains("chipset"))
                {
                    return Regex.Match(Data[i], @"[a-zA-Z]+\d+[a-zA-Z]?+").Value;
                }

            }

            return null;

        }

        public static string GetRAMType(string Data)
        {
            string[] _dataArray = { Data };
            return GetRAMType(_dataArray);
        }
        
        public static string GetRAMType(string[] Data)
        {

            for (int i = 0; i <= Data.Length; i++)
            {
                if (Data[i].ToLower().Contains("DDR2") || Data[i].ToLower().Contains("DDR3") || Data[i].ToLower().Contains("DDR3 ECC") || Data[i].ToLower().Contains("DDR4") || Data[i].ToLower().Contains("DDR4 ECC"))
                {
                    return Regex.Match(Data[i], @"^DDR\d( ECC)?$").Value;
                }
            }

            return null;

        }

        public static int GetRAMSlots (string[] Data)
        {
            int Index = -1;
            for (int i = 0; i <= Data.Length; i++)
            {
                if (Data[i].ToLower().Contains("memory slots"))
                {
                    Index = i;
                }
            }

            if (Index < 0)
                return 0;

            int MemorySlots = int.Parse(Regex.Match(Regex.Match(Data[Index], @"\d x").Value, @"\d").Value);
            return MemorySlots;

        }

        public static int GetPowerSupply(string Data)
        {
            int PowerSupply = int.Parse(Regex.Match(Regex.Match(Data, @"\d+W").Value, @"\d+").Value);
            return PowerSupply;
        }


    }
}