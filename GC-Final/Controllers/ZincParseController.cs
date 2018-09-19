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

            for (int i = 0; i <= 5; i++)
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
                    tempGPU.Description = "x";
                    tempGPU.Brand = part["brand"].ToString();
                    tempGPU.Price = int.Parse(part["price"].ToString());
                    tempGPU.Stars = float.Parse(part["stars"].ToString());
                    tempGPU.ImageLink = part["main_image"].ToString();
                    tempGPU.Manufacturer = "x";
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
        public static ActionResult GetSaveGPUToDB(string partid)
        {
            JObject chosenpart= GetPartData(partid);
            Entities ORM = new Entities();

            GPU tempGPU = new GPU(chosenpart["title"].ToString());

            List<GPU> z = new List<GPU>();
            z = ORM.GPUs.Where(x => x.ProductID == partid).ToList();

            if (z.Count < 1)
            {
                tempGPU.ProductID = chosenpart["product_id"].ToString();
                tempGPU.Description = "x"; //chosenpart["product_description"].ToString();
                tempGPU.Brand = chosenpart["brand"].ToString();
                tempGPU.Price = int.Parse(chosenpart["price"].ToString());
                tempGPU.Stars = float.Parse(chosenpart["stars"].ToString());
                tempGPU.ImageLink = chosenpart["main_image"].ToString();
                tempGPU.Manufacturer = "x";
                int[] res = GetMaxScreenResolution(ParseToArray(chosenpart["feature_bullets"]));
                tempGPU.ResX = res[0];
                tempGPU.ResY = res[1];
                tempGPU.RAMType = GetRAMType(ParseToArray(chosenpart["feature_bullets"]));
                tempGPU.RAMAmount = GetRAMSlots(ParseToArray(chosenpart["feature_bullets"]));
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
                tempCPU.Description = "x";
                tempCPU.Brand = chosenpart["brand"].ToString();
                tempCPU.Price = int.Parse(chosenpart["price"].ToString());
                tempCPU.Stars = float.Parse(chosenpart["stars"].ToString());
                tempCPU.ImageLink = chosenpart["main_image"].ToString();
                tempCPU.Manufacturer = "x";
                tempCPU.MaxSpeed = null;
                tempCPU.Wattage = null;
                tempCPU.Threads = null;
                tempCPU.Speed = null;
                tempCPU.MaxRAM = null;
                tempCPU.Fan = null;
                tempCPU.Cores = null;
                tempCPU.Cache = null;

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
                    tempCPU.Description = "x";
                    tempCPU.Brand = part["brand"].ToString();
                    int price;
                    if (part["price"] == null)
                        { price = 0; }
                    else
                        { price = int.Parse(part["price"].ToString()); }
                    tempCPU.Price = price;
                    tempCPU.Stars = float.Parse(part["stars"].ToString());
                    tempCPU.ImageLink = part["main_image"].ToString();
                    tempCPU.Manufacturer = "x";
                    tempCPU.MaxSpeed = null;
                    tempCPU.Wattage = null;
                    tempCPU.Threads = null;
                    tempCPU.Speed = null;
                    tempCPU.MaxRAM = null;
                    tempCPU.Fan = null;
                    tempCPU.Cores = null;
                    tempCPU.Cache = null;

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
                    tempMB.Description = "x";
                    tempMB.Brand = part["brand"].ToString();
                    tempMB.Price = int.Parse(part["price"].ToString());
                    tempMB.Stars = float.Parse(part["stars"].ToString());
                    tempMB.ImageLink = part["main_image"].ToString();
                    tempMB.Manufacturer = "x";
                    tempMB.RAMType = null;
                    tempMB.Wattage = null;
                    tempMB.Socket = null;
                    tempMB.Socket = null;
                    tempMB.SLILimit = null;
                    tempMB.SATASlots = null;
                    tempMB.RAMType = null;
                    tempMB.RAMSlots = null;
                    tempMB.PCISlots = null;
                    tempMB.FormFactor = null;
                    tempMB.CrossfireLimit = null;
                    tempMB.Chipset = null;

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
                tempObj.Description = "x"; //chosenpart["product_description"].ToString();
                tempObj.Brand = chosenpart["brand"].ToString();
                tempObj.Price = int.Parse(chosenpart["price"].ToString());
                tempObj.Stars = float.Parse(chosenpart["stars"].ToString());
                tempObj.ImageLink = chosenpart["main_image"].ToString();
                tempObj.Manufacturer = "x";
                tempObj.Wattage = null;
                tempObj.Socket = GetSocketType(ParseToArray(chosenpart["feature_bullets"]));
                tempObj.SLILimit = null;
                tempObj.SATASlots = GetSATA_Slots(ParseToArray(chosenpart["feature_bullets"]));
                tempObj.RAMType = GetRAMType(ParseToArray(chosenpart["feature_bullets"]));
                tempObj.RAMSlots = null; GetRAMSlots(ParseToArray(chosenpart["feature_bullets"]));
                tempObj.PCISlots = GetPCI_Slots(ParseToArray(chosenpart["feature_bullets"]));
                tempObj.FormFactor = GetFormFactor(ParseToArray(chosenpart["feature_bullets"]));
                tempObj.CrossfireLimit = null;
                tempObj.Chipset = GetChipset(ParseToArray(chosenpart["feature_bullets"]));

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
                    tempPSU.Description = "x";//part["product_description"].ToString();
                    tempPSU.Brand = part["brand"].ToString();
                    tempPSU.Price = int.Parse(part["price"].ToString());
                    tempPSU.Stars = float.Parse(part["stars"].ToString());
                    tempPSU.ImageLink = part["main_image"].ToString();
                    tempPSU.Manufacturer = "x";
                    tempPSU.Width = null;
                    tempPSU.Wattage = null;
                    tempPSU.Length = null;
                    tempPSU.Height = null;
                    tempPSU.FormFactor = null;

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
                tempObj.Description = "x";
                tempObj.Brand = chosenpart["brand"].ToString();
                tempObj.Price = int.Parse(chosenpart["price"].ToString());
                tempObj.Stars = float.Parse(chosenpart["stars"].ToString());
                tempObj.ImageLink = chosenpart["main_image"].ToString();
                tempObj.Manufacturer = "x";
                tempObj.Width = null;
                tempObj.Wattage = null;
                tempObj.Length = null;
                tempObj.Height = null;
                tempObj.FormFactor = GetFormFactor(ParseToArray(chosenpart["feature_bullets"]));

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
                    tempPCCase.Description = "x";//part["product_description"].ToString();
                    tempPCCase.Brand = part["brand"].ToString();
                    tempPCCase.Price = int.Parse(part["price"].ToString());
                    tempPCCase.Stars = float.Parse(part["stars"].ToString());
                    tempPCCase.ImageLink = part["main_image"].ToString();
                    tempPCCase.Manufacturer = "x";
                    tempPCCase.Width = null;
                    tempPCCase.TwoSlots = null;
                    tempPCCase.ThreeSlots = null;
                    tempPCCase.Style = null;
                    tempPCCase.Length = null;
                    tempPCCase.Height = null;
                    tempPCCase.ExpansionSlots = null;

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
                tempObj.Description = "x";
                tempObj.Brand = chosenpart["brand"].ToString();
                tempObj.Price = int.Parse(chosenpart["price"].ToString());
                tempObj.Stars = float.Parse(chosenpart["stars"].ToString());
                tempObj.ImageLink = chosenpart["main_image"].ToString();
                tempObj.Manufacturer = "x";
                tempObj.Width = null;
                tempObj.TwoSlots = null;
                tempObj.ThreeSlots = null;
                tempObj.Style = null;
                tempObj.Length = null;
                tempObj.Height = null;
                tempObj.ExpansionSlots = null;

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
                    tempRAM.Description = "x";//part["product_description"].ToString();
                    tempRAM.Brand = part["brand"].ToString();
                    tempRAM.Price = int.Parse(part["price"].ToString());
                    tempRAM.Stars = float.Parse(part["stars"].ToString());
                    tempRAM.ImageLink = part["main_image"].ToString();
                    tempRAM.Manufacturer = "x";
                    tempRAM.BusSpeed = null;
                    tempRAM.Quantity = null;
                    tempRAM.RAMType = null;
                    tempRAM.TotalCapacity = null;
                    tempRAM.Voltage = null;

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
                tempObj.Description = "x";
                tempObj.Brand = chosenpart["brand"].ToString();
                tempObj.Price = int.Parse(chosenpart["price"].ToString());
                tempObj.Stars = float.Parse(chosenpart["stars"].ToString());
                tempObj.ImageLink = chosenpart["main_image"].ToString();
                tempObj.Manufacturer = "x";
                tempObj.BusSpeed = null;
                tempObj.Quantity = null;
                tempObj.RAMType = GetRAMType(ParseToArray(chosenpart["feature_bullets"]));
                tempObj.TotalCapacity = null;
                tempObj.Voltage = null;

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
                    tempObj.Description = "x";//part["product_description"].ToString();
                    tempObj.Brand = part["brand"].ToString();
                    tempObj.Price = int.Parse(part["price"].ToString());
                    tempObj.Stars = float.Parse(part["stars"].ToString());
                    tempObj.ImageLink = part["main_image"].ToString();
                    tempObj.Manufacturer = "x";
                    tempObj.RefreshRate = null;
                    tempObj.ResX = null;
                    tempObj.ResY = null;
                    
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
                tempObj.Description = "x";
                tempObj.Brand = chosenpart["brand"].ToString();
                tempObj.Price = int.Parse(chosenpart["price"].ToString());
                tempObj.Stars = float.Parse(chosenpart["stars"].ToString());
                tempObj.ImageLink = chosenpart["main_image"].ToString();
                tempObj.Manufacturer = "x";
                tempObj.RefreshRate = null;
                int[] res = GetMaxScreenResolution(ParseToArray(chosenpart["feature_bullets"]));
                tempObj.ResX = res[0];
                tempObj.ResY = res[1];

                ORM.Monitors.Add(tempObj);
                ORM.SaveChanges();
            }
        }

        public static void SaveHardDrivesToDB()
        {
            List<JObject> searchedparts = new List<JObject>();
            searchedparts = GetPartData(GetParts("Internal+Hard+Drive"));
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
                    tempObj.Description = "x";//part["product_description"].ToString();
                    tempObj.Brand = part["brand"].ToString();
                    tempObj.Price = int.Parse(part["price"].ToString());
                    tempObj.Stars = float.Parse(part["stars"].ToString());
                    tempObj.ImageLink = part["main_image"].ToString();
                    tempObj.Manufacturer = "x";
                    tempObj.BuildDisks = null;
                    tempObj.Capacity = 0;//null;
                    tempObj.CapacityUnits = null;
                    tempObj.Interface = null;
                    tempObj.SlotSize = false;//null;


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
                tempObj.Description = "x";
                tempObj.Brand = chosenpart["brand"].ToString();
                tempObj.Price = int.Parse(chosenpart["price"].ToString());
                tempObj.Stars = float.Parse(chosenpart["stars"].ToString());
                tempObj.ImageLink = chosenpart["main_image"].ToString();
                tempObj.Manufacturer = "x";
                tempObj.Capacity = 0;//null;
                tempObj.CapacityUnits = null;
                tempObj.Interface = null;
                tempObj.SlotSize = false;//null;

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
                    tempObj.Description = "x";//part["product_description"].ToString();
                    tempObj.Brand = part["brand"].ToString();
                    tempObj.Price = int.Parse(part["price"].ToString());
                    tempObj.Stars = float.Parse(part["stars"].ToString());
                    tempObj.ImageLink = part["main_image"].ToString();
                    tempObj.Manufacturer = "x";
                    tempObj.WriteSpeed = 0;// null;
                    tempObj.Wattage = 0;// null;
                    tempObj.Type = null;
                    tempObj.Rewritable = false;// null;
                    tempObj.ReadSpeed = 0;// null;
                    tempObj.Interface = null;


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
                tempObj.Description = "x";
                tempObj.Brand = chosenpart["brand"].ToString();
                tempObj.Price = int.Parse(chosenpart["price"].ToString());
                tempObj.Stars = float.Parse(chosenpart["stars"].ToString());
                tempObj.ImageLink = chosenpart["main_image"].ToString();
                tempObj.Manufacturer = "x";
                tempObj.WriteSpeed = 0;// null;
                tempObj.Wattage = 0;// null;
                tempObj.Type = null;
                tempObj.Rewritable = false;// null;
                tempObj.ReadSpeed = 0;// null;
                tempObj.Interface = null;

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
                    tempObj.Description = "x";//part["product_description"].ToString();
                    tempObj.Brand = part["brand"].ToString();
                    tempObj.Price = int.Parse(part["price"].ToString());
                    tempObj.Stars = float.Parse(part["stars"].ToString());
                    tempObj.ImageLink = part["main_image"].ToString();
                    tempObj.Manufacturer = "x";


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
                tempObj.Description = "x";
                tempObj.Brand = chosenpart["brand"].ToString();
                tempObj.Price = int.Parse(chosenpart["price"].ToString());
                tempObj.Stars = float.Parse(chosenpart["stars"].ToString());
                tempObj.ImageLink = chosenpart["main_image"].ToString();
                tempObj.Manufacturer = "x";

                ORM.PCICards.Add(tempObj);
                ORM.SaveChanges();
            }
        }

        public static string[] ParseToArray(JToken specs)
        {
            int x = 0;
            foreach (JToken y in specs)
            {
                x = x + 1;
            }

            string[] specarray = new string[x];

            foreach (JToken z in specs)
            {
                x = x - 1;
                specarray[x] = z.ToString();
            }
            return specarray;
        }
    }
}

