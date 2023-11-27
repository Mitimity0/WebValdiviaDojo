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

        public ActionResult ClaseInscrita(string v_rut)
        {
            List<observacion> ob = ListarObservacion(v_rut,null);

            ViewBag.observacion = ob;

            return View();
        }
        public ActionResult ComentarAvance(string v_rut)
        {
            DateTime fechaActual = DateTime.Now;
            List<observacion> ob = ListarObservacion(v_rut, fechaActual.ToString("dd/MM/yy"));

            ViewBag.observacion = ob;

            return View();
        }

        public ActionResult ComentarObserva(string p_observacion,int p_id,int p_rut)
        {

            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                cliente.ModObservacion(p_observacion, p_id, p_rut);
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
            return RedirectToAction("ComentarAvance");
        }


        public List<observacion> ListarObservacion(string p_rut,string p_fecha)
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                return cliente.ListadoObser(p_rut, p_fecha).ToList();
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