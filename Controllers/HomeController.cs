using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using WebValdiviaDojo.WS_ValdiviaDojo;


namespace WebValdiviaDojo.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            List<comentario> ob = ListarComentario();
            ViewBag.comentario = ob;
            return View();
        }

        public ActionResult Tienda(int? v_rut)
        {
            int rut = v_rut ?? 0;

            if (rut == 0)
            {
                ViewBag.Mensaje = "Para realizar una compra debe iniciar sesión";
            }
            else
            {
                List<carrito> car = ListarCarrito(rut);
                ViewBag.Carrito = car;
            }

            List<prodGeneral> ob = ListarProdGen();
            ViewBag.ProdGen = ob;

            return View();
        }



        public ActionResult Contact()
        {
            return View();
        }


        public ActionResult Producto(string v_nom)
        {
            List<prodEspe> esp = ListarProdEspe(v_nom);

            ViewBag.ProdEsp = esp;
            return View();
        }

        [HttpPost]
        public ActionResult Producto(int p_talla, int p_rut)
        {
            WS_DojoClient cliente = new WS_DojoClient();
            try
            {
                int resultado = cliente.AgDetalleCom(p_talla, 1, p_rut);
                if (resultado == 1)
                {
                    ViewBag.Mensaje = "Producto añadido al carrito";
                    List<carrito> car = ListarCarrito(p_rut);
                    ViewBag.Carrito = car;
                }
                else
                {
                    ViewBag.Mensaje = "Ha ocurrido un error";
                }
            }
            catch (Exception)
            {
                ViewBag.Mensaje = "Ha ocurrido un error";
            }
            finally
            {
                cliente.Close();
            }
            return RedirectToAction("Tienda", new { v_rut= p_rut });
        }

        //
        public List<prodGeneral> ListarProdGen()
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                return cliente.ListadoProdGen().ToList();
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


        //lISTAR COMENTARIOS
        public List<comentario> ListarComentario()
        {
            WS_DojoClient cliente = new WS_DojoClient();

            try
            {
                return cliente.ListaComentario(1).ToList();
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
