using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebValdiviaDojo.WS_ValdiviaDojo;

namespace WebValdiviaDojo.Controllers
{
    public class AdmController : Controller
    {
        // ADMINISTRAR PRODUCTO GET
        public ActionResult AdmProducto()
        {
            
            List<talla> ta = Listartalla();
            List<prodEspe> esp = ListarProdEspe(null);
            List<descuentoProd> des = ListarDescuento();
            List<tipoProducto> tp = ListarTipoProd();
            ViewBag.TipoProducto = tp;
            ViewBag.Descuento = des;
            ViewBag.Talla = ta;
            ViewBag.ProdEsp = esp;
            return View();
        }
        public ActionResult AdmDescuentos()
        {
            List<descuentoProd> des = ListarDescuento();
            ViewBag.Descuento = des;
            return View();
        }


        //ADMINISTRAR ALUMNOS 
        public ActionResult AdmAlumno()
        {
            List<usuario> usus = ListarUsuarios();
            List<tipoUsuario> tpu = ListarTipoUsu();
            List<genero> gen = ListarGenero();
            List<cinturon> cin = ListarCinturon();
            ViewBag.Usuarios = usus;
            ViewBag.TipoUsuario = tpu;
            ViewBag.Genero = gen;
            ViewBag.Cinturon = cin;

            return View();
        }

        public ActionResult ModUsuario(int rut, string pnombre, string snombre, string apater, string amater, string celular, string celularemer, string dire, string peso, string altura, DateTime fechanac, int p_gen, int p_t_usu, int p_cin)
        {
            WS_DojoClient cliente = new WS_DojoClient();

            //
            try
            {
                int resultado = cliente.ModUsuario(rut, pnombre, snombre, apater, amater, fechanac.ToString("dd/MM/yyyy"), celular, celularemer, dire, peso, altura, p_gen, p_t_usu, p_cin);
                ViewBag.Mensaje = "Datos actualizados exitosamente";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al llamar al servicio web: " + ex.Message);
            }
            finally
            {
                cliente.Close();
            }
            return RedirectToAction("AdmAlumno");
        }



        // ADMINISTRAR PRODUCTO POST
        [HttpPost]
        public ActionResult AdmProducto(HttpPostedFileBase imagen, int p_id, string p_nom, string p_des, int p_precio, int p_stock, string p_tipo_prod, string p_desc, string p_talla)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                if (imagen != null && imagen.ContentLength > 0)
                {
                    string nombreArchivo = p_nom + ".jpg";
                    string rutaCarpetaProd = Server.MapPath("~/Img/GaleriaProductos/");
                    if (!Directory.Exists(rutaCarpetaProd))
                    {
                        Directory.CreateDirectory(rutaCarpetaProd);
                    }
                    string ruta = Path.Combine(rutaCarpetaProd, nombreArchivo);
                    if (System.IO.File.Exists(ruta))
                    {
                        System.IO.File.Delete(ruta);
                    }
                    imagen.SaveAs(ruta);
                }
                cliente.ModProducto(p_id,p_nom, p_des, p_precio, p_stock, p_tipo_prod, p_desc, p_talla);
                ViewBag.Mensaje = "Producto modificado exitosamente.";
            }
            catch
            {
                ViewBag.Mensaje = "Ha ocurrido un error al agregar producto";
                throw;
            }
            return RedirectToAction("AdmProducto");
        }


        //AGREGAR PRODUCTO GET
        public ActionResult AdmAddProd()
        {
            List<talla> ta = Listartalla();
            List<descuentoProd> des = ListarDescuento();
            List<tipoProducto> tp = ListarTipoProd();
            ViewBag.TipoProducto = tp;
            ViewBag.Descuento = des;
            ViewBag.Talla = ta;
            return View();
        }


        //AGREGAR PRODUCTO POST
        [HttpPost]
        public ActionResult AdmAddProd(HttpPostedFileBase imagen,string p_nom, string p_des, int p_precio,int p_stock, string p_tipo_prod, string p_desc, string p_talla )
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                if (imagen != null && imagen.ContentLength > 0)
                {
                    // Nombre específico para la imagen (puedes personalizarlo según tus necesidades)
                    string nombreArchivo = p_nom + ".png";
                    // Ruta completa del archivo
                    string rutaCarpetaProd = Server.MapPath("~/Img/GaleriaProductos/");

                    // Verificar si la carpeta "Usuario" existe, si no, crearla
                    if (!Directory.Exists(rutaCarpetaProd))
                    {
                        Directory.CreateDirectory(rutaCarpetaProd);
                    }

                    string ruta = Path.Combine(rutaCarpetaProd, nombreArchivo);

                    // Verificar si el archivo ya existe
                    if (System.IO.File.Exists(ruta))
                    {
                        // Si existe, eliminar el archivo existente
                        System.IO.File.Delete(ruta);
                    }
                    // Guardar la nueva imagen con el nombre específico
                    imagen.SaveAs(ruta);
                }
                cliente.AgProducto(p_nom, p_des, p_precio, p_stock, p_tipo_prod, p_desc, p_talla);
                ViewBag.Mensaje = "Producto registrado exitosamente.";
            }
            catch
            {
                ViewBag.Mensaje = "Ha ocurrido un error al agregar producto";
                throw;
            }

            return RedirectToAction("AdmAddProd");
        }

        //ADMINISTRAR SOLICITUDES GET
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
        //ADMINISTRAR SOLICITUDES POST
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

        //ADMINISTRAR EVENTOS
        public ActionResult AdmEvento()
        {
            List<evento> ev = ListarEvento();
            List<tipoEvento> tpev = ListarTipoEvento();
            ViewBag.TipoEvento = tpev;
            ViewBag.Evento = ev;
            return View();
        }

        //AGREGAR EVENTOS GET
        public ActionResult AdmAddEvento()
        {

            List<tipoEvento> tpev = ListarTipoEvento();
            ViewBag.TipoEvento = tpev;
            return View();
        }

        //AGREGAR EVENTOS POST
        [HttpPost]
        public ActionResult AdmAddEvento(HttpPostedFileBase imagen, string p_nom, string p_des, string p_dire, DateTime p_hora, int p_t_eve)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                if (imagen != null && imagen.ContentLength > 0)
                {
                    string nombreArchivo = p_nom + ".png";
                    string rutaCarpetaProd = Server.MapPath("~/Img/GaleriaEvento/");
                    if (!Directory.Exists(rutaCarpetaProd))
                    {
                        Directory.CreateDirectory(rutaCarpetaProd);
                    }
                    string ruta = Path.Combine(rutaCarpetaProd, nombreArchivo);
                    if (System.IO.File.Exists(ruta))
                    {
                        System.IO.File.Delete(ruta);
                    }
                    imagen.SaveAs(ruta);
                }
                cliente.AgEvento(p_nom, p_des, p_dire, p_hora.ToString("dd/MM/yyyy hh:mm"), p_t_eve);
                ViewBag.Mensaje = "Evento registrado exitosamente.";
            }
            catch
            {
                ViewBag.Mensaje = "Ha ocurrido un error al agregar evento";
                throw;
            }

            return RedirectToAction("AdmAddEvento");
        }

        //MODIFICAR EVENTOS
        [HttpPost]
        public ActionResult ModEvento(HttpPostedFileBase imagen, int p_id, string p_nom, string p_des, string p_dire, DateTime p_hora, string p_t_eve)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                if (imagen != null && imagen.ContentLength > 0)
                {
                    string nombreArchivo = p_nom + ".png";
                    string rutaCarpetaProd = Server.MapPath("~/Img/GaleriaEvento/");
                    if (!Directory.Exists(rutaCarpetaProd))
                    {
                        Directory.CreateDirectory(rutaCarpetaProd);
                    }
                    string ruta = Path.Combine(rutaCarpetaProd, nombreArchivo);
                    if (System.IO.File.Exists(ruta))
                    {
                        System.IO.File.Delete(ruta);
                    }
                    imagen.SaveAs(ruta);
                }
                cliente.ModEvento(p_id, p_nom, p_des, p_dire, p_hora.ToString("dd/MM/yyyy hh:mm"), p_t_eve);
            }
            catch
            {
                throw;
            }

            return RedirectToAction("AdmEvento");
        }
        //ELIMINAR EVENTOS
        public ActionResult EliEvento(int p_id)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                cliente.EliEvento(p_id);
            }
            catch
            {
                throw;
            }

            return RedirectToAction("AdmEvento");
        }






        //ADMIN CLASE GET
        public ActionResult AdmAddClase()
        {
            List<clases> cls = ListarClases();
            List<clasesNivel> clsniv = ListarClasesNivel();

            ViewBag.Clase = cls;
            ViewBag.ClaseNivel = clsniv;
            return View();
        }

        //ADMIN CLASE POST
        [HttpPost]
        public ActionResult AdmAddClase(string p_dia_semana, string p_hora_inicio,string p_hora_fin, int p_id_clase, int p_id_nivel)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                cliente.AgHorario(p_dia_semana, p_hora_inicio, p_hora_fin, p_id_clase, p_id_nivel);
                ViewBag.Mensaje = "Horario registrado exitosamente.";
            }
            catch
            {
                ViewBag.Mensaje = "Ha ocurrido un error al agregar horario";
                throw;
            }

            return RedirectToAction("AdmAddClase");
        }

        //ADM USUARIOS GET
        public ActionResult AdmUsuario()
        {
            List<usuario> usu = ListarUsuarios();
            List<tipoUsuario> tipousu = ListarTipoUsu();
            ViewBag.Usuario = usu;
            ViewBag.TipoUsuario = tipousu;
            return View();
        }

        //AdM TIPO USUARIO GET

        public ActionResult AdmTipoUsuario()
        {
            List<tipoUsuario> tipousu = ListarTipoUsu();

            ViewBag.TipoUsuario = tipousu;
            return View();
        }

        //ADM TIPO EVENTO GET
        public ActionResult AdmTipoEvento()
        {
            List<tipoEvento> tipo = ListarTipoEvento();

            ViewBag.TipoEvento = tipo;
            return View();
        }

        //AGREGAR TIPO EVENTO GET
        public ActionResult AdmAddTipoEve()
        {
            return View();
        }

        //AGREGAR TIPO EVENTO POST
        [HttpPost]
        public ActionResult AdmAddTipoEve(string p_nom)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                cliente.AgTipoEvento(p_nom);
            }
            catch
            {
                throw;
            }

            return RedirectToAction("AdmAddTipoEve");
        }

        [HttpPost]
        //MODIFICAR TIPO EVENTO
        public ActionResult ModTipoEve(int p_id,string p_nom)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                cliente.ModTipoEve(p_id,p_nom);
            }
            catch
            {
                throw;
            }

            return RedirectToAction("AdmTipoEvento");
        }

        //ELIMINAR TIPO EVENTO
        public ActionResult EliTipoEve(int p_id)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                cliente.EliTipoEve(p_id);
            }
            catch
            {
                throw;
            }

            return RedirectToAction("AdmTipoEvento");
        }

        //ADM TIPO SOLICITUD
        public ActionResult AdmTipoSolicitud()
        {
            List<tipoSolicitud> tipo = ListarTipoSolicitud();

            ViewBag.TipoSolicitud = tipo;
            return View();
        }


        //AGREGAR TIPO SOLICITUD GET
        public ActionResult AdmAddTipoSoli()
        {
            return View();
        }
        //AGREGAR TIPO SOLICITUD POST
        [HttpPost]
        public ActionResult AdmAddTipoSoli(string p_nombre)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                cliente.AgTipoSolicitud(p_nombre);
            }
            catch
            {
                throw;
            }

            return RedirectToAction("AdmAddTipoSoli");
        }
        [HttpPost]
        //MODIFICAR TIPO SOLICITUD
        public ActionResult ModTipoSoli(int p_id, string p_nombre)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                cliente.ModTipoSoli(p_id, p_nombre);
            }
            catch
            {
                throw;
            }

            return RedirectToAction("AdmTipoSolicitud");
        }

        //ELIMINAR TIPO SOLICITUD
        public ActionResult EliTipoSoli(int p_id)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                cliente.EliTipoSoli(p_id);
            }
            catch
            {
                throw;
            }

            return RedirectToAction("AdmTipoSolicitud");
        }

        //ADM TIPO PRODUCTO
        public ActionResult AdmTipoProducto()
        {
            List<tipoProducto> tipo = ListarTipoProd();

            ViewBag.TipoProducto = tipo;
            return View();
        }

        //AGREGAR TIPO PRODUCTO GET
        public ActionResult AdmAddTipoProd()
        {
            return View();
        }

        //AGREGAR TIPO PRODUCTO POST
        [HttpPost]
        public ActionResult AdmAddTipoProd(string p_des)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                cliente.AgTipoProducto(p_des);
            }
            catch
            {
                throw;
            }

            return RedirectToAction("AdmAddTipoProd");
        }
        //MODIFICAR TIPO PRODUCTO
        public ActionResult ModTipoProd(int p_id, string p_des)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                cliente.ModTipoProducto(p_id, p_des);
            }
            catch
            {
                throw;
            }

            return RedirectToAction("AdmTipoProducto");
        }
        //ELIMINAR TIPO PRODUCTO
        public ActionResult EliTipoProd(int p_id)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                cliente.EliTipoProducto(p_id);
            }
            catch
            {
                throw;
            }

            return RedirectToAction("AdmTipoProducto");
        }

        //ADM TIPO CLASE
        public ActionResult AdmTipoClase()
        {
            List<clases> tipo = ListarClases();

            ViewBag.Clases = tipo;
            return View();
        }

        //AGREGAR TIPO CLASE GET
        public ActionResult AdmAddTipoClase()
        {
            return View();
        }

        //AGREGAR TIPO CLASE POST
        [HttpPost]
        public ActionResult AdmAddTipoClase(string p_nombre)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                cliente.AgTipoClase(p_nombre);
            }
            catch
            {
                throw;
            }

            return RedirectToAction("AdmAddTipoClase");
        }

        //MODIFICAR TIPO CLASE
        public ActionResult ModTipoClase(int p_id, string p_nombre)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                cliente.ModTipoClase(p_id, p_nombre);
            }
            catch
            {
                throw;
            }

            return RedirectToAction("AdmTipoClase");
        }

        //ELIMINAR TIPO CLASE
        public ActionResult EliTipoClase(int p_id)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                cliente.EliTipoClase(p_id);
            }
            catch
            {
                throw;
            }

            return RedirectToAction("AdmTipoClase");
        }

        //ADM CLASE NIVEL
        public ActionResult AdmClaseNivel()
        {
            List<clasesNivel> tipo = ListarClasesNivel();

            ViewBag.ClasesNivel = tipo;
            return View();
        }

        //AGREGAR CLASE NIVEL GET
        public ActionResult AdmAddClaNivel()
        {
            return View();
        }

        //AGREGAR CLASE NIVEL POST
        [HttpPost]
        public ActionResult AdmAddClaNivel(string p_nombre)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                cliente.AgClaseNivel(p_nombre);
            }
            catch
            {
                throw;
            }

            return RedirectToAction("AdmAddClaNivel");
        }

        //MODIFICAR CLASE NIVEL
        public ActionResult ModClaseNivel(int p_id, string p_nombre)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                cliente.ModClaseNivel(p_id, p_nombre);
            }
            catch
            {
                throw;
            }

            return RedirectToAction("AdmClaseNivel");
        }

        //ELIMINAR CLASE NIVEL
        public ActionResult EliClaseNivel(int p_id)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                cliente.EliClaseNivel(p_id);
            }
            catch
            {
                throw;
            }

            return RedirectToAction("AdmClaseNivel");
        }

        //ADM CINTURON
        public ActionResult AdmCinturon()
        {
            List<cinturon> tipo = ListarCinturon();

            ViewBag.Cinturon = tipo;
            return View();
        }

        //AGREGAR CINTURON GET
        public ActionResult AdmAddCinturon()
        {
            return View();
        }

        //AGREGAR CINTURON POST
        [HttpPost]
        public ActionResult AdmAddCinturon(string p_nombre, string p_color)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                cliente.AgCinturon(p_nombre, p_color);
            }
            catch
            {
                throw;
            }

            return RedirectToAction("AdmAddCinturon");
        }
        //MODIFICAR CINTURON
        public ActionResult ModCinturon(int p_id, string p_nombre, string p_color)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                cliente.ModCinturon(p_id, p_nombre, p_color);
            }
            catch
            {
                throw;
            }

            return RedirectToAction("AdmCinturon");
        }

        //ELIMINAR CINTURON
        public ActionResult EliCinturon(int p_id)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                cliente.EliCinturon(p_id);
            }
            catch
            {
                throw;
            }

            return RedirectToAction("AdmCinturon");
        }


        //ADM TIPO ENCUESTA
        public ActionResult AdmTipoEncuesta()
        {
            List<tipoEncuesta> tipo = ListarTipoEncuesta();

            ViewBag.TipoEncuesta = tipo;
            return View();
        }

        //AGREGAR TIPO ENCUESTA GET
        public ActionResult AdmAddTipoEncu()
        {
            return View();
        }

        //AGREGAR TIPO ENCUESTA POST
        [HttpPost]
        public ActionResult AdmAddTipoEncu(string p_nom)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                cliente.AgTipoEncuesta(p_nom);
            }
            catch
            {
                throw;
            }

            return RedirectToAction("AdmAddTipoEncu");
        }

        //MODIFICAR TIPO ENCUESTA
        public ActionResult ModTipoEncuesta(int p_id, string p_nom)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                cliente.ModTipoEncuesta(p_id, p_nom);
            }
            catch
            {
                throw;
            }

            return RedirectToAction("AdmTipoEncuesta");
        }

        //ELIMINAR TIPO ENCUESTA
        public ActionResult EliTipoEncuesta(int p_id)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                cliente.EliTipoEncuesta(p_id);
            }
            catch
            {
                throw;
            }

            return RedirectToAction("AdmTipoEncuesta");
        }









        //FUNCIONES
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


        public ActionResult EliminarProducto(int p_id,string p_nom)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                // Ruta completa del archivo
                string rutaCarpetaProd = Server.MapPath("~/Img/GaleriaProductos/");

                // Nombre específico para la imagen
                string nombreArchivo = p_nom + ".png";

                // Ruta completa del archivo
                string ruta = Path.Combine(rutaCarpetaProd, nombreArchivo);

                // Verificar si el archivo existe antes de intentar eliminarlo
                if (System.IO.File.Exists(ruta))
                {
                    System.IO.File.Delete(ruta);
                }

                cliente.EliProducto(p_id);
                ViewBag.Mensaje = "Producto eliminado";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al llamar al servicio web: " + ex.Message);
            }
            finally
            {
                cliente.Close();
            }
            return RedirectToAction("AdmProducto");
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

        public List<usuario> ListarUsuarios()
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                return cliente.ListarUsuarios().ToList();
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

        public List<tipoProducto> ListarTipoProd()
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                return cliente.ListaTipoProd().ToList();
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
        public List<prodEspe> ListarProdEspe(string v_nom)
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                return cliente.ListadoProdEspe(v_nom).ToList();
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
        public List<talla> Listartalla()
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                return cliente.ListadoTalla().ToList();
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
        public List<tipoUsuario> ListarTipoUsu()
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                return cliente.ListarTipoUsuarios().ToList();
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
        public List<cinturon> ListarCinturon()
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                return cliente.ListarCinturon().ToList();
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

        public List<tipoEncuesta> ListarTipoEncuesta()
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                return cliente.ListaTipoEncuesta().ToList();
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



        /*
         * 
         *                  ADMINISTRAR DESCUENTOS
         * 
         */
        public List<descuentoProd> ListarDescuento()
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                return cliente.ListadoDescuento().ToList();
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
        //agregar descuento
        public ActionResult AgDescuento(DateTime p_fecha_inicio, DateTime p_fecha_fin, string p_valor_desc, string p_porc_desc)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                cliente.AgreDescuento(p_fecha_inicio.ToString("dd/MM/yyyy"),p_fecha_fin.ToString("dd/MM/yyyy"), p_valor_desc, p_porc_desc);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al llamar al servicio web: " + ex.Message);
            }
            finally
            {
                cliente.Close();
            }
            return RedirectToAction("AdmDescuentos");
        }
        //Modificar descuento
        public ActionResult ModDescuento(int p_id, DateTime p_fecha_inicio, DateTime p_fecha_fin, string p_valor_desc, string p_porc_desc)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                cliente.ModDescuento(p_id,p_fecha_inicio.ToString("dd/MM/yyyy"), p_fecha_fin.ToString("dd/MM/yyyy"), p_valor_desc, p_porc_desc);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al llamar al servicio web: " + ex.Message);
            }
            finally
            {
                cliente.Close();
            }
            return RedirectToAction("AdmDescuentos");
        }

        public ActionResult EliDescuento(int p_id)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                cliente.EliDescuento(p_id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al llamar al servicio web: " + ex.Message);
            }
            finally
            {
                cliente.Close();
            }
            return RedirectToAction("AdmDescuentos");
        }
    }
}