using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CorParques.Negocio.Entidades
{

    public class UsuarioVisitanteViewModel
    {

        #region Propiedades

        public int Id { get; set; }

        public string Nombres { get; set; }

        public string Apellidos { get; set; }

        public string TipoDocumento { get; set; }

        public string NumeroDocumento { get; set; }


        public string Correo { get; set; }

        public string Telefono { get; set; }

        public int IdUsuarioCreacion { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaActualizacion { get; set; }

        public bool Activo { get; set; }
        public bool Aprobacion { get; set; }

        public IEnumerable<Cortesia> CortesiasPQRS { get; set; }

        public IEnumerable<DetalleCortesia> ListDetalleCortesia { get; set; }

        public IEnumerable<Producto> listaProductosAgregar { get; set; }

        public int Cantidad { get; set; }
        public string NumeroTicket { get; set; }
        public string Descripcion { get; set; }

        public string Archivo { get; set; }

        public string RutaSoporte { get; set; }

        public string Observacion { get; set; }

        public string NumTarjetaFAN { get; set; }
        public int IdTipoCortesia { get; set; }
        public int IdComplejidad { get; set; }
        public int IdPlazo { get; set; }
        public int IdRedencion { get; set; }

        public int BanderaFan { get; set; }
        public DateTime FechaInicialFan { get; set; }
        public DateTime FechaFinalFan { get; set; }
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }


        public string DescripcionBeneficioFAN { get; set; }

        public string EstadoRedencion { get; set; }

        public HttpPostedFileBase ArchivoSoporte { get; set; }

        #endregion Propiedades

        #region Contructor
        #endregion Contructor

        #region Metodos 
        #endregion Metodos

    }
}
