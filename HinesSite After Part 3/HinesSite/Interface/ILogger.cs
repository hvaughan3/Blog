#region Usings

using System;

#endregion
// ReSharper disable MissingXmlDoc

namespace HinesSite.Interface {

    /// <summary>
    /// Logging Interface: provides 3 tracing levels to indicate importance of logs and TraceApi, designed to give latency info for external
    //   service calls such as DB queries.
    /// </summary>
    public interface ILogger {

        void Information(string message);
        void Information(string fmt, params object[] vars);
        void Information(Exception exception, string fmt, params object[] vars);

        void Warning(string message);
        void Warning(string fmt, params object[] vars);
        void Warning(Exception exception, string fmt, params object[] vars);

        void Error(string message);
        void Error(string fmt, params object[] vars);
        void Error(Exception exception, string fmt, params object[] vars);

        void TraceApi(string componentName, string method, TimeSpan timespan);
        void TraceApi(string componentName, string method, TimeSpan timespan, string properties);
        void TraceApi(string componentName, string method, TimeSpan timespan, string fmt, params object[] vars);
    }
}