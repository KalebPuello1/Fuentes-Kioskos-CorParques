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

	public class ServicioAperturaBase : IServicioAperturaBase
	{

		private readonly IRepositorioAperturaBase _repositorio;
        private readonly IRepositorioApertura _repositorioApertura;
        private readonly IRepositorioTipoDenominacion _repositorioDenominacion;
        private readonly IRepositorioPuntos _repositorioPunto;
        private readonly IRepositorioAperturaBrazalete _repositorioBrazalete;

        #region Constructor
        public ServicioAperturaBase (IRepositorioAperturaBase repositorio, IRepositorioApertura repositorio2
            , IRepositorioTipoDenominacion respositorioDenominacion, IRepositorioPuntos repositorioPunto, IRepositorioAperturaBrazalete repositorioBrazalete)
		{

			_repositorio = repositorio;
            _repositorioApertura = repositorio2;
            _repositorioDenominacion = respositorioDenominacion;
            _repositorioPunto = repositorioPunto;
            _repositorioBrazalete = repositorioBrazalete;

        }

        public string ActualizarAperturaBase(Apertura apertura)
        {
            return _repositorio.ActualizarAperturaBase(apertura);
        }

        public string InsertarAperturaBase(Apertura apertura)
        {
            return _repositorio.InsertarAperturaBase(apertura); 
        }

        public IEnumerable<AperturaBase> ObtenerAperturaBase(int IdPunto, DateTime? Fecha = null)
        {
            return _repositorio.ObtenerAperturaBase(IdPunto, Fecha);
        }

        bool IServicioBase<AperturaBase>.Actualizar(AperturaBase modelo)
        {
            throw new NotImplementedException();
        }

        AperturaBase IServicioBase<AperturaBase>.Crear(AperturaBase modelo)
        {
            throw new NotImplementedException();
        }

        AperturaBase IServicioBase<AperturaBase>.Obtener(int id)
        {
            throw new NotImplementedException();
        }

        IEnumerable<AperturaBase> IServicioBase<AperturaBase>.ObtenerTodos()
        {
            throw new NotImplementedException();
        }

        public DetalleInventario ObtenerDetalleInventario(string Puntos)
        {
            DetalleInventario _detalle = new DetalleInventario();
            _detalle.TipoDenomicacionMoneda = _repositorioDenominacion.ObtenerTodosActivos();

            List<Apertura> _list = new List<Apertura>();
            foreach (var id in Puntos.Split(','))
            {
                var obj = new Apertura();
                var AperturaBase = ObtenerAperturaBase(Convert.ToInt32(id));
                if (AperturaBase != null && AperturaBase.Count() > 0)
                {
                    obj = _repositorioApertura.Obtener(AperturaBase.Select(x => x.IdApertura).FirstOrDefault());
                    obj.AperturaBase = AperturaBase ?? new List<AperturaBase>();
                    obj.ListaPuntos = new List<Entidades.Puntos>() { _repositorioPunto.Obtener(Convert.ToInt32(id)) };
                    obj.AperturaElemento = _repositorioApertura.ObtenerElementosPorIdPunto(Convert.ToInt32(id), DateTime.Now.Date) ?? new List<AperturaElementos>();
                    obj.AperturaBrazalete = _repositorioBrazalete.ObtenerApeturaBrazalete(obj.IdSupervisor)?? new List<AperturaBrazalete>();
                    foreach (var item in obj.AperturaBrazalete)
                    {
                        item.BrazaleteDetalle = new AperturaBrazaleteDetalle();
                    }
                    obj.TiposDenominacion = _detalle.TipoDenomicacionMoneda;
                    _list.Add(obj);
                }
                else {
                    int idApertura = _repositorioApertura.ObtenerIdAperturaPorPunto(Convert.ToInt32(id));
                    obj = _repositorioApertura.Obtener(idApertura);
                    obj = obj == null ? new Apertura() : obj;
                    obj.AperturaBase = new List<AperturaBase>();
                    obj.ListaPuntos = new List<Entidades.Puntos>() { _repositorioPunto.Obtener(Convert.ToInt32(id)) };
                    obj.AperturaElemento = _repositorioApertura.ObtenerElementosPorIdPunto(Convert.ToInt32(id), DateTime.Now.Date) ?? new List<AperturaElementos>();
                    obj.AperturaBrazalete = _repositorioBrazalete.ObtenerApeturaBrazalete(obj.IdSupervisor) ?? new List<AperturaBrazalete>();
                    foreach (var item in obj.AperturaBrazalete)
                    {
                        item.BrazaleteDetalle = new AperturaBrazaleteDetalle();
                    }
                    obj.TiposDenominacion = _detalle.TipoDenomicacionMoneda;
                    _list.Add(obj);
                }
            }

            
            _detalle.Apertura = _list.AsEnumerable();

            return _detalle;
        }

        public IEnumerable<AperturaBrazalete> ObtenerApeturaBrazalete(int IdSupervisor)
        {
           var rta =  _repositorioBrazalete.ObtenerApeturaBrazalete(IdSupervisor) ?? new List<AperturaBrazalete>();

            foreach (var item in rta)
            {
                item.BrazaleteDetalle = new AperturaBrazaleteDetalle();
            }
            return rta;

        }
        #endregion
    }
}
