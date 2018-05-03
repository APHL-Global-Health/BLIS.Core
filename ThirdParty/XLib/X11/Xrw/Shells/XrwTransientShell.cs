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

	/// <summary> The windowed base class for popup shells, that interact with the window manager. </summary>
	/// <remarks> The member attribute Window contains the *** own *** window pointer. </remarks>
	public class XrwTransientShell : XrwWmShell
	{

        // ###############################################################################
        // ### C O N S T A N T S
        // ###############################################################################

        #region Constants

        /// <summary> The class name constant. </summary>
        public new const string		CLASS_NAME = "XrwTransientShell";
		
        #endregion

		// ###############################################################################
        // ### A T T R I B U T E S
        // ###############################################################################

		#region Attributes
		
		/// <summary> The delete message from the windows manager. Closing an app via window
		/// title functionality doesn't generate a window message - it only generates a
		/// window manager message, thot must be routed to the window (message loop). </summary>
		protected IntPtr				_wmDeleteMessage;
		
		/// <summary> Define wether the windows manager shell is application modal or not. </summary>
		protected bool					_appModal			= false;
		
		/// <summary> Indicate wether signals, messages and events can be processed or not. </summary>
		protected bool					_disposed			= false;

        #endregion

		// ###############################################################################
        // ### E V E N T S
        // ###############################################################################

		#region Events
		
		/// <summary> Register WmShellClose event handler. </summary>
		public event WmShellCloseDelegate WmShellClose;

        #endregion
		
        // ###############################################################################
        // ### C O N S T R U C T I O N   A N D   I N I T I A L I Z A T I O N
        // ###############################################################################

        #region Construction
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="parent"> The parent (associated top-level) shell (an override shell can't be a top-level shell. <see cref="IntPtr"/> </param>
		public XrwTransientShell (XrwApplicationShell parent)
			: base (parent.Display, parent.Screen, parent.Window)
		{
			_parent = parent;
			
			_assignedSize = PreferredSize ();
			TPoint assignedPosition = new TPoint (0, 0);
			InitializeTransientShellResources (ref assignedPosition);
		}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="parent"> The parent (associated top-level) shell (an override shell can't be a top-level shell. <see cref="IntPtr"/> </param>
		/// <param name="assignedPosition"> The position of the top left top corner assigned by the window manager (for shell widgets) or geometry management (by non-shell widgets). Passed as reference to avoid structure copy constructor calls. <see cref="TPoint"/> </param>
		public XrwTransientShell (XrwApplicationShell parent, ref TPoint assignedPosition)
			: base (parent.Display, parent.Screen, parent.Window)
		{
			_parent = parent;
			
			_assignedSize = PreferredSize ();
			InitializeTransientShellResources (ref assignedPosition);
		}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="diaplay">The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window for X11 calls. This widget has no X11 *** own *** window. <see cref="IntPtr"/> </param>
		/// <param name="fixedSize"> The fixed size, that ist to use (if set) rather than the calcualated size. <see cref="TSize"/> </param>
		public XrwTransientShell (XrwApplicationShell parent, ref TSize fixedSize)
			: base (parent.Display, parent.Screen, parent.Window, ref fixedSize)
		{
			_parent = parent;
			
			TPoint assignedPosition = new TPoint (0, 0);
			_assignedSize = PreferredSize ();
			InitializeTransientShellResources (ref assignedPosition);
		}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="diaplay">The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window for X11 calls. This widget has no X11 *** own *** window. <see cref="IntPtr"/> </param>
		/// <param name="assignedPosition"> The position of the top left top corner assigned by the window manager (for shell widgets) or geometry management (by non-shell widgets). Passed as reference to avoid structure copy constructor calls. <see cref="TPoint"/> </param>
		/// <param name="fixedSize"> The fixed size, that ist to use (if set) rather than the calcualated size. <see cref="TSize"/> </param>
		public XrwTransientShell (XrwApplicationShell parent, ref TPoint assignedPosition, ref TSize fixedSize)
			: base (parent.Display, parent.Screen, parent.Window, ref fixedSize)
		{
			_parent = parent;
			
			_assignedSize = PreferredSize ();
			// The offset will be carried by the underlaying window, NOT relative to the parent.
			InitializeTransientShellResources (ref assignedPosition);
		}
		
		/// <summary> Initialize local ressources for all constructors. </summary>
		/// <param name="assignedPosition"> The position of the top left top corner assigned by the window manager (for shell widgets) or geometry management (by non-shell widgets). Passed as reference to avoid structure copy constructor calls. <see cref="TPoint"/> </param>
		public void InitializeTransientShellResources (ref TPoint offset)
		{
			/* Get the colors black and white. */
			TPixel black = X11lib.XBlackPixel (_display, _screenNumber);	/* get color black */
			TPixel white = X11lib.XWhitePixel (_display, _screenNumber);	/* get color white */
			
			/* Once the display is initialized, create the window.
			   It will have the foreground white and background black */
			_window = X11lib.XCreateSimpleWindow (_display, X11lib.XDefaultRootWindow(_display), (TInt)offset.X, (TInt)offset.Y, (TUint)_assignedSize.Width, (TUint)_assignedSize.Height, 0, 0, black);
			if (_window == IntPtr.Zero)
			{
				Console.WriteLine (CLASS_NAME + "::InitializeTransientShellResources () ERROR. Can not create transient shell.");
				return;
			}
			
			X11lib.XSelectInput (_display, _window,
			                     EventMask.StructureNotifyMask	| EventMask.ExposureMask		|
			                     EventMask.ButtonPressMask		| EventMask.ButtonReleaseMask	|
			              		 EventMask.EnterWindowMask		| EventMask.LeaveWindowMask		|
			                     EventMask.PointerMotionMask	| EventMask.FocusChangeMask		|
			                     EventMask.KeyPressMask			| EventMask.KeyReleaseMask		|
						 		 EventMask.SubstructureNotifyMask );

			_hasOwnWindow = true;
			
			/* Hook the closing event from windows manager. */
			_wmDeleteMessage = X11lib.XInternAtom (_display, "WM_DELETE_WINDOW", false);
			if (X11lib.XSetWMProtocols (_display, _window, ref _wmDeleteMessage, (X11.TInt)1) == 0)
			{
				Console.WriteLine (CLASS_NAME + "::InitializeTransientShellResources () WARNING: Failed to register 'WM_DELETE_WINDOW' event.");
			}
			
			X11lib.XSetTransientForHint (_display, _window, Parent.Window);

			/* Recreate the foreground Graphics Context with. */
			if (_gc != IntPtr.Zero)
			{
				if (XrwApplicationSettings.VERBOSE_OUTPUT_TO_CONSOLE)
					Console.WriteLine (CLASS_NAME + "::InitializeTransientShellResources () Replace the foreground GC.");
				X11lib.XFreeGC (_display, _gc);
				_gc = IntPtr.Zero;
			}
		    _gc = X11lib.XCreateGC(_display, _window, 0, IntPtr.Zero);        
			X11lib.XSetForeground (_display, _gc, white);
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
			_disposed = true;
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

		#region Properties
		
		/// <summary> Define wether the windows manager shell is application modal or not. </summary>
		public bool AppModal
		{
			get	{	return _appModal;	}
		}
		
 		#endregion
		
        // ###############################################################################
        // ### M E T H O D S
        // ###############################################################################

		#region XrwRectObj overrides
		
		/// <summary> Show the widget and map the window (if any). </summary>
		public override void Show()
		{
			_shown = true;

		 	X11lib.XMapWindow (_display, _window);
			X11lib.XFlush (_display);
		}
		
		/// <summary> Hide the widget and unmap the window (if any). </summary>
		public override void Hide()
		{
			_shown = false;
			
			for (int cntChildren = _children.Count - 1; cntChildren >= 0; cntChildren--)
			{
				_children[cntChildren].Hide ();
			}
			
	  		X11lib.XUnmapWindow (_display, _window);
	  		X11lib.XFlush (_display);
		}
		
		#endregion
		
		#region Methods
		
		/// <summary> Calcualate layout after a position or size change. </summary>
		/// <remarks> This method is defined at XrwRectObj for compatibility, but must be implemented for composite widgets only. </remarks>
		public void CalculateChildLayout ()
		{
			TPoint childPosition = new TPoint (_borderWidth, _borderWidth);
			List<ChildData> childData = new List<ChildData>();
			TSize childrenPreferredSize  = new TSize(0, 0);

			for (int counter = 0; counter < _children.Count; counter++)
			{
				TSize preferredSize  = _children[counter].PreferredSize ();
				childData.Add (new ChildData (_children[counter], preferredSize));
				
				if (childrenPreferredSize.Width  < preferredSize.Width)
					childrenPreferredSize.Width  = preferredSize.Width;
				if (childrenPreferredSize.Height < preferredSize.Height)
					childrenPreferredSize.Height = preferredSize.Height;
			}
			
			for (int counter = 0; counter < childData.Count; counter++)
			{
				ChildData cd = childData[counter];
				
				if (cd.Widget.ExpandToAvailableWidth == true)
					cd.Size.Width = childrenPreferredSize.Width;
				if (cd.Widget.ExpandToAvailableHeight == true)
					cd.Size.Height = childrenPreferredSize.Height;

				GeometryManagerAccess.SetAssignedGeometry (cd.Widget, childPosition, cd.Size);
			}
		}
		
        #endregion
		
        #region Event handler

		/// <summary> Handle the ClientMessage event. </summary>
		/// <param name="e"> The event data. <see cref="XrwClientMessageEvent"/> </param>
		/// <remarks> Set XawClientMessageEvent. Set result to nonzero to stop further event processing. </remarks>
		public virtual void OnWmClose (XrwClientMessageEvent e)
		{
			WmShellCloseDelegate wmShellClose = WmShellClose;
			if (wmShellClose != null)
				wmShellClose (this, e);
		}
		
		/// <summary> Default processing for the Close event. </summary>
		/// <param name="source"> The widget, the Close event is assigned to. <see cref="XrwTransientShell"/> </param>
		/// <returns> True to continue closing, false otherwise. </returns>
		public bool DefaultClose ()
		{
			if (_disposed == true)
				return true;
			
			// PROBLEM: Double dispose!

			Hide ();
			Dispose ();
			Parent.RemoveTransientShell (this);
			
			// Don't disconnect from X server - application shell is still running.
			
			return true;
		}
		
		#endregion
		
	}
}

