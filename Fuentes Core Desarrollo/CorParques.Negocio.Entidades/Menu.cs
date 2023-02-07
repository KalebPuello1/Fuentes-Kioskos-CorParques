using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

    [Table("TB_Menu", Schema = "sec")]
    public class Menu
    {

        #region Propiedades

        [Key]
        [Column("IdMenu")]
        public int IdMenu { get; set; }
        [Column("Nombre")]
        public string Nombre { get; set; }

        public string Clase { get; set; }
        [Column("Controlador")]
        public string Controlador { get; set; }
        [Column("IdPadre")]
        public int? IdPadre { get; set; }
        [Column("Orden")]
        public int Orden { get; set; }
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

        public bool Administracion { get; set; }
        public IEnumerable<Menu> MenuHijos { get; set; }
        #endregion


    }
}
