using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

    [Table("TB_EstructuraEmpleado")]
    public class EstructuraEmpleado
    {

        #region Propiedades

        [Key]
        [Column("IdEstructuraEmpleado")]
        public int IdEstructuraEmpleado { get; set; }
        [Column("CodigoSap")]
        public string CodigoSap { get; set; }
        [Column("Nombres")]
        public string Nombres { get; set; }
        [Column("Apellidos")]
        public string Apellidos { get; set; }
        [Column("Documento")]
        public string Documento { get; set; }
        [Column("Area")]
        public string Area { get; set; }
        [Column("IdEstado")]
        public int IdEstado { get; set; }
        [Column("CodigoSapCargo")]
        public string CodigoSapCargo { get; set; }
        [Column("CupoRestante")]
        public decimal CupoRestante { get; set; }

        [Editable(false)]
        public bool EsCertificado { get; set; }

        [Editable(false)]
        public bool SoloDescuentoEmpleado { get; set; }

        /// <summary>
        /// RDSH: Esta propiedad se usa para cuando van a remover un auxiliar de la lista de ubicaciones, si no esta asociado a 
        /// una ubicacion en el punto, no se podria remover y se debe mostrar un mensaje de que no tiene ubicaciones para remover.
        /// </summary>
        [Editable(false)]
        public int TieneUbicacion { get; set; }

        [Editable(false)]
        public string Valor { get; set; }

        [Editable(false)]
        public string Texto { get; set; }

        #endregion


    }
}
