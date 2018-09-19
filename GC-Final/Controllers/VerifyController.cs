using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GC_Final.Models;

namespace GC_Final.Controllers
{
    public class VerifyController : ApiController
    {
        [HttpPost]
        public string GetID(string partType, string partName)
        {
            Entities ORM = new Entities();
            if (partType == "Motherboard")
            {
                return ORM.Motherboards.Where(x => x.Name == partName).Select(x => x.ProductID).First();
            }
            else if (partType == "CPU")
            {
                return ORM.CPUs.Where(x => x.Name == partName).Select(x => x.ProductID).First();
            }
            else if (partType == "GPU")
            {
                return ORM.GPUs.Where(x => x.Name == partName).Select(x => x.ProductID).First();
            }
            else if (partType == "PSU")
            {
                return ORM.PSUs.Where(x => x.Name == partName).Select(x => x.ProductID).First();
            }
            else if (partType == "PCCase")
            {
                return ORM.PCCases.Where(x => x.Name == partName).Select(x => x.ProductID).First();
            }
            else if (partType == "RAM")
            {
                return ORM.RAMs.Where(x => x.Name == partName).Select(x => x.ProductID).First();
            }
            else if (partType == "Monitor")
            {
                return ORM.Monitors.Where(x => x.Name == partName).Select(x => x.ProductID).First();
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public Dictionary<string, string> CheckCompataibility(string motherboard_id, string cpu_id, string psu_id, string case_id, string gpu_id, byte gpu_count, string[] ram_ids, string[] harddrive_ids, string[] optical_ids, string[] pci_ids, string[] peripheral_ids, string[] monitor_ids)
        {
            Entities ORM = new Entities();

            Build temp = new Build();
            temp.Motherboard = ORM.Motherboards.Find(motherboard_id);
            temp.CPU = ORM.CPUs.Find(cpu_id);
            temp.PSU = ORM.PSUs.Find(psu_id);
            temp.PCCase = ORM.PCCases.Find(case_id);
            temp.GPU = ORM.GPUs.Find(gpu_id);
            temp.GPUCount = gpu_count;
            temp.BuildsRAMs = new List<BuildsRAM>();

            List<RAM> _tempR = new List<RAM>();
            foreach (string s in ram_ids)
            {
                _tempR.Add(ORM.RAMs.Find(s));
            }
            RAM[] tempR = _tempR.ToArray();

            List<PCICard> _tempP = new List<PCICard>();
            foreach (string s in pci_ids)
            {
                _tempP.Add(ORM.PCICards.Find(s));
            }
            PCICard[] tempP = _tempP.ToArray();

            List<Peripheral> _tempPE = new List<Peripheral>();
            foreach (string s in peripheral_ids)
            {
                _tempPE.Add(ORM.Peripherals.Find(s));
            }
            Peripheral[] tempPE = _tempPE.ToArray();



            return temp.GetCompat();
        }

    }
}
