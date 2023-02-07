using System;
using System.Collections.Generic;
using System.Text;

namespace CorParques.Negocio.Entidades
{
    public class ProductosTienda
    {
        public int IdProducto { get; set; }

        public DateTime FechaUso { get; set; }

        public int Precio { get; set; }

        public bool AplicaBrazalete { get; set; }

        public string Consecutivo { get; set; }

        public int IdBoleteria { get; set; }

        public string Nombre { get; set; }
        public string CodigoSap { get; set; }

        public bool Entregado { get; set; }

        public string CorreoUsuario { get; set; }        


    }
}

