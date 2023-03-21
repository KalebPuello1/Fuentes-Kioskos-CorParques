using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorParques.Negocio.Entidades
{
    [Table("Tb_OrdenMantenimiento")]
    public class OrdenMantenimiento
    {
        [Key]
        [Column("IdOrdenMantenimiento")]
        public int IdOrdenMantenimiento { get; set; }
        [Column("CodSapPunto")]
        public string CodSapPunto { get; set; }
        [Column("Fecha")]
        public DateTime Fecha { get; set; }
        [Column("NumeroOrden")]
        public long NumeroOrden { get; set; }
        [Column("Nombre")]
        public string Nombre { get; set; }
        [Column("Procesado")]
        public bool Procesado { get; set; }

        public string Punto { get; set; }
        public bool Aprobado { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime HoraFin { get; set; }
        public string Observacion { get; set; }
        public int NumeroOperacion { get; set; }

        public IEnumerable<Operaciones> ListaOperaciones { get; set; }
    }
}
