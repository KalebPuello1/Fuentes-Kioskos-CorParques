using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{

	public interface IServicioConvenioParqueadero
	{
        
        //RDSH: Lista principal de convenios parqueadero.
        IEnumerable<ConvenioParqueadero> ObtenerLista();

        //RDSH: Inserta un convenio parqueadero.
        bool Insertar(ConvenioParqueadero modelo, out string error);

        //RDSH: Actualiza un convenio parqueadero.
        bool Actualizar(ConvenioParqueadero modelo, out string error);

        //RDSH: borrado logico convenio parqueadero.
        bool Eliminar(ConvenioParqueadero modelo, out string error);

        //RDSH: Obtiene un objeto ConvenioParqueadero filtrando por placa.
        ConvenioParqueadero ObtenerPorPlaca(string Placa);

        //RDSH: Retorna un objeto ConvenioParqueadero para la edicion.
        ConvenioParqueadero ObtenerPorId(int Id);

        IEnumerable<EstructuraEmpleado> ObtenerListaEmpleados();
    }
}
