using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using WebValdiviaDojo.WS_ValdiviaDojo;


namespace WebValdiviaDojo.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Tienda(int v_rut)
        {
            List<carrito> car = ListarCarrito(v_rut);
            List<prodGeneral> ob = ListarProdGen();

            ViewBag.Carrito = car;
            ViewBag.ProdGen = ob;
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult Producto(string v_nom)
        {
            List<prodEspe> esp = ListarProdEspe(v_nom);

            ViewBag.ProdEsp = esp;
            return View();
        }

        [HttpPost]
        public ActionResult Producto(int p_talla, int p_rut)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                int resultado = cliente.AgDetalleCom(p_talla, 1, p_rut);
                if (resultado == 1)
                {
                    ViewBag.Mensaje = "Producto añadido al carrito";
                    List<carrito> car = ListarCarrito(p_rut);
                    ViewBag.Carrito = car;
                }
                else
                {
                    ViewBag.Mensaje = "Ha ocurrido un error";
                }
            }
            catch (Exception)
            {
                ViewBag.Mensaje = "Ha ocurrido un error";
            }
            finally
            {
                cliente.Close();
            }
            // Realiza una redirección a la acción que carga la vista Tienda.cshtml
            return RedirectToAction("Tienda", new { v_rut= p_rut });
            //return View("~/Views/Home/Tienda.cshtml");
        }

        //
        public List<prodGeneral> ListarProdGen()
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                return cliente.ListadoProdGen().ToList();
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

        public List<prodEspe> ListarProdEspe(string v_nom)
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                return cliente.ListadoProdEspe(v_nom).ToList();
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

    }
}
