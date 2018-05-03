// =====================
// The "Roma Widget Set"
// =====================

/*
 * Created by Mono Develop 2.4.1.
 * User: PloetzS
 * Date: November 2013
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

	/// <summary> The windowed toggle button widget. </summary>
	/// <remarks> The member attribute Window contains the window pointer of the *** own *** window. </remarks>
	public class XrwRadio : XrwToggle
	{

        // ###############################################################################
        // ### C O N S T A N T S
        // ###############################################################################

        #region Constants

        /// <summary> The class name constant. </summary>
        public new const string	CLASS_NAME = "XrwRadio";
		
        #endregion

		// ###############################################################################
        // ### A T T R I B U T E S
        // ###############################################################################

		#region Attributes

        #endregion
		
        // ###############################################################################
        // ### C O N S T R U C T I O N   A N D   I N I T I A L I Z A T I O N
        // ###############################################################################

        #region Construction
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window that will be the parent for an X11 *** own ** window. <see cref="IntPtr"/> </param>
		/// <param name="label"> The label to display. <see cref="System.String"/> </param>
		/// <param name="offBitmap"> The right bitmap to display. <see cref="XrwBitmap"/> </param>
		/// <param name="offShared"> Indicate wether the offBitmap is shared (desposed by caller) or private (disposed together with tis label). <see cref="System.Boolean"/> </param>
		/// <param name="onBitmap"> The left bitmap to display. <see cref="XrwBitmap"/> </param>
		/// <param name="onShared"> Indicate wether the onBitmap is shared (desposed by caller) or private (disposed together with tis label). <see cref="System.Boolean"/> </param>
		public XrwRadio (IntPtr display, X11.TInt screen, IntPtr parentWindow, string label, X11Graphic offBitmap, bool offShared, X11Graphic onBitmap, bool onShared)
			: base (display, screen, parentWindow, label, offBitmap, offShared, onBitmap, onShared)
		{
		}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window that will be the parent for an X11 *** own ** window. <see cref="IntPtr"/> </param>
		/// <param name="fixedSize"> The fixed size, that ist to use (if set) rather than the calcualated size. <see cref="TIntSize"/> </param>
		/// <param name="label"> The label to display. <see cref="System.String"/> </param>
		/// <param name="offBitmap"> The right bitmap to display. <see cref="XrwBitmap"/> </param>
		/// <param name="offShared"> Indicate wether the offBitmap is shared (desposed by caller) or private (disposed together with tis label). <see cref="System.Boolean"/> </param>
		/// <param name="onBitmap"> The left bitmap to display. <see cref="XrwBitmap"/> </param>
		/// <param name="onShared"> Indicate wether the onBitmap is shared (desposed by caller) or private (disposed together with tis label). <see cref="System.Boolean"/> </param>
		public XrwRadio (IntPtr display, X11.TInt screen, IntPtr parentWindow, ref TSize fixedSize, string label, X11Graphic offBitmap, bool offShared, X11Graphic onBitmap, bool onShared)
			: base (display, screen, parentWindow, ref fixedSize, label, offBitmap, offShared, onBitmap, onShared)
		{
		}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window that will be the parent for an X11 *** own ** window. <see cref="IntPtr"/> </param>
		/// <param name="assignedPosition"> The position of the top left top corner assigned by the window manager (for shell widgets) or geometry management (by non-shell widgets). <see cref="TPoint"/> </param>
		/// <param name="fixedSize"> The fixed size, that ist to use (if set) rather than the calcualated size. <see cref="TIntSize"/> </param>
		/// <param name="label"> The label to display. <see cref="System.String"/> </param>
		/// <param name="offBitmap"> The right bitmap to display. <see cref="XrwBitmap"/> </param>
		/// <param name="offShared"> Indicate wether the offBitmap is shared (desposed by caller) or private (disposed together with tis label). <see cref="System.Boolean"/> </param>
		/// <param name="onBitmap"> The left bitmap to display. <see cref="XrwBitmap"/> </param>
		/// <param name="onShared"> Indicate wether the onBitmap is shared (desposed by caller) or private (disposed together with tis label). <see cref="System.Boolean"/> </param>
		public XrwRadio (IntPtr display, X11.TInt screenNumber, IntPtr parentWindow, ref TPoint assignedPosition, ref TSize fixedSize, string label, X11Graphic offBitmap, bool offShared, X11Graphic onBitmap, bool onShared)
			: base (display, screenNumber, parentWindow, ref assignedPosition, ref fixedSize, label, offBitmap, offShared, onBitmap, onShared)
		{	
		}
		
		#endregion
		
        // ###############################################################################
        // ### D E S T R U C T I O N
        // ###############################################################################

        #region Destruction
		
		/// <summary> IDisposable implementation. </summary>
		public override void Dispose ()
		{	
			if (XrwApplicationSettings.VERBOSE_OUTPUT_TO_CONSOLE)
				Console.WriteLine (CLASS_NAME + "::Dispose ()");
			
			this.DisposeByParent ();
		}

		/// <summary> Dispose by parent. </summary>
		public override void DisposeByParent ()
		{	
			base.DisposeByParent ();
		}
		
		#endregion
		
	}
}

