using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

	[Table("tb_clientesfidelizacion")]
	public class ClienteFideliacion
	{

	#region Propiedades

	    [Key]
	    public string Documento { get; set; }
	    public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int? Genero { get; set; }
        public byte[] Foto { get; set; }
        [NotMapped]
        public string FotoTexto { get; set; }

        [NotMapped]
        public string Consecutivo { get; set; }
        [NotMapped]
        public int? Recarga { get; set; }
        [NotMapped]
        public string Fecha { get; set; }
        [NotMapped]
        public int Puntos { get; set; }

        #endregion


    }
}
