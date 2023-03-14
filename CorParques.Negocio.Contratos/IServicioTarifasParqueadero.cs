using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{
    public interface IServicioTarifasParqueadero : IServicioBase<TarifasParqueadero>
    {
        
        //RDSH: Obtiene una lista simple de tipos de tarifas para parqueadero.
        IEnumerable<TipoGeneral> ObtenerListaSimple();

        //RDSH: Inserta un tipo tarifa parqueadero.
        bool Insertar(TarifasParqueadero modelo, out string error);

        //RDSH: Actualiza un tipo de tarifa parqueadero.
        bool Actualizar(TarifasParqueadero modelo, out string error);

        //RDSH: Elimina un tipo de tarifa parqueadero.
        bool Eliminar(TarifasParqueadero modelo, out string error);

        //RDSH: Obtiene la lista de tipos de tarifas para parqueadero.
        IEnumerable<TipoGeneral> ObtenerTarifaPorIdTipoVehiculo(int IdTipoVehiculo);
    }
}
