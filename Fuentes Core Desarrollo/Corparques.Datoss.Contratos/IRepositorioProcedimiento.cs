using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{
    public interface IRepositorioProcedimiento
    {

        //RDSH: Lista de procedimientos realizados
        IEnumerable<Procedimiento> ObtenerLista();

        //RDSH: Inserta un procedimiento.
        bool Insertar(Procedimiento modelo, out string error);

        //RDSH: Actualiza un procedimiento
        bool Actualizar(Procedimiento modelo, out string error);

        //RDSH: borrado logico de un procedimiento.
        bool Eliminar(Procedimiento modelo, out string error);        

        //RDSH: Retorna un objeto de tipo procedimiento para su edicion.
        Procedimiento ObtenerPorId(int Id);

        //RDSH: Reporte de atenciones centro medico.
        IEnumerable<Procedimiento> ReporteAtenciones(int IdTipoDocumentoPaciente, int IdCategoriaAtencion, int IdTipoPaciente, string FechaInicial, string FechaFinal, int IdProcedimiento, int IdZonaArea, int IdUbicacion);        

    }
}
