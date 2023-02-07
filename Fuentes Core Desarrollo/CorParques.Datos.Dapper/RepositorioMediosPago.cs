using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Dapper
{
    public class RepositorioMediosPago : RepositorioBase<MediosPago>, IRepositorioMediosPagos
    {
        public IEnumerable<TipoGeneral> ListaSimple()
        {
            return ObtenerLista().Where(x => x.IdEstado == (int)Enumerador.Estados.Activo).Select(x => new TipoGeneral
            {
                Id = x.Id,
                Nombre = x.Nombre
            });
        }
    }
}
