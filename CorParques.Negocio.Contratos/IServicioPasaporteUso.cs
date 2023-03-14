using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{
    public interface IServicioPasaporteUso
    {
        bool ActualizarPasaporteUso(PasaporteUso Modelo);
        //IEnumerable<PasaporteUso> ObtenerTodosPasaporteUso();
        IEnumerable<TipoGeneral> ObtenerListaSimple();
        IEnumerable<TipoGeneral> ObtenerListaSimpleAtraccion();
        IEnumerable<TipoGeneral> ObtenerListaSimpleEstado();
        PasaporteUso Obtener(int id);
    }
}
