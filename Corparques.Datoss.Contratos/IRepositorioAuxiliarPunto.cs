using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Datos.Contratos;

namespace CorParques.Datos.Contratos
{

	public interface IRepositorioAuxiliarPunto : IRepositorioBase<AuxiliarPunto>
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
