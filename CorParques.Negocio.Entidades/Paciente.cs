using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

    [Table("TB_Paciente")]
    public class Paciente
    {

        #region Propiedades

        [Key]
        [Column("IdPaciente")]
        public int IdPaciente { get; set; }
        [Column("IdTipoDocumento")]
        public int IdTipoDocumento { get; set; }
        [Column("Documento")]
        public string Documento { get; set; }
        [Column("Nombre")]
        public string Nombre { get; set; }
        [Column("Apellido")]
        public string Apellido { get; set; }
        [Column("Acudiente")]
        public string Acudiente { get; set; }
        [Column("TelefonoPaciente")]
        public string TelefonoPaciente { get; set; }
        [Column("TelefonoAcudiente")]
        public string TelefonoAcudiente { get; set; }
        [Column("Correo")]
        public string Correo { get; set; }
        [Column("IdEstado")]
        public int IdEstado { get; set; }
        [Column("IdUsuarioCreacion")]
        public int IdUsuarioCreacion { get; set; }
        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; }
        [Column("IdUsuarioModificacion")]
        public int IdUsuarioModificacion { get; set; }
        [Column("FechaModificacion")]
        public DateTime FechaModificacion { get; set; }

        //public IEnumerable<TipoGeneral> ListaEstados { get; set; }
        public IEnumerable<TipoGeneral> ListaTipoDocumento { get; set; }

        [Editable(false)]
        public string TipoDocumento { get; set; }

        #endregion


    }
}
