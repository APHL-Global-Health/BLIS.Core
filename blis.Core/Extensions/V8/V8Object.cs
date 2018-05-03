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

using System;
using System.Collections.Generic;
using V8.Net;
using Xilium.CefGlue;

namespace blis.Core.Extensions.V8
{
    public class V8Object : V8NativeObject
    {
        #region Class
        internal class V8ObjectHandler : CefV8Handler
        {
            internal V8Object owner = null;

            public V8ObjectHandler(V8Object owner)
            {
                this.owner = owner; 
            }

            #region ConvertToV8Value
            public dynamic ConvertToV8Value(CefV8Value result)
            {
                if (result != null)
                {
                    if (result.IsBool) return result.GetBoolValue();
                    else if (result.IsDate) return result.GetDateValue();
                    else if (result.IsDouble) return result.GetDoubleValue();
                    else if (result.IsInt) return result.GetIntValue();
                    else if (result.IsNull || result.IsUndefined) return null;
                    else if (result.IsString) return result.GetStringValue();
                    else if (result.IsUInt) return result.GetUIntValue();
                    else if (result.IsArray)
                    {
                        var values = new List<dynamic>();
                        for (int i = 0; i < result.GetArrayLength(); i++)
                        {
                            var value = result.GetValue(i);
                            values.Add(ConvertToV8Value(value));
                        }
                        return values;
                    }
                    else if (result.IsFunction)
                    {

                    }
                    else if (result.IsObject)
                    {

                    }
                }
                return null;
            }
            #endregion

            #region ConvertArguments
            public InternalHandle[] ConvertArguments(CefV8Value[] arguments)
            {
                var args = new List<InternalHandle>();
                if (this.owner != null && this.owner.Engine != null)
                {
                    for (int l = 0; l < arguments.Length; l++)
                    {
                        var argument = ConvertToV8Value(arguments[l]);
                        if (argument != null) args.Add(this.owner.Engine.CreateValue(argument));
                        else args.Add(this.owner.Engine.CreateNullValue());
                    }
                }
                return args.ToArray();
            }
            #endregion

            protected override bool Execute(string name, CefV8Value obj, CefV8Value[] arguments, out CefV8Value returnValue, out string exception)
            {
                returnValue = CefV8Value.CreateNull();
                exception = null;

                var args = ConvertArguments(arguments);
                if (this.owner != null && this.owner.Engine != null)
                {
                    if (name == "log") this.owner.log(this.owner.Engine, false, this.owner, args);
                }

                //if (name == "log" && arguments.Length >= 1) log(arguments[0].GetStringValue());
                return true;
            }
        }
        #endregion

        #region Variables
        private CefV8Value V8Value;
        private V8ObjectHandler V8Handler;
        #endregion

        #region Constructor
        public V8Object()
        {
            V8Handler = new V8ObjectHandler(this);
        }

        public V8Object(CefBrowser browser, CefFrame frame, CefV8Context context)
            :this()
        {
        }
        #endregion

        #region CreateObject
        public CefV8Value CreateObject()
        {            
            V8Value = CefV8Value.CreateObject();
            V8Value.SetValue("log", CefV8Value.CreateFunction("log", V8Handler), CefV8PropertyAttribute.None);
            
            return V8Value;
        }
        #endregion

        #region Initialize
        public override InternalHandle Initialize(bool isConstructCall, params InternalHandle[] args)
        {
            this.AsDynamic.log = Engine.CreateFunctionTemplate().GetFunctionObject(log);

            return base.Initialize(isConstructCall, args);
        }
        #endregion

        #region log 
        private InternalHandle log(V8Engine engine, bool isConstructCall, InternalHandle _this, InternalHandle[] args)
        {
            if (args.Length == 1) Infrastructure.Log.Trace(args[0].Value.ToString(), true);
            return null;
        }
        #endregion
    }
}
