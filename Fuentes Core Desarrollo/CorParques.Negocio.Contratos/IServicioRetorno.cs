using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{

	public interface IServicioRetorno 
	{
        bool Insertar(Retorno modelo, out string error);
        bool InsertarDetalle(RetornoDetalle modelo, out string error);
    }
}
