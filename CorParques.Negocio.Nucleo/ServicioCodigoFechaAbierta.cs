using CorParques.Datos.Contratos;
using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Nucleo
{



    public class ServicioCodigoFechaAbierta : IServicioCodigoFechaAbierta
    {

        #region Declaraciones

        private readonly IRepositorioCodigoFechaAbierta _repositorio;

        #endregion

        #region Constructor
        public ServicioCodigoFechaAbierta(IRepositorioCodigoFechaAbierta repositorio)
        {
            _repositorio = repositorio;
        }
        #endregion


        public bool Actualizar(CodigoFechaAbierta modelo, out string error)
        {
            return _repositorio.Actualizar(modelo, out error);
        }

        public bool Eliminar(CodigoFechaAbierta modelo, out string error)
        {
            throw new NotImplementedException();
        }

        public bool Insertar(CodigoFechaAbierta modelo, out string error)
        {
            throw new NotImplementedException();
        }


        public CodigoFechaAbierta ObtenerPorId(int Id)
        {
            return _repositorio.ObtenerPorId(Id);
        }
        public IEnumerable<CodigoFechaAbierta> VerFacturas(string IdDestreza, int? IdAtraccion)
        {
            return _repositorio.VerFacturas(IdDestreza, IdAtraccion);
        }

        public IEnumerable<CodigoFechaAbierta> ObtenerTodos(string CodSap)
        {
            return _repositorio.ObtenerTodos(CodSap);
        }

        public IEnumerable<CortesiaPunto> test(int IdDestreza, int IdAtraccion)
        {
            return _repositorio.test(IdDestreza, IdAtraccion);
        }

        public string EnviarCorreoCodConfirmacion(string correo, string CodConfirma)
        {
            string correoo = _repositorio.EnviarCorreoCodConfirmacion(correo, CodConfirma);
            return "Metodo no Funciona";
        }
        public string EnviarQRCorreo(string correo, string pathQR)
        {
            string correoo = _repositorio.EnviarQRCorreo(correo, pathQR);
            return "";
        }
        public string EnviarUsuario(CodigoFechaAbierta datosAventurita)
        {
            if (datosAventurita.enviarProductos)
            {
                string correoo = _repositorio.EnviarUsuarioProductos(datosAventurita);
            }
            else
            {
                string correoo = _repositorio.EnviarUsuario(datosAventurita);
            }
            return "";
        }
        public IEnumerable<CodigoFechaAbierta> VerPedidosClienteExterno(string cod)
        {
            var pedidos = _repositorio.VerPedidosClienteExterno(cod);
            return pedidos;
        }
        public string UpdatePedidosClienteExterno(string pedido, int cantidad)
        {
            var pedidos = _repositorio.UpdatePedidosClienteExterno(pedido, cantidad);
            return pedidos;
        }
        public string InsertarAsignacionBoleta(IEnumerable<CodigoFechaAbierta> FechaAbierta)
        {
            var pedidos = _repositorio.InsertarAsignacionBoleta(FechaAbierta);
            return pedidos;
        }
        public string CorreoAsigancionBoleta(string pedido, int cantidad, string codSapProducto)
        {
            _repositorio.CorreoAsigancionBoleta(pedido, cantidad, codSapProducto);
            return "";
        }

        public string UpdateFechaBoletas(string consecutivoGenerado, string codsap, string Valor, DateTime? FechaInicial, DateTime? FechaFinal)
        {
            var dato = _repositorio.UpdateFechaBoletas(consecutivoGenerado, codsap, Valor, FechaInicial, FechaFinal);
            return dato;
        }
        public string InsertarBoleteriaCodigosExternos(CodigoFechaAbierta FechaAbierta)
        {
            var dato = _repositorio.InsertarBoleteriaCodigosExternos(FechaAbierta);
            return dato;
        }
        public string ObteneridCliente(string numeroPedido)
        {
            var dato = _repositorio.ObteneridCliente(numeroPedido);
            return dato;
        }
        public string ObtenerPedidosPorUsuario(string codPedido, string NomUsuario)
        {
            var dato = _repositorio.ObtenerPedidosPorUsuario(codPedido, NomUsuario);
            return dato;
        }
        public CodigoFechaAbierta VerPedidosClienteExternoMultiple(string pedido)
        {

            List<CodigoFechaAbierta> lista = new List<CodigoFechaAbierta>();
            List<List<CodigoFechaAbierta>> listaList = new List<List<CodigoFechaAbierta>>();
            CodigoFechaAbierta codigo = new CodigoFechaAbierta();
            codigo.ListaListFechaAbierta = new List<List<CodigoFechaAbierta>>();
            if (pedido.Contains("|"))
            {
                var list = pedido.Split('|');
                foreach (var item in list)
                {
                    if (item != "")
                    {
                        codigo.ListaCodigoFechaAbierta = null;
                        //Numero del pedido
                        lista = new List<CodigoFechaAbierta>();
                        lista = _repositorio.VerPedidosClienteExternoMultiple(item).ToList();
                        codigo.ListaListFechaAbierta.Add(lista);
                    }
                }
            }
            else
            {
                codigo.ListaListFechaAbierta = null;
                codigo.ListaCodigoFechaAbierta = _repositorio.VerPedidosClienteExternoMultiple(pedido).ToList();
            }
            return codigo;
        }
        public string updatePedidoUsuarioExterno(string pedido, int idUsuario, string IdSap)
        {
            var dato = _repositorio.updatePedidoUsuarioExterno(pedido, idUsuario, IdSap);
            return dato;
        }
        public List<PedidosPorCliente> verPedidosPorIdCliente(string IdCliente)
        {
            var dato = "";
            PedidosPorCliente pedidos = new PedidosPorCliente();
            List<PedidosPorCliente> ListaPedidos = new List<PedidosPorCliente>();
            if (IdCliente.Contains('|'))
            {
                var cod = IdCliente.Split('|');
                foreach (var item in cod)
                {
                    pedidos = new PedidosPorCliente();
                    if (item != "")
                    {
                        //dato += _repositorio.verPedidosPorIdCliente(item) + "|";
                        dato = _repositorio.verPedidosPorIdCliente(item);
                        pedidos.IdCliente = item;
                        pedidos.Pedido = dato;
                        ListaPedidos.Add(pedidos);
                    }
                    else
                    {
                        continue;
                    }
                }
                //return dato;
                return ListaPedidos;
            }
            else
            {
                if (IdCliente != "")
                {
                    dato = _repositorio.verPedidosPorIdCliente(IdCliente);
                    pedidos.IdCliente = IdCliente;
                    pedidos.Pedido = dato;
                    ListaPedidos.Add(pedidos);
                }
                //return dato;
                return ListaPedidos;
            }

        }
        public MemoryStream intentoImgBD(string rtaLogo, string IdClienteSap)
        {
            var dato = _repositorio.intentoImgBD(rtaLogo, IdClienteSap);
            return dato;
        }
        public MemoryStream ObtenerLogoCliente(string IdClienteSap)
        {
            ReportePDF Rpdf = new ReportePDF();
            Rpdf.repositorio("inicio obt logo ");
            var dato = _repositorio.ObtenerLogoCliente(IdClienteSap);
            return dato;
        }

    }
}
