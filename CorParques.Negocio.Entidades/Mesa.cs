using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_Mesa")]
    public class Mesa
    {

        [Key]
        [Column("IdMesa")]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Estado { get; set; }

        public int Id_Factura { get; set; }

        public int EstadoMesa { get; set; }
        public string NombreM { get; set; }
        public string Apellido { get; set; }

        public string NombreCliente { get; set; }

        public int IdZona { get; set; }

        public int IdTipo { get; set; }
    }
}

