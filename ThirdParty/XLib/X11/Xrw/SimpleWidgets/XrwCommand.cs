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

	/// <summary> The windowed command button widget. </summary>
	/// <remarks> The member attribute Window contains the window pointer of the *** own *** window. </remarks>
	public class XrwCommand : XrwLabel
	{

        // ###############################################################################
        // ### C O N S T A N T S
        // ###############################################################################

        #region Constants

        /// <summary> The class name constant. </summary>
        public new const string	CLASS_NAME = "XrwCommand";

        #endregion

		// ###############################################################################
        // ### A T T R I B U T E S
        // ###############################################################################

		#region Attributes
		
		/// <summary> The focused color. </summary>
		protected TPixel		_focusedColorPixel;
		
		/// <summary> The focused indicator. </summary>
		protected bool			_focused				= false;
		
		/// <summary> The pressed indicator. </summary>
		protected bool			_pressed				= false;
		
        #endregion

		// ###############################################################################
        // ### E V E N T S
        // ###############################################################################

		#region Events
		
		/// <summary> Register switched off event handler. </summary>
		public event SwitchedOffDelegate SwitchedOff;
		
		/// <summary> Register switched on event handler. </summary>
		public event SwitchedOnDelegate SwitchedOn;

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
		public XrwCommand (IntPtr display, X11.TInt screen, IntPtr parentWindow, string label)
			: base (display, screen, parentWindow, label)
		{
			// Create a *** own *** window and exchange the *** parent ***  window against it.
			base.InitializeOwnWindow ();
			
			InitializeCommandRessources ();
		}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window that will be the parent for an X11 *** own ** window. <see cref="IntPtr"/> </param>
		/// <param name="fixedSize"> The fixed size, that ist to use (if set) rather than the calcualated size. <see cref="TIntSize"/> </param>
		/// <param name="label"> The label to display. <see cref="System.String"/> </param>
		public XrwCommand (IntPtr display, X11.TInt screen, IntPtr parentWindow, ref TSize fixedSize, string label)
			: base (display, screen, parentWindow, ref fixedSize, label)
		{
			// Create a *** own *** window and exchange the *** parent ***  window against it.
			base.InitializeOwnWindow ();
			
			InitializeCommandRessources ();
		}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window that will be the parent for an X11 *** own ** window. <see cref="IntPtr"/> </param>
		/// <param name="assignedPosition"> The position of the top left top corner assigned by the window manager (for shell widgets) or geometry management (by non-shell widgets). <see cref="TPoint"/> </param>
		/// <param name="fixedSize"> The fixed size, that ist to use (if set) rather than the calcualated size. <see cref="TIntSize"/> </param>
		/// <param name="label"> The label to display. <see cref="System.String"/> </param>
		public XrwCommand (IntPtr display, X11.TInt screenNumber, IntPtr parentWindow, ref TPoint assignedPosition, ref TSize fixedSize, string label)
			: base (display, screenNumber, parentWindow, ref assignedPosition, ref fixedSize, label)
		{	
			// Create a *** own *** window and exchange the *** parent ***  window against it.
			base.InitializeOwnWindow ();
			
			InitializeCommandRessources ();
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
		public XrwCommand (IntPtr display, X11.TInt screenNumber, IntPtr parentWindow, string label, X11Graphic leftBitmap, bool leftShared, X11Graphic rightBitmap, bool rightShared)
			: base (display, screenNumber, parentWindow, label, leftBitmap, leftShared, rightBitmap, rightShared)
		{	
			// Create a *** own *** window and exchange the *** parent ***  window against it.
			base.InitializeOwnWindow ();
			
			InitializeCommandRessources ();
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
		public XrwCommand (IntPtr display, X11.TInt screen, IntPtr parentWindow, ref TSize fixedSize, string label, X11Graphic leftBitmap, bool leftShared, X11Graphic rightBitmap, bool rightShared)
			: base (display, screen, parentWindow, ref fixedSize, label, leftBitmap, leftShared, rightBitmap, rightShared)
		{
			// Create a *** own *** window and exchange the *** parent ***  window against it.
			base.InitializeOwnWindow ();
			
			InitializeCommandRessources ();
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
		public XrwCommand (IntPtr display, X11.TInt screenNumber, IntPtr parentWindow, ref TPoint assignedPosition, ref TSize fixedSize, string label, X11Graphic leftBitmap, bool leftShared, X11Graphic rightBitmap, bool rightShared)
			: base (display, screenNumber, parentWindow, ref assignedPosition, ref fixedSize, label, leftBitmap, leftShared, rightBitmap, rightShared)
		{	
			// Create a *** own *** window and exchange the *** parent ***  window against it.
			base.InitializeOwnWindow ();

			InitializeCommandRessources ();
		}
		
		/// <summary> Initialize local ressources for all constructors. </summary>
		private void InitializeCommandRessources ()
		{
			_frameWidth = XrwTheme.InteractingFrameWidth;
			_frameType  = (TFrameTypeExt)XrwTheme.InteractingFrameType;

			_focusedColorPixel = X11lib.XAllocParsedColorByName (_display, _screenNumber, XrwTheme.FocusedColor);
			
			Enter += this.HandleEnterDefault;
			Leave += this.HandleLeaveDefault;
			ButtonPress   += this.HandleButtonPressDefault;
			ButtonRelease += this.HandleButtonReleaseDefault;
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
			get { return _focused;	}
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
				TFrameTypeExt frameType = _frameType;
				if (_pressed == true && frameType == TFrameTypeExt.Raised)
					frameType = TFrameTypeExt.Sunken;
				else if (_pressed == true && frameType == TFrameTypeExt.Sunken)
					frameType = TFrameTypeExt.Raised;
				else if (_pressed == true && frameType == TFrameTypeExt.Chiseled)
					frameType = TFrameTypeExt.Ledged;
				else if (_pressed == true && frameType == TFrameTypeExt.Ledged)
					frameType = TFrameTypeExt.Chiseled;
				else if (_pressed != true && frameType == TFrameTypeExt.RaisedTopTab)
					frameType = TFrameTypeExt.UnsunkenTopTab;
				else if (_pressed != true && frameType == TFrameTypeExt.RaisedTopTabTail)
					frameType = TFrameTypeExt.UnsunkenTopTabTail;
				else if (_pressed != true && frameType == TFrameTypeExt.RaisedLeftTab)
					frameType = TFrameTypeExt.UnsunkenLeftTab;
				else if (_pressed != true && frameType == TFrameTypeExt.RaisedRightTab)
					frameType = TFrameTypeExt.UnsunkenRightTab;
				else if (_pressed != true && frameType == TFrameTypeExt.RaisedBottomTab)
					frameType = TFrameTypeExt.UnsunkenBottomTab;
				else if (_pressed != true && frameType == TFrameTypeExt.SunkenTopTab)
					frameType = TFrameTypeExt.UnraisedTopTab;
				else if (_pressed != true && frameType == TFrameTypeExt.SunkenTopTabTail)
					frameType = TFrameTypeExt.UnraisedTopTabTail;
				else if (_pressed != true && frameType == TFrameTypeExt.SunkenLeftTab)
					frameType = TFrameTypeExt.UnraisedLeftTab;
				else if (_pressed != true && frameType == TFrameTypeExt.SunkenRightTab)
					frameType = TFrameTypeExt.UnraisedRightTab;
				else if (_pressed != true && frameType == TFrameTypeExt.SunkenBottomTab)
					frameType = TFrameTypeExt.UnraisedBottomTab;
				base.DrawFrame (_display, _window, _gc, clientArea, frameType, _darkShadowColorPixel, _lightShadowColorPixel);
			}
			
			base.RedrawContent ();
		}
		
		#endregion
		
		#region Event handler
		
		/// <summary> Handle the switched off event. </summary>
		public virtual void OnSwitchedOff ()
		{
			SwitchedOffDelegate switchedOff = SwitchedOff;
			if (switchedOff != null)
				switchedOff (this);
		}
		
		/// <summary> Handle the switched on event. </summary>
		public virtual void OnSwitchedOn ()
		{
			SwitchedOnDelegate switchedOn = SwitchedOn;
			if (switchedOn != null)
				switchedOn (this);
		}

		/// <summary> Handle the Enter event. </summary>
		/// <param name="source"> The widget, the Enter event is assigned to. <see cref="XrwRectObj"/> </param>
		/// <param name="e"> The event data. <see cref="XrwCrossingEvent"/> </param>
		/// <remarks> Set XrwCrossingEvent. Set result to nonzero to stop further event processing. </remarks>
		protected void HandleEnterDefault (XrwRectObj source, XrwCrossingEvent e)
		{
			_focused = true;
			if (XrwObject.SendExposeEvent (_display, _window, _window) == 0)
			{
				Console.WriteLine (CLASS_NAME + "::HandleFocusIn () ERROR. Can not send expose event.");
			}
			e.Result = 1;
		}
		
		/// <summary> Handle the Leave event. </summary>
		/// <param name="source"> The widget, the Leave event is assigned to. <see cref="XrwRectObj"/> </param>
		/// <param name="e"> The event data. <see cref="XrwCrossingEvent"/> </param>
		/// <remarks> Set XrwCrossingEvent. Set result to nonzero to stop further event processing. </remarks>
		protected void HandleLeaveDefault (XrwRectObj source, XrwCrossingEvent e)
		{
			_focused = false;
			_pressed = false;
		
			if (XrwObject.SendExposeEvent (_display, _window, _window) == 0)
			{
				Console.WriteLine (CLASS_NAME + "::HandleFocusOut () ERROR. Can not send expose event.");
			}
			OnSwitchedOff ();
			e.Result = 1;
		}

		/// <summary> Handle the ButtonPress event. </summary>
		/// <param name="source"> The widget, the ButtonPress event is assigned to. <see cref="XrwRectObj"/> </param>
		/// <param name="e"> The event data. <see cref="XrwButtonEvent"/> </param>
		/// <remarks> Set XrwButtonEvent. Set result to nonzero to stop further event processing. </remarks>
		protected void HandleButtonPressDefault (XrwRectObj source, XrwButtonEvent e)
		{
			_pressed = true;
			
			if (XrwObject.SendExposeEvent (_display, _window, _window) == 0)
			{
				Console.WriteLine (CLASS_NAME + "::HandleFocusOut () ERROR. Can not send expose event.");
			}
			OnSwitchedOn ();
			e.Result = 1;
		}

		/// <summary> Handle the ButtonRelease event. </summary>
		/// <param name="source"> The widget, the ButtonRelease event is assigned to. <see cref="XrwRectObj"/> </param>
		/// <param name="e"> The event data. <see cref="XrwButtonEvent"/> </param>
		/// <remarks> Set XrwButtonEvent. Set result to nonzero to stop further event processing. </remarks>
		protected void HandleButtonReleaseDefault (XrwRectObj source, XrwButtonEvent e)
		{
			_pressed = false;
			
			if (XrwObject.SendExposeEvent (_display, _window, _window) == 0)
			{
				Console.WriteLine (CLASS_NAME + "::HandleFocusOut () ERROR. Can not send expose event.");
			}
			OnSwitchedOff ();
			e.Result = 1;
		}

		#endregion

	}

}

