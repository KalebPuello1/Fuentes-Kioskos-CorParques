using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;

namespace CorParques.Datos.Dapper
{
    public class RepositorioReimpresion : IRepositorioReimpresion
    {

        #region Declaraciones

        private readonly SqlConnection _cnn;

        #endregion

        #region Constructor

        public RepositorioReimpresion()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }

        #endregion

        public IEnumerable<Reimpresion> ObtenerReimpresion(string _Punto, string _FechaInicial, string _FechaFinal, string _HoraInicial, string _HoraFinal, string _CodBrazalete, string _CodInicialFactura, string _CodFinalFactura)
        {

            if (_Punto == null || _Punto == "0") { _Punto = "0"; }
            if (_FechaInicial == null || _FechaInicial =="0") { _FechaInicial = "0"; }
            if (_FechaFinal == null || _FechaFinal == "0") { _FechaFinal = "0"; }
            if (_HoraInicial == null || _HoraInicial == "0") { _HoraInicial = "0"; }
            if (_HoraFinal == null || _HoraFinal == "0") { _HoraFinal = "0"; }
            if (_CodBrazalete == null || _CodBrazalete == "0") { _CodBrazalete = "0"; }
            if (_CodInicialFactura == null || _CodInicialFactura == "0") { _CodInicialFactura = "0"; }
            if (_CodFinalFactura == null || _CodFinalFactura == "0") { _CodFinalFactura = "0"; }

            var objReimpresion = _cnn.Query<Reimpresion>("SP_ObtenerDetalleReimpresion", param: new
            {
                Punto = _Punto,
                FechaInicial = _FechaInicial,
                FechaFinal = _FechaFinal,
                HoraInicial = _HoraInicial,
                HoraFinal = _HoraFinal,
                CodBrazalete = _CodBrazalete,
                CodInicialFactura = _CodInicialFactura,
                CodFinalFactura = _CodFinalFactura
            }, commandType: System.Data.CommandType.StoredProcedure).ToList();
            return objReimpresion;
        }

        public IEnumerable<Reimpresion> GetReimpresion(ReimpresionFiltros modelo)
        {

            var objReimpresion = _cnn.Query<Reimpresion>("SP_ObtenerDetalleReimpresion", param: new
            {
                Punto = modelo.CodPunto,
                FechaInicial = modelo.FechaInicial,
                FechaFinal = modelo.FechaFinal,
                HoraInicial = modelo.HoraInicial,
                HoraFinal = modelo.HoraFinal,
                CodBrazalete = modelo.CodBrazalete,
                CodInicialFactura = modelo.CodInicialFactura,
                CodFinalFactura = modelo.CodFinalFactura
            }, commandType: System.Data.CommandType.StoredProcedure).ToList();
            return objReimpresion;
        }
    }
}
