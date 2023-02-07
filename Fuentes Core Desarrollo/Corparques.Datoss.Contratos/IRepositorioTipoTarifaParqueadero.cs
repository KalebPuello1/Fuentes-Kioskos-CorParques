using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{
    public interface IRepositorioTipoTarifaParqueadero
    {

        //RDSH: Obtiene la lista de tipos de tarifas para parqueadero.
        IEnumerable<TipoTarifaParqueadero> ObtenerLista();

        //RDSH: Obtiene una lista simple de tipos de tarifas para parqueadero.
        IEnumerable<TipoGeneral> ObtenerListaSimple();

        //RDSH: Inserta un tipo tarifa parqueadero.
        bool Insertar(TipoTarifaParqueadero modelo, out string error);

        //RDSH: Actualiza un tipo de tarifa parqueadero.
        bool Actualizar(TipoTarifaParqueadero modelo, out string error);

        //RDSH: Elimina un tipo de tarifa parqueadero.
        bool Eliminar(TipoTarifaParqueadero modelo, out string error);

        TipoTarifaParqueadero Obtener(int intIdTipoTarifaParqueadero);

    }
}
