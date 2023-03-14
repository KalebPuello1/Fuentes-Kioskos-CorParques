using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using CorParques.Transversales.Util;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Dapper
{
    public class RepositorioReporteDestreza : RepositorioBase<ReporteDestrezas>, IRepositorioReporteDestreza
    {
        public IEnumerable<ReporteDestrezas> ObtenerReporte(string FechaInicial, string FechaFinal, string CodigoPunto = null, string TipoBoleta=null, string Cliente=null , bool? tipoVenta=null)
        {

            try
            {


                //= new List<ReporteDestrezas>();
                IEnumerable<ReporteDestrezas> lista = _cnn.Query<ReporteDestrezas>("SP_ReporteDestrezas",
                                                    param: new
                                                    {
                                                        FechaDesde = DateTime.Parse(FechaInicial),
                                                        FechaHasta = DateTime.Parse(FechaFinal),
                                                        IdPunto = CodigoPunto,
                                                        TipoVenta= tipoVenta,                                                        
                                                        ProductoTipoBoleta = TipoBoleta,
                                                        Cliente = Cliente
                                                    },
                                                    commandType: CommandType.StoredProcedure);//.FirstOrDefault();
                return lista;
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "RepositorioReporteDestreza_ObtenerReporte");
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// RDSH: Se pusieron dos hojas para este reporte una para la información general y otra para poder sacar los premios y sus cantidades.
        /// </summary>
        /// <param name="FechaInicial"></param>
        /// <param name="FechaFinal"></param>
        /// <param name="CodigoPunto"></param>
        /// <param name="CodigoSeries"></param>
        /// <param name="TipoBoleta"></param>
        /// <param name="Cliente"></param>
        /// <param name="tipoVenta"></param>
        /// <returns></returns>
        public IEnumerable<ReporteDestrezas>[] ObtenerReporteNuevo(string FechaInicial, string FechaFinal, string CodigoPunto = null, string CodigoSeries = null, string TipoBoleta = null, string Cliente = null, bool? tipoVenta = null)
        {

            try
            {
                                
                var lista = _cnn.QueryMultiple("SP_ReporteDestrezas",
                                                    param: new
                                                    {
                                                        FechaDesde = DateTime.Parse(FechaInicial),
                                                        FechaHasta = DateTime.Parse(FechaFinal),
                                                        IdPunto = CodigoPunto,
                                                        TipoVenta = tipoVenta,
                                                        IdProductoSerie = CodigoSeries,
                                                        ProductoTipoBoleta = TipoBoleta,
                                                        Cliente = Cliente
                                                    },
                                                    commandType: CommandType.StoredProcedure);

                IEnumerable<ReporteDestrezas>[] objListaConsultas = new IEnumerable<ReporteDestrezas>[2];
                objListaConsultas[0] = lista.Read<ReporteDestrezas>().ToList();
                objListaConsultas[1] = lista.Read<ReporteDestrezas>().ToList();

                return objListaConsultas;

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "RepositorioReporteDestreza_ObtenerReporteNuevo");
                return null;
            }
        }

    }
}
