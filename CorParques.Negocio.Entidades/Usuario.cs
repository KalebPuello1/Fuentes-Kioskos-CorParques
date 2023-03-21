using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_Usuario", Schema ="sec")]
    public class Usuario
    {

        #region Propiedades

        [Key, Column("IdUsuario")]
        public int Id { get; set; }

        [Column("Nombre")]
        public string Nombre { get; set; }

        [Column("Apellido")]
        public string Apellido { get; set; }

        [Column("Usuario")]
        public string NombreUsuario { get; set; }

        [Column("Password")]
        public string Password { get; set; }


        [Column("NumeroIntentos")]
        public int? NumeroIntentos { get; set; }

        public bool Logueado { get; set; }
        public int IdPuntoLogueado { get; set; }
        [Editable(false)]
        public int IdTipoPuntoLogueado { get; set; }

        [Column("Correo")]
        public string Correo { get; set; }

        [Column("IdEstado")]
        public int IdEstado { get; set; }

        [Column("IdUsuarioCreacion")]
        public int IdUsuarioCreacion { get; set; }


        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; }


        [Column("IdUsuarioModificacion")]
        public int? IdUsuarioModificacion { get; set; }

        [Column("FechaModificacion")]
        public DateTime? FechaModificacion { get; set; }

        [Column("IdEstructuraEmpleado"), Editable(false)]
        public int IdEstructuraEmpleado { get; set; }

        public List<TipoGeneral> ListaPerfiles { get; set; }
        public List<Menu> ListaMenu { get; set; }
        public List<Puntos> ListaPuntos { get; set; }


        
        public string Password2 { get; set; }

        public bool CambioPassword { get; set; }       

        [Editable(false)]
        public string ConfirmPassword2 { get; set; }
        [Editable(false)]
        public int IdEmpleado { get; set; }

       
        public IEnumerable<Perfil> Perfiles { get; set; }
        

        #endregion Propiedades

        #region Contructor
        #endregion Contructor

        #region Metodos 
        #endregion Metodos

    }
}
