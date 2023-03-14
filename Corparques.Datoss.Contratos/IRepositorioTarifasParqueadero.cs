using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{
    public interface IRepositorioTarifasParqueadero
    {
        //RDSH: Obtiene la lista de  tarifas para parqueadero.
        IEnumerable<TarifasParqueadero> ObtenerLista();

        //RDSH: Obtiene una lista simple de tarifas para parqueadero.
        IEnumerable<TipoGeneral> ObtenerListaSimple();

        //RDSH: Inserta una tarifa parqueadero.
        bool Insertar(TarifasParqueadero modelo, out string error);

        //RDSH: Actualiza un tarifa parqueadero.
        bool Actualizar(TarifasParqueadero modelo, out string error);

        //RDSH: Actualiza el estado a inactivo de una tarifa parqueadero.
        bool Eliminar(TarifasParqueadero modelo, out string error);

        TarifasParqueadero Obtener(int intId);

        //RDSH: Obtiene la lista de tipos de tarifas para parqueadero.
        IEnumerable<TipoGeneral> ObtenerTarifaPorIdTipoVehiculo(int IdTipoVehiculo);

    }
}
