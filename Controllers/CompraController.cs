using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebValdiviaDojo.WS_ValdiviaDojo;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using System.IO;
using QuestPDF.Infrastructure;

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


        

        public ActionResult Boleta(string v_rut, string p_id_matricula, string p_id_mensualidad, string p_id_compra, string p_producto)
        {
            List<boleta> bol = ListarBoleta(p_id_matricula, p_id_mensualidad, p_id_compra);

            ViewBag.v_rut = v_rut;
            ViewBag.prod = p_producto;
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



        public ActionResult ConfirmacionCompra(string v_rut)
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
        //PDF

        public ActionResult DescargarPDF(string p_nombre, string direccion, string p_nboleta, string p_rut, string p_total,string p_producto)
        {
            try
            {
                // Configura la licencia de QuestPDF
                QuestPDF.Settings.License = LicenseType.Community;

                var data = Document.Create(document =>
                {
                    document.Page(page =>
                    {
                        page.Margin(30);

                        page.Header().ShowOnce().Row(row =>
                        {
                            string imagePath = Server.MapPath("~/Img/Publicidad/logoSinFondo.png");
                            byte[] imageData = System.IO.File.ReadAllBytes(imagePath);

                            row.ConstantItem(150).Image(imageData);


                            row.RelativeItem().Column(col =>
                            {
                                col.Item().AlignCenter().Text("Valdivia Dojo").Bold().FontSize(14);
                                col.Item().AlignCenter().Text("Los Toros 1932, Puente Alto, Región Metropolitana, Chile.").FontSize(9);
                                col.Item().AlignCenter().Text("+56 9 9931 5803").FontSize(9);

                            });

                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Border(1).BorderColor("#257272")
                                .AlignCenter().Text("RUT Empresa ---");

                                col.Item().Background("#257272").Border(1)
                                .BorderColor("#257272").AlignCenter()
                                .Text("Boleta de venta").FontColor("#fff");

                                col.Item().Border(1).BorderColor("#257272").
                                AlignCenter().Text(p_nboleta);


                            });
                        });

                        page.Content().PaddingVertical(10).Column(col1 =>
                        {
                            col1.Item().Column(col2 =>
                            {
                                col2.Item().Text("Datos del cliente").Underline().Bold();

                                col2.Item().Text(txt =>
                                {
                                    txt.Span("Nombre: ").SemiBold().FontSize(10);
                                    txt.Span(p_nombre).FontSize(10);
                                });

                                col2.Item().Text(txt =>
                                {
                                    txt.Span("RUT: ").SemiBold().FontSize(10);
                                    txt.Span(p_rut).FontSize(10);
                                });

                                col2.Item().Text(txt =>
                                {
                                    txt.Span("Direccion: ").SemiBold().FontSize(10);
                                    txt.Span(direccion).FontSize(10);
                                });
                            });

                            col1.Item().LineHorizontal(0.5f);

                            col1.Item().Table(tabla =>
                            {
                                tabla.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();

                                });
                                tabla.Header(header =>
                                {
                                    header.Cell().Background("#257272")
                                    .Padding(2).Text("Producto").FontColor("#fff");
                                });
                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                             .Padding(2).Text(p_producto.ToString()).FontSize(10);
                            });

                            col1.Item().AlignRight().Text("Total: " + string.Format("${0:N0}", p_total)).FontSize(12);

                            if (1 == 1)
                                col1.Item().Background(Colors.Grey.Lighten3).Padding(10)
                                .Column(column =>
                                {
                                    column.Item().Text("Comentarios").FontSize(14);
                                    column.Item().Text("La presente factura ejemplar se proporciona a título ilustrativo y no guarda relación alguna con ninguna transacción real. Se ruega no hacer uso indebido de la misma y respetar su carácter meramente ejemplar, evitando cualquier aplicación con propósitos inapropiados.");
                                    column.Spacing(5);
                                });

                            col1.Spacing(10);
                        });


                        page.Footer()
                        .AlignRight()
                        .Text(txt =>
                        {
                            txt.Span("Pagina ").FontSize(10);
                            txt.CurrentPageNumber().FontSize(10);
                            txt.Span(" de ").FontSize(10);
                            txt.TotalPages().FontSize(10);
                        });
                    });
                }).GeneratePdf();

                Stream stream = new MemoryStream(data);
                return File(stream, "application/pdf", "detalleventa.pdf");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
