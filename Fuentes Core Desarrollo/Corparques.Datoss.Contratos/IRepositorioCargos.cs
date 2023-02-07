using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CorParques.Datos.Contratos
{
    /// <summary>
    /// KADM Configuración Cargos
    /// </summary>
    public interface IRepositorioCargos
    {
        IEnumerable<TipoGeneral> ObtenerListaSimple();

        bool Insertar(Cargos modelo, out string error);

        bool GuardarCargoPerfil(Cargos modelo, out string error);

        IEnumerable<Cargos> ConsultarCargo(Cargos model);

        IEnumerable<Cargos> ObtenerLista();

        Cargos ObtenerxId(int IdCargo);

        bool Actualizar(Cargos cargos);

        IEnumerable<CargosPerfil> ObtenerListaCargoPerfil(int idCargo);


    }
}
