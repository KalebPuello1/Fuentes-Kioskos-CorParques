using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

    [Table("TB_Cortesia")]
    public class Cortesia
    {

        #region Propiedades

        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; }
        [Column("IdUsuarioCreacion")]
        public int? IdUsuarioCreacion { get; set; }
        [Column("IdUsuarioVisitante")]
        public int? IdUsuarioVisitante { get; set; }

        [Column("Cantidad")]
        public int Cantidad { get; set; }

        [Column("NumeroTicket")]
        public string NumeroTicket { get; set; }
        [Column("Descripcion")]
        public string Descripcion { get; set; }

        [Column("FechaInicial")]
        public DateTime FechaInicial { get; set; }
        [Column("FechaFinal")]
        public DateTime FechaFinal { get; set; }

        [Column("RutaSoporte")]
        public string RutaSoporte { get; set; }

        [Column("Activo")]
        public bool Activo { get; set; }

        [Column("Observacion")]
        public string Observacion { get; set; }
        [Column("IdTipoCortesia")]
        public int IdTipoCortesia { get; set; }
        [Column("Aprobacion")]
        public bool Aprobacion { get; set; }
        [Column("IdComplejidad")]
        public int IdComplejidad { get; set; }
        [Column("IdPlazo")]
        public int IdPlazo { get; set; }
        [Column("IdRedencion")]
        public int IdRedencion { get; set; }

        [Column("NumTarjetaFAN")]
        public string NumTarjetaFAN { get; set; }

        [Column("CorreoAPP")]
        public string CorreoAPP { get; set; }
        #endregion
    }
}
