using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CorParques.Transversales.Util;

namespace CorParques.Datos.Dapper
{

	public class RepositorioAperturaBase : RepositorioBase<AperturaBase>,  IRepositorioAperturaBase
	{

        public string InsertarAperturaBase(Apertura apertura)
        {

            string strListPuntos = string.Empty;
            
            if (apertura.ListaPuntos != null && apertura.ListaPuntos.Any())
            {
                var _listID = (from x in apertura.ListaPuntos select x.Id).ToList();
                strListPuntos = string.Join(",", _listID);
            }

            var rta = _cnn.Query<int>("SP_InsertarApertura",
                       commandType: CommandType.StoredProcedure,
                       param: new
                       {
                           IdPunto = strListPuntos, Observacion = apertura.ObservacionNido, IdEstado = apertura.IdEstado,
                           usuarioCreado = apertura.UsuarioCreado, FechaCreado = apertura.FechaCreado,
                           Fecha = apertura.Fecha,
                           IdpuntoCreado = apertura.IdPuntoCreado,
                           AperturaBase = Utilidades.convertTable(apertura.AperturaBase
                                                        .Select(x => new TablaGeneral
                                                        {
                                                            col1 = x.IdTipoDenominacion.ToString(),
                                                            col2 = x.CantidadNido.ToString(),
                                                            col3 = x.TotalNido.ToString()
                                                        }))
                       });

            return rta.Single().ToString();
        }

        public IEnumerable<AperturaBase> ObtenerAperturaBase(int IdPunto, DateTime? Fecha)
        {
            Fecha = (Fecha == null) ? System.DateTime.Now : Fecha;
            return _cnn.Query<AperturaBase>("SP_RetornarAperturaBasePunto", param: new { IdPunto = IdPunto, Fecha = Fecha}, commandType: System.Data.CommandType.StoredProcedure).ToList();

        }


        public string ActualizarAperturaBase(Apertura apertura)
        {

            var rta = _cnn.Query<string>("SP_ActualizarApertura",
                       commandType: CommandType.StoredProcedure,
                       param: new
                       {
                           IdApertura = apertura.Id,
                           ObservacionNido = apertura.ObservacionNido,
                           ObservacionSupervisor = apertura.ObservacionSupervisor,
                           ObservacionPunto = apertura.ObservacionPunto,
                           IdEstado = apertura.IdEstado,
                           UsuarioModificado = apertura.UsuarioModificado,
                           FechaModificado = apertura.FechaModificado,
                           IdSupervisor = apertura.IdSupervisor,
                           IdTaquillero = apertura.IdTaquillero,
                           AperturaBase = Utilidades.convertTable(apertura.AperturaBase
                                                        .Select(x => new TablaGeneral
                                                        {
                                                            col1 = x.IdTipoDenominacion.ToString(),
                                                            col2 = x.CantidadNido.ToString(),
                                                            col3 = x.CantidadSupervisor.ToString(),
                                                            col4 = x.CantidadPunto.ToString(),
                                                            col5 = x.TotalNido.ToString(),
                                                            col6 = x.TotalSupervisor.ToString(),
                                                            col7 = x.TotalPunto.ToString(),
                                                        }))
                       });

            return rta.Single().ToString();
        }
    }
}
