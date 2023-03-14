using Corparques.Datos.Contratos;
using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Nucleo
{
    public class ServicioVuycoom : IServicioVuycoom
    {
        #region properties 
        public readonly IRepositorioVuycoom repoVuycomm;
        #endregion
            #region constructor
            public ServicioVuycoom(IRepositorioVuycoom repo) 
        {

            repoVuycomm = repo;
        }
            #endregion
        public IEnumerable<Factura> facturas()
        {
            var _list = repoVuycomm.facturas();
            return _list;
        }

        public int getNumeroFactuar() 
        {
            return repoVuycomm.getNumeroFactuar();
        }

        public Vyucoom getClienteNuevo(string datoCliente)
        {
            var dato = repoVuycomm.getClienteNuevo(datoCliente);
            return dato;
        }

        /*public string InsertarFactura(string vyucoom)
        {
            return repoVuycomm.InsertarFactura(vyucoom);
        }*/
        public string InsertarFactura(Vyucoom vyucoom)
        {
            return repoVuycomm.InsertarFactura(vyucoom);
        }
        public string InsertarDato(Vyucoom vyucoom)
        {
            var dato = repoVuycomm.InsertarDato(vyucoom);
            return dato;
        }

        public Parametro BuscarPrecioFijo() 
        {
            Parametro _list = repoVuycomm.BuscarPrecioFijo();
            return _list;
        }

        public int pedido(string data)
        {
            return repoVuycomm.pedido(data);
        }
        public string NCliPedido(string data)
        {
            return repoVuycomm.NCliPedido(data);
        }

        public string ModificarPagoReciboCaja(Vyucoom cliente)
        {
            return repoVuycomm.ModificarPagoReciboCaja(cliente);
        }

        public string InsertarRecCaja(Vyucoom data)
        {
            return repoVuycomm.InsertarRecCaja(data);
        }

        public IEnumerable<Vyucoom>[] Reimprimir(string Npedido)
        {
            return repoVuycomm.Reimprimir(Npedido);
        }

        public Vyucoom UsuarioReciboCajaVyucoom(string dato)
        {
            return repoVuycomm.UsuarioReciboCajaVyucoom(dato);
        }

        public string UsuarioPorId(string id)
        {
            return repoVuycomm.UsuarioPorId(id);
        }

        public string PuntoPorId(string id)
        {
            return repoVuycomm.PuntoPorId(id);
        }
    }
}
