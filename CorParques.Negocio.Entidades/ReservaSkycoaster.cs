using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_Reserva")]
    public class ReservaSkycoaster
    {
        [Key]
        [Column("IdReserva")]
        public int? IdReserva { get; set; }
        public int? IdTicket { get; set; }
        public int? IdPunto { get; set; }

        public int Capacidad { get; set; }
        
        public int CapacidadDisponible { get; set; }

        public int IdUsuarioCreacion { get; set; }

        public DateTime FechaCreacion { get; set; }

        public int? IdUsuarioModificacion { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public bool Cerrar { get; set; }
        public string FechaReserva { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }
        public string Consecutivo { get; set; }

    }
}
