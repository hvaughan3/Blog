#region Usings

using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Diagnostics;
using HinesSite.Interface;

#endregion
// ReSharper disable MissingXmlDoc

namespace HinesSite.Logging {

    /// <summary>
    /// Interceptor class that EF will call into every time it is going to send a query to the DB. This one will be used
    ///   for logging and must derive from the DbCommandInterceptor class. Method overrides will be used which will be
    ///   automatically called when a query is about to be executed.
    /// For successful queries and commands the code writes an Information log with latency and for exceptions, it creates an Error log
    /// </summary>
    public class BlogInterceptorLogging : DbCommandInterceptor {

        private          ILogger      _logger = new Logger();
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public override void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext) {

            base.ScalarExecuting(command, interceptionContext);
            _stopwatch.Restart();
        }

        public override void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext) {

            _stopwatch.Stop();

            if(interceptionContext.Exception != null) {
                _logger.Error(interceptionContext.Exception, "Error executing command: {0}", command.CommandText);
            }
            else {
                _logger.TraceApi("SQL Database", "BlogInterceptor.ScalarExecuted", _stopwatch.Elapsed, "Command: {0}: ", command.CommandText);
            }
            base.ScalarExecuted(command, interceptionContext);
        }

        public override void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext) {

            base.NonQueryExecuting(command, interceptionContext);
            _stopwatch.Restart();
        }

        public override void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext) {

            _stopwatch.Stop();

            if(interceptionContext.Exception != null) {
                _logger.Error(interceptionContext.Exception, "Error executing command: {0}", command.CommandText);
            }
            else {
                _logger.TraceApi("SQL Database", "BlogInterceptor.NonQueryExecuted", _stopwatch.Elapsed, "Command: {0}: ", command.CommandText);
            }
            base.NonQueryExecuted(command, interceptionContext);
        }

        public override void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext) {

            base.ReaderExecuting(command, interceptionContext);
            _stopwatch.Restart();
        }
        public override void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext) {

            _stopwatch.Stop();

            if(interceptionContext.Exception != null) {
                _logger.Error(interceptionContext.Exception, "Error executing command: {0}", command.CommandText);
            }
            else {
                _logger.TraceApi("SQL Database", "BlogInterceptor.ReaderExecuted", _stopwatch.Elapsed, "Command: {0}: ", command.CommandText);
            }
            base.ReaderExecuted(command, interceptionContext);
        }
    }
}