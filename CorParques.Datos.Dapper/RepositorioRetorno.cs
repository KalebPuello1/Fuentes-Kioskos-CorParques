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
    public class RepositorioRetorno: RepositorioBase<Retorno>, IRepositorioRetorno
    {
        #region Declaraciones

        private readonly SqlConnection _cnn;

        #endregion

        #region Constructor
        public RepositorioRetorno()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }
        #endregion        

        public bool Insertar(Retorno modelo, out string error)
        {
            error = string.Empty;

            try
            {
                error = _cnn.Query<string>("SP_InsertarRetorno", param: new
                {
                    NumeroOrden = modelo.NumeroOrden,
                    Aprobado = modelo.Aprobado,
                    Observacion = modelo.Observacion,
                    IdOperaciones = modelo.IdOperaciones,
                    Procesado = modelo.Procesado,
                    IdPunto = modelo.IdPunto,
                    IdUsuarioAprobador = modelo.IdUsuarioAprobador,
                    HoraInicio = modelo.HoraInicio,
                    HoraFin = modelo.HoraFin
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();


            }
            catch (Exception ex)
            {
                error = string.Concat("Error inesperado en Insertar_RepositorioRetorno: ", ex.Message);
            }

            return string.IsNullOrEmpty(error);
        }

        public bool InsertarDetalle(RetornoDetalle modelo,out string error)
        {
            error = string.Empty;

            try
            {
                error = _cnn.Query<string>("SP_InsertarRetornoDetalle", param: new
                {
                    NumeroOrden = modelo.NumeroOrden,
                    NumeroOperacion = modelo.NumeroOperacion,                    
                    Observacion = modelo.Observacion,
                    Aprobado = modelo.Aprobado,
                    IdUsuarioCreacion = modelo.IdUsuarioCreacion,
                    IdOrdenMantenimiento = modelo.IdOrdenMantenimiento,
                    Procesado = modelo.Procesado
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

            }
            catch (Exception ex)
            {
                error = string.Concat("Error inesperado en InsertarDetalle_RepositorioRetorno: ", ex.Message);
            }

            return string.IsNullOrEmpty(error);
        }
    }
}
