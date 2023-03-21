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
    public class RepositorioAperturaBrazalete : RepositorioBase<AperturaBrazalete>, IRepositorioAperturaBrazalete
    {
        public int InsertarAperturaBrazaleteDetalle(AperturaBrazaleteDetalle modelo)
        {
            var rta =  _cnn.Insert<int>(modelo);
            _cnn.Query<string>(sql: "SP_ActualizarAlerta",
                        commandType: CommandType.StoredProcedure,
                        param: new { IdUsuario = modelo.IdTaquillero, IdProducto=modelo.IdAperturaBrazalete });
            return rta;

        }

        public AperturaBrazalete ObtieneAperturaBrazaleteParaReabastecimiento(AperturaBrazalete brazalete)
        {
            AperturaBrazalete brazaleteRetorno = null;

            try
            {
                string sWhere = $"WHERE DATEADD(dd, 0, DATEDIFF(dd, 0, Fecha)) = DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE()))";
                sWhere += $" AND IdBrazalete = '{brazalete.IdBrazalete}' AND IdSupervisor = '{brazalete.IdSupervisor}'";
                var listaConsultada = _cnn.GetList<AperturaBrazalete>(sWhere);
                if (listaConsultada != null && listaConsultada.Count() > 0)
                {
                    brazaleteRetorno = listaConsultada.FirstOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }

            return brazaleteRetorno;
        }

        public IEnumerable<AperturaBrazalete> ObtenerApeturaBrazalete(int IdSupervisor)
        {
            var _rta = _cnn.Query<AperturaBrazalete>(sql: "SP_ObtenerAperturaBrazalete", 
                        commandType: CommandType.StoredProcedure, 
                        param: new { IdSupervisor = IdSupervisor });

            return _rta;

        }
    }
}
