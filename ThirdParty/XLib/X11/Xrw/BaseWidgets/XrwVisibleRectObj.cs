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
	/// <summary> The shadow type to apply a 3D look. </summary>
	internal enum TFrameTypeExt
	{
		/// <summary> No 3D effect at all. </summary>
		None	= X11.TFrameType.None,
		
		/// <summary> The frame appears in raised 3D effect. </summary>
		Raised	= X11.TFrameType.Raised,
		
		/// <summary> The frame appears in sunken 3D effect. </summary>
		Sunken	= X11.TFrameType.Sunken,
		
		/// <summary> The frame appears in chiseled 3D effect. </summary>
		Chiseled	= X11.TFrameType.Chiseled,
		
		/// <summary> The frame appears in ledged 3D effect. </summary>
		Ledged	= X11.TFrameType.Ledged,
		
		/// <summary> The frame appears as top aligned tabulator raised 3D effect. </summary>
		RaisedTopTab,
		RaisedTopTabTail,
		
		/// <summary> The frame appears as left aligned tabulator raised 3D effect. </summary>
		RaisedLeftTab,
		
		/// <summary> The frame appears as right aligned tabulator raised 3D effect. </summary>
		RaisedRightTab,
		
		/// <summary> The frame appears as bottom aligned tabulator raised 3D effect. </summary>
		RaisedBottomTab,
		
		/// <summary> The frame appears as top aligned tabulator unraised 3D effect. </summary>
		UnraisedTopTab,
		UnraisedTopTabTail,
		
		/// <summary> The frame appears as left aligned tabulator unraised 3D effect. </summary>
		UnraisedLeftTab,
		
		/// <summary> The frame appears as right aligned tabulator unraised 3D effect. </summary>
		UnraisedRightTab,
		
		/// <summary> The frame appears as bottom aligned tabulator unraised 3D effect. </summary>
		UnraisedBottomTab,
		
		/// <summary> The frame appears as top aligned tabulator sunken 3D effect. </summary>
		SunkenTopTab,
		SunkenTopTabTail,
		
		/// <summary> The frame appears as left aligned tabulator sunken 3D effect. </summary>
		SunkenLeftTab,
		
		/// <summary> The frame appears as right aligned tabulator sunken 3D effect. </summary>
		SunkenRightTab,
		
		/// <summary> The frame appears as bottom aligned tabulator sunken 3D effect. </summary>
		SunkenBottomTab,
		
		/// <summary> The frame appears as top aligned tabulator unsunken 3D effect. </summary>
		UnsunkenTopTab,
		UnsunkenTopTabTail,
		
		/// <summary> The frame appears as left aligned tabulator unsunken 3D effect. </summary>
		UnsunkenLeftTab,
		
		/// <summary> The frame appears as right aligned tabulator unsunken 3D effect. </summary>
		UnsunkenRightTab,
		
		/// <summary> The frame appears as bottom aligned tabulator unsunken 3D effect. </summary>
		UnsunkenBottomTab
	}
	
	/// <summary> The windowless fundamental invisible object class with minimalistic appearence (background, shadow). </summary>
	/// <remarks> This class took the idea of Athenas *** Unknown *** and avoids double definitions in XrwCore and XrwSme. </remarks>
	public class XrwVisibleRectObj : XrwRectObj
	{
		

        // ###############################################################################
        // ### C O N S T A N T S
        // ###############################################################################

        #region Constants

        /// <summary> The class name constant. </summary>
        public new const string		CLASS_NAME = "XrwVisibleRectObj";
	
        #endregion

		// ###############################################################################
        // ### A T T R I B U T E S
        // ###############################################################################

		#region Attributes
		
		/// <summary> The X11 display pointer. </summary>
		protected IntPtr		_display;
		
		/// <summary> The X11 screen number. </summary>
		protected TInt			_screenNumber;
		
		/// <summary> The X11 *** parent *** window for XrwObject/XrwRectObj/XrwCore and derived widgets.
		/// Or the *** own *** window for all XrwSimple and derived widgets. </summary>
		protected IntPtr		_window;
		
		/// <summary> The X11 graphics context to draw on. </summary>
		protected IntPtr		_gc;
		
		/// <summary> The X11 background pixel do use for background drawing. </summary>
		protected TPixel		_backgroundColorPixel;

		/// <summary> The frame type. </summary>
		internal TFrameTypeExt	_frameType				= TFrameTypeExt.None;
		
		/// <summary> The border width of the outside border. </summary>
		protected int			_frameWidth				= XrwTheme.NonInteractingFrameWidth;
		
		/// <summary> The X11 border pixel do use for dark shadow drawing. </summary>
		protected TPixel		_darkShadowColorPixel;
		
		/// <summary> The X11 border pixel do use for light shadow drawing. </summary>
		protected TPixel		_lightShadowColorPixel;
		
        #endregion

        // ###############################################################################
        // ### C O N S T R U C T I O N   A N D   I N I T I A L I Z A T I O N
        // ###############################################################################

        #region Construction
		
		/// <summary> Minimal initializing constructor. Use this constructor for *** application and toplevel shell classes *** only! </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <remarks> This constructor skips the initialization of a graphics context. Therefor the graphics context must be initialized by caller. </remarks>
		protected XrwVisibleRectObj (IntPtr display, TInt screenNumber)
		{
			if (display == IntPtr.Zero)
				throw new ArgumentNullException ("display");
			
			_display = display;
			_screenNumber = screenNumber;
		}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window for X11 calls. This widget has no X11 *** own *** window. <see cref="System.IntPtr"/> </param>
		public XrwVisibleRectObj (IntPtr display, TInt screenNumber, IntPtr parentWindow)
		{
			if (display == IntPtr.Zero)
				throw new ArgumentNullException ("display");
			if (parentWindow == IntPtr.Zero)
				throw new ArgumentNullException ("parentWindow");
			
			_display = display;
			_screenNumber = screenNumber;
			_window = parentWindow;
			
			InitializeXrwVisibleRectObjRessources ();
		}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window for X11 calls. This widget has no X11 *** own *** window. <see cref="IntPtr"/> </param>
		/// <param name="assignedPosition"> The position of the top left top corner assigned by the window manager (for shell widgets) or geometry management (by non-shell widgets). Passed as reference to avoid structure copy constructor calls. <see cref="TPoint"/> </param>
		public XrwVisibleRectObj (IntPtr display, X11.TInt screenNumber, IntPtr parentWindow, ref TPoint assignedPosition)
			: base (ref assignedPosition)
		{
			if (display == IntPtr.Zero)
				throw new ArgumentNullException ("display");
			if (parentWindow == IntPtr.Zero)
				throw new ArgumentNullException ("parentWindow");
			
			_display = display;
			_screenNumber = screenNumber;
			_window = parentWindow;
			
			InitializeXrwVisibleRectObjRessources ();
		}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window for X11 calls. This widget has no X11 *** own *** window. <see cref="IntPtr"/> </param>
		/// <param name="fixedSize"> The fixed size, that ist to use (if set) rather than the calcualated size. Passed as reference to avoid structure copy constructor calls. <see cref="TSize"/> </param>
		public XrwVisibleRectObj (IntPtr display, X11.TInt screenNumber, IntPtr parentWindow, ref TSize fixedSize)
			: base (ref fixedSize)
		{
			if (display == IntPtr.Zero)
				throw new ArgumentNullException ("display");
			if (parentWindow == IntPtr.Zero)
				throw new ArgumentNullException ("parentWindow");
			
			_display = display;
			_screenNumber = screenNumber;
			_window = parentWindow;
			
			InitializeXrwVisibleRectObjRessources ();
		}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window for X11 calls. This widget has no X11 *** own *** window. <see cref="IntPtr"/> </param>
		/// <param name="assignedPosition"> The position of the top left top corner assigned by the window manager (for shell widgets) or geometry management (by non-shell widgets). Passed as reference to avoid structure copy constructor calls. <see cref="TPoint"/> </param>
		/// <param name="fixedSize"> The fixed size, that ist to use (if set) rather than the calcualated size. Passed as reference to avoid structure copy constructor calls. <see cref="TSize"/> </param>
		public XrwVisibleRectObj (IntPtr display, X11.TInt screenNumber, IntPtr parentWindow, ref TPoint assignedPosition, ref TSize fixedSize)
			: base (ref assignedPosition, ref fixedSize)
		{
			if (display == IntPtr.Zero)
				throw new ArgumentNullException ("display");
			if (parentWindow == IntPtr.Zero)
				throw new ArgumentNullException ("parentWindow");
			
			_display = display;
			_screenNumber = screenNumber;
			_window = parentWindow;
			
			InitializeXrwVisibleRectObjRessources ();
		}
		
		/// <summary> Initialize local ressources for all constructors. </summary>
		private void InitializeXrwVisibleRectObjRessources ()
		{
			_gc = X11lib.XCreateGC(_display, _window, 0, IntPtr.Zero);        
			if (_gc == IntPtr.Zero)
				throw new OperationCanceledException ("Failed to create graphics context.");

			_backgroundColorPixel  = X11lib.XAllocParsedColorByName (_display, _screenNumber, XrwTheme.BackGroundColor);
			_darkShadowColorPixel  = X11lib.XAllocParsedColorByName (_display, _screenNumber, XrwTheme.DarkShadowColor);
			_lightShadowColorPixel = X11lib.XAllocParsedColorByName (_display, _screenNumber, XrwTheme.LightShadowColor);
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
			// Free shared ressources.
			if (_gc != IntPtr.Zero)
			{
				X11lib.XFreeGC (_display, _gc);
				_gc = IntPtr.Zero;
			}

			base.DisposeByParent ();
		}
		
		#endregion
		
        // ###############################################################################
        // ### P R O P E R T I E S
        // ###############################################################################

		#region Properties
		
		/// <summary> Get the display pointer. </summary>
		public IntPtr Display
		{
			get { return _display; }
		}
		
		/// <summary> Get the screen number. </summary>
		public X11.TInt Screen
		{
			get { return _screenNumber; }
		}
		
		/// <summary> The X11 *** parent *** window for XrwObject/XrwRectObj/XrwCore and derived widgets.
		/// Or the *** own *** window for all XrwSimple and derived widgets. </summary>
		public IntPtr Window
		{
			get { return _window; }
		}
		
		/// <summary> Get the X11 graphics context to draw on. </summary>
		public IntPtr GC
		{
			get	{	return _gc;	}
		}
		
		/// <summary> Get or set the X11 background pixel do use for background drawing. </summary>
		public TPixel BackgroundColor
		{
			get	{	return _backgroundColorPixel;	}
			set {	_backgroundColorPixel = value;	}
		}

		/// <summary> Get or set the frame type. </summary>
		public virtual TFrameType FrameType
		{
			get {	return (TFrameType)_frameType;		}
			set{	_frameType = (TFrameTypeExt)value;	}
		}

		/// <summary> Get or set the frame type. </summary>
		internal virtual TFrameTypeExt FrameTypeExt
		{
			get {	return _frameType;		}
			set{	_frameType = value;	}
		}
		
		/// <summary> Get or set frame width. </summary>
		/// <param name="value"> The frame width to set. <see cref="System.Int32"/> </param>
		/// <returns> The frame width. <see cref="System.Int32"/> </returns>
		public virtual int FrameWidth
		{
			get { return _frameWidth; }
			set
			{
				_frameWidth = (Math.Max (0, value));
				if (value < 0)
					Console.WriteLine (CLASS_NAME + "::FrameWidth () WARNING: The frame width must be greater or equal 0.");
			}
		}
		
		/// <summary> Get the X11 porder pixel do use for dark shadow drawing. </summary>
		public TPixel DarkShadowColor
		{
			get { return _darkShadowColorPixel;	}
		}
		
		/// <summary> Get the X11 porder pixel do use for light shadow drawing. </summary>
		public TPixel LightShadowColor
		{
			get { return _lightShadowColorPixel;	}
		}

		#endregion
	
        // ###############################################################################
        // ### M E T H O D S
        // ###############################################################################
		
		#region Methods
		
		/// <summary> Get the window attributes of the underlaying X11 window. </summary>
		/// <param name="windowAttributes"> The structure containing the determined window attributes. <see cref="X11lib.XWindowAttributes"/> </param>
		/// <returns> True on success, or false on any error. <see cref="System.Boolean"/> </returns>
		public bool GetWindowAttributes (ref X11lib.XWindowAttributes windowAttributes)
		{
			if (X11lib.XGetWindowAttributes (_display, _window, ref windowAttributes) != (TInt)0)
				return true;
			else
				return false;
		}
		
		#endregion

		#region Drawing methods
		
		/// <summary> Draw the background. </summary>
		/// <param name="display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="window"> The window to draw on. <see cref="IntPtr"/> </param>
		/// <param name="gc"> The graphics context to use for drawing. <see cref="IntPtr"/> </param>
		/// <param name="backgroundArea"> The area to draw the background on. <see cref="TRectangle"/> </param>
		public virtual void DrawBackground (IntPtr display, IntPtr window, IntPtr gc, TRectangle backgroundArea)
		{
			if (display == IntPtr.Zero)
			{
				Console.WriteLine (CLASS_NAME + "::DrawBackground () ERROR: Argument null: display");
				return;
			}
			if (window == IntPtr.Zero)
			{
				Console.WriteLine (CLASS_NAME + "::DrawBackground () ERROR: Argument null: window");
				return;
			}
			if (gc == IntPtr.Zero)
			{
				Console.WriteLine (CLASS_NAME + "::DrawBackground () ERROR: Argument null: gc");
				return;
			}
			
			X11lib.XFillRectangle (display, window, gc, (TInt)backgroundArea.X, (TInt)backgroundArea.Y,
			                       (TUint)backgroundArea.Width, (TUint)backgroundArea.Height);
		}

		/// <summary> Draw the border. </summary>
		/// <param name="display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="window"> The window to draw on. <see cref="IntPtr"/> </param>
		/// <param name="gc"> The graphics context to use for drawing. <see cref="IntPtr"/> </param>
		/// <param name="borderArea"> The area to draw the border on. <see cref="TRectangle"/> </param>
		/// <param name="borderWidth"> The width of the border. <see cref="System.Int32"/> </param>
		public virtual void DrawBorder (IntPtr display, IntPtr window, IntPtr gc, TRectangle borderArea, int borderWidth)
		{
			if (display == IntPtr.Zero)
			{
				Console.WriteLine (CLASS_NAME + "::DrawOuterBorder () ERROR: Argument null: display");
				return;
			}
			if (window == IntPtr.Zero)
			{
				Console.WriteLine (CLASS_NAME + "::DrawOuterBorder () ERROR: Argument null: window");
				return;
			}
			if (gc == IntPtr.Zero)
			{
				Console.WriteLine (CLASS_NAME + "::DrawOuterBorder () ERROR: Argument null: gc");
				return;
			}

			if (borderWidth <= 0)
				return;
			
			for (int counter = 0; counter < (int)borderWidth; counter++)
			{
				// Top: L ==> R
				X11lib.XDrawLine  (display, window, gc, (X11.TInt)(borderArea.Left + counter),           (X11.TInt)(borderArea.Top + counter),
				                                        (X11.TInt)((int)borderArea.Right - counter - 1), (X11.TInt)(borderArea.Top + counter));
				// Right: T ==> B
				X11lib.XDrawLine  (display, window, gc, (X11.TInt)((int)borderArea.Right - counter - 1), (X11.TInt)(borderArea.Top + counter),
				                                        (X11.TInt)((int)borderArea.Right - counter - 1), (X11.TInt)((int)borderArea.Bottom - counter - 1));
				// Bottom: R ==> L
				X11lib.XDrawLine  (display, window, gc, (X11.TInt)((int)borderArea.Right - counter - 1), (X11.TInt)((int)borderArea.Bottom - counter - 1),
				                                        (X11.TInt)(borderArea.Left + counter),           (X11.TInt)((int)borderArea.Bottom - counter - 1));
				// Left: B ==> T
				X11lib.XDrawLine  (display, window, gc, (X11.TInt)(borderArea.Left + counter),           (X11.TInt)((int)borderArea.Bottom - counter - 1),
				                                        (X11.TInt)(borderArea.Left + counter),           (X11.TInt)(borderArea.Top + counter));
				// Fix X11lib drawing problems (not drawn pixel at bottom right corner).
				X11lib.XDrawPoint (display, window, gc, (X11.TInt)((int)borderArea.Right - (int)counter - 1), (X11.TInt)(borderArea.Bottom - (int)counter - 1));
			}
		}
		
		/// <summary> Draw the frame ( 3D effect). </summary>
		/// <param name="display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="window"> The window to draw on. <see cref="IntPtr"/> </param>
		/// <param name="gc"> The graphics context to use for drawing. <see cref="IntPtr"/> </param>
		/// <param name="clientArea"> The area to draw the shadow on. <see cref="TRectangle"/> </param>
		/// <param name="frameType"> The frame type to draw. <see cref="TFrameTypeExt"/> </param>
		/// <param name="darkShadow"> The X11 border pixel do use for dark shadow drawing. <see cref="TPixel"/> </param>
		/// <param name="lightShadow"> The X11 border pixel do use for light shadow drawing.<see cref="TPixel"/> </param>
		internal virtual void DrawFrame (IntPtr display, IntPtr window, IntPtr gc, TRectangle clientArea, TFrameTypeExt frameType, TPixel darkShadow, TPixel lightShadow)
		{
			if (display == IntPtr.Zero)
			{
				Console.WriteLine (CLASS_NAME + "::DrawFrame () ERROR: Argument null: display");
				return;
			}
			if (window == IntPtr.Zero)
			{
				Console.WriteLine (CLASS_NAME + "::DrawFrame () ERROR: Argument null: window");
				return;
			}
			if (gc == IntPtr.Zero)
			{
				Console.WriteLine (CLASS_NAME + "::DrawFrame () ERROR: Argument null: gc");
				return;
			}

			if (frameType == TFrameTypeExt.None)
				return;
			
			X11lib.XGCValues xgcValues = new X11lib.XGCValues ();
			xgcValues.line_width       = (X11.TInt)1;
			TUint            xgcMask   = (TUint)(X11lib.GCattributemask.GCLineWidth);
			X11lib.XChangeGC (display, gc, xgcMask, ref xgcValues);
			
			if (frameType == TFrameTypeExt.Raised             || frameType == TFrameTypeExt.Sunken             ||
			    frameType == TFrameTypeExt.RaisedTopTab       || frameType == TFrameTypeExt.RaisedLeftTab      ||
			    frameType == TFrameTypeExt.RaisedRightTab     || frameType == TFrameTypeExt.RaisedBottomTab    ||
			    frameType == TFrameTypeExt.RaisedTopTabTail   || frameType == TFrameTypeExt.SunkenTopTabTail   ||
			    frameType == TFrameTypeExt.UnraisedTopTab     || frameType == TFrameTypeExt.UnraisedLeftTab    ||
			    frameType == TFrameTypeExt.UnraisedRightTab   || frameType == TFrameTypeExt.UnraisedBottomTab  ||
			    frameType == TFrameTypeExt.UnraisedTopTabTail || frameType == TFrameTypeExt.UnsunkenTopTabTail ||
			    frameType == TFrameTypeExt.SunkenTopTab       || frameType == TFrameTypeExt.SunkenLeftTab      ||
			    frameType == TFrameTypeExt.SunkenRightTab     || frameType == TFrameTypeExt.SunkenBottomTab    ||
			    frameType == TFrameTypeExt.UnsunkenTopTab     || frameType == TFrameTypeExt.UnsunkenLeftTab    ||
			    frameType == TFrameTypeExt.UnsunkenRightTab   || frameType == TFrameTypeExt.UnsunkenBottomTab)
			{
				bool raised = true;
				
				if (frameType == TFrameTypeExt.Sunken ||
				    frameType == TFrameTypeExt.SunkenTopTab     || frameType == TFrameTypeExt.SunkenTopTabTail   ||
				    frameType == TFrameTypeExt.SunkenLeftTab    || frameType == TFrameTypeExt.SunkenRightTab     ||
				    frameType == TFrameTypeExt.SunkenBottomTab  ||
				    frameType == TFrameTypeExt.UnsunkenTopTab   || frameType == TFrameTypeExt.UnsunkenTopTabTail ||
				    frameType == TFrameTypeExt.UnsunkenLeftTab  || frameType == TFrameTypeExt.UnsunkenRightTab   ||
				    frameType == TFrameTypeExt.UnsunkenBottomTab)
					raised = false;
				
				// Right and bottom edge.
				X11lib.XSetForeground (display, gc, (raised ? darkShadow : lightShadow));
				for (int width = 0; width < _frameWidth; width++)
				{
					// Right.
					if (frameType != TFrameTypeExt.RaisedLeftTab      && frameType != TFrameTypeExt.SunkenLeftTab     &&
					    frameType != TFrameTypeExt.UnraisedRightTab   && frameType != TFrameTypeExt.UnraisedTopTab    &&
					    frameType != TFrameTypeExt.UnraisedTopTabTail && frameType != TFrameTypeExt.UnraisedBottomTab &&
					    frameType != TFrameTypeExt.UnsunkenRightTab   && frameType != TFrameTypeExt.UnsunkenTopTab    &&
					    frameType != TFrameTypeExt.UnsunkenTopTabTail && frameType != TFrameTypeExt.UnsunkenBottomTab)
					{
						if		(frameType == TFrameTypeExt.UnraisedLeftTab  || frameType == TFrameTypeExt.UnsunkenLeftTab)
							X11lib.XDrawLine      (display, window, gc, (TInt)clientArea.Right - 1 - width, (TInt)clientArea.Top    - 1,
							                                            (TInt)clientArea.Right - 1 - width, (TInt)clientArea.Bottom + 1);
						else if (frameType == TFrameTypeExt.RaisedTopTab     || frameType == TFrameTypeExt.SunkenTopTab ||
								 frameType == TFrameTypeExt.RaisedTopTabTail || frameType == TFrameTypeExt.SunkenTopTabTail)
							X11lib.XDrawLine      (display, window, gc, (TInt)clientArea.Right - 1 - width, (TInt)clientArea.Top,
							                                            (TInt)clientArea.Right - 1 - width, (TInt)clientArea.Bottom);
						else if (frameType == TFrameTypeExt.RaisedBottomTab  || frameType == TFrameTypeExt.SunkenBottomTab)
							X11lib.XDrawLine      (display, window, gc, (TInt)clientArea.Right - 1 - width, (TInt)clientArea.Top    - 1,
							                                            (TInt)clientArea.Right - 1 - width, (TInt)clientArea.Bottom);
						else
							X11lib.XDrawLine      (display, window, gc, (TInt)clientArea.Right - 1 - width, (TInt)clientArea.Top    + width,
							                                            (TInt)clientArea.Right - 1 - width, (TInt)clientArea.Bottom - width);
					}
					// Bottom.
					if (frameType != TFrameTypeExt.RaisedTopTab      && frameType != TFrameTypeExt.RaisedTopTabTail &&
					    frameType != TFrameTypeExt.SunkenTopTab      && frameType != TFrameTypeExt.SunkenTopTabTail &&
					    frameType != TFrameTypeExt.UnraisedBottomTab && frameType != TFrameTypeExt.UnraisedLeftTab  && frameType != TFrameTypeExt.UnraisedRightTab  &&
					    frameType != TFrameTypeExt.UnsunkenBottomTab && frameType != TFrameTypeExt.UnsunkenLeftTab  && frameType != TFrameTypeExt.UnsunkenRightTab)
					{
						if (frameType == TFrameTypeExt.UnraisedTopTab || frameType == TFrameTypeExt.UnraisedTopTabTail ||
						    frameType == TFrameTypeExt.UnsunkenTopTab || frameType == TFrameTypeExt.UnsunkenTopTabTail)
							X11lib.XDrawLine      (display, window, gc, (TInt)clientArea.Left      - 1, (TInt)clientArea.Bottom - 1 - width,
							                                            (TInt)clientArea.Right - 1 + 1, (TInt)clientArea.Bottom - 1 - width);
						else
							X11lib.XDrawLine      (display, window, gc, (TInt)clientArea.Left      + width, (TInt)clientArea.Bottom - 1 - width,
							                                            (TInt)clientArea.Right - 1 - width, (TInt)clientArea.Bottom - 1 - width);
					}
				}
				
				// Top and Left edge.
				X11lib.XSetForeground (display, gc, (raised ? lightShadow : darkShadow));
				for (int width = 0; width < _frameWidth; width++)
				{
					// Top.
					if (frameType != TFrameTypeExt.RaisedBottomTab   && frameType != TFrameTypeExt.SunkenBottomTab    &&
					    frameType != TFrameTypeExt.UnraisedTopTab    && frameType != TFrameTypeExt.UnraisedTopTabTail &&
					    frameType != TFrameTypeExt.UnraisedLeftTab   && frameType != TFrameTypeExt.UnraisedRightTab   &&
					    frameType != TFrameTypeExt.UnsunkenTopTab    && frameType != TFrameTypeExt.UnsunkenTopTabTail &&
					    frameType != TFrameTypeExt.UnsunkenLeftTab   && frameType != TFrameTypeExt.UnsunkenRightTab)
					{
						if (frameType == TFrameTypeExt.UnraisedBottomTab  || frameType == TFrameTypeExt.UnsunkenBottomTab)
							X11lib.XDrawLine      (display, window, gc, (TInt)clientArea.Left  - 1, (TInt)clientArea.Top    + width,
						    	                                        (TInt)clientArea.Right + 1, (TInt)clientArea.Top    + width);
						else
							X11lib.XDrawLine      (display, window, gc, (TInt)clientArea.Left  + width, (TInt)clientArea.Top    + width,
						    	                                        (TInt)clientArea.Right - width, (TInt)clientArea.Top    + width);
					}
					// Left.
					if (frameType != TFrameTypeExt.RaisedRightTab     && frameType != TFrameTypeExt.SunkenRightTab    &&
					    frameType != TFrameTypeExt.UnraisedLeftTab    && frameType != TFrameTypeExt.UnraisedTopTab    &&
					    frameType != TFrameTypeExt.UnraisedTopTabTail && frameType != TFrameTypeExt.UnraisedBottomTab &&
					    frameType != TFrameTypeExt.UnsunkenLeftTab    && frameType != TFrameTypeExt.UnsunkenTopTab    &&
					    frameType != TFrameTypeExt.UnsunkenTopTabTail && frameType != TFrameTypeExt.UnsunkenBottomTab)
					{
						if		(frameType == TFrameTypeExt.UnraisedRightTab || frameType == TFrameTypeExt.UnsunkenRightTab)
							X11lib.XDrawLine      (display, window, gc, (TInt)clientArea.Left  + width, (TInt)clientArea.Top    - 1,
						    	                                        (TInt)clientArea.Left  + width, (TInt)clientArea.Bottom + 1);
						else if (frameType == TFrameTypeExt.RaisedTopTab     || frameType == TFrameTypeExt.SunkenTopTab ||
								 frameType == TFrameTypeExt.RaisedTopTabTail || frameType == TFrameTypeExt.SunkenTopTabTail)
							X11lib.XDrawLine      (display, window, gc, (TInt)clientArea.Left  + width, (TInt)clientArea.Top,
						    	                                        (TInt)clientArea.Left  + width, (TInt)clientArea.Bottom + 1);
						else if (frameType == TFrameTypeExt.RaisedBottomTab  || frameType == TFrameTypeExt.SunkenBottomTab)
							X11lib.XDrawLine      (display, window, gc, (TInt)clientArea.Left  + width, (TInt)clientArea.Top    - 1,
						    	                                        (TInt)clientArea.Left  + width, (TInt)clientArea.Bottom - width);
						else
							X11lib.XDrawLine      (display, window, gc, (TInt)clientArea.Left  + width, (TInt)clientArea.Top    + width,
						    	                                        (TInt)clientArea.Left  + width, (TInt)clientArea.Bottom - width);
					}
				}
				// Finish right bottom edge.
				if (frameType == TFrameTypeExt.RaisedTopTab     || frameType == TFrameTypeExt.SunkenTopTab)
				{
					X11lib.XSetForeground (display, gc, (raised ? lightShadow : darkShadow));
					for (int width = 0; width < _frameWidth; width++)
					{
						if (width >= 1)
							X11lib.XDrawLine      (display, window, gc, (TInt)clientArea.Right - _frameWidth + width, (TInt)clientArea.Bottom - width,
							                                            (TInt)clientArea.Right - _frameWidth + width, (TInt)clientArea.Bottom);
					}
				}
				if (frameType == TFrameTypeExt.UnraisedTopTabTail || frameType == TFrameTypeExt.UnsunkenTopTabTail)
				{
					X11lib.XSetForeground (display, gc, (raised ? lightShadow : darkShadow));
					for (int width = 0; width < _frameWidth; width++)
					{
						if (width >= 1)
							X11lib.XDrawLine      (display, window, gc, (TInt)clientArea.Right - _frameWidth + width, (TInt)clientArea.Bottom - width,
							                                            (TInt)clientArea.Right - _frameWidth + width, (TInt)clientArea.Bottom);
					}
				}
				// Finish top left edge.
				// else if (frameType == TFrameTypeExt.RaisedBottomTab     || frameType == TFrameTypeExt.SunkenBottomTab)
			}
			else if (frameType == TFrameTypeExt.Chiseled || frameType == TFrameTypeExt.Ledged)
			{
				int halfWidth = _frameWidth / 2;
				
				// Bottom and right edge.
				X11lib.XSetForeground (display, gc, (frameType == TFrameTypeExt.Chiseled ? lightShadow : darkShadow));
				for (int width = 0; width < halfWidth; width++)
				{
					X11lib.XDrawLine      (display, window, gc, (TInt)clientArea.Right - 1 - width, (TInt)clientArea.Top    + width,
					                                            (TInt)clientArea.Right - 1 - width, (TInt)clientArea.Bottom - width);
					X11lib.XDrawLine      (display, window, gc, (TInt)clientArea.Left      + width, (TInt)clientArea.Bottom - 1 - width,
					                                            (TInt)clientArea.Right - 1 - width, (TInt)clientArea.Bottom - 1 - width);
				}
				X11lib.XSetForeground (display, gc, (frameType == TFrameTypeExt.Chiseled ? darkShadow : lightShadow));
				for (int width = halfWidth; width < _frameWidth; width++)
				{
					X11lib.XDrawLine      (display, window, gc, (TInt)clientArea.Right - 1 - width, (TInt)clientArea.Top    + width,
					                                            (TInt)clientArea.Right - 1 - width, (TInt)clientArea.Bottom - width);
					X11lib.XDrawLine      (display, window, gc, (TInt)clientArea.Left      + width, (TInt)clientArea.Bottom - 1 - width,
					                                            (TInt)clientArea.Right - 1 - width, (TInt)clientArea.Bottom - 1 - width);
				}
				
				// Top and left edge.
				X11lib.XSetForeground (display, gc, (frameType == TFrameTypeExt.Chiseled ? darkShadow : lightShadow));
				for (int width = 0; width < halfWidth; width++)
				{
					X11lib.XDrawLine      (display, window, gc, (TInt)clientArea.Left  + width, (TInt)clientArea.Top    + width,
					                                            (TInt)clientArea.X     + width, (TInt)clientArea.Bottom - width);
					X11lib.XDrawLine      (display, window, gc, (TInt)clientArea.Left  + width, (TInt)clientArea.Top    + width,
					                                            (TInt)clientArea.Right - width, (TInt)clientArea.Top    + width);
				}
				X11lib.XSetForeground (display, gc, (frameType == TFrameTypeExt.Chiseled ? lightShadow : darkShadow));
				for (int width = halfWidth; width < _frameWidth; width++)
				{
					X11lib.XDrawLine      (display, window, gc, (TInt)clientArea.Left  + width, (TInt)clientArea.Top    + width,
					                                            (TInt)clientArea.X     + width, (TInt)clientArea.Bottom - width);
					X11lib.XDrawLine      (display, window, gc, (TInt)clientArea.Left  + width, (TInt)clientArea.Top    + width,
					                                            (TInt)clientArea.Right - width, (TInt)clientArea.Top    + width);
				}
			}
		}
		
		/// <summary> Measure the text using the default font of indicated graphics context. </summary>
		/// <param name="display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="gc"> The crapchics context to use for measuring. <see cref="IntPtr"/> </param>
		/// <param name="text"> The text to measure. <see cref="X11.TChar[]"/> </param>
		/// <returns> The measured size of the indicated text. <see cref="TSize"/> </returns>
		public virtual TSize MeasureTextLine (IntPtr display, IntPtr gc, X11.TChar[] text)
		{
			TSize result = new TSize (5, 5);
			
			if (display == IntPtr.Zero)
			{
				Console.WriteLine (CLASS_NAME + "::MeasureTextLine () ERROR: Argument null: display");
				return result;
			}
			if (gc == IntPtr.Zero)
			{
				Console.WriteLine (CLASS_NAME + "::MeasureTextLine () ERROR: Argument null: gc");
				return result;
			}
				
			TInt gcID = X11lib.XGContextFromGC (gc);
			if (gcID != 0)
			{
				TInt direction = 0;
				TInt fontAscent = 0;
				TInt fontDescent = 0;
				X11lib.XCharStruct xCharStruct = new X11lib.XCharStruct ();
				
				X11lib.XQueryTextExtents (display, gcID, text, (X11.TInt)text.Length, ref direction, ref fontAscent, ref fontDescent, ref xCharStruct);
				result.Width  = (int)xCharStruct.width;
				result.Height = (int)fontAscent + (int)fontDescent;
			}
			else
			{
				Console.WriteLine (CLASS_NAME + "::MeasureTextLine () ERROR: Cannot investigate default font ressource ID of underlaying graphics context.");
			}
			return result;
		}
		
		/// <summary> Draw text with current font of indicated graphics context. </summary>
		/// <param name="display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="window"> The window to draw on. <see cref="IntPtr"/> </param>
		/// <param name="gc"> The graphics context to use for drawing. <see cref="IntPtr"/> </param>
		/// <param name="text"> The tyt to draw. <see cref="X11.TChar[]"/> </param>
		/// <param name="area"> The area to draw within. <see cref="Rectangle"/> </param>
		/// <param name="horzAlign"> The horizontal alignment (0.5F is centered). <see cref="XFloat"/> </param>
		/// <param name="vertAlign"> The vertical alignment (0.5F is centered). <see cref="XFloat"/> </param>
		public virtual void DrawTextLine (IntPtr display, IntPtr window, IntPtr gc, X11.TChar[] text, X11.TRectangle area, float horzAlign, float vertAlign)
		{
			if (display == IntPtr.Zero)
			{
				Console.WriteLine (CLASS_NAME + "::DrawTextLine () ERROR: Argument null: display");
				return;
			}
			if (window == IntPtr.Zero)
			{
				Console.WriteLine (CLASS_NAME + "::DrawTextLine () ERROR: Argument null: window");
				return;
			}
			if (gc == IntPtr.Zero)
			{
				Console.WriteLine (CLASS_NAME + "::DrawTextLine () ERROR: Argument null: gc");
				return;
			}

			if (horzAlign < 0.0F || horzAlign > 1.0F)
			{
				Console.WriteLine (CLASS_NAME + "::DrawTextLine () ERROR: Argument out of range (0.0 ... 1.0): horzAlign");
				if (horzAlign < 0.0F)
					horzAlign = 0.0F;
				if (horzAlign > 1.0F)
					horzAlign = 1.0F;
			}
			if (vertAlign < 0.0F || vertAlign > 1.0F)
			{
				Console.WriteLine (CLASS_NAME + "::DrawTextLine () ERROR: Argument out of range (0.0 ... 1.0): vertAlign");
				if (vertAlign < 0.0F)
					vertAlign = 0.0F;
				if (vertAlign > 1.0F)
					vertAlign = 1.0F;
			}
				
			TSize textMeasure = MeasureTextLine (display, gc, text);
			int textX = (int)((area.Width  - textMeasure.Width ) * horzAlign);
			if (textX < _borderWidth + 2)
				textX = _borderWidth + 2;
			if (textX > area.Width - textMeasure.Width - _borderWidth - 2)
				textX = area.Width - textMeasure.Width - _borderWidth - 2;

			int textY = (int)((area.Height - textMeasure.Height) * vertAlign) + (int)(textMeasure.Height * 0.85F);
			
			X11lib.XDrawString    (display, window, gc, (TInt)((int)area.Left + (int)textX), (TInt)((int)area.Top + (int)textY), text, (X11.TInt)text.Length);
		}
		
		// ToDo: Clip
		protected void DrawTextLines (Multiline lines, TRectangle clientArea, float horzTextAlign, float vertTextAlign)
		{
			int			FUZZY           = 4; // Avoid line hiding on rounding errors. 
			int			drawableLines	= lines.CompleteDrawableLines (clientArea.Height + FUZZY);
			TSize		textMeasure		= lines.Measure (0, drawableLines - 1);
			int			extraHeight		= (int)((clientArea.Height - textMeasure.Height) * vertTextAlign);
			TRectangle	lineArea		= new TRectangle (clientArea.X, clientArea.Y + extraHeight, clientArea.Width, 0);
			
			for (int cntLines = 0; cntLines < drawableLines; cntLines++)
			{
				DrawTextLine (_display, _window, _gc, lines[cntLines].Text, lineArea, horzTextAlign, 0.0F);
				lineArea.Y += lines[cntLines].HeightInPixel;
			}
		}
		
		#endregion

		#region XrwRectObj overrides
		
		/// <summary> Calculate the preferred size. </summary>
		/// <returns> The preferred size. <see cref="TSize"/> </returns>
		public override TSize PreferredSize ()
		{
			TSize preferredSize = new TSize (MIN_WIDTH + _borderWidth * 2 + _frameWidth * 2, MIN_HEIGHT + _borderWidth * 2 + _frameWidth * 2);
			
			if (_fixedSize.Width >= 0 && _fixedSize.Width > MIN_WIDTH)
				preferredSize.Width = _fixedSize.Width + _borderWidth * 2 + _frameWidth * 2;
			
			if (_fixedSize.Height >= 0 && _fixedSize.Height > MIN_HEIGHT)
				preferredSize.Height = _fixedSize.Height + _borderWidth * 2 + _frameWidth * 2;
			
			return preferredSize;
		}
		
		#endregion
		
	}
}

