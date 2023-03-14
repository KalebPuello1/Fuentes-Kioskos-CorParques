using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorParques.Negocio.Entidades
{
    public class ProductosMesaCocinaGroup
    {
        public int IdMesa { get; set; }
        public string NombreMesa { get; set; }
        public int Id_Estado { get; set; }
        public int Id_TipoProdRestaurante { get; set; }
        public int Id_Vendedor { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }

    }
}
