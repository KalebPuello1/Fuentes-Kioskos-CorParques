using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;
using CorParques.Transversales.Util;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CorParques.Servicios.WebApi.Controllers
{
    public class EstadoController : ApiController
    {

        private readonly IServicioEstado _service;

        public EstadoController(IServicioEstado service)
        {
            _service = service;
        }
        [HttpGet]
        [Route("api/Estado/ObtenerEstados/{IdModulo}")]
        public HttpResponseMessage ObtenerEstados(int IdModulo)
        {
            var list = Cache.GetCache<IEnumerable<TipoGeneral>>($"Estados({IdModulo})");
            if (list==null) {
                list = _service.ObtenerEstados(IdModulo);
                Cache.SetCache($"Estados({IdModulo})", list, Cache.Long);
            }
            return list.Count() == 0 ? Request.CreateResponse(HttpStatusCode.NotFound)
                            : Request.CreateResponse(HttpStatusCode.OK, list);
        }
        
    }
}
