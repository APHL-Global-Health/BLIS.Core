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
	
	/// <summary> The windowless fundamental widget class (with a *** parent *** not an *** onw *** window). </summary>
	/// <remarks> The member attribute Window contains the window pointer of the *** parent ***. </remarks>
	public class XrwCore : XrwVisibleRectObj
	{

        // ###############################################################################
        // ### C O N S T A N T S
        // ###############################################################################

        #region Constants

        /// <summary> The class name constant. </summary>
        public new const string		CLASS_NAME = "XrwCore";
	
        #endregion

		// ###############################################################################
        // ### A T T R I B U T E S
        // ###############################################################################

		#region Attributes
		
		/// <summary> Determine whether this object has it's own window. </summary>
		protected bool	_hasOwnWindow			= false;
		
        #endregion
		
        // ###############################################################################
        // ### C O N S T R U C T I O N   A N D   I N I T I A L I Z A T I O N
        // ###############################################################################

        #region Construction

		/// <summary> Initializing constructor. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window for X11 calls. This widget has no X11 *** own *** window. <see cref="IntPtr"/> </param>
		public XrwCore (IntPtr display, X11.TInt screenNumber, IntPtr parentWindow)
			: base (display, screenNumber, parentWindow)
		{
			InitializeCoreRessources ();
		}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window for X11 calls. This widget has no X11 *** own *** window. <see cref="IntPtr"/> </param>
		/// <param name="assignedPosition"> The position of the top left top corner assigned by the window manager (for shell widgets) or geometry management (by non-shell widgets). Passed as reference to avoid structure copy constructor calls. <see cref="TPoint"/> </param>
		public XrwCore (IntPtr display, X11.TInt screenNumber, IntPtr parentWindow, ref TPoint assignedPosition)
			: base (display, screenNumber, parentWindow, ref assignedPosition)
		{
			InitializeCoreRessources ();
		}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window for X11 calls. This widget has no X11 *** own *** window. <see cref="IntPtr"/> </param>
		/// <param name="fixedSize"> The fixed size, that ist to use (if set) rather than the calcualated size. <see cref="TSize"/> Passed as reference to avoid structure copy constructor calls. </param>
		public XrwCore (IntPtr display, X11.TInt screenNumber, IntPtr parentWindow, ref TSize fixedSize)
			: base (display, screenNumber, parentWindow, ref fixedSize)
		{
			InitializeCoreRessources ();
		}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window for X11 calls. This widget has no X11 *** own *** window. <see cref="System.IntPtr"/> </param>
		/// <param name="assignedPosition"> The position of the top left top corner assigned by the window manager (for shell widgets) or geometry management (by non-shell widgets). Passed as reference to avoid structure copy constructor calls. <see cref="TPoint"/> </param>
		/// <param name="fixedSize"> The fixed size, that ist to use (if set) rather than the calcualated size. Passed as reference to avoid structure copy constructor calls. <see cref="TSize"/> </param>
		public XrwCore (IntPtr display, X11.TInt screenNumber, IntPtr parentWindow, ref TPoint assignedPosition, ref TSize fixedSize)
			: base (display, screenNumber, parentWindow, ref assignedPosition, ref fixedSize)
		{
			InitializeCoreRessources ();
		}
		
		/// <summary> Initialize local ressources for all constructors. </summary>
		private void InitializeCoreRessources ()
		{
			_borderColorPixel = X11lib.XAllocParsedColorByName (_display, _screenNumber, XrwTheme.OuterBorderColor);
		}
		
		/// <summary> Initialize an *** own *** window. </summary>
		/// <remarks> Use thei initialization from XrwCore +++ inheridet +++ classes, that should act as a widget, not as a gadget. </remarks>
		protected void InitializeOwnWindow ()
		{
			// Create a *** own *** window and exchange the *** parent ***  window against it.
			_window = X11lib.XCreateSimpleWindow(_display, _window, (TInt)_assignedPosition.X, (TInt)_assignedPosition.Y, (TUint)_fixedSize.Width, (TUint)_fixedSize.Height, 0, 0, _backgroundColorPixel);
			if (_window == IntPtr.Zero)
			{
				Console.WriteLine (CLASS_NAME + "::InitializeOwnWindow () ERROR. Can not create transient shell.");
				throw new OperationCanceledException ("Failed to create own window.");
			}

			X11lib.XSelectInput (_display, _window,
			                     EventMask.ExposureMask    | EventMask.ButtonPressMask | EventMask.ButtonReleaseMask |
			              		 EventMask.EnterWindowMask | EventMask.LeaveWindowMask | EventMask.PointerMotionMask |
			              		 EventMask.FocusChangeMask | EventMask.KeyPressMask    |
			                     EventMask.KeyReleaseMask  | EventMask.SubstructureNotifyMask );
	
			_hasOwnWindow = true;
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
			if (_hasOwnWindow)
			{
				if (_window == IntPtr.Zero)
					return;
	
				// Step 1:
				// =======
				
				// Stop event listening.
				X11lib.XSelectInput (_display, _window, EventMask.NoEventMask);
				
				// Unmap from display.
		  		X11lib.XUnmapWindow (_display, _window);
				
				// Step 2:
				// =======
				
				// Free shared ressources.
				base.DisposeByParent ();
	
				// Step 3:
				// =======
				
				// Destroy window resources.
				X11lib.XDestroyWindow (_display, _window);
				_window = IntPtr.Zero;
			}
			else
				base.DisposeByParent ();
		}
		
		#endregion
		
        // ###############################################################################
        // ### P R O P E R T I E S
        // ###############################################################################

		#region Properties
		
		/// <summary> Determine whether object has an *** own *** window. </summary>
		public override bool HasOwnWindow
		{	get {	return _hasOwnWindow;	}
		}
		
		#endregion
		
        // ###############################################################################
        // ### M E T H O D S
        // ###############################################################################

		#region XrwRectObj overrides
		
		/// <summary> Get a list of all windowed widget based on this widget - including the own and all windowed children. </summary>
		/// <returns> The list of all window based widgets on this widget. <see cref="List<IWindowedWidget>"/> </returns>
		public override List<XrwCore> AllWindowedWidgets()
		{
			List<XrwCore> result = new List<XrwCore> ();
			
			if (_hasOwnWindow)
				result.Add (this);

			return result;
		}
		
		/// <summary> Show the widget and map the window (if any). </summary>
		public override void Show()
		{
			_shown = true;
			
			if (_hasOwnWindow)
			{
			 	X11lib.XMapWindow (_display, _window);
				X11lib.XFlush (_display);
			}
		}
		
		/// <summary> Hide the widget and unmap the window (if any). </summary>
		public override void Hide()
		{
			_shown = false;

			if (_hasOwnWindow)
			{
		  		X11lib.XUnmapWindow (_display, _window);
		  		X11lib.XFlush (_display);
			}
		}
		
		/// <summary> Handle the ExposeEvent. </summary>
		/// <param name="e"> The event data. <see cref="XrwExposeEvent"/> </param>
		/// <remarks> Set XrwExposeEvent. Set result to nonzero to stop further event processing. </remarks>
		/// <remarks> This metod is called from applications message loop for windowed widgets only. </remarks>
		public override void OnExpose (XrwExposeEvent e)
		{
			if (_shown == false)
				return;
			
			Redraw ();
			e.Result = 1; // TRUE
		}
		
		/// <summary> Redraw the widget. </summary>
		protected virtual void Redraw ()
		{
			if (_assignedSize.Width <  1 || _assignedSize.Height <  1)
				return;
			
			TRectangle completeArea = new TRectangle (_assignedPosition, _assignedSize);
			
			X11lib.XSetForeground (_display, _gc, _backgroundColorPixel);
			base.DrawBackground (_display, _window, _gc, completeArea);

			if ((int)_borderWidth > 0)
			{
				X11lib.XSetForeground (_display, _gc, _borderColorPixel);
				base.DrawBorder (_display, _window, _gc, new TRectangle (_assignedPosition, _assignedSize), _borderWidth);
			}

			if (_frameType != TFrameTypeExt.None && _frameWidth > 0)
				base.DrawFrame (_display, _window, _gc, this.ClientArea (), _frameType, _darkShadowColorPixel, _lightShadowColorPixel);
		}
		
		#endregion
		
	}

}