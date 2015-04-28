#region Usings

using System.Web.Mvc;
using HinesSite.Controllers;
using HinesSite.Data;
using HinesSite.Data.Repository;
using HinesSite.Interface;
using HinesSite.Logging;
using Microsoft.Practices.Unity;
using Unity.Mvc3;

#endregion

namespace HinesSite {

    public static class Bootstrapper {

        public static void Initialise() {

            IUnityContainer container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer() {

            UnityContainer container = new UnityContainer();

            container.RegisterType<AccountController>(new InjectionConstructor());

            container.RegisterType<ILogger,             Logger            >(new HierarchicalLifetimeManager());
            container.RegisterType<IUnitOfWork,         UnitOfWork        >(new HierarchicalLifetimeManager());
            container.RegisterType<IBlogpostRepository, BlogpostRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ITagRepository,      TagRepository     >(new HierarchicalLifetimeManager());
            container.RegisterType<IFileRepository,     FileRepository    >(new HierarchicalLifetimeManager());

            return container;
        }
    }
}