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
	public class XrwToggle : XrwCore
	{

        // ###############################################################################
        // ### C O N S T A N T S
        // ###############################################################################

        #region Constants

        /// <summary> The class name constant. </summary>
        public new const string	CLASS_NAME = "XrwToggle";
		
        #endregion

		// ###############################################################################
        // ### A T T R I B U T E S
        // ###############################################################################

		#region Attributes
		
		/// <summary> The text color. </summary>
		protected TPixel		_textColorPixel;
		
		/// <summary> The label to display. </summary>
		protected string		_label					= "";
		
		/// <summary> The label to display, transformed into measured lines. </summary>
		protected Multiline		_lines					= null;
		
		/// <summary> Spacing from left bitmap to label (if != null) and from label to top/bottom. </summary>
		public const int		_space = 4;
		
		/// <summary> The horizontal text alignment (0.5F is centered). </summary>
		protected float			_horzTextAlign			= 0.0F;
		
		/// <summary> The vertical text alignment (0.5F is centered). </summary>
		protected float			_vertTextAlign			= 0.5F;
		
		/// <summary> The space (in pixels) that will be lef free between the left edge and the content. </summary>
		protected int			_leftMargin				= 4;
		
		/// <summary> The space (in pixels) that will be lef free between the right edge and the content. </summary>
		protected int			_rightMargin			= 4;
		
		/// <summary> The image of the off bitmap. </summary>
		protected X11Graphic	_offBitmap			= null;
		
		/// <summary> Indicate wether the offBitmap is shared (desposed by caller) or private (disposed together with tis label). </summary>
		protected bool			_offBitmapShared		= false;
		
		/// <summary> The image of the on bitmap. </summary>
		protected X11Graphic	_onBitmap				= null;
		
		/// <summary> Indicate wether the onBitmap is shared (desposed by caller) or private (disposed together with tis label). </summary>
		protected bool			_onBitmapShared			= false;
		
		/// <summary> The on bitmap vertical alignment (0.5F is centered). </summary>
		protected float			_vertBitmapAlign		= 0.5F;
		
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
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window for X11 calls. This widget has no X11 *** own *** window. <see cref="System.IntPtr"/> </param>
		/// <param name="label"> The label to display. <see cref="System.String"/> </param>
		/// <param name="offBitmap"> The right bitmap to display. <see cref="XrwBitmap"/> </param>
		/// <param name="offShared"> Indicate wether the offBitmap is shared (desposed by caller) or private (disposed together with tis label). <see cref="System.Boolean"/> </param>
		/// <param name="onBitmap"> The left bitmap to display. <see cref="XrwBitmap"/> </param>
		/// <param name="onShared"> Indicate wether the onBitmap is shared (desposed by caller) or private (disposed together with tis label). <see cref="System.Boolean"/> </param>
		public XrwToggle (IntPtr display, X11.TInt screenNumber, IntPtr parentWindow, string label, X11Graphic offBitmap, bool offShared, X11Graphic onBitmap, bool onShared)
			: base (display, screenNumber, parentWindow)
		{	
			_label = label;
			_lines = new Multiline (this, _label);
			_offBitmap = offBitmap;
			_offBitmapShared = offShared;
			_onBitmap = onBitmap;
			_onBitmapShared = onShared;
			
			// Create a *** own *** window and exchange the *** parent ***  window against it.
			base.InitializeOwnWindow ();

			InitializeToggleRessources ();
		}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window for X11 calls. This widget has no X11 *** own *** window. <see cref="System.IntPtr"/> </param>
		/// <param name="fixedSize"> The fixed size, that ist to use (if set) rather than the calcualated size. Passed as reference to avoid structure copy constructor calls. <see cref="TIntSize"/> </param>
		/// <param name="label"> The label to display. <see cref="System.String"/> </param>
		/// <param name="offBitmap"> The right bitmap to display. <see cref="XrwBitmap"/> </param>
		/// <param name="offShared"> Indicate wether the offBitmap is shared (desposed by caller) or private (disposed together with tis label). <see cref="System.Boolean"/> </param>
		/// <param name="onBitmap"> The left bitmap to display. <see cref="XrwBitmap"/> </param>
		/// <param name="onShared"> Indicate wether the onBitmap is shared (desposed by caller) or private (disposed together with tis label). <see cref="System.Boolean"/> </param>
		public XrwToggle (IntPtr display, X11.TInt screen, IntPtr parentWindow, ref TSize fixedSize, string label, X11Graphic offBitmap, bool offShared, X11Graphic onBitmap, bool onShared)
			: base (display, screen, parentWindow, ref fixedSize)
		{
			_label = label;
			_lines = new Multiline (this, _label);
			_offBitmap = offBitmap;
			_offBitmapShared = offShared;
			_onBitmap = onBitmap;
			_onBitmapShared = onShared;
			
			// Create a *** own *** window and exchange the *** parent ***  window against it.
			base.InitializeOwnWindow ();

			InitializeToggleRessources ();
		}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window for X11 calls. This widget has no X11 *** own *** window. <see cref="System.IntPtr"/> </param>
		/// <param name="assignedPosition"> The position of the top left top corner assigned by the window manager (for shell widgets) or geometry management (by non-shell widgets). Passed as reference to avoid structure copy constructor calls. <see cref="TPoint"/> </param>
		/// <param name="fixedSize"> The fixed size, that ist to use (if set) rather than the calcualated size. Passed as reference to avoid structure copy constructor calls. <see cref="TIntSize"/> </param>
		/// <param name="label"> The label to display. <see cref="System.String"/> </param>
		/// <param name="offBitmap"> The right bitmap to display. <see cref="XrwBitmap"/> </param>
		/// <param name="offShared"> Indicate wether the offBitmap is shared (desposed by caller) or private (disposed together with tis label). <see cref="System.Boolean"/> </param>
		/// <param name="onBitmap"> The left bitmap to display. <see cref="XrwBitmap"/> </param>
		/// <param name="onShared"> Indicate wether the onBitmap is shared (desposed by caller) or private (disposed together with tis label). <see cref="System.Boolean"/> </param>
		public XrwToggle (IntPtr display, X11.TInt screenNumber, IntPtr parentWindow, ref TPoint assignedPosition, ref TSize fixedSize, string label, X11Graphic offBitmap, bool offShared, X11Graphic onBitmap, bool onShared)
			: base (display, screenNumber, parentWindow, ref assignedPosition, ref fixedSize)
		{	
			_label = label;
			_lines = new Multiline (this, _label);
			_offBitmap = offBitmap;
			_offBitmapShared = offShared;
			_onBitmap = onBitmap;
			_onBitmapShared = onShared;

			// Create a *** own *** window and exchange the *** parent ***  window against it.
			base.InitializeOwnWindow ();

			InitializeToggleRessources ();
		}
		
		/// <summary> Initialize local ressources for all constructors. </summary>
		private void InitializeToggleRessources ()
		{
			_frameWidth = XrwTheme.InteractingFrameWidth;
			_frameType  = (TFrameTypeExt)XrwTheme.InteractingFrameType;

			_textColorPixel = X11lib.XAllocParsedColorByName (_display, _screenNumber, XrwTheme.TextColor);
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
			if (_offBitmap != null && _offBitmapShared == false)
			{
				_offBitmap.Dispose ();
				_offBitmap = null;
			}
			if (_onBitmap != null && _onBitmapShared == false)
			{
				_onBitmap.Dispose ();
				_onBitmap = null;
			}
			base.DisposeByParent ();
		}
		
		#endregion
		
        // ###############################################################################
        // ### P R O P E R T I E S
        // ###############################################################################

		#region Properties
		
		/// <summary> Get or set the label to display. </summary>
		public string Label
		{
			get { return _label;	}
			set { _label = value;	_lines = new Multiline (this, _label);  Redraw();	}
		}
		
		/// <summary> Get or set the horizontal text alignment (0.5F is centered). </summary>
		/// <param> Value from 0.0 (top align) to 1.0 (bottom align). <see cref="System.Single"/> </param>
		/// <remarks> Alignment is applied only if there is space left. </remarks>
		public float HorzTextAlign
		{
			get { return _horzTextAlign;	}
			set { _horzTextAlign = Math.Min (1.0F, Math.Max(0.0F, value));	}
		}
		
		/// <summary> Get or set the vertical text alignment (0.5F is centered). </summary>
		/// <param> Value from 0.0 (top align) to 1.0 (bottom align). <see cref="System.Single"/> </param>
		/// <remarks> Alignment is applied only if there is space left. </remarks>
		public float VertTextAlign
		{
			get { return _vertTextAlign;	}
			set { _vertTextAlign = Math.Min (1.0F, Math.Max(0.0F, value));	}
		}
		
		/// <summary> Get or set the left margin, the space (in pixels) that will be lef free between the left edge and the content. </summary>
		public int LeftMargin
		{	get	{	return _leftMargin;	}
			set {	_leftMargin = value;	}
		}
		
		/// <summary> Get or set the right margin, the space (in pixels) that will be lef free between the right edge and the content. </summary>
		public int RightMargin
		{	get	{	return _rightMargin;	}
			set {	_rightMargin = value;	}
		}
		
		/// <summary> Get off bitmap. </summary>
		public X11Graphic OffBitmap
		{
			get { return _offBitmap; }
		}
		
		/// <summary> Get of bitmap shared flag. </summary>
		public bool OffBitmapShared
		{
			get { return _offBitmapShared; }
		}
	
		/// <summary> Get on bitmap. </summary>
		public X11Graphic OnBitmap
		{
			get { return _onBitmap; }
		}
		
		/// <summary> Get on bitmap shared flag. </summary>
		public bool OnBitmapShared
		{
			get { return _onBitmapShared; }
		}
		
		/// <summary> Get or set the vertical left bitmap alignment (0.5F is centered). </summary>
		/// <param> Value from 0.0 (top align) to 1.0 (bottom align). <see cref="System.Single"/> </param>
		/// <remarks> Alignment is applied only if there is space left. </remarks>
		public float VertBitmapAlign
		{
			get { return _vertBitmapAlign;	}
			set { _vertBitmapAlign = Math.Min (1.0F, Math.Max(0.0F, value));	}
		}
		
		/// <summary> Get the focused indicator. </summary>
		public bool Focused
		{
			get { return _focused;	}
		}

		/// <summary> Get or set the pressed indicator. </summary>
		public bool Pressed
		{
			get { return _pressed;	}
			set { if (value != _pressed) { _pressed = value; Redraw(); }	}
		}

		#endregion
		
        // ###############################################################################
        // ### M E T H O D S
        // ###############################################################################

		#region XrwRectObj overrides
		
		/// <summary> Calculate the preferred size. </summary>
		/// <returns> The preferred size. <see cref="TSize"/> </returns>
		public override TSize PreferredSize ()
		{
			TSize textMeasure   = _lines.Measure(new TSize(0, _space * 2));
			TSize preferredSize = new TSize (Math.Max (MIN_WIDTH          + _borderWidth * 2 + _frameWidth * 2 + _leftMargin + _rightMargin,
			                                           textMeasure.Width  + _borderWidth * 2 + _frameWidth * 2 + _leftMargin + _rightMargin),
			                                 Math.Max (MIN_HEIGHT         + _borderWidth * 2 + _frameWidth * 2,
			                                           textMeasure.Height + _borderWidth * 2 + _frameWidth * 2));
			
			TSize bitmapMeasure = new TSize (0, 0);
			if (_offBitmap != null)
			{
				bitmapMeasure.Width  = _offBitmap.Width + _space;
				bitmapMeasure.Height = Math.Max (preferredSize.Height, _offBitmap.Height + _space * 2 + _borderWidth * 2 + _frameWidth * 2);
			}
			if (_onBitmap != null)
			{
				bitmapMeasure.Width  = Math.Max(bitmapMeasure.Width,  _onBitmap.Width + _space);
				bitmapMeasure.Height = Math.Max(bitmapMeasure.Height, Math.Max (preferredSize.Height, _onBitmap.Height + _space * 2 + _borderWidth * 2 + _frameWidth * 2));
			}
			preferredSize.Width += bitmapMeasure.Width;
			preferredSize.Height = Math.Max (preferredSize.Height, bitmapMeasure.Height);
			
			if (_fixedSize.Width >= 0 && _fixedSize.Width > MIN_WIDTH)
				preferredSize.Width = _fixedSize.Width + _borderWidth * 2 + _frameWidth * 2 + _leftMargin + _rightMargin;
			
			if (_fixedSize.Height >= 0 && _fixedSize.Height > MIN_HEIGHT)
				preferredSize.Height = _fixedSize.Height + _borderWidth * 2 + _frameWidth * 2;

			return preferredSize;
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
			
			RedrawContent ();
		}

		/// <summary> Redraw the widget. </summary>
		protected virtual void RedrawContent ()
		{
			if (_assignedSize.Width <  1 || _assignedSize.Height <  1)
				return;
			
			TRectangle clientArea = ClientArea();
			clientArea.X += _frameWidth;
			clientArea.Width -= _frameWidth * 2;
			clientArea.Y += _frameWidth;
			clientArea.Height -= _frameWidth * 2;
			
			// Content.
			X11lib.XSetForeground (_display, _gc, _textColorPixel);
			if (_pressed == false && _offBitmap != null)
			{
				int vertAlignOffset = (int)((clientArea.Height - _offBitmap.Height - _space * 2) * _vertBitmapAlign);
				_offBitmap.Draw (_window, _gc, (TInt)(clientArea.X + _leftMargin), (TInt)(clientArea.Y + _space + vertAlignOffset));
				
				clientArea.X      += _offBitmap.Width + _leftMargin + _space;
				clientArea.Width  -= _offBitmap.Width + _leftMargin + _space + _rightMargin;
			}
			else if (_pressed == true && _onBitmap != null)
			{
				int vertAlignOffset = (int)((clientArea.Height - _onBitmap.Height - _space * 2) * _vertBitmapAlign);
				_onBitmap.Draw (_window, _gc, (TInt)(clientArea.X + _leftMargin), (TInt)(clientArea.Y + _space + vertAlignOffset));
				
				clientArea.X      += _onBitmap.Width + _leftMargin + _space;
				clientArea.Width  -= _onBitmap.Width + _leftMargin + _space + _rightMargin;
			}
			else
			{
				clientArea.X      += _leftMargin;
				clientArea.Width  -= _leftMargin + _rightMargin;
			}
			
			clientArea.Y      += _space;
			clientArea.Height -= _space * 2;
			
			/* DEBUG START */
			// X11lib.XDrawLine (_display, _window, _gc, (TInt)clientArea.X, (TInt)clientArea.Y, (TInt)clientArea.Right, (TInt)clientArea.Bottom);
			/* DEBUG END */

			clientArea.X += 2; /* No idea why, but it must be done! */
			base.DrawTextLines (_lines, clientArea, _horzTextAlign, _vertTextAlign);
		}
		
        #endregion
		
		#region Methods
		
		/// <summary> Set off bitmap. </summary>
		/// <param name="offBitmap"> The off bitmap to display. <see cref="XrwBitmap"/> </param>
		/// <param name="offShared"> Indicate wether the offBitmap is shared (desposed by caller) or private (disposed together with tis label). <see cref="System.Boolean"/> </param>
		public void SetOffBitmap (X11Graphic offBitmap, bool offShared)
		{
			if (_offBitmap != null && _offBitmapShared == false)
			{
				_offBitmap.Dispose ();
				_offBitmap = null;
			}
			_offBitmap = offBitmap;
			_offBitmapShared = offShared;
		}
		
		/// <summary> Set on bitmap. </summary>
		/// <param name="onBitmap"> The on bitmap to display. <see cref="XrwBitmap"/> </param>
		/// <param name="onShared"> Indicate wether the onBitmap is shared (desposed by caller) or private (disposed together with tis label). <see cref="System.Boolean"/> </param>
		public void SetOnBitmap (X11Graphic onBitmap, bool onShared)
		{
			if (_onBitmap != null && _onBitmapShared == false)
			{
				_onBitmap.Dispose ();
				_onBitmap = null;
			}
			_onBitmap = onBitmap;
			_onBitmapShared = onShared;
		}

		#endregion
		
		#region Event handler
		
		/// <summary> Handle the StitchedOff event. </summary>
		public virtual void OnSwitchedOff ()
		{
			SwitchedOffDelegate switchedOff = SwitchedOff;
			if (switchedOff != null)
				switchedOff (this);
		}
		
		/// <summary> Handle the StitchedOn event. </summary>
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
			if (!(this is XrwToggle) && !(this is XrwRadio))
			{
				_pressed = false;
				OnSwitchedOff ();
			}

			if (XrwObject.SendExposeEvent (_display, _window, _window) == 0)
			{
				Console.WriteLine (CLASS_NAME + "::HandleFocusOut () ERROR. Can not send expose event.");
			}
			e.Result = 1;
		}

		/// <summary> Handle the ButtonPress event. </summary>
		/// <param name="source"> The widget, the ButtonPress event is assigned to. <see cref="XrwRectObj"/> </param>
		/// <param name="e"> The event data. <see cref="XrwButtonEvent"/> </param>
		/// <remarks> Set XrwButtonEvent. Set result to nonzero to stop further event processing. </remarks>
		protected void HandleButtonPressDefault (XrwRectObj source, XrwButtonEvent e)
		{
			if (!(this is XrwToggle))
			{
				_pressed = true;
				OnSwitchedOn ();
			}
			if (XrwObject.SendExposeEvent (_display, _window, _window) == 0)
			{
				Console.WriteLine (CLASS_NAME + "::HandleFocusOut () ERROR. Can not send expose event.");
			}
			e.Result = 1;
		}

		/// <summary> Handle the ButtonRelease event. </summary>
		/// <param name="source"> The widget, the ButtonRelease event is assigned to. <see cref="XrwRectObj"/> </param>
		/// <param name="e"> The event data. <see cref="XrwButtonEvent"/> </param>
		/// <remarks> Set XrwButtonEvent. Set result to nonzero to stop further event processing. </remarks>
		protected void HandleButtonReleaseDefault (XrwRectObj source, XrwButtonEvent e)
		{
			if (this is XrwRadio)
			{
				_pressed = true;
				OnSwitchedOn ();
			}
			else if (this is XrwToggle)
			{
				_pressed = !_pressed;
				if (_pressed == true)
					OnSwitchedOn ();
				else
					OnSwitchedOff ();
			}
			
			if (XrwObject.SendExposeEvent (_display, _window, _window) == 0)
			{
				Console.WriteLine (CLASS_NAME + "::HandleFocusOut () ERROR. Can not send expose event.");
			}
			e.Result = 1;
		}

		#endregion
			
	}
}

