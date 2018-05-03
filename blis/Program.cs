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
// This is a port from CefGlue.WindowsForms sample of CefGlue. Mostly provided as-is. 
// For more info: https://bitbucket.org/xilium/xilium.cefglue/wiki/Home
// </note>
// --------------------------------------------------------------------------------------------------------------------

#region Using
using blis.Core;
using blis.Core.Configurations;
using blis.Core.Handlers;
using blis.Core.Helpers;
using blis.Core.Infrastructure;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
#endregion

namespace blis
{
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1400:AccessModifierMustBeDeclared", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    public class Program
    {
        #region Main
        public static void Main(string[] args)
        {
            #region FirstChanceException & UnhandledException
            AppDomain.CurrentDomain.FirstChanceException += (s, arg) => {
                Log.Error(arg.Exception);
            };

            AppDomain.CurrentDomain.UnhandledException += (s, arg) => {
                Log.Error("IsTerminating: " + arg.IsTerminating + ", Error: " + (arg.ExceptionObject != null ? arg.ExceptionObject : "") + Environment.NewLine);
            };
            #endregion

            #region Configurations / Browser Host
            try
            {
                #region Configurations
                Configuration configuration = null;
                var path = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);

                try
                {
                    if (!Directory.Exists("Extensions")) Directory.CreateDirectory("Extensions");
                    if (!Directory.Exists("Web")) Directory.CreateDirectory("Web");

                    if (File.Exists(Path.Combine(path, "Configurations.xml"))) configuration = Configuration.Load(Path.Combine(path, "Configurations.xml"));
                    else
                    {
                        configuration = new Configuration("Default", 60, true)
                                            .Save(Path.Combine(path, "Configurations.xml"));
                    }

                    if (!Directory.Exists(Path.Combine("Web", configuration.Application))) Directory.CreateDirectory(Path.Combine("Web", configuration.Application));
                    if (!File.Exists(Path.Combine("Web", configuration.Application, "index.htm"))) File.WriteAllText(Path.Combine("Web", configuration.Application, "index.htm"), Configuration.WebStartupFile());

                }
                catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, true); }

                var config = CefConfiguration
                            .Create()
                            .WithHostTitle("eplug")
                            .WithHostIconFile("logo.ico")
                            .WithAppArgs(args)
                            .WithHostSize(1024, 600) //1440x900
                            .WithLogFile("logs\\eplug.cef_new.log")
                            .WithStartUrl($"http://{configuration.Application}//index.htm")
                            .WithLogSeverity(LogSeverity.Error)
                            .UseDefaultLogger("logs\\eplug_new.log")
                            .UseDefaultResourceSchemeHandler("http", string.Empty)
                            .UseDefaultHttpSchemeHandler("http", "eplug.com")
                            .RegisterSchemeHandler("local", string.Empty, new CustomResourceHandler())
                            .WithCustomSetting(CefSettingKeys.CachePath, Path.Combine(path, "Logs", "Cache"))
                            .WithCustomSetting(CefSettingKeys.ResourcesDirPath, Path.GetDirectoryName(new Uri(Assembly.GetEntryAssembly().CodeBase).LocalPath))
                            .WithCustomSetting(CefSettingKeys.RemoteDebuggingPort, configuration.RemoteDebuggingPort)
                            .WithCustomSetting(CefSettingKeys.NoSandbox, true);
                #endregion

                #region Browser Host
                var app = CefGlueBrowserHost.CreateHost(config);
                if (app != null)
                {
                    app.RegisterServiceAssemblies(Path.Combine(path, "Extensions"));
                    app.ScanAssemblies();
                    app.Run(args);
                }
                #endregion
            }
            catch (Exception exception) { Log.Error(exception); }
            #endregion
        }
        #endregion
    }
}
