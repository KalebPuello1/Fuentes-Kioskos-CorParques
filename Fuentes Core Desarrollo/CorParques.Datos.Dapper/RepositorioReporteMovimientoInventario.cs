using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace CorParques.Datos.Dapper
{
    public class RepositorioReporteMovimientoInventario :RepositorioBase<ReporteMovimientoInventario>,
        IRepositorioReporteMovimientoInventario
    {
        public IEnumerable<ReporteMovimientoInventario> ObtenerReporte(ReporteMovimientoInventario modelo)
        {
            return _cnn.Query<ReporteMovimientoInventario>(
                sql: "SP_ReporteMovimientoInventario",
                param: new
                {
                    @FechaInicial = modelo.FechaInicial,
                    @FechaFinal = modelo.FechaFinal,
                    @CodSapMaterial = modelo.CodigoMaterial,
                    @idTipoMovimiento = modelo.IdTipoMovimiento,
                    //@IdPuntoOrigen = modelo.IdPuntoOrigen,
                    //@IdPuntoDestino = modelo.PuntoDestino,
                    @CodSapAlmacenOrigen = modelo.CodSapAlmacenOrigen,
                    @CodSapAlmacenDestino = modelo.CodSapAlmacenDestino,
                    @IdUsuarioResponsable = modelo.IdPersonaResponsable,
                    @UnidadMedida = modelo.UnidadMedida
                }, commandType : System.Data.CommandType.StoredProcedure
            );
        }

        public IEnumerable<ReportePedidoRestaurante> ObtenerReporteRestaurante(ReportePedidoRestaurante modelo)
        {
            
            var rta = _cnn.QueryMultiple("SP_ReportePedidosRestaurante", param: new
            {
                FechaInicial = modelo.FechaInicial,
                FechaFinal = modelo.FechaFinal,
                IdUsuarioResponsable = modelo.Id_Vendedor,
                CodSapAlmacenOrigen = modelo.CodSapAlmacenOrigen,
                IdZona = modelo.IdZona,
                IdMesa = modelo.Id_Mesa,
                IdEstadoMesa = modelo.IdEstadoMesa
            }, commandType: System.Data.CommandType.StoredProcedure);
            var attractions = rta.Read<ReportePedidoRestaurante>().ToList();
            return attractions;

        }
        public IEnumerable<ReporteBonoRegalo> ObtenerReporteBonoRegalo(ReporteBonoRegalo modelo)
        {

            var rta = _cnn.QueryMultiple("SP_ReporteBonoRegalo", param: new
            {
                FechaInicial = modelo.FechaInicialP,
                FechaFinal = modelo.FechaFinalP,
                IdTipoPedido = modelo.IdTipoPedido,
                CodCliente = modelo.CodCliente,
                CodVendedor = modelo.CodVendedor,
                CodPedido = modelo.CodPedido
            }, commandType: System.Data.CommandType.StoredProcedure);
            var attractions = rta.Read<ReporteBonoRegalo>().ToList();
            return attractions;

        }
        public IEnumerable<TipoMovimiento> ObtenerTiposMovimiento()
        {
            return _cnn.GetList<TipoMovimiento>();
        }

        public string[] ObtenerUnidadMedida()
        {
            return _cnn.Query<string>(sql: "SP_ObtenerUnidadMedida", 
                commandType: System.Data.CommandType.StoredProcedure).ToArray();
        }



    }
}
