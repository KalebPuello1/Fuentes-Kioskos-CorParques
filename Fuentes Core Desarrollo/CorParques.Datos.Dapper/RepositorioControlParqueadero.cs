﻿using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace CorParques.Datos.Dapper
{
    public class RepositorioControlParqueadero : RepositorioBase<ControlParqueadero>, IRepositorioControlParqueadero
    {
    }
}