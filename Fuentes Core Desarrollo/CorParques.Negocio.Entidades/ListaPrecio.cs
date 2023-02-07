using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_LISTAPRECIOS_HEAD")]
    public class ListaPrecio
    {
        public ListaPrecio()
        {
            Creado = 1;
            FechaCreado = DateTime.Now;
            Modificado = 0;
            FechaModificado = DateTime.Now;
            EstadoId = "A";
        }

        [Key]
        [Column("CODLISTA")]
        public int Id { get; set; }
        [Column("DESCLISTA")]
        public string Descripcion { get; set; }

        [Column("FECINIVIG")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaInicioVigencia { get; set; }

        [Column("FECFINVIG")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaFinVigencia { get; set; }
        [Column("VALORTOTAL")]
        public decimal ValorTotal { get; set; }
        [Column("ESTADO")]
        public string EstadoId { get; set; }


        [Column("CREADO")]
        public int Creado { get; set; }
        [Column("FECCREADO")]
        public DateTime FechaCreado { get; set; }
        [Column("MODIFICADO")]
        public int Modificado { get; set; }
        [Column("FECMODIFCD")]
        public DateTime FechaModificado { get; set; }
    }
}
