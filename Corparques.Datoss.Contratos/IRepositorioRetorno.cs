using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{

	public interface IRepositorioRetorno 
	{
        bool Insertar(Retorno modelo, out string error);
        bool InsertarDetalle(RetornoDetalle modelo, out string error);
    }
}
