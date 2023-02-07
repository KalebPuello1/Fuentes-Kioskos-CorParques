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
    public class ServicioConvenioSAP : IServicioConvenioSAP
    {

        private readonly IRepositorioConvenioSAP _repositorio;
        private readonly IRepositorioParametros _repoParametros;

        public ServicioConvenioSAP(IRepositorioConvenioSAP repositorio, IRepositorioParametros repoParametros)
        {
            _repositorio = repositorio;
            _repoParametros = repoParametros;
        }

        public bool Actualizar(Convenio modelo, out string error)
        {
            error = "";
            return _repositorio.Actualizar(ref modelo);
        }

        public bool Eliminar(Convenio modelo, out string error)
        {
            error = "";
            return _repositorio.Eliminar(modelo);
        }

        public bool Insertar(Convenio modelo, out string error)
        {
            error = "";
            int newId = _repositorio.Insertar(ref modelo);
            return newId != 0 ? true : false;
        }

        public IEnumerable<Convenio> ObtenerLista()
        {
            return _repositorio.ObtenerLista();
        }

        public Convenio ObtenerPorId(int Id)
        {
            return _repositorio.Obtener(Id);
        }

        public IEnumerable<ConvenioDetalle> ObtenerDetalleConvenio(int IdConvenio)
        {
            return _repositorio.ObtenerDetalleConvenio(IdConvenio);
        }

        public IEnumerable<ConvenioDetalle> ObtenerDetalleConvenioByApp(string Consecutivo, string productos, int Otroconvenio)
        {
            return _repositorio.ObtenerDetalleConvenioByApp(Consecutivo, productos, Otroconvenio);
        }

        public bool[] ValidarConvenioDaviplataFan(string Consecutivo)
        {
            var rta = _repositorio.ObtenerConvenioXConsecutivo(Consecutivo);
            var valDaviplata = _repoParametros.ObtenerParametroPorNombre("ConvenioDaviplata");
            var valFan = _repoParametros.ObtenerParametroPorNombre("IdClientefan");
            var res = new bool[2];
            res[0] = rta.ToString() == valDaviplata.Valor;
            res[1] = rta.ToString() == valFan.Valor;
            return res;

        }
        public IEnumerable<ConvenioDetalle> ObtenerDetalleConvenioByConsec(string Consecutivo)
        {
            return _repositorio.ObtenerDetalleConvenioByConsec(Consecutivo);
        }

        public string InsertarConvenioConsecutivos(List<ConvenioConsecutivos> lista)
        {
            return _repositorio.InsertarConvenioConsecutivos(lista);
        }

    }
}
