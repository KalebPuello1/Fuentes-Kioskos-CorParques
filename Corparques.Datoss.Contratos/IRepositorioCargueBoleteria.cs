using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{

	public interface IRepositorioCargueBoleteria : IRepositorioBase<CargueBoleteria>
	{

        /// RDSH: Inserta un cargue masivo de boleteria.        
        bool Insertar(CargueBoleteria modelo, out string error);

        /// RDSH: Actualiza el estado del cargue de boleteria.        
        bool Actualizar(CargueBoleteria modelo, out string error);
        
        /// RDSH: Retorna la lista de cargues realizados en la tabla TB_CargueBoleteria.        
        IEnumerable<CargueBoleteria> ObtenerListaCargueBoleteria();
        
        /// RDSH: Retorna los productos de tipo boleteria.        
        IEnumerable<TipoGeneral> ObtenerTipoBoleteria();

    }
}
