
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

    [Table("TB_Perfil", Schema = "sec")]
    public class Perfil
    {

        #region Propiedades

        [Key]
        [Column("IdPerfil")]
        public int IdPerfil { get; set; }
        [Column("Nombre")]
        public string Nombre { get; set; }
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

        /// <summary>
        /// Menus relacionados con el perfil
        /// </summary>
        public IEnumerable<Menu> Menus { get; set; }

        /// <summary>
        /// Menus para cargar el arbol y asociar al perfil
        /// </summary>
        public IEnumerable<Menu> ListaMenus { get; set; }

        /// <summary>
        /// Lista para asignar el estado al perfil
        /// </summary>
        public IEnumerable<TipoGeneral> ListaEstados { get; set; }

        /// <summary>
        /// Lista para asignar el estado al perfil
        /// </summary>
        public IEnumerable<Perfil> Lista { get; set; }
        public SegregacionFunciones ListaPerfil { get; set; }
        public SegregacionFunciones IdPerfilConflicto { get; set; }
        public IEnumerable<Perfil> ListaPerfiles { get; set; }

        #endregion


    }
}
