//using Corparques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{
    public interface IServicioReporte_SI_Consumo_SF_FechaAbierta
    {
        IEnumerable<Reporte_SI_Consumo_SF_FechaAbierta> getSI_SF(string fechaI, string fechaF, string Npedido, string redencion);
        IEnumerable<Reporte_SI_Consumo_SF_FechaAbierta>[] getSI_SFF(string dato);
    }
}
