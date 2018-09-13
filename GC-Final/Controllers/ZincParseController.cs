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
//using GC-Final.Models;

namespace GC_Final.Controllers
{
    public partial class ZincParseController : ApiController
    {
        public string PullData()
        {
            for (int i = 1; i < 2; i++) //NEEDS- add multiple page queries
            {
                HttpWebRequest apiRequest = WebRequest.CreateHttp($"https://api.zinc.io/v1/search?query=GPU&page={i}&retailer=amazon");
                apiRequest.Headers.Add("Authorization", ConfigurationManager.AppSettings["apizinc"]); //used to add keys
                apiRequest.Headers.Add("-u", ConfigurationManager.AppSettings["apizinc"]);
                apiRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows Phone OS 7.5; Trident/5.0; IEMobile/9.0)";

                JObject jsoninfo;

                HttpWebResponse apiResponse = (HttpWebResponse)apiRequest.GetResponse();
                if (apiResponse.StatusCode == HttpStatusCode.OK) //http error 200
                {
                    StreamReader responseData = new StreamReader(apiResponse.GetResponseStream());

                    string info = responseData.ReadToEnd();

                    jsoninfo = JObject.Parse(info);
                   // ViewBag.zinc = jsoninfo;
                }
            }
           return ("x");

        }


    }
}