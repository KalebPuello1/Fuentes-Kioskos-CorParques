using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{
    public interface IServicioSerieCodBarra
    {
        SerieCodigoBarras ObtenerSerie(int id);
        SerieCodigoBarras ObtenerSerie(string consecutivo);
    }
}
