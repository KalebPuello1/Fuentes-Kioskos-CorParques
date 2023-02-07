using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{

	public interface IServicioResolucionFactura : IServicioBase<TipoGeneral>
	{
        IEnumerable<ResolucionFactura> ObtenerResoluciones(int aprovador);
        string InsertarResolucion(ResolucionFactura resolucion);
        string EliminarResolucion(int id);
        string AprobarResolucion(int id);
        IEnumerable<ResolucionFactura> ObtenerPrefijoConsecutivo(string prefijo, int consecutivoInicial);
    }
}
