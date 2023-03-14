using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{

	public interface IServicioAuxiliarPunto : IServicioBase<AuxiliarPunto>
	{
        //RDSH: Inserta una asociacion de auxiliares a un punto.
        bool Insertar(AuxiliarPunto modelo, out string error);

        //RDSH: Actualiza la fecha de modificacion de un auxiliar.
        bool Actualizar(AuxiliarPunto modelo, out string error);

        //RDSH: Lista de auxiliares asociados al punto.
        IEnumerable<AuxiliarPunto> ObtenerListaAuxiliarPunto(int intIdPunto);

        //RDSH: Consulta la informacion de un empleado para agregarlo a los auxiliares de la atraccion.
        EstructuraEmpleado ObtenerInformacionAuxiliar(int intIdPunto, string strDocumento);

    }
}
