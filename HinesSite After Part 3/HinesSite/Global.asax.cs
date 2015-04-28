#region Usings

using System.Data.Entity.Infrastructure.Interception;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using HinesSite.Data;
using HinesSite.Data.Repository;
using HinesSite.Logging;

#endregion
// ReSharper disable MissingXmlDoc

namespace HinesSite {

    public class MvcApplication : System.Web.HttpApplication {

        protected void Application_Start() {

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            // Below being used for Unity DI
            Bootstrapper.Initialise();

            // Used to create Blob container if one does not already exist
            FileRepository fileRepository = new FileRepository(new UnitOfWork(), new Logger());
            fileRepository.CreateAndConfigureAsync();

            /*
             * The 2 lines below cause the interceptor code to be run when EF sends queries to the DB and they
             *   can be independently enabled and disabled since separate interceptor classes for transient error simulation and
             *   logging were created
             * DbInterception.Add method can be anywhere in the code, not just Application_Start. It can be placed
             *   in the DbConfiguration class used to configure the execution policy.
             */
            //DbInterception.Add(new BlogInterceptorTransientErrors());
            DbInterception.Add(new BlogInterceptorLogging());
        }
    }
}
