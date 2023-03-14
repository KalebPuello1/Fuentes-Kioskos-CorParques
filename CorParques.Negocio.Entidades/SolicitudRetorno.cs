using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    
	public class SolicitudRetorno
    {
        public int? Id { get; set; }
        public DateTime? FechaSolicitud { get; set; }
        public string UsuarioCrea { get; set; }
        public string FechaUso { get; set; }
        public string CodSapPedido { get; set; }
        public string Asesor { get; set; }
        public string Cliente { get; set; }
        public string Tipo { get; set; }
        public string Producto { get; set; }
        public string CodigoSap { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
        public string Estado { get; set; }
        public string Entrega { get; set; }
        public string Recibe { get; set; }
        public DateTime FechaRecibe { get; set; }
        public string Motivo { get; set; }
        public string Observacion { get; set; }



    }
}
