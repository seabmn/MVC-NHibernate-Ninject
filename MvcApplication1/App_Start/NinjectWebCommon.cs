using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Core.NHibernate;
using Ninject.Extensions.Logging.Log4net;
using Ninject.Modules;

[assembly: WebActivator.PreApplicationStartMethod(typeof(MvcApplication1.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(MvcApplication1.App_Start.NinjectWebCommon), "Stop")]


namespace MvcApplication1.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            Bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            Bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            var databaseConfigurer = ConfigurationManager.AppSettings.GetValues("DatabaseConfigurer");
            var databaseConfigurerType = Type.GetType(databaseConfigurer.First());
            kernel.Load(new List<INinjectModule> { new NHibernateModule(databaseConfigurerType), new MVCApplicationModule() });
            log4net.Config.XmlConfigurator.Configure();
        }        
    }
}
