using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebValdiviaDojo.WS_ValdiviaDojo;

namespace WebValdiviaDojo.Controllers
{
    public class SesionController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(String p_correo, String p_pass)
        {
            List<usuario> resultado = IniciarSesion(p_correo, p_pass);

            if (resultado != null && resultado.Any())
            {
                var primerUsuario = resultado.First();
                HttpContext.Session["Usuario"] = primerUsuario;
                return View("~/Views/Home/Index.cshtml");
            }

            ViewBag.Mensaje = "Usuario no encontrado";
            return View();
        }


        public ActionResult MostrarDatosUsuario()
        {
            // Obtener la información del usuario desde TempData
            var usuario = TempData["Usuario"] as usuario;

            if (usuario != null)
            {
                ViewBag.Usuario = usuario;

                return View("~/Views/Home/Index.cshtml");
            }

            // Manejar el caso en el que no se encuentre información del usuario
            return RedirectToAction("NuevoUsuario");
        }
        public ActionResult CerrarSesion()
        {
            // Limpiar la sesión
            HttpContext.Session.Clear();

            // Redirigir a la página de inicio
            return View("~/Views/Home/Index.cshtml");
        }



        public ActionResult NuevoUsuario()
        {
            List<genero> generos = ListarGenero();
            ViewBag.generos = generos;
            var usuario = TempData["Usuario"] as List<usuario>;
            ViewBag.Usuario = usuario;

            return View();
        }

        [HttpPost]
        public ActionResult NuevoUsuario(String correo, String pass, int rut, String dv, String pnombre, String snombre, String apater, String amater, String celular, String celularemer, DateTime fechanac, int gene, String dire)
        {
            List<genero> generos = ListarGenero();
            ViewBag.generos = generos;

            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                var usu = cliente.CrearUsuario(correo, pass, rut, dv, pnombre, snombre, apater, amater, fechanac.ToString("dd/MM/yyyy"), celular, celularemer, dire, null, null, gene, 4, 1);

                return View();
            }

            catch (Exception ex)
            {
                ViewBag.Mensaje = "Error al llamar al servicio web: " + ex.Message;
            }
            finally
            {
                cliente.Close();
            }

            return View();
        }


        public List<genero> ListarGenero()
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                return cliente.ListadoGenero().ToList();
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

        public List<usuario> IniciarSesion(string correo, string pass)
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                return cliente.Login(correo, pass).ToList();
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