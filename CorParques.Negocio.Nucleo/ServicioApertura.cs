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

	public class ServicioApertura : IServicioApertura
	{

		private readonly IRepositorioApertura _repositorio;
        private readonly IRepositorioPuntos _repositoriopuntos;
        private readonly IRepositorioAperturaBrazalete _repositorioBrazalete;

        #region Constructor
        public ServicioApertura (IRepositorioApertura repositorio, IRepositorioPuntos repositoriopuntos
                    ,IRepositorioAperturaBrazalete repositorioBrazalete)
		{
            _repositoriopuntos = repositoriopuntos;
            _repositorio = repositorio;
            _repositorioBrazalete = repositorioBrazalete;

        }
        #endregion

        public bool Actualizar(Apertura modelo)
        {
            throw new NotImplementedException();
        }

        public Apertura Crear(Apertura modelo)
        {
            throw new NotImplementedException();
        }

        public Apertura Obtener(int id)
        {
            return _repositorio.Obtener(id);
        }

        public IEnumerable<Puntos> ObtenerPuntosConApertura(DateTime? Fecha = null)
        {
            return _repositorio.ObtenerPuntosConApertura(Fecha);
        }
        
        public IEnumerable<Puntos> ObtenerPuntosParaAperturaElementos()
        {
            return _repositorio.ObtenerPuntosParaAperturaElementos();
        }

        public IEnumerable<Puntos> ObtenerPuntosSinApertura(DateTime? Fecha = null)
        {           
            return _repositorio.ObtenerPuntosSinApertura(Fecha);
        }

        public IEnumerable<Puntos> ObtenerPuntosAperturaEnProceso()
        {
            return _repositorio.ObtenerPuntosAperturaEnProceso();
        }

        public IEnumerable<Apertura> ObtenerTodos()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TipoGeneral> ObtenerTipoElementos()
        {
            return _repositorio.ObtenerTipoElementos();
        }

        public bool InsertElementos(AperturaElementosInsert modelo)
        {
            bool resp = false;

            int IdHeader = _repositorio.ObtieneIdAperturaElementoHeader(modelo.IdPunto, DateTime.Parse(modelo.Fecha));
            if (IdHeader == 0)
            {
                AperturaElementosHeader objHeader = new AperturaElementosHeader();
                objHeader.IdPunto = modelo.IdPunto;
                objHeader.Fecha = DateTime.Parse(modelo.Fecha);
                objHeader.IdEstado = 6;
                objHeader.UsuarioCreado = modelo.IdUsuario;
                objHeader.FechaCreado = DateTime.Now;
                IdHeader = _repositorio.InsertarAperturaElementoHeader(objHeader);
            }

            /***************************************/
            //int IdApertura = _repositorio.ObtenerIdAperturaPorPunto(IdPunto);
            int IdApertura = 0;
            foreach (AperturaElementos item in modelo.Elementos)
            {

                if (item.Id == -1)
                {
                    item.IdApertura = IdApertura;
                    item.IdEstado = 1;
                    item.IdAperturaElementosHeader = IdHeader;
                    int NewIdResp = _repositorio.InsertarAperturaElemento(item);
                    if (NewIdResp == 0)
                    {
                        resp = false;
                        break;
                    }
                    else
                    {
                        resp = true;
                        item.Id = NewIdResp;
                    }
                }
                else {
                    //Inactivar
                    _repositorio.EliminarElementoPorIdAperturaElemento(item);
                }
            }
            return resp;
        }

        public bool EliminarElementoPorIdAperturaElemento(AperturaElementos modelo)
        {
            return _repositorio.EliminarElementoPorIdAperturaElemento(modelo);
        }


        public IEnumerable<AperturaElementos> ElementosPorIdPunto(int IdPunto, DateTime Fecha)
        {
            IEnumerable<AperturaElementos> listaRet = new List<AperturaElementos>().ToArray();
            listaRet = _repositorio.ObtenerElementosPorIdPunto(IdPunto, Fecha);
            //int IdApertura = _repositorio.ObtenerIdAperturaPorPunto(IdPunto);
            //if (IdApertura > 0)
            //{
            //    listaRet = _repositorio.ObtenerElementosPorIdPunto(IdPunto, Fecha);
            //}

            return listaRet;
        }

        public bool InsertarAperturaBrazalete(List<AperturaBrazalete> modelo)
        {
            AperturaBrazalete _apertura = new AperturaBrazalete();

            foreach (var item in modelo)
            {
                _apertura = item;
                _repositorioBrazalete.Insertar(ref _apertura);
            }

            return true;
        }

        public bool ActualizarAperturaBrazalete(List<AperturaBrazalete> modelo)
        {
            AperturaBrazalete _apertura = new AperturaBrazalete();

            foreach (var item in modelo)
            {
                AperturaBrazalete brazaleteConsultado = _repositorioBrazalete.ObtieneAperturaBrazaleteParaReabastecimiento(item);
                if (brazaleteConsultado == null)
                {
                    //Si no existe el brazalete lo inserta
                    _apertura = item;
                    _repositorioBrazalete.Insertar(ref _apertura);
                }
                else
                {
                    //Si existe el brazalete actualiza el registro
                    brazaleteConsultado.CantidadFinal += item.CantidadInicial;
                    _repositorioBrazalete.Actualizar(ref brazaleteConsultado);
                }
            }

            return true;
        }


        public bool InsertarAperturaBrazaleteDetalle(AperturaBrazaleteDetalle modelo)
        {
            bool rta = _repositorioBrazalete.InsertarAperturaBrazaleteDetalle(modelo) > 0;
            var AperturaBrazalete = _repositorioBrazalete.Obtener(modelo.IdAperturaBrazalete);
            int cantidadFinal = AperturaBrazalete.CantidadFinal - modelo.Cantidad;
            AperturaBrazalete.CantidadFinal = cantidadFinal > 0 ? cantidadFinal : 0;
            _repositorioBrazalete.Actualizar(ref AperturaBrazalete);

            return rta;
        }

        public AperturaBrazalete ObtenerAperturaBrazalete(int id)
        {
            return _repositorioBrazalete.Obtener(id);
        }

        public List<AperturaBrazalete> ObtenerListaAperturaBrazalete(int IdSupervisor)
        {
            return _repositorioBrazalete.ObtenerLista($"WHERE IdSupervisor = {IdSupervisor} AND CONVERT(VARCHAR,Fecha,103) = '{DateTime.Now.ToString("dd/MM/yyyy")}'" ).ToList();
        }



        public bool ActualizarValidSupervisorElemento(List<AperturaElementos> _listElementos)
        {
            bool blnResp = true;

            foreach (var item in _listElementos)
            {
               var rta= _repositorio.ActualizarValidSupervisor(item.Id, item.ValidSupervisor);
                if(rta == 0)
                {
                    blnResp = false;
                    break;
                }                    
            }
            return blnResp;
        }

        public bool ActualizarValidTaquillaElemento(List<AperturaElementos> modelo)
        {
            bool blnResp = true;

            foreach (var item in modelo)
            {
                var rta = _repositorio.ActualizarValidTaquilla(item.Id, item.ValidTaquilla);
                if (rta == 0)
                {
                    blnResp = false;
                    break;
                }
            }
            return blnResp;
        }


        public bool ActualizarTotalBrazalete(AperturaBrazalete modelo)
        {
            AperturaBrazalete _obj = modelo;
            return _repositorioBrazalete.Actualizar(ref _obj);
        }

        public AperturaBrazalete ObtenerAperturaBrazalete(int idSupervisor, int idBrazalete)
        {
            AperturaBrazalete brazalete = new AperturaBrazalete();
            var rta = _repositorioBrazalete.ObtenerLista($"WHERE IdSupervisor = {idSupervisor} AND CONVERT(VARCHAR,Fecha,103) = '{DateTime.Now.ToString("dd/MM/yyyy")}' AND IdBrazalete ={idBrazalete}").ToList();
            if (rta != null && rta.Count > 0)
                brazalete = rta.First();
            else
                brazalete = null;

            return brazalete;
        }

        public IEnumerable<Apertura> ObtenerListaApertura(int IdPunto, int IdEstado)
        {
            return _repositorio.ObtenerLista($"WHERE CONVERT(VARCHAR,Fecha,103) = '{DateTime.Now.ToString("dd/MM/yyyy")}' AND IdPunto = {IdPunto} AND IdEstado = {IdEstado}");
        }

        public IEnumerable<Apertura> ObtenerAperturasTaquillero(int IdTaquillero, int IdEstado)
        {
            return _repositorio.ObtenerLista($"WHERE CONVERT(VARCHAR,Fecha,103) = '{DateTime.Now.ToString("dd/MM/yyyy")}' AND IdTaquillero = {IdTaquillero} AND IdEstado = {IdEstado}");
        }


        public AperturaElementosHeader ObtenerAperturaElementoHeader(int id)
        {
            return _repositorio.ObtenerAperturaElementoHeader(id);
        }

        public string ActualizarAperturaElementoHeader(AperturaElementosHeader aperturaElementosHeader)
        {
            return _repositorio.ActualizarAperturaElementoHeader(aperturaElementosHeader);
        }

        public IEnumerable<Puntos> ObtenerPuntosConElementosReabastecimiento()
        {
            return _repositorio.ObtenerPuntosConElementosReabastecimiento();
        }

        /// RDSH: Retorna los brazaletes que tiene asignado un supervisor o taquillero para poder realizar la impresion
        /// de reabastecimiento.
        public IEnumerable<CierreBrazalete> ObtenerBoleteriaAsignada(int intIdUsuario, bool blnEsSupervisor)
        {
            return _repositorio.ObtenerBoleteriaAsignada(intIdUsuario, blnEsSupervisor);

        }
    }
}
