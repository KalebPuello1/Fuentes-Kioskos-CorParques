﻿using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{
    public interface IServicioCentroImpresion
    {
        int? InsertarSolicitudImpresion(SolicitudBoleteria modelo);
        IEnumerable<SolicitudBoleteria> ObtenerListSolicitudBoleteria(int idUsuario);
        IEnumerable<SolicitudBoleteria> ObtenerTodasSolicitudes();
        IEnumerable<SolicitudBoleteria> GestionarCentroImpresion(SolicitudBoleteria modelo);
        bool EliminarSolicitudImpresion(SolicitudBoleteria modelo);
    }
}
