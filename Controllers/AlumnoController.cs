using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebValdiviaDojo.WS_ValdiviaDojo;

namespace WebValdiviaDojo.Controllers
{
    public class AlumnoController : Controller
    {
        //
        public ActionResult EnvioSolicitud(int? v_rut)
        {
            int rut = v_rut ?? 0;

            if (rut == 0)
            {
                ViewBag.Mensaje = "Debe iniciar sesión";
            }
            ViewBag.rut = v_rut;
            List<tipoSolicitud> ob = ListarTipoSolicitud();
            ViewBag.tpSolicitud = ob;
            return View();
        }



        public List<tipoSolicitud> ListarTipoSolicitud()
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                return cliente.ListaTipoSolicitud().ToList();
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

        [HttpPost]
        public ActionResult EnvioSolicitud(string p_sol, int p_tipo, int p_rut)
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                cliente.AgreSolicitud(p_sol, p_tipo, p_rut);
                ViewBag.Mensaje = "Solicitud enviada";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al llamar al servicio web: " + ex.Message);
                return View("EnvioSolicitud");
            }
            finally
            {
                cliente.Close();
            }

            // Redirigir a la página de inicio
            return RedirectToAction("EnvioSolicitud", new { v_rut = p_rut });
        }

    }
}