using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebValdiviaDojo.WS_ValdiviaDojo;

namespace WebValdiviaDojo.Controllers
{
    public class InscripcionController : Controller
    {
        
        public ActionResult ValoresInscrip()
        {
            List<clases> clases = ListarClases();
            ViewBag.clases = clases;
            return View();
        }

        // GET: Inscripcion
        public ActionResult ClasesDisponibles(string p_id_clase, string p_id_nivel, string mens)
        {
            List<horario> horarios = Listarhorario(p_id_clase, p_id_nivel);
            List<clases> clases = ListarClases();
            List<clasesNivel> nivel = ListarClasesNivel();

            ViewBag.horarios = horarios;
            ViewBag.clases = clases;
            ViewBag.clasesNivel = nivel;
            if (mens!=null)
            {
                ViewBag.mensaje = mens;
            }
            // Validar y asignar valores seleccionados
            ViewBag.SelectedClase = clases.Any(c => c.id_clase.ToString() == p_id_clase) ? p_id_clase : "";
            ViewBag.SelectedNivel = nivel.Any(n => n.id_nivel.ToString() == p_id_nivel) ? p_id_nivel : "";
            return View();
        }

        public ActionResult AgregarMensual(int p_id_clase, int p_id_nivel, int p_rut)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            int p_estado = 3;
            try
            {
                DateTime p_fech = DateTime.Now;
                cliente.AgMensualidad(p_fech.ToString("dd/MM/yyyy"),p_rut, p_id_clase, p_estado, p_id_nivel);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al llamar al servicio web: " + ex.Message);
            }

            return RedirectToAction("ClasesDisponibles", new { mens = "Se ha ingresado con exito su mensualidad, ¿desea realizar el pago de inmediato?" });
        }

        /*  */
        public List<horario> Listarhorario(String id_clase, String id_nivel)
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                return cliente.ListadoHorario(id_clase, id_nivel).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al llamar al servicio web: " + ex.Message);
            }

            return null;
        }

        public List<clases> ListarClases()
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                return cliente.ListadoClase().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al llamar al servicio web: " + ex.Message);
            }

            return null;
        }
        public List<clasesNivel> ListarClasesNivel()
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                return cliente.ListadoClaseNivel().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al llamar al servicio web: " + ex.Message);
            }

            return null;
        }


    }
}
