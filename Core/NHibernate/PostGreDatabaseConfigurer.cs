using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Core.NHibernate
{
    public class PostgreDatabaseConfigurer : IDatabaseConfigurer
    {
        public IPersistenceConfigurer Configurer()
        {
            return PostgreSQLConfiguration.PostgreSQL82.ConnectionString("Server=127.0.0.1;Port=5432;Database=MVCApp1;User Id=mvcuser;Password=mvcpassword;");
        }

        public void BuildSchema(Configuration configuration)
        {
            new SchemaExport(configuration).Create(false, true);
        }
    }
}