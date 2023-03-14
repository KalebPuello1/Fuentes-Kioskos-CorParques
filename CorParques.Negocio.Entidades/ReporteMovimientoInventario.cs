using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ReporteMovimientoInventario
    {
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }
        public int IdTipoMovimiento { get; set; }
        public string CodigoMaterial { get; set; }
        
        public int IdPuntoOrigen { get; set; }
        public int PuntoDestino { get; set; }
        public int IdPersonaResponsable { get; set; }

        //--------------------------------------------
        //Reporte

        //public string FechaMovimiento { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public string TipoMovimiento { get; set; }
        
        public string CodigoSapMaterial { get; set; }
        public string NombreMaterial { get; set; }
        public string UnidadMedida { get; set; }
        //public string UnidadMedida { get; set; }
        
        public string Cantidad { get; set; }
        public string CodSapAlmacen { get; set; }
        public string CodSapAlmacenOrigen { get; set; }
        public string PuntoOrigen { get; set; }
        public string CodSapAlmacenDestino { get; set; }
        public string PuntoSalida { get; set; }
        public string Reponsable { get; set; }
        public string Costo { get; set; }
        public string CostoTotal { get; set; }
        public string CentroBeneficio { get; set; }
        
        public string Motivo { get; set; }
        public string Observaciones { get; set; }
    }
}