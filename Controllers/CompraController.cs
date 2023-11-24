using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebValdiviaDojo.WS_ValdiviaDojo;

namespace WebValdiviaDojo.Controllers
{
    public class CompraController : Controller
    {
        //
        public ActionResult OrdenCompra(int v_rut)
        {
            if (v_rut==null)
            {
                View("~/Views/Home/Index.cshtml");
            }
            List<carrito> car = ListarCarrito(v_rut);
            ViewBag.Carrito = car;
            return View();
        }
        [HttpPost]
        public ActionResult OrdenCompra(int p_total,int v_rut)
        {
            List<carrito> car = ListarCarrito(v_rut);
            ViewBag.Carrito = car;
            return View();
        }
        //
        public ActionResult Pago()
        {
            return View();
        }
        //
        public ActionResult ConfirmacionCompra()
        {
            return View();
        }
        //Listar productos 
        public List<carrito> ListarCarrito(int p_rut)
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                return cliente.ListadoCarrito(p_rut).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al llamar al servicio web: " + ex.Message);
                return null;
            }
            finally
            {
                cliente.Close();
            }
        }
        //
    }
}