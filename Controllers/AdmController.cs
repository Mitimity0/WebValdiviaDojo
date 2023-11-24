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

    }
}