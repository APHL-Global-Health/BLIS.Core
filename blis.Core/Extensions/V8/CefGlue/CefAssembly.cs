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

using blis.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xilium.CefGlue;

namespace blis.Core.Extensions.V8.CefGlue
{
    public class CefAssembly : CefV8Handler
    {
        #region Valid Types
        private List<Type> validTypes = new List<Type>()
            {
                typeof(void),
                typeof(string),
                typeof(int),
                typeof(double),
                typeof(DateTime),
                typeof(bool),
                typeof(uint),
                typeof(Enum),
                typeof(char),
                typeof(sbyte),
                typeof(byte),
                typeof(short),
                typeof(ushort),
                typeof(long),
                typeof(ulong),
                typeof(float),
                typeof(decimal),
                typeof(string[]),
                typeof(int[]),
                typeof(double[]),
                typeof(DateTime[]),
                typeof(bool[]),
                typeof(uint[]),
                typeof(Enum[]),
                typeof(char[]),
                typeof(sbyte[]),
                typeof(byte[]),
                typeof(short[]),
                typeof(ushort[]),
                typeof(long[]),
                typeof(ulong[]),
                typeof(float[]),
                typeof(decimal[])
            };
        #endregion

        #region Constructor
        public CefAssembly(CefBrowser browser, CefFrame frame, CefV8Context context)
        {
        }
        #endregion

        #region CreateObject
        public void CreateObject(CefV8Value handler, string assemblyName, string @namespace = "")
        {            
            Assembly assmebly = null;
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (a.GetName().Name == assemblyName)
                    assmebly = a;
            }

            if (assmebly != null)
                CreateObject(handler, assmebly, @namespace);
        }

        public void CreateObject(CefV8Value handler, List<string> assemblies, string @namespace = "")
        {
            foreach (var name in assemblies.Distinct())
            {
                foreach (var assmebly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (assmebly.GetName().Name == name)
                        CreateObject(handler, assmebly, @namespace);
                }
            }
        }

        public void CreateObject(CefV8Value handler, List<Assembly> assemblies, string @namespace = "")
        {
            foreach (var assmebly in assemblies)
            {
                CreateObject(handler, assmebly, @namespace);
            }
        }

        public void CreateObject(CefV8Value handler, Assembly assmebly, string @namespace = "")
        {          
            if (assmebly != null)
            {
                foreach (var type in assmebly.GetTypes())
                {
                    if (string.IsNullOrEmpty(@namespace) || (!string.IsNullOrEmpty(@namespace) && type.Namespace == @namespace))
                    {
                        CefV8Value methodHandler = null;
                        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
                        foreach (var method in methods)
                        {
                            if (validTypes.Contains(method.ReturnType))
                            {
                                var parameters = method.GetParameters();
                                var p = parameters.Where(ss => validTypes.Contains(ss.ParameterType)).ToList();
                                if (parameters.Length == p.Count)
                                {
                                    if (methodHandler == null) methodHandler = CefV8Value.CreateFunction(type.FullName, this);
                                    methodHandler.SetValue(method.Name, CefV8Value.CreateFunction(type.FullName + "." + method.Name, this), CefV8PropertyAttribute.None);
                                }
                            }
                        }
                        
                        if (methodHandler != null)
                        {
                            CefV8Value current = handler;
                            var sections = type.FullName.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < sections.Length; i++)
                            {
                                var section = sections[i];
                                if (current.GetKeys().Contains(section))
                                {
                                    if (i + 1 == sections.Length) current.SetValue(section, methodHandler, CefV8PropertyAttribute.None);
                                    else current = current.GetValue(section);
                                }
                                else
                                {
                                    if (i + 1 == sections.Length)
                                        current.SetValue(section, methodHandler, CefV8PropertyAttribute.None);
                                    else
                                    {
                                        var temp = CefV8Value.CreateObject();
                                        current.SetValue(section, temp, CefV8PropertyAttribute.None);
                                        current = temp;
                                    }
                                }
                            }
                        }
                    }
                }
            }
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

                var className = name.Substring(0, name.LastIndexOf('.'));
                var methodName = name.Substring(name.LastIndexOf('.') + 1);
                
                if (!string.IsNullOrEmpty(className) && !string.IsNullOrEmpty(methodName))
                {
                    var type = GetClassType(className);
                    if (type != null)
                    {
                        var ret = false;
                        foreach (var method in type.GetMethods())
                        {
                            if (method.Name == methodName && validTypes.Contains(method.ReturnType))
                            {
                                var parameters = method.GetParameters();
                                var pc = parameters.Where(ss => validTypes.Contains(ss.ParameterType)).Count();
                                if (parameters.Length == pc && arguments.Length >= parameters.Length)
                                {
                                    var args = new List<object>();
                                    CefV8Value callback = null;
                                    if (parameters.Length + 1 == arguments.Length) callback = arguments[arguments.Length - 1];

                                    for (int x = 0; x < parameters.Length; x++)
                                    {
                                        var a = Extension.ConvertToValue(arguments[x]);
                                        var p = parameters[x];
                                        if (a == null || (a != null && a.GetType() == p.ParameterType)) args.Add(a);
                                    }

                                    if (method.ReturnType != typeof(void))
                                    {
                                        if (callback != null && callback.IsFunction)
                                        {
                                            ret = true;
                                            var runner = CefTaskRunner.GetForCurrentThread();
                                            new Task(() =>
                                            {
                                                context.Enter();
                                                var value = method.Invoke(this, args.ToArray());
                                                if (runner != null) runner.PostTask(new CallbackTask(context, callback, value));
                                                context.Exit();
                                            }).Start();
                                        }
                                        else
                                        {
                                            returnValue = InvokeMethod(method, args, method.ReturnType);
                                            if (returnValue != null) ret = true;
                                        }
                                    }                                    
                                    break;
                                }
                            }
                        }
                        return ret;
                    }
                    else
                    {
                        type = GetClassType(name);
                        if (type != null)
                        {
                            var args = new List<object>();
                            var argTypes = new List<Type>();
                            for (int x = 0; x < arguments.Length; x++)
                            {
                                var result = arguments[x];
                                if (result.IsBool)
                                {
                                    args.Add(result.GetBoolValue());
                                    argTypes.Add(typeof(bool));
                                }
                                else if (result.IsDate)
                                {
                                    args.Add(result.GetDateValue());
                                    argTypes.Add(typeof(DateTime));
                                }
                                else if (result.IsInt)
                                {
                                    args.Add(result.GetIntValue());
                                    argTypes.Add(typeof(int));
                                }
                                else if (result.IsDouble)
                                {
                                    args.Add(result.GetDoubleValue());
                                    argTypes.Add(typeof(double));
                                }
                                else if (result.IsNull || result.IsUndefined)
                                {
                                    args.Add(null);
                                    argTypes.Add(null);
                                }
                                else if (result.IsString)
                                {
                                    args.Add(result.GetStringValue());
                                    argTypes.Add(typeof(string));
                                }
                                else if (result.IsUInt)
                                {
                                    args.Add(result.GetUIntValue());
                                    argTypes.Add(typeof(uint));
                                }
                                else if (result.IsArray)
                                {
                                    var values = new List<dynamic>();
                                    for (int i = 0; i < result.GetArrayLength(); i++)
                                    {
                                        var value = result.GetValue(i);
                                        values.Add(Extension.ConvertToV8Value(value));
                                    }
                                    args.Add(values);
                                    argTypes.Add(typeof(Array));
                                }
                            }

                            ConstructorInfo constructor = null;
                            foreach(var constr in type.GetConstructors())
                            {
                                var parameters = constr.GetParameters().Where(ss => validTypes.Contains(ss.ParameterType)).ToList();
                                if (parameters.Count == argTypes.Count)
                                {
                                    constructor = type.GetConstructor(argTypes.ToArray());
                                    if (constructor == null)
                                    {
                                        for (int x = 0; x < parameters.Count; x++)
                                        {
                                            var a = argTypes[x];
                                            var p = parameters[x];
                                            var v = args[x];

                                            if (p.ParameterType != a)
                                            {
                                                argTypes[x] = p.ParameterType;
                                                if (v != null)
                                                {
                                                    if(p.ParameterType == typeof(char[])  && a == typeof(string))
                                                        args[x] = ((string)v).ToCharArray();
                                                    else
                                                        args[x] = Convert.ChangeType(v, p.ParameterType);
                                                }
                                            }
                                        }
                                        constructor = type.GetConstructor(argTypes.ToArray());
                                        if (constructor != null) break;
                                    }
                                    else break;
                                }

                            }

                            returnValue = InvokeContructor(constructor, args, type);
                            if (returnValue != null) return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                returnValue = CefV8Value.CreateNull();
                exception = ex.Message;
                return true;
            }
        }
        #endregion

        #region Invoke
        private CefV8Value InvokeContructor(ConstructorInfo constructor, List<object> args, Type type)
        {
            if (constructor != null)
            {
                if (type == typeof(bool)) return CefV8Value.CreateBool((bool)constructor.Invoke(args.ToArray()));
                else if (type == typeof(DateTime)) return CefV8Value.CreateDate((DateTime)constructor.Invoke(args.ToArray()));
                else if (type == typeof(double)) return CefV8Value.CreateDouble((double)constructor.Invoke(args.ToArray()));
                else if (type == typeof(int)) return CefV8Value.CreateInt((int)constructor.Invoke(args.ToArray()));
                else if (type == typeof(string)) return CefV8Value.CreateString((string)constructor.Invoke(args.ToArray()));
                else if (type == typeof(uint)) return CefV8Value.CreateUInt((uint)constructor.Invoke(args.ToArray()));
                else if (type == typeof(string[]) ||
                    type == typeof(int[]) ||
                    type == typeof(double[]) ||
                    type == typeof(DateTime[]) ||
                    type == typeof(bool[]) ||
                    type == typeof(uint[]) ||
                    type == typeof(Enum[]))
                {
                    return Extension.ConvertToV8Value(constructor.Invoke(args.ToArray()));
                }
            }
            return null;
        }

        private CefV8Value InvokeMethod(MethodInfo method, List<object> args, Type type)
        {
            if (method != null)
            {
                if (type == typeof(bool)) return CefV8Value.CreateBool((bool)method.Invoke(this, args.ToArray()));
                else if (type == typeof(DateTime)) return CefV8Value.CreateDate((DateTime)method.Invoke(this, args.ToArray()));
                else if (type == typeof(double)) return CefV8Value.CreateDouble((double)method.Invoke(this, args.ToArray()));
                else if (type == typeof(int)) return CefV8Value.CreateInt((int)method.Invoke(this, args.ToArray()));
                else if (type == typeof(string)) return CefV8Value.CreateString((string)method.Invoke(this, args.ToArray()));
                else if (type == typeof(uint)) return CefV8Value.CreateUInt((uint)method.Invoke(this, args.ToArray()));
                else if (type == typeof(string[]) ||
                    type == typeof(int[]) ||
                    type == typeof(double[]) ||
                    type == typeof(DateTime[]) ||
                    type == typeof(bool[]) ||
                    type == typeof(uint[]) ||
                    type == typeof(Enum[]))
                {
                    return Extension.ConvertToV8Value(method.Invoke(this, args.ToArray()));
                }
            }
            return null;
        }
        #endregion

        #region GetClassType
        private Type GetClassType(string className)
        {
            var type = Type.GetType(className);
            if (type == null)
            {
                foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
                {
                    type = a.GetType(className);
                    if (type != null)
                        break;
                }
            }
            return type;
        }
        #endregion
    }
}
