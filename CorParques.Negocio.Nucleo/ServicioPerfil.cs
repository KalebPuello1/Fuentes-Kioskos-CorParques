using CorParques.Negocio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;
using CorParques.Datos.Contratos;

namespace CorParques.Negocio.Nucleo
{
    /// <summary>
    /// NMSR Configuración Perfiles-Menu
    /// </summary>
	public class ServicioPerfil : IServicioPerfil
    {

        private readonly IRepositorioPerfil _repositorio;

        #region Constructor

        public ServicioPerfil(IRepositorioPerfil repositorio)
        {

            _repositorio = repositorio;
        }

        #endregion

        public bool Insertar(Perfil modelo, out string error)
        {
            return _repositorio.Insertar(modelo, out error);
        }

        public bool Actualizar(Perfil modelo, out string error)
        {
            return _repositorio.Actualizar(modelo, out error);
        }

        public IEnumerable<Perfil> PerfilActivos(int IdPerfilActual)
        {
            return _repositorio.PerfilActivos(IdPerfilActual);
        }

        public IEnumerable<TipoGeneral> ObtenerListaSimple()
        {
            return _repositorio.ObtenerListaSimple();
        }

        public Perfil Obtener(int id)
        {
            return _repositorio.Obtener(id);
        }

        public IEnumerable<Perfil> ObtenerTodos()
        {
            return _repositorio.ObtenerLista();
        }

        public Perfil Crear(Perfil modelo)
        {
            throw new NotImplementedException();
        }

        public bool Actualizar(Perfil modelo)
        {
            return _repositorio.Inactivar(modelo);
        }

        public string ActualizarSegregacion(SegregacionFunciones model)
        {
            return _repositorio.ActualizarSegregacion(model);
        }

        public IEnumerable<Perfil> ConsultarSegregacion(int idPerfil)
        {
            return _repositorio.ConsultarSegregacion(idPerfil);
        }

        public string ValidarSegregacion(IEnumerable<Perfil> perfiles)
        {

            string retonar = string.Empty;
            foreach (Perfil perfil in perfiles)
            {
                IEnumerable<Perfil> perfilesConflicto = _repositorio.ConsultarSegregacion(perfil.IdPerfil);

                foreach (Perfil item in perfiles.Where(x => x.IdPerfil != perfil.IdPerfil ))
                {
                    if (perfilesConflicto.Count(x => x.IdPerfil == item.IdPerfil) > 0)
                    {
                        if (string.IsNullOrWhiteSpace(retonar))
                        {

                            retonar = string.Concat("Se presentan conflictos con los siguientes perfiles: ",  perfilesConflicto.Where(x => x.IdPerfil == item.IdPerfil).FirstOrDefault().Nombre);
                        }
                        else
                        {
                            retonar = string.Concat(retonar, ",", perfilesConflicto.Where(x => x.IdPerfil == item.IdPerfil).FirstOrDefault().Nombre);
                        }

                    }
                }

            }

            return retonar;
        }

    }
}
