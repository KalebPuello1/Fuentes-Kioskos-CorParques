using System;
using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CorParques.Negocio.Contratos
{
    public interface IServicioCodigoFechaAbierta

    {
        //RDSH: Inserta una CortesiaPunto.
        bool Insertar(CodigoFechaAbierta modelo, out string error);


        //RDSH: Actualiza una CortesiaPunto.
        bool Actualizar(CodigoFechaAbierta modelo, out string error);

        //RDSH: borrado logico CortesiaPunto.
        bool Eliminar(CodigoFechaAbierta modelo, out string error);

        //RDSH: Retorna una lista de CortesiaPunto donde se puede filtrar por IdDestreza o por IdAtraccion o para traer todas enviar cero en ambos parametros.
        IEnumerable<CodigoFechaAbierta> VerFacturas(string IdDestreza, int? IdAtraccion);

        //RDSH: Retorna un objeto CortesiaPunto para la edicion.
        CodigoFechaAbierta ObtenerPorId(int Id);

        IEnumerable<CortesiaPunto> test(int IdDestreza, int IdAtraccion);

        IEnumerable<CodigoFechaAbierta> ObtenerTodos(string CodSap);

        string EnviarCorreoCodConfirmacion(string correo, string CodConfirma);
        string EnviarQRCorreo(string correo, string pathQR);
        string EnviarUsuario(CodigoFechaAbierta datosAventurita);
        IEnumerable<CodigoFechaAbierta> VerPedidosClienteExterno(string cod);
        CodigoFechaAbierta VerPedidosClienteExternoMultiple(string pedido);
        string updatePedidoUsuarioExterno(string pedido, int idUsuario, string IdSap);
        //string VerPedidosClienteExternoMultiple(string pedido);
        List<PedidosPorCliente> verPedidosPorIdCliente(string IdCliente);
        string UpdatePedidosClienteExterno(string pedido, int cantidad);
        string InsertarAsignacionBoleta(IEnumerable<CodigoFechaAbierta> FechaAbierta);
        string CorreoAsigancionBoleta(string pedido, int cantidad, string codSapProducto);
        string UpdateFechaBoletas(string consecutivoGenerado, string codsap, string Valor, DateTime? FechaInicial, DateTime? FechaFinal);
        string InsertarBoleteriaCodigosExternos(CodigoFechaAbierta FechaAbierta);
        string ObteneridCliente(string numeroPedido);
        string ObtenerPedidosPorUsuario(string idUsuario, string NomUsuario);
        MemoryStream intentoImgBD(string rtaLogo, string IdClienteSap);
        MemoryStream ObtenerLogoCliente(string IdClienteSap);
    }
}

