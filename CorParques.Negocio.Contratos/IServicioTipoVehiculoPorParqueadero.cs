using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{

	public interface IServicioTipoVehiculoPorParqueadero
	{

        //RDSH: Obtiene la lista de los tipos de vehiculo por parqueadero activos para la grilla principal.
        IEnumerable<TipoVehiculoPorParqueadero> ObtenerLista();

        //RDSH: Inserta la disponibilidad del parqueadero.
        bool Insertar(TipoVehiculoPorParqueadero modelo, out string error);

        //RDSH: Actualiza la disponibilidad del parqueadero.
        bool Actualizar(TipoVehiculoPorParqueadero modelo, out string error);

        //RDSH: borrado logico disponibilidad parqueadero.
        bool Eliminar(TipoVehiculoPorParqueadero modelo, out string error);

        //RDSH: Obtiene la lista de los tipos de vehiculo por parqueadero activos y filtrados por id tipo de vehiculo.
        IEnumerable<TipoVehiculoPorParqueadero> ObtenerPorIdTipoVehiculo(int IdTipoVehiculo);

        //RDSH: Retorna un objeto TipoVehiculoPorParqueadero para la edicion.
        TipoVehiculoPorParqueadero ObtenerPorId(int Id);
    }
}
