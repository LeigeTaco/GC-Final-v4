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
        // GET: Users
        public ActionResult Display(string id)
        {
            Entities ORM = new Entities();
            //ViewBag.Username = 

            return View();
        }


    }
}