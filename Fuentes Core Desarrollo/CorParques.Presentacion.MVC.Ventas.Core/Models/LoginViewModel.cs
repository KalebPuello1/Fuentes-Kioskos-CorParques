using CorParques.Negocio.Entidades;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CorParques.Presentacion.MVC.Ventas.Core.Models
{
    public class LoginViewModel
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string NombreUsuario { get; set; }

        public string Password { get; set; }

        public int? NumeroIntentos { get; set; }

        public string Correo { get; set; }

        public int IdEstado { get; set; }

        public int IdUsuarioCreacion { get; set; }

        public DateTime FechaCreacion { get; set; }

        public int? IdUsuarioModificacion { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public List<TipoGeneral> ListaPerfiles { get; set; }
        public List<Menu> ListaMenu { get; set; }
        public List<Puntos> ListaPuntos { get; set; }

        public Puntos Punto { get; set; }

        public Enumerador.TiposPuntos TipoPunto { get; set; }
    }
}