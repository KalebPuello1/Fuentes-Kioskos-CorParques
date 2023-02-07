using CorParques.Datos.Contratos;
using CorParques.Negocio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;

namespace CorParques.Negocio.Nucleo
{
    public class ServicioCentroImpresion: IServicioCentroImpresion
    {
        private readonly IRepositorioCentroImpresion _repositorio;

        public ServicioCentroImpresion(IRepositorioCentroImpresion repositorio)
        {
            this._repositorio = repositorio;
        }

        public int? InsertarSolicitudImpresion(SolicitudBoleteria modelo)
        {
            return _repositorio.InsertarSolicitudImpresion(modelo);
        }

        public IEnumerable<SolicitudBoleteria> ObtenerListSolicitudBoleteria(int idUsuario)
        {
            return _repositorio.ObtenerListSolicitudBoleteria(idUsuario);
        }

        
        public IEnumerable<SolicitudBoleteria> ObtenerTodasSolicitudes()
        {
            return _repositorio.ObtenerTodasSolicitudes();
        }

        public IEnumerable<SolicitudBoleteria> GestionarCentroImpresion(SolicitudBoleteria modelo)
        {
            return _repositorio.GestionarCentroImpresion(modelo);
        }
        public bool EliminarSolicitudImpresion(SolicitudBoleteria modelo)
        {
            return _repositorio.EliminarSolicitudImpresion(modelo);
        }
    }
}
