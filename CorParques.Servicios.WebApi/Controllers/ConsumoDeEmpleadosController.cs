using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using CorParques.Negocio.Nucleo;
using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;

namespace CorParques.Servicios.WebApi.Controllers
{
    public class ConsumoDeEmpleadosController : ApiController
    {
        #region properties
        private readonly IServicioConsumoDeEmpleados _servicio;
        #endregion


        #region constructor
        public ConsumoDeEmpleadosController(IServicioConsumoDeEmpleados servicio)
        {
            this._servicio = servicio;
        }
        #endregion

        #region methods
        [HttpGet]
        [Route("api/ConsumoDeEmpleados/BuscarEmpresas")]
        public HttpResponseMessage buscarEmpresa()
        {
            var _list = _servicio.buscarEmpresas();
            return _list.Count() == 0 ? Request.CreateResponse(System.Net.HttpStatusCode.NotFound)
                : Request.CreateResponse(System.Net.HttpStatusCode.OK, _list);
        }

        [HttpGet]
        [Route("api/ConsumoDeEmpleados/Buscar/{FInicial}/{FFinal}/{NDocumento}/{Areaa}")]
        public HttpResponseMessage buscar(string FInicial, string FFinal, string NDocumento, string Areaa)
        { 
           IEnumerable<ConsumoDeEmpleados>[] _List = _servicio.buscar(FInicial, FFinal, NDocumento, Areaa);
            return _List.Count() == 0 ? Request.CreateResponse(System.Net.HttpStatusCode.NotFound)
                 : Request.CreateResponse(System.Net.HttpStatusCode.OK, _List);
        }
        #endregion

    }
}
