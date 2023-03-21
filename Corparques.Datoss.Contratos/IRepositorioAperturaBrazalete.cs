using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{
    public interface IRepositorioAperturaBrazalete : IRepositorioBase<AperturaBrazalete>
    {
        int InsertarAperturaBrazaleteDetalle(AperturaBrazaleteDetalle modelo);
        AperturaBrazalete ObtieneAperturaBrazaleteParaReabastecimiento(AperturaBrazalete brazalete);
        IEnumerable<AperturaBrazalete> ObtenerApeturaBrazalete(int IdSupervisor);
    }
}
