using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebValdiviaDojo.Controllers
{
    public class CompraController : Controller
    {
        // GET: Compra
        public ActionResult Pago()
        {
            return View();
        }

        public ActionResult ConfirmacionCompra()
        {
            return View();
        }
    }
}