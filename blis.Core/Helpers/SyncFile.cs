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

using blis.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace blis.Helpers
{
    public class SyncFile
    {
        public string Name { get; set; }
        public string Created { get; set; }
        public string Modified { get; set; }
        public string Extension { get; set; }
        public string Size { get; set; }
        public string Directory { get; set; }

        public SyncFile()
        {
            this.Name = "";
            this.Created = "";
            this.Modified = "";
            this.Extension = "";
            this.Size = "";
            this.Directory = "";
        }

        public SyncFile(string name, string path)
            :this()
        {
            if (File.Exists(name))
            {
                var dir = Path.GetDirectoryName(name).Replace(path.TrimEnd(new char[] { '\\' }), "").TrimStart(new char[] { '\\' });
                
                var info = new FileInfo(name);
                this.Name = info.Name;
                this.Created = info.CreationTime.ToString("yyyy-MM-dd H:mm:ss");
                this.Modified = info.LastWriteTime.ToString("yyy-MM-dd H:mm:ss");
                this.Extension = info.Extension;
                this.Size = info.Length.ToString();
                this.Directory = dir;
            }

            //try
            //{
            //    var root = Environment.CurrentDirectory + "\\Web\\" + ConfigurationHelper.Configurations.Application + "\\";
            //    var source = root + "sync\\" + ConfigurationHelper.Configurations.Application;
            //    var dest = root;
            //
            //    var syncProvider = new SyncProvider(source, dest);
            //    syncProvider.SyncFileSystemOneWay();
            //    syncProvider.DetectChangesOnFileSystem();
            //    var sttt = 0;
            //}
            //catch(Exception ex)
            //{
            //    var sddsds = 0;
            //}
        }

        public static void Synchronize(string destination, List<SyncFile> files, string source, bool update = false)
        {
            if (!string.IsNullOrEmpty(destination) && !System.IO.Directory.Exists(destination))
            {
                try { System.IO.Directory.CreateDirectory(destination); }
                catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, true); }
            }

            if (System.IO.Directory.Exists(destination) && files != null)
            {
                foreach (var file in files)
                {
                    var dir = (string.IsNullOrEmpty(file.Directory) ? "" : file.Directory + "\\");
                    var path = destination + "\\" + dir;
                    if (!string.IsNullOrEmpty(dir) && !System.IO.Directory.Exists(path))
                    {
                        try { System.IO.Directory.CreateDirectory(path); }
                        catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, true); }
                    }

                    if (System.IO.Directory.Exists(path))
                    {
                        try
                        {
                            Uri uri = new Uri(source + "\\" + dir.Replace("\\", "/") + file.Name);
                            if (uri.Scheme == "file") SaveFile(source, destination, dir, file, File.ReadAllBytes(uri.LocalPath), update);
                            else
                            {
                                var client = new WebClient();
                                SaveFile(source, destination, dir, file, client.DownloadData(uri), update);
                                //client.DownloadDataCompleted += (s, args) => 
                                //{
                                //    try { SaveFile(source, destination, dir, file, args.Result, update); }
                                //    catch (Exception ex)
                                //    {
                                //        if(ex.InnerException != null) Log.Trace(ex.InnerException.Message, ex.InnerException.StackTrace, ConfigurationHelper.Configurations.Verbose);
                                //        else Log.Trace(ex.Message, ex.StackTrace, ConfigurationHelper.Configurations.Verbose);
                                //    }
                                //};
                                //client.DownloadDataAsync(uri);
                            }
                        }
                        catch (Exception ex) { Log.Trace(ex.Message, ex.StackTrace, true); }
                    }
                }
            }
        }

        private static void SaveFile(string source, string destination, string directory, SyncFile file, byte[] bytes, bool update)
        {
            var targetSource        = source + "\\" + directory + file.Name;
            var targetDestination   = destination + "\\" + directory + file.Name;
            if (!File.Exists(targetDestination)) File.WriteAllBytes(targetDestination, bytes);
            else
            {
                if (update) File.WriteAllBytes(targetDestination, bytes);
                else
                {
                    var current = new FileInfo(targetDestination);
                    if (file.Size != current.Length.ToString() && file.Modified != current.LastWriteTime.ToString("yyyy-MM-dd H:mm:ss")) File.WriteAllBytes(targetDestination, bytes);
                }

                
            }
        }
    }
}
