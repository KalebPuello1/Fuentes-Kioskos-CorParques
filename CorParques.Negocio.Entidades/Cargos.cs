using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

    [Table("TB_Cargos")]
    public class Cargos
    {

        #region Propiedades

        [Key]
        [Column("IdCargo")]
        public int IdCargo { get; set; }

        [Column("Nombre")]
        public string Nombre { get; set; }

        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; }

        [Column("UsarioCreacion")]
        public int UsuarioCreacion { get; set; }

        [Column("IdEstado")]
        public int IdEstado { get; set; }

        [Editable(false)]
        public int IdPerfil { get; set; }

        /// <summary>
        /// Perfiles relacionados con el cargo
        /// </summary>
        public IEnumerable<Perfil> Perfiles { get; set; }


        /// <summary>
        /// Menus para cargar el arbol y asociar al perfil
        /// </summary>
        public IEnumerable<Perfil> ListaPerfiles { get; set; }
        public IEnumerable<TipoGeneral> ListaEstados { get; set; }


        #endregion


    }
}
