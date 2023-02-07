using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ImagenAdmin
    {
        public int Id { get; set; }
        public string CodPantalla { get; set; }
        public string NombreImagen { get; set; }
        public bool EstadoImagen { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string Ruta { get; set; }
        public int Frecuencia { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
    }
}
