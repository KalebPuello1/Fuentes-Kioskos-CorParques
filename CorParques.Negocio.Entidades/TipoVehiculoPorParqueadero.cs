using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

    [Table("TB_TipoVehiculoPorParqueadero")]
    public class TipoVehiculoPorParqueadero
    {

        #region Propiedades

        [Key]
        [Column("IdTipoVehiculoPorParqueadero")]
        public int IdTipoVehiculoPorParqueadero { get; set; }
        [Column("IdTipoVehiculo")]
        public int IdTipoVehiculo { get; set; }
        [Column("Cantidad")]
        public int Cantidad { get; set; }

        [Column("EspaciosPorVehiculo")]
        public decimal EspaciosPorVehiculo { get; set; }

        [Editable(false)]
        public string strEspaciosPorVehiculo
        {
            get
            {
                string valR = EspaciosPorVehiculo.ToString().Replace(",", ".");
                return valR;
            }
            set
            {
                EspaciosPorVehiculo = decimal.Parse(value.Replace(".", ","));
            }
        }


        [Column("EspaciosReservados")]
        public int EspaciosReservados { get; set; }

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

        [Editable(false)]
        public string Estado { get; set; }

        [Editable(false)]
        public string TipoVehiculo { get; set; }

        [Editable(false)]
        public string DisponibilidadConNombre
        {
            get
            {
                if (!string.IsNullOrEmpty(this.TipoVehiculo))
                    return string.Format("{0} | Disponible: {1}", this.TipoVehiculo, this.Cantidad.ToString("####"));
                else
                    return string.Format("{0}", this.Cantidad.ToString("####"));
            }
        }

        public IEnumerable<TipoGeneral> ListaTipoVehiculo { get; set; }
        public IEnumerable<TipoGeneral> ListaEstados { get; set; }
        #endregion


    }
}
