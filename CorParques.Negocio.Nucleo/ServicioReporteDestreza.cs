using CorParques.Datos.Contratos;
using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Nucleo
{
    public class ServicioReporteDestreza : IServicioReporteDestreza
    {
        private readonly IRepositorioReporteDestreza _repositorio;

        public ServicioReporteDestreza(IRepositorioReporteDestreza repositorio)
        {

            _repositorio = repositorio;
        }


        public IEnumerable<ReporteDestrezas> ObtenerReporte(string FechaInicial = null, string FechaFinal = null, string CodigoPunto = null, string NombreTipoBoleta = null, string NombreCliente = null, bool? tipoVenta=null)
        {
            if (CodigoPunto == "")
                CodigoPunto = null;       
            if (NombreTipoBoleta == "")
                NombreTipoBoleta = null;
            if (NombreCliente == "")
                NombreCliente = null;

            var lista = _repositorio.ObtenerReporte(FechaInicial, FechaFinal, CodigoPunto, NombreTipoBoleta, NombreCliente, tipoVenta);
            return lista;
        }

        /// <summary>
        /// RDSH: Se pusieron dos hojas para este reporte una para la información general y otra para poder sacar los premios y sus cantidades.
        /// </summary>
        /// <param name="FechaInicial"></param>
        /// <param name="FechaFinal"></param>
        /// <param name="CodigoPunto"></param>
        /// <param name="CodigoSeries"></param>
        /// <param name="NombreTipoBoleta"></param>
        /// <param name="NombreCliente"></param>
        /// <param name="tipoVenta"></param>
        /// <returns></returns>
        public IEnumerable<ReporteDestrezas>[] ObtenerReporteNuevo(string FechaInicial, string FechaFinal, string CodigoPunto = null, string CodigoSeries = null, string NombreTipoBoleta = null, string NombreCliente = null, bool? tipoVenta = default(bool?))
        {
            if (CodigoPunto == "")
                CodigoPunto = null;
            if (CodigoSeries == "")
                CodigoSeries = null;
            if (NombreTipoBoleta == "")
                NombreTipoBoleta = null;
            if (NombreCliente == "")
                NombreCliente = null;

            var objResultado = _repositorio.ObtenerReporteNuevo(FechaInicial, FechaFinal, CodigoPunto, CodigoSeries, NombreTipoBoleta, NombreCliente, tipoVenta);
            return objResultado;
        }
    }
}
