using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GC_Final.Models;

namespace GC_Final.Controllers
{

    public class PartDetails
    {

        public string Name { set; get; }
        public string ProductID { set; get; }
        public string Desc { set; get; }
        public string Brand { set; get; }
        public double Price { set { Price = value / 100; } get { return Price; } }
        public string Stars { set; get; }
        public string Manufacturer { set; get; }

        public PartDetails()
        {
            Name = "";
            ProductID = "";
            Desc = "";
            Brand = "";
            Price = 0.00;
            Stars = "";
            Manufacturer = "";
        }

    }

    public class PartBag
    {
        private List<PartDetails> _Bag;

        public int Count { get { return _Bag.Count; } }
        public List<PartDetails> AsList { get { return _Bag; } }
        public Dictionary<string, int> Quantities {
            get
            {
                Dictionary<string, int> _out = new Dictionary<string, int>();

                foreach (PartDetails p in _Bag)
                {
                    if (_out[p.Name] != 0)
                    {
                        _out[p.Name]++;
                    }
                    else
                    {
                        _out.Add(p.Name, 1);
                    }
                }

                return _out;
            }
        }

        public void Add(PartDetails part)
        {
            _Bag.Add(part);
        }
        //Add method to add by ID
        public void Remove(PartDetails part)
        {
            _Bag.Remove(part);
        }
        public List<PartDetails> ToList()
        {
            return _Bag;
        }
    }

    public class BuildDetails
    {
        
        public string Name { set; get; }
        public string OwnerID { set; get; }
        public CaseDetails Case { set; get; }
        public MotherBoardDetails MB { set; get; }
        public CPUDetails CPU { set; get; }
        public GPUDetails GPU { set; get; }
        public byte GPUCount { set; get; }
        public PSUDetails PSU { set; get; }
        public PartBag RAM { set; get; }
        public PartBag Monitor { set; get; }
        public PartBag OD { set; get; }
        public PartBag HD { set; get; }
        public PartBag Peripherals { set; get; }
        public PartBag PCI { set; get; }
        /*
        public Dictionary<string, string> CheckCompatability()
        {
            Dictionary<string, string> Flags = new Dictionary<string, string>();

            //Check Case and Board
            bool compat = false;
            foreach (string FF in Case.Style.Split(','))
            {
                if (FF == MB.FormFactor)
                {
                    compat = true;
                }
            }
            if (!compat)
            {
                Flags.Add("00x01", "Case does not fit Motherboard!");
            }

            //Check Case and Drive Count
            compat = true;
            if (Case.Drives != (OD.Count + HD.Count).ToString())
            {
                Flags.Add("00x07", "Case cannot fit this many drives!");
            }

            //Split above Check into OD and HD?
            //compat = true;

            //Check Board and CPU
            compat = true;
            if (MB.Socket != CPU.Socket)
            {
                Flags.Add("01x02", "Sockets incompatible! Swap Motherboard or CPU!");
            }

            //Check Board and GPU Count
            compat = true;
            if(MB.SLI < GPUCount)
            {
                Flags.Add("01x03", "Motherboard does not support this many cards in SLI. Ignore if Crossfire.");
            }
            if (MB.XFIRE < GPUCount && Flags["01x03"] != null)
            {
                Flags.Add("01x03", "Motherboard does not support this many cards in SLI. Ignore if Crossfire.");
            }
            //Check GPU and GPU Count
            //Check Board and RAM
            //Check CPU and RAM
            //Check PSU and Total Wattage
            //Check GPU/MB and Monitor

            return Flags;
        }
        */

        public BuildDetails()
        {

            Name = "";
            OwnerID = "";
            Case = new CaseDetails();
            MB = new MotherBoardDetails();
            CPU = new CPUDetails();
            GPU = new GPUDetails();
            GPUCount = 0;
            PSU = new PSUDetails();
            RAM = new PartBag();
            Monitor = new PartBag();
            OD = new PartBag();
            HD = new PartBag();
            PCI = new PartBag();
            Peripherals = new PartBag();

        }

        public BuildDetails(Build bass)
        {

            Entities ORM = new Entities();

            Name = bass.BuildName;
            OwnerID = bass.OwnerID;
            Case = new CaseDetails(bass.Case);
            MB = new MotherBoardDetails(bass.Motherboard);
            CPU = new CPUDetails(bass.CPU);
            GPU = new GPUDetails(bass.GPU);
            GPUCount = (byte)bass.GPUCount;
            PSU = new PSUDetails(bass.PSU);
            RAM = new PartBag();
            foreach(RAM r in ORM.BuildsRAMs.Where(x => x.BuildID == bass.BuildID).Select(x => x.RAM).ToArray())
            {
                RAM.Add(new RAMDetails(r));
            }
            Monitor = new PartBag();
            foreach (Monitor r in ORM.BuildMonitors.Where(x => x.BuildID == bass.BuildID).Select(x => x.Monitor).ToArray())
            {
                Monitor.Add(new MonitorDetails(r));
            }
            OD = new PartBag();
            foreach (OpticalDriver r in ORM.BuildODs.Where(x => x.BuildID == bass.BuildID).Select(x => x.OpticalDriver).ToArray())
            {
                OD.Add(new ODDetails(r));
            }
            HD = new PartBag();
            foreach (HardDrive r in ORM.BuildDisks.Where(x => x.BuildID == bass.BuildID).Select(x => x.HardDrive).ToArray())
            {
                HD.Add(new HDDetails(r));
            }
            Peripherals = new PartBag();
            foreach (Peripheral r in ORM.BuildsPeripherals.Where(x => x.BuildID == bass.BuildID).Select(x => x.Peripheral).ToArray())
            {
                Peripherals.Add(new PeripheralDetails(r));
            }
            PCI = new PartBag();
            foreach (PCICard r in ORM.BuildPCIs.Where(x => x.BuildID == bass.BuildID).Select(x => x.PCICard).ToArray())
            {
                PCI.Add(new PCIDetails(r));
            }

        }

    }

    public class CaseDetails : PartDetails
    {

        public string CaseID { set; get; }
        public string PID;
        public int Size;
        public int Dimensions;
        public byte PortCount;
        public string Drives;
        public bool SSDSupport;
        public string Ports;
        public string Style;

        public CaseDetails() : base()
        {
            CaseID = "";
            PID = "";
            Size = 0;
            Dimensions = 0;
            PortCount = 0;
            Drives = "";
            SSDSupport = false;
            Ports = "";
            Style = "";
        }

        public CaseDetails(Case bass)
        {

        }

    }

    public class MotherBoardDetails : PartDetails
    {
        
        public string MBID { set; get; }
        public string PID { set; get; }
        public string Socket { set; get; }
        public string Chipset { set; get; }
        public byte RAMSlots { set; get; }
        public byte SLI { set; get; }
        public byte XFIRE { set; get; }
        public string FormFactor { set; get; }

        public MotherBoardDetails() : base()
        {

            MBID = "";
            PID = "";
            Socket = "";
            Chipset = "";
            RAMSlots = 0;
            SLI = 0;
            XFIRE = 0;
            FormFactor = "";

        }

        public MotherBoardDetails(Motherboard bass)
        {

        }

    }

    public class CPUDetails : PartDetails
    {

        public string CPUID { set; get; }
        public string PID { set; get; }
        public string Cache { set; get; }
        public string Socket { set; get; }
        public int Wattage { set { Wattage = Convert.ToInt32(value); } get { return Wattage; } }
        public bool Fan { set; get; }
        public byte Threads { set { Threads = Convert.ToByte(value); } get { return Threads; } }
        public string ProcessingUnits { set; get; }
        public double Speed { set { Speed = Convert.ToDouble(value); } get { return Speed; } }

        public CPUDetails() : base()
        {

            CPUID = "";
            PID = "";
            Cache = "";
            Wattage = 0;
            Fan = false;
            Threads = 0;
            ProcessingUnits = "";
            Speed = 0.0;

        }

        public CPUDetails(CPU bass)
        {

        }

    }

    public class PSUDetails
    {
        public string PSUID;
        public string PID;
        public string PowerSource;
        public string Dimensions;
        public int Wattage;

        public PSUDetails() : base()
        {

        }

        public PSUDetails(PSU bass)
        {

        }

    }

    public class GPUDetails : PartDetails
    {
        public string GPUID;

        public GPUDetails(GPU bass)
        {

        }

        public GPUDetails() : base()
        {
            GPUID = "";
        }
    }

    public class RAMDetails : PartDetails
    {
        public RAMDetails(RAM bass)
        {

        }
    }

    public class ODDetails : PartDetails
    {
        public ODDetails(OpticalDriver bass)
        {

        }
    }

    public class HDDetails : PartDetails
    {
        public string HDID;

        public HDDetails(HardDrive bass)
        {

        }
    }

    public class MonitorDetails : PartDetails
    {
        public MonitorDetails(Monitor bass)
        {

        }
    }

    public class PeripheralDetails : PartDetails
    {
        public PeripheralDetails(Peripheral bass)
        {

        }
    }

    public class PCIDetails : PartDetails
    {
        public PCIDetails(PCICard bass)
        {

        }
    }

    public partial class ZincParseController : ApiController
    {



    }
}