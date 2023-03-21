using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Dapper
{
    public class RepositorioGestionMantenimiento : RepositorioBase<GestionMantenimiento>, IRepositorioGestionMantenimiento
    {
        public IEnumerable<TipoGeneral> ObtenerListaSimple(int idAtraccion)
        {
            //return _cnn.GetList<GestionMantenimiento>(string.Concat("where IdAtraccion = ", idAtraccion)).Select(x => new TipoGeneral { Id = x.Id, Nombre = x.Descripcion });
            return _cnn.GetList<GestionMantenimiento>().Where(x => x.IdAtraccion == idAtraccion).Select(x => new TipoGeneral { Id = x.Id, Nombre = x.Descripcion });
        }
    }
}
