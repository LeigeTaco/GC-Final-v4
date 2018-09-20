using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace GC_Final.Models
{
    public partial class PCCase
    {
        public double GetPrice()
        {
            return Convert.ToDouble(Price) / 100;
        }

        public PCCase(string name)
        {
            CaseID = Guid.NewGuid().ToString("D");
            Style = " ";
            Name = name;
            Description = " ";
            Brand = " ";
            Manufacturer = " ";
            ImageLink = " ";
        }

        public PCCase(string productID, double height, double length, double width, byte x25Slots, byte x35Slots, byte expansionSlots, string style, string name, string description, string brand, int price, float stars, string imagelink, string manufacturer)
        {
            CaseID = Guid.NewGuid().ToString("D");
            if (Regex.IsMatch(productID, @"^[a-zA-Z0-9]{10}$"))
            {
                ProductID = productID.ToUpper();
            }
            if (height > 0)
            {
                Height = height;
            }
            if (length > 0)
            {
                Length = length;
            }
            if (width > 0)
            {
                Width = width;
            }
            TwoSlots = x25Slots;                //Slots for 2.5" drives
            ThreeSlots = x35Slots;              //Slots for 3.5" drives
            ExpansionSlots = expansionSlots;    //PCI Slots (GPUs too)
            Style = style;
            Name = name;
            Description = description;
            Brand = brand;
            if (price > 0)
            {
                Price = price;
            }
            if (stars > 0)
            {
                Stars = stars;
            }
            if (imagelink.ToLower().Contains("http"))
            {
                ImageLink = imagelink;
            }
            Manufacturer = manufacturer;
        }
    }

    public partial class CPU
    {
        public double GetPrice()
        {
            return Convert.ToDouble(Price) / 100;
        }

        public CPU(string name)
        {
            CPUID = Guid.NewGuid().ToString("D");
            ProductID = "xxxxxxxxxx";
            Cache = " ";
            Name = name;
            Description = " ";
            Brand = " ";
            Manufacturer = " ";
            ImageLink = " ";
        }
    }

    public partial class GPU
    {
        public double GetPrice()
        {
            return Convert.ToDouble(Price) / 100;
        }

        public GPU(string name)
        {
            GPUID = Guid.NewGuid().ToString("D");
            ProductID = "xxxxxxxxxx";
            RAMType = " ";
            Name = name;
            Description = " ";
            Brand = " ";
            Manufacturer = " ";
            ImageLink = " ";
        }
    }

    public partial class HardDrive
    {
        public double GetPrice()
        {
            return Convert.ToDouble(Price) / 100;
        }

        public HardDrive(string name)
        {
            HardDriveID = Guid.NewGuid().ToString("D");
            ProductID = "xxxxxxxxxx";
            Interface = " ";
            SlotSize = true;
            Capacity = 0;
            CapacityUnits = " ";
            Description = " ";
            Brand = " ";
            Price = 0;
            Stars = 0.0F;
            ImageLink = " ";
            Manufacturer = " ";
        }
    }

    public partial class Monitor
    {
        public double GetPrice()
        {
            return Convert.ToDouble(Price) / 100;
        }

        public Monitor(string name)
        {
            ProductID = "xxxxxxxxxx";
            MonitorID = Guid.NewGuid().ToString("D");
            Name = name;
            Description = " ";
            Brand = " ";
            Price = 0;
            Stars = 0.0F;
            Manufacturer = " ";
            ImageLink = " ";
        }
    }

    public partial class Motherboard
    {
        public double GetPrice()
        {
            return Convert.ToDouble(Price) / 100;
        }

        public Motherboard(string name)
        {
            MotherboardID = Guid.NewGuid().ToString("D");
            ProductID = "xxxxxxxxxx";
            Socket = " ";
            Chipset = " ";
            RAMType = " ";
            FormFactor = " ";
            Name = name;
            Description = " ";
            Brand = " ";
            Price = 0;
            Stars = 0.0F;
            Manufacturer = " ";
            ImageLink = " ";
        }
    }

    public partial class OpticalDriver
    {
        public double GetPrice()
        {
            return Convert.ToDouble(Price) / 100;
        }

        public OpticalDriver(string name)
        {
            OpticalDriverID = Guid.NewGuid().ToString("D");
            ProductID = "xxxxxxxxxx";
            Rewritable = false;
            Interface = " ";
            ReadSpeed = 0;
            WriteSpeed = 0;
            Wattage = 0;
            Name = name;
            Description = " ";
            Brand = " ";
            Price = 0;
            Stars = 0.0F;
            ImageLink = " ";
            Manufacturer = " ";
        }
    }

    public partial class PCICard
    {
        public double GetPrice()
        {
            return Convert.ToDouble(Price) / 100;
        }

        public PCICard(string name)
        {
            PCIcardID = Guid.NewGuid().ToString("D");
            Description = " ";
            Name = name;
            ProductID = "xxxxxxxxxx";
            Brand = " ";
            Price = 0;
            Stars = 0.0F;
            ImageLink = " ";
            Manufacturer = " ";
        }
    }

    public partial class Peripheral
    {
        public double GetPrice()
        {
            return Convert.ToDouble(Price) / 100;
        }

        public Peripheral(string name)
        {
            PeripheralsID = Guid.NewGuid().ToString("D");
            ProductID = "xxxxxxxxxx";
            Name = name;
            Description = " ";
            Brand = " ";
            Price = 0;
            Stars = 0.0F;
            ImageLink =  " ";
            Manufacturer = " ";
        }
    }

    public partial class PSU
    {
        public double GetPrice()
        {
            return Convert.ToDouble(Price) / 100;
        }

        public PSU(string name)
        {
            PSUID = Guid.NewGuid().ToString("D");
            Name = name;
        }
    }

    public partial class RAM
    {
        public double GetPrice()
        {
            return Convert.ToDouble(Price) / 100;
        }

        public RAM(string name)
        {
            RAMID = Guid.NewGuid().ToString("D");
            Name = name;
        }
    }

    public partial class Build
    {
        public void Glorp(Build _goop)
        {

            if (_goop.Motherboard != null && Motherboard == null)
            {
                MBID = _goop.Motherboard.MotherboardID;
                Motherboard = _goop.Motherboard;
            }
            if (_goop.CPU != null && CPU == null)
            {
                CPUID = _goop.CPU.CPUID;
                CPU = _goop.CPU;
            }
            if (_goop.GPU != null && GPU == null)
            {
                GPUID = _goop.GPU.GPUID;
                GPU = _goop.GPU;
                GPUCount = _goop.GPUCount;
            }
            if (_goop.PSU != null && PSU == null)
            {
                PSUID = _goop.PSU.PSUID;
                PSU = _goop.PSU;
            }
            if (_goop.PCCase != null && PCCase == null)
            {
                CaseID = _goop.PCCase.CaseID;
                PCCase = _goop.PCCase;
            }

        }

        public double GetPrice()
        {
            double _total = 0.0;

            _total += PCCase.GetPrice();
            _total += Motherboard.GetPrice();
            _total += CPU.GetPrice();
            _total += GPU.GetPrice() * (double)GPUCount;
            _total += PSU.GetPrice();
            foreach (RAM r in BuildsRAMs.Select(x => x.RAM).ToArray())
            {
                _total += r.GetPrice();
            }
            if (BuildDisks != null)
            {
                foreach (HardDrive d in BuildDisks.Select(x => x.HardDrive).ToArray())
                {
                    _total += d.GetPrice();
                }
            }
            if (BuildODs != null)
            {
                foreach (OpticalDriver o in BuildODs.Select(x => x.OpticalDriver).ToArray())
                {
                    _total += o.GetPrice();
                }
            }
            if (BuildMonitors != null)
            {
                foreach (Monitor m in BuildMonitors.Select(x => x.Monitor).ToArray())
                {
                    m.GetPrice();
                }
            }
            if (BuildPCIs != null)
            {
                foreach (PCICard c in BuildPCIs.Select(x => x.PCICard).ToArray())
                {
                    _total += c.GetPrice();
                }
            }
            if (BuildsPeripherals != null)
            {
                foreach (Peripheral p in BuildsPeripherals.Select(x => x.Peripheral).ToArray())
                {
                    _total += p.GetPrice();
                }
            }

            return _total;
        }

        public List<string[]> GetCompat(RAM[] _ram, PCICard[] _pci, HardDrive[] _hd, OpticalDriver[] _od)
        {
            //RAM[] _ram = BuildsRAMs.Select(x => x.RAM).ToArray();
            byte? _ramCount = 0;
            int? _ramCap = 0;
            int? TotalWattage = 0;
            byte? _3 = Convert.ToByte(_od.Length + _hd.Where(x => x.SlotSize == true).ToArray().Length);
            byte? _2 = (byte?)_hd.Where(x => !x.SlotSize == true).ToArray().Length;
            byte? _x = Convert.ToByte(GPUCount + _pci.Length);
            foreach (RAM r in _ram)
            {
                _ramCount += r.Quantity;
                _ramCap += r.TotalCapacity;
                //TotalWattage += r.Voltage * AMPERAGE;
            }
            //TotalWattage += GPU.Wattage * GPUCount;
            TotalWattage += Motherboard.Wattage + CPU.Wattage;
            /*
            foreach (PCICard p in BuildPCIs.Select(x => x.PCICard).ToArray())
            {
                TotalWattage += p.Wattage;
            }
            foreach (HardDrive p in BuildDisks.Select(x => x.HardDrive).ToArray())
            {
                TotalWattage += p.Wattage;
            }
            foreach (OpticalDriver p in BuildODs.Select(x => x.OpticalDriver).ToArray())
            {
                TotalWattage += p.Wattage;
            }
            */

            List<string[]> _out = new List<string[]>();
            //MB and CPU
            if (Motherboard.Socket != CPU.Socket)
            {
                _out.Add(new string[] { "Socket Mismatch", $"Your motherboard has a {Motherboard.Socket} socket and your CPU has a {CPU.Socket}"});
                //Add more specific error
            }
            //MB and RAM
            if (Motherboard.RAMSlots < _ramCount && (Motherboard.RAMSlots != null && _ramCount != null))
            {
                _out.Add(new string[] { "RAM Quantity Error (MB)", $"You have {_ramCount - Motherboard.RAMSlots} too many sticks of RAM (Non-Fatal)"});
            }
            for (int i = 0; i < _ram.Length; i++)
            {
                if (Motherboard.RAMType != _ram[i].RAMType)
                {
                    _out.Add(new string[] { $"RAM Type Error{i}", $"Your RAM selection in index {i + 1} does not match the motherboard's type."});
                }
            }
            //MB and GPU(s)
            if (Motherboard.PCISlots < GPUCount + BuildPCIs.Select(x => x.PCICard).ToArray().Length)
            {
                _out.Add(new string[] { "PCI Quantity Error", "Your motherboard cannot support this many GPUs and PCI Cards."});
            }
            if (GPU.MultiGPUType == true)   //True is SLI
            {
                if (GPUCount > Motherboard.SLILimit || GPUCount > GPU.MultiGPULimit)
                {
                    _out.Add(new string[] { "SLI Error", "You have too many GPUs for your build. You are too powerful."});
                }
            }
            if (GPU.MultiGPUType == false)
            {
                if (GPU.MultiGPULimit > Motherboard.CrossfireLimit)
                {
                    _out.Add(new string[] { "Crossfire Error", "You have too many GPUs for your build. You are too powerful."});
                }
            }
            //CPU and RAM
            if (CPU.MaxRAM < _ramCap)
            {
                _out.Add(new string[] { "RAM Capacity Error", "Your CPU cannot handle this amount of RAM (Fatal)."});
            }
            //Case and MB
            if (!PCCase.Style.Contains(Motherboard.FormFactor))
            {
                _out.Add(new string[] { "Form Factor Mismatch", $"Your case does not support {Motherboard.FormFactor} motherboards." });
            }
            //Case and PSU
            if (PSU.Width > PCCase.Width || PSU.Length > PCCase.Length || PSU.Height > PCCase.Height)
            {
                _out.Add(new string[] { "PSU Dim Error", "Your power supply is too large for your case."});
            }
            //Case and Drives
            if (PCCase.ThreeSlots < _3)
            {
                _out.Add(new string[] { "3.5\" Drive Error", "Your case cannot hold this many 3.5\" drives (Non-Fatal)"});
            }
            if (PCCase.TwoSlots < _2)
            {
                _out.Add(new string[] { "2.5\" Drive Error", "Your case cannot hold this many 2.5\" drives (Non-Fatal)"});
            }
            //MB and Drives
            if (Motherboard.SATASlots < BuildDisks.Select(x => x.HardDrive).ToArray().Length + BuildODs.Select(x => x.OpticalDriver).ToArray().Length)
            {
                _out.Add(new string[] { "Too Many SATAs", "Your build has too many SATA drives for your motherboard (Non-Fatal)."});
            }
            //GPU and Monitors

            //Final Boss: PSU and Total
            if (TotalWattage > PSU.Wattage)
            {
                _out.Add(new string[] {"Brown", "Your PC does not have enough power, use a better PSU (Very Fatal)."});
            }

            return _out;
        }

        public List<string[]> GetCompat()
        {
            RAM[] _ram = BuildsRAMs.Select(x => x.RAM).ToArray();
            byte? _ramCount = 0;
            int? _ramCap = 0;
            int? TotalWattage = 0;
            //byte _3 = Convert.ToByte(BuildODs.Select(x => x.OpticalDriver).ToArray().Length + BuildDisks.Select(x => x.HardDrive).Where(x => x.SlotSize == true).ToArray().Length);
            //byte _2 = (byte)BuildDisks.Select(x => x.HardDrive).Where(x => !x.SlotSize == true).ToArray().Length;
            //byte _x = Convert.ToByte(GPUCount + BuildPCIs.Select(x => x.PCICard).ToArray().Length);
            foreach (RAM r in _ram)
            {
                _ramCount += r.Quantity;
                _ramCap += r.TotalCapacity;
                //TotalWattage += r.Voltage * AMPERAGE;
            }
            //TotalWattage += GPU.Wattage * GPUCount;
            //TotalWattage += Motherboard.Wattage + CPU.Wattage;
            /*
            foreach (PCICard p in BuildPCIs.Select(x => x.PCICard).ToArray())
            {
                TotalWattage += p.Wattage;
            }
            foreach (HardDrive p in BuildDisks.Select(x => x.HardDrive).ToArray())
            {
                TotalWattage += p.Wattage;
            }
            foreach (OpticalDriver p in BuildODs.Select(x => x.OpticalDriver).ToArray())
            {
                TotalWattage += p.Wattage;
            }
            */

            List<string[]> _out = new List<string[]>();

            if (Motherboard.Socket != CPU.Socket)
            {
                _out.Add(new string[] { "Socket Mismatch", $"Your motherboard has a {Motherboard.Socket} socket and your CPU has a {CPU.Socket}" });
                //Add more specific error
            }
            //MB and RAM
            if (Motherboard.RAMSlots < _ramCount && (Motherboard.RAMSlots != null && _ramCount != null))
            {
                _out.Add(new string[] { "RAM Quantity Error (MB)", $"You have {_ramCount - Motherboard.RAMSlots} too many sticks of RAM (Non-Fatal)" });
            }
            for (int i = 0; i < _ram.Length; i++)
            {
                if (Motherboard.RAMType != _ram[i].RAMType)
                {
                    _out.Add(new string[] { $"RAM Type Error {i + 1}", $"Your RAM selection in index {i + 1} does not match the motherboard's type." });
                }
            }
            //MB and GPU(s)
            if (Motherboard.PCISlots < GPUCount + BuildPCIs.Select(x => x.PCICard).ToArray().Length)
            {
                _out.Add(new string[] { "PCI Quantity Error", "Your motherboard cannot support this many GPUs and PCI Cards." });
            }
            if (GPU.MultiGPUType == true)   //True is SLI
            {
                if (GPUCount > Motherboard.SLILimit || GPUCount > GPU.MultiGPULimit)
                {
                    _out.Add(new string[] { "SLI Error", "You have too many GPUs for your build. You are too powerful." });
                }
            }
            if (GPU.MultiGPUType == false)
            {
                if (GPU.MultiGPULimit > Motherboard.CrossfireLimit)
                {
                    _out.Add(new string[] { "Crossfire Error", "You have too many GPUs for your build. You are too powerful." });
                }
            }
            //CPU and RAM
            if (CPU.MaxRAM < _ramCap)
            {
                _out.Add(new string[] { "RAM Capacity Error", "Your CPU cannot handle this amount of RAM (Fatal)." });
            }
            //Case and MB
            //if (!PCCase.Style.Contains(Motherboard.FormFactor))
            //{
            //    _out.Add(new string[] { "Form Factor Mismatch", $"Your case does not support {Motherboard.FormFactor} motherboards." });
            //}
            //Case and PSU
            if (PSU.Width > PCCase.Width || PSU.Length > PCCase.Length || PSU.Height > PCCase.Height)
            {
                _out.Add(new string[] { "PSU Dim Error", "Your power supply is too large for your case." });
            }
            //Case and Drives
            //if (PCCase.ThreeSlots < _3)
            //{
            //    _out.Add(new string[] { "3.5\" Drive Error", "Your case cannot hold this many 3.5\" drives (Non-Fatal)" });
            //}
            //if (PCCase.TwoSlots < _2)
            //{
            //    _out.Add(new string[] { "2.5\" Drive Error", "Your case cannot hold this many 2.5\" drives (Non-Fatal)" });
            //}
            //MB and Drives
            if (Motherboard.SATASlots < BuildDisks.Select(x => x.HardDrive).ToArray().Length + BuildODs.Select(x => x.OpticalDriver).ToArray().Length)
            {
                _out.Add(new string[] { "Too Many SATAs", "Your build has too many SATA drives for your motherboard (Non-Fatal)." });
            }
            //GPU and Monitors

            //Final Boss: PSU and Total
            if (TotalWattage > PSU.Wattage)
            {
                _out.Add(new string[] { "Brown", "Your PC does not have enough power, use a better PSU (Very Fatal)." });
            }
            _out.Add(new string[] { "End", "End" });

            return _out;
        }

        public Build(Controllers.BuildDetails bass)
        {

            BuildName = bass.Name;
            BuildID = bass.BuildID;
            OwnerID = bass.OwnerID;
            CaseID = bass.Case.CaseID;
            MBID = bass.MB.MBID;
            CPUID = bass.CPU.CPUID;
            PSUID = bass.PSU.PSUID;
            GPUID = bass.GPU.GPUID;
            PSUID = bass.PSU.PSUID;
            GPUCount = bass.GPUCount;

        }

        public Build(string name)
        {
            BuildName = name;
            PCCase = new PCCase();
            Motherboard = new Motherboard();
            CPU = new CPU();
            PSU = new PSU();
            GPU = new GPU();
            GPUCount = 0;
        }
    }
}