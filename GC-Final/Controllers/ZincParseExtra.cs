using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GC_Final.Controllers
{

    public class MotherBoardDetails
    {
        
        public string MBID { set; get; }
        public string PID { set; get; }
        public string Socket { set; get; }
        public string Chipset { set; get; }
        public byte RAMSlots { set; get; }
        public byte SLI { set; get; }
        public byte XFIRE { set; get; }
        public string FormFactor { set; get; }
        public string Name { set; get; }
        public string Desc { set; get; }
        public string Brand { set; get; }
        public double Price { set { Price = value / 100; } get { return Price; } }
        public string Stars { set; get; }
        public string Manufacturer { set; get; }

        public MotherBoardDetails()
        {

            MBID = "";
            PID = "";
            Socket = "";
            Chipset = "";
            RAMSlots = 0;
            SLI = 0;
            XFIRE = 0;
            FormFactor = "";
            Name = "";
            Desc = "";
            Brand = "";
            Price = 0.00;
            Stars = "";
            Manufacturer = "";

        }

    }

    public partial class ZincParseController : ApiController
    {



    }
}