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

using System.Collections.Generic;
using System.IO;

namespace blis.Core.Helpers
{
    public interface IUniqueNameManager
    {
        string GetUniqueName(string inputName, string alternate);
    }

    public class UniqueNameManager : IUniqueNameManager
    {
        private readonly object mapLock = new object();
        private readonly Dictionary<string, uint> map = new Dictionary<string, uint>();

        #region IUniqueNameManager implementation

        public string GetUniqueName(string inputName, string alternate)
        {
            lock (mapLock)
            {
                var nonBlankName = MiscHelpers.EnsureNonBlank(inputName, alternate);

                uint count;
                map.TryGetValue(nonBlankName, out count);

                map[nonBlankName] = ++count;
                return (count < 2) ? nonBlankName : string.Concat(nonBlankName, " [", count, "]");
            }
        }

        #endregion
    }

    internal class UniqueFileNameManager : IUniqueNameManager
    {
        private readonly object mapLock = new object();
        private readonly Dictionary<string, uint> map = new Dictionary<string, uint>();

        #region IUniqueNameManager implementation

        public string GetUniqueName(string inputName, string alternate)
        {
            lock (mapLock)
            {
                var nonBlankName = MiscHelpers.EnsureNonBlank(Path.GetFileNameWithoutExtension(inputName), alternate);
                var extension = Path.GetExtension(inputName);

                uint count;
                map.TryGetValue(nonBlankName, out count);

                map[nonBlankName] = ++count;
                return (count < 2) ? string.Concat(nonBlankName, extension) : string.Concat(nonBlankName, " [", count, "]", extension);
            }
        }

        #endregion
    }
}
