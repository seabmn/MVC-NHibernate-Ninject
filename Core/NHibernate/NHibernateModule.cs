using System;
using NHibernate;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;

namespace Core.NHibernate
{
    public class NHibernateModule : NinjectModule
    {
        private readonly Type _databaseConfigurer;

        public NHibernateModule(Type databaseConfigurer)
        {
            _databaseConfigurer = databaseConfigurer;
        }

        public override void Load()
        {
            //NOTE: Change this "To" if you want to use a different database, there is a configurer for SQlite and Postgre.
            //TODO: This could possibly be set up so its configurable from the configuration file, so it binds to the correct database configurer.
            Bind<IDatabaseConfigurer>().To(_databaseConfigurer);
            Bind<INHibernateBuilder>().To<NHibernateBuilder>().InSingletonScope();
            Bind<ISessionFactory>().ToMethod(context => context.Kernel.Get<INHibernateBuilder>().CreateSessionFactory()).InSingletonScope();
            Bind<ISession>().ToMethod(context => context.Kernel.Get<ISessionFactory>().OpenSession()).InRequestScope();
        }
    }
}