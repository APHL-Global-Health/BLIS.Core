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

namespace Xrw
{

	/// <summary> The windowed base class for dialog shells, that interact with the window manager. </summary>
	/// <remarks> The member attribute Window contains the *** own *** window pointer. </remarks>
	public class XrwDialogShell : XrwTransientShell
	{

        // ###############################################################################
        // ### C O N S T A N T S
        // ###############################################################################

        #region Constants

        /// <summary> The class name constant. </summary>
        public new const string		CLASS_NAME = "XrwDialogShell";
		
		public enum Result
		{
			/// <summary> Dialog shell without result / initial state / still running. </summary>
			None		= -1,
			
			/// <summary> Dialog shell ended by cancel. </summary>
			Cancel		= 0,
			
			/// <summary> Dialog shell ended by OK. </summary>
			OK = 1
		}
		
        #endregion

		// ###############################################################################
        // ### A T T R I B U T E S
        // ###############################################################################

		#region Attributes

        #endregion

		// ###############################################################################
        // ### E V E N T S
        // ###############################################################################

		#region Events
		
		/// <summary> Register DialogShellEnd event handler. </summary>
		public event DialogShellEndDelegate DialogShellEnd;

        #endregion
		
        // ###############################################################################
        // ### C O N S T R U C T I O N   A N D   I N I T I A L I Z A T I O N
        // ###############################################################################

        #region Construction
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="parent"> The parent (associated top-level) shell (an override shell can't be a top-level shell. <see cref="IntPtr"/> </param>
		public XrwDialogShell (XrwApplicationShell parent)
			: base (parent)
		{	;	}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="parent"> The parent (associated top-level) shell (an override shell can't be a top-level shell. <see cref="IntPtr"/> </param>
		/// <param name="assignedPosition"> The position of the top left top corner assigned by the window manager (for shell widgets) or geometry management (by non-shell widgets). Passed as reference to avoid structure copy constructor calls. <see cref="TPoint"/> </param>
		public XrwDialogShell (XrwApplicationShell parent, ref TPoint assignedPosition)
			: base (parent, ref assignedPosition)
		{	;	}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="diaplay">The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window for X11 calls. This widget has no X11 *** own *** window. <see cref="IntPtr"/> </param>
		/// <param name="fixedSize"> The fixed size, that ist to use (if set) rather than the calcualated size. <see cref="TSize"/> </param>
		public XrwDialogShell (XrwApplicationShell parent, ref TSize fixedSize)
			: base (parent, ref fixedSize)
		{	;	}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="diaplay">The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window for X11 calls. This widget has no X11 *** own *** window. <see cref="IntPtr"/> </param>
		/// <param name="assignedPosition"> The position of the top left top corner assigned by the window manager (for shell widgets) or geometry management (by non-shell widgets). Passed as reference to avoid structure copy constructor calls. <see cref="TPoint"/> </param>
		/// <param name="fixedSize"> The fixed size, that ist to use (if set) rather than the calcualated size. <see cref="TSize"/> </param>
		public XrwDialogShell (XrwApplicationShell parent, ref TPoint assignedPosition, ref TSize fixedSize)
			: base (parent, ref assignedPosition, ref fixedSize)
		{	;	}
		
        #endregion
		
        #region Event handler

		/// <summary> Handle the ClientMessage event. </summary>
		/// <param name="data"> The event data. <see cref="System.Object"/> </param>
		public void OnEnd (object data)
		{
			DialogShellEndDelegate dialogShellEnd = DialogShellEnd;
			if (dialogShellEnd != null)
				dialogShellEnd (this, data);
		}
		
        #endregion
		
	}
}

