using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{

	public interface IRepositorioPlaneacion : IRepositorioBase<Planeacion>
	{

        // RDSH: Obtiene la lista de indicadores.
        IEnumerable<TipoGeneral> ObtenerListaIndicadores();

        //RDSH: Retorna una planeacion filtrada por id indicador y fecha.
        IEnumerable<PlaneacionAuxiliar> ConsultarPlaneacion(int intIdIndicador, string strFecha);

        //RDSH: Inserta la planeacion mensual.
        bool Insertar(Planeacion modelo, out string error);
    }
}
