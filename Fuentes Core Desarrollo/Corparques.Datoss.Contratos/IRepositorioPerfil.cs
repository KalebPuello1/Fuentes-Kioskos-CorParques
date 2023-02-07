using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CorParques.Datos.Contratos
{
    /// <summary>
    /// NMSR Configuración Perfiles-Menu
    /// </summary>
    public interface IRepositorioPerfil
    {
        IEnumerable<TipoGeneral> ObtenerListaSimple();

        bool Insertar(Perfil modelo, out string error);

        bool Actualizar(Perfil modelo, out string error);

        IEnumerable<Perfil> PerfilActivos(int IdPerfilActual);

        Perfil Obtener(int idPerfil);

        IEnumerable<Perfil> ObtenerLista();

        bool Inactivar(Perfil modelo);

        string ActualizarSegregacion(SegregacionFunciones model);

        IEnumerable<Perfil> ConsultarSegregacion(int idPerfil);
    }
}
