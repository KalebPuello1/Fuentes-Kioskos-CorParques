using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

    [Table("TB_SegregacionFunciones", Schema = "sec")]
    public class SegregacionFunciones
    {

        #region Propiedades

        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("IdPerfil")]
        public int IdPerfil { get; set; }

        [Column("IdPerfilConflicto")]
        public int IdPerfilConflicto { get; set; }

        [Column("IdUsuarioCreacion")]
        public int IdUsuarioCreacion { get; set; }

        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; }
        public bool Administracion { get; set; }
       
        public List<Perfil> ListaPerfilConflicto { get; set; }

        #endregion


    }
}
