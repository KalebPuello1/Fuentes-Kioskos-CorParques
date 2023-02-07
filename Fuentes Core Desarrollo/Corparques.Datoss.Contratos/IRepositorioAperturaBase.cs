using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{

	public interface IRepositorioAperturaBase : IRepositorioBase<AperturaBase>
	{

        string InsertarAperturaBase(Apertura apertura);
        IEnumerable<AperturaBase> ObtenerAperturaBase(int IdPunto, DateTime? Fecha);
        string ActualizarAperturaBase(Apertura apertura);

    }
}
