using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{

	public interface IRepositorioConvenio : IRepositorioBase<Convenio>
	{

        // RDSH: Retorna la información de los pedidos.
        IEnumerable<Convenio> ObtenerListaConvenios();

        // RDSH: Actualiza un convenio.
        bool Actualizar(Convenio modelo, out string error);

        // RDSH: Inserta un convenio. Retorna el id del convenio en la propiedad error
        bool Insertar(Convenio modelo, out string error);

        //RDSH: Retorna el detalle de un convenio por su codigo sap.
        //EDSP: Se actualiza para que reciba el id del convenio 20/12/2017
        IEnumerable<ConvenioDetalle> ObtenerDetalleConvenio(int IdConvenio);

        //EDSP: Se adiciona nuevo método para actualizar los precios de los productos
        string ActualizarPreciosConvenios(ActualizarPrecios modelo);

        //EDSP: Insertar exclusion del convenio
        string InsertarExclusionConvenio(ExclusionConvenio modelo);

        //EDSP: Deshabilitar exclusion 
        int DeshabilitarExclusion(ExclusionConvenio modelo);

        //EDSP: Obtener todas las exclusiones
        IEnumerable<ExclusionConvenio> ObtenerExclusionesConvenio();

        //EDSP: Obtener exclusion
        ExclusionConvenio ObtenerExclusion(int id);

        //EDSP: Obtener exclusiones por ID convenio
        IEnumerable<ExclusionConvenio> ObtenerExclusionesPorIdConvenio(int IdConvenio);

        //EDSP: Obtener productos por convenio
        IEnumerable<ConvenioProducto> ObtenerProductoConvenio(int IdConvenio);

        string ActualizarProductosConvenio(IEnumerable<ConvenioProducto> modelo);

    }
}
