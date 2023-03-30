using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ReporteCortesias
    {
        public int Id { get; set; }
        public string FechaCreacion { get; set; }
        public int Cantidad { get; set; }
        public int Activo { get; set; }
        public string NumeroTicket { get; set; }
        public string Descripcion { get; set; }
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }
        public string Observacion { get; set; }
        public int IdRedencion { get; set; }
        public string NumTarjetaFAN { get; set; }
        public string CodSapProducto { get; set; }
        public string Nombre_Producto { get; set; }
        public string Consecutivo { get; set; }
        public string TipoCortesia { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string FechaEntrega { get; set; }
        public string CodSapEntregado { get; set; }
        public string Producto_Entregado { get; set; }        
    }
}
