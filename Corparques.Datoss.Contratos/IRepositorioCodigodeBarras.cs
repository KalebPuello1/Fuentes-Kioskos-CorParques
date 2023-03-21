using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;

namespace CorParques.Datos.Contratos
{
    public interface IRepositorioCodigodeBarras : IRepositorioBase<ConsultaCodigodeBarras>
    {

        // RDSH: Valida los requisitos para que un punto pueda operar.  
        ConsultaCodigodeBarras ValidarPermisoOperativoPunto(int intIdPunto, out string strError);

        //RDSH: Valida si un codigo de barras es valido para ingresar a una atracción o destreza.
        ConsultaCodigodeBarras ValidarCodigoBarras(string strCodigoBarras, int intIdPunto, int intIdUsuario, long lngValorAcumulado, long lngValorAcumuladoConvenio, string strCodigoDeBarrasDescargar, out string strError);


        /// RDSH: Transfiere el dinero acumulado de varios codigos de barra a uno que haya sido leido dos veces.
        ConsultaCodigodeBarras TransferirSaldo(string strCodigoACargar, string strCodigoDeBarrasDescargar, long lngValorAcumulado, int intIdUsuario, out string strError);

    }
}
