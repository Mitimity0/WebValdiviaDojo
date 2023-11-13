using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebValdiviaDojo.WS_ValdiviaDojo;

namespace WebValdiviaDojo.Controllers
{
    public class SesionController : Controller
    {
        public ActionResult Login()
        {
            ViewBag.Mensaje = "";

            return View();
        }
        [HttpPost]
        public ActionResult Login(String p_correo, String p_pass)
        {
            var resultado= IniciarSesion(p_correo, p_pass);
            ViewBag.Mensaje = resultado;

            return View();
        }

        public ActionResult NuevoUsuario()
        {
            List<genero> generos = ListarGenero();
            ViewBag.generos = generos;

            return View();
        }

        [HttpPost]
        public ActionResult NuevoUsuario(String correo, String pass,String rut, String dv, String pnombre, String snombre, String apater, String amater, String celular, String celularemer, String fechanac, String gene, String dire)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            List<genero> generos = ListarGenero();
            ViewBag.generos = generos;
            try
            {
                
                var usu = cliente.CrearUsuario(rut, dv, pnombre, snombre, apater, amater, fechanac, celular, celularemer, dire,null,null, gene,"4","1");
                var cre = cliente.CrearCredencial(correo, pass, rut);
                ViewBag.Mensaje = "Usuario Creado"+cre+" - "+usu;
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

        public int IniciarSesion(string correo, string pass)
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                int resultado = cliente.Login(correo, pass);
                return resultado;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al llamar al servicio web: " + ex.Message);
                return -1;
            }
            finally
            {
                cliente.Close();
            }
        }

    }
}