#region Usings

using System;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using HinesSite.Interface;

#endregion

namespace HinesSite.Logging {

    /// <summary>
    /// Only overrides the ReaderExecuting method which is called for queries that can return multiple rows of data.
    ///    To check connection resiliency for other types of queries, you can override the NonQueryExecuting and
    ///    ScalarExecuting methods, as the logging interceptor does.
    /// The Transient Error get created when the word "Throw" is entered as a search string. Code below creates a
    ///    dummy SQL DB exception for error #20 (a type known to be transient). Other error #s known to be transient are:
    ///    64, 233, 10053, 10054, 10060, 10928, 10929, 40197, 40501, abd 40613, but these are subject to change in new
    ///    versions of SQL Database.
    /// The code returns the exception to EF instead of running the query and passing back results. The transient exception
    ///    is returned 4 times, and then the code reverts to passing the query to the DB as usual.
    /// The value entered in the search box will be in command.Parameter[0] and [1] (one is used for first name and the other is for
    ///    last name (can probably take the last one out)). When the "Throw" value is found, it is replaced by "po" so that
    ///    blog posts will be found and returned.
    /// </summary>
    public class BlogInterceptorTransientErrors : DbCommandInterceptor {

        #region Properties

        private int     _counter;
        private ILogger _logger = new Logger();

        #endregion

        #region Methods

        /// <summary>
        /// Reads the command to try and find the 'Throw' param value, then initiates the dummy exception below
        /// </summary>
        /// <param name="command">The SQL command being executed</param>
        /// <param name="interceptionContext">The context of the current SQL command</param>
        public override void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext) {

            bool throwTransientErrors = false;

            if(command.Parameters.Count > 0 && command.Parameters[0].Value.ToString() == "Throw") {

                throwTransientErrors        = true;
                command.Parameters[0].Value = "po";
                command.Parameters[1].Value = "po";
            }

            if(throwTransientErrors && _counter < 4) {

                _logger.Information("Returning transient error for command: {0}", command.CommandText);
                _counter++;

                interceptionContext.Exception = CreateDummySqlException();
            }
        }

        /// <summary>
        /// Creates the Dummy Transient Error Exception
        /// </summary>
        /// <returns>SqlException</returns>
        private SqlException CreateDummySqlException() {

            // The instance of SQL Server you attempted to connect to does not support encryption
            const int sqlErrorNumber = 20;

            ConstructorInfo sqlErrorCtor = typeof(SqlError).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic).Single(c => c.GetParameters().Count() == 7);
                     object sqlError     = sqlErrorCtor.Invoke(new object[] { sqlErrorNumber, (byte)0, (byte)0, "", "", "", 1 });

            object errorCollection = Activator.CreateInstance(typeof(SqlErrorCollection), true);
              MethodInfo addMethod = typeof(SqlErrorCollection).GetMethod("Add", BindingFlags.Instance | BindingFlags.NonPublic);

            addMethod.Invoke(errorCollection, new[] { sqlError });

            ConstructorInfo sqlExceptionCtor = typeof(SqlException).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic).Single(c => c.GetParameters().Count() == 4);
            SqlException    sqlException     = (SqlException)sqlExceptionCtor.Invoke(new [] { "Dummy", errorCollection, null, Guid.NewGuid() });

            return sqlException;
        }

        #endregion
    }
}