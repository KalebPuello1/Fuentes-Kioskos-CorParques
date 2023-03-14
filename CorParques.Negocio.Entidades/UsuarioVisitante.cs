using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_UsuarioVisitante")]
    public class UsuarioVisitante
    {

        #region Propiedades

        [Key, Column("Id")]
        public int Id { get; set; }

        [Column("Nombres")]
        public string Nombres { get; set; }

        [Column("Apellidos")]
        public string Apellidos { get; set; }

        [Column("TipoDocumento")]
        public string TipoDocumento { get; set; }

        [Column("NumeroDocumento")]
        public string NumeroDocumento { get; set; }


        [Column("Correo")]
        public string Correo { get; set; }

        [Column("Telefono")]
        public string Telefono { get; set; }

        [Column("IdUsuarioCreacion")]
        public int IdUsuarioCreacion { get; set; }

        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; }

        [Column("FechaActualizacion")]
        public DateTime FechaActualizacion { get; set; }

        [Column("Activo")]
        public bool Activo { get; set; }

  
        //public IEnumerable<CortesiaPQRS> CortesiasPQRS { get; set; }

        //public int Cantidad { get; set; }
        //public string NumeroTicket { get; set; }
        //public string Descripcion { get; set; }

      //  public IFormFile ArchivoSoporte { get; set; }

        #endregion Propiedades

        #region Contructor
        #endregion Contructor

        #region Metodos 
        #endregion Metodos

    }
}
