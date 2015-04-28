#region Usings

using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.SqlServer;
using HinesSite.Logging;

#endregion

namespace HinesSite.Data.Context {

    /// <summary>
    /// This class enables connection resiliency with databases by deriving from the DbConfiguration class.
    ///   It will retry SQL commands for you and is already setup for Azure by default. EF automatically
    ///   runs code that derives from DbConfiguration to do config tasks
    /// </summary>
    public class BlogConfiguration : DbConfiguration {

        /// <summary>
        /// BlogConfiguration's default constructor, setting DB related configurations
        /// </summary>
        public BlogConfiguration() {

            // Setting up the SQL DB execution strategy (A.K.A. the retry policy)
            SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());

            // These were already added in the Global.asax file but can be added here also
            //DbInterception.Add(new BlogInterceptorTransientErrors());
            DbInterception.Add(new BlogInterceptorLogging());
        }
    }
}