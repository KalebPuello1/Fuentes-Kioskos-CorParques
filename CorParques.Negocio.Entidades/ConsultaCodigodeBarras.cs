using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorParques.Negocio.Entidades
{
    
    public class ConsultaCodigodeBarras
    {
        
        [Editable(false)]    
        public string CodigodeBarras { get; set; }

        [Editable(false)]
        public int Tipo { get; set; }

        [Editable(false)]
        public int IdPunto { get; set; }

        [Editable(false)]
        public long Saldo { get; set; }

        [Editable(false)]
        public long PrecioPunto { get; set; }

        [Editable(false)]
        public string MensajeAcceso { get; set; }

        [Editable(false)]
        public string CodigoMensaje { get; set; }

        [Editable(false)]
        public int IngresosDisponibles { get; set; }

        [Editable(false)]
        public long ValorRecarga { get; set; }

        [Editable(false)]
        public long SaldoActual { get; set; }

        [Editable(false)]
        public bool Valido { get; set; }

        [Editable(false)]
        public long ValorAcumulado { get; set; }

        [Editable(false)]
        public long ValorDescuentoConvenio { get; set; }

        [Editable(false)]
        public int NumeroAccesosDia { get; set; }

        [Editable(false)]
        public byte[] foto { get; set; }
        [Editable(false)]
        public string fotoTexto { get; set; }
    }
}
