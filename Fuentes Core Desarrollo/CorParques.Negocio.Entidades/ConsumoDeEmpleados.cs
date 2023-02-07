using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ConsumoDeEmpleados
    {
        #region constructor
        public ConsumoDeEmpleados()
        {

        }
        #endregion


        public DateTime? Fecha { get; set; }
        
        [Column("Documento")]
        public string Documento { get; set; }

        public string codigosap { get; set; }

        public string Cod_SAP { get; set; }

        public string Nombres { get; set; }
        public string Apellidos { get; set; }

        [Column("Empleado")]
        public string Empleado { get; set; }

        [Column("Punto")]
        public string Punto { get; set; }
       
        [Column("Area")]
        public string Area { get; set; }

        [Column("Valor")]
        public int Valor { get; set; }
        
        [Column("Id_Factura")]
        public string Factura { get; set; }

        public DateTime? hora { get; set; }
        
        public string No_factura { get; set; }
        
        public string Producto { get; set; }

        public int Cantidad { get; set; } //string
        
        public string fechaFinal { get; set; }

        public string fechaInicial { get; set; }
    }
}
