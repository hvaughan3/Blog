#region Usings

using System;
using HinesSite.Controllers;
using HinesSite.Data;
using HinesSite.Data.Repository;
using HinesSite.Interface;
using HinesSite.Logging;
using Microsoft.Practices.Unity;

#endregion

namespace HinesSite {

    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig {

        #region Unity Container

        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() => {
            UnityContainer container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer() {
            return container.Value;
        }

        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or API controllers (unless you want to
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container) {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();z

            // TODO: Register your types here
            // container.RegisterType<IProductRepository, ProductRepository>();
            container.RegisterType<AccountController>(new InjectionConstructor());

            container.RegisterType<ILogger,             Logger            >(new HierarchicalLifetimeManager());
            container.RegisterType<IUnitOfWork,         UnitOfWork        >(new HierarchicalLifetimeManager());
            container.RegisterType<IBlogpostRepository, BlogpostRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ITagRepository,      TagRepository     >(new HierarchicalLifetimeManager());
            container.RegisterType<IFileRepository,     FileRepository    >(new HierarchicalLifetimeManager());
        }
    }
}
