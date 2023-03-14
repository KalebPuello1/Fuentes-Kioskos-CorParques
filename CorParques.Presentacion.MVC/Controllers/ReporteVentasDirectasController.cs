//Cambioquitar: Este controlador usa el enumerador de perfiles.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CorParques.Negocio.Entidades;
using System.Threading.Tasks;
using CorParques.Presentacion.MVC.Models;
using CorParques.Transversales.Util;

namespace CorParques.Presentacion.MVC.Controllers
{
    public class ReporteVentasDirectasController : ControladorBase
    {
        
        public async Task<ActionResult> Index()
        {
            var modelo = new ReporteVentasFiltros();
            var _listPuntos = await GetAsync<IEnumerable<TipoGeneral>>("Puntos/GetAllSimple");
            //string strPerfiles = ((int)Enumerador.Perfiles.Taquilla).ToString();
            string strPerfiles = string.Empty;
            Parametro objParametro = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/IdPerfilesReporteVentasDirectas");
            strPerfiles = objParametro.Valor;

            var _listUsuarios = await GetAsync<IEnumerable<Usuario>>($"Usuario/ObtenerUsuariosPorPerfil?idsPerfil={strPerfiles}");
            var _listMediosPagos = await GetAsync<IEnumerable<TipoGeneral>>("MediosPago/GetAllSimple");
            var _listFranquicias = await GetAsync<IEnumerable<TipoGeneral>>("Franquicia/ObtenerTodos");
            IEnumerable<TipoGeneral> convenio = (await GetAsync<IEnumerable<Convenio>>("Convenio/ObtenerListaConvenios")).Select(x => new TipoGeneral { Id = x.IdConvenio, CodSAP = x.CodSapConvenio, Nombre = x.Nombre });

            ViewBag.Puntos = _listPuntos;
            ViewBag.Taquilleros = _listUsuarios.Select(x => new TipoGeneral { Id = x.Id, Nombre = string.Concat(x.Nombre, " ", x.Apellido) });
            ViewBag.MetodosPago = _listMediosPagos;
            ViewBag.Franquicias = _listFranquicias;
            ViewBag.Convenios = convenio;
            ViewBag.Filtros = null;
            return View(modelo);
        }
        [HttpGet]
        public async Task<string> GenerarArchivo(ReporteVentasFiltros  modelo)
        {
            string strRetorno = string.Empty;
            Reportes objReportes = new Reportes();
            try
            {
                var VentasDirectas =
                        await GetAsync<IEnumerable<ReporteVentas>[]>($"ReporteVentas/ObtenerReporte/{modelo.FechaInicial.ToString("yyyy-MM-dd")}/{modelo.FechaFinal.ToString("yyyy-MM-dd")}/{(modelo.IdPunto == null ? "null" : modelo.IdPunto.ToString())}/{(modelo.IdTaquillero == null ? "null" : modelo.IdTaquillero.ToString())}/{ (modelo.IdFormaPago == null ? "null" : modelo.IdFormaPago.ToString())}/{ (modelo.IdFranquicia == null ? "null" : modelo.IdFranquicia.ToString())}/{ (string.IsNullOrEmpty(modelo.CentroBeneficio) ? "null" : modelo.CentroBeneficio)}");

                 if (VentasDirectas != null)
                {
                    if (VentasDirectas.Count() > 0)
                        strRetorno = objReportes.GenerarReporteVentasDirectas(VentasDirectas);
                }
            }
            catch (Exception ex)
            {
                strRetorno = string.Concat("Error generando reporte: ", ex.Message);
            }

            if (strRetorno.Contains("No hay datos"))
                strRetorno = "";
            return strRetorno;
        }

        //public async Task<ActionResult> ObtenerDatosReporte(ReporteVentasFiltros modelo)
        //{
        //    var _listPuntos = await GetAsync<IEnumerable<TipoGeneral>>("Puntos/GetAllSimple");

        //    //string strPerfiles = ((int)Enumerador.Perfiles.Taquilla).ToString();
        //    string strPerfiles = string.Empty;
        //    Parametro objParametro = await GetAsync<Parametro>($"Parameters/ObtenerParametroPorNombre/IdPerfilesReporteVentasDirectas");
        //    strPerfiles = objParametro.Valor;

        //    var _listUsuarios = await GetAsync<IEnumerable<Usuario>>($"Usuario/ObtenerUsuariosPorPerfil?idsPerfil={strPerfiles}");
        //    var _listMediosPagos = await GetAsync<IEnumerable<TipoGeneral>>("MediosPago/GetAllSimple");
        //    var _listFranquicias = await GetAsync<IEnumerable<TipoGeneral>>("Franquicia/ObtenerTodos");
        //    IEnumerable<TipoGeneral> CentroBeneficio = await GetAsync<IEnumerable<TipoGeneral>>("TipoProducto/ObtenerListaTipoProduto");
        //    ViewBag.centroB = CentroBeneficio;

        //    ViewBag.Puntos = _listPuntos;
        //    ViewBag.Taquilleros = _listUsuarios.Select(x => new TipoGeneral { Id = x.Id, Nombre = string.Concat(x.Nombre, " ", x.Apellido) });
        //    ViewBag.MetodosPago = _listMediosPagos;
        //    ViewBag.Franquicias = _listFranquicias;
        //    ViewBag.CentroBeneficio = CentroBeneficio;
        //    modelo.FechaInicial = modelo.FechaInicial == null ? "0" : modelo.FechaInicial.ToString().Replace(@"/", "");
        //    modelo.FechaFinal = modelo.FechaFinal == null ? "0" : modelo.FechaFinal.ToString().Replace(@"/", "");
        //    modelo.IdFormaPago = modelo.IdFormaPago == null ? 0 : modelo.IdFormaPago;
        //    modelo.IdPunto = modelo.IdPunto == null ? 0 : modelo.IdPunto;
        //    modelo.IdTaquillero = modelo.IdTaquillero == null ? 0 : modelo.IdTaquillero;
        //    modelo.IdFranquicia = modelo.IdFranquicia == null ? 0 : modelo.IdFranquicia;
        //    modelo.CentroBeneficio = modelo.CentroBeneficio == null ? "null" : modelo.CentroBeneficio;
        //    try
        //    {
        //        var objReporteVentas = await GetAsync<IEnumerable<ReporteVentas>[]>($"ReporteVentas/ObtenerReporte/{modelo.FechaInicial}/{modelo.FechaFinal}/{modelo.IdPunto}/{modelo.IdTaquillero}/{modelo.IdFormaPago}/{modelo.IdFranquicia}/{modelo.CentroBeneficio}");
        //        //var objReporte = await GetAsync<IEnumerable<ReporteVentas>>($"ReporteVentas/ObtenerReporte/{modelo.FechaInicial}/{modelo.FechaFinal}/{modelo.IdPunto}/{modelo.IdTaquillero}/{modelo.IdFormaPago}/{modelo.IdFranquicia}");

        //        var objReporte = new List<ReporteVentas>();

        //        foreach (var item in objReporteVentas[0])
        //        {
        //            var reporte = new ReporteVentas();
        //            reporte.Anulaciones = item.Anulaciones;
        //            reporte.Cantidad = item.Cantidad;
        //            reporte.Consecutivo = item.Consecutivo;
        //            reporte.Fecha = item.Fecha;
        //            reporte.Franquicia = item.Franquicia;
        //            reporte.IdFranquicia = item.IdFranquicia;
        //            reporte.IdMedioPago = item.IdMedioPago;
        //            reporte.IdProducto = item.IdProducto;
        //            reporte.IdPunto = item.IdPunto;
        //            reporte.IdTaquillero = item.IdTaquillero;
        //            reporte.Impuesto = item.Impuesto;
        //            reporte.MedioPago = item.MedioPago;
        //            reporte.NotaCredito = item.NotaCredito;
        //            reporte.NumAprobacion = item.NumAprobacion;
        //            reporte.NumNotaCredito = item.NumNotaCredito;
        //            reporte.Producto = item.Producto;
        //            reporte.Taquilla = item.Taquilla;
        //            reporte.Taquillero = item.Taquillero;
        //            reporte.TotalRecibido = item.TotalRecibido;
        //            reporte.ValorSinImpuesto = item.ValorSinImpuesto;
        //            objReporte.Add(reporte);
        //        }
        //        modelo.objReporteVentas = objReporte;
        //        return await Index(modelo);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
            
        //}

        //[HttpPost]
        //public async Task<ActionResult> Index(ReporteVentasFiltros modelo)
        //{
        //    ViewBag.Filtros = modelo.FechaInicial + "|" + modelo.FechaFinal + "|" + modelo.IdPunto + "|" + modelo.IdTaquillero + "|" + modelo.IdFormaPago + "|" + modelo.IdFranquicia + "|" + modelo.CentroBeneficio;
        //    return View("Index", modelo);
        //}

        //public async Task<string> ExportarExcel(string Filtros)
        //{
        //    ReporteVentasFiltros modelo = new ReporteVentasFiltros();
        //    string strRetorno = string.Empty;
        //    Reportes objReportes = new Reportes();
        //    try
        //    {
        //        var filtros = Filtros.Split('|');
        //        if (filtros.Count() > 0)
        //        {
        //            modelo.FechaInicial = filtros[0];
        //            modelo.FechaFinal = filtros[1];
        //            modelo.IdPunto = int.Parse(filtros[2]);
        //            modelo.IdTaquillero = int.Parse(filtros[3]);
        //            modelo.IdFormaPago = int.Parse(filtros[4]);
        //            modelo.IdFranquicia = int.Parse(filtros[5]);
        //            modelo.CentroBeneficio = filtros[6];
        //        }
        //        //var objReporteProductos = await GetAsync<IEnumerable<ReporteVentas>>($"ReporteVentas/ObtenerReporte/{modelo.FechaInicial}/{modelo.FechaFinal}/{modelo.IdPunto}/{modelo.IdTaquillero}/{modelo.IdFormaPago}/{modelo.IdFranquicia}");
        //        //modelo.objReporteVentas = objReporteProductos;

        //        var objReporteVentas = await GetAsync<IEnumerable<ReporteVentas>[]>($"ReporteVentas/ObtenerReporte/{modelo.FechaInicial}/{modelo.FechaFinal}/{modelo.IdPunto}/{modelo.IdTaquillero}/{modelo.IdFormaPago}/{modelo.IdFranquicia}/{modelo.CentroBeneficio}");

        //        //IEnumerable<ReporteVentas>[] listas = new IEnumerable<ReporteVentas>[2];
        //        //listas[0] = objReporteProductos;

        //        strRetorno = objReportes.GenerarReporteVentasDirectas(objReporteVentas);                
        //    }
        //    catch (Exception ex)
        //    {
        //        strRetorno = string.Concat("Error generando reporte ventas directas: ", ex.Message);
        //    }
        //    return strRetorno;
        //}

        [HttpGet]
        public virtual FileContentResult Download(string Data)
        {
            FileContentResult objFileContentResult = null;
            try
            {
                objFileContentResult = File(Transversales.Util.Utilidades.ObtenerBytesArchivo(Data), System.Net.Mime.MediaTypeNames.Application.Octet, Data);
            }
            catch (Exception)
            {
                return null;
            }
            return objFileContentResult;
        }
    }
}