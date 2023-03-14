using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading;
using CorParques.Negocio.Contratos;
using CorParques.Servicios.WebApi.App_Start;
using CorParques.Negocio.Nucleo;
using System.Threading.Tasks;
using System.Net.Http;
using CorParques.Negocio.Entidades;
using CorParques.Transversales.Util;
using System.Configuration;
using System.Net.Http.Headers;

namespace CorParques.Servicios.WebApi.SignalR
{
    [HubName("NotificacionesHub")]
    public class NotificacionesHub : Hub
    {
    }

    public static class StartHub
    {
        public static HttpClient client;
        public static void Initializer()
        {
            int valor = 0;
            var context = GlobalHost.ConnectionManager.GetHubContext<NotificacionesHub>();
            Thread tarea = new Thread(() =>
            {
                do
                {
                    var result = GetAsync<IEnumerable<NotificacionAlerta>>("Recoleccion/ObtenerAlertas");

                    context.Clients.All.clienteAlertas(result);
                    valor++;
                   Thread.Sleep(20000);
                } while (true);
            });
            tarea.Start();
        }
        public static void SetLogOut(int idUsuario, int punto)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<NotificacionesHub>();
            context.Clients.All.clienteCerrarSesion(idUsuario, punto);
        }
        /// <summary>
        /// envia notificacion a todos los clientes, cada cliente validara cual le corresponde a cada uno
        /// </summary>
        /// <param name="lista">lista de los valore que se van a enviar a os clenetes</param>
        public static void SR2_UpdateAllClientNotification(IEnumerable<Notificacion> lista)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<NotificacionesHub>();
            context.Clients.All.verMensajePunto(lista);
        }
        internal static T GetAsync<T>(string path) where T : class
        {
            if (client == null)
            {
                client = new HttpClient();
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["UrlService"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
            HttpResponseMessage response = AsyncHelpers.RunSync(() => client.GetAsync(path));
            if (response.IsSuccessStatusCode)
            {
                var result = AsyncHelpers.RunSync(() => response.Content.ReadAsAsync<T>());
                return result;
            }
            return null;
        }
    }
}