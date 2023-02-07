using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace CorParques.Servicios.Contigencia
{
    partial class Service : ServiceBase
    {
        private Timer tmServicio;
        private Timer tmServer;

        bool proceso = true;  // Obtener Data servidor principal a Local 
        bool proceso2 = true; // Enviar Data de local a principal

        public Service()
        {
            InitializeComponent();
            tmServicio = new Timer(Convert.ToInt32(ConfigurationManager.AppSettings["TiempoSincronizacion"].ToString()));
            tmServicio.Elapsed += new ElapsedEventHandler(Servicio_Elapsed);

            tmServer = new Timer(Convert.ToInt32(ConfigurationManager.AppSettings["TiempoServerPrincipal"].ToString()));
            tmServer.Elapsed += new ElapsedEventHandler(EnviarDataServer);
        }

        protected override void OnStart(string[] args)
        {
            tmServicio.Start();
            tmServer.Start();
            EventLog.WriteEntry("Contingencia corparques", "Inicio servicio");
        }

        protected override void OnStop()
        {
            tmServicio.Stop();
            tmServer.Stop();

            EventLog.WriteEntry("Contingencia corparques", "Se detuvo servicio");
        }

        void Servicio_Elapsed(object sender, ElapsedEventArgs arg)
        {
            if (proceso)
            {
                proceso = false;

                try
                {
                    var _proceso = new Negocio();
                    _proceso.SincronizarTablas();
                    _proceso.ActualizarDiccionarios();
                    _proceso.ObtenerUltimaFacturaPunto();

                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry("Contingencia corparques", "Ocurrio el siguiente error al sincronizar las tablas: " + ex.Message);
                }
                finally
                {
                    proceso = true;
                }
            }
        }
        
        void EnviarDataServer(object sender, ElapsedEventArgs arg)
        {
            if (proceso2)
            {
                proceso2 = false;

                try
                {
                    Negocio negocio = new Negocio();
                    negocio.SincronizarLocalPrincipal();
                    negocio.ObtenerUltimaFacturaPunto();
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry("Contingencia corparques", "Ocurrio el siguiente error al enviar la Data al servidor principal :" + ex.Message);
                }
                finally
                {
                    proceso2 = true;
                }
            }
        } 
    }
}
