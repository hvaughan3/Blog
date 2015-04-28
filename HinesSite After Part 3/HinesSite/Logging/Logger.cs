#region Usings

using System;
using System.Diagnostics;
using System.Text;
using HinesSite.Interface;

#endregion

namespace HinesSite.Logging {

    /// <summary>
    /// Uses the System diagnostics to do the tracing. Many "listeners" can be used with System.Diagnostics tracing to write
    /// logs to files or to write them to blob storage in Azure. For more links and options:
    /// http://www.windowsazure.com/en-us/develop/net/tutorials/troubleshoot-web-sites-in-visual-studio/S
    /// </summary>
    public class Logger : ILogger {

        /// <summary>
        /// Writes an Information related message using Trace
        /// </summary>
        public void Information(string message) {

            Trace.TraceInformation("\n" + message + "\n");
        }

        /// <summary>
        /// Writes an Information related message using Trace
        /// </summary>
        public void Information(string fmt, params object[] vars) {

            Trace.TraceInformation(fmt, vars);
        }

        /// <summary>
        /// Writes an Information related message using Trace
        /// </summary>
        public void Information(Exception exception, string fmt, params object[] vars) {

            Trace.TraceInformation("\n" + FormatExceptionMessage(exception, fmt, vars) + "\n");
        }

        /// <summary>
        /// Writes a Warning related message using Trace
        /// </summary>
        public void Warning(string message) {

            Trace.TraceWarning("\n" + message + "\n");
        }

        /// <summary>
        /// Writes a Warning related message using Trace
        /// </summary>
        public void Warning(string fmt, params object[] vars) {

            Trace.TraceWarning(fmt, vars);
        }

        /// <summary>
        /// Writes a Warning related message using Trace
        /// </summary>
        public void Warning(Exception exception, string fmt, params object[] vars) {

            Trace.TraceWarning("\n" + FormatExceptionMessage(exception, fmt, vars) + "\n");
        }

        /// <summary>
        /// Writes an Error related message using Trace
        /// </summary>
        public void Error(string message) {

            Trace.TraceError("\n" + message + "\n");
        }

        /// <summary>
        /// Writes an Error related message using Trace
        /// </summary>
        public void Error(string fmt,params object[] vars) {

            Trace.TraceError(fmt, vars);
        }

        /// <summary>
        /// Writes an Error related message using Trace
        /// </summary>
        public void Error(Exception exception,string fmt,params object[] vars) {

            Trace.TraceError("\n" + FormatExceptionMessage(exception, fmt, vars) + "\n");
        }

        /// <summary>
        /// Writes a Trace API related message using Trace
        /// </summary>
        public void TraceApi(string componentName, string method, TimeSpan timespan) {

            TraceApi(componentName, method, timespan, "");
        }

        /// <summary>
        /// Writes a Trace API related message using Trace
        /// </summary>
        public void TraceApi(string componentName,string method,TimeSpan timespan, string fmt, params object[] vars) {

            TraceApi(componentName,method,timespan, string.Format(fmt, vars));
        }

        /// <summary>
        /// Writes a Trace API related message using Trace
        /// </summary>
        public void TraceApi(string componentName,string method,TimeSpan timespan,string properties) {

            string message = string.Concat("Component:", componentName, method, ";Timespan:", timespan.ToString(), ";Properties:", properties);
            Trace.TraceInformation("\n" + message + "\n");
        }

        /// <summary>
        /// Formats an passed in exception for the Trace Logging methods
        /// </summary>
        /// <returns>string</returns>s
        private static string FormatExceptionMessage(Exception exception, string fmt, object[] vars) {

            /*
             * Simple exception formating: for a more comprehensive version, see:
             *   http://code.msdn.microsoft.com/windowsazure/Fix-It-app-for-Building-cdd80df4
             */
            return "\n" + string.Format(fmt, vars) + " Exception: " + exception + "\n";
        }
    }
}