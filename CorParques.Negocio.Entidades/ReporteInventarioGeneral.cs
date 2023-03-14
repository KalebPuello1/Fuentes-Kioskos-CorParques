using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ReporteInventarioGeneral
    {
        
        [Editable(false)]
        public string fechaInicial { get; set; }

        [Editable(false)]
        public string fechaFinal { get; set; }

        [Editable(false)]
        public int idPunto { get; set; }

        [Editable(false)]
        public int idMaterial { get; set; }

     

        [Editable(false)]
        public DateTime? Fecha { get; set; }
        [Editable(false)]
        public  string Almacen { get; set; }
        [Editable(false)]
        public string CodSapMaterial { get; set; }
        [Editable(false)]
        public string Nombre { get; set; }
        [Editable(false)]
        public string Unidad { get; set; }
        [Editable(false)]
        public double CostoPromedio { get; set; }
        [Editable(false)]
        public double Inicial { get; set; }
        [Editable(false)]
        public double CostoInicial { get; set; }
        [Editable(false)]
        public double Entradas { get; set; }
        [Editable(false)]
        public double CostoEntradas { get; set; }
        [Editable(false)]
        public double Salidas { get; set; }
        [Editable(false)]
        public double CostoSalidas { get; set; }
        [Editable(false)]
        public double Final { get; set; }
        [Editable(false)]
        public double CostoFinal { get; set; }

        [Editable(false)]
        public int CB { get; set; }

    }
}
