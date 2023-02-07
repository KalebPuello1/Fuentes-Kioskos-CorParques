using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_archivoBrazalete")]
    public class ArchivoBrazalete
    {
        [Key]
        [Column("IdArchivoBrazalete")]
        public int IdArchivoBrazalete { get; set; }

        [Column("IdProducto")]
        public int IdProducto { get; set; }

        [Column("IdEstado")]
        public int IdEstado { get; set; }

        [Column("Archivo")]
        public string Archivo { get; set; }

        [Column("UsuarioCreacion")]
        public string UsuarioCreacion { get; set; }

        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; }

        [Column("Observacion")]
        public string Observacion { get; set; }

        [Column("UsuarioModificacion")]
        public string UsuarioModificacion { get; set; }

        [Column("FechaModificacion")]
        public DateTime? FechaModificacion { get; set; }
    }

}
