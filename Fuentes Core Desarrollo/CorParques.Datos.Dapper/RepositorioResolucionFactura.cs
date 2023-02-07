using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Transversales.Util;

namespace CorParques.Datos.Dapper
{

    public class RepositorioResolucionFactura : RepositorioBase<ResolucionFactura>, IRepositorioResolucionFactura
    {
        public string AprobarResolucion(int id)
        {
            try
            {

                var consulta = _cnn.Query<string>("SP_AprobarResolucionFactura", param: new { IdResolucion = id }, commandType: System.Data.CommandType.StoredProcedure);
                return "";
            }
            catch (Exception ex)
            {
                return string.Concat("Error al inactivar la resolucion: ", ex.Message);
            }
        }



        #region Metodos

        public string EliminarResolucion(int id)
        {
            try
            {

                var consulta = _cnn.Query<string>("SP_InactivarResolucionFactura", param: new { IdResolucion = id }, commandType: System.Data.CommandType.StoredProcedure);
                return "";
            }
            catch (Exception ex)
            {
                return string.Concat("Error al inactivar la resolucion: ", ex.Message);
            }
        }

        public string InsertarResolucion(ResolucionFactura resolucion)
        {
            try
            {

                var consulta = _cnn.Query<string>("SP_InsertarResolucionFactura", param: new {
                    IdPunto = resolucion.IdPunto,
                    Resolucion = resolucion.Resolucion,
                    Prefijo = resolucion.Prefijo,
                    ConsecutivoInicial = resolucion.ConsecutivoInicial,
                    ConsecutivoFinal = resolucion.ConsecutivoFinal,
                    FechaInicial=  System.DateTime.Parse(resolucion.FechaInicio),
                    FechaFinal = System.DateTime.Parse(resolucion.FechaFinal),
                    Usuario = resolucion.Usuario
                }, commandType: System.Data.CommandType.StoredProcedure);
                return "";
            }
            catch (Exception ex)
            {
                return string.Concat("Error al crear la resolucion: ", ex.Message);
            }
        }

        public IEnumerable<ResolucionFactura> ObtenerResoluciones(int aprovador)
        {
            var consulta = _cnn.Query<ResolucionFactura>("SP_ObtenerResoluciones", param:new { Aprovador=aprovador}, commandType: System.Data.CommandType.StoredProcedure);
            return consulta;
        }

        public IEnumerable<ResolucionFactura> ObtenerPrefijoConsecutivo(string prefijo, int consecutivoInicial)
        {
            var consulta = _cnn.GetList<ResolucionFactura>().Where(x => x.Prefijo == prefijo && 
                                                                  (x.ConsecutivoInicial >= consecutivoInicial && 
                                                                   x.ConsecutivoFinal <= consecutivoInicial)).ToList();
            return consulta;
        }

        #endregion


    }
}
