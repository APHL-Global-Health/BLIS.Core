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
	/// <summary> The windowless static label widget, that can display a left bitmap and a multilinr label. </summary>
	/// <remarks> The member attribute Window contains the window pointer of the *** parent ***. </remarks>
	/// <remarks> Different to Athena widget set the label is inherited from core and not from simple - so it uses it's parent window. </remarks>
	public class XrwLabel : XrwCore
	{

        // ###############################################################################
        // ### C O N S T A N T S
        // ###############################################################################

        #region Constants

        /// <summary> The class name constant. </summary>
        public new const string	CLASS_NAME = "XrwLabel";
		
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
		
		/// <summary> Spacing around the label. Used from left bitmap to label (if != null),
		/// from label to right bitmap (if != null) and from label to top/bottom. </summary>
		public const int		_space = 4;
		
		/// <summary> The horizontal text alignment (0.5F is centered). </summary>
		protected float			_horzTextAlign			= 0.0F;
		
		/// <summary> The vertical text alignment (0.5F is centered). </summary>
		protected float			_vertTextAlign			= 0.5F;
		
		/// <summary> The space (in pixels) that will be lef free between the left edge and the content. </summary>
		protected int			_leftMargin				= 4;
		
		/// <summary> The space (in pixels) that will be lef free between the right edge and the content. </summary>
		protected int			_rightMargin			= 4;
		
		/// <summary> The image of the left bitmap. </summary>
		protected X11Graphic	_leftBitmap				= null;
		
		/// <summary> Indicate wether the leftBitmap is shared (desposed by caller) or private (disposed together with tis label). </summary>
		protected bool			_leftBitmapShared		= false;
		
		/// <summary> The left bitmap vertical alignment (0.5F is centered). </summary>
		protected float			_leftBitmapVertAlign	= 0.5F;
		
		/// <summary> The image of the right bitmap. </summary>
		protected X11Graphic	_rightBitmap			= null;
		
		/// <summary> Indicate wether the rightBitmap is shared (desposed by caller) or private (disposed together with tis label). </summary>
		protected bool			_rightBitmapShared		= false;
		
		/// <summary> The right bitmap vertical alignment (0.5F is centered). </summary>
		protected float			_rightBitmapVertAlign	= 0.5F;
		
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
		public XrwLabel (IntPtr display, X11.TInt screenNumber, IntPtr parentWindow, string label)
			: base (display, screenNumber, parentWindow)
		{	
			_label = label;
			_lines = new Multiline (this, _label);

			InitializeLabelRessources ();
		}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window for X11 calls. This widget has no X11 *** own *** window. <see cref="System.IntPtr"/> </param>
		/// <param name="fixedSize"> The fixed size, that ist to use (if set) rather than the calcualated size. Passed as reference to avoid structure copy constructor calls. <see cref="TIntSize"/> </param>
		/// <param name="label"> The label to display. <see cref="System.String"/> </param>
		public XrwLabel (IntPtr display, X11.TInt screen, IntPtr parentWindow, ref TSize fixedSize, string label)
			: base (display, screen, parentWindow, ref fixedSize)
		{
			_label = label;
			_lines = new Multiline (this, _label);

			InitializeLabelRessources ();
		}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window for X11 calls. This widget has no X11 *** own *** window. <see cref="System.IntPtr"/> </param>
		/// <param name="assignedPosition"> The position of the top left top corner assigned by the window manager (for shell widgets) or geometry management (by non-shell widgets). Passed as reference to avoid structure copy constructor calls. <see cref="TPoint"/> </param>
		/// <param name="fixedSize"> The fixed size, that ist to use (if set) rather than the calcualated size. Passed as reference to avoid structure copy constructor calls. <see cref="TIntSize"/> </param>
		/// <param name="label"> The label to display. <see cref="System.String"/> </param>
		public XrwLabel (IntPtr display, X11.TInt screenNumber, IntPtr parentWindow, ref TPoint assignedPosition, ref TSize fixedSize, string label)
			: base (display, screenNumber, parentWindow, ref assignedPosition, ref fixedSize)
		{	
			_label = label;
			_lines = new Multiline (this, _label);

			InitializeLabelRessources ();
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
		public XrwLabel (IntPtr display, X11.TInt screenNumber, IntPtr parentWindow, string label, X11Graphic leftBitmap, bool leftShared, X11Graphic rightBitmap, bool rightShared)
			: base (display, screenNumber, parentWindow)
		{	
			_label = label;
			_lines = new Multiline (this, _label);
			_leftBitmap = leftBitmap;
			_leftBitmapShared = leftShared;
			_rightBitmap = rightBitmap;
			_rightBitmapShared = rightShared;
			
			InitializeLabelRessources ();
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
		public XrwLabel (IntPtr display, X11.TInt screen, IntPtr parentWindow, ref TSize fixedSize, string label, X11Graphic leftBitmap, bool leftShared, X11Graphic rightBitmap, bool rightShared)
			: base (display, screen, parentWindow, ref fixedSize)
		{
			_label = label;
			_lines = new Multiline (this, _label);
			_leftBitmap = leftBitmap;
			_leftBitmapShared = leftShared;
			_rightBitmap = rightBitmap;
			_rightBitmapShared = rightShared;
			
			InitializeLabelRessources ();
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
		public XrwLabel (IntPtr display, X11.TInt screenNumber, IntPtr parentWindow, ref TPoint assignedPosition, ref TSize fixedSize, string label, X11Graphic leftBitmap, bool leftShared, X11Graphic rightBitmap, bool rightShared)
			: base (display, screenNumber, parentWindow, ref assignedPosition, ref fixedSize)
		{	
			_label = label;
			_lines = new Multiline (this, _label);
			_leftBitmap = leftBitmap;
			_leftBitmapShared = leftShared;
			_rightBitmap = rightBitmap;
			_rightBitmapShared = rightShared;

			InitializeLabelRessources ();
		}
		
		/// <summary> Initialize local ressources for all constructors. </summary>
		private void InitializeLabelRessources ()
		{
			_textColorPixel = X11lib.XAllocParsedColorByName (_display, _screenNumber, XrwTheme.TextColor);
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
			if (_leftBitmap != null && _leftBitmapShared == false)
			{
				_leftBitmap.Dispose ();
				_leftBitmap = null;
			}
			if (_rightBitmap != null && _rightBitmapShared == false)
			{
				_rightBitmap.Dispose ();
				_rightBitmap = null;
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
		
		/// <summary> Get or set the space (in pixels) that will be lef free between the left edge and the content. </summary>
		public int LeftMargin
		{	get	{	return _leftMargin;	}
			set {	_leftMargin = value;	}
		}
		
		/// <summary> Get or set space (in pixels) that will be lef free between the right edge and the content. </summary>
		public int RightMargin
		{	get	{	return _rightMargin;	}
			set {	_rightMargin = value;	}
		}
	
		/// <summary> Get left bitmap. </summary>
		public X11Graphic LeftBitmap
		{
			get { return _leftBitmap; }
		}
		
		/// <summary> Get left bitmap shared flag. </summary>
		public bool LeftBitmapShared
		{
			get { return _leftBitmapShared; }
		}
		
		/// <summary> Get or set the vertical left bitmap alignment (0.5F is centered). </summary>
		/// <param> Value from 0.0 (top align) to 1.0 (bottom align). <see cref="System.Single"/> </param>
		/// <remarks> Alignment is applied only if there is space left. </remarks>
		public float VertLeftBitmapAlign
		{
			get { return _leftBitmapVertAlign;	}
			set { _leftBitmapVertAlign = Math.Min (1.0F, Math.Max(0.0F, value));	}
		}
		
		/// <summary> Get right bitmap. </summary>
		public X11Graphic RightBitmap
		{
			get { return _rightBitmap; }
		}
		
		/// <summary> Get right bitmap shared flag. </summary>
		public bool RightBitmapShared
		{
			get { return _rightBitmapShared; }
		}
		
		/// <summary> Get or set the vertical right bitmap alignment (0.5F is centered). </summary>
		/// <param> Value from 0.0 (top align) to 1.0 (bottom align). <see cref="System.Single"/> </param>
		/// <remarks> Alignment is applied only if there is space left. </remarks>
		public float VertRightBitmapAlign
		{
			get { return _rightBitmapVertAlign;	}
			set { _rightBitmapVertAlign = Math.Min (1.0F, Math.Max(0.0F, value));	}
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
			
			if (_leftBitmap != null)
			{
				preferredSize.Width += _leftBitmap.Width + _space;
				preferredSize.Height = Math.Max (preferredSize.Height, _leftBitmap.Height + _space * 2 + _borderWidth * 2 + _frameWidth * 2);
			}
			
			if (_rightBitmap != null)
			{
				preferredSize.Width += _rightBitmap.Width + _space;
				preferredSize.Height = Math.Max (preferredSize.Height, _rightBitmap.Height + _space * 2 + _borderWidth * 2 + _frameWidth * 2);
			}
			
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
			// -- (windowless)
			
			// Outer border.
			if ((int)_borderWidth > 0)
			{
				X11lib.XSetForeground (_display, _gc, _borderColorPixel);
				base.DrawBorder (_display, _window, _gc, completeArea, _borderWidth);
			}

			TRectangle clientArea = ClientArea();
			
			// Frame.
			if (_frameType != TFrameTypeExt.None)
				base.DrawFrame (_display, _window, _gc, clientArea, _frameType, _darkShadowColorPixel, _lightShadowColorPixel);
			
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
			if (_leftBitmap != null)
			{
				int vertAlignOffset = (int)((clientArea.Height - _leftBitmap.Height - _space * 2) * _leftBitmapVertAlign);
				_leftBitmap.Draw (_window, _gc, (TInt)(clientArea.X + _leftMargin), (TInt)(clientArea.Y + _space + vertAlignOffset));
				
				clientArea.X      += _leftBitmap.Width + _leftMargin + _space;
				clientArea.Width  -= _leftBitmap.Width + _leftMargin + _space;
			}
			else
			{
				clientArea.X      += _leftMargin;
				clientArea.Width  -= _leftMargin;
			}
			
			if (_rightBitmap != null)
			{
				int vertAlignOffset = (int)((clientArea.Height - _rightBitmap.Height - _space * 2) * _rightBitmapVertAlign);
				_rightBitmap.Draw (_window, _gc, (TInt)(clientArea.Right - _rightBitmap.Width - _rightMargin), (TInt)(clientArea.Y + _space + vertAlignOffset));
				
				clientArea.Width  -= _rightBitmap.Width + _space + _rightMargin;
			}
			else
			{
				clientArea.Width  -= _rightMargin;
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
		
		/// <summary> Set left bitmap and bitmap shared flag. </summary>
		/// <param name="leftBitmap"> The left bitmap to display. <see cref="XrwBitmap"/> </param>
		/// <param name="leftShared"> Indicate wether the leftBitmap is shared (desposed by caller) or private (disposed together with tis label). <see cref="System.Boolean"/> </param>
		public void SetLeftBitmap (X11Graphic leftBitmap, bool leftShared)
		{
			if (_leftBitmap != null && _leftBitmapShared == false)
			{
				_leftBitmap.Dispose ();
				_leftBitmap = null;
			}
			_leftBitmap = leftBitmap;
			_leftBitmapShared = leftShared;
		}
		
		/// <summary> Set right bitmap and bitmap shared flag. </summary>
		/// <param name="rightBitmap"> The right bitmap to display. <see cref="XrwBitmap"/> </param>
		/// <param name="rightShared"> Indicate wether the rightBitmap is shared (desposed by caller) or private (disposed together with tis label). <see cref="System.Boolean"/> </param>
		public void SetRightBitmap (X11Graphic rightBitmap, bool rightShared)
		{
			if (_rightBitmap != null && _rightBitmapShared == false)
			{
				_rightBitmap.Dispose ();
				_rightBitmap = null;
			}
			_rightBitmap = rightBitmap;
			_rightBitmapShared = rightShared;
		}

		#endregion
			
	}
}

