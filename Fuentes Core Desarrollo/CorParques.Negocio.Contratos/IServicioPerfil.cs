using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{
    /// <summary>
    /// NMSR Configuración Perfiles-Menu
    /// </summary>
	public interface IServicioPerfil : IServicioBase<Perfil>
	{

        IEnumerable<TipoGeneral> ObtenerListaSimple();

        bool Insertar(Perfil modelo, out string error);

        bool Actualizar(Perfil modelo, out string error);

        IEnumerable<Perfil> PerfilActivos(int IdPerfilActual);

        string ActualizarSegregacion(SegregacionFunciones model);

        IEnumerable<Perfil> ConsultarSegregacion(int idPerfil);

        string ValidarSegregacion(IEnumerable<Perfil> perfiles);
    }
}
