// =====================
// The "Roma Widget Set"
// =====================

/*
 * Created by Mono Develop 2.4.1.
 * User: PloetzS
 * Date: April 2013
 * --------------------------------
 * Author: Steffen Ploetz
 * eMail:  Steffen.Ploetz@cityweb.de
 * 
 */

// //////////////////////////////////////////////////////////////////////
//
// Copyright (C) 2013 Steffen Ploetz
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// This copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// //////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

using X11;

using Xrw;

namespace Utils
{
				
	/// <summary> Prototype the WriteStatusText event. </summary>
	/// <param name="message"> The message to write to the status field. <see cref="System.String"/> </param>
	public delegate void WriteStatusTextDelegate (string message);
	
	public static class ApplicationFramework
	{
		
        // ###############################################################################
        // ### C O N S T A N T S
        // ###############################################################################

        #region Constants

        /// <summary> The class name constant. </summary>
        public const string		CLASS_NAME = "ApplicationFramework";
		
        #endregion

		// ###############################################################################
        // ### E V E N T S
        // ###############################################################################

		#region Events
		
		/// <summary> Register write status event handler. </summary>
		public static event WriteStatusTextDelegate WriteStatusText;

		#endregion
		
		/// <summary> Handle the WriteStatus event. </summary>
		/// <param name="message"> The message to write to the status field. <see cref="System.String"/> </param>
		public static void WriteStatus (string message)
		{
			WriteStatusTextDelegate writeStatusText = WriteStatusText;
			if (writeStatusText != null)
				writeStatusText (message);
		}
		
		/// <summary> Set the shell icon for a WM shell. </summary>
		/// <param name="application"> The WM shell to set the icon for. <see cref="XrwWmShell"/> </param>
		/// <param name="iconPath"> The path (relative, if applicable) to the icon to set. <see cref="System.String"/> </param>
		/// <returns> Returns true on success, or false otherwise. <see cref="System.Boolean"/> </returns>
		public static bool SetWmShellIcon (XrwWmShell application, string iconPath)
		{
			bool result = false;
			
			if (application == null)
			{
				Console.WriteLine (CLASS_NAME + "::SetWmShellIcon() ERROR: Paramerter 'application' null.");
				return result;
			}
			if (string.IsNullOrEmpty (iconPath))
			{
				Console.WriteLine (CLASS_NAME + "::SetWmShellIcon() ERROR: Paramerter 'iconPath' null or empty.");
				return result;
			}
			
			using (X11Graphic appIcon			= new X11Graphic (application.Display, application.Screen, iconPath))
			{
				IntPtr     appGraphicPixMap	= appIcon.CreateIndependentGraphicPixmap (application.Display, application.Window);
				IntPtr     appMaskPixMap	= appIcon.CreateIndependentMaskPixmap (application.Display, application.Window);
				if (appGraphicPixMap != IntPtr.Zero && appMaskPixMap != IntPtr.Zero)
				{
					X11lib.XWMHints wmHints = X11lib.XAllocWMHints ();
					wmHints.flags = X11lib.XWMHintMask.IconPixmapHint | X11lib.XWMHintMask.IconPositionHint | X11lib.XWMHintMask.IconMaskHint;
				    wmHints.icon_pixmap = appGraphicPixMap;
					wmHints.icon_mask   = appMaskPixMap;
				    wmHints.icon_x = 0;
				    wmHints.icon_y = 0;
					X11lib.XSetWMHints (application.Display, application.Window, ref wmHints);
					
					result = true;
				}
				else
					Console.WriteLine (CLASS_NAME + "::SetWmShellIcon () ERROR: Can not create application icon.");
			}

			return result;
		}
		
	}
}

