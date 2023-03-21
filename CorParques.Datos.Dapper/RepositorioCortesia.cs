using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using CorParques.Transversales.Util;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Dapper
{

    public class RepositorioCortesia : RepositorioBase<Cortesia>, IRepositorioCortesia
    {

        public List<UsuarioVisitanteViewModel> ObtenerCortesiaPorDocumento(string documento, string numTarjeta, string correoApp, string documentoEjecutivo)
        {
            int tipoCortesia = 1;
            if (!string.IsNullOrEmpty(documentoEjecutivo))
            {
                tipoCortesia = 5;
            }
            else if (!string.IsNullOrEmpty(numTarjeta))
            {
                tipoCortesia = 2;
            }
            else if (!string.IsNullOrEmpty(correoApp))
            {
                tipoCortesia = 3;
            }

            var cortesia = new List<UsuarioVisitanteViewModel>();
            var rta = _cnn.Query<UsuarioVisitanteViewModel>("SP_ObtenerCortesia", param: new
            {
                Documento = documento,
                NumeroTarjeta = numTarjeta,
                CorreoApp = correoApp,
                TipoCortesia = tipoCortesia,
                DocumentoEjecutivo = documentoEjecutivo
            }, commandType: CommandType.StoredProcedure);
            if (rta != null && rta.Count() > 0)
            {
                string docu = "";
                cortesia = rta.ToList();

                if (tipoCortesia == 5 || tipoCortesia == 1 || tipoCortesia == 2 || tipoCortesia == 3)
                {
                    if (tipoCortesia == 5)
                    {
                        docu = documentoEjecutivo;
                    }
                    if (tipoCortesia == 1)
                    {
                        docu = documento;
                    }
                    var Detallecortesia = new List<DetalleCortesia>();
                    var rtadetalle = _cnn.Query<DetalleCortesia>("SP_ObtenerDetalleCortesias", param: new
                    {
                        NumeroDocumento = docu,
                        NumeroTarjeta = numTarjeta,
                        CorreoApp = correoApp,
                        TipoCortesia = tipoCortesia
                    }, commandType: CommandType.StoredProcedure);
                    if (rtadetalle != null && rtadetalle.Count() > 0)
                    {
                        cortesia.FirstOrDefault().ListDetalleCortesia = rtadetalle.ToList();
                    }
                }



            }

            return cortesia;
        }

        public List<ListaCortesia> ListarCortesias()
        {

            var cortesia = new List<ListaCortesia>();
            var rta = _cnn.Query<ListaCortesia>("SP_ListarCortesias", null, commandType: CommandType.StoredProcedure);
            if (rta != null && rta.Count() > 0)
            {
                string docu = "";
                cortesia = rta.ToList();
            }

            return cortesia;
        }
        public List<UsuarioVisitante> ListarObtenerUsuarioVisitanteCortesias(int Idtipocortesia)
        {

            var cortesia = new List<UsuarioVisitante>();
            var rta = _cnn.Query<UsuarioVisitante>("SP_ListarObtenerUsuarioVisitanteCortesias", param: new
            {
                Idtipocortesia = Idtipocortesia

            }, commandType: CommandType.StoredProcedure);
            if (rta != null && rta.Count() > 0)
            {
                string docu = "";
                cortesia = rta.ToList();
            }

            return cortesia;
        }

        public List<CategoriaImagenes> ListarCategoriaImagenes()
        {

            var categoria = new List<CategoriaImagenes>();
            var rta = _cnn.Query<CategoriaImagenes>("SP_ListarCategoriaImagenes", null, commandType: CommandType.StoredProcedure);
            if (rta != null && rta.Count() > 0)
            {
                string docu = "";
                categoria = rta.ToList();
            }

            return categoria;
        }
        public List<MensajesVisual> ListarMensajesVisual()
        {

            var categoria = new List<MensajesVisual>();
            var rta = _cnn.Query<MensajesVisual>("SP_ListarMensajesVisual", null, commandType: CommandType.StoredProcedure);
            if (rta != null && rta.Count() > 0)
            {
                string docu = "";
                categoria = rta.ToList();
            }

            return categoria;
        }
        public SolicitudCodigo ConsultarCodConfirmacion(int Consecutivo,object tiempoVigenciaCodigo)
        {

            var codigo = new List<SolicitudCodigo>();
            var solicit = new SolicitudCodigo();
            var rta = _cnn.Query<SolicitudCodigo>("SP_ConsultarCodConfirmacion", param: new
            {
                Consecutivo = Consecutivo

            }, commandType: CommandType.StoredProcedure);
            if (rta != null && rta.Count() > 0)
            {
                    solicit = rta.FirstOrDefault();
            }
            else
            {
                solicit.RespuestaPeticion = "Codigo invalido";
            }

            return solicit;
        }

        

        public List<MensajesVisual> ObtenerMensajesVisualXCod(string Codigo)
        {

            var imagenpanta = new List<MensajesVisual>();
            var rta = _cnn.Query<MensajesVisual>("SP_ObtenerMensajesVisualXCod", param: new
            {
                Codigo = Codigo

            }, commandType: CommandType.StoredProcedure);
            if (rta != null && rta.Count() > 0)
            {
                string docu = "";
                imagenpanta = rta.ToList();
            }

            return imagenpanta;
        }
        public string EliminarMensajesVisual(string Codigo)
        {

            int cortesia = 0;
            var rta = _cnn.Query<int>("SP_EliminarMensajesVisual", param: new
            {
                Codigo = Codigo
            }, commandType: CommandType.StoredProcedure);

            return rta.ToString();
        }
        public string ActualizarMensajesVisual(MensajesVisual modelo)
        {

            var rta = _cnn.Query<string>("SP_ActualizarMensajesVisual", param: new
            {
                UsuarioModificacion = modelo.UsuarioModificacion,
                Codigo = modelo.Codigo,
                Texto = modelo.Texto,
                Frecuencia = modelo.Frecuencia

            }, commandType: CommandType.StoredProcedure);
            return rta.ToString();


        }
        public string AgregarMensajesVisual(MensajesVisual modelo)
        {

            var rta = _cnn.Query<string>("SP_AgregarMensajesVisual", param: new
            {
                UsuarioModificacion = modelo.UsuarioModificacion,
                Codigo = modelo.Codigo,
                Texto = modelo.Texto,
                Frecuencia = modelo.Frecuencia

            }, commandType: CommandType.StoredProcedure);
            return rta.ToString();


        }
        public string SolicitudCodConfirmacion(SolicitudCodigo modelo)
        {
           
            var rta = _cnn.Query<string>("SP_AgregarSolicitudCodConfirmacion", param: new
            {
                Correo = modelo.Correo,
                Nombre = modelo.Nombre,
                Consecutivo = modelo.Consecutivo

            }, commandType: CommandType.StoredProcedure);


            return rta.ToString();
        }
        
        public List<VisualPantallas> ListarVisualPantallas(int IdCategoria)
        {

            var codpantallas = new List<VisualPantallas>();
            var rta = _cnn.Query<VisualPantallas>("SP_ListarVisualPantallas", param: new
            {
                IdCategoria = IdCategoria

            }, commandType: CommandType.StoredProcedure);
            if (rta != null && rta.Count() > 0)
            {
                string docu = "";
                codpantallas = rta.ToList();
            }

            return codpantallas;
        }

        public List<ImagenAdmin> ObtenerImagenAdminXCodpantalla(string CodPantalla)
        {

            var imagenpanta = new List<ImagenAdmin>();
            var rta = _cnn.Query<ImagenAdmin>("SP_ObtenerImagenAdminXCodpantalla", param: new
            {
                CodPantalla = CodPantalla

            }, commandType: CommandType.StoredProcedure);
            if (rta != null && rta.Count() > 0)
            {
                string docu = "";
                imagenpanta = rta.ToList();
            }

            return imagenpanta;
        }

        public int AprobacionCortesia(int IdCortesia)
        {

            int cortesia = 0;
            var rta = _cnn.Query<int>("SP_AprobacionCortesia", param: new
            {
                IdCortesia = IdCortesia
            }, commandType: CommandType.StoredProcedure);
            if (rta != null)
            {
                cortesia = rta.FirstOrDefault();
            }

            return cortesia;
        }

        public string ActualizarAdminImagenes(ImagenAdmin modelo)
        {

                var rta = _cnn.Query<string>("SP_ActualizarAdminImagenes", param: new
                {
                    Id = modelo.Id,
                    UsuarioModificacion = modelo.UsuarioModificacion,
                    CodPantalla = modelo.CodPantalla,
                    NombreImagen = modelo.NombreImagen,
                    Frecuencia = modelo.Frecuencia,
                    Ruta = modelo.Ruta

                }, commandType: CommandType.StoredProcedure);
                return rta.ToString();


        }
        public string GuardarCortesiaUsuarioVisitante(GuardarCortesiaUsuarioVisitante modelo)
        {
            var mensaje = string.Empty;
            int tipoCortesia = 1;
            if (modelo.TipoCortesia != 0)
            {
                tipoCortesia = modelo.TipoCortesia;
            }

            if (tipoCortesia == 1 || tipoCortesia == 5)
            {

            }
            if (tipoCortesia != 3)
            {


                var rta = _cnn.Query<string>("SP_GuardarCortesiaUsuarioVisitante", param: new
                {
                    DetalleProducto = Utilidades.convertTable(modelo.ListaProductos.Select(x => new TablaGeneral
                    {
                        col1 = x.IdDetalleProducto.ToString(),
                        col2 = x.CodigoSap,
                        col3 = x.IdProducto.ToString()
                    })),
                    IdUsuario = modelo.idUsuario,
                    Documento = modelo.Documento,
                    TipoCortesia = tipoCortesia,
                    ValorGenerico = modelo.ValorGenerico,
                    IdDetalleCortesia = modelo.IdDetalleCortesia
                }, commandType: CommandType.StoredProcedure);
                return rta.Single();
            }
            else
            {
                var rta = _cnn.Query<string>("SP_GuardarCortesiaUsuarioVisitante", param: new
                {
                    DetalleProducto = Utilidades.convertTable(modelo.ListaProductosAPP.Select(x => new TablaGeneral
                    {
                        col1 = x.IdDetalleCortesia.ToString(),
                        col2 = x.CodigoSap,
                        col3 = x.IdProducto.ToString()
                    })),
                    IdUsuario = modelo.idUsuario,
                    Documento = modelo.Documento,
                    TipoCortesia = tipoCortesia,
                    ValorGenerico = modelo.ValorGenerico
                }, commandType: CommandType.StoredProcedure);

                return rta.Single();
            }


        }
    }
}
