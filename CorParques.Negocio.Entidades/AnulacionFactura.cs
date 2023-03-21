using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class AnulacionFactura
    {
        public int IdFactura { get; set; }
        public string CodigoFactura { get; set; }
        public string Observacion { get; set; }
        public DateTime Fecha { get; set; }
        public int IdPunto { get; set; }
        public string NombrePunto { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public string UsuarioCreacion { get; set; }

        public int IdUsuarioModificacion { get; set; }
        public DateTime FechaModificacion { get; set; }

        public int IdEstado { get; set; }
        public string Estado { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal Valor { get; set; }
    }
}
