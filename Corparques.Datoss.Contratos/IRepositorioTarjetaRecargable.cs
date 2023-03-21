using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{

	public interface IRepositorioTarjetaRecargable 
    {       

        string ValidarDocumento(string doc);
        string ConsultarVencimientoTarjeta(string Tarjeta);
    }
}
