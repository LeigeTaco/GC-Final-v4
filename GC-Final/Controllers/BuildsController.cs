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
        private Build _Algorithm(string bName, string bID, string oID, Motherboard _MB, CPU _CPU, GPU _GPU, PSU _PSU, RAM _RAM, PCCase _CASE, double? maxPrice)
        {
            //Strings: bName, bID, oID
            //Parts: MB, CPU, GPU, PSU, RAM, PCCase
            //Double: Price

            Entities ORM = new Entities();
            Build _out = new Build();
            _out.BuildName = bName;
            _out.BuildID = bID;
            _out.OwnerID = oID;
            bool isCompatible = false;

            do
            {

                if (maxPrice != null)
                {
                    double mPrice = (double)maxPrice;

                    if (_MB == null)
                    {
                        _MB = ORM.Motherboards.Where(x => x.GetPrice() <= mPrice).First();
                    }

                }


            } while (!isCompatible);

            return _out;

        }

        // GET: Builds
        public ActionResult Create()
        {
            Entities ORM = new Entities();

            ViewBag.GPUs = ORM.GPUs;
            ViewBag.CPUs = ORM.CPUs;
            ViewBag.Motherboards = ORM.Motherboards;
            ViewBag.PSUs = ORM.PSUs;
            ViewBag.RAMs = ORM.RAMs;
            ViewBag.Monitors = ORM.Monitors;
            ViewBag.PCCases = ORM.PCCases;
            ViewBag.HardDrives = ORM.HardDrives;
            ViewBag.OpticalDrivers = ORM.OpticalDrivers;
            ViewBag.PCICards = ORM.PCICards;
            return View();
        }

       
        public ActionResult Create(newPart)
        {
            Entities ORM = new Entities();

            ViewBag.GPUs = ORM.GPUs;
            ViewBag.CPUs = ORM.CPUs;
            ViewBag.Motherboards = ORM.Motherboards;
            ViewBag.PSUs = ORM.PSUs;
            ViewBag.RAMs = ORM.RAMs;
            ViewBag.Monitors = ORM.Monitors;
            ViewBag.PCCases = ORM.PCCases;
            ViewBag.HardDrives = ORM.HardDrives;
            ViewBag.OpticalDrivers = ORM.OpticalDrivers;
            ViewBag.PCICards = ORM.PCICards;
            return View();
        }
        

        [RequireParameter("autoComplete")]
        public ActionResult Edit(string buildName, string motherboard, string gpu, string cpu, string psu, string casename, string ram, double? price, string preferredBrands, bool autoComplete)
        {

            Entities ORM = new Entities();
            Build UserBuild = new Build();
            UserBuild.BuildName = buildName;
            UserBuild.BuildID = Guid.NewGuid().ToString("D");
            UserBuild.OwnerID = User.Identity.GetUserId();

            Motherboard _MB = new Motherboard();
            CPU _CPU = new CPU();
            RAM _RAM = new RAM();
            PSU _PSU = new PSU();
            GPU _GPU = new GPU();
            PCCase _CASE = new PCCase();
            if (motherboard != null)
            {
                _MB = ORM.Motherboards.Where(x => x.Name == motherboard).First();
            }
            if (cpu != null)
            {
                _CPU = ORM.CPUs.Where(x => x.Name == cpu).First();
            }
            if (ram != null)
            {
                _RAM = ORM.RAMs.Where(x => x.Name == ram).First();
            }
            if (psu != null)
            {
                _PSU = ORM.PSUs.Where(x => x.Name == psu).First();
            }
            if (gpu != null)
            {
                _GPU = ORM.GPUs.Where(x => x.Name == gpu).First();
            }
            if (casename != null)
            {
                _CASE = ORM.PCCases.Where(x => x.Name == casename).First();
            }

            UserBuild.Glorp(_Algorithm(UserBuild.BuildName, UserBuild.BuildID, UserBuild.OwnerID, _MB, _CPU, _GPU, _PSU, _RAM, _CASE, price));
            ORM.Builds.Add(UserBuild);
            ORM.SaveChanges();

            return _Edit(UserBuild.BuildID, User.Identity.GetUserId());
        }
        
        [RequireParameter("buildName")]
        public ActionResult Edit(string buildName, string motherboard, string gpu, string cpu, string psu, string casename, string ram)
        {
            Entities ORM = new Entities();
            Build UserBuild = new Build();
            BuildsRAM userRam = new BuildsRAM();

            UserBuild.BuildID = Guid.NewGuid().ToString("D");
            UserBuild.OwnerID = User.Identity.GetUserId().ToString();
            UserBuild.BuildName = buildName;
            UserBuild.MBID = ORM.Motherboards.Where(x => x.Name == motherboard).Select(x => x.MotherboardID).ToArray()[0];
            UserBuild.CPUID = ORM.CPUs.Where(x => x.Name == cpu).Select(x => x.CPUID).ToArray()[0];
            UserBuild.GPUID = ORM.GPUs.Where(x => x.Name == gpu).Select(x => x.GPUID).ToArray()[0];
            UserBuild.GPUCount = 1;
            userRam.BuildID = UserBuild.BuildID;
            userRam.RAMID = ORM.RAMs.Where(x => x.Name == ram).Select(x => x.RAMID).ToArray()[0];
            UserBuild.PSUID = ORM.PSUs.Where(x => x.Name == psu).Select(x => x.PSUID).ToArray()[0];
            UserBuild.CaseID = ORM.PCCases.Where(x => x.Name == casename).Select(x => x.CaseID).ToArray()[0];
            ORM.BuildsRAMs.Add(userRam);
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
                return RedirectToAction("Display", new { id = BuildID });
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
            Build _build = ORM.Builds.Find(id);

            if (_build == null)
            {
                ViewBag.Message = "The build you were searching for could not be found!";
                return View("Error");
            }

            Motherboard _mb = _build.Motherboard;
            CPU _cpu = _build.CPU;
            GPU _gpu = _build.GPU;
            int _count = (int)_build.GPUCount;
            PSU _psu = _build.PSU;
            PCCase _case = _build.PCCase;
            RAM[] _ram = _build.BuildsRAMs.Select(x => x.RAM).ToArray();
            HardDrive[] _hd = _build.BuildDisks.Select(x => x.HardDrive).ToArray();
            OpticalDriver[] _od = _build.BuildODs.Select(x => x.OpticalDriver).ToArray();
            PCICard[] _pci = _build.BuildPCIs.Select(x => x.PCICard).ToArray();
            Peripheral[] _per = _build.BuildsPeripherals.Select(x => x.Peripheral).ToArray();
            Monitor[] _mon = _build.BuildMonitors.Select(x => x.Monitor).ToArray();

            if (_mb != null)
            {
                ViewBag.MB_Exists = true;
                ViewBag.MB_Name = _mb.Name;                 //All of these need to be
                ViewBag.MB_Description = _mb.Description;   //Validated in Razor
                ViewBag.MB_Brand = _mb.Brand;
                ViewBag.MB_Stars = _mb.Stars;
                ViewBag.MB_Manufacturer = _mb.Manufacturer;
                ViewBag.MB_Price = _mb.GetPrice();          //Remember this is a double
                ViewBag.MB_Chipset = _mb.Chipset;           //Even though price is an
                ViewBag.MB_SLI = _mb.SLILimit;              //int in the databse.
                ViewBag.MB_XFire = _mb.CrossfireLimit;
                ViewBag.MB_Socket = _mb.Socket;
                ViewBag.MB_PCI = _mb.PCISlots;
                ViewBag.MB_SATA = _mb.SATASlots;
                ViewBag.MB_RAMType = _mb.RAMType;
                ViewBag.MB_RAMSlots = _mb.RAMSlots;
                ViewBag.MB_Image = _mb.ImageLink;
                ViewBag.MB_Wattage = _mb.Wattage;
                ViewBag.MB_PID = _mb.ProductID;
                ViewBag.MB_FF = _mb.FormFactor;
            }
            if (_cpu != null)
            {
                ViewBag.CPU_Exists = true;
                ViewBag.CPU_Name = _cpu.Name;
                ViewBag.CPU_Description = _cpu.Description;
                ViewBag.CPU_Brand = _cpu.Brand;
                ViewBag.CPU_Image = _cpu.ImageLink;
                ViewBag.CPU_Stars = _cpu.Stars;
                ViewBag.CPU_Price = _cpu.GetPrice();
                ViewBag.CPU_Manufacturer = _cpu.Manufacturer;
                ViewBag.CPU_Socket = _cpu.Socket;
                ViewBag.CPU_PID = _cpu.ProductID;
                ViewBag.CPU_Cache = _cpu.Cache;
                ViewBag.CPU_Wattage = _cpu.Wattage;
                ViewBag.CPU_Fan = _cpu.Fan;
                ViewBag.CPU_Threads = _cpu.Threads;
                ViewBag.CPU_Cores = _cpu.Cores;
                ViewBag.CPU_MRAM = _cpu.MaxRAM;
                ViewBag.CPU_Speed = _cpu.Speed;
                ViewBag.CPU_OC = _cpu.MaxSpeed;
            }
            if (_gpu != null)
            {
                ViewBag.GPU_Exists = true;
                ViewBag.GPU_Name = _gpu.Name;
                ViewBag.GPU_Description = _gpu.Description;
                ViewBag.GPU_Brand = _gpu.Brand;
                ViewBag.GPU_Image = _gpu.ImageLink;
                ViewBag.GPU_Stars = _gpu.Stars;
                ViewBag.GPU_Price = _gpu.GetPrice();
                ViewBag.GPU_PID = _gpu.ProductID;
                ViewBag.GPU_Limit = _gpu.MultiGPULimit;
                ViewBag.GPU_MType = _gpu.MultiGPUType;
                ViewBag.GPU_MaxMonitors = _gpu.MaxMonitors;
                ViewBag.GPU_MaxX = _gpu.ResX;
                ViewBag.GPU_MaxY = _gpu.ResY;
                ViewBag.GPU_RAMType = _gpu.RAMType;
                ViewBag.GPU_RAMAmount = _gpu.RAMAmount;
            }
            if (_psu != null)
            {
                ViewBag.PSU_Exists = true;
                ViewBag.PSU_Name = _psu.Name;
                ViewBag.PSU_Description = _psu.Description;
                ViewBag.PSU_Brand = _psu.Brand;
                ViewBag.PSU_Price = _psu.GetPrice();
                ViewBag.PSU_Stars = _psu.Stars;
                ViewBag.PSU_Manufacturer = _psu.Manufacturer;
                ViewBag.PSU_Image = _psu.ImageLink;
                ViewBag.PSU_PID = _psu.ProductID;
                ViewBag.PSU_FF = _psu.FormFactor;
                ViewBag.PSU_Wattage = _psu.Wattage;
                ViewBag.PSU_Dimensions = new int[] { (int)_psu.Height, (int)_psu.Length, (int)_psu.Width };
            }
            if (_case != null)
            {
                ViewBag.Case_Exists = true;
                ViewBag.Case_Name = _case.Name;
                ViewBag.Case_Description = _case.Description;
                ViewBag.Case_Brand = _case.Brand;
                ViewBag.Case_Price = _case.GetPrice();
                ViewBag.Case_Stars = _case.Stars;
                ViewBag.Case_Image = _case.ImageLink;
                ViewBag.Case_Manufacturer = _case.Manufacturer;
                ViewBag.Case_FF = _case.Style;
                ViewBag.Case_PID = _case.ProductID;
                ViewBag.Case_2Slots = _case.TwoSlots;
                ViewBag.Case_3Slots = _case.ThreeSlots;
                ViewBag.Case_XSlots = _case.ExpansionSlots;
                ViewBag.Case_Dimensions = new int[] { (int)_case.Height, (int)_case.Length, (int)_case.Width };

            }

            return View();
        }
    }
}