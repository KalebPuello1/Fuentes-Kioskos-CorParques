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

    public class RepositorioRegistroFallas : RepositorioBase<RegistroFallas>, IRepositorioRegistroFallas
    {
        #region Metodos
        public bool insertarRegistroFalla(Negocio.Entidades.RegistroFallas modelo)
        {
            var consulta = _cnn.Query<string>("SP_insertaRegistroFalla", new {
                idPunto = modelo.idPunto,
                idArea = modelo.idArea,
                idOrdenFalla = modelo.idOrdenFalla,
                descripcion = modelo.descripcion,
                tecnico = modelo.tecnico,
                fechaFalla = modelo.fechaFalla,
                horaFalla = modelo.horaFalla,
                observacionTecnica = modelo.observacionTecnica,
                fechaRespuesta = modelo.fechaRespuesta,
                horaRespuesta = modelo.horaRespuesta,
                fechaLlegadaTec = modelo.fechaLlegadaTec,
                horaLlegadaTec = modelo.horaLlegadaTec
            } , commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            if(int.Parse(consulta) > 0)
            { return true; }
            else
            {
                return false;
            }
            
        }
        #endregion


    }
}
