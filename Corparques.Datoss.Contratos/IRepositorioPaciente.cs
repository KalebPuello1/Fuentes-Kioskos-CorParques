using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{
    public interface IRepositorioPaciente
    {

        //RDSH: Lista principal de pacientes
        IEnumerable<Paciente> ObtenerLista();

        //RDSH: Inserta un convenio parqueadero.
        bool Insertar(Paciente modelo, out string error);

        //RDSH: Actualiza un paciente.
        bool Actualizar(Paciente modelo, out string error);

        //RDSH: borrado logico de pacientes
        bool Eliminar(Paciente modelo, out string error);

        //RDSH: Obtiene un objeto Paciente filtrando por el tipo de documento y el numero de documento.
        Paciente ObtenerPorTipoDocumento(int IdTipoDocumento, string Documento);

        //RDSH: Retorna un objeto Paciente por id.
        Paciente ObtenerPorId(int Id);

        //RDSH: Lista de pacientes para combo box.
        IEnumerable<TipoGeneral> ObtenerListaSimple();

    }
}
