using CorParques.Negocio.Entidades;
using System.Collections.Generic;

namespace CorParques.Datos.Contratos
{

    public interface IRepositorioEstructuraEmpleado : IRepositorioBase<EstructuraEmpleado>
    {
        EstructuraEmpleado ObtenerEmpleadoPorConsecutivo(string Consecutivo);
        EstructuraEmpleado ObtenerEmpleadoPorDocumento(string Documento);
        ConsumoCupoEmpleado InsertarConsumo(ConsumoCupoEmpleado Consumo);
        CortesiasEmpleado ObtenerCortesiaEmpleado(string documento);
        string GuardarCortesiaEmpleado(GuardarCortesiaEmpleado modelo);
    }
}
