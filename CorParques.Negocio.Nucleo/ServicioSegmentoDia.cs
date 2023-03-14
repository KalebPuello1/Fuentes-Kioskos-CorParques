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
    public class ServicioSegmentoDia : IServicioSegmentoDia
    {   
        private readonly IRepositorioSegmentoDia _repositorio;

        public ServicioSegmentoDia(IRepositorioSegmentoDia repositorio)
        {
            _repositorio = repositorio;
        }

        public bool Actualizar(TipoGeneral modelo)
        {
            throw new NotImplementedException();
        }

        public TipoGeneral Crear(TipoGeneral modelo)
        {
            throw new NotImplementedException();
        }

        public TipoGeneral Obtener(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TipoGeneral> ObtenerTodos()
        {
            return _repositorio.StoreProcedure("SP_ConsultarSegmentosDia", null);
        }
    }
}