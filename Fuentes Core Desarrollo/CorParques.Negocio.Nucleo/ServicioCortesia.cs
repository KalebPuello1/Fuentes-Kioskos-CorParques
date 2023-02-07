using CorParques.Datos.Contratos;
using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;
using CorParques.Transversales.Contratos;
using CorParques.Transversales.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace CorParques.Negocio.Nucleo
{
    public class ServicioCortesia : IServicioCortesia
    {

        IRepositorioUsuarioVisitante _repositorioUsuario;

        IRepositorioCortesia _repositorioCortesia;

        IRepositorioParametros _repositorioParametro;

        IRepositorioPlazoCortesia _repositorioPlazoCortesia;
        IRepositorioComplejidadCortesia _repositorioComplejidadCortesia;
        IRepositorioTipoRedencionCortesia _repositorioTipoRedencionCortesia;


        public ServicioCortesia(IRepositorioUsuarioVisitante repositorioUsuario, IRepositorioCortesia repositorioCortesia, IRepositorioParametros repositorioParametro, IRepositorioPlazoCortesia repositorioPlazoCortesia, IRepositorioComplejidadCortesia repositorioComplejidadCortesia, IRepositorioTipoRedencionCortesia repositorioTipoRedencionCortesia)
        {
            _repositorioUsuario = repositorioUsuario;
            _repositorioCortesia = repositorioCortesia;
            _repositorioParametro = repositorioParametro;
            _repositorioPlazoCortesia = repositorioPlazoCortesia;
            _repositorioComplejidadCortesia = repositorioComplejidadCortesia;
            _repositorioTipoRedencionCortesia = repositorioTipoRedencionCortesia;
        }

        public IEnumerable<UsuarioVisitante> ObtenerTodosUsuarioVisitante()
        {
            return _repositorioUsuario.ObtenerLista();
        }
        public bool ActualizarUsuarioVisitante(ref UsuarioVisitante usuarioVisitante)
        {
            return _repositorioUsuario.Actualizar(ref usuarioVisitante);
        }
        public bool EliminarUsuarioVisitante(UsuarioVisitante usuarioVisitante)
        {
            return _repositorioUsuario.Eliminar(usuarioVisitante);
        }
        public int InsertarUsuarioVisitante(ref UsuarioVisitanteViewModel usuarioVisitante)
        {
            string _mesesRedemirCortesia = _repositorioParametro.ObtenerParametroPorNombre("MesesCortesiaPQRS").Valor;
            return _repositorioUsuario.InsertarUsuarioVisitante(usuarioVisitante, Convert.ToInt32(_mesesRedemirCortesia));
        }

        public UsuarioVisitante ObtenerUsuarioVisitante(int idUsuarioVisitante)
        {
            var usuarioVisitante = _repositorioUsuario.Obtener(idUsuarioVisitante);
            return usuarioVisitante;
        }

        public IEnumerable<Cortesia> ObtenerTodosCortesia()
        {
            return _repositorioCortesia.ObtenerLista();
        }
        public IEnumerable<PlazoCortesia> ObtenerPlazoCortesia()
        {
            return _repositorioPlazoCortesia.ObtenerLista();
        }
        public IEnumerable<ComplejidadCortesia> ObtenerComplejidadCortesia()
        {
            return _repositorioComplejidadCortesia.ObtenerLista();
        }
        public IEnumerable<TipoRedencionCortesia> ObtenerTipoRedencionCortesia()
        {
            return _repositorioTipoRedencionCortesia.ObtenerLista();
        }
        public bool ActualizarCortesia(ref Cortesia cortesiaPQRS)
        {
            return _repositorioCortesia.Actualizar(ref cortesiaPQRS);
        }
        public bool EliminarCortesia(Cortesia cortesiaPQRS)
        {
            return _repositorioCortesia.Eliminar(cortesiaPQRS);
        }
        public int InsertarCortesia(ref Cortesia cortesiaPQRS)
        {
            return _repositorioCortesia.Insertar(ref cortesiaPQRS);
        }
        public Cortesia ObtenerCortesia(int idCortesiaPQRS)
        {
            var cortesiaPQR = _repositorioCortesia.Obtener(idCortesiaPQRS);
            return cortesiaPQR;
        }

        public IEnumerable<ListaCortesia> ListarCortesias()
        {
            return _repositorioCortesia.ListarCortesias();
        }

        public IEnumerable<UsuarioVisitante> ListarObtenerUsuarioVisitanteCortesias(int Idtipocortesia)
        {
            return _repositorioCortesia.ListarObtenerUsuarioVisitanteCortesias(Idtipocortesia);
        }
        public SolicitudCodigo ConsultarCodConfirmacion(int Consecutivo, object tiempoVigenciaCodigo)
        {
            return _repositorioCortesia.ConsultarCodConfirmacion(Consecutivo,  tiempoVigenciaCodigo);
        }
        
        public IEnumerable<CategoriaImagenes> ListarCategoriaImagenes()
        {
            return _repositorioCortesia.ListarCategoriaImagenes();
        }
        public IEnumerable<MensajesVisual> ListarMensajesVisual()
        {
            return _repositorioCortesia.ListarMensajesVisual();
        }
        public IEnumerable<MensajesVisual> ObtenerMensajesVisualXCod(string Codigo)
        {
            return _repositorioCortesia.ObtenerMensajesVisualXCod(Codigo);
        }
        public string EliminarMensajesVisual(string Codigo)
        {
            return _repositorioCortesia.EliminarMensajesVisual(Codigo);
        }
        public string ActualizarMensajesVisual(MensajesVisual modelo)
        {
            return _repositorioCortesia.ActualizarMensajesVisual(modelo);
        }
        public string AgregarMensajesVisual(MensajesVisual modelo)
        {
            return _repositorioCortesia.AgregarMensajesVisual(modelo);
        }
        public string SolicitudCodConfirmacion(SolicitudCodigo modelo)
        {
            return _repositorioCortesia.SolicitudCodConfirmacion(modelo);
        }

        

        public IEnumerable<VisualPantallas> ListarVisualPantallas(int IdCategoria)
        {
            return _repositorioCortesia.ListarVisualPantallas(IdCategoria);
        }
        public IEnumerable<ImagenAdmin> ObtenerImagenAdminXCodpantalla(string CodPantalla)
        {
            return _repositorioCortesia.ObtenerImagenAdminXCodpantalla(CodPantalla);
        }



        public int AprobacionCortesia(int IdCortesia)
        {
            return _repositorioCortesia.AprobacionCortesia(IdCortesia);
        }
        public UsuarioVisitanteViewModel ObtenerCortesiaUsuarioVisitante(string documento, string numTarjeta, string correoApp, string documentoEjecutivo)
        {
            List<UsuarioVisitanteViewModel> listaUsuariosCortesias= _repositorioCortesia.ObtenerCortesiaPorDocumento(documento, numTarjeta, correoApp, documentoEjecutivo);
            UsuarioVisitanteViewModel usuarioCortesia = new UsuarioVisitanteViewModel();
            if (listaUsuariosCortesias != null && listaUsuariosCortesias.Count>0) 
            {
                usuarioCortesia.Activo = listaUsuariosCortesias.First().Activo;
                usuarioCortesia.Apellidos = listaUsuariosCortesias.First().Apellidos;
                usuarioCortesia.Nombres = listaUsuariosCortesias.First().Nombres;
                usuarioCortesia.ArchivoSoporte = listaUsuariosCortesias.First().ArchivoSoporte;
                usuarioCortesia.Cantidad = listaUsuariosCortesias.Select(x => x.Cantidad).Sum();
                usuarioCortesia.Correo = listaUsuariosCortesias.First().Correo;
                usuarioCortesia.TipoDocumento = listaUsuariosCortesias.First().TipoDocumento;
                usuarioCortesia.NumeroDocumento = listaUsuariosCortesias.First().NumeroDocumento;
                usuarioCortesia.IdTipoCortesia = listaUsuariosCortesias.First().IdTipoCortesia;
                usuarioCortesia.Observacion = listaUsuariosCortesias.First().Observacion;
                usuarioCortesia.NumTarjetaFAN = listaUsuariosCortesias.First().NumTarjetaFAN;
                usuarioCortesia.ListDetalleCortesia = listaUsuariosCortesias.First().ListDetalleCortesia;
                usuarioCortesia.Telefono = listaUsuariosCortesias.First().Telefono;
                usuarioCortesia.DescripcionBeneficioFAN = listaUsuariosCortesias.First().DescripcionBeneficioFAN;
                usuarioCortesia.FechaInicial = listaUsuariosCortesias.First().FechaInicial;
                usuarioCortesia.FechaFinal = listaUsuariosCortesias.First().FechaFinal;
               
            }
            return usuarioCortesia;
        }

        public string GuardarCortesiaUsuarioVisitante(GuardarCortesiaUsuarioVisitante modelo)
        {
            return _repositorioCortesia.GuardarCortesiaUsuarioVisitante(modelo);
        }
        public string ActualizarAdminImagenes(ImagenAdmin modelo)
        {
            return _repositorioCortesia.ActualizarAdminImagenes(modelo);
        }
        
    }
}
