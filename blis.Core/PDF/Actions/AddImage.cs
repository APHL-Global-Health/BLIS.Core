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
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.IO;
#endregion

namespace blis.Core.PDF.Actions
{
    public class AddImage : iAction
    {
        public string Url { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public double Width { get; set; }
        public double Height { get; set; }

        public AddImage(dynamic details)
            :base((string)details["Action"])
        {
            Url = (string)details["Url"];
            X = (double)details["X"];
            Y = (double)details["Y"];

            Width = (double)details["Width"];
            Height = (double)details["Height"];
        }

        public override void Process(iPDF owner, ref PdfDocument Document, ref PdfPage Page, ref XGraphics Graphics)
        {
            string globalPath = owner.Directory + "\\Web\\";
            if (!string.IsNullOrEmpty(Url))
            {
                if (Url.StartsWith("../")) Url = Url.Replace("../","").Replace("/", "\\");

                string imgPath = globalPath + Url;
                if (!File.Exists(imgPath))
                {
                    imgPath = globalPath + owner.Project + "\\" + Url;
                    if (!File.Exists(imgPath))
                    {
                        imgPath = "";
                    }
                }

                if (!string.IsNullOrEmpty(imgPath))
                {
                    XImage logo = XImage.FromFile(imgPath);
                    double width = logo.PixelWidth * Width / logo.HorizontalResolution;
                    double height = logo.PixelHeight * Height / logo.HorizontalResolution;
                    Graphics.DrawImage(logo, X, Y, Width, Height);
                }  
            }
        }
    }
}
