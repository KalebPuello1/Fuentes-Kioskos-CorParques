using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ReporteDestrezas
    {        
        public string Punto { get; set; } //Punto:	Nombre de la destreza
        public string Asesor { get; set; } //Asesor 	Persona que realiza venta en institucional 
        public string Cliente { get; set; } //Cliente 	Persona o empresa con quien se ejerce la venta institucional 
        public string Convenio { get; set; } //Convenio	Modalidad de negocio con la que el cliente adquiere el material para su uso       
        public string ValorTotalIngreso { get; set; } //Valor ingresos venta uno a uno  Valor total neto de ingresos en la operación, venta uno a uno
        public string ValorVentaUnoAUno { get; set; }
        public string ValorVentaInstitucional{ get; set; }
        public string ValorUso { get; set; } //Valor uso   Valor de la boleta descargada por tipo de boleta
        public string TipoBoletaDescargada { get; set; } //Tipo Boleta Nombre de la boleta descargada(Eje: 1 uso en destreza cupo debito u otra)
        public string CantidadUsos { get; set; } //Cantidad de usos Cantidad de usos descargados por tipo de boleta y valor de la misma
        public string CantidadPremios { get; set; } //Cantidad Premio entregado Tipo de premio entregado(Serie 1, Serie 2, Serie 3, etc.,) con su respectiva cantidad
        public string TotalCantPremios { get; set; }        
        public string Porcentaje { get; set; } // ValorTotalIngresos / TotalCantPremios


        //public int IdPunto { get; set; }
        //public string ValorTotalIngresosSinImp { get; set; } //Valor total ingresos sin impuesto Valor total neto de ingresos en la operación(Venta 1 a 1 e institucional), sin impuesto
        //public string CostoTotal { get; set; } //Costo total     Sumatoria de las unidades de premios entregados
        //public string PjeCosto { get; set; } //Porcentaje costo total:	Porcentaje de costo(costo de total de premios*100/valor total de ventas uno a uno + valor total de ventas institucionales)
        //Observaciones	
        //-	El ingreso institucional se da por el ingreso a diario y no por el valor del pedido de la venta institucional.
        //-	Valor de ingresos en orden descendente, por tipo de boleta y agrupación por modalidad de venta.
        //public string ValorIngresoSinImp { get; set; } //Valor ingresos venta uno a uno sin impuesto Valor total neto de ingresos en la operación, venta uno a uno sin impuesto
        //public string ValorTotalIngresoVtaIns { get; set; } //Valor total ingresos institucional  Valor total neto de ingresos en la operación, venta institucional
        //public string ValorTotalIngresoVtaInsSinImp { get; set; } //Valor total ingresos institucional sin impuesto Valor total neto de ingresos en la operación, venta institucional sin impuesto
        //public string CostoPremio { get; set; } //Costo premio    Corresponde al valor unitario antes de IVA* la cantidad por tipo de premio
        //Estas propiedades son para la hoja de premios.
        //public DateTime? Fecha { get; set; }
        //public string Premio { get; set; }
        //public int Cantidad { get; set; }

    }

}
