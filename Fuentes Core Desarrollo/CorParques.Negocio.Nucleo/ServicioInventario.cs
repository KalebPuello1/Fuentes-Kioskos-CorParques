using CorParques.Negocio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;
using CorParques.Datos.Contratos;
using CorParques.Transversales.Contratos;
using System.Net.Mail;

namespace CorParques.Negocio.Nucleo
{

    public class ServicioInventario : IServicioInventario
    {

        private readonly IRepositorioInventario _repositorio;
        private readonly IRepositorioProducto _repositorioProducto;
        private readonly IRepositorioUsuario _repositorioUsuario;
        private readonly IRepositorioParametros _repositorioParametros;
        private readonly IEnvioMails _Email;

        #region Constructor
        public ServicioInventario(IRepositorioInventario repositorio, IRepositorioProducto repositorioProducto, IRepositorioParametros repositorioParametros, IEnvioMails email, IRepositorioUsuario repositorioUsuario, IRepositorioCodigoFechaAbierta emailinv)
        {
            _repositorioProducto = repositorioProducto;
            _repositorioParametros = repositorioParametros;
            _Email = email;
            _repositorioUsuario = repositorioUsuario;
			_repositorio = repositorio;
		}

        public bool Actualizar(TransladoInventario modelo)
        {
            throw new NotImplementedException();
        }

        public string ActualizarInventario(Inventario Inventario)
        {
            return _repositorio.ActualizarInventario(Inventario);
        }

        public string EntregaPedido(IEnumerable<TransladoInventario> Inventario)
        {

            var rta = _repositorio.RegistrarSalidaPedido(Inventario.First().IdUsuarioRegistro, Inventario.First().Pedido, Inventario.First().idUsuario);
            if (string.IsNullOrEmpty(rta))
            {
                var tiposProductosBoletas = _repositorioParametros.ObtenerParametroPorNombre("CodigoSapTipoProductoBoleterias").Valor.Split(',').ToList();
                tiposProductosBoletas.Add(_repositorioParametros.ObtenerParametroPorNombre("TarjetasRecargablesPOS").Valor);
                var codigos = Inventario.Select(x => x.CodSapMaterial).ToArray();
                var productosTrasladados = _repositorioProducto.ObtenerLista($"WHERE CodigoSap in ('{string.Join("','", codigos)}')");
                if (productosTrasladados.Count() > 0)
                {
                    var correo = _repositorioUsuario.Obtener(Inventario.First().idUsuario).Correo;
                    if (string.IsNullOrWhiteSpace(correo))
                        correo = _repositorioParametros.ObtenerParametroPorNombre("CorreoTrasladosBoleteria").Valor;
                    var prod = "";
                    foreach (var item in productosTrasladados)
                    {
                        prod += $"{item.Nombre}: {Inventario.First(x => x.CodSapMaterial.Equals(item.CodigoSap)).Cantidad}<br/>";
                    }
                    _Email.EnviarCorreo(correo, "Entrega de pedido",
                        $"Se ha realizado la entrega del pedido '{Inventario.First().Pedido}', con los siguientes productos.<br/><br/>{prod}", System.Net.Mail.MailPriority.Normal, null);

                }
            }
            return rta;
        }


        public string TrasladoPedido(IEnumerable<TransladoInventario> TransladoInventario)
        {
            var rta = ActualizarTransladoInventario(TransladoInventario);
            if (string.IsNullOrEmpty(rta))
                _repositorio.ActualizarPedidoTrasladado(TransladoInventario.First().IdPuntoDestino, TransladoInventario.First().Pedido);
            return rta;
        }

        public string ActualizarTransladoInventario(IEnumerable<TransladoInventario> TransladoInventario)
        {
            var rta = _repositorio.ActualizarTransladoInventario(TransladoInventario);
            if (string.IsNullOrEmpty(rta))
            {
                var tiposProductosBoletas = _repositorioParametros.ObtenerParametroPorNombre("CodigoSapTipoProductoBoleterias").Valor.Split(',').ToList();
                tiposProductosBoletas.Add(_repositorioParametros.ObtenerParametroPorNombre("TarjetasRecargablesPOS").Valor);
                var codigos = TransladoInventario.Select(x => x.CodSapMaterial).ToArray();
                //var productosTrasladados = _repositorioProducto.ObtenerLista($"WHERE CodigoSap in ('{string.Join("','", codigos)}') and CodSapTipoProducto in({string.Join(",", tiposProductosBoletas)})");
                var productosTrasladados = _repositorioProducto.ObtenerListaa(codigos, tiposProductosBoletas);
                if (productosTrasladados.Count() > 0)
                {
                    var correo = _repositorioUsuario.Obtener(TransladoInventario.First().idUsuario).Correo;
                    if (string.IsNullOrWhiteSpace(correo))
                        correo = _repositorioParametros.ObtenerParametroPorNombre("CorreoTrasladosBoleteria").Valor;
                    var prod = "";
                    foreach (var item in productosTrasladados)
                    {
                        prod += $"{item.Nombre}: {TransladoInventario.First(x => x.CodSapMaterial.Equals(item.CodigoSap)).Cantidad}<br/>";
                    }
                    _Email.EnviarCorreo(correo, "Traslado de boleteria",
                        $"Se ha realizado el traslado de los siguientes productos.<br/><br/>{prod}", System.Net.Mail.MailPriority.Normal, null);

                }
            }
            return rta;
        }

        public TransladoInventario Crear(TransladoInventario modelo)
        {
            throw new NotImplementedException();
        }

        public TransladoInventario Obtener(int id)
        {
            return _repositorio.Obtener(id);
        }

        public IEnumerable<TransladoInventario> ObtenerTodos()
        {
            return _repositorio.ObtenerLista();
        }

        public string InsertarAjusteInventario(IEnumerable<AjustesInventario> Ajustes)
        {
            string resp = "";

            try
            {
                DateTime fechaHoraActual = DateTime.Now;
                foreach (var item in Ajustes)
                {
                    string sapAlmacen = ObtenerCodSapAlmacen(item.IdPunto);

                    item.FechaAjuste = fechaHoraActual;
                    item.FechaRegistro = fechaHoraActual;
                    item.CodSapAlmacen = sapAlmacen;
                    item.Id = _repositorio.InsertarAjusteInventario(item);
                }
            }
            catch (Exception ex)
            {
                resp = ex.Message;
            }


            return resp;
        }

        public IEnumerable<TipoAjusteInventario> ObtenerTiposAjuste()
        {
            return _repositorio.ObtenerTiposAjuste();
        }

        public IEnumerable<MotivosInventario> ObtenerMotivosAjuste(string CodSapAjuste)
        {
            return _repositorio.ObtenerMotivosAjuste(CodSapAjuste);
        }
        public IEnumerable<MotivosInventario> BuscarMotivosInventario()
        {
            return _repositorio.BuscarMotivosInventario();
        }
        public string ActualizarAjustesInventario(IEnumerable<AjustesInventario> AjusteInventario)
        {
            return _repositorio.ActualizarAjustesInventario(AjusteInventario);
        }

        public IEnumerable<ResumenCierre> ObtenerResumenCierre(DateTime? Fecha)
        {
            return _repositorio.ObtenerResumenCierre(Fecha);
        }

        public IEnumerable<MotivosInventario> ObtenerTodosMotivos()
        {
            return _repositorio.ObtenerTodosMotivos();
        }

        public string ObtenerCodSapAlmacen(int idPunto)
        {
            return _repositorio.ObtenerCodSapAlmacen(idPunto);
        }
        public IEnumerable<SolicitudRetorno> ConsultarPedidoRetorno(string pedido)
        {
            return _repositorio.ConsultarPedidoRetorno(pedido);
        }
        public IEnumerable<SolicitudRetorno> ObtenerSolicitudesDevolucion()
        {
            return _repositorio.ObtenerSolicitudesDevolucion();
        }
        public IEnumerable<ProductosPedidos> ObtenerPedidosTraslado()
        {
            return _repositorioProducto.ObtenerPedidosTraslado();
        }
        public IEnumerable<SolicitudRetorno> ObtenerPedidosEntregaAsesor(int idPunto)
        {
            return _repositorioProducto.ObtenerPedidosEntregaAsesor(idPunto);
        }

        public IEnumerable<TipoGeneral> ConsultarMotivosRetorno()
        {
            return _repositorio.ConsultarMotivosRetorno();
        }
        public string CrearSolicitudRetorno(SolicitudRetorno modelo)
        {
            return _repositorio.CrearSolicitudRetorno(modelo);
        }
        public bool enviarMail(string to, string subject, string mensaje, MailPriority mpPriority, List<string> attachment) {
          bool envio;
          return  envio = _Email.EnviarCorreo(to, subject, $"Gracias por la atención prestada.<br/><br/>{mensaje}", System.Net.Mail.MailPriority.High, attachment);
        }

        #endregion
    }
}
