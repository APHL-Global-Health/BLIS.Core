// --------------------------------------------------------------------------------------------------------------------
//<license>
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </license>
// <note>
// blis project is licensed under MIT License. CefGlue may have additional licensing.
// </note>
// --------------------------------------------------------------------------------------------------------------------

namespace blis.Core.Infrastructure
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Global logger implementation.
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// The logger object.
        /// </summary>
        private static iLogger mLogger;

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public static iLogger Logger
        {
            get => mLogger ?? (mLogger = GetCurrentLogger);
            set => mLogger = value;
        }

        /// <summary>
        /// Gets the get current logger.
        /// </summary>
        private static iLogger GetCurrentLogger
        {
            get
            {
                var logger = IoC.GetInstance(typeof(iLogger), typeof(Log).FullName);
                if (logger is iLogger ePlugLogger)
                {
                    return ePlugLogger;
                }

                return new SimpleLogger();
            }
        }

        /// <summary>
        /// Logs an info message type.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public static void Info(string message)
        {
            Logger.Info(message);
        }

        /// <summary>
        /// Logs a debug message type.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public static void Debug(string message)
        {
            Logger.Debug(message);
        }

        /// <summary>
        /// Logs a verbose message type.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public static void Verbose(string message)
        {
            Logger.Verbose(message);
        }

        /// <summary>
        /// Logs a warning message type.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public static void Warn(string message)
        {
            Logger.Warn(message);
        }

        /// <summary>
        /// Logs an error message type.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public static void Error(string message)
        {
            Logger.Error(message);
        }

        /// <summary>
        /// Logs an error message type.
        /// </summary>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        public static void Error(Exception exception, string message = null)
        {
            Logger.Error(exception, message);
        }

        /// <summary>
        /// Logs a fatal message type.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public static void Fatal(string message)
        {
            Logger.Fatal(message);
        }

        /// <summary>
        /// Logs a critical message type.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public static void Critial(string message)
        {
            Logger.Fatal(message);
        }

        private static object Lock = new object();


        #region Trace
        public static void Trace(string message,
            bool verbose = false,
            [CallerMemberName] string callingMethod = null,
            [CallerFilePath] string callingFilePath = null,
            [CallerLineNumber] int callingFileLineNumber = 0)
        {
            lock (Lock)
            {
                if (verbose)
                {
                    var trimmed = "passed";
                    if (!string.IsNullOrEmpty(message)) trimmed = message.Trim();

                    if (!string.IsNullOrEmpty(trimmed))
                    {
                        var dt = DateTime.Now.ToString("dd.MM.yyyy H.mm.ss.fff");
                        if (!string.IsNullOrEmpty(callingFilePath) && !string.IsNullOrEmpty(callingMethod))
                        {
                            var msg = "[" + dt + "] " + "Source: " + Path.GetFileNameWithoutExtension(callingFilePath) + ", Line: " + callingFileLineNumber + ", Method: " + callingMethod;
                            if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debug.Write(msg);
                            else Console.Write(msg);

                            if (!string.IsNullOrEmpty(message))
                            {
                                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debug.Write(", Message: " + message);
                                else Console.Write(", Message: " + message);
                            }

                            if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debug.WriteLine("");
                            else Console.WriteLine();
                        }
                    }
                }
            }
        }

        public static void Trace(string message, string stack,
            bool verbose = false,
            [CallerMemberName] string callingMethod = null,
            [CallerFilePath] string callingFilePath = null,
            [CallerLineNumber] int callingFileLineNumber = 0)
        {
            lock (Lock)
            {
                if (verbose)
                {
                    var trimmed = "passed";
                    if (!string.IsNullOrEmpty(message)) trimmed = message.Trim();

                    if (!string.IsNullOrEmpty(trimmed))
                    {
                        var dt = DateTime.Now.ToString("dd.MM.yyyy H.mm.ss.fff");
                        if (!string.IsNullOrEmpty(callingFilePath) && !string.IsNullOrEmpty(callingMethod))
                        {
                            var msg = "[" + dt + "] " + "Source: " + Path.GetFileNameWithoutExtension(callingFilePath) + ", Line: " + callingFileLineNumber + ", Method: " + callingMethod;
                            if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debug.Write(msg);
                            else Console.Write(msg);

                            if (!string.IsNullOrEmpty(message))
                            {
                                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debug.Write(", Message: " + message);
                                else Console.Write(", Message: " + message);

                                if (!string.IsNullOrEmpty(stack))
                                {
                                    if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debug.Write(", Stack: " + stack);
                                    else Console.Write(", Stack: " + stack);
                                }
                            }

                            if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debug.WriteLine("");
                            else Console.WriteLine();
                        }
                    }
                }
            }
        }

        public static void Trace(string source, int line, string message, bool verbose = false)
        {
            lock (Lock)
            {
                if (verbose)
                {
                    var dt = DateTime.Now.ToString("dd.MM.yyyy H.mm.ss.fff");
                    if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debug.WriteLine("[" + dt + "] " + "Source: " + source + ", Line: " + line + ", Message: " + message);
                    else Console.WriteLine("[" + dt + "] " + "Source: " + source + ", Line: " + line + ", Message: " + message);
                }
            }
        }
        #endregion
    }
}