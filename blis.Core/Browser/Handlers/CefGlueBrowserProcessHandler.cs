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

using System;
using System.Reflection;
using Xilium.CefGlue;

namespace blis.Core.Browser.Handlers
{
    public class CefGlueBrowserProcessHandler: CefBrowserProcessHandler
    {
        protected override void OnBeforeChildProcessLaunch(CefCommandLine commandLine)
        {
            Console.WriteLine("AppendExtraCommandLineSwitches: {0}", commandLine);
            Console.WriteLine(" Program == {0}", commandLine.GetProgram());

            // .NET in Windows treat assemblies as native images, so no any magic required.
            // Mono on any platform usually located far away from entry assembly, so we want prepare command line to call it correctly.
            if (Type.GetType("Mono.Runtime") != null)
            {
                if (!commandLine.HasSwitch("cefglue"))
                {
                    var path = new Uri(Assembly.GetEntryAssembly().CodeBase).LocalPath;
                    commandLine.SetProgram(path);

                    var mono = CefRuntime.Platform == CefRuntimePlatform.Linux ? "/usr/bin/mono" : @"C:\Program Files\Mono-2.10.8\bin\monow.exe";
                    commandLine.PrependArgument(mono);

                    //commandLine.AppendSwitch("disable-web-security", "1");

                    //commandLine.AppendSwitch("high-dpi-support", "1");
                    //commandLine.AppendSwitch("force-device-scale-factor", "1");

                    //commandLine.AppendSwitch("no-proxy-server", "1");
                    //commandLine.AppendSwitch("no-sandbox");

                    //commandLine.AppendSwitch("disable-gpu", "1");
                    //commandLine.AppendSwitch("disable-gpu-compositing", "1");
                    //commandLine.AppendSwitch("enable-begin-frame-scheduling", "1");
                    // commandLine.AppendSwitch("disable-smooth-scrolling", "1");

                    commandLine.AppendSwitch("--disable-web-security");
                    commandLine.AppendSwitch("--user-data-dir");
                    commandLine.AppendSwitch("--allow-file-access-to-files");
                    commandLine.AppendSwitch("--enable-media-stream");
                    commandLine.AppendSwitch("cefglue", "w");
                }
            }

            Console.WriteLine("  -> {0}", commandLine);
        }
    }
}
