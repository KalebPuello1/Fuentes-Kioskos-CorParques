using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{
    /// <summary>
    ///  KADM Configuración Cargos
    /// </summary>
	public interface IServicioCargos : IServicioBase<Cargos>
	{

        IEnumerable<TipoGeneral> ObtenerListaSimple();

        bool Insertar(Cargos modelo, out string error);

        bool GuardarCargoPerfil(Cargos modelo);

        IEnumerable<Cargos> ConsultarCargo(Cargos model);

        Cargos ObtenerxId(int IdCargo);

        IEnumerable<CargosPerfil> ObtenerCargosPerfil(int idCargo);

        bool ActualizarEmail(Cargos modelo);
    }
}
