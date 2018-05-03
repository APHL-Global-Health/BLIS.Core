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
	
	/// <summary> The windowless fundamental simple menu entry class. </summary>
	/// <remarks> The member attribute Window contains the window pointer of the *** parent ***. </remarks>
	public class XrwSme : XrwLabel
	{

        // ###############################################################################
        // ### C O N S T A N T S
        // ###############################################################################

        #region Constants

        /// <summary> The class name constant. </summary>
        public new const string	CLASS_NAME = "XrwSme";
		
        #endregion

		// ###############################################################################
        // ### A T T R I B U T E S
        // ###############################################################################

		#region Attributes

		/// <summary> The focused color. </summary>
		TPixel					_focusedColorPixel;
		
		/// <summary> The hilight indicator. </summary>
		private bool			_focused				= false;
		
        #endregion

        // ###############################################################################
        // ### C O N S T R U C T I O N   A N D   I N I T I A L I Z A T I O N
        // ###############################################################################

        #region Construction
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window for X11 calls. This widget has no X11 *** own *** window. <see cref="System.IntPtr"/> </param>
		public XrwSme (IntPtr display, X11.TInt screen, IntPtr parentWindow, string label)
			: base (display, screen, parentWindow, label)
		{
			InitializeSmeRessources ();
		}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window that will be the parent for an X11 *** own ** window. <see cref="IntPtr"/> </param>
		/// <param name="fixedSize"> The fixed size, that ist to use (if set) rather than the calcualated size. <see cref="TIntSize"/> </param>
		/// <param name="label"> The label to display. <see cref="System.String"/> </param>
		public XrwSme (IntPtr display, X11.TInt screen, IntPtr parentWindow, ref TSize fixedSize, string label)
			: base (display, screen, parentWindow, ref fixedSize, label)
		{
			InitializeSmeRessources ();
		}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window that will be the parent for an X11 *** own ** window. <see cref="IntPtr"/> </param>
		/// <param name="assignedPosition"> The position of the top left top corner assigned by the window manager (for shell widgets) or geometry management (by non-shell widgets). <see cref="TPoint"/> </param>
		/// <param name="fixedSize"> The fixed size, that ist to use (if set) rather than the calcualated size. <see cref="TIntSize"/> </param>
		/// <param name="label"> The label to display. <see cref="System.String"/> </param>
		public XrwSme (IntPtr display, X11.TInt screenNumber, IntPtr parentWindow, ref TPoint assignedPosition, ref TSize fixedSize, string label)
			: base (display, screenNumber, parentWindow, ref assignedPosition, ref fixedSize, label)
		{	
			InitializeSmeRessources ();
		}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window for X11 calls. This widget has no X11 *** own *** window. <see cref="System.IntPtr"/> </param>
		/// <param name="label"> The label to display. <see cref="System.String"/> </param>
		/// <param name="leftBitmap"> The left bitmap to display. <see cref="XrwBitmap"/> </param>
		/// <param name="leftShared"> Indicate wether the leftBitmap is shared (desposed by caller) or private (disposed together with tis label). <see cref="System.Boolean"/> </param>
		/// <param name="rightBitmap"> The right bitmap to display. <see cref="XrwBitmap"/> </param>
		/// <param name="rightShared"> Indicate wether the rightBitmap is shared (desposed by caller) or private (disposed together with tis label). <see cref="System.Boolean"/> </param>
		public XrwSme (IntPtr display, X11.TInt screenNumber, IntPtr parentWindow, string label, X11Graphic leftBitmap, bool leftShared, X11Graphic rightBitmap, bool rightShared)
			: base (display, screenNumber, parentWindow, label, leftBitmap, leftShared, rightBitmap, rightShared)
		{	
			InitializeSmeRessources ();
		}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window for X11 calls. This widget has no X11 *** own *** window. <see cref="System.IntPtr"/> </param>
		/// <param name="fixedSize"> The fixed size, that ist to use (if set) rather than the calcualated size. Passed as reference to avoid structure copy constructor calls. <see cref="TIntSize"/> </param>
		/// <param name="label"> The label to display. <see cref="System.String"/> </param>
		/// <param name="leftBitmap"> The left bitmap to display. <see cref="XrwBitmap"/> </param>
		/// <param name="leftShared"> Indicate wether the leftBitmap is shared (desposed by caller) or private (disposed together with tis label). <see cref="System.Boolean"/> </param>
		/// <param name="rightBitmap"> The right bitmap to display. <see cref="XrwBitmap"/> </param>
		/// <param name="rightShared"> Indicate wether the rightBitmap is shared (desposed by caller) or private (disposed together with tis label). <see cref="System.Boolean"/> </param>
		public XrwSme (IntPtr display, X11.TInt screen, IntPtr parentWindow, ref TSize fixedSize, string label, X11Graphic leftBitmap, bool leftShared, X11Graphic rightBitmap, bool rightShared)
			: base (display, screen, parentWindow, ref fixedSize, label, leftBitmap, leftShared, rightBitmap, rightShared)
		{	
			InitializeSmeRessources ();
		}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window for X11 calls. This widget has no X11 *** own *** window. <see cref="System.IntPtr"/> </param>
		/// <param name="assignedPosition"> The position of the top left top corner assigned by the window manager (for shell widgets) or geometry management (by non-shell widgets). Passed as reference to avoid structure copy constructor calls. <see cref="TPoint"/> </param>
		/// <param name="fixedSize"> The fixed size, that ist to use (if set) rather than the calcualated size. Passed as reference to avoid structure copy constructor calls. <see cref="TIntSize"/> </param>
		/// <param name="label"> The label to display. <see cref="System.String"/> </param>
		/// <param name="leftBitmap"> The left bitmap to display. <see cref="XrwBitmap"/> </param>
		/// <param name="leftShared"> Indicate wether the leftBitmap is shared (desposed by caller) or private (disposed together with tis label). <see cref="System.Boolean"/> </param>
		/// <param name="rightBitmap"> The right bitmap to display. <see cref="XrwBitmap"/> </param>
		/// <param name="rightShared"> Indicate wether the rightBitmap is shared (desposed by caller) or private (disposed together with tis label). <see cref="System.Boolean"/> </param>
		public XrwSme (IntPtr display, X11.TInt screenNumber, IntPtr parentWindow, ref TPoint assignedPosition, ref TSize fixedSize, string label, X11Graphic leftBitmap, bool leftShared, X11Graphic rightBitmap, bool rightShared)
			: base (display, screenNumber, parentWindow, ref assignedPosition, ref fixedSize, label, leftBitmap, leftShared, rightBitmap, rightShared)
		{	
			InitializeSmeRessources ();
		}
		
		/// <summary> Initialize local ressources for all constructors. </summary>
		private void InitializeSmeRessources ()
		{
			_focusedColorPixel = X11lib.XAllocParsedColorByName (_display, _screenNumber, XrwTheme.FocusedColor);
			
			Enter += HandleEnterDefault;
			Leave += HandleLeaveDefault;
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
		
        // ###############################################################################
        // ### P R O P E R T I E S
        // ###############################################################################
		
		#region Properties
		
		/// <summary> Get the focused indicator. </summary>
		public bool Focused
		{
			get { return _focused; }
			set { _focused = value; }
		}

		#endregion
	
        // ###############################################################################
        // ### M E T H O D S
        // ###############################################################################

		#region XrwRectObj overrides
		
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
		protected override void Redraw ()
		{
			if (_assignedSize.Width <  1 || _assignedSize.Height <  1)
				return;
			
			TRectangle completeArea = new TRectangle (_assignedPosition, _assignedSize);
			
			// Background.
			if (_focused)
				X11lib.XSetForeground (_display, _gc, _focusedColorPixel);
			else
				X11lib.XSetForeground (_display, _gc, _backgroundColorPixel);
			base.DrawBackground (_display, _window, _gc, completeArea);
			
			// Outer border.
			if ((int)_borderWidth > 0)
			{
				X11lib.XSetForeground (_display, _gc, _borderColorPixel);
				base.DrawBorder (_display, _window, _gc, completeArea, _borderWidth);
			}

			TRectangle clientArea = ClientArea();
			
			// Frame.
			if (_frameType != TFrameTypeExt.None)
			{
				bool _pressed = false;
				
				TFrameTypeExt frameType = _frameType;
				if (_pressed == true && frameType == TFrameTypeExt.Raised)
					frameType = TFrameTypeExt.Sunken;
				else if (_pressed == true && frameType == TFrameTypeExt.Sunken)
					frameType = TFrameTypeExt.Raised;
				else if (_pressed == true && frameType == TFrameTypeExt.Chiseled)
					frameType = TFrameTypeExt.Ledged;
				else if (_pressed == true && frameType == TFrameTypeExt.Ledged)
					frameType = TFrameTypeExt.Chiseled;			
				base.DrawFrame (_display, _window, _gc, clientArea, frameType, _darkShadowColorPixel, _lightShadowColorPixel);
			}

			base.RedrawContent ();
		}

        #endregion
		
		#region Event handler

		/// <summary> Prototype the Enter event. </summary>
		/// <param name="source"> The widget, the FocusOutDelegate event is assigned to. <see cref="XrwRectObj"/> </param>
		/// <param name="e"> The event data. <see cref="XCrossingEvent"/> </param>
		/// <remarks> Set XrwCrossingEvent. Set result to nonzero to stop further event processing. </remarks>
		private void HandleEnterDefault (XrwRectObj source, XrwCrossingEvent e)
		{
			_focused = true;
			XrwApplicationShell appShell = ApplicationShell;
			if (appShell != null)
			{
				if (XrwObject.SendExposeEvent (_display, _window, _window) == 0)
				{
					Console.WriteLine (CLASS_NAME + "::HandleFocusIn () ERROR. Can not send expose event.");
				}
			}
			else
				Console.WriteLine (CLASS_NAME + "::Show () ERROR. Can not investigate application shell.");
			e.Result = 1;
		}
		
		/// <summary> Handle the Leave event. </summary>
		/// <param name="source"> The widget, the FocusIn event is assigned to. <see cref="XrwRectObj"/> </param>
		/// <param name="e"> The event data. <see cref="XCrossingEvent"/> </param>
		/// <remarks> Set XrwCrossingEvent. Set result to nonzero to stop further event processing. </remarks>
		private void HandleLeaveDefault (XrwRectObj source, XrwCrossingEvent e)
		{
			_focused = false;	
			XrwApplicationShell appShell = ApplicationShell;
			if (appShell != null)
			{
				if (XrwObject.SendExposeEvent (_display, _window, _window) == 0)
				{
					Console.WriteLine (CLASS_NAME + "::HandleFocusOut () ERROR. Can not send expose event.");
				}
			}
			else
				Console.WriteLine (CLASS_NAME + "::HandleFocusOut () ERROR. Can not investigate application shell.");
			e.Result = 1;
		}

		#endregion
		
	}
}

