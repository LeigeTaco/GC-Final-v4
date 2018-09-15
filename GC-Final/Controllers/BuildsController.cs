using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using GC_Final.Models;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Configuration;
using System.Reflection;

namespace GC_Final.Controllers
{
    public class RequireParameterAttribute : ActionMethodSelectorAttribute
    {
        public string ValueName { set; get; }
        public RequireParameterAttribute(string valueName)
        {
            ValueName = valueName;
        }
        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            return (controllerContext.HttpContext.Request[ValueName] != null);
        }
    }

    [Authorize]
    public class BuildsController : Controller
    {
        // GET: Builds
        public ActionResult Create()
        {
            ViewBag.GPUs = GetGPUData(GetGPUs());
            ViewBag.CPUs = GetCPUData(GetCPUs());
            ViewBag.Motherboards = GetMotherboardData(GetMotherboards());
            ViewBag.PSUs = GetPSUData(GetPSUs());
            ViewBag.RAMs = GetRAMData(GetRAMs());
            ViewBag.Cases = GetCaseData(GetCases());
            return View();
        }
        
        [RequireParameter("buildName")]
        public ActionResult Edit(string buildName, string motherboard, string gpu, string cpu, string psu, string casename, string ram)
        {
            Entities ORM = new Entities();
            Build UserBuild = new Build();
            UserBuild.OwnerID = User.Identity.GetUserId().ToString();
            Motherboard tempMB = new Motherboard(motherboard);
            ORM.Motherboards.Add(tempMB);
            UserBuild.Motherboard = tempMB;
            GPU tempGPU = new GPU(gpu);
            ORM.GPUs.Add(tempGPU);
            UserBuild.GPU = tempGPU;
            UserBuild.GPUCount = 1;
            CPU tempCPU = new CPU(cpu);
            ORM.CPUs.Add(tempCPU);
            UserBuild.CPU = tempCPU;
            PSU tempPSU = new PSU(psu);
            ORM.PSUs.Add(tempPSU);
            UserBuild.PSU = tempPSU;
            PCCase tempCase = new PCCase(casename);
            ORM.PCCases.Add(tempCase);
            UserBuild.PCCase = tempCase;
            RAM tempRAM = new RAM(ram);
            ORM.RAMs.Add(tempRAM);
            ORM.SaveChanges();
            UserBuild.MBID = tempMB.MotherboardID;
            UserBuild.GPUID = tempGPU.GPUID;
            UserBuild.CPUID = tempCPU.CPUID;
            UserBuild.PSUID = tempPSU.PSUID;
            UserBuild.CaseID = tempCase.CaseID;
            UserBuild.BuildID = Guid.NewGuid().ToString("D");
            UserBuild.OwnerID = User.Identity.GetUserId().ToString();
            ORM.Builds.Add(UserBuild);
            ORM.SaveChanges();

            return _Edit(UserBuild.BuildID, User.Identity.GetUserId());
        }

        ////edit when given a buildID
        private ActionResult _Edit(string BuildID, string UserID)
        {
            Entities ORM = new Entities();
            Build temp = ORM.Builds.Find(BuildID);

            if (temp.OwnerID == UserID)
            {
                //ViewBag Stuff here
                return View("Edit");
            }
            else
            {
                return View("Display", "Builds", BuildID);
            }
        }

        public ActionResult Edit(string id)
        {
            return _Edit(id, User.Identity.GetUserId());
        }

        [AllowAnonymous]
        public ActionResult Display(string id)
        {
            Entities ORM = new Entities();
            Build temp = ORM.Builds.Find(id);

            if (temp == null)
            {
                ViewBag.Message = "The build you were searching for could not be found!";
                return View("Error");
            }

            return View();
        }

        #region GetParts


        public JObject GetGPUs()
        {
            HttpWebRequest apiRequest = WebRequest.CreateHttp($"https://api.zinc.io/v1/search?query=GPU&page=2&retailer=amazon");
            apiRequest.Headers.Add("Authorization", ConfigurationManager.AppSettings["ZINCkey"]);
            apiRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows Phone OS 7.5; Trident/5.0; IEMobile/9.0)";


            HttpWebResponse apiResponse = (HttpWebResponse)apiRequest.GetResponse();

            StreamReader responseData = new StreamReader(apiResponse.GetResponseStream());

            string info = responseData.ReadToEnd();

            JObject jsoninfo = JObject.Parse(info);

            return jsoninfo;
        }

        public List<JObject> GetGPUData(JObject jsoninfo)
        {
            List<JObject> Parts = new List<JObject>();
            for (int i = 0; i <= 2; i++)
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

        public JObject GetCPUs()
        {
            HttpWebRequest apiRequest = WebRequest.CreateHttp($"https://api.zinc.io/v1/search?query=CPU&page=2&retailer=amazon");
            apiRequest.Headers.Add("Authorization", ConfigurationManager.AppSettings["ZINCkey"]);
            apiRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows Phone OS 7.5; Trident/5.0; IEMobile/9.0)";


            HttpWebResponse apiResponse = (HttpWebResponse)apiRequest.GetResponse();

            StreamReader responseData = new StreamReader(apiResponse.GetResponseStream());

            string info = responseData.ReadToEnd();

            JObject jsoninfo = JObject.Parse(info);

            return jsoninfo;
        }

        public List<JObject> GetCPUData(JObject jsoninfo)
        {
            List<JObject> Parts = new List<JObject>();
            for (int i = 0; i <= 2; i++)
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

        public JObject GetMotherboards()
        {
            HttpWebRequest apiRequest = WebRequest.CreateHttp($"https://api.zinc.io/v1/search?query=Motherboard&page=2&retailer=amazon");
            apiRequest.Headers.Add("Authorization", ConfigurationManager.AppSettings["ZINCkey"]);
            apiRequest.Headers.Add("-u", ConfigurationManager.AppSettings["apizinc"]);
            apiRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows Phone OS 7.5; Trident/5.0; IEMobile/9.0)";


            HttpWebResponse apiResponse = (HttpWebResponse)apiRequest.GetResponse();

            StreamReader responseData = new StreamReader(apiResponse.GetResponseStream());

            string info = responseData.ReadToEnd();

            JObject jsoninfo = JObject.Parse(info);

            return jsoninfo;
        }

        public List<JObject> GetMotherboardData(JObject jsoninfo)
        {
            List<JObject> Parts = new List<JObject>();
            for (int i = 0; i <= 2; i++)
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
        public JObject GetPSUs()
        {
            HttpWebRequest apiRequest = WebRequest.CreateHttp($"https://api.zinc.io/v1/search?query=PSU&page=2&retailer=amazon");
            apiRequest.Headers.Add("Authorization", ConfigurationManager.AppSettings["ZINCkey"]);
            apiRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows Phone OS 7.5; Trident/5.0; IEMobile/9.0)";


            HttpWebResponse apiResponse = (HttpWebResponse)apiRequest.GetResponse();

            StreamReader responseData = new StreamReader(apiResponse.GetResponseStream());

            string info = responseData.ReadToEnd();

            JObject jsoninfo = JObject.Parse(info);

            return jsoninfo;
        }

        public List<JObject> GetPSUData(JObject jsoninfo)
        {
            List<JObject> Parts = new List<JObject>();
            for (int i = 0; i <= 2; i++)
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
        public JObject GetRAMs()
        {
            HttpWebRequest apiRequest = WebRequest.CreateHttp($"https://api.zinc.io/v1/search?query=RAM&page=2&retailer=amazon");
            apiRequest.Headers.Add("Authorization", ConfigurationManager.AppSettings["ZINCkey"]);
            apiRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows Phone OS 7.5; Trident/5.0; IEMobile/9.0)";


            HttpWebResponse apiResponse = (HttpWebResponse)apiRequest.GetResponse();

            StreamReader responseData = new StreamReader(apiResponse.GetResponseStream());

            string info = responseData.ReadToEnd();

            JObject jsoninfo = JObject.Parse(info);

            return jsoninfo;
        }

        public List<JObject> GetRAMData(JObject jsoninfo)
        {
            List<JObject> Parts = new List<JObject>();
            for (int i = 0; i <= 2; i++)
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
        public JObject GetCases()
        {
            HttpWebRequest apiRequest = WebRequest.CreateHttp($"https://api.zinc.io/v1/search?query=Computer+Case&page=2&retailer=amazon");
            apiRequest.Headers.Add("Authorization", ConfigurationManager.AppSettings["ZINCkey"]);
            apiRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows Phone OS 7.5; Trident/5.0; IEMobile/9.0)";


            HttpWebResponse apiResponse = (HttpWebResponse)apiRequest.GetResponse();

            StreamReader responseData = new StreamReader(apiResponse.GetResponseStream());

            string info = responseData.ReadToEnd();

            JObject jsoninfo = JObject.Parse(info);

            return jsoninfo;
        }

        public List<JObject> GetCaseData(JObject jsoninfo)
        {
            List<JObject> Parts = new List<JObject>();
            for (int i = 0; i <= 2; i++)
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
        #endregion
    }
}