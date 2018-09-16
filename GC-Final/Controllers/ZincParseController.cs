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

namespace GC_Final.Controllers
{
    public partial class ZincParseController : ApiController
    {
        public object ViewBag { get; private set; }

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
            List<JObject> GPUs = new List<JObject>();
            for (int i = 4; i <= 7; i++)
            {
                string x = jsoninfo["results"][i]["product_id"].ToString();

                HttpWebRequest apiRequest1 = WebRequest.CreateHttp($"https://api.zinc.io/v1/products/{x}?retailer=amazon");
                apiRequest1.Headers.Add("Authorization", ConfigurationManager.AppSettings["ZINCKey"]); //used to add keys
                apiRequest1.Headers.Add("-u", ConfigurationManager.AppSettings["apizinc"]);
                apiRequest1.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows Phone OS 7.5; Trident/5.0; IEMobile/9.0)";

                HttpWebResponse apiResponse1 = (HttpWebResponse)apiRequest1.GetResponse();

                StreamReader responseData1 = new StreamReader(apiResponse1.GetResponseStream());

                string gpuinfo = responseData1.ReadToEnd();

                JObject Temp = JObject.Parse(gpuinfo);

                GPUs.Add(Temp);
            }

            return GPUs;
        }

        //overloaed searches specific part
        public JObject GetPartData(string partid)
        {
            HttpWebRequest apiRequest1 = WebRequest.CreateHttp($"https://api.zinc.io/v1/products/{partid}?retailer=amazon");
            apiRequest1.Headers.Add("Authorization", ConfigurationManager.AppSettings["ZINCKey"]); //used to add keys
            //apiRequest1.Headers.Add("-u", ConfigurationManager.AppSettings["apizinc"]);
            apiRequest1.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows Phone OS 7.5; Trident/5.0; IEMobile/9.0)";

            HttpWebResponse apiResponse1 = (HttpWebResponse)apiRequest1.GetResponse();

            //NEEDS - Add if apiresponse error

            StreamReader responseData1 = new StreamReader(apiResponse1.GetResponseStream());

            string partdetails = responseData1.ReadToEnd();

            JObject ChosenPart = JObject.Parse(partdetails);

            return ChosenPart;
        }

        //for mass saves
        public void SaveGPUsToDB()
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
        public ActionResult SaveGPUToDB(string partid)
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
        public void SaveCPUToDB(string partid)
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
    }
}