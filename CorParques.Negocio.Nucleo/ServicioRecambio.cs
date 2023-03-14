using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;
using CorParques.Datos.Contratos;

namespace CorParques.Negocio.Nucleo
{
    public class ServicioRecambio : IServicioRecambio
    {
        private readonly IRepositorioRecambio _repositorio;

        public ServicioRecambio(IRepositorioRecambio repositorio)
        {
            _repositorio = repositorio;
        }

        public bool InsertarRecambio(Recambio modelo, out string error, out int IdRecambio)
        {
            return _repositorio.InsertarRecambio(modelo, out error, out IdRecambio);
        }
    }
}
