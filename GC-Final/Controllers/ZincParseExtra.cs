using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GC_Final.Models;
using System.Text.RegularExpressions;

namespace GC_Final.Controllers
{
    #region PartDetails
    /*
    public static class PartDetails
    {

        public static string Name { set; get; }
        public static string ProductID { set; get; }
        public static string Desc { set; get; }
        public static string Brand { set; get; }
        public static double Price { set { Price = value / 100; } get { return Price; } }
        public static string Stars { set; get; }
        public static string Manufacturer { set; get; }

        public static PartDetails()
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

    public static class PartBag
    {
        private static List<PartDetails> _Bag;

        public static int Count { get { return _Bag.Count; } }
        public static List<PartDetails> AsList { get { return _Bag; } }
        public static Dictionary<string, int> Quantities {
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

        public static void Add(PartDetails part)
        {
            _Bag.Add(part);
        }
        //Add method to add by ID
        public static void Remove(PartDetails part)
        {
            _Bag.Remove(part);
        }
        public static List<PartDetails> ToList()
        {
            return _Bag;
        }
    }

    public static class BuildDetails
    {
        
        public static string Name { set; get; }
        public static string BuildID { set; get; }
        public static string OwnerID { set; get; }
        public static CaseDetails Case { set; get; }
        public static MotherBoardDetails MB { set; get; }
        public static CPUDetails CPU { set; get; }
        public static GPUDetails GPU { set; get; }
        public static byte GPUCount { set; get; }
        public static PSUDetails PSU { set; get; }
        public static PartBag RAM { set; get; }
        public static PartBag Monitor { set; get; }
        public static PartBag OD { set; get; }
        public static PartBag HD { set; get; }
        public static PartBag Peripherals { set; get; }
        public static PartBag PCI { set; get; }
        /*
        public static Dictionary<string, string> CheckCompatability()
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
        /*
        public static BuildDetails()
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

        public static BuildDetails(Build bass)
        {

            Entities ORM = new Entities();

            Name = bass.BuildName;
            OwnerID = bass.OwnerID;
            Case = new CaseDetails(bass.PCCase);
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

    public static class CaseDetails : PartDetails
    {

        public static string CaseID { set; get; }
        public static string PID;
        public static int Size;
        public static int Dimensions;
        public static byte PortCount;
        public static string Drives;
        public static bool SSDSupport;
        public static string Ports;
        public static string Style;

        public static CaseDetails() : base()
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

        public static CaseDetails(PCCase bass)
        {

        }

    }

    public static class MotherBoardDetails : PartDetails
    {
        
        public static string MBID { set; get; }
        public static string PID { set; get; }
        public static string Socket { set; get; }
        public static string Chipset { set; get; }
        public static byte RAMSlots { set; get; }
        public static byte SLI { set; get; }
        public static byte XFIRE { set; get; }
        public static string FormFactor { set; get; }

        public static MotherBoardDetails() : base()
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

        public static MotherBoardDetails(Motherboard bass)
        {

        }

    }

    public static class CPUDetails : PartDetails
    {

        public static string CPUID { set; get; }
        public static string PID { set; get; }
        public static string Cache { set; get; }
        public static string Socket { set; get; }
        public static int Wattage { set { Wattage = Convert.ToInt32(value); } get { return Wattage; } }
        public static bool Fan { set; get; }
        public static byte Threads { set { Threads = Convert.ToByte(value); } get { return Threads; } }
        public static string ProcessingUnits { set; get; }
        public static double Speed { set { Speed = Convert.ToDouble(value); } get { return Speed; } }

        public static CPUDetails() : base()
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

        public static CPUDetails(CPU bass)
        {

        }

    }

    public static class PSUDetails
    {
        public static string PSUID;
        public static string PID;
        public static string PowerSource;
        public static string Dimensions;
        public static int Wattage;

        public static PSUDetails() : base()
        {

        }

        public static PSUDetails(PSU bass)
        {

        }

    }

    public static class GPUDetails : PartDetails
    {
        public static string GPUID;

        public static GPUDetails(GPU bass)
        {

        }

        public static GPUDetails() : base()
        {
            GPUID = "";
        }
    }

    public static class RAMDetails : PartDetails
    {
        public static RAMDetails(RAM bass)
        {

        }
    }

    public static class ODDetails : PartDetails
    {
        public static ODDetails(OpticalDriver bass)
        {

        }
    }

    public static class HDDetails : PartDetails
    {
        public static string HDID;

        public static HDDetails(HardDrive bass)
        {

        }
    }

    public static class MonitorDetails : PartDetails
    {
        public static MonitorDetails(Monitor bass)
        {
            
        }
    }

    public static class PeripheralDetails : PartDetails
    {
        public static PeripheralDetails(Peripheral bass)
        {

        }
    }

    public static class PCIDetails : PartDetails
    {
        public static PCIDetails(PCICard bass)
        {
            
        }
    }*/
    #endregion
    
    public partial class ZincParseController : ApiController
    {
        private static  string[] FormFactors = new string[]
        {
            "at",
            "baby at",
            "atx",
            "mini atx",
            "micro atx",
            "flex atx",
            "lpx",
            "nlx",
            "form factor"
        };

        private static  int SafeNextIndex(int index, int collectionLength)
        {
            if (index < collectionLength)
            {
                return index;
            }
            return collectionLength - 1;
        }

        public static int[] GetMaxScreenResolution(string Data)
        {
            int MaxResX;
            int MaxResY;
            string ScreenResolution = Regex.Match(Data, @"\d+( )?x( )?\d+").Value;
            string[] SplitScreenRes = ScreenResolution.Split('x');
            SplitScreenRes[0] = Regex.Match(SplitScreenRes[0], @"\d+").Value;
            SplitScreenRes[1] = Regex.Match(SplitScreenRes[1], @"\d+").Value;
            MaxResX = int.Parse(SplitScreenRes[0]);
            MaxResY = int.Parse(SplitScreenRes[1]);
            int[] MaxScreenResolution = { MaxResX, MaxResY };
            return MaxScreenResolution;
        }


        public static int[] GetMaxScreenResolution(string[] Data)
        {
            try
            {
                foreach (string _data in Data)
                {
                    if (Regex.IsMatch(_data, @"\d+( )?[Xx]( )?\d+"))
                    {
                        return GetMaxScreenResolution(_data);
                    }
                }

            }
            catch (Exception)
            {
                return new int[] { 0, 0 };
            }

            return new int[] { 0, 0 };

        }

        public static string GetSocketType(string Data)
        {
            string _out = "";
            return _out + Regex.Match(Data, @"[PpLl][Gg][Aa] \d+").Value;
        }

        public static string GetSocketType(string[] Data)
        {
            try
            {
                foreach (string d in Data)
                {
                    if (Regex.IsMatch(d, @"[PpLl][Gg][Aa] \d+"))
                    {
                        return GetSocketType(d);
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return null;
        }

        public static string GetFormFactor(string Data)
        {
            string _FFDetails = "";
            string[] _data = Data.Split(' ');
            int j = 0;
            for (int i = 0; i < _data.Length; i++)
            {
                if (_data[i].ToLower().Contains("at") && !_data[j].ToLower().Contains("baby"))
                {
                    _FFDetails += "AT,";
                }
                else if (_data[i].ToLower().Contains("baby") && _data[SafeNextIndex(i + 1, _data.Length)].ToLower().Contains("at"))
                {
                    _FFDetails += "Baby AT,";
                }
                else if (Regex.IsMatch(_data[i], @"[Aa][Tt][Xx]") && !(_data[j].ToLower().Contains("mini") || _data[j].ToLower().Contains("micro") || _data[j].ToLower().Contains("flex")))
                {
                    _FFDetails += "ATX,";
                }
                else if (_data[i].ToLower().Contains("mini") && _data[SafeNextIndex(i + 1, _data.Length)].ToLower().Contains("atx"))
                {
                    _FFDetails += "Mini ATX,";
                }
                else if (_data[i].ToLower().Contains("micro") && _data[SafeNextIndex(i + 1, _data.Length)].ToLower().Contains("atx"))
                {
                    _FFDetails += "Micro ATX,";
                }
                else if (_data[i].ToLower().Contains("flex") && _data[SafeNextIndex(i + 1, _data.Length)].ToLower().Contains("atx"))
                {
                    _FFDetails += "Flex ATX,";
                }
                else if (_data[i].ToLower().Contains("lpx"))
                {
                    _FFDetails += "LPX,";
                }
                else if (_data[i].ToLower().Contains("nlx"))
                {
                    _FFDetails += "NLX,";
                }
            }
            if (_FFDetails != "")
            {
                _FFDetails = _FFDetails.Substring(0, _FFDetails.Length - 1);
            }
            return _FFDetails;
        }

        public static string GetFormFactor(string[] Data)
        {
            foreach (string s in Data)
            {
                foreach (string ff in FormFactors)
                {
                    if (s.ToLower().Contains(ff))
                    {
                        return GetFormFactor(s);
                    }
                }
            }

            return null;
        }

        public static string GetChipset(string[] Data)
        {
            foreach (string s in Data)
            {
                if (s.ToLower().Contains("chipset"))
                {
                    return GetChipset(s);
                }
            }
            return null;
        }
        public static string GetChipset(string Data)
        {
            if (Data.ToLower().Contains("chipset"))
            {
                return Regex.Match(Data.ToUpper(), @"[A-Z]{0,2}\d+-?[A-Z]{0,2}").Value;
            }
            return null;
        }

        public static string GetRAMType(string[] Data)
        {
            foreach (string s in Data)
            {
                if (Regex.IsMatch(s.ToLower(), @"ddr\d"))
                {
                    return GetRAMType(s);
                }
            }
            return null;
        }
        public static string GetRAMType(string Data)
        {
            if (Regex.IsMatch(Data.ToLower(), @"ddr\d"))
            {
                return $"DDR{Regex.Match(Regex.Match(Data.ToLower(), @"ddr\d").Value, @"\d").Value}";
            }
            return null;
        }

        public static byte GetRAMSlots(string[] Data)
        {
            int Index = -1;
            for (int i = 0; i < Data.Length; i++)
            {
                if (Data[i].ToLower().Contains("memory slots"))
                {
                    Index = i;
                }
            }

            if (Index < 0)
                return 0;

            byte MemorySlots = byte.Parse(Regex.Match(Regex.Match(Data[Index], @"\d x").Value, @"\d").Value);
            return MemorySlots;

        }

        public static int GetPowerSupply(string Data)
        {
            int PowerSupply = int.Parse(Regex.Match(Regex.Match(Data, @"\d+W").Value, @"\d+").Value);
            return PowerSupply;
        }

        public static float GetCPU_Speed(string[] Data)
        {
            foreach (string s in Data)
            {
                if (Regex.IsMatch(s.ToLower(), @"\d+\.\d+( )?ghz"))
                {
                    return GetCPU_Speed(s);
                }
            }
            return 0;
        }
        public static float GetCPU_Speed(string Data)
        {
            if (Regex.IsMatch(Data.ToLower(), @"\d+\.\d+( )?ghz"))
            {
                return float.Parse(Regex.Match(Regex.Match(Data.ToLower(), @"\d+\.\d+( )?ghz").Value, @"\d+\.\d+").Value);
            }
            return 0;
        }

        public static byte GetSATA_Slots(string[] Data)
        {
            foreach (string s in Data)
            {
                if (Regex.IsMatch(s.ToLower(), @"sata"))
                {
                    return GetSATA_Slots(s);
                }
            }
            return 6;
        }

        public static byte GetSATA_Slots(string Data)
        {
            //This will do something eventually.

            return 6;
        }

        public static byte GetPCI_Slots(string[] Data)
        {
            for (int i = 0; i < Data.Length; i++)
            {
                if (Data[i].ToLower().Contains("pci"))
                {
                    byte PCI_Slots = 0;
                    string temp = Regex.Match(Data[i], @"\d+( )?[Xx]( )?[Pp][Cc][Ii](-| )?[Ee]([Xx][Pp][Rr][Ee][Ss][Ss])?( )?(\d(.0)?)?( )?[Xx]( )?(\d+)?").Value;
                    temp = Regex.Match(temp, @"\d+( )?[Xx]").Value;
                    temp = Regex.Match(temp, @"\d+").Value;
                    PCI_Slots = byte.Parse(temp);

                    return PCI_Slots;
                }
            }

            return 0;
        }

        public static byte GetMultiGPULimit(string[] Data)
        {
            foreach (string s in Data)
            {
                if (s.ToLower().Contains("sli"))
                {
                    return GetMultiGPULimit(s);
                }
                else if (Regex.IsMatch(s.ToLower(), @"^(cross|x)(-| )?fire$"))
                {
                    return GetMultiGPULimit(s);
                }
            }
            return 0;
        }

        public static byte GetMultiGPULimit(string Data)
        {
            if (Data.ToLower().Contains("sli"))
            {
                return GetMultiGPULimit(Data, false);
            }
            else if (Regex.IsMatch(Data.ToLower(), @"(cross|x)(-| )?fire"))
            {
                return GetMultiGPULimit(Data, true);
            }
            return 0;
        }

        private static byte GetMultiGPULimit(string Data, bool SOX)
        {
            if (SOX)    //Crossfire
            {
                if (Regex.IsMatch(Data.ToLower(), @"two(-| )?way"))
                {
                    return 2;
                }
                else if (Regex.IsMatch(Data.ToLower(), @"three(-| )?way"))
                {
                    return 3;
                }
                else if (Regex.IsMatch(Data.ToLower(), @"four(-| )?way"))
                {
                    return 4;
                }
                else if (Regex.IsMatch(Data.ToLower(), @"\d(-| )?way"))
                {
                    return byte.Parse(Regex.Match(Data.ToLower(), @"\d(-| )?way").Value);
                }
            }
            else        //SLI
            {
                if (Regex.IsMatch(Data.ToLower(), @"two(-| )?way"))
                {
                    return 2;
                }
                else if (Regex.IsMatch(Data.ToLower(), @"three(-| )?way"))
                {
                    return 3;
                }
                else if (Regex.IsMatch(Data.ToLower(), @"four(-| )?way"))
                {
                    return 4;
                }
                else if (Regex.IsMatch(Data.ToLower(), @"\d(-| )?way"))
                {
                    return byte.Parse(Regex.Match(Data.ToLower(), @"\d(-| )?way").Value);
                }
            }
            return 2;
        }

        public static string GetHardDrive_Type(string[] Data)
        {
            foreach (string s in Data)
            {
                if (Regex.IsMatch(s.ToLower(), @"(ssd|sshd|hdd)"))
                {
                    return GetHardDrive_Type(s);
                }
            }
            return null;
        }

        public static string GetHardDrive_Type(string Data)
        {
            if (Regex.IsMatch(Data.ToLower(), @"(ssd|sshd|hdd)"))
            {
                return Regex.Match(Data.ToLower(), @"(ssd|sshd|hdd)").Value;
            }
            return null;
        }

        public static bool GetHardDrive_Size(string[] Data)
        {
            foreach (string s in Data)
            {
                if (Regex.IsMatch(s.ToLower(), "(2" + @"\." + "5(-| )?(\"|inch)|3" + @"\." + "5(-| )?(\")|(three(-| )?(point(five|5)|and(-| )a(-| )half) inch)|(two(-| )?(point(five|5)|and(-| )a(-| )half) inch)"))
                {
                    return GetHardDrive_Size(s);
                }
            }
            return false;
        }

        public static bool GetHardDrive_Size(string Data)
        {
            if (Regex.IsMatch(Data.ToLower(), "(2" + @"\." + "5(-| )?(\"|inch)|(two(-| )?(point(five|5)|and(-| )a(-| )half) inch)"))
            {
                return false;
            }
            else if (Regex.IsMatch(Data.ToLower(), "3" + @"\." + "5(-| )?(\")|(three(-| )?(point(five|5)|and(-| )a(-| )half) inch)"))
            {
                return true;
            }
            return false;
        }

        public static int GetOpticalDrive_ReadSpead(string[] Data)     //omegaLuL speAd
        {
            foreach (string s in Data)
            {
                if (Regex.IsMatch(s.ToLower(), @"(read(-| )speed)(:| )((bd(-| )r( )?\d+x)|(dvd(-| )r( )?\d+x)|(cd(-| )r( )?\d+x))+"))
                {
                    return GetOpticalDrive_ReadSpead(s);
                }
            }
            return 0;
        }
        public static int GetOpticalDrive_ReadSpead(string Data)      //Convert to MB/s or GB/s
        {
            if (Regex.IsMatch(Data.ToLower(), @"(read(-| )speed)(:| )((bd(-| )r( )?\d+x)|(dvd(-| )r( )?\d+x)|(cd(-| )r( )?\d+x))+"))
            {
                int _ = 0;
                foreach (Match m in Regex.Matches(Data.ToLower(), @"\d+x"))
                {
                    int __ = int.Parse(Regex.Match(m.Value, @"\d").Value);
                    _ = _ < __ ? __ : _;
                    return _;
                }
            }
            return 0;
        }

        public static int GetOpticalDrive_WriteSpeed(string[] Data)
        {
            foreach (string s in Data)
            {
                if (Regex.IsMatch(s.ToLower(), @"(write(-| )speed)(:| )((bd(-| )r( )?\d+x)|(dvd(-| )r( )?\d+x)|(cd(-| )r( )?\d+x))+"))
                {
                    return GetOpticalDrive_WriteSpeed(s);
                }
            }
            return 0;
        }
        public static int GetOpticalDrive_WriteSpeed(string Data)     //Also convert this
        {
            if (Regex.IsMatch(Data.ToLower(), @"(write(-| )speed)(:| )((bd(-| )r( )?\d+x)|(dvd(-| )r( )?\d+x)|(cd(-| )r( )?\d+x))+"))
            {
                int _ = 0;
                foreach (Match m in Regex.Matches(Data.ToLower(), @"\d+x"))
                {
                    int __ = int.Parse(Regex.Match(m.Value, @"\d").Value);
                    _ = _ < __ ? __ : _;
                    return _;
                }
            }
            return 0;
        }

        public static string GetOpticalDrive_Types(string[] Data)
        {
            for (int i = 0; i < Data.Length; i++)
            {
                if (Data[i].ToLower().Contains("cd"))
                {
                    return "CD";
                }
                else if (Data[i].ToLower().Contains("dvd"))
                {
                    return "DVD";
                }
                else if (Data[i].ToLower().Contains("blu-ray"))
                {
                    return "Blu-Ray";
                }
                else if (Data[i].ToLower().Contains("floppy"))
                {
                    return "Floppy";
                }
            }

            return null;
        }

        public static int GetHardDrive_ReadSpeed(string[] Data)
        {
            for (int i = 0; i < Data.Length; i++)
            {
                if (Data[i].ToLower().Contains("read"))
                {
                    int Speeds = int.Parse(Regex.Match(Data[i], @"\d+( )?[Mm][Bb]/[Ss]").Value);
                    return Speeds;
                }
                else if (Data[i].ToLower().Contains("gb/s"))
                {
                    int Speeds = int.Parse(Regex.Match(Data[i], @"\d+( )?[Gg][Bb]/[Ss]").Value);
                    Speeds *= 1024;
                    return Speeds;
                }
            }

            return 0;
        }

        public static int GetHardDrive_WriteSpeed(string[] Data)
        {
            for (int i = 0; i < Data.Length; i++)
            {
                if (Data[i].ToLower().Contains("write"))
                {
                    int Speeds = int.Parse(Regex.Match(Data[i], @"\d+( )?[Mm][Bb]/[Ss]").Value);
                    return Speeds;
                }
                else if (Data[i].ToLower().Contains("gb/s"))
                {
                    int Speeds = int.Parse(Regex.Match(Data[i], @"\d+( )?[Gg][Bb]/[Ss]").Value);
                    Speeds *= 1024;
                    return Speeds;
                }
            }

            return 0;
        }

    }
}