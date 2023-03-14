using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    [Table("TARIFAS_PARQUEADERO")]
    public class TarifasParqueadero
    {
        [Key]
        [Column("IdTarifaParqueadero")]
        public int Id { get; set; }

        [Column("IdTipoTarifaParqueadero")]
        public int IdTipoTarifaParqueadero { get; set; }

        [Column("IdTipoVehiculo")]
        public int IdTipoVehiculo { get; set; }

        [Column("Nombre")]
        public string Nombre { get; set; }

        //public int Cantidad  { get; set; }

        [Column("Valor")]
        public decimal Valor { get; set; }

        [Column("IdEstado")]
        public int IdEstado { get; set; }

        [Editable(false)]
        public string Estado { get; set; }

        [Editable(false)]
        public string TipoTarifaParqueadero { get; set; }

        [Editable(false)]
        public string TipoVehiculo { get; set; }

        [Column("IdUsuarioCreacion")]
        public int UsuarioCreacion { get; set; }

        public DateTime FechaCreacion { get; set; }

        [Column("IdUsuarioModificacion")]
        public int? UsuarioModificacion { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public IEnumerable<TipoGeneral> ListaTipoTarifaParqueadero { get; set; }

        public IEnumerable<TipoGeneral> ListaTipoVehiculo { get; set; }

        public IEnumerable<TipoGeneral> ListaEstados { get; set; }
    }
}
