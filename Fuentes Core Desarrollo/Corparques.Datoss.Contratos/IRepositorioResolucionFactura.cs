using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{

	public interface IRepositorioResolucionFactura : IRepositorioBase<ResolucionFactura>
    {       

        IEnumerable<ResolucionFactura> ObtenerResoluciones(int aprovador);

        string InsertarResolucion(ResolucionFactura resolucion);
        string EliminarResolucion(int id);
        string AprobarResolucion(int id);
        IEnumerable<ResolucionFactura> ObtenerPrefijoConsecutivo(string prefijo, int consecutivoInicial);

    }
}
