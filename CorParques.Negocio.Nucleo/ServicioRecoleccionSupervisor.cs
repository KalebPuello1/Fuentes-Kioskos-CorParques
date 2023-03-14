using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;
using CorParques.Datos.Contratos;
using CorParques.Transversales.Util;

namespace CorParques.Negocio.Nucleo
{
    public class ServicioRecoleccionSupervisor : IServicioRecoleccionSupervisor
    {
        
        
        private readonly IRepositorioRecoleccionSupervisor _repositorio;
        private readonly IRepositorioDetalleRecoleccionMonetaria _repositorioDetalleMonetario;
        private readonly IRepositorioDetalleRecoleccionDocumento _repositorioDetalleDocumento;
        private readonly IRepositorioDetalleRecoleccionNovedad _repositorioDetalleNovedad;
        

        public ServicioRecoleccionSupervisor(IRepositorioRecoleccionSupervisor repositorio, IRepositorioDetalleRecoleccionMonetaria repositorioDetalleMonetario, IRepositorioDetalleRecoleccionDocumento repositorioDetalleDocumento, IRepositorioDetalleRecoleccionNovedad repositorioDetalleNovedad)
        {
            _repositorio = repositorio;
            _repositorioDetalleMonetario = repositorioDetalleMonetario;
            _repositorioDetalleDocumento = repositorioDetalleDocumento;
            _repositorioDetalleNovedad = repositorioDetalleNovedad;
        }

        public bool ActualizarRecoleccion(Recoleccion modelo, out string error)
        {
            return _repositorio.ActualizarRecoleccion(modelo, out error);
        }

        public bool ActualizarRecoleccionDocumentos(DetalleRecoleccionDocumento modelo, out string error)
        {
            return _repositorio.ActualizarRecoleccionDocumentos(modelo, out error);
        }

        public bool ActualizarRecoleccionMonetaria(DetalleRecoleccionMonetaria modelo, out string error)
        {
            return _repositorio.ActualizarRecoleccionMonetaria(modelo, out error);
        }
        

        public Recoleccion ObtenerRecoleccionActiva(int IdUsuario, int IdPunto, int IdEstado)
        {
            Recoleccion objRecoleccion = null;
            IEnumerable<DetalleRecoleccionMonetaria> objDetalleRecoleccionMonetaria;
            IEnumerable<DetalleRecoleccionDocumento> objDetalleRecoleccionDocumento;

            try
            {
                objRecoleccion = _repositorio.ObtenerRecoleccionActiva(IdUsuario, IdPunto, IdEstado);

                if (objRecoleccion != null)
                {
                    objDetalleRecoleccionMonetaria = _repositorioDetalleMonetario.ObtenerPorIDRecoleccion(objRecoleccion.IdRecoleccion);
                    objDetalleRecoleccionDocumento = _repositorioDetalleDocumento.ObtenerPorIDRecoleccion(objRecoleccion.IdRecoleccion);

                    if (objDetalleRecoleccionMonetaria != null)
                    {
                        objRecoleccion.RecoleccionBase = objDetalleRecoleccionMonetaria.Where(x => x.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Base);
                        objRecoleccion.RecoleccionCorte = objDetalleRecoleccionMonetaria.Where(x => x.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Corte);
                    }
                    if (objDetalleRecoleccionDocumento != null)
                    {
                        objRecoleccion.RecoleccionVoucher = objDetalleRecoleccionDocumento.Where(x => x.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Voucher);
                        objRecoleccion.RecoleccionDocumentos = objDetalleRecoleccionDocumento.Where(x => x.IdTipoRecoleccion == (int)Enumerador.TipoRecoleccion.Documentos);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return objRecoleccion;
        }

        public bool InsertaObservacion(ObservacionRecoleccion modelo, out string error, out int IdRecoleccion)
        {
            return _repositorio.InsertaObservacion(modelo, out error, out IdRecoleccion);
        }

        public IEnumerable<MediosPagoFactura> ObtenerDocumentos(int intIdUsuario)
        {
            return _repositorio.ObtenerDocumentos(intIdUsuario);
        }

        public bool ActualizarRecoleccionNovedades(DetalleRecoleccionNovedad modelo, out string error)
        {
            return _repositorio.ActualizarRecoleccionNovedades(modelo, out error);
        }

        public IEnumerable<DetalleRecoleccionNovedad> ObtenerNovedadPorIdRecoleccion(int IdRecoleccion)
        {
            return _repositorioDetalleNovedad.ObtenerPorIDRecoleccion(IdRecoleccion);
        }

        public IEnumerable<NovedadArqueo> ObtenerNovedadPorIdUsuario(int IdUsuario)
        {
            return _repositorio.ObtenerNovedadPorIdUsuario(IdUsuario);
        }

        public IEnumerable<DetalleRecoleccion> ObtenerDetalleRecoleccion(int IdApertura, int TipoConsulta)
        {
            return _repositorio.ObtenerDetalleRecoleccion(IdApertura, TipoConsulta);
        }

        public string RegresarEstado(int idEstado, int idApertura)
        {
            return _repositorio.RegresarEstado(idEstado, idApertura);
        }
    }
}
