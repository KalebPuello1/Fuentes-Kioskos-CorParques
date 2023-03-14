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
    public class RepositorioGestionMantenimientoControl : RepositorioBase<GestionMantenimientoControl>, IRepositorioGestionMantenimientoControl
    {
        public bool ActualizarMantenbimientoControl(GestionMantenimientoControl Modelo)
        {

            try
            {
                _cnn.Query<bool>("ActualizarMatenimiento",
                param: new { IDMantenimiento_Control = Modelo.Id, Descripcion = Modelo.Descripcion, IdAtraccion = Modelo.IdAtraccion, UsuarioCreado = Modelo.UsuarioCreacion, FechaCreado = Modelo.FechaCreacion, UsuarioModificacion = Modelo.UsuarioModicifacion, FechaModificacion = Modelo.FechaModificacion, AtracionesVerificadas = Modelo.Verificadas },
                commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public IEnumerable<GestionMantenimientoControl> ObtenerTodosMantenimientos()
        {
            return _cnn.Query<GestionMantenimientoControl>("RetornarMatenimientoControl",  param: new { IDMantenimiento_Control = 0 },
                commandType: System.Data.CommandType.StoredProcedure).ToList();
        }

        public GestionMantenimientoControl ObtenerxMantenimiento(int id)
        {
            return _cnn.Query<GestionMantenimientoControl>("RetornarMatenimientoControl", param: new { IDMantenimiento_Control = id },
                commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
        }
    }
}
