using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GC_Final.Models;

namespace GC_Final.Controllers
{
    public class UsersController : Controller
    {
        private int _displayLimit(int count)
        {
            if (count < 15)
            {
                return count % 15;
            }
            else
            {
                return 15;
            }
        }

        // GET: Users
        public ActionResult Display(string id)
        {
            Entities ORM = new Entities();
            if (ORM.AspNetUsers.Find(id) != null)
            {
                ViewBag.Username = ORM.AspNetUsers.Find(id).Email;
                Random R = new Random();
                Build[] arr = ORM.Builds.ToArray();
                List<Build> _arr = new List<Build>();
                for (int i = 0; i < _displayLimit(arr.Length); i++)
                {
                    _arr.Add(arr[R.Next(arr.Length)]);
                }
                ViewBag.UserBuilds = _arr.ToArray();
                return View();
            }
            return View("Error");
        }
    }
}