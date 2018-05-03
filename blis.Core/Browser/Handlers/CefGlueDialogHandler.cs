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

using System.Windows.Forms;
using Xilium.CefGlue;

namespace blis.Core.Browser.Handlers
{
    public class CefGlueDialogHandler : CefDialogHandler
    {        
        protected override bool OnFileDialog(CefBrowser browser, CefFileDialogMode mode, string title, string defaultFilePath, string[] acceptFilters, int selectedAcceptFilter, CefFileDialogCallback callback)
        {
            if (mode == (CefFileDialogMode.Save | CefFileDialogMode.OverwritePromptFlag | CefFileDialogMode.HideReadOnlyFlag))
            {
                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = defaultFilePath;
                //saveFileDialog.Filter = string.Join("Supported Files|*", acceptFilters);
                if (saveFileDialog.ShowDialog() == DialogResult.OK && saveFileDialog.FileName != "")
                    callback.Continue(selectedAcceptFilter, saveFileDialog.FileNames);
            }
            else if (mode == (CefFileDialogMode.OpenFolder | CefFileDialogMode.OverwritePromptFlag | CefFileDialogMode.HideReadOnlyFlag))
            {
                var dialog = new FolderBrowserDialog();
                if (dialog.ShowDialog() == DialogResult.OK && dialog.SelectedPath != "")
                    callback.Continue(selectedAcceptFilter, new string[] { dialog.SelectedPath });
            }
            else if (mode == (CefFileDialogMode.OverwritePromptFlag | CefFileDialogMode.HideReadOnlyFlag))
            {
                var dialog = new OpenFileDialog();
                dialog.Multiselect = mode == CefFileDialogMode.OpenMultiple;
                if (dialog.ShowDialog() == DialogResult.OK && dialog.FileName != "")
                    callback.Continue(selectedAcceptFilter, dialog.FileNames);
            }
            //else callback.Cancel();

            return base.OnFileDialog(browser, mode, title, defaultFilePath, acceptFilters, selectedAcceptFilter, callback);
        }
    }
}
