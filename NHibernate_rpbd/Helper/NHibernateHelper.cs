using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
// <summary>
// NHibernate Helper
// </summary>
// <remarks>
// Because the SessionFactory creation is lazy-loaded, you technically never need to bootstrap NHibernate
// and instead can just call OpenSession() as it will do it for you the first time you make the call.
// </remarks>

using NHibernate_rpbd;

public static class NHibernateHelper
{
    #region Private Fields

    private static ISessionFactory _sessionFactory;

    #endregion Private Fields

    #region Public Properties

    /// <summary>
    /// Creates <c>ISession</c>s.
    /// </summary>
    public static ISessionFactory SessionFactory
    {
        get { return _sessionFactory ?? (_sessionFactory = CreateSessionFactory()); }
    }

    /// <summary>
    /// Allows the application to specify properties and mapping documents to be used when creating a <see cref="T:NHibernate.ISessionFactory"/>.
    /// </summary>
    public static NHibernate.Cfg.Configuration Configuration { get; set; }

    #endregion Public Properties

    #region Public Methods

    /// <summary>
    /// Open a new NHibenate Session
    /// </summary>
    /// <returns>A new ISession</returns>
    public static ISession OpenSession()
    {
        var session = SessionFactory.OpenSession();

        return session;
    }

    /// <summary>
    /// Open a new stateless NHibernate Session
    /// </summary>
    /// <returns>Stateless NHibernate Session</returns>
    public static IStatelessSession OpenStatelessSession()
    {
        var session = SessionFactory.OpenStatelessSession();

        return session;
    }

    #endregion Public Methods

    #region Private Methods

    private static ISessionFactory CreateSessionFactory()
    {
        if (Configuration == null)
        {
            Configuration = new Configuration();

            Configuration.BeforeBindMapping += OnBeforeBindMapping;

            Configuration = Fluently.Configure()
                .Database(
                    PostgreSQLConfiguration.Standard
                        .ConnectionString(c => c.Host("127.0.0.1")
                            .Port(5432)
                            .Database("RPBD_1")
                            .Username("postgres")
                            .Password("password"))
                )
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Program>()
                    .Conventions.Add(Table.Is(x => x.TableName.ToLower())))
                .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                .BuildConfiguration();
        }

        return Configuration.BuildSessionFactory();
    }

    private static void OnBeforeBindMapping(object sender, BindMappingEventArgs bindMappingEventArgs)
    {
        // Force using the fully qualified type name instead of just the class name.
        // This will get rid of any duplicate mapping/class name issues.
        bindMappingEventArgs.Mapping.autoimport = false;
    }

    #endregion Private Methods
}