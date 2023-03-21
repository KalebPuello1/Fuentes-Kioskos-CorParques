using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{
    public interface IRepositorioPasaporteUso
    {
        IEnumerable<TipoGeneral> ObtenerListaSimple();
        bool ActualizarPasaporteUso(PasaporteUso modelo);
        //IEnumerable<PasaporteUso> ObtenerTodosPasaporte();
        PasaporteUso ObtenerPasaporte(int idTipoBrazalete);
    }
}
