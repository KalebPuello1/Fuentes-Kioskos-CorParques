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
    public class ServicioBitacoraDia : IServicioBitacoraDia
    {
        private readonly IRepositorioBitacoraDia _repositorio;

        public ServicioBitacoraDia(IRepositorioBitacoraDia repositorio)
        {
            _repositorio = repositorio;
        }

        public bool Actualizar(BitacoraDia modelo)
        {
            throw new NotImplementedException();
        }

        public BitacoraDiaLista Asignar(BitacoraDiaLista modelo)
        {
            string mensaje = string.Empty;
            BitacoraDia retorno = null;

            foreach (var item in modelo.BitacoraDiaList)
            {
                item.CantidadPersonans = modelo.CantidadPersonas;
                retorno = _repositorio.Asignar(item);
                if (!string.IsNullOrEmpty(retorno.Mensaje))
                    mensaje += retorno.Mensaje;
            }
            modelo.Mensaje = mensaje;
            return modelo;
        }

        public BitacoraDia Crear(BitacoraDia modelo)
        {
            throw new NotImplementedException();
        }

        public BitacoraDia Obtener(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BitacoraDia> Obtener(string fecha)
        {
            DateTime fecha_ = DateTime.Parse(fecha);
            return _repositorio.Obtener(fecha_);
        }

        public int? ObtenerCantidadPersonas()
        {
            return _repositorio.ObtenerCantidadPersonas();
        }

        public IEnumerable<BitacoraDia> ObtenerTodos()
        {
            return _repositorio.ObtenerTodos();
        }
    }
}
