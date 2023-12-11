using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebValdiviaDojo.WS_ValdiviaDojo;
using System.Security.Cryptography;
using System.Text;
using EFTEC;

namespace WebValdiviaDojo.Controllers
{
    public class SesionController : Controller
    {
        public ActionResult PerfilUsu()
        {

            // Obtener la ruta de la carpeta "Usuario"
            string rutaCarpetaUsuario = Server.MapPath("~/Img/Usuario/");

            // Verificar si la carpeta "Usuario" existe, si no, crearla
            if (!Directory.Exists(rutaCarpetaUsuario))
            {
                Directory.CreateDirectory(rutaCarpetaUsuario);
            }

            // Obtener la lista de archivos en la carpeta "Usuario"
            string[] archivos = Directory.GetFiles(rutaCarpetaUsuario);

            // Pasar la lista de archivos a la vista
            ViewBag.Archivos = archivos;

            return View();
        }

        [HttpPost]
        public ActionResult PerfilUsu(HttpPostedFileBase imagen, int rut, string pnombre, string snombre, string apater, string amater, string celular, string celularemer, string dire, string peso, string altura, DateTime fechanac, int p_gen, int p_t_usu, int p_cin)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                if (imagen != null && imagen.ContentLength > 0)
                {
                    // Nombre específico para la imagen (puedes personalizarlo según tus necesidades)
                    string nombreArchivo = rut + ".png";

                    // Ruta completa del archivo
                    string rutaCarpetaUsuario = Server.MapPath("~/Img/Usuario/");

                    // Verificar si la carpeta "Usuario" existe, si no, crearla
                    if (!Directory.Exists(rutaCarpetaUsuario))
                    {
                        Directory.CreateDirectory(rutaCarpetaUsuario);
                    }

                    string ruta = Path.Combine(rutaCarpetaUsuario, nombreArchivo);

                    // Verificar si el archivo ya existe
                    if (System.IO.File.Exists(ruta))
                    {
                        // Si existe, eliminar el archivo existente
                        System.IO.File.Delete(ruta);
                    }
                    // Guardar la nueva imagen con el nombre específico
                    imagen.SaveAs(ruta);

                    ViewBag.Mensaje = "Imagen cambiada exitosamente.";
                    
                }

                int resultado = cliente.ModUsuario(rut, pnombre, snombre, apater, amater, fechanac.ToString("dd/MM/yyyy"), celular,celularemer,dire,peso,altura, p_gen, p_t_usu,p_cin);
                ViewBag.Mensaje = "Datos actualizados exitosamente, estos se veran reflejados al reiniciar su sesion.";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al llamar al servicio web: " + ex.Message);
            }
            finally
            {
                cliente.Close();
            }
            return View();
        }



        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string p_correo, string p_pass)
        {
            List<usuario> resultado = IniciarSesion(p_correo, p_pass);

            if (resultado != null && resultado.Any())
            {
                var primerUsuario = resultado.First();
                HttpContext.Session["Usuario"] = primerUsuario;
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Mensaje = "Usuario no encontrado ";
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

            ViewBag.Mensaje = "Ha finalizado su sesión";
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
        public ActionResult NuevoUsuario(string correo, string pass, string passCon, int rut, string dv, string pnombre, string snombre, string apater, string amater, string celular, string celularemer, DateTime fechanac, int gene, string dire)
        {
            List<genero> generos = ListarGenero();
            ViewBag.generos = generos;

            WS_DojoClient cliente = new WS_DojoClient();
            // Validar formato del RUT y calcular dígito verificador
            if (!EsRutValido(rut, dv))
            {
                ViewBag.Mensaje = "El RUT ingresado no es válido.";
                return View();
            }

            // Validar que la contraseña y la confirmación coincidan
            if (pass != passCon)
            {
                ViewBag.Mensaje = "La contraseña y la confirmación no coinciden.";
                return View();
            }
            try
            {
                var usu = cliente.CrearUsuario(correo, HashMD5(pass), rut, dv, pnombre, snombre, apater, amater, fechanac.ToString("dd/MM/yyyy"), celular, celularemer, dire, null, null, gene, 4, 1);
                // Una vez creado el usuario, intentamos iniciar sesión automáticamente
                var resultado = IniciarSesion(correo, pass);
                if (resultado != null && resultado.Any())
                {
                    var primerUsuario = resultado.First();
                    HttpContext.Session["Usuario"] = primerUsuario;
                    TempData["Usuario"] = primerUsuario;
                }
                ViewBag.Mensaje = "Su cuenta ha sido creada exitosamente.";
                return RedirectToAction("MostrarDatosUsuario");
            }

            catch
            {
                ViewBag.Mensaje = "No se ha podido registrar su cuenta.";
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
            string p_pass = HashMD5(pass);
            try
            {
                return cliente.Login(correo, p_pass).ToList();
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

        //ENCRIPTACION
        public string HashMD5(string clave)
        {
            MD5 md5 = MD5.Create();

            var claveArray = Encoding.UTF8.GetBytes(clave);

            var claveHash = md5.ComputeHash(claveArray);

            return toMd5String(claveHash);
        }
        public string toMd5String(byte[] hashArray)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hashArray.Length; i++)
            {
                sb.Append(hashArray[i].ToString());
            }

            return sb.ToString();
        }
        // Validación de RUT
        public bool EsRutValido(int rut, string dv)
        {
            // Formatear el RUT completo
            string rutCompleto = rut.ToString() + "-" + dv;

            // Validar el RUT usando RutChile
            if (RutChile.ValidarRut(rutCompleto))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}