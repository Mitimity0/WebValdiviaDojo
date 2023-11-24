using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebValdiviaDojo.WS_ValdiviaDojo;

namespace WebValdiviaDojo.Controllers
{
    public class ClaseController : Controller
    {
        // GET: Clase
        public ActionResult Clase()
        {
            return View();
        }

        public ActionResult ClaseInscrita(int v_rut)
        {
            List<observacion> ob = ListarObservacion(v_rut);

            ViewBag.observacion = ob;

            return View();
        }

        public List<observacion> ListarObservacion(int p_rut)
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                return cliente.ListadoObser(p_rut).ToList();
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