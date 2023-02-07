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
    public class ServicioPaciente : IServicioPaciente
    {
        IRepositorioPaciente _repositorio;

        public ServicioPaciente(IRepositorioPaciente repositorio)
        {
            _repositorio = repositorio;
        }

        public bool Actualizar(Paciente modelo, out string error)
        {
            return _repositorio.Actualizar(modelo, out error);
        }

        public bool Eliminar(Paciente modelo, out string error)
        {
            return _repositorio.Eliminar(modelo, out error);
        }

        public bool Insertar(Paciente modelo, out string error)
        {
            return _repositorio.Insertar(modelo, out error);
        }

        public IEnumerable<Paciente> ObtenerLista()
        {
            return _repositorio.ObtenerLista();
        }

        public IEnumerable<TipoGeneral> ObtenerListaSimple()
        {
            return _repositorio.ObtenerListaSimple();
        }

        public Paciente ObtenerPorId(int Id)
        {
            return _repositorio.ObtenerPorId(Id);
        }

        public Paciente ObtenerPorTipoDocumento(int IdTipoDocumento, string Documento)
        {
            return _repositorio.ObtenerPorTipoDocumento(IdTipoDocumento, Documento);
        }
    }
}
