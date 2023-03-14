using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{

	public interface IRepositorioUbicacionPunto : IRepositorioBase<UbicacionPunto>
	{
        bool Insertar(UbicacionPunto modelo, out string error);
        bool ActualizarUbicacion(UbicacionPunto modelo, out string error);
        bool EliminarLogica(int id);
    }
}
