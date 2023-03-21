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
    public class RepositorioReporteTarjetaRecargable : RepositorioBase<ReporteDestrezas>, IRepositorioReporteTarjetaRecargable
    {
        public IEnumerable<TipoGeneral> ObtenerClientes()
        {
            return _cnn.Query<TipoGeneral>("SP_ObtenerClientesFidelizacion",
                                                   commandType: CommandType.StoredProcedure);

        }
        public IEnumerable<ReporteTarjetaRecargable> ObtenerReporte(string FechaInicial = null, string FechaFinal = null, string FechaCompra = null, string FechaVencimiento = null, string Cliente = null)
        {

            try
            {
                DateTime? fi = null;
                if (FechaInicial != null)
                    fi = DateTime.Parse(FechaInicial);
                DateTime? ff = null;
                if (FechaFinal != null)
                    ff = DateTime.Parse(FechaFinal);
                DateTime? fc = null;
                if (FechaCompra != null)
                    fc = DateTime.Parse(FechaCompra);
                DateTime? fv = null;
                if (FechaVencimiento != null)
                    fv = DateTime.Parse(FechaVencimiento);
                
                return _cnn.Query<ReporteTarjetaRecargable>("SP_ReporteTarjetaRecargable",
                                                    param: new
                                                    {
                                                        @FechaIni = fi,
                                                        @FechaFin = ff,
                                                        @FechaCompra = fc,
                                                        @FechaVencimiento = fv,
                                                        @Cliente = Cliente
                                                    },

                                                    commandType: CommandType.StoredProcedure);
                

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "RepositorioReporteTarjetaRecargable_SP_ReporteTarjetaRecargable");
                return null;
            }
        }

    }
}
