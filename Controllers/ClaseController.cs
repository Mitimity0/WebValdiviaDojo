﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebValdiviaDojo.Controllers
{
    public class ClaseController : Controller
    {
        // GET: Clase
        public ActionResult Clase()
        {
            return View();
        }

        public ActionResult ClaseInscrita()
        {
            return View();
        }

    }
}