using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{
    public interface IServicioVuycoom
    {
        IEnumerable<Factura> facturas();

        int getNumeroFactuar();

        //string InsertarFactura(string vyucoom);
        string InsertarFactura(Vyucoom vyucoom);
        string InsertarDato(Vyucoom vyucoom);

        Vyucoom getClienteNuevo(string datoCliente);

        string ModificarPagoReciboCaja(Vyucoom cliente);

        Parametro BuscarPrecioFijo();

        int pedido(string pedido);

        string NCliPedido(string data);

        string InsertarRecCaja(Vyucoom data);

        IEnumerable<Vyucoom>[] Reimprimir(string Npedido);

        Vyucoom UsuarioReciboCajaVyucoom(string dato);

        string UsuarioPorId(string id);

        string PuntoPorId(string id);
    }
}
