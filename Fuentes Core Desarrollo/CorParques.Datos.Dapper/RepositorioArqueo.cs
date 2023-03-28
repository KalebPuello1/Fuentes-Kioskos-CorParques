using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using CorParques.Transversales.Util;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Dapper
{

	public class RepositorioArqueo : RepositorioBase<NovedadArqueo>, IRepositorioArqueo
	{
        public IEnumerable<Arqueo> ObtenerArqueo(int IdUsuario)
        {
            
            return _cnn.Query<Arqueo>("SP_ObtenerDetalleArqueo", param: new { IdUsuario = IdUsuario }, commandType: System.Data.CommandType.StoredProcedure).ToList();        
           
        }

        public bool InsertarNovedadArqueo(NovedadArqueo modelo)
        {

            try
            {
                _cnn.Query<bool>("SP_InsertarNovedadArqueo", param: new { IdPunto = modelo.IdPunto, IdEstado = modelo.IdEstado, IdTaquillero = modelo.IdTaquillero,
                                   TipoNovedad = modelo.TipoNovedad, Valor = modelo.Valor, FechaCreado= modelo.FechaCreado, UsuarioCreado = modelo.UsuarioCreado,
                                   IdTipoNovedadArqueo = modelo.IdTipoNovedadArqueo, Observarciones = modelo.Observaciones}, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }


    }
}
