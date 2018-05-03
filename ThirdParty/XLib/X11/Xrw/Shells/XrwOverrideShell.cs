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

	/// <summary> The windowed base class for popup shells, that bypass the window manager. See transient shell for popup shells, that interact with the window manager. </summary>
	/// <remarks> The member attribute Window contains the *** own *** window pointer. </remarks>
	public class XrwOverrideShell : XrwShell
	{

        // ###############################################################################
        // ### C O N S T A N T S
        // ###############################################################################

        #region Constants

        /// <summary> The class name constant. </summary>
        public new const string		CLASS_NAME = "XrwOverrideShell";
		
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
		/// <param name="parent"> The parent (associated top-level) shell (an override shell can't be a top-level shell. <see cref="IntPtr"/> </param>
		public XrwOverrideShell (XrwApplicationShell parent)
			: base (parent.Display, parent.Screen, parent.Window)
		{
			_parent = parent;
			
			TPoint assignedPosition = new TPoint (0, 0);
			InitializeOverrideShellResources (ref assignedPosition);
		}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="parent"> The parent (associated top-level) shell (an override shell can't be a top-level shell. <see cref="IntPtr"/> </param>
		/// <param name="assignedPosition"> The position of the top left top corner assigned by the window manager (for shell widgets) or geometry management (by non-shell widgets). Passed as reference to avoid structure copy constructor calls. <see cref="TPoint"/> </param>
		public XrwOverrideShell (XrwApplicationShell parent, ref TPoint assignedPosition)
			: base (parent.Display, parent.Screen, parent.Window)
		{
			_parent = parent;
			
			InitializeOverrideShellResources (ref assignedPosition);
		}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="parent"> The parent (associated top-level) shell (an override shell can't be a top-level shell. <see cref="IntPtr"/> </param>
		/// <param name="fixedSize"> The fixed size, that ist to use (if set) rather than the calcualated size. Passed as reference to avoid structure copy constructor calls. <see cref="TSize"/> </param>
		public XrwOverrideShell (XrwApplicationShell parent, ref TSize fixedSize)
			: base (parent.Display, parent.Screen, parent.Window, ref fixedSize)
		{
			_parent = parent;
			
			TPoint assignedPosition = new TPoint (0, 0);
			InitializeOverrideShellResources (ref assignedPosition);
		}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="parent"> The parent (associated top-level) shell (an override shell can't be a top-level shell. <see cref="IntPtr"/> </param>
		/// <param name="assignedPosition"> The position of the top left top corner assigned by the window manager (for shell widgets) or geometry management (by non-shell widgets). Passed as reference to avoid structure copy constructor calls. <see cref="TPoint"/> </param>
		/// <param name="fixedSize"> The fixed size, that ist to use (if set) rather than the calcualated size. Passed as reference to avoid structure copy constructor calls. <see cref="TSize"/> </param>
		public XrwOverrideShell (XrwApplicationShell parent, ref TPoint assignedPosition, ref TSize fixedSize)
			: base (parent.Display, parent.Screen, parent.Window, ref fixedSize)
		{
			_parent = parent;
			
			_assignedSize = PreferredSize ();
			// The assignedPosition will be carried by the underlaying window, NOT relative to the parent.
			InitializeOverrideShellResources (ref assignedPosition);
		}
		
		/// <summary> Initialize local ressources for all constructors. </summary>
		/// <param name="assignedPosition"> The position of the top left top corner assigned by the window manager (for shell widgets) or geometry management (by non-shell widgets). Passed as reference to avoid structure copy constructor calls. <see cref="TPoint"/> </param>
		public void InitializeOverrideShellResources (ref TPoint assignedPosition)
		{
			TInt depth = X11lib.XDefaultDepth (_display, _screenNumber);
			X11lib.WindowAttributeMask mask = X11lib.WindowAttributeMask.CWOverrideRedirect | X11lib.WindowAttributeMask.CWSaveUnder;
			X11lib.XSetWindowAttributes attributes = new X11lib.XSetWindowAttributes ();
			attributes.override_redirect = (TBoolean)1;
			attributes.save_under = (TBoolean)1;
			_window = X11lib.XCreateWindow (_display, X11lib.XDefaultRootWindow(_display), (TInt)assignedPosition.X, (TInt)assignedPosition.Y, (TUint)_assignedSize.Width, (TUint)_assignedSize.Height,
			                                0, depth, (TUint)X11lib.WindowClass.InputOutput, IntPtr.Zero, mask, ref attributes);
			if (_window == IntPtr.Zero)
			{
				Console.WriteLine (CLASS_NAME + "::InitializeOverrideShellResources () ERROR. Can not create menu shell.");
				return;
			}
			
			_hasOwnWindow = true;

			X11lib.XSelectInput (_display, _window,
			                     EventMask.ExposureMask    | EventMask.ButtonPressMask | EventMask.ButtonReleaseMask |
			              		 EventMask.EnterWindowMask | EventMask.LeaveWindowMask | EventMask.PointerMotionMask |
			              		 EventMask.FocusChangeMask | EventMask.KeyPressMask    | EventMask.KeyReleaseMask |
						 		 EventMask.SubstructureNotifyMask );

			/* Create the foreground Graphics Context. */
			if (_gc != IntPtr.Zero)
			{
				if (XrwApplicationSettings.VERBOSE_OUTPUT_TO_CONSOLE)
					Console.WriteLine (CLASS_NAME + "::InitializeOverrideShellResources () Replace the foreground GC.");
				X11lib.XFreeGC (_display, _gc);
				_gc = IntPtr.Zero;
			}
		    _gc = X11lib.XCreateGC(_display, _window, 0, IntPtr.Zero);        
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
				Console.WriteLine (CLASS_NAME + ":: Dispose ()");
			
			this.DisposeByParent ();
		}

		/// <summary> Dispose by parent. </summary>
		public override void DisposeByParent ()
		{	
			base.DisposeByParent ();
		}
		
		#endregion
		
        // ###############################################################################
        // ### P R O P E R T I E S
        // ###############################################################################

		#region XrwRectObj overrides
		
		/// <summary> Get the parent widget. </summary>
		public new XrwApplicationShell Parent
		{	get	{ return base._parent as XrwApplicationShell;	}	}
		
 		#endregion

		#region IWindowedWidget implementation
		
 		#endregion
		
        // ###############################################################################
        // ### M E T H O D S
        // ###############################################################################

		#region XrwRectObj overrides
		
		/// <summary> Show the widget and map the window (if any). </summary>
		/// <remarks> Adds the override shell to her application schell's children, if not already done. </remarks>
		public override void Show()
		{
			_shown = true;
			
		 	X11lib.XMapWindow (_display, _window);
			X11lib.XFlush (_display);
			X11lib.XSetInputFocus (_display, _window, X11lib.TRevertTo.RevertToParent, (TInt)0);
		}
		
		/// <summary> Hide the widget and unmap the window (if any). </summary>
		public override void Hide()
		{
			_shown = false;
			
			X11lib.XUnmapWindow (_display, _window);
	  		X11lib.XFlush (_display);
		}
		
		/// <summary> Get a list of all windowed widget based on this widget - including the own and all windowed children. </summary>
		/// <returns> The list of all window based widgets on this widget. <see cref="List<IWindowedWidget>"/> </returns>
		public override List<XrwCore> AllWindowedWidgets()
		{
			List<XrwCore> result = new List<XrwCore> ();
			
			result.Add (this);
			result.AddRange (base.AllWindowedWidgets ());

			return result;
		}
		
		#endregion
		
		#region Methods

		#endregion
		
	}
}

