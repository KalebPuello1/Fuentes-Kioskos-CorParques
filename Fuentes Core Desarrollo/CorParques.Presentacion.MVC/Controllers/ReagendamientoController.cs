using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CorParques.Transversales.Util;

namespace CorParques.Presentacion.MVC.Controllers
{
    public class ReagendamientoController : ControladorBase
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, Route("api/ObtenerProducto/{consecutivo}")]
        public async Task<ActionResult> ObtenerProducto(string consecutivo)
        {
            var Consecutivo = consecutivo;

            ReportePDF rePDF = new ReportePDF();
            rePDF.repositorio(Consecutivo);

            Boleteria dato = await GetAsync<Boleteria>($"Reagendamiento/ObtenerDetalleeReagendamientoFecha/{consecutivo}");
            rePDF.repositorio(dato.NombreProducto);

            var datoo = "";
            if (dato != null)
            {
                if (dato.NombreProducto.Contains("No existe"))
                {
                    datoo = "No existe";
                }
                else
                {
                    datoo = "existe";
                }
            }
            Boleteria producto = new Boleteria();
            producto = await GetAsync<Boleteria>($"Reagendamiento/productoReagendar/{Consecutivo}");


            if (producto != null)
            {


                if (producto.IdEstado == 2)
                {

                    return Json(new { existe = "El pasaporte se encuentra inactivo", error = "error" });
                }
                else if (producto.IdEstado == 3)
                {
                    return Json(new { existe = "El pasaporte se encuentra anulado", error = "error" });
                }
                else if (producto.IdEstado == 8)
                {
                    return Json(new { existe = "El pasaporte se encuentra entregado", error = "error" });
                }
                else if (producto.IdEstado == 16)
                {
                    return Json(new { existe = "El pasaporte se encuentra bloqueado", error = "error" });
                }
                else
                {

                    var prod = await GetAsync<Producto>($"Pos/ObtenerProducto/{producto.IdProducto}");
                    if (prod != null)
                    {

                        DateTime MyDateTime = producto.FechaUsoInicial;
                        int year = MyDateTime.Year;
                        int month = MyDateTime.Month;
                        int day = MyDateTime.Day;

                        var FechaEditada = string.Format("{0}/{1}/{2}", day, month, year);


                        producto.NombreProducto = prod.Nombre;
                        producto.AtraccionesDisponibles = prod.AtraccionesDisponibles;
                        prod.ReagendamientoPasaportes = producto.ReagendamientoPasaportes;
                        prod.UsosPasaportes = producto.UsosPasaportes;
                        producto.FechaEditada = FechaEditada;

                        if (prod.ReagendamientoPasaportes >= 4)
                        {
                            return Json(new { existe = "¡ Este consecutivo ya fue reagendado 3 veces o más !" });
                        }

                        if (prod.UsosPasaportes >= 4)
                        {
                            return Json(new { existe = "¡ Este consecutivo ya tiene un total de 4 usos !" });
                        }
                        else
                        {
                            return PartialView("_check", producto);
                        }
                    }
                    else
                    {
                        return Json(new { existe = "Este producto no es valido" });
                    }
                }
            }
            else
            {
                return Json(new { existe = "Su operacion falló - no se logró identificar el consecutivo", error = "error" });
            }

        }

        [HttpPost, Route("api/ObtenerFacturaReagendamiento/{CodBarra}")]
        public async Task<ActionResult> ObtenerFacturaReagendamiento(string CodBarra)
        {
            List<ObtenerFacturaReagendamiento> ObjetoListaMadre = new List<ObtenerFacturaReagendamiento>();
            List<string> L = new List<string>();

            var rta = await GetAsync<Reagendamiento>($"Reagendamiento/ObtenerFacturaReagendamiento/{CodBarra}");

            if (rta == null)
            {

                return Json(new { existe = "Su operacion falló - no se logró identificar el código de factura ", error = "error" });

            }
            else
            {

                var ListaBoleteria = rta.Boleteria;
                List<DetalleFactura> ListaDetalleFactura = (List<DetalleFactura>)rta.DetalleFactura;

                int ListaD = ListaDetalleFactura.Count();
                int a = 0;

                if (ListaBoleteria.Count() == 0 && ListaDetalleFactura.Count() == 0 || ListaBoleteria.Count() > 0 && ListaDetalleFactura.Count() == 0)
                {
                    return Json(new { existe = "Su operacion falló - no se logró identificar el código de factura", error = "error" });

                }
                else
                {

                    foreach (var item in ListaDetalleFactura)
                    {
                        ObtenerFacturaReagendamiento objq = new ObtenerFacturaReagendamiento();

                        var C = (Boleteria)ListaBoleteria.Where(x => x.IdBoleteria == item.IdDetalleProducto).FirstOrDefault();

                        objq.Nombre = item.Nombre;
                        objq.CantidadPasaportes = item.Cantidad;
                        objq.FechaCreacion = item.FechaCreacion;


                        DateTime MyDateTime = objq.FechaCreacion;
                        int year = MyDateTime.Year;
                        int month = MyDateTime.Month;
                        int day = MyDateTime.Day;

                        var FechaEditada = string.Format("{0}/{1}/{2}", day, month, year);

                        objq.FechaEditada = FechaEditada;


                        if (C != null)
                        {
                            objq.Consecutivo = C.Consecutivo;
                            var InfBoleteria = await GetAsync<Boleteria>($"Reagendamiento/productoReagendar/{objq.Consecutivo}");
                            objq.IdProducto = C.IdProducto;
                            var prod = await GetAsync<Producto>($"Pos/ObtenerProducto/{objq.IdProducto}");

                            objq.UsosPasaportes = InfBoleteria.UsosPasaportes;
                            //objq.AtraccionesDisponibles = prod.objListaRecetaProducto.Count();
                            objq.AtraccionesDisponibles = prod.AtraccionesDisponibles;
                            objq.ReagendamientoPasaportes = InfBoleteria.ReagendamientoPasaportes;

                            objq.Id_Estado = InfBoleteria.IdEstado;
                            objq.FechaUsoInicial = C.FechaUsoInicial;
                            objq.FechaUsoFinal = C.FechaUsoFinal;
                            objq.FechaInicioEvento = C.FechaInicioEvento;
                            objq.FechaFinEvento = C.FechaFinEvento;
                            objq.CodigoSap = prod.CodigoSap;


                        }
                        else
                        {
                            objq.Nombre = item.Nombre;
                            objq.CantidadPasaportes = item.Cantidad;
                            objq.CodigoSap = item.CodigoSap;
                            objq.ProductoId = item.Id_Producto;

                        }

                        ObjetoListaMadre.Insert(a, objq);
                        a++;
                    }

                    return PartialView("_checkfactura", ObjetoListaMadre.Count() == 0 ? null : ObjetoListaMadre);
                }
            }
        }

        [HttpPost, Route("api/ModificarFecha/{pasaporteR}/{fechaactualR}/{fechanuevaR}")]
        public async Task<JsonResult> ModificarFecha(string pasaportesR, string fechaactualR, string fechanuevaR)
        {
            var fechaInicial = fechanuevaR + " 00:00:00.000";
            var fechaFinal = fechanuevaR + " 23:59:00.000";
            var fechaInicialAnterior = fechaactualR;
            var fechaFinalAnterior = fechaactualR;
            int idUsuarioLogueado = IdUsuarioLogueado;
            string Consecutivo = pasaportesR;

            Reagendamiento reagendamiento = new Reagendamiento();
            reagendamiento.fechaInicial = fechaInicial;
            reagendamiento.fechaFinal = fechaFinal;
            reagendamiento.fechaInicialAnterior = fechaInicialAnterior;
            reagendamiento.fechaFinalAnterior = fechaFinalAnterior;
            reagendamiento.Consecutivo = Consecutivo;
            reagendamiento.idUsuarioLogueado = idUsuarioLogueado;

            CambioFechaBoleta producto = new CambioFechaBoleta();
            producto.Consecutivo = pasaportesR;
            producto.FechaUsoInicial = fechanuevaR;
            producto.FechaUsoFinal = fechanuevaR;

            Boleteria Boleteria = new Boleteria();

            var datoo = await PostAsync<CambioFechaBoleta, string>("Reagendamiento/ModificarFecha", producto);
            Boleteria productoBoleteria = new Boleteria();
            productoBoleteria = await GetAsync<Boleteria>($"Reagendamiento/productoReagendar/{pasaportesR}");
            Producto prod = await GetAsync<Producto>($"Pos/ObtenerProducto/{productoBoleteria.IdProducto}");
            productoBoleteria.NombreProducto = prod.Nombre;
            productoBoleteria.FechaUsoInicial.ToString();
            productoBoleteria.FechaUsoFinal.ToString();

            DateTime MyDateTime = productoBoleteria.FechaUsoInicial;
            int year = MyDateTime.Year;
            int month = MyDateTime.Month;
            int day = MyDateTime.Day;

            var FechaEditada = string.Format("{0}/{1}/{2}", day, month, year);

            productoBoleteria.FechaEditada = FechaEditada;

            if (!datoo.Elemento.ToString().Contains("fallo") || productoBoleteria.NombreProducto != null)
            {
                var dato = await PostAsync<Reagendamiento, string>("Reagendamiento/InsertarDetalleReagendamientoFecha", reagendamiento);
                return Json(new { dato = "Su operación fue satisfactoria", success = "success", ProductoModificado = productoBoleteria, fi = productoBoleteria.FechaEditada, fa = productoBoleteria.FechaUsoFinal.ToString() });
            }
            else
            {
                return Json(new { dato = "Su operación falló - no se logró modificar las fechas correctamente", error = "error" });
            }
        }
    }
}