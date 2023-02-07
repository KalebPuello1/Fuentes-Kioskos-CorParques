using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{

	public interface IServicioAperturaBase : IServicioBase<AperturaBase>
	{

        string InsertarAperturaBase(Apertura apertura);
        IEnumerable<AperturaBase> ObtenerAperturaBase(int IdPunto, DateTime? Fecha = null);
        string ActualizarAperturaBase(Apertura apertura);
        DetalleInventario ObtenerDetalleInventario(string Puntos);
        IEnumerable<AperturaBrazalete> ObtenerApeturaBrazalete(int IdSupervisor);
    }
}
