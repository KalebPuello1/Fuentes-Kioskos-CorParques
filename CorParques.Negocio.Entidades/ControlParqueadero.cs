using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_ControlParqueadero")]
    public class ControlParqueadero
    {
        [Key]
        [Column("IdControlParqueadero")]
        public int Id { get; set; }

        [Column("Placa")]
        [Required]
        public string Placa { get; set; }

        [Column("IdTipoVehiculo")]
        [Required]
        public int IdTipoVehiculo{ get; set; }

        [Column("FechaHoraIngreso")]
        public DateTime FechaHoraIngreso { get; set; }

        [Column("IdUsuarioIngreso")]
        public int CodUsuarioIngreso { get; set; }

        [Column("IdEstado")]
        public int IdEstado { get; set; }

        [Column("FechaHoraSalida")]
        public DateTime? FechaHoraSalida { get; set; }

        [Column("IdUsuarioSalida")]
        public int? CodUsuarioSalida { get; set; }

        [Editable(false)]
        public double ValorPago { get; set; }

        [Column("IdTarifa")]
        public int IdTarifaParqueadero { get; set; }

        [Editable(false)]
        public IEnumerable<TipoVehiculoPorParqueadero> TiposVehiculoDisponible { get; set; }
    }
}
