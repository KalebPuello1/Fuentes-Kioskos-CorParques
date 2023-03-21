using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corparques.Datos.Contratos
{
    public interface IRepositorioVuycoom
    {
        IEnumerable<Factura> facturas();

        int getNumeroFactuar();

        Vyucoom getClienteNuevo(string documento);

        //string InsertarFactura(string vyucoom);
        string InsertarFactura(Vyucoom vyucoom);
        IEnumerable<Apertura> apertura();

        string ModificarPagoReciboCaja(Vyucoom cliente);

        string InsertarDato(Vyucoom vyucoom);
        Parametro BuscarPrecioFijo();

        int pedido(string dato);

        string NCliPedido(string data);

        string InsertarRecCaja(Vyucoom data);

        IEnumerable<Vyucoom>[] Reimprimir(string Npedido);

        Vyucoom UsuarioReciboCajaVyucoom(string dato);

        string UsuarioPorId(string id);

        string PuntoPorId(string id);
    }
}
