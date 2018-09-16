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
            SavePCCasesToDB();

            return View();
        }

        [Authorize(Roles ="Admin")]
        public ActionResult Admin()
        {
            SaveMotherBoardsToDB();

            return View();
        }


        public ActionResult SavePart (JObject part)
        {


            return RedirectToAction("About");
        }

       
        public JObject GetParts(string partType)
        {
            HttpWebRequest apiRequest = WebRequest.CreateHttp($"https://api.zinc.io/v1/search?query=CPU&page=1&retailer=amazon");
            apiRequest.Headers.Add("Authorization", ConfigurationManager.AppSettings["ZINCkey"]); //used to add keys
            //apiRequest.Headers.Add("-u", ConfigurationManager.AppSettings["apizinc"]);
            apiRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows Phone OS 7.5; Trident/5.0; IEMobile/9.0)";


            HttpWebResponse apiResponse = (HttpWebResponse)apiRequest.GetResponse();

            StreamReader responseData = new StreamReader(apiResponse.GetResponseStream());

            string info = responseData.ReadToEnd();

            JObject jsoninfo = JObject.Parse(info);

            return jsoninfo;
        }

        public List<JObject> GetPartData(JObject jsoninfo)
        {
            List<JObject> Parts = new List<JObject>();
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

        public ActionResult SaveGPUsToDB()
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
            return RedirectToAction("Admin");
        }

        public void SaveCPUsToDB()
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
                    tempCPU.Price = null; // (int.Parse(part["price"].ToString())) / 100;
                    tempCPU.Stars = null; //float.Parse(part["stars"].ToString());
                    tempCPU.ImageLink = part["main_image"].ToString();
                    tempCPU.Manufacturer = null;

                    ORM.CPUs.Add(tempCPU);
                    ORM.SaveChanges();
                }
            }
        }

        public ActionResult SaveMotherBoardsToDB()
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
            return RedirectToAction("Admin");
        }
        public ActionResult SavePSUsToDB()
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
            return RedirectToAction("Admin");
        }

        public ActionResult SavePCCasesToDB()
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
            return RedirectToAction("Admin");
        }

        public ActionResult SaveRAMsToDB()
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
            return RedirectToAction("Admin");
        }
    }
}