[assembly: WebActivator.PostApplicationStartMethod(typeof(CorParques.Presentacion.MVC.Ventas.Core.App_Start.SimpleInjectorWebApiInitializer), "Initialize")]

namespace CorParques.Presentacion.MVC.Ventas.Core.App_Start
{

    using System.Web.Mvc;
    using SimpleInjector;
    using SimpleInjector.Integration.Web;
    using SimpleInjector.Integration.Web.Mvc;
    using System.Reflection;
    using Transversales.Contratos;
    using Transversales.Util;

    public static class SimpleInjectorWebApiInitializer
    {
               
        /// <summary>Initialize the container and register it as Web API Dependency Resolver.</summary>
        public static void Initialize()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            InitializeContainer(container);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        private static void InitializeContainer(Container container)
        {
            
            #region Transversales

            container.Register<IServicioImprimir, ServicioImprimir>(Lifestyle.Scoped);

            #endregion
        }
    }
}