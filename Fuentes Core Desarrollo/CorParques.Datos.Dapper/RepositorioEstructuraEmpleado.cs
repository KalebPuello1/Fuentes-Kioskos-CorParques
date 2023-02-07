using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using CorParques.Transversales.Util;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CorParques.Datos.Dapper
{
    public class RepositorioEstructuraEmpleado : RepositorioBase<EstructuraEmpleado>, IRepositorioEstructuraEmpleado
    {
        public EstructuraEmpleado ObtenerEmpleadoPorConsecutivo(string Consecutivo)
        {
            EstructuraEmpleado empleado = new EstructuraEmpleado();

            //var listaConsec = _cnn.GetList<EstructuraEmpleadoConsecutivos>($"WHERE Consecutivo = '{Consecutivo}'");
            //if (listaConsec != null && listaConsec.Count() > 0)
            //{
            //    EstructuraEmpleadoConsecutivos objConsec = listaConsec.FirstOrDefault();
            //    var listaEmpledos = _cnn.GetList<EstructuraEmpleado>($"WHERE CodigoSap = '{objConsec.CodigoSapEmpleado}'");
            //    if (listaEmpledos != null && listaEmpledos.Count() > 0)
            //    {
            //        empleado = listaEmpledos.FirstOrDefault();
            //    }
            //}
            empleado = ObtenerEmpleadoPorDocumento(Consecutivo);
            return empleado;
        }

        public EstructuraEmpleado ObtenerEmpleadoPorDocumento(string Documento)
        {
            EstructuraEmpleado empleado = new EstructuraEmpleado();

            //var listaEmpledos = _cnn.GetList<EstructuraEmpleado>($"WHERE Documento = '{Documento}'");
            var listaEmpledos = _cnn.Query<EstructuraEmpleado>("SP_ObtenerEmpleadoPorDocumento", param: new
            {
                Documento = Documento
            }, commandType: CommandType.StoredProcedure).ToList();

            if (listaEmpledos != null && listaEmpledos.Count() > 0)
            {
                empleado = listaEmpledos.FirstOrDefault();
            }

            return empleado;
        }

        public ConsumoCupoEmpleado InsertarConsumo(ConsumoCupoEmpleado Consumo)
        {
            //EstructuraEmpleado empleado = new EstructuraEmpleado();
            var resp = _cnn.Insert<int>(Consumo);
            Consumo.IdConsumoCupoEmpleado = resp;
            return Consumo;
        }

        public CortesiasEmpleado ObtenerCortesiaEmpleado(string documento)
        {
            var cortesia = new CortesiasEmpleado();
            var rta = _cnn.Query<CortesiasEmpleado>("SP_ObtenerCortesiaEmpleado", param: new
            {
                Documento = documento
            },commandType: CommandType.StoredProcedure);
            if (rta != null && rta.Count() > 0 )
                cortesia = rta.First();

            return cortesia;
        }

        public string GuardarCortesiaEmpleado(GuardarCortesiaEmpleado modelo)
        {
            var mensaje = string.Empty;
            var rta = _cnn.Query<string>("SP_GuardarCortesiaEmpleado", param: new
            {
                DetalleProducto = Utilidades.convertTable(modelo.ListaProductos.Select(x => new TablaGeneral
                {
                    col1 = x.IdDetalleProducto.ToString(), col2 = x.CodigoSap, col3 = x.IdProducto.ToString()
                })),
                IdUsuario = modelo.idUsuario,
                Documento = modelo.Documento
            }, commandType: CommandType.StoredProcedure);

            return rta.Single();
        }

    }
}
