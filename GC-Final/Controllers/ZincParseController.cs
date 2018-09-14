﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GC_Final.Controllers
{
    public partial class ZincParseController : ApiController
    {
        public JObject GetGPUs()
        {
            JObject jsoninfo;
            string info;
            //for (int i = 1; i < 9; i++) //NEEDS- add multiple page queries
            //{
            HttpWebRequest apiRequest = WebRequest.CreateHttp($"https://api.zinc.io/v1/search?query=GPU&page=1&retailer=amazon");
            apiRequest.Headers.Add("Authorization", ConfigurationManager.AppSettings["apizinc"]); //used to add keys
            apiRequest.Headers.Add("-u", ConfigurationManager.AppSettings["apizinc"]);
            apiRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows Phone OS 7.5; Trident/5.0; IEMobile/9.0)";


            HttpWebResponse apiResponse = (HttpWebResponse)apiRequest.GetResponse();
            //if (apiResponse.StatusCode != HttpStatusCode.OK) //http error 200
            //{
            //    return (error);
            //}

            StreamReader responseData = new StreamReader(apiResponse.GetResponseStream());

            info = responseData.ReadToEnd();

            jsoninfo = JObject.Parse(info);
            
            return jsoninfo;
        }

        public List<JObject> GetGPUData(JObject jsoninfo)
        {
            List<JObject> GPUs = new List<JObject>();
            for (int i = 0; i <= 14; i++)
            {
                string x = jsoninfo["results"][i]["product_id"].ToString();

                HttpWebRequest apiRequest1 = WebRequest.CreateHttp($"https://api.zinc.io/v1/products/{x}?retailer=amazon");
                apiRequest1.Headers.Add("Authorization", ConfigurationManager.AppSettings["apizinc"]); //used to add keys
                apiRequest1.Headers.Add("-u", ConfigurationManager.AppSettings["apizinc"]);
                apiRequest1.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows Phone OS 7.5; Trident/5.0; IEMobile/9.0)";

                HttpWebResponse apiResponse1 = (HttpWebResponse)apiRequest1.GetResponse();

                //NEEDS - Add if apiresponse error

                StreamReader responseData1 = new StreamReader(apiResponse1.GetResponseStream());

                string gpuinfo = responseData1.ReadToEnd();

                JObject Temp = JObject.Parse(gpuinfo);

                GPUs.Add(Temp);
            }

            return GPUs;
        }

        public JObject GetParts(string PartType)
        {
            JObject partlist;
            string partinfo;
            //for (int i = 1; i < 9; i++) //NEEDS- add multiple page queries
            //{
            HttpWebRequest apiRequest = WebRequest.CreateHttp($"https://api.zinc.io/v1/search?query={PartType}&page=1&retailer=amazon");
            apiRequest.Headers.Add("Authorization", ConfigurationManager.AppSettings["apizinc"]); //used to add keys
            apiRequest.Headers.Add("-u", ConfigurationManager.AppSettings["apizinc"]);
            apiRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows Phone OS 7.5; Trident/5.0; IEMobile/9.0)";


            HttpWebResponse apiResponse = (HttpWebResponse)apiRequest.GetResponse();
            //if (apiResponse.StatusCode != HttpStatusCode.OK) //http error 200
            //{
            //    return (error);
            //}

            StreamReader responseData = new StreamReader(apiResponse.GetResponseStream());

            partinfo = responseData.ReadToEnd();

            partlist = JObject.Parse(partinfo);

            return partlist;
        }

        public List<JObject> GetPartData(JObject jsonpartinfo)
        {
            List<JObject> Parts = new List<JObject>();
            for (int i = 0; i <= 14; i++)
            {
                string x = jsonpartinfo["results"][i]["product_id"].ToString();

                HttpWebRequest apiRequest1 = WebRequest.CreateHttp($"https://api.zinc.io/v1/products/{x}?retailer=amazon");
                apiRequest1.Headers.Add("Authorization", ConfigurationManager.AppSettings["apizinc"]); //used to add keys
                //apiRequest1.Headers.Add("-u", ConfigurationManager.AppSettings["apizinc"]);
                apiRequest1.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows Phone OS 7.5; Trident/5.0; IEMobile/9.0)";

                HttpWebResponse apiResponse1 = (HttpWebResponse)apiRequest1.GetResponse();

                //NEEDS - Add if apiresponse error

                StreamReader responseData1 = new StreamReader(apiResponse1.GetResponseStream());

                string partdetails = responseData1.ReadToEnd();

                JObject Temp = JObject.Parse(partdetails);

                Parts.Add(Temp);
            }

            return Parts;
        }

        //public List<JObject> GetGPUs()
        //{
        //    JObject jsoninfo;
        //    string info;
        //    //for (int i = 1; i < 9; i++) //NEEDS- add multiple page queries
        //    //{
        //    HttpWebRequest apiRequest = WebRequest.CreateHttp($"https://api.zinc.io/v1/search?query=GPU&page=1&retailer=amazon");
        //    apiRequest.Headers.Add("Authorization", ConfigurationManager.AppSettings["apizinc"]); //used to add keys
        //    apiRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows Phone OS 7.5; Trident/5.0; IEMobile/9.0)";


        //    HttpWebResponse apiResponse = (HttpWebResponse)apiRequest.GetResponse();
        //    //if (apiResponse.StatusCode != HttpStatusCode.OK) //http error 200
        //    //{
        //    //    return (error);
        //    //}

        //    StreamReader responseData = new StreamReader(apiResponse.GetResponseStream());

        //    info = responseData.ReadToEnd();

        //    jsoninfo = JObject.Parse(info);

        //    List<JObject> GPUs = new List<JObject>();

        //    for (int i = 0; i < 1; i++)
        //    {
        //        string x = jsoninfo["results"][i]["product_id"].ToString();

        //        HttpWebRequest apiRequest1 = WebRequest.CreateHttp($"https://api.zinc.io/v1/products/{x}?retailer=amazon");
        //        apiRequest1.Headers.Add("Authorization", ConfigurationManager.AppSettings["apizinc"]); //used to add keys

        //        //HttpWebResponse apiResponse1 = (HttpWebResponse)apiResponse.GetResponseStream();

        //        //NEEDS - Add if apiresponse error

        //        StreamReader responseData1 = new StreamReader(apiResponse.GetResponseStream());
        //        string gpuinfo = responseData1.ReadToEnd();
        //        JObject Temp = JObject.Parse(gpuinfo);
        //        GPUs.Add(Temp);
        //    }

        //    return GPUs;          
        //}
    }
}