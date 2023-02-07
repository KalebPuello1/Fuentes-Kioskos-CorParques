using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

    [Table("TB_AuxiliarPunto")]
    public class AuxiliarPunto
    {

        #region Propiedades

        [Key]
        [Column("IdAuxiliarPunto")]
        public int IdAuxiliarPunto { get; set; }
        [Column("IdPunto")]
        public int IdPunto { get; set; }
        [Column("IdEstructuraEmpleado")]
        public int IdEstructuraEmpleado { get; set; }
        [Column("IdUbicacionPunto")]
        public int IdUbicacionPunto { get; set; }
        [Column("Certificado")]
        public bool Certificado { get; set; }
        [Column("IdUsuarioCreacion")]
        public int IdUsuarioCreacion { get; set; }
        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; }
        [Column("IdUsuarioModificacion")]
        public int IdUsuarioModificacion { get; set; }
        [Column("FechaModificacion")]
        public DateTime FechaModificacion { get; set; }

        
        public string NombreEmpleado { get; set; }
                
        public string UbicacionPunto { get; set; }

        public string Documento { get; set; }

        public IEnumerable<TipoGeneral> ListaUbicacionesPunto { get; set; }

        /// <summary>
        /// RDSH: Esta propiedad se usa para cuando van a remover un auxiliar de la lista de ubicaciones, si no esta asociado a 
        /// una ubicacion en el punto, no se podria remover y se debe mostrar un mensaje de que no tiene ubicaciones para remover.
        /// Esta propiedad tambien la tiene la entidad EstructuraEmpleado.
        /// </summary>
        public int TieneUbicacion { get; set; }

        #endregion


    }
}
