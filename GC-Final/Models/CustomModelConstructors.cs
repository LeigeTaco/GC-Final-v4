using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GC_Final.Models
{
    public partial class Case
    {
        public Case(string titleFromAPI)
        {
            CaseID = Guid.NewGuid().ToString("D");
            title = titleFromAPI;
        }
    }

    public partial class CPU
    {
        public CPU(string titleFromAPI)
        {
            CPUID = Guid.NewGuid().ToString("D");
            title = titleFromAPI;
        }
    }

    public partial class GPU
    {
        public GPU(string titleFromAPI)
        {
            GPUID = Guid.NewGuid().ToString("D");
            title = titleFromAPI;
        }
    }

    public partial class HardDrive
    {
        public HardDrive(string name)
        {
            HardDriveID = Guid.NewGuid().ToString("D");
            product_id = "xxxxxxxxxx";
            @interface = " ";
            size = true;
            capacity = " ";
            speed = " ";
            product_description = " ";
            brand = " ";
            price = 0;
            stars = " ";
            main_image = new byte[] { 0, 0, 0, 0 };
            manufacturer = " ";
        }
    }

    public partial class Monitor
    {
        public Monitor(string name)
        {
            MonitorID = Guid.NewGuid().ToString("D");
            //title = name;
        }
    }

    public partial class Motherboard
    {
        public Motherboard(string name)
        {
            MotherboardID = Guid.NewGuid().ToString("D");
            title = name;
        }
    }

    public partial class OpticalDriver
    {
        public OpticalDriver(string name)
        {
            OpticalDriverID = Guid.NewGuid().ToString("D");
            product_id = "xxxxxxxxxx";
            rewriteable = true;
            drive_type = " ";
            read_speed = 0;
            write_speed = 0;
            wattage = 0;
            type = " ";
            title = name;
            product_description = " ";
            brand = " ";
            price = 0;
            stars = " ";
            main_image = new byte[] { 0, 0, 0, 0 };
            manufacturer = " ";
        }
    }

    public partial class PCICard
    {
        public PCICard(string name)
        {
            PCIcardID = Guid.NewGuid().ToString("D");
            product_description = " ";
            title = name;
            product_id = "xxxxxxxxxx";
            brand = " ";
            price = 0;
            stars = " ";
            main_image = new byte[] { 0, 0, 0, 0 };
            manufacturer = " ";
        }
    }

    public partial class Peripheral
    {
        public Peripheral(string name)
        {
            PeripheralsID = Guid.NewGuid().ToString("D");
            product_id = "xxxxxxxxxx";
            title = name;
            product_description = " ";
            brand = " ";
            price = 0;
            stars = " ";
            main_image = new byte[] { 0, 0, 0, 0 };
            manufacturer = " ";
        }
    }

    public partial class PSU
    {
        public PSU(string name)
        {
            PSUID = Guid.NewGuid().ToString("D");
            title = name;
        }
    }

    public partial class RAM
    {
        public RAM(string name)
        {
            RAMID = Guid.NewGuid().ToString("D");
            title = name;
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
            Case = new Case();
            Motherboard = new Motherboard();
            CPU = new CPU();
            PSU = new PSU();
            GPU = new GPU();
            GPUCount = 0;
        }
    }
}