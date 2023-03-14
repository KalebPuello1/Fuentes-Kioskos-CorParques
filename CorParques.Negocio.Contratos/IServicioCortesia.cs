using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{
    public interface IServicioCortesia
    {
        IEnumerable<Cortesia> ObtenerTodosCortesia();
        int InsertarCortesia(ref Cortesia cortesiaPQRS);
        bool EliminarCortesia(Cortesia cortesiaPQRS);
        bool ActualizarCortesia(ref Cortesia cortesiaPQRS);
        Cortesia ObtenerCortesia(int idCortesiaPQRS);
        IEnumerable<UsuarioVisitante> ObtenerTodosUsuarioVisitante();
        IEnumerable<ComplejidadCortesia> ObtenerComplejidadCortesia();
        IEnumerable<PlazoCortesia> ObtenerPlazoCortesia();
        IEnumerable<TipoRedencionCortesia> ObtenerTipoRedencionCortesia();
        
        int InsertarUsuarioVisitante(ref UsuarioVisitanteViewModel usuarioVisitante);
        bool EliminarUsuarioVisitante(UsuarioVisitante usuarioVisitante);
        bool ActualizarUsuarioVisitante(ref UsuarioVisitante usuarioVisitante);
        UsuarioVisitante ObtenerUsuarioVisitante(int idUsuarioVisitante);
        UsuarioVisitanteViewModel ObtenerCortesiaUsuarioVisitante(string documento, string numTarjeta, string correoApp, string documentoEjecutivo);

        IEnumerable<ListaCortesia> ListarCortesias();
        IEnumerable<UsuarioVisitante> ListarObtenerUsuarioVisitanteCortesias(int Idtipocortesia);
        SolicitudCodigo ConsultarCodConfirmacion(int Consecutivo,object tiempoVigenciaCodigo);
        
        IEnumerable<CategoriaImagenes> ListarCategoriaImagenes();

        IEnumerable<MensajesVisual> ListarMensajesVisual();
        IEnumerable<MensajesVisual> ObtenerMensajesVisualXCod(string Codigo);
        string EliminarMensajesVisual(string Codigo);
        string ActualizarMensajesVisual(MensajesVisual modelo);
        string AgregarMensajesVisual(MensajesVisual modelo);
        string SolicitudCodConfirmacion(SolicitudCodigo modelo);
        
        IEnumerable<VisualPantallas> ListarVisualPantallas(int IdCategoria);

        IEnumerable<ImagenAdmin> ObtenerImagenAdminXCodpantalla(string CodPantalla);

        int AprobacionCortesia(int IdCortesia);

        string ActualizarAdminImagenes(ImagenAdmin modelo);
        
        string GuardarCortesiaUsuarioVisitante(GuardarCortesiaUsuarioVisitante modelo);

    }
}
