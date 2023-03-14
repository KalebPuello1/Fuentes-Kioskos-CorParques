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
using CorParques.Transversales.Util;

namespace CorParques.Datos.Dapper
{
    public class RepositorioOrdenMantenimiento : IRepositorioOrdenMantenimiento
    {
        #region Declaraciones

        private readonly SqlConnection _cnn;

        #endregion

        #region Constructor
        public RepositorioOrdenMantenimiento()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }
        #endregion

        public IEnumerable<OrdenMantenimiento> ObtenerOrdenesMantenimiento(string Punto, long NumeroOrden)
        {
            var lista = _cnn.Query<OrdenMantenimiento>("SP_ObtenerOrdenesMantenimiento",
                param: new
                {
                    Punto = Punto,
                    NumeroOrden = NumeroOrden
                }, commandType: System.Data.CommandType.StoredProcedure).ToList();
            return lista;
        }

        public bool ActualizaHoraOrden(string observaciones, int idUsuarioAprobador, long NumeroOrden, int Aprobado, int IdOperaciones, int Procesado, string CodSapPunto)
        {
            try
            {
                _cnn.Query<bool>("SP_ActualizarOrdenRetorno",
                param: new { Observaciones = observaciones, idUsuarioAprobador = idUsuarioAprobador, NumeroOrden = NumeroOrden, Aprobado = Aprobado, IdOperaciones = IdOperaciones, Procesado = Procesado, CodSapPunto = CodSapPunto },
                commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<Retorno> ObtenerRetornoPorNumeroOrden(long NumeroOrden)
        {
            var rta = _cnn.Query<Retorno>("SP_ObtenerOrdenRetorno",
               param: new { NumeroOrden = NumeroOrden },
               commandType: System.Data.CommandType.StoredProcedure).ToList();
            return rta;
        }

        /// <summary>
        /// RDSH: Retorna una orden de mantenimiento por el numero de la orden.
        /// </summary>
        /// <param name="intNumeroOrden"></param>
        /// <returns></returns>
        public OrdenMantenimiento ObtenerOrdenMantenimiento(long intNumeroOrden)
        {

            OrdenMantenimiento objOrdenMantenimiento;

            try
            {
                objOrdenMantenimiento = _cnn.Query<OrdenMantenimiento>("SP_ObtenerOrdenMantenimiento", param: new
                {
                    NumeroOrden = intNumeroOrden
                },
                    commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

            }
            catch (Exception ex)
            {
                objOrdenMantenimiento = null;
                Utilidades.RegistrarError(ex, "RepositorioOrdenMantenimiento_ObtenerOrdenMantenimiento_");
            }

            return objOrdenMantenimiento;
        }

    }
}
