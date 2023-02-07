using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;


namespace CorParques.Datos.Contratos
{
    public interface IRepositorioRecambio
    {
        bool InsertarRecambio(Recambio modelo, out string error, out int IdRecambio);
    }
}
