using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebValdiviaDojo.WS_ValdiviaDojo;

namespace WebValdiviaDojo.Controllers
{
    public class AdmController : Controller
    {
        // GET: Adm
        public ActionResult Producto()
        {
            return View();
        }

        public ActionResult AdmSolicitud()
        {
            List<solicitud> Solicitud = ListarSolicitud();
            ViewBag.sol = Solicitud;
            return View();
        }


        public List<solicitud> ListarSolicitud()
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                // Asignar valores predeterminados no nulos
                int parametro1 = 0; // o cualquier otro valor predeterminado que tenga sentido en tu contexto
                int parametro2 = 0;
                int parametro3 = 0;

                return cliente.ListaSolicitud(parametro1, parametro2, parametro3).ToList();
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