using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CorParques.Negocio.Entidades
{

    [Table("GESTION_MANTENIMIENTO_CONTROL")]
    public class GestionMantenimientoControl
    {
        [Key]
        [Column("IDMANTENIMIENTO_CONTROL")]
        public int Id { get; set; }
        public string Descripcion{ get; set; }
        public int IdAtraccion { get; set; }
        public IEnumerable<TipoGeneral> Atracciones { get; set; }
        public IEnumerable<GestionMantenimientoDetalle> MantenimientoDetalle { get; set; }
        [Editable(false)]
        public string Verificadas { get; set; }
        public string Atraccion { get; set; }
        [Column("USUARIOCREADO")]        
        public int UsuarioCreacion { get; set; }
        [Column("FECHACREADO")]        
        public DateTime FechaCreacion { get; set; }
        [Column("USUARIOMODIFICADO")]        
        public int? UsuarioModicifacion { get; set; }
        [Column("FECHAMODIFICADO")]        
        public DateTime? FechaModificacion { get; set; }
    }
}
    