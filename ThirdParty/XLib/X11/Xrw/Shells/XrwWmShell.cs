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

using X11;

namespace Xrw
{
	/// <summary> The windowless fundamental widget class for shells, that interact wit the windows manager. </summary>
	/// <remarks> The member attribute Window contains the *** parent *** window pointer. </remarks>
	public class XrwWmShell : XrwShell
	{

        // ###############################################################################
        // ### C O N S T A N T S
        // ###############################################################################

        #region Constants

        /// <summary> The class name constant. </summary>
        public new const string		CLASS_NAME = "XrwWmShell";
	
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
		
		/// <summary> Minimal initializing constructor. Use this constructor only for *** shell classes *** only! </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		protected XrwWmShell (IntPtr display)
			: base (display,
			        X11lib.XDefaultScreen(display),		/* Select the current screen to display on. */
			        X11lib.XDefaultRootWindow(display))	/* Select display's root window as parent window. */
		{	;	}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window at construction time for X11 calls. Will be overwritten by an *** own *** window wuring initialization. <see cref="IntPtr"/> </param>
		public XrwWmShell (IntPtr display, X11.TInt screenNumber, IntPtr parentWindow)
			: base (display, screenNumber, parentWindow)
		{	;	}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window at construction time for X11 calls. Will be overwritten by an *** own *** window wuring initialization. <see cref="IntPtr"/> </param>
		/// <param name="fixedSize"> The fixed size, that ist to use (if set) rather than the calcualated size. Passed as reference to avoid structure copy constructor calls. <see cref="TSize"/> </param>
		public XrwWmShell (IntPtr display, X11.TInt screenNumber, IntPtr parentWindow, ref TSize fixedSize)
			: base (display, screenNumber, parentWindow, ref fixedSize)
		{	;	}
		
        #endregion
		
        // ###############################################################################
        // ### D E S T R U C T I O N
        // ###############################################################################

        #region Destruction

        #endregion
		
        // ###############################################################################
        // ### P R O P E R T I E S
        // ###############################################################################

		#region Properties
		
		#endregion
		
        // ###############################################################################
        // ### M E T H O D S
        // ###############################################################################

		#region Methods
		
		#endregion
		
        #region Event handler
		
		/// <summary> Handle the ConfigureEvent event. </summary>
		/// <param name="e"> The event data. <see cref="XawClientMessageEvent"/> </param>
		/// <remarks> Set XawClientMessageEvent. Set result to nonzero to stop further event processing. </remarks>
		public virtual void OnConfigure (XrwConfigureEvent e)
		{
			// Prevent useless reconfiguration, if widt / height did not change.
			if (_assignedSize.Width  != (int)e.Event.width ||
			    _assignedSize.Height != (int)e.Event.height  )
			{
				_assignedPosition.X = (int)e.Event.x;
				_assignedPosition.Y = (int)e.Event.y;
				_assignedSize.Width = (int)e.Event.width;
				_assignedSize.Height = (int)e.Event.height;
				
				for (int cntChildren = 0; cntChildren < _children.Count; cntChildren++)
				{
					XrwRectObj widget = _children[cntChildren];
					// Every direct child can occupy the complete size.
					GeometryManagerAccess.SetAssignedGeometry (widget, new TPoint (0, 0), _assignedSize);
				}
			}
			// Position can change without the nessesarity to calculate a new geometry.
			else
			{
				_assignedPosition.X = (int)e.Event.x;
				_assignedPosition.Y = (int)e.Event.y;
			}
		}
		
		#endregion
		
	}
}

