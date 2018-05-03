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
// blis project is licensed under MIT License. MySqlBackup may have additional licensing.
// This is a port from CefGlue.WindowsForms sample of CefGlue. Mostly provided as-is. 
// For more info: https://bitbucket.org/xilium/xilium.cefglue/wiki/Home
// </note>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace MySql.Data.MySqlClient
{
    public class ExportCompleteArgs
    {
        DateTime _timeStart, _timeEnd;
        TimeSpan _timeUsed = new TimeSpan();
        Exception _exception;

        MySqlBackup.ProcessEndType _completionType = MySqlBackup.ProcessEndType.UnknownStatus;

        /// <summary>
        /// The Starting time of export process.
        /// </summary>
        public DateTime TimeStart { get { return _timeStart; } }

        /// <summary>
        /// The Ending time of export process.
        /// </summary>
        public DateTime TimeEnd { get { return _timeEnd; } }

        /// <summary>
        /// Total time used in current export process.
        /// </summary>
        public TimeSpan TimeUsed { get { return _timeUsed;}}

        public MySqlBackup.ProcessEndType CompletionType { get { return _completionType; } }

        public Exception LastError { get { return _exception; } }

        public bool HasError { get { if (LastError != null) return true; return false; } }
        
        public ExportCompleteArgs(DateTime timeStart, DateTime timeEnd, MySqlBackup.ProcessEndType endType, Exception exception)
        {
            _completionType = endType;
            _timeStart = timeStart;
            _timeEnd = timeEnd;
            _timeUsed = timeStart - timeEnd;
            _exception = exception;
        }
    }
}
