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
        // GET: Inscripcion
        public ActionResult ClasesDisponibles()
        {
            List<horario> horarios = Listarhorario(null, null);
            List<clases> clases = ListarClases();
            List<clasesNivel> nivel = ListarClasesNivel();

            ViewBag.horarios = horarios;
            ViewBag.clases = clases;
            ViewBag.clasesNivel = nivel;
            return View();
        }
        [HttpPost]
        public ActionResult ClasesDisponibles(string p_id_clase, string p_id_nivel)
        {
            List<horario> horarios = Listarhorario(p_id_clase, p_id_nivel);
            List<clases> clases = ListarClases();
            List<clasesNivel> nivel = ListarClasesNivel();

            ViewBag.horarios = horarios;
            ViewBag.clases = clases;
            ViewBag.clasesNivel = nivel;

            // Validar y asignar valores seleccionados
            ViewBag.SelectedClase = clases.Any(c => c.id_clase.ToString() == p_id_clase) ? p_id_clase : "";
            ViewBag.SelectedNivel = nivel.Any(n => n.id_nivel.ToString() == p_id_nivel) ? p_id_nivel : "";

            return View();
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
