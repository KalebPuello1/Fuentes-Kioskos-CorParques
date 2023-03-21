using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_Apertura")]
    public class Apertura
    {
        [Key]
        [Column("IdApertura")]
        public int Id { get; set; }

        public int IdPunto { get; set; }
        public DateTime Fecha { get; set; }
        public string ObservacionNido { get; set; }
        public string ObservacionSupervisor { get; set; }
        public string ObservacionPunto { get; set; }
        public int IdEstado { get; set; }
        public int UsuarioCreado { get; set; }
        public DateTime  FechaCreado { get; set; }
        public int UsuarioModificado { get; set; }
        public DateTime  FechaModificado { get; set; }
        public IEnumerable<AperturaBase> AperturaBase { get; set; }
        public List<Puntos> ListaPuntos { get; set; }
        public IEnumerable<TipoDenominacion> TiposDenominacion { get; set; }

        public IEnumerable<TipoGeneral> TipoElementos { get; set; }

        public IEnumerable<AperturaElementos> AperturaElemento { get; set; }

        public IEnumerable<TipoBrazalete> TiposBrazaletes { get; set; }

        public IEnumerable<AperturaBrazalete> AperturaBrazalete { get; set; }
        public int IdSupervisor { get; set; }
        public int IdTaquillero { get; set; }
        [Editable(false)]
        public string FechaString { get; set; }
        //Validar si la apertura es por Bitacora elemento
        [Editable(false)]
        public bool BitacoraElemento { get; set; }
        public int IdPuntoCreado { set; get; }

}
}
