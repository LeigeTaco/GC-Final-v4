using GC_Final.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using GC_Final.Controllers;

namespace GC_Final.Controllers
{
    public partial class ZincParseController : ApiController
    {
        public object ViewBag { get; private set; }

        public static JObject GetParts(string partType)
        {
            HttpWebRequest apiRequest = WebRequest.CreateHttp($"https://api.zinc.io/v1/search?query={partType}&page=1&retailer=amazon");
            apiRequest.Headers.Add("Authorization", ConfigurationManager.AppSettings["ZINCkey"]);
            apiRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows Phone OS 7.5; Trident/5.0; IEMobile/9.0)";


            HttpWebResponse apiResponse = (HttpWebResponse)apiRequest.GetResponse();

            StreamReader responseData = new StreamReader(apiResponse.GetResponseStream());

            string info = responseData.ReadToEnd();

            JObject jsoninfo = JObject.Parse(info);

            return jsoninfo;
        }

        public static List<JObject> GetPartData(JObject jsoninfo)
        {
            List<JObject> Parts = new List<JObject>();

            if (jsoninfo.Count < 1)
            {
                return null;
            }

            for (int i = 0; i <= 3; i++)
            {
                string x = jsoninfo["results"][i]["product_id"].ToString();

                HttpWebRequest apiRequest = WebRequest.CreateHttp($"https://api.zinc.io/v1/products/{x}?retailer=amazon");
                apiRequest.Headers.Add("Authorization", ConfigurationManager.AppSettings["ZINCKey"]); //used to add keys
                apiRequest.Headers.Add("-u", ConfigurationManager.AppSettings["apizinc"]);
                apiRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows Phone OS 7.5; Trident/5.0; IEMobile/9.0)";

                HttpWebResponse apiResponse = (HttpWebResponse)apiRequest.GetResponse();

                StreamReader responseData = new StreamReader(apiResponse.GetResponseStream());

                string partinfo = responseData.ReadToEnd();

                JObject Temp = JObject.Parse(partinfo);

                Parts.Add(Temp);
            }

            return Parts;
        }

        //overloaed searches specific part
        public static JObject GetPartData(string partid)
        {
            HttpWebRequest apiRequest = WebRequest.CreateHttp($"https://api.zinc.io/v1/products/{partid}?retailer=amazon");
            apiRequest.Headers.Add("Authorization", ConfigurationManager.AppSettings["ZINCKey"]);
            apiRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows Phone OS 7.5; Trident/5.0; IEMobile/9.0)";

            HttpWebResponse apiResponse = (HttpWebResponse)apiRequest.GetResponse();

            //NEEDS - Add if apiresponse error

            StreamReader responseData = new StreamReader(apiResponse.GetResponseStream());

            string partdetails = responseData.ReadToEnd();

            JObject ChosenPart = JObject.Parse(partdetails);

            return ChosenPart;
        }

        //for mass saves
        public static void SaveGPUsToDB()
        {
            List<JObject> gpuparts = new List<JObject>();
            gpuparts = GetPartData(GetParts("GPU"));
            Entities ORM = new Entities();

            foreach (JObject part in gpuparts)
            {
                string y = part["product_id"].ToString();
                GPU tempGPU = new GPU(part["title"].ToString());

                List<GPU> z = new List<GPU>();
                z = ORM.GPUs.Where(x => x.ProductID == y).ToList();

                if (z.Count < 1)
                {
                    tempGPU.ProductID = part["product_id"].ToString();
                    tempGPU.Description = part["product_description"].ToString();
                    tempGPU.Brand = part["brand"].ToString();
                    tempGPU.Price = (int.Parse(part["price"].ToString())) / 100;
                    tempGPU.Stars = null; //float.Parse(part["stars"].ToString());
                    tempGPU.ImageLink = part["main_image"].ToString();
                    tempGPU.Manufacturer = null;
                    tempGPU.ResX = null;
                    tempGPU.ResY = null;
                    tempGPU.RAMType = null;
                    tempGPU.RAMAmount = null;
                    tempGPU.MultiGPULimit = null;
                    tempGPU.MultiGPUType = null;
                    tempGPU.MaxMonitors = null;

                    ORM.GPUs.Add(tempGPU);
                    ORM.SaveChanges();
                }
            }
        }

        //for single save
        public static ActionResult SaveGPUToDB(string partid)
        {
            JObject chosenpart= GetPartData(partid);
            Entities ORM = new Entities();

            GPU tempGPU = new GPU(chosenpart["title"].ToString());

            List<GPU> z = new List<GPU>();
            z = ORM.GPUs.Where(x => x.ProductID == partid).ToList();

            if (z.Count < 1)
            {
                tempGPU.ProductID = chosenpart["product_id"].ToString();
                tempGPU.Description = chosenpart["product_description"].ToString();
                tempGPU.Brand = chosenpart["brand"].ToString();
                tempGPU.Price = (int.Parse(chosenpart["price"].ToString())) / 100;
                tempGPU.Stars = null; //float.Parse(part["stars"].ToString());
                tempGPU.ImageLink = chosenpart["main_image"].ToString();
                tempGPU.Manufacturer = null;
                tempGPU.ResX = null;
                tempGPU.ResY = null;
                tempGPU.RAMType = null;
                tempGPU.RAMAmount = null;
                tempGPU.MultiGPULimit = null;
                tempGPU.MultiGPUType = null;
                tempGPU.MaxMonitors = null;

                ORM.GPUs.Add(tempGPU);
                ORM.SaveChanges();
            }
            return null;
            
        }
        public static void SaveCPUToDB(string partid)
        {
            JObject chosenpart = GetPartData(partid);
            Entities ORM = new Entities();

            CPU tempCPU = new CPU(chosenpart["title"].ToString());

            List<CPU> z = new List<CPU>();
            z = ORM.CPUs.Where(x => x.ProductID == partid).ToList();

            if (z.Count < 1)
            {
                tempCPU.ProductID = chosenpart["product_id"].ToString();
                tempCPU.Description = chosenpart["product_description"].ToString();
                tempCPU.Brand = chosenpart["brand"].ToString();
                tempCPU.Price = null; // (int.Parse(chosenpart["price"].ToString())) / 100;
                tempCPU.Stars = null; //float.Parse(part["stars"].ToString());
                tempCPU.ImageLink = chosenpart["main_image"].ToString();
                tempCPU.Manufacturer = null;

                ORM.CPUs.Add(tempCPU);
                ORM.SaveChanges();
            }
        }
        public static void SaveCPUsToDB()
        {
            List<JObject> searchedparts = new List<JObject>();
            searchedparts = GetPartData(GetParts("CPU"));
            Entities ORM = new Entities();

            foreach (JObject part in searchedparts)
            {
                string y = part["product_id"].ToString();
                CPU tempCPU = new CPU(part["title"].ToString());

                List<CPU> z = new List<CPU>();
                z = ORM.CPUs.Where(x => x.ProductID == y).ToList();

                if (z.Count < 1)
                {
                    tempCPU.ProductID = part["product_id"].ToString();
                    tempCPU.Description = part["product_description"].ToString();
                    tempCPU.Brand = part["brand"].ToString();
                    tempCPU.Price = null; // (int.Parse(chosenpart["price"].ToString())) / 100;
                    tempCPU.Stars = null; //float.Parse(chosenpart["stars"].ToString());
                    tempCPU.ImageLink = part["main_image"].ToString();
                    tempCPU.Manufacturer = null;

                    ORM.CPUs.Add(tempCPU);
                    ORM.SaveChanges();
                }
            }
        }

        public static void SaveMotherBoardsToDB()
        {
            List<JObject> searchedparts = new List<JObject>();
            searchedparts = GetPartData(GetParts("Motherboard"));
            Entities ORM = new Entities();

            foreach (JObject part in searchedparts)
            {
                string y = part["product_id"].ToString();
                Motherboard tempMB = new Motherboard(part["title"].ToString());

                List<Motherboard> z = new List<Motherboard>();
                z = ORM.Motherboards.Where(x => x.ProductID == y).ToList();

                if (z.Count < 1)
                {
                    tempMB.ProductID = part["product_id"].ToString();
                    tempMB.Description = null;//part["product_description"].ToString();
                    tempMB.Brand = part["brand"].ToString();
                    tempMB.Price = (int.Parse(part["price"].ToString())) / 100;
                    tempMB.Stars = float.Parse(part["stars"].ToString());
                    tempMB.ImageLink = part["main_image"].ToString();
                    tempMB.Manufacturer = null;
                    tempMB.RAMType = null;


                    ORM.Motherboards.Add(tempMB);
                    ORM.SaveChanges();
                }
            }
        }

        public static void SaveMotherboardToDB(string partid)
        {
            JObject chosenpart = GetPartData(partid);
            Entities ORM = new Entities();

            Motherboard tempObj = new Motherboard(chosenpart["title"].ToString());

            List<Motherboard> z = new List<Motherboard>();
            z = ORM.Motherboards.Where(x => x.ProductID == partid).ToList();

            if (z.Count < 1)
            {
                tempObj.ProductID = chosenpart["product_id"].ToString();
                tempObj.Description = chosenpart["product_description"].ToString();
                tempObj.Brand = chosenpart["brand"].ToString();
                tempObj.Price = (int.Parse(chosenpart["price"].ToString())) / 100;
                tempObj.Stars = float.Parse(chosenpart["stars"].ToString());
                tempObj.ImageLink = chosenpart["main_image"].ToString();
                tempObj.Manufacturer = null;

                ORM.Motherboards.Add(tempObj);
                ORM.SaveChanges();
            }
        }

        public static void SavePSUsToDB()
        {
            List<JObject> searchedparts = new List<JObject>();
            searchedparts = GetPartData(GetParts("PSU"));
            Entities ORM = new Entities();

            foreach (JObject part in searchedparts)
            {
                string y = part["product_id"].ToString();
                PSU tempPSU = new PSU(part["title"].ToString());

                List<PSU> z = new List<PSU>();
                z = ORM.PSUs.Where(x => x.ProductID == y).ToList();

                if (z.Count < 1)
                {
                    tempPSU.ProductID = part["product_id"].ToString();
                    tempPSU.Description = null;//part["product_description"].ToString();
                    tempPSU.Brand = part["brand"].ToString();
                    tempPSU.Price = (int.Parse(part["price"].ToString())) / 100;
                    tempPSU.Stars = float.Parse(part["stars"].ToString());
                    tempPSU.ImageLink = part["main_image"].ToString();
                    tempPSU.Manufacturer = null;


                    ORM.PSUs.Add(tempPSU);
                    ORM.SaveChanges();
                }
            }
        }

        public static void SavePSUToDB(string partid)
        {
            JObject chosenpart = GetPartData(partid);
            Entities ORM = new Entities();

            PSU tempObj = new PSU(chosenpart["title"].ToString());

            List<PSU> z = new List<PSU>();
            z = ORM.PSUs.Where(x => x.ProductID == partid).ToList();

            if (z.Count < 1)
            {
                tempObj.ProductID = chosenpart["product_id"].ToString();
                tempObj.Description = chosenpart["product_description"].ToString();
                tempObj.Brand = chosenpart["brand"].ToString();
                tempObj.Price = (int.Parse(chosenpart["price"].ToString())) / 100;
                tempObj.Stars = float.Parse(chosenpart["stars"].ToString());
                tempObj.ImageLink = chosenpart["main_image"].ToString();
                tempObj.Manufacturer = null;

                ORM.PSUs.Add(tempObj);
                ORM.SaveChanges();
            }
        }

        public static void SavePCCasesToDB()
        {
            List<JObject> searchedparts = new List<JObject>();
            searchedparts = GetPartData(GetParts("Computer+Case"));
            Entities ORM = new Entities();

            foreach (JObject part in searchedparts)
            {
                string y = part["product_id"].ToString();
                PCCase tempPCCase = new PCCase(part["title"].ToString());

                List<PCCase> z = new List<PCCase>();
                z = ORM.PCCases.Where(x => x.ProductID == y).ToList();

                if (z.Count < 1)
                {
                    tempPCCase.ProductID = part["product_id"].ToString();
                    tempPCCase.Description = null;//part["product_description"].ToString();
                    tempPCCase.Brand = part["brand"].ToString();
                    tempPCCase.Price = null;  //(int.Parse(part["price"].ToString())) / 100;
                    tempPCCase.Stars = null;  //float.Parse(part["stars"].ToString());
                    tempPCCase.ImageLink = part["main_image"].ToString();
                    tempPCCase.Manufacturer = null;


                    ORM.PCCases.Add(tempPCCase);
                    ORM.SaveChanges();
                }
            }
        }

        public static void SavePCCaseToDB(string partid)
        {
            JObject chosenpart = GetPartData(partid);
            Entities ORM = new Entities();

            PCCase tempObj = new PCCase(chosenpart["title"].ToString());

            List<PCCase> z = new List<PCCase>();
            z = ORM.PCCases.Where(x => x.ProductID == partid).ToList();

            if (z.Count < 1)
            {
                tempObj.ProductID = chosenpart["product_id"].ToString();
                tempObj.Description = chosenpart["product_description"].ToString();
                tempObj.Brand = chosenpart["brand"].ToString();
                tempObj.Price = (int.Parse(chosenpart["price"].ToString())) / 100;
                tempObj.Stars = float.Parse(chosenpart["stars"].ToString());
                tempObj.ImageLink = chosenpart["main_image"].ToString();
                tempObj.Manufacturer = null;

                ORM.PCCases.Add(tempObj);
                ORM.SaveChanges();
            }
        }

        public static void SaveRAMsToDB()
        {
            List<JObject> searchedparts = new List<JObject>();
            searchedparts = GetPartData(GetParts("RAM"));
            Entities ORM = new Entities();

            foreach (JObject part in searchedparts)
            {
                string y = part["product_id"].ToString();
                RAM tempRAM = new RAM(part["title"].ToString());

                List<RAM> z = new List<RAM>();
                z = ORM.RAMs.Where(x => x.ProductID == y).ToList();

                if (z.Count < 1)
                {
                    tempRAM.ProductID = part["product_id"].ToString();
                    tempRAM.Description = null;//part["product_description"].ToString();
                    tempRAM.Brand = part["brand"].ToString();
                    tempRAM.Price = 99;  //(int.Parse(part["price"].ToString())) / 100;
                    tempRAM.Stars = 99;  //float.Parse(part["stars"].ToString());
                    tempRAM.ImageLink = part["main_image"].ToString();
                    tempRAM.Manufacturer = null;


                    ORM.RAMs.Add(tempRAM);
                    ORM.SaveChanges();
                }
            }
        }
        public static void SaveRAMToDB(string partid)
        {
            JObject chosenpart = GetPartData(partid);
            Entities ORM = new Entities();

            RAM tempObj = new RAM(chosenpart["title"].ToString());

            List<RAM> z = new List<RAM>();
            z = ORM.RAMs.Where(x => x.ProductID == partid).ToList();

            if (z.Count < 1)
            {
                tempObj.ProductID = chosenpart["product_id"].ToString();
                tempObj.Description = chosenpart["product_description"].ToString();
                tempObj.Brand = chosenpart["brand"].ToString();
                tempObj.Price = (int.Parse(chosenpart["price"].ToString())) / 100;
                tempObj.Stars = float.Parse(chosenpart["stars"].ToString());
                tempObj.ImageLink = chosenpart["main_image"].ToString();
                tempObj.Manufacturer = null;

                ORM.RAMs.Add(tempObj);
                ORM.SaveChanges();
            }
        }

        public static void SaveMonitorsToDB()
        {
            List<JObject> searchedparts = new List<JObject>();
            searchedparts = GetPartData(GetParts("CPU+Monitor"));
            Entities ORM = new Entities();

            foreach (JObject part in searchedparts)
            {
                string y = part["product_id"].ToString();
                Monitor tempObj = new Monitor(part["title"].ToString());

                List<Monitor> z = new List<Monitor>();
                z = ORM.Monitors.Where(x => x.ProductID == y).ToList();
                if (z.Count < 1)
                {
                    tempObj.ProductID = part["product_id"].ToString();
                    tempObj.Description = null;//part["product_description"].ToString();
                    tempObj.Brand = part["brand"].ToString();
                    tempObj.Price = 99;  //(int.Parse(part["price"].ToString())) / 100;
                    tempObj.Stars = 99;  //float.Parse(part["stars"].ToString());
                    tempObj.ImageLink = part["main_image"].ToString();
                    tempObj.Manufacturer = null;


                    ORM.Monitors.Add(tempObj);
                    ORM.SaveChanges();
                }
            }
        }
        public static void SaveMonitorToDB(string partid)
        {
            JObject chosenpart = GetPartData(partid);
            Entities ORM = new Entities();

            Monitor tempObj = new Monitor(chosenpart["title"].ToString());

            List<Monitor> z = new List<Monitor>();
            z = ORM.Monitors.Where(x => x.ProductID == partid).ToList();

            if (z.Count < 1)
            {
                tempObj.ProductID = chosenpart["product_id"].ToString();
                tempObj.Description = chosenpart["product_description"].ToString();
                tempObj.Brand = chosenpart["brand"].ToString();
                tempObj.Price = (int.Parse(chosenpart["price"].ToString())) / 100;
                tempObj.Stars = float.Parse(chosenpart["stars"].ToString());
                tempObj.ImageLink = chosenpart["main_image"].ToString();
                tempObj.Manufacturer = null;

                ORM.Monitors.Add(tempObj);
                ORM.SaveChanges();
            }
        }

        public static void SaveHardDrivesToDB()
        {
            List<JObject> searchedparts = new List<JObject>();
            searchedparts = GetPartData(GetParts("HardDrive"));
            Entities ORM = new Entities();

            foreach (JObject part in searchedparts)
            {
                string y = part["product_id"].ToString();
                HardDrive tempObj = new HardDrive(part["title"].ToString());

                List<HardDrive> z = new List<HardDrive>();
                z = ORM.HardDrives.Where(x => x.ProductID == y).ToList();
                if (z.Count < 1)
                {
                    tempObj.ProductID = part["product_id"].ToString();
                    tempObj.Description = null;//part["product_description"].ToString();
                    tempObj.Brand = part["brand"].ToString();
                    tempObj.Price = 99;  //(int.Parse(part["price"].ToString())) / 100;
                    tempObj.Stars = 99;  //float.Parse(part["stars"].ToString());
                    tempObj.ImageLink = part["main_image"].ToString();
                    tempObj.Manufacturer = null;


                    ORM.HardDrives.Add(tempObj);
                    ORM.SaveChanges();
                }
            }
        }
        public static void SaveHardDriveToDB(string partid)
        {
            JObject chosenpart = GetPartData(partid);
            Entities ORM = new Entities();

            HardDrive tempObj = new HardDrive(chosenpart["title"].ToString());

            List<HardDrive> z = new List<HardDrive>();
            z = ORM.HardDrives.Where(x => x.ProductID == partid).ToList();

            if (z.Count < 1)
            {
                tempObj.ProductID = chosenpart["product_id"].ToString();
                tempObj.Description = chosenpart["product_description"].ToString();
                tempObj.Brand = chosenpart["brand"].ToString();
                tempObj.Price = (int.Parse(chosenpart["price"].ToString())) / 100;
                tempObj.Stars = float.Parse(chosenpart["stars"].ToString());
                tempObj.ImageLink = chosenpart["main_image"].ToString();
                tempObj.Manufacturer = null;

                ORM.HardDrives.Add(tempObj);
                ORM.SaveChanges();
            }
        }

        public static void SaveOpticalDriversToDB()
        {
            List<JObject> searchedparts = new List<JObject>();
            searchedparts = GetPartData(GetParts("Optical+Driver"));
            Entities ORM = new Entities();

            foreach (JObject part in searchedparts)
            {
                string y = part["product_id"].ToString();
                OpticalDriver tempObj = new OpticalDriver(part["title"].ToString());

                List<OpticalDriver> z = new List<OpticalDriver>();
                z = ORM.OpticalDrivers.Where(x => x.ProductID == y).ToList();
                if (z.Count < 1)
                {
                    tempObj.ProductID = part["product_id"].ToString();
                    tempObj.Description = null;//part["product_description"].ToString();
                    tempObj.Brand = part["brand"].ToString();
                    tempObj.Price = 99;  //(int.Parse(part["price"].ToString())) / 100;
                    tempObj.Stars = 99;  //float.Parse(part["stars"].ToString());
                    tempObj.ImageLink = part["main_image"].ToString();
                    tempObj.Manufacturer = null;


                    ORM.OpticalDrivers.Add(tempObj);
                    ORM.SaveChanges();
                }
            }
        }
        public static void SaveOpticalDriverToDB(string partid)
        {
            JObject chosenpart = GetPartData(partid);
            Entities ORM = new Entities();

            OpticalDriver tempObj = new OpticalDriver(chosenpart["title"].ToString());

            List<OpticalDriver> z = new List<OpticalDriver>();
            z = ORM.OpticalDrivers.Where(x => x.ProductID == partid).ToList();

            if (z.Count < 1)
            {
                tempObj.ProductID = chosenpart["product_id"].ToString();
                tempObj.Description = chosenpart["product_description"].ToString();
                tempObj.Brand = chosenpart["brand"].ToString();
                tempObj.Price = (int.Parse(chosenpart["price"].ToString())) / 100;
                tempObj.Stars = float.Parse(chosenpart["stars"].ToString());
                tempObj.ImageLink = chosenpart["main_image"].ToString();
                tempObj.Manufacturer = null;

                ORM.OpticalDrivers.Add(tempObj);
                ORM.SaveChanges();
            }
        }

        public static void SavePCICardsToDB()
        {
            List<JObject> searchedparts = new List<JObject>();
            searchedparts = GetPartData(GetParts("PCI+Card"));
            Entities ORM = new Entities();

            foreach (JObject part in searchedparts)
            {
                string y = part["product_id"].ToString();
                PCICard tempObj = new PCICard(part["title"].ToString());

                List<PCICard> z = new List<PCICard>();
                z = ORM.PCICards.Where(x => x.ProductID == y).ToList();
                if (z.Count < 1)
                {
                    tempObj.ProductID = part["product_id"].ToString();
                    tempObj.Description = null;//part["product_description"].ToString();
                    tempObj.Brand = part["brand"].ToString();
                    tempObj.Price = 99;  //(int.Parse(part["price"].ToString())) / 100;
                    tempObj.Stars = 99;  //float.Parse(part["stars"].ToString());
                    tempObj.ImageLink = part["main_image"].ToString();
                    tempObj.Manufacturer = null;


                    ORM.PCICards.Add(tempObj);
                    ORM.SaveChanges();
                }
            }
        }
        public static void SavePCICardToDB(string partid)
        {
            JObject chosenpart = GetPartData(partid);
            Entities ORM = new Entities();

            PCICard tempObj = new PCICard(chosenpart["title"].ToString());

            List<PCICard> z = new List<PCICard>();
            z = ORM.PCICards.Where(x => x.ProductID == partid).ToList();

            if (z.Count < 1)
            {
                tempObj.ProductID = chosenpart["product_id"].ToString();
                tempObj.Description = chosenpart["product_description"].ToString();
                tempObj.Brand = chosenpart["brand"].ToString();
                tempObj.Price = (int.Parse(chosenpart["price"].ToString())) / 100;
                tempObj.Stars = float.Parse(chosenpart["stars"].ToString());
                tempObj.ImageLink = chosenpart["main_image"].ToString();
                tempObj.Manufacturer = null;

                ORM.PCICards.Add(tempObj);
                ORM.SaveChanges();
            }
        }
    }
}