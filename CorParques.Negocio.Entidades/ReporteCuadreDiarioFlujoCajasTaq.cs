using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ReporteCuadreDiarioFlujoCajasTaq
    {
        [Column("Fecha")]
        public string Fecha { get; set; }

        [Column("Efectivo")]
        public string Efectivo { get; set; }

        [Column("Deposito")]
        public string Deposito { get; set; }

        [Column("TotalEfectivo")]
        public string TotalEfectivo { get; set; }

        [Column("Tarjetas")]
        public string Tarjetas { get; set; }

        [Column("BonoRegalo")]
        public string BonoRegalo { get; set; }

        [Column("BonoSodexo")]
        public string BonoSodexo { get; set; }    

        [Column("Payulatam")]
        public string Payulatam { get; set; }     

        [Column("DescuentoEmpleadoActivos")]
        public string DescuentoEmpleadoActivos { get; set; }

        [Column("DescuentoEmpleadoServiola")]
        public string DescuentoEmpleadoServiola { get; set; }

        [Column("DescuentoEmpleadoCorparques")]
        public string DescuentoEmpleadoCorparques { get; set; }

        [Column("Recaudos")]//////////////
        public string Recaudos { get; set; }

        [Column("ConsumosCortesias")]
        public string ConsumosCortesias { get; set; }

        [Column("Sobrantes")]
        public string Sobrantes { get; set; }

        [Column("Faltantes")]
        public string Faltantes { get; set; }

        [Editable(false)]
        public string fechaInicial { get; set; }

        [Editable(false)]
        public string fechaFinal { get; set; }

        [Editable(false)]
        public int idTipIngreso { get; set; }

        [Editable(false)]
        public string TipNovedad { get; set; }

        [Editable(false)]
        public string TipConsnumo { get; set; }




    }
}
