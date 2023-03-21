using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{

    public interface IRepositorioCortesia : IRepositorioBase<Cortesia>
    {
        List<UsuarioVisitanteViewModel> ObtenerCortesiaPorDocumento(string documento, string numTarjeta, string correoApp, string documentoEjecutivo);

        List<ListaCortesia> ListarCortesias();

        List<UsuarioVisitante> ListarObtenerUsuarioVisitanteCortesias(int Idtipocortesia);
        SolicitudCodigo ConsultarCodConfirmacion(int Consecutivo, object tiempoVigenciaCodigo);
        
        List<CategoriaImagenes> ListarCategoriaImagenes();
        List<MensajesVisual> ListarMensajesVisual();
        List<MensajesVisual> ObtenerMensajesVisualXCod(string Codigo);
        string EliminarMensajesVisual(string Codigo);
        string ActualizarMensajesVisual(MensajesVisual modelo);
        string AgregarMensajesVisual(MensajesVisual modelo);

        string SolicitudCodConfirmacion(SolicitudCodigo modelo);
        

        List<VisualPantallas> ListarVisualPantallas(int IdCategoria);

        List<ImagenAdmin> ObtenerImagenAdminXCodpantalla(string CodPantalla);



        int AprobacionCortesia(int IdCortesia);


        string ActualizarAdminImagenes(ImagenAdmin modelo);
        string GuardarCortesiaUsuarioVisitante(GuardarCortesiaUsuarioVisitante modelo);
    }
}
