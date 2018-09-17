using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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