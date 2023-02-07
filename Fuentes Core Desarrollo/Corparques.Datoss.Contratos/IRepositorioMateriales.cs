using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{

	public interface IRepositorioMateriales : IRepositorioBase<Materiales>
	{
        IEnumerable<Materiales> ObtenerMaterialesxPuntos(int IdPunto, DateTime? Fecha);
        IEnumerable<Materiales> ObtenerTodos();

    }
}
