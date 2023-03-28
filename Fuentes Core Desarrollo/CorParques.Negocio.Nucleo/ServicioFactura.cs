using CorParques.Negocio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;
using CorParques.Datos.Contratos;

namespace CorParques.Negocio.Nucleo
{

	public class ServicioFactura : IServicioFactura
	{

        #region Constructor

        private readonly IRepositorioFactura _repositorio;

        public ServicioFactura (IRepositorioFactura repositorio)
        {
            _repositorio = repositorio;
        }

        public bool Actualizar(Factura modelo)
        {
            throw new NotImplementedException();
        }

        public string BorrarFacturaContingencia(List<Factura> _factura)
        {
            return _repositorio.BorrarFacturaContingencia(_factura);
        }

        public Factura Crear(Factura modelo)
        {
            throw new NotImplementedException();
        }

        public Factura Obtener(int id)
        {
            throw new NotImplementedException();
        }

        public DiccionarioContigencia ObtenerDiccionarioContigencia()
        {
            return _repositorio.ObtenerDiccionarioContigencia();
        }

        public IEnumerable<Factura> ObtenerFacturaContingencia()
        {
            return _repositorio.ObtenerFacturaContingencia();
        }

        public IEnumerable<Factura> ObtenerTodos()
        {
            throw new NotImplementedException();
        }

        public Factura ObtenerUltimaFactura(string CodSapPunto)
        {
            return _repositorio.ObtenerUltimaFactura(CodSapPunto);
        }
        public RespuestaTransaccionRedaban ObtenerIdFranquiciaRedeban(string CodFranquicia)
        {
            return _repositorio.ObtenerIdFranquiciaRedeban(CodFranquicia);
        }
        
        public List<Factura> ProcesaFacturaContingencia(IEnumerable<Factura> _factura)
        {
            return _repositorio.ProcesaFacturaContingencia(_factura);
        }

        public string GenerarNumeroFactura(int IdPunto)
        {
            var resultado= _repositorio.GenerarNumeroFactura(IdPunto);
            return resultado;
        }
        public bool FlujoRedebanXPunto(int IdPunto)
        {
            var resultado = _repositorio.FlujoRedebanXPunto(IdPunto);
            return resultado;
        }
        

        public IEnumerable<DetalleFactura> ObtenerDetallesConsecutivoConvenioDia(string consecutivoConvenio)
        {
            return _repositorio.ObtenerDetallesConsecutivoConvenioDia(consecutivoConvenio);
        }

        #endregion
    }
}
