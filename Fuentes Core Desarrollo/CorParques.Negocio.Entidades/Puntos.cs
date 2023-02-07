using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_Puntos")]
    public class Puntos
    {
        [Key]
        [Column("IdPunto")]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string CodigoPunto { get; set; }
        public decimal? Valor { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }
        public string Imagen { get; set; }
        public int EstadoId { get; set; }
        public int IdTipoPunto { get; set; }
        [Editable(false)]
        public string TipoPunto { get; set; }
        public string CodigoSap { get; set; }
        public IEnumerable<TipoGeneral> TipoPuntos { get; set; }
        [Editable(false)]
        public string Estado { get; set; }
        public string DescripcionCierre { get; set; }
        [Column("UsuarioCreado")]
        public int UsuarioCreacion { get; set; }
        [Column("FechaCreado")]
        public DateTime FechaCreacion { get; set; }
        [Column("UsuarioModificado")]
        public int? UsuarioModicifacion { get; set; }
        [Column("FechaModificado")]
        public DateTime? FechaModificacion { get; set; }
        public decimal? MontoRecaudo { get; set; }
        [Editable(false)]
        public bool ActualizaHoraTipopunto { get; set; }
        public int CordenadaX { get; set; }

        public int CordenadaY { get; set; }

        public float Latitud { get; set; }

        public float Longitud { get; set; }
        public IEnumerable<TipoGeneral> ListaTipoPunto { get; set; }


        //INICIO EDSP RESERVA ATRACCION
        [Column("Consecutivo")]
        public string Consecutivo { get; set; }
        [Column("CantidadCupos")]
        public int? CantidadCupos { get; set; }
        [Column("IntervaloTurno")]
        public int? IntervaloTurno { get; set; }

        //FIN EDSP RESERVA ATRACCION

        //GALD Se adiciona para doble impresion 
        public bool DobleFactura { get; set; }

        public IEnumerable<Producto> Brazaletes { get; set; }
        [Editable(false)]
        public int[] BrazaletesAsociados { get; set; }
        [Editable(false)]
        public bool AplicaBrazalete { get; set; }

    }
}
