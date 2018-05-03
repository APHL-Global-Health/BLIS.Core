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
using blis.Core.Configurations;
using blis.Core.Helpers;
using blis.Core.Infrastructure;
using LitJson;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using V8.Net;
using Xilium.CefGlue;
#endregion

namespace blis.Windows.Extensions.V8.CefGlue
{
    public class V8EngineObject : CefV8Handler
    {
        #region Variables
        private CefV8Value Handler;
        private V8Engine engine;
        private Configuration configuration;
        #endregion

        public Core.Extensions.V8.V8Object v8object { get; private set; }

        #region Constructor
        public V8EngineObject(CefBrowser browser, CefFrame frame, CefV8Context context)
        {
            configuration = Extension.GetConfiguration();

            try
            {
                engine = new V8Engine();
                engine.RegisterType(typeof(System.Object), "Object", true, ScriptMemberSecurity.Locked);
                engine.RegisterType(typeof(Type), "Type", true, ScriptMemberSecurity.Locked);
                engine.RegisterType(typeof(String), "String", true, ScriptMemberSecurity.Locked);
                engine.RegisterType(typeof(Boolean), "Boolean", true, ScriptMemberSecurity.Locked);
                engine.RegisterType(typeof(Array), "Array", true, ScriptMemberSecurity.Locked);
                engine.RegisterType(typeof(System.Collections.ArrayList), null, true, ScriptMemberSecurity.Locked);
                engine.RegisterType(typeof(char), null, true, ScriptMemberSecurity.Locked);
                engine.RegisterType(typeof(int), null, true, ScriptMemberSecurity.Locked);
                engine.RegisterType(typeof(Int16), null, true, ScriptMemberSecurity.Locked);
                engine.RegisterType(typeof(Int32), null, true, ScriptMemberSecurity.Locked);
                engine.RegisterType(typeof(Int64), null, true, ScriptMemberSecurity.Locked);
                engine.RegisterType(typeof(UInt16), null, true, ScriptMemberSecurity.Locked);
                engine.RegisterType(typeof(UInt32), null, true, ScriptMemberSecurity.Locked);
                engine.RegisterType(typeof(UInt64), null, true, ScriptMemberSecurity.Locked);
                engine.RegisterType(typeof(Enumerable), null, true, ScriptMemberSecurity.Locked);
                engine.RegisterType(typeof(File), null, true, ScriptMemberSecurity.Locked);
                
                InternalHandle hSystem = engine.CreateObject();
                engine.DynamicGlobalObject.System = hSystem;
                hSystem.SetProperty(typeof(System.Object)); // (Note: No optional parameters used, so this will simply lookup and apply the existing registered type details above.)
                hSystem.SetProperty(typeof(String));
                hSystem.SetProperty(typeof(Boolean));
                hSystem.SetProperty(typeof(Array));
                engine.GlobalObject.SetProperty(typeof(Type));
                engine.GlobalObject.SetProperty(typeof(System.Collections.ArrayList));
                engine.GlobalObject.SetProperty(typeof(char));
                engine.GlobalObject.SetProperty(typeof(int));
                engine.GlobalObject.SetProperty(typeof(Int16));
                engine.GlobalObject.SetProperty(typeof(Int32));
                engine.GlobalObject.SetProperty(typeof(Int64));
                engine.GlobalObject.SetProperty(typeof(UInt16));
                engine.GlobalObject.SetProperty(typeof(UInt32));
                engine.GlobalObject.SetProperty(typeof(UInt64));
                engine.GlobalObject.SetProperty(typeof(Enumerable));
                engine.GlobalObject.SetProperty(typeof(Environment));
                engine.GlobalObject.SetProperty(typeof(File));
                engine.GlobalObject.SetProperty(typeof(Console), V8PropertyAttributes.Locked, null, true, ScriptMemberSecurity.Locked);

                var db              = engine.CreateObject<V8.DatabaseObject>();
                var console         = engine.CreateObject<V8.ConsoleObject>();
                var window          = engine.CreateObject<V8.WindowObject>();
                var server          = engine.CreateObject<V8.ServerObject>();
                var xhr             = engine.CreateObject<V8.xhrObject>();

                v8object            = engine.CreateObject<Core.Extensions.V8.V8Object>();


                //db.AddParent(this.Parent);
                //console.AddParent(this.Parent);
                //server.AddParent(this.Parent);
                window.AddBrowser(browser);
                
                engine.DynamicGlobalObject.db           = db;
                engine.DynamicGlobalObject.console      = console;
                engine.DynamicGlobalObject.window       = window;
                engine.DynamicGlobalObject.Server       = server;
                engine.DynamicGlobalObject.jsXHR        = xhr;
                engine.DynamicGlobalObject.v8object     = v8object;                
            }
            catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, configuration.Verbose); }
        }
        #endregion

        #region CreateObject
        public CefV8Value CreateObject()
        {
            Handler = CefV8Value.CreateObject();
            Handler.SetValue("compile", CefV8Value.CreateFunction("compile", this), CefV8PropertyAttribute.None);
            Handler.SetValue("consoleExecute", CefV8Value.CreateFunction("consoleExecute", this), CefV8PropertyAttribute.None);
            Handler.SetValue("execute", CefV8Value.CreateFunction("execute", this), CefV8PropertyAttribute.None);
            Handler.SetValue("loadScript", CefV8Value.CreateFunction("loadScript", this), CefV8PropertyAttribute.None);
            Handler.SetValue("loadScriptCompiled", CefV8Value.CreateFunction("loadScriptCompiled", this), CefV8PropertyAttribute.None);
            Handler.SetValue("terminateExecution", CefV8Value.CreateFunction("terminateExecution", this), CefV8PropertyAttribute.None);
            Handler.SetValue("verboseConsoleExecute", CefV8Value.CreateFunction("verboseConsoleExecute", this), CefV8PropertyAttribute.None);
            Handler.SetValue("runStartupFile", CefV8Value.CreateFunction("runStartupFile", this), CefV8PropertyAttribute.None);
            return Handler;
        }
        #endregion

        #region Execute
        protected override bool Execute(string name, CefV8Value obj, CefV8Value[] arguments, out CefV8Value returnValue, out string exception)
        {
            returnValue = CefV8Value.CreateNull();
            exception = null;

            try
            {
                var context = CefV8Context.GetCurrentContext();
                var browser = context.GetBrowser();
                var frame = browser.GetMainFrame();

                if (name == "compile" || name == "loadScript" || name == "loadScriptCompiled")
                {
                    if (arguments.Length >= 3)
                    {
                        var script                  = arguments[0].GetStringValue();
                        var sourceName              = arguments[1].GetStringValue();
                        var throwExceptionOnError   = arguments[2].GetBoolValue();
                        var callback                = (arguments.Length == 4 ? arguments[3] : null);

                        if (!string.IsNullOrEmpty(script) && !string.IsNullOrEmpty(sourceName))
                        {
                            if (callback != null && callback.IsFunction)
                            {
                                var runner = CefTaskRunner.GetForCurrentThread();
                                new Task(() =>
                                {
                                    context.Enter();
                                    if (name == "compile") engine.Compile(script, sourceName, throwExceptionOnError);
                                    else if (name == "loadScript") engine.LoadScript(script, sourceName, throwExceptionOnError);
                                    else if (name == "loadScriptCompiled") engine.LoadScriptCompiled(script, sourceName, throwExceptionOnError);
                                    runner.PostTask(new CallbackTask(context, callback, JsonMapper.ToJson(new StatusHelper("OK", name))));
                                    context.Exit();
                                }).Start();
                            }
                            else
                            {
                                if (name == "compile") engine.Compile(script, sourceName, throwExceptionOnError);
                                else if (name == "loadScript") engine.LoadScript(script, sourceName, throwExceptionOnError);
                                else if (name == "loadScriptCompiled") engine.LoadScriptCompiled(script, sourceName, throwExceptionOnError);
                                returnValue = CefV8Value.CreateString(JsonMapper.ToJson(new StatusHelper("OK", name)));
                            }
                        }
                        else returnValue = DontMeetRequirements(context, callback);
                    }
                    else returnValue = DontMeetRequirements(context, null);
                }
                else if (name == "consoleExecute" || name == "execute" || name == "verboseConsoleExecute")
                {
                    if (arguments.Length >= 4)
                    {
                        var script = arguments[0].GetStringValue();
                        var sourceName = arguments[1].GetStringValue();
                        var throwExceptionOnError = arguments[2].GetBoolValue();
                        var timeOut = arguments[3].GetIntValue();
                        var callback = (arguments.Length == 5 ? arguments[4] : null);

                        if (!string.IsNullOrEmpty(script) && !string.IsNullOrEmpty(sourceName))
                        {
                            if (callback != null && callback.IsFunction)
                            {
                                var runner = CefTaskRunner.GetForCurrentThread();
                                new Task(() =>
                                {
                                    context.Enter();
                                    if (name == "consoleExecute") engine.ConsoleExecute(script, sourceName, throwExceptionOnError, timeOut);
                                    else if (name == "execute") engine.Execute(script, sourceName, throwExceptionOnError, timeOut);
                                    else if (name == "verboseConsoleExecute") engine.VerboseConsoleExecute(script, sourceName, throwExceptionOnError, timeOut);
                                    runner.PostTask(new CallbackTask(context, callback, JsonMapper.ToJson(new StatusHelper("OK", name))));
                                    context.Exit();
                                }).Start();
                            }
                            else
                            {
                                if (name == "consoleExecute") engine.ConsoleExecute(script, sourceName, throwExceptionOnError, timeOut);
                                else if (name == "execute") engine.Execute(script, sourceName, throwExceptionOnError, timeOut);
                                else if (name == "verboseConsoleExecute") engine.VerboseConsoleExecute(script, sourceName, throwExceptionOnError, timeOut);
                                returnValue = CefV8Value.CreateString(JsonMapper.ToJson(new StatusHelper("OK", name)));
                            }
                        }
                        else returnValue = DontMeetRequirements(context, callback);
                    }
                    else returnValue = DontMeetRequirements(context, null);
                }
                else if (name == "terminateExecution")
                {
                    if (arguments.Length >= 0)
                    {
                        var callback = (arguments.Length == 1 ? arguments[0] : null);

                        if (callback != null && callback.IsFunction)
                        {
                            var runner = CefTaskRunner.GetForCurrentThread();
                            new Task(() =>
                            {
                                context.Enter();
                                engine.TerminateExecution();
                                runner.PostTask(new CallbackTask(context, callback, JsonMapper.ToJson(new StatusHelper("OK", "TerminateExecution"))));
                                context.Exit();
                            }).Start();
                        }
                        else
                        {
                            engine.TerminateExecution();
                            returnValue = CefV8Value.CreateString(JsonMapper.ToJson(new StatusHelper("OK", "TerminateExecution")));
                        }
                    }
                    else returnValue = DontMeetRequirements(context, null); ;
                }
                else if (name == "runStartupFile")
                {
                    if (arguments.Length >= 0)
                    {
                        var callback = (arguments.Length == 1 ? arguments[0] : null);

                        if (callback != null && callback.IsFunction)
                        {
                            var runner = CefTaskRunner.GetForCurrentThread();
                            new Task(() =>
                            {
                                context.Enter();

                                var message = "V8 Script Engine : Started";
                                Log.Trace(message, configuration.Verbose);
                                runner.PostTask(new CallbackTask(context, callback, JsonMapper.ToJson(new StatusHelper("OK", message))));

                                context.Exit();
                            }).Start();
                        }
                        else
                        {
                            var message = "V8 Script Engine : Started";
                            Log.Trace(message, configuration.Verbose);
                            returnValue = CefV8Value.CreateString(JsonMapper.ToJson(new StatusHelper("OK", message)));
                        }
                    }
                    else returnValue = DontMeetRequirements(context, null); ;
                }                
                return true;
            }
            catch (Exception ex)
            {
                returnValue = CefV8Value.CreateNull();
                exception = ex.Message;
                return true;
            }
        }
        #endregion

        #region Dont Meet Requirements
        private CefV8Value DontMeetRequirements(CefV8Context context, CefV8Value callback)
        {
            var msg = JsonMapper.ToJson(new StatusHelper("Error", "Arguments do not meet function requirement"));
            if (callback != null)
            {
                var runner = CefTaskRunner.GetForCurrentThread();
                new Task(() =>
                {
                    context.Enter();
                    runner.PostTask(new CallbackTask(context, callback, msg));
                    context.Exit();
                }).Start();
                return CefV8Value.CreateNull();
            }
            else return CefV8Value.CreateString(msg);
        }
        #endregion

    }
}
