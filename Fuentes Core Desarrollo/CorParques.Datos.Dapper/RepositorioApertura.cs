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

    public class RepositorioApertura : RepositorioBase<Apertura>, IRepositorioApertura
    {
        public IEnumerable<Puntos> ObtenerPuntosSinApertura(DateTime? Fecha)
        {
            Fecha = (Fecha == null) ? System.DateTime.Now : Fecha;

            return _cnn.Query<Puntos>("SP_RetornarPuntosFecha", param: new { Fecha = Fecha, IdEstado = (int)Enumerador.Estados.Entregado, tipo = 0 }, commandType: System.Data.CommandType.StoredProcedure).ToList();

        }

        public IEnumerable<Puntos> ObtenerPuntosSurtido()
        {

            return _cnn.Query<Puntos>("SP_ObtenerPuntosSurtido", commandType: System.Data.CommandType.StoredProcedure).ToList();

        }

        public IEnumerable<Puntos> ObtenerPuntosParaAperturaElementos()
        {
            return _cnn.Query<Puntos>("SP_RetornarPuntosFechaElementos"
                , param: new { Fecha = System.DateTime.Now, tipo = 0 }
                , commandType: System.Data.CommandType.StoredProcedure).ToList();
        }


        public IEnumerable<Puntos> ObtenerPuntosConApertura(DateTime? Fecha)
        {
            Fecha = (Fecha == null) ? System.DateTime.Now : Fecha;
            return _cnn.Query<Puntos>("SP_RetornarPuntosFecha", param: new { Fecha = Fecha, IdEstado = (int)Enumerador.Estados.Abierto, tipo = 1 }, commandType: System.Data.CommandType.StoredProcedure).ToList();

        }

        public IEnumerable<Puntos> ObtenerPuntosAperturaEnProceso()
        {

            return _cnn.Query<Puntos>("SP_RetornarPuntosFecha", param: new { Fecha = System.DateTime.Now, IdEstado = (int)Enumerador.Estados.Entregado, tipo = 1 }, commandType: System.Data.CommandType.StoredProcedure).ToList();

        }

        public IEnumerable<TipoGeneral> ObtenerTipoElementos()
        {
            return _cnn.GetList<TipoElementos>().Select(x => new TipoGeneral { Id = x.Id, Nombre = x.Nombre });
        }

        public IEnumerable<AperturaElementos> ObtenerElementosPorIdApertura(int _IdApertura)
        {
            var lista = _cnn.GetList<AperturaElementos>(new { IdApertura = _IdApertura }).Where(l => l.IdEstado == 1).ToList();
            foreach (AperturaElementos item in lista)
                item.Elemento = _cnn.Get<TipoElementos>(item.IdElemento);
            return lista;
        }

        public int InsertarAperturaElemento(AperturaElementos modelo)
        {
            int resp = _cnn.Insert<int>(modelo);
            return resp;
        }

        public bool EliminarElementoPorIdAperturaElemento(AperturaElementos modelo)
        {
            var resp = _cnn.Delete(modelo);
            return resp == 1 ? true : false;
        }

        public int InsertarAperturaElementoHeader(AperturaElementosHeader modelo)
        {
            int resp = _cnn.Insert<int>(modelo);
            return resp;
        }

        public int ObtieneIdAperturaElementoHeader(int _Punto, DateTime _Fecha)
        {
            int IdAperturaElementosHeader = 0;
            var lista = _cnn.GetList<AperturaElementosHeader>(new { IdPunto = _Punto, Fecha = _Fecha });
            if (lista != null)
            {
                foreach (var item in lista)
                {
                    IdAperturaElementosHeader = item.Id;
                }
            }
            return IdAperturaElementosHeader;
        }

        public int ObtenerIdAperturaPorPunto(int IdPunto)
        {
            var resp = _cnn.Query<int>("SP_RetornarIdAperturaPorPunto", param: new { Fecha = System.DateTime.Now, IdEstado = (int)Enumerador.Estados.Entregado, IdPunto = IdPunto }, commandType: System.Data.CommandType.StoredProcedure).ToList();
            return resp.FirstOrDefault();
        }

        public int ActualizarValidSupervisor(int IdElemento, bool CheckSupervisor)
        {
            var _elemento = _cnn.Get<AperturaElementos>(IdElemento);
            _elemento.ValidSupervisor = CheckSupervisor;
            return _cnn.Update(_elemento);
        }

        public int ActualizarValidTaquilla(int IdElemento, bool CheckTaquilla)
        {
            var _elemento = _cnn.Get<AperturaElementos>(IdElemento);           
            _elemento.ValidTaquilla = CheckTaquilla;
            return _cnn.Update(_elemento);
        }

        public BitacoraElementos ObtenerBitacora(int IdBitacoraElemento)
        {
            var rta = _cnn.Get<BitacoraElementos>(IdBitacoraElemento);
            if (rta != null)
                rta.BitacoraElementoDetalle = _cnn.GetList<BitacoraElementosDetalle>();
            return rta;
        }
        public bool ActualizarBitacora(BitacoraElementos modelo)
        {
            var rta = _cnn.Update(modelo);
            return rta > 0;
        }

        public IEnumerable<AperturaElementos> ObtenerElementosPorIdPunto(int _IdPunto, DateTime _Fecha)
        {
            int IdHeader = this.ObtieneIdAperturaElementoHeader(_IdPunto, _Fecha);
            var lista = _cnn.GetList<AperturaElementos>(new { IdAperturaElementosHeader = IdHeader }).Where(l=>l.IdEstado == 1).ToList();
            foreach (AperturaElementos item in lista)
                item.Elemento = _cnn.Get<TipoElementos>(item.IdElemento);

            return lista;
        }


        public AperturaElementosHeader ObtenerAperturaElementoHeader(int id)
        {
            var lista = _cnn.GetList<AperturaElementosHeader>(new { Id = id });
            AperturaElementosHeader objretorno = null;
            foreach (AperturaElementosHeader item in lista)
                objretorno = item;
            return objretorno;
        }

        public string ActualizarAperturaElementoHeader(AperturaElementosHeader aperturaElementosHeader)
        {
            var rta = _cnn.Query<string>("SP_ActualizarAperturaElementosHeader",
                       commandType: CommandType.StoredProcedure,
                       param: new
                       {
                           IdAperturaElementosHeader = aperturaElementosHeader.Id,
                           ObservacionNido = aperturaElementosHeader.ObservacionNido,
                           ObservacionSupervisor = aperturaElementosHeader.ObservacionSupervisor,
                           ObservacionPunto = aperturaElementosHeader.ObservacionPunto,
                           IdEstado = aperturaElementosHeader.IdEstado,
                           UsuarioModificado = aperturaElementosHeader.UsuarioModificado,
                           FechaModificado = aperturaElementosHeader.FechaModificado,
                           IdSupervisor = aperturaElementosHeader.IdSupervisor,
                           IdTaquillero = aperturaElementosHeader.IdTaquillero
                       });

            return rta.Single().ToString();
        }


        public IEnumerable<Puntos> ObtenerPuntosConElementosReabastecimiento()
        {
            return _cnn.Query<Puntos>("SP_RetornarPuntosConElementosReabastecimiento"
                , param: new { Fecha = System.DateTime.Now }
                , commandType: System.Data.CommandType.StoredProcedure).ToList();
        }

        /// <summary>
        /// RDSH: Valida si taquillero tiene una apertura activa.
        /// </summary>
        /// <param name="intIdUsuario"></param>
        /// <returns></returns>
        public string ValidaPermiteAnularUsuario(int intIdUsuario)
        {
            int intRespuesta = 0;
            try
            {
                var resp = _cnn.Query<int>("SP_ValidaPermiteAnular", param: new { IdUsuario = intIdUsuario }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
                intRespuesta = resp;

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "");
            }
            return (intRespuesta > 0 ? "1" : "0");
        }

        #region Reabastecimiento

        /// <summary>
        /// RDSH: Retorna los brazaletes que tiene asignado un supervisor o taquillero para poder realizar la impresion
        /// de reabastecimiento.
        /// </summary>
        /// <param name="intIdUsuario"></param>
        /// <param name="blnEsSupervisor"></param>
        /// <returns></returns>
        public IEnumerable<CierreBrazalete> ObtenerBoleteriaAsignada(int intIdUsuario, bool blnEsSupervisor)
        {

            try
            {

                var objCorteBrazalete = _cnn.Query<CierreBrazalete>("SP_ObtenerBoleteriaAsignada", param: new
                {
                    IdUsuario = intIdUsuario,
                    EsSupervisor = blnEsSupervisor
                }, commandType: System.Data.CommandType.StoredProcedure).ToList();

                return objCorteBrazalete;

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "RepositorioApertura_ObtenerBoleteriaAsignada");
                return null;
            }
        }


        #endregion

    }
}
