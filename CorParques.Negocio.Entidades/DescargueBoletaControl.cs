using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class DescargueBoletaControl
    {
        /// <summary>
        /// Lista de productos de la boleta control
        /// </summary>
        public IEnumerable<Producto> Productos { get; set; }
        /// <summary>
        /// Mensaje si se presenta un error o no se cumple con una validación
        /// </summary>
        public string Mensaje { get; set; }
        
        /// <summary>
        /// Tipos de productos que se mostraran sin depender de la cantidad, para el pistoleo del codigo de barras
        /// </summary>
        public string TipoProd { get; set; }
        /// <summary>
        /// Cantidad para mostrar todos los productos para el pistoleo del codigo de barras
        /// </summary>
        public int Cantidad { get; set; }
    }
}
