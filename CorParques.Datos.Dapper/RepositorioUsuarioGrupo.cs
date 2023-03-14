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
    public class RepositorioUsuarioGrupo : RepositorioBase<UsuarioGrupo>, IRepositorioUsuarioGrupo
    {


        public IEnumerable<UsuarioGrupo> ObtenerxGrupo(int id)
        {
            return _cnn.GetList<UsuarioGrupo>().Where(x => x.Idgrupo == id);
        }
    }
}
