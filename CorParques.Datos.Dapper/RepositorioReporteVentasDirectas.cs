using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using System.Data;
using CorParques.Transversales.Util;

namespace CorParques.Datos.Dapper
{
    public class RepositorioReporteVentasDirectas : RepositorioBase<ReporteVentas>, IRepositorioReporteVentasDirectas
    {

        #region Declaraciones

        private readonly SqlConnection _cnn;

        #endregion

        #region Constructor

        public RepositorioReporteVentasDirectas()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }
        #endregion

        public IEnumerable<ReporteVentas>[] ObtenerReporte(string FechaInicial, string FechaFinal, int? IdPunto, int? IdTaquillero, int? IdFormaPago, int? IdFranquicia, string Convenio)
        {
            try
            {

                //var objReporteVentas = _cnn.Query<ReporteVentas>("SP_ReporteVentasDirectas", param: new
                //{
                //    FechaInicial = FechaInicial,
                //    FechaFinal = FechaFinal,
                //    IdTaquillero = IdTaquillero,
                //    IdPunto = IdPunto,
                //    IdFormaPago = IdFormaPago,
                //    IdFranquicia = IdFranquicia
                //}, commandType: System.Data.CommandType.StoredProcedure).ToList();
                var rta = _cnn.QueryMultiple("SP_ReporteVentasDirectas", param: new
                {
                    FechaInicial = FechaInicial,
                    FechaFinal = FechaFinal,
                    IdTaquillero = IdTaquillero,
                    IdPunto = IdPunto,
                    IdFormaPago = IdFormaPago,
                    IdFranquicia = IdFranquicia,
                    Convenio = Convenio
                }, commandType: CommandType.StoredProcedure);
                IEnumerable<ReporteVentas>[] listaVentas = new IEnumerable<ReporteVentas>[2];
                listaVentas[0] =  rta.Read<ReporteVentas>().ToList();
                listaVentas[1] = rta.Read<ReporteVentas>().ToList();

                //IEnumerable<ReporteVentas> objReporteVentas = new List<ReporteVentas>();
                
                //CommandDefinition cmd = new CommandDefinition("SP_ReporteVentasDirectas", , null, null, CommandType.StoredProcedure);

                ////var reader = _cnn.ExecuteReader(cmd);
                ////while (reader.Read())
                ////{    
                ////    rpt = new ReporteVentas();
                ////    rpt.Fecha = reader.GetDateTime(0);
                ////    rpt.Consecutivo = reader.GetString(1);
                ////    rpt.IdPunto = reader.GetInt32(2);
                ////    rpt.Taquilla = reader.GetString(3);
                ////    rpt.IdProducto = reader.GetInt32(4);
                ////    rpt.Producto = reader.GetString(5);
                ////    rpt.Cantidad = reader.GetInt32(6);
                ////    rpt.ValorSinImpuesto = reader.GetDouble(7);
                ////    rpt.Impuesto = Convert.ToDouble(reader.GetDecimal(8));
                ////    rpt.TotalRecibido = reader.GetDouble(9);
                ////    rpt.IdTaquillero = reader.GetInt32(10);
                ////    rpt.Taquillero = reader.GetString(11);
                ////    rpt.NumNotaCredito = reader.IsDBNull(12) ? "" : reader.GetInt32(12).ToString();
                ////    rpt.Anulaciones = reader.IsDBNull(13) ? "0" : reader.GetString(13);
                ////    rpt.NotaCredito = reader.IsDBNull(14) ? "0" : reader.GetString(14);
                ////    rpt.NotaCredito = reader.IsDBNull(14) ? "0" : reader.GetString(15);
                ////    rptVentas.Add(rpt);
                ////    objReporteVentas = rptVentas;
                ////}

                //reader.NextResult();
                //objReporteVentas = new List<ReporteVentas>();
                //rptVentas = new List<ReporteVentas>();

                

                //while (reader.Read())
                //{
                //    rpt = new ReporteVentas();
                //    rpt.Consecutivo = reader.GetString(0);
                //    rpt.Fecha = reader.GetDateTime(1);
                //    rpt.IdPunto = reader.GetInt32(2);
                //    rpt.Taquilla = reader.GetString(3);
                //    rpt.IdMedioPago = reader.GetInt32(4);
                //    rpt.MedioPago = reader.GetString(5);
                //    rpt.IdFranquicia = reader.IsDBNull(6) ? 0 : reader.GetInt32(6);
                //    rpt.Franquicia = reader.IsDBNull(7) ? "" : reader.GetString(7);
                //    rpt.NumAprobacion = reader.IsDBNull(8) ? "" : reader.GetString(8);
                //    rpt.IdTaquillero = reader.GetInt32(9);
                //    rpt.Taquillero = reader.GetString(10);
                //    rpt.NumNotaCredito = reader.IsDBNull(11) ? "" : reader.GetInt32(11).ToString();
                //    rpt.Propina = reader.IsDBNull(12) ? "0" : reader.GetString(12);
                //    rpt.TipoCliente = reader.IsDBNull(13) ? "0" : reader.GetString(13);
                //    rpt.idCliente = reader.IsDBNull(14) ? "0" : reader.GetString(14);
                //    rpt.Nombre = reader.IsDBNull(15) ? "0" : reader.GetString(15);
                //    rpt.NotaCredito = reader.IsDBNull(16) ? "0" : reader.GetString(16);
                //    rpt.TotalRecibido = reader.GetDouble(17);
                //    //Consecutivo	Fecha	IdPunto	Taquilla	IdMedioPago	MedioPago	IdFranquicia	Franquicia	NumAprobacion	IdTaquillero	Taquillero	NumNotaCredito	Propina	TipoCliente	idCliente	Nombre	NotaCredito
                //    rptVentas.Add(rpt);
                //    objReporteVentas = rptVentas;
                //}
                //listaVentas[1] = objReporteVentas;
                return listaVentas;
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "RepositorioReporteVentasDirectas.ObtenerReporte");
                throw new ArgumentException(string.Concat("Se presentó un error al realizar la consulta: ", ex.Message));
            }
        }
    }

}
