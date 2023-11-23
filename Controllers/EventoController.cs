using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebValdiviaDojo.WS_ValdiviaDojo;

namespace WebValdiviaDojo.Controllers
{
    public class EventoController : Controller
    {
        // GET: Evento
        public ActionResult Evento()
        {
            List<evento> ev = ListarEvento();
            List<tipoEvento> tpev = ListarTipoEvento();

            ViewBag.even = ev;
            ViewBag.tipoEven = tpev;

            return View();
        }

        //OPERACION CUANDO ENVIE A UN POST
        [HttpPost]
        public ActionResult Evento(int p_id_ev,int v_rut)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                cliente.AgParticipacion(p_id_ev, null, v_rut);
                ViewBag.Mensaje = "Esta registrado para participar.";
            }
            catch (Exception)
            {
                ViewBag.Mensaje = "Ocurrio un error.";
                throw;
            }
            List<evento> ev = ListarEvento();
            List<tipoEvento> tpev = ListarTipoEvento();

            ViewBag.even = ev;
            ViewBag.tipoEven = tpev;

            return View();
        }

        //LISTADOS
        public List<evento> ListarEvento()
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                return cliente.ListadoEvento().ToList();
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
        public List<tipoEvento> ListarTipoEvento()
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                return cliente.ListadoTipoEvento().ToList();
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