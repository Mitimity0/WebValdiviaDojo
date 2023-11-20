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

        public ActionResult Tienda()
        {

            List<prodGeneral> ob = ListarProdGen();

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

    }
}
