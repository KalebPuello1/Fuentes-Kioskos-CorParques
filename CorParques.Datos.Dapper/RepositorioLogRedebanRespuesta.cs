using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Dapper
{

	public class RepositorioLogRedebanRespuesta : RepositorioBase<LogRedebanRespuesta>, IRepositorioLogRedebanRespuesta
	{
        public int ActualizarLogRedebanRespuesta(LogRedebanRespuesta logRedebanRespuesta)
        {
            LogRedebanRespuesta log = new LogRedebanRespuesta();
            int IdLogRedeban = 0;

            var var = _cnn.GetList<LogRedebanRespuesta>().Where(x => x.NumeroRecibo == logRedebanRespuesta.NumeroRecibo && x.Id== logRedebanRespuesta.Id).ToList();
            if (var != null && var.Count > 0)
            {
                log = var[0];
                log.LocalizacionAnulacion = logRedebanRespuesta.LocalizacionAnulacion;
                log.IPAnulacion = logRedebanRespuesta.IPAnulacion;
                log.IdUsuarioAnulacion = logRedebanRespuesta.IdUsuarioAnulacion;
                log.FechaAnulacion = logRedebanRespuesta.FechaAnulacion;
                log.FacturaAnulada = logRedebanRespuesta.FacturaAnulada;
                log.DireccionMACAnulacion = logRedebanRespuesta.DireccionMACAnulacion;
                _cnn.Update(log);
                IdLogRedeban = log.Id;
            }

            return IdLogRedeban;
        }

    }
}
