using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{

	public interface IServicioRegistroTorniquete : IServicioBase<RegistroTorniquete>
	{

        // RDSH:Actualiza el registro de torniquete y remueve a los auxiliares asociados al punto.
        bool Actualizar(RegistroTorniquete modelo, out string error);

        // RDSH:Actualiza el registro de torniquete y remueve a los auxiliares asociados al punto.
        bool Insertar(RegistroTorniquete modelo, out string error);

        // RSDH: Retorna un objeto torniquete con la informacion de del dia o del ultimo registro.        
        RegistroTorniquete ObtenerRegistroTorniquete(int intIdPunto);

    }
}
