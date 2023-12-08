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
        public ActionResult Referido(string v_rut)
        {
            ViewBag.rut = v_rut;

            List<referido> ob = ListarReferido(v_rut);
            ViewBag.referido = ob;
            return View();
        }



        public ActionResult admSolicitudAlumno(string v_rut)
        {
            if (v_rut == null)
            {
                ViewBag.Mensaje = "Debe iniciar sesión";
            }
            List<tipoSolicitud> ob = ListarTipoSolicitud();
            List<solicitud> Solicitud = ListarSolicitud(v_rut);
            ViewBag.tpSolicitud = ob;
            ViewBag.sol = Solicitud;

            return View();
        }

        //LISTAR
        public List<solicitud> ListarSolicitud(string p_rut)
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                return cliente.ListaSolicitud(null, null, p_rut).ToList();
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
        
        public List<referido> ListarReferido(string p_rut)
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                return cliente.ListaReferido(p_rut).ToList();
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

        //FUNCIONES

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
            return RedirectToAction("admSolicitudAlumno", new { v_rut = p_rut });
        }


        [HttpPost]
        public ActionResult ModificarSolicitud(int p_id, string p_sol, string p_res, int p_tipo_sol, int p_estado, int p_rut)
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
                return RedirectToAction("admSolicitudAlumno", new { v_rut = p_rut });
            }
            finally
            {
                cliente.Close();
            }

            // Redirigir a la página de inicio
            return RedirectToAction("admSolicitudAlumno", new { v_rut = p_rut });
        }
        [HttpPost]
        public ActionResult ElSolicitud(int p_id,int v_rut)
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                cliente.EliSolicitud(p_id);
                ViewBag.Mensaje = "Solicitud eliminada";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al llamar al servicio web: " + ex.Message);
                return RedirectToAction("admSolicitudAlumno", new { v_rut = v_rut });
            }
            finally
            {
                cliente.Close();
            }

            // Redirigir a la página de inicio
            return RedirectToAction("admSolicitudAlumno", new { v_rut = v_rut  });
        }


        [HttpPost]
        public ActionResult ElReferido(int p_rut, int p_rut_referido)
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                cliente.EliReferidos(p_rut, p_rut_referido);
                ViewBag.Mensaje = "Familiar eliminado";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al llamar al servicio web: " + ex.Message);
                return RedirectToAction("Referido", new { v_rut = p_rut_referido });
            }
            finally
            {
                cliente.Close();
            }

            // Redirigir a la página de inicio
            return RedirectToAction("Referido", new { v_rut = p_rut_referido });
        }

        [HttpPost]
        public ActionResult AgReferido(int p_rut, int p_rut_referido)
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                cliente.AgReferidos(p_rut, p_rut_referido);
                ViewBag.Mensaje = "Familiar Agregado";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al llamar al servicio web: " + ex.Message);
                return RedirectToAction("Referido", new { v_rut = p_rut_referido });
            }
            finally
            {
                cliente.Close();
            }

            // Redirigir a la página de inicio
            return RedirectToAction("Referido", new { v_rut = p_rut_referido });
        }

    }
}








