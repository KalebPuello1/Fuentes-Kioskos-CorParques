using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{

	public interface IServicioConvenio : IServicioBase<Convenio>
	{
        
        // RDSH: Retorna la información de los pedidos.
        IEnumerable<Convenio> ObtenerListaConvenios();

        // RDSH: Actualiza un convenio.
        bool Actualizar(Convenio modelo, out string error);

        // RDSH: Inserta un convenio. Retorna el id del convenio en la propiedad error
        bool Insertar(Convenio modelo, out string error);

        //RDSH: Retorna la informacion de un convenio para su edicion.
        Convenio ObtenerPorId(int Id);

        //EDSP: Método que permite atualizar los convenios
        string ActualizarPreciosConvenios(ActualizarPrecios modelo);

        //EDSP: Insertar exclusion del convenio
        string InsertarExclusionConvenio(ExclusionConvenio modelo);

        //EDSP: Deshabilitar exclusion 
        int DeshabilitarExclusion(ExclusionConvenio modelo);

        //Obtener todas las exclusiones
        IEnumerable<ExclusionConvenio> ObtenerExclusionesConvenio();

        //Obtener exclusiones por id convenio
        IEnumerable<ExclusionConvenio> ObtenerExclusionesPorIdConvenio(int IdConvenio);

        //Obtener productos por id convenio
        IEnumerable<ConvenioProducto> ObtenerProductoConvenio(int IdConvenio);

        string ActualizarProductosConvenio(IEnumerable<ConvenioProducto> modelo);
    }
}
