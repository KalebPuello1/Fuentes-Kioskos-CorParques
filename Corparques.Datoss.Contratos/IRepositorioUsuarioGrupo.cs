﻿using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{
    public interface IRepositorioUsuarioGrupo : IRepositorioBase<UsuarioGrupo>
    {
        IEnumerable<UsuarioGrupo> ObtenerxGrupo(int id);
        
    }
}
