using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

    [Table("TB_CargoPerfil", Schema = "sec")]
    public class CargosPerfil
    {

        #region Propiedades

        [Key]
        [Column("IdCargoPerfil")]
        public int IdCargoPerfil { get; set; }

        [Column("IdCargo")]
        public int IdCargo { get; set; }

        [Column("IdPerfil")]
        public int IdPerfil { get; set; }
    
        #endregion


    }
}
