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
            List<tipoSolicitud> ob = ListarTipoSolicitud();
            List<estado> es = ListarEstado();
            List<solicitud> Solicitud = ListarSolicitud();

            ViewBag.tpSolicitud = ob;
            ViewBag.estado = es;
            ViewBag.sol = Solicitud;
            return View();
        }

        public ActionResult EliminarSolicitud(int p_id_sol)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                cliente.EliSolicitud(p_id_sol);
                ViewBag.Mensaje = "Solicitud eliminada";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al llamar al servicio web: " + ex.Message);
            }
            finally
            {
                cliente.Close();
            }
            return RedirectToAction("AdmSolicitud");
        }
        [HttpPost]
        public ActionResult AdmSolicitud(int p_id, String p_sol, String p_res, int p_tipo_sol, int p_estado, int p_rut)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                cliente.ModSolicitud(p_id, p_sol, p_res, p_tipo_sol, p_estado, p_rut);
                ViewBag.Mensaje = "Solicitud Modificada";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al llamar al servicio web: " + ex.Message);
            }
            finally
            {
                cliente.Close();
            }
            List<tipoSolicitud> ob = ListarTipoSolicitud();
            List<estado> es = ListarEstado();
            List<solicitud> Solicitud = ListarSolicitud();

            ViewBag.tpSolicitud = ob;
            ViewBag.estado = es;
            ViewBag.sol = Solicitud;
            return View();
        }

        public List<solicitud> ListarSolicitud()
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                return cliente.ListaSolicitud(null, null, null).ToList();
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
        public List<estado> ListarEstado()
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                return cliente.ListaEstado().ToList();
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