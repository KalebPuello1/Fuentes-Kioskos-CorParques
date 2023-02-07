﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;

namespace CorParques.Datos.Contratos
{
    public interface IRepositorioTipoPuntos : IRepositorioBase<TipoPuntos>
    {
        IEnumerable<TipoGeneral> ObtenerListaSimple();
    }
}
