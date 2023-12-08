using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebValdiviaDojo.WS_ValdiviaDojo;

namespace WebValdiviaDojo.Controllers
{
    public class CompraController : Controller
    {
        //
        public ActionResult PagosPendientes(string v_rut)
        {
            List<matricula> mat = ListarMatricula(v_rut, "3");
            List<mensualidad> men = ListarMensualidad(v_rut, "3");
            List<ordenCompra> ord = ListarOrdenCompra(v_rut,"3");

            ViewBag.Matricula = mat;
            ViewBag.Mensualidad = men;
            ViewBag.OrdenCompra = ord;
            return View();
        }


        

        public ActionResult Boleta(string v_rut, string p_id_matricula, string p_id_mensualidad, string p_id_compra)
        {
            List<boleta> bol = ListarBoleta(p_id_matricula, p_id_mensualidad, p_id_compra);

            ViewBag.v_rut = v_rut;
            ViewBag.Boleta = bol;
            return View();
        }

        public ActionResult ListaPagos(string v_rut)
        {
            List<matricula> mat = ListarMatricula(v_rut, "1");
            List<mensualidad> men = ListarMensualidad(v_rut, "1");
            List<ordenCompra> ord = ListarOrdenCompra(v_rut, "1");

            ViewBag.Matricula = mat;
            ViewBag.Mensualidad = men;
            ViewBag.OrdenCompra = ord;
            return View();
        }

        public ActionResult OrdenCompra(int v_rut)
        {
            List<carrito> car = ListarCarrito(v_rut);
            ViewBag.Carrito = car;
            return View();
        }

        public ActionResult NuevaOrdenCompra(int v_rut)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                cliente.AgOrdenCompra(v_rut);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al llamar al servicio web: " + ex.Message);
            }
            return RedirectToAction("PagosPendientes",new { v_rut= v_rut });
        }
        //

        public ActionResult Pago(int v_monto, string v_mat, string v_men, string v_nro)
        {
            ViewBag.Monto = v_monto;
            ViewBag.Mat = v_mat;
            ViewBag.Men = v_men;
            ViewBag.Nro = v_nro;

            List<metodoPago> met = ListarMetodoPago();
            ViewBag.MetodoPago = met;
            return View();
        }
        //
        [HttpPost]
        public ActionResult IngresarPago(string p_metodo, string v_monto, string v_mat, string v_men, string v_nro)
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                // Verificar y asignar valores por defecto si son nulos
                v_mat = v_mat ?? string.Empty;
                v_men = v_men ?? string.Empty;
                v_nro = v_nro ?? string.Empty;

                cliente.AgPago(p_metodo, v_monto, v_mat, v_men, v_nro);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al llamar al servicio web: " + ex.Message);
            }
            finally
            {
                cliente.Close();
            }
            return RedirectToAction("ConfirmacionCompra");
        }



        public ActionResult ConfirmacionCompra()
        {
            return View();
        }

        //Listar metodo pago 
        public List<metodoPago> ListarMetodoPago()
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                return cliente.ListaMetodoPago().ToList();
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
        //Listar Boleta
        public List<boleta> ListarBoleta(string p_id_matricula,string p_id_mensualidad,string p_id_compra)
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                return cliente.ListaBoleta(p_id_matricula, p_id_mensualidad, p_id_compra).ToList();
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
        //Listar productos 
        public List<carrito> ListarCarrito(int p_rut)
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                return cliente.ListadoCarrito(p_rut).ToList();
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
        //Listar Matricula 
        public List<matricula> ListarMatricula(string p_rut,string p_id_estado)
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                return cliente.ListaMatricula(p_rut, p_id_estado).ToList();
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
        //Listar Mensualidad 
        public List<mensualidad> ListarMensualidad(string p_rut,string p_id_estado)
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                return cliente.ListaMensualidad(p_rut, p_id_estado).ToList();
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
        //Listar Orden compra 
        public List<ordenCompra> ListarOrdenCompra(string p_rut, string p_id_compra)
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                return cliente.ListaOrdenCompra(p_rut, p_id_compra).ToList();
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
        //
    }
}