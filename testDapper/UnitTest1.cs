using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CorParques.Datos.Dapper;
using CorParques.Negocio.Entidades;

namespace testDapper
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var data = new PagoFactura()
            {
                listaProducto = new System.Collections.Generic.List<Producto>()
                {
                    new Producto
                    {
                        IdProducto = 1, CodigoSap = "asd", Nombre = "sad", Precio = 50000
                    }
                },
                listMediosPago = new System.Collections.Generic.List<PagoFacturaMediosPago>()
                {
                    new PagoFacturaMediosPago
                    {
                        IdMedioPago = 1, Valor = 5000
                    },
                    new PagoFacturaMediosPago
                    {
                        IdMedioPago= 2, Valor = 6000
                    }
                }
            };
            //new RepositorioFactura().test(data);
        }
    }
}
