using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;

namespace CorParques.Negocio.Contratos
{
    public interface IServicioTipoTarifaParqueadero : IServicioBase<TipoTarifaParqueadero>
    {
                
        //RDSH: Obtiene una lista simple de tipos de tarifas para parqueadero.
        IEnumerable<TipoGeneral> ObtenerListaSimple();

        //RDSH: Inserta un tipo tarifa parqueadero.
        bool Insertar(TipoTarifaParqueadero modelo, out string error);

        //RDSH: Actualiza un tipo de tarifa parqueadero.
        bool Actualizar(TipoTarifaParqueadero modelo, out string error);

        //RDSH: Elimina un tipo de tarifa parqueadero.
        bool Eliminar(TipoTarifaParqueadero modelo, out string error);

    }
}
