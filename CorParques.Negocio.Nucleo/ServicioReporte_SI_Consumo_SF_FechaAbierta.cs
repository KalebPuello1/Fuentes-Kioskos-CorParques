using CorParques.Datos.Contratos;
using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Nucleo
{
    public class ServicioReporte_SI_Consumo_SF_FechaAbierta : IServicioReporte_SI_Consumo_SF_FechaAbierta
    {

        private readonly IRepositorioReporte_SI_Consumo_SF_FechaAbierta repooSI_SF;
        public ServicioReporte_SI_Consumo_SF_FechaAbierta(IRepositorioReporte_SI_Consumo_SF_FechaAbierta repositorio)
        {
            repooSI_SF = repositorio;
        }

        public IEnumerable<Reporte_SI_Consumo_SF_FechaAbierta> getSI_SF(string fechaI, string fechaF, string Npedido, string redencion)
        {
            return repooSI_SF.getSI_SF(fechaI,fechaF,Npedido,redencion);
        }

        public IEnumerable<Reporte_SI_Consumo_SF_FechaAbierta>[] getSI_SFF(string dato)
        {
            return repooSI_SF.getSI_SFF("");
        }
    }
}
