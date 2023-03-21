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

	public class ServicioArqueo : IServicioArqueo
	{

        #region Constructor

        private readonly IRepositorioArqueo _repositorio;
        private readonly IRepositorioRecoleccion _repositorioDetalleBrazalete;
        
        public ServicioArqueo(IRepositorioArqueo repositorio, IRepositorioRecoleccion repositorioDetalleBrazalete)
        {
            _repositorio = repositorio;
            _repositorioDetalleBrazalete = repositorioDetalleBrazalete;

        }

        public bool Actualizar(NovedadArqueo modelo)
        {
            throw new NotImplementedException();
        }

        public bool ActualizarNovedadArqueo(List<NovedadArqueo> modelo)
        {
            foreach(NovedadArqueo item in modelo)
            {
                if (!_repositorio.InsertarNovedadArqueo(item))
                {
                    return false;
                }
            }

            return true;
        }

        public bool Actualizar(Arqueo modelo)
        {
            throw new NotImplementedException();
        }

        public NovedadArqueo Crear(NovedadArqueo modelo)
        {
            var resul = _repositorio.Insertar(ref modelo);
            return _repositorio.Obtener(resul);
        }

        public Arqueo Crear(Arqueo modelo)
        {
            throw new NotImplementedException();
        }

        public Arqueo Obtener(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Arqueo> ObtenerArqueo(int IdUsuario, int IdPunto)
        {
            var Arqueo = _repositorio.ObtenerArqueo(IdUsuario);

            foreach (Arqueo item in Arqueo)
            {
                item.Brazalete = _repositorioDetalleBrazalete.ObtenerBrazaletesRestantes(IdUsuario, IdPunto);
            }

            return Arqueo;
        }

        public IEnumerable<Arqueo> ObtenerTodos()
        {
            throw new NotImplementedException();
        }

        NovedadArqueo IServicioBase<NovedadArqueo>.Obtener(int id)
        {
            throw new NotImplementedException();
        }

        IEnumerable<NovedadArqueo> IServicioBase<NovedadArqueo>.ObtenerTodos()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
