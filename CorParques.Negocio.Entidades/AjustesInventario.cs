using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_AjustesInventario")]
    public class AjustesInventario
    {
        public AjustesInventario()
        {
            ListaPuntos = new List<Puntos>();
            ListaMateriales = new List<Materiales>();
            ListaTipoAjuste = new List<TipoAjusteInventario>();
            ListaMotivos = new List<MotivosInventario>();
        }

        [Editable(false)]
        public List<Puntos> ListaPuntos { get; set; }

        [Editable(false)]
        public List<Materiales> ListaMateriales { get; set; }

        [Editable(false)]
        public List<TipoAjusteInventario> ListaTipoAjuste { get; set; }

        [Editable(false)]
        public List<MotivosInventario> ListaMotivos { get; set; }

        [Editable(false)]
        public int IdPunto { get; set; }

        [Editable(false)]
        public Materiales Material { get; set; }

        [Editable(false)]
        public string DescSapTipoAjuste { get; set; }

        [Editable(false)]
        public string DescSapMotivo { get; set; }

        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("CodSapMaterial")]
        public string CodSapMaterial { get; set; }

        [Column("Cantidad")]
        public double Cantidad { get; set; }
        
        [Column("CodSapAlmacen")]
        public string CodSapAlmacen { get; set; }

        [Column("CodSapTipoAjuste")]
        public string CodSapTipoAjuste { get; set; }

        [Column("CodSapMotivo")]
        public string CodSapMotivo { get; set; }

        [Column("FechaAjuste")]
        public DateTime FechaAjuste { get; set; }

        [Column("FechaRegistro")]
        public DateTime FechaRegistro { get; set; }

        [Column("IdUsuarioRegistro")]
        public int IdUsuarioRegistro { get; set; }

        [Column("IdUsuarioAjuste")]
        public int IdUsuarioAjuste { get; set; }

        [Column("Procesado")]
        public bool Procesado { get; set; }

        [Column("Observaciones")]
        public string Observaciones { get; set; }

        [Column("UnidadMedida")]
        public string UnidadMedida { get; set;}

    }
}
