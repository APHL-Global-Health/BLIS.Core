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

using blis.Core.Configurations;
using blis.Core.Helpers;
using LitJson;
using Simplexcel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using V8.Net;
using Xilium.CefGlue;

namespace blis
{
    public static class Extension
    {
        #region IsEmpty
        public static bool IsEmpty(string value)
        {
            return string.IsNullOrEmpty(value) || (!string.IsNullOrEmpty(value) && string.IsNullOrEmpty(value.Trim()));
        }
        #endregion

        #region IsNumber
        public static bool IsNumber(this object value)
        {
            if (value == null) return false;
            else return value is sbyte
                || value is byte
                || value is short
                || value is ushort
                || value is int
                || value is uint
                || value is long
                || value is ulong
                || value is float
                || value is double
                || value is decimal;
        }
        #endregion

        #region ConvertToV8List
        public static List<dynamic> ConvertToV8List(CefV8Value[] arguments)
        {
            var args = new List<dynamic>();
            if (arguments != null)
            {
                foreach (var arg in arguments)
                {
                    var value = ConvertToV8Value(arg);
                    if (value != null) args.Add(value);
                }
            }
            return args;
        }

        public static List<dynamic> ConvertToV8List(CefListValue arguments)
        {
            var args = new List<dynamic>();
            if (arguments != null)
            {
                for (var i = 0; i < arguments.Count; i++)
                {
                    var type = arguments.GetValueType(i);
                    object value;
                    switch (type)
                    {
                        case CefValueType.Null: value = null; break;
                        case CefValueType.String: value = arguments.GetString(i); break;
                        case CefValueType.Int: value = arguments.GetInt(i); break;
                        case CefValueType.Double: value = arguments.GetDouble(i); break;
                        case CefValueType.Bool: value = arguments.GetBool(i); break;
                        default: value = null; break;
                    }
                    if (value != null) args.Add(value);
                }
            }
            return args;
        }
        #endregion

        #region ConvertToV8Value
        public static CefV8Value ConvertToV8Value(dynamic result)
        {
            if (result != null)
            {
                if (result is bool) return CefV8Value.CreateBool(result);
                else if (result is DateTime) return CefV8Value.CreateDate(result);
                else if (result is double) return CefV8Value.CreateDouble(result);
                else if (result is int) return CefV8Value.CreateInt(result);
                else if (result is string) return CefV8Value.CreateString(result);
                else if (result is uint) return CefV8Value.CreateUInt(result);
                else if (result is Array)
                {
                    var source = (Array)result;
                    var destination = CefV8Value.CreateArray(source.Length);
                    for (int x = 0; x < source.Length; x++)
                    {
                        destination.SetValue(x, ConvertToV8Value(source.GetValue(x)));
                    }
                    return destination;
                }
                else if (result is System.Collections.IList)
                {
                    var source = (System.Collections.IList)result;
                    var destination = CefV8Value.CreateArray(source.Count);
                    for (int x = 0; x < source.Count; x++)
                    {
                        destination.SetValue(x, ConvertToV8Value(source[x]));
                    }
                    return destination;
                }
                //else throw new NotSupportedException("V8 Value");
            }
            return CefV8Value.CreateNull();
        }
        #endregion

        #region ConvertToV8Value
        public static dynamic ConvertToV8Value(CefV8Value result)
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
            }
            return null;
        }
        #endregion

        #region ConvertToValue
        public static object ConvertToValue(CefV8Value result)
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
                        values.Add(ConvertToValue(value));
                    }
                    return values;
                }
            }
            return null;
        }
        #endregion

        #region CefV8ValueToJson
        public static dynamic CefV8ValueToJson(CefV8Value result)
        {
            if (result != null)
            {                
                if (result.IsObject)
                {
                    string item = "{";
                    var keys = result.GetKeys();
                    for(int x = 0; x < keys.Length; x++)
                    {
                        var key = keys[x];
                        var value = result.GetValue(key);

                        item += "\"" + key + "\" : ";
                        if (value.IsArray)
                        {
                            item += "[";
                            for (int i = 0; i < value.GetArrayLength(); i++)
                                item += CefV8ValueToJson(value.GetValue(i));
                            item += "]";
                        }
                        else
                        {
                            if (value.IsBool) item += value.GetBoolValue();
                            else if (value.IsDouble) item += value.GetDoubleValue();
                            else if (value.IsInt) item += value.GetIntValue();
                            else if (value.IsUInt) item += value.GetUIntValue();
                            else if (value.IsString) item += "\"" + value.GetStringValue() + "\"";
                            else if (value.IsDate) item += "\"" + value.GetDateValue() + "\"";
                            else if (value.IsNull || value.IsUndefined) item += "";
                            else item += "";
                        }
                    }
                    item += "}";
                    return item;
                }
                else if (result.IsArray)
                {
                    string items = "[";
                    for (int i = 0; i < result.GetArrayLength(); i++)
                        items += CefV8ValueToJson(result.GetValue(i));
                    items += "]";
                    return items;
                }                
            }
            return null;
        }
        #endregion

        #region AddHeader
        public static void AddHeader(Worksheet sheet, string cell, string value)
        {
            sheet.Cells[cell] = value;
            sheet.Cells[cell].FontName = "Arial Black";
            sheet.Cells[cell].Bold = true;
        }
        #endregion

        #region AddCellValue
        public static void AddCellValue(Worksheet sheet, int row, int column, object value)
        {
            if (value != null)
            {
                if (value is DateTime) sheet.Cells[row, column] = ((DateTime)value).ToString("yyyy-MM-dd"); // H:mm:ss:fff
                else sheet.Cells[row, column] = value.ToString();
            }
            else sheet.Cells[row, column] = "";

        }
        #endregion
        
        #region ObjectToString
        public static string ObjectToString(object item)
        {
            if (item == null) return "";
            else
            {
                if (item is DateTime) return ((DateTime)item).ToString("dd/MM/yyyy H:mm:ss:fff");
                else return item.ToString();
            }
        }
        #endregion
        
        #region Dont Meet Requirements
        public static CefV8Value DontMeetRequirements(CefV8Context context, CefV8Value callback)
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

        #region GetConfiguration
        public static Configuration GetConfiguration(string file = "Configurations.xml")
        {
            var path = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            if (File.Exists(Path.Combine(path, file)))
                return Configuration.Load(Path.Combine(path, file));

            return null;
        }
        #endregion

        #region IfNullMakeEmpty
        public static string IfNullMakeEmpty(object item)
        {
            if (item == null) return "";
            else return item.ToString();
        }
        #endregion

        #region InvokeIfRequired
        public static void InvokeIfRequired(this System.Windows.Forms.Control control, System.Windows.Forms.MethodInvoker action)
        {
            if (control.InvokeRequired) control.Invoke(action);
            else action();
        }
        #endregion
    }
}
