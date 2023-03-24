using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ReporteFANVendidas
    {
        public int IdBoleteria { get; set; }
        public int IdProducto { get; set; }
        public string Consecutivo { get; set; }
        public int IdSolicitudBoleteria { get; set; }
        public int IdEstado { get; set; }
        public int Valor { get; set; }
        public string CodigoSapConvenio { get; set; }
        public string CodigoVenta { get; set; }
        public DateTime FechaImpresion { get; set; }
        public DateTime FechaActivacion { get; set; }
        public string Documento { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int Genero { get; set; }
        public string Direccion { get; set; }

        
    }
}
