using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{
    public interface IServicioConvenioSAP
    {
        IEnumerable<Convenio> ObtenerLista();

        bool Insertar(Convenio modelo, out string error);

        bool Actualizar(Convenio modelo, out string error);

        bool Eliminar(Convenio modelo, out string error);

        Convenio ObtenerPorId(int Id);

        IEnumerable<ConvenioDetalle> ObtenerDetalleConvenio(int IdConvenio);
        IEnumerable<ConvenioDetalle> ObtenerDetalleConvenioByConsec(string Consecutivo);
        bool[] ValidarConvenioDaviplataFan(string Consecutivo);
        IEnumerable<ConvenioDetalle> ObtenerDetalleConvenioByApp(string Consecutivo, string productos, int Otroconvenio);

        string InsertarConvenioConsecutivos(List<ConvenioConsecutivos> lista);
    }
}
