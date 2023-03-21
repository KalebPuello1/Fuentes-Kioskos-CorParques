using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;

namespace CorParques.Datos.Contratos
{
    public interface IRepositorioCodigoFechaAbierta : IRepositorioBase<CodigoFechaAbierta>
    {
        //Ver todas las facturas
        IEnumerable<CodigoFechaAbierta> CodigoFacturaa(int c);

        bool Actualizar(CodigoFechaAbierta modelo, out string error);

        IEnumerable<CodigoFechaAbierta> VerFacturas(string IdDestreza, int? IdAtraccion);

        //RDSH: Retorna un objeto CortesiaPunto para la edicion.
        CodigoFechaAbierta ObtenerPorId(int Id);

        IEnumerable<CodigoFechaAbierta> ObtenerTodos(string CodSap);

        IEnumerable<CortesiaPunto> test(int IdDestreza, int IdAtraccion);

        string EnviarQRCorreo(string correo, string pathQR);
        string EnviarCorreoCodConfirmacion(string correo, string CodConfirma);
        string EnviarUsuario(CodigoFechaAbierta datosAventurita);
        IEnumerable<CodigoFechaAbierta> VerPedidosClienteExterno(string cod);
        string UpdatePedidosClienteExterno(string pedido, int cantidad);
        string InsertarAsignacionBoleta(IEnumerable<CodigoFechaAbierta> FechaAbierta);
        string CorreoAsigancionBoleta(string pedido, int cantidad, string codSapProducto);
        string UpdateFechaBoletas(string consecutivoGenerado, string codsap, string Valor, DateTime? FechaInicial, DateTime? FechaFinal);
        string InsertarBoleteriaCodigosExternos(CodigoFechaAbierta FechaAbierta);
        string ObteneridCliente(string numeroPedido);
        string ObtenerPedidosPorUsuario(string codPedido, string NomUsuario);
        IEnumerable<CodigoFechaAbierta> VerPedidosClienteExternoMultiple(string pedido);
        string updatePedidoUsuarioExterno(string pedido, int idUsuario, string IdSap);
        string verPedidosPorIdCliente(string IdCliente);
        string EnviarUsuarioProductos(CodigoFechaAbierta datosAventurita);
        MemoryStream intentoImgBD(string rtaLogo, string IdClienteSap);
        MemoryStream ObtenerLogoCliente(string IdClienteSap);
        bool EnviarCorreo(string sTo, string sSubject, string sMensaje, MailPriority mpPriority, List<string> attachmentList);


    }
}

