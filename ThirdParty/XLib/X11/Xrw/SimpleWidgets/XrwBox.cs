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
	
	/// <summary> The windowless box widget, arranging the child widgets horizontally (HBox) or vertically (VBox). </summary>
	/// <remarks> The member attribute Window contains the window pointer of the *** parent ***. </remarks>
	public class XrwBox : XrwConstraint
	{

        // ###############################################################################
        // ### C O N S T A N T S
        // ###############################################################################

        #region Constants

        /// <summary> The class name constant. </summary>
        public new const string	CLASS_NAME = "XrwBox";
		
        #endregion

		// ###############################################################################
        // ### A T T R I B U T E S
        // ###############################################################################

		#region Attributes
		
		/// <summary> The orientation of the children. </summary>
		/// <remarks> TOrientation.Horizontal acts as a HBox, TOrientation.Vertical acts as a VBox. </remarks>
		protected TOrientation	_orientation = TOrientation.Horizontal;
		
		/// <summary> The child alignment (0.5F is centered). </summary>
		/// <remarks> Alignment is applied only if there is space left.
		/// XrmRectObj.ExpandToAvailableWidth has higher priority than alignment and might not leave any space left. </remarks>
		private float			_childAlign	= 0.5F;
			
        #endregion		
		
        // ###############################################################################
        // ### C O N S T R U C T I O N   A N D   I N I T I A L I Z A T I O N
        // ###############################################################################

        #region Construction
		
		/// <summary> Hidden initializing constructor. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window for X11 calls. This widget has no X11 *** own *** window. <see cref="IntPtr"/> </param>
		protected XrwBox (IntPtr display, X11.TInt screenNumber, IntPtr parentWindow)
			: base (display, screenNumber, parentWindow)
		{
			_expandToAvailableWidth = true;

			_borderColorPixel     = X11lib.XAllocParsedColorByName (_display, _screenNumber, XrwTheme.OuterBorderColor);
			_backgroundColorPixel = X11lib.XAllocParsedColorByName (_display, _screenNumber, XrwTheme.BackGroundColor);
		}
		
		/// <summary> Static constructor for a horizontally oriented box. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window for X11 calls. This widget has no X11 *** own *** window. <see cref="IntPtr"/> </param>
		public static XrwBox NewHBox (IntPtr display, X11.TInt screenNumber, IntPtr parentWindow)
		{
			XrwBox b = new XrwBox (display, screenNumber, parentWindow);
			return b;
		}
		
		/// <summary> Static constructor for a vertically oriented box. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window for X11 calls. This widget has no X11 *** own *** window. <see cref="IntPtr"/> </param>
		public static XrwBox NewVBox (IntPtr display, X11.TInt screenNumber, IntPtr parentWindow)
		{
			XrwBox b = new XrwBox (display, screenNumber, parentWindow);
			b._orientation = TOrientation.Vertical;
			return b;
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
		
		/// <summary> Get or set the child alignment (0.5F is centered). </summary>
		/// <param> Value from 0.0 (left align) to 1.0 (right align). <see cref="System.Single"/> </param>
		/// <remarks> Alignment is applied only if there is space left.
		/// XrmRectObj.ExpandToAvailableWidth has higher priority than alignment and might not leave any space left. </remarks>
		public float ChildAlign
		{
			get	{	return _childAlign;	}
			set	{	_childAlign = Math.Min (1.0F, Math.Max (0.0F, value));	}
		}
		
		/// <summary> Get the orientation. </summary>
		public TOrientation Orientation
		{	get {	return _orientation;	}
		}
			
        #endregion
		
        // ###############################################################################
        // ### M E T H O D S
        // ###############################################################################

		#region XrwRectObj overrides
		
		/// <summary> Calculate the preferred size. </summary>
		/// <returns> The preferred size. <see cref="TIntSize"/> </returns>
		public override TSize PreferredSize ()
		{
			if (_orientation == TOrientation.Horizontal)
				return PreferredSizeHBox();
			else
				return PreferredSizeVBox();
		}
		
		/// <summary> Calculate the preferred size acting like a HBox. </summary>
		/// <returns> The preferred size. <see cref="TIntSize"/> </returns>
		private TSize PreferredSizeHBox ()
		{
			TSize preferredSize = new TSize (0, 0);
			
			for (int counter = 0; counter < _children.Count; counter++)
			{
				TSize childPreferredSize = _children[counter].PreferredSize ();
				if ((int)childPreferredSize.Width > 0)
					preferredSize.Width = preferredSize.Width + childPreferredSize.Width;
				if ((int)childPreferredSize.Height > 0)
					preferredSize.Height = Math.Max (preferredSize.Height, childPreferredSize.Height);
				
				if (counter > 0)
					preferredSize.Width = preferredSize.Width + _horzSpacing;
			}
			preferredSize.Width  = preferredSize.Width  + _borderWidth * 2 + _frameWidth * 2;
			preferredSize.Height = preferredSize.Height + _borderWidth * 2 + _frameWidth * 2;
			
			return preferredSize;
		}
		
		/// <summary> Calculate the preferred size acting like a VBox. </summary>
		/// <returns> The preferred size. <see cref="TIntSize"/> </returns>
		private TSize PreferredSizeVBox ()
		{
			TSize preferredSize = new TSize (0, 0);
			
			for (int counter = 0; counter < _children.Count; counter++)
			{
				TSize childPreferredSize = _children[counter].PreferredSize ();
				if ((int)childPreferredSize.Width > 0)
					preferredSize.Width = Math.Max (preferredSize.Width, childPreferredSize.Width);
				if ((int)childPreferredSize.Height > 0)
					preferredSize.Height = preferredSize.Height + childPreferredSize.Height;
				
				if (counter > 0)
					preferredSize.Height = preferredSize.Height + _vertSpacing;
			}
			preferredSize.Width  = preferredSize.Width  + _borderWidth * 2 + _frameWidth * 2;
			preferredSize.Height = preferredSize.Height + _borderWidth * 2 + _frameWidth * 2;
			
			return preferredSize;
		}
		
		/// <summary> Calcualate layout after a position or size change. </summary>
		/// <param name="assignedPosition"> The position of the top left top corner assigned by the window manager (for shell widgets) or geometry management (by non-shell widgets). <see cref="TPoint"/> </param>
		/// <param name="availableSize"> The available size. <see cref="TIntSize"/> </param>
		/// <remarks> This method is defined at XrwRectObj for compatibility, but must be implemented for composite widgets only. </remarks>
		public override void CalculateChildLayout (TPoint assignedPosition, TSize availableSize)
		{
			if (_orientation == TOrientation.Horizontal)
				CalculateChildLayoutHBox (assignedPosition, availableSize);
			else
				CalculateChildLayoutVBox (assignedPosition, availableSize);
		}
		
		/// <summary> Calcualate layout after a position or size change acting like a HBox. </summary>
		/// <param name="assignedPosition"> The position of the top left top corner assigned by the window manager (for shell widgets) or geometry management (by non-shell widgets). <see cref="TPoint"/> </param>
		/// <param name="availableSize"> The available size. <see cref="TIntSize"/> </param>
		/// <remarks> This method is defined at XrwRectObj for compatibility, but must be implemented for composite widgets only. </remarks>
		private void CalculateChildLayoutHBox (TPoint assignedPosition, TSize availableSize)
		{
			int borderAndFrame = _borderWidth + _frameWidth;
			TPoint childPosition = new TPoint (assignedPosition.X + borderAndFrame,
			                                   assignedPosition.Y + borderAndFrame);
			List<ChildData> childData = new List<ChildData>();
			int countMaxSiblingWidthExpand = 0;
			int countAvailableWidthExpand  = 0;
			int childrenPreferredWidth     = 0;
			int maxSiblingWidth            = 0;
			int unexpandedMaxSiblingWidth  = 0;

			for (int counter = 0; counter < _children.Count; counter++)
			{
				TSize preferredSize  = _children[counter].PreferredSize ();
				childData.Add (new ChildData (_children[counter], preferredSize));
				
				if (_children[counter].ExpandToMaxSiblingWidth == true)
				{
					countMaxSiblingWidthExpand++;
					unexpandedMaxSiblingWidth += preferredSize.Width;
				}
				if (_children[counter].ExpandToAvailableWidth == true)
					countAvailableWidthExpand++;
				
				childrenPreferredWidth += preferredSize.Width;
				if (counter > 0)
					childrenPreferredWidth += _horzSpacing;
				
				if (maxSiblingWidth < preferredSize.Width)
					maxSiblingWidth = preferredSize.Width;
			}

			int widthForAvailableWidthExpand  = (availableSize.Width > childrenPreferredWidth && countAvailableWidthExpand > 0 ?
			                                     (availableSize.Width - childrenPreferredWidth) / countAvailableWidthExpand - _frameWidth - _frameWidth :
			                                     (availableSize.Width - childrenPreferredWidth) - _frameWidth - _frameWidth);
			int widthForMaxSiblingWidthExpand = Math.Min (widthForAvailableWidthExpand, maxSiblingWidth * countMaxSiblingWidthExpand - unexpandedMaxSiblingWidth);
			widthForAvailableWidthExpand -= widthForMaxSiblingWidthExpand;
			
			if (countAvailableWidthExpand == 0 && widthForAvailableWidthExpand > 0)
			{
				childPosition.X = childPosition.X + (int)(widthForAvailableWidthExpand * _childAlign);
			}
			
			for (int counter = 0; counter < childData.Count; counter++)
			{
				ChildData cd = childData[counter];
				
				if (cd.Widget.ExpandToMaxSiblingWidth == true)
				{
					int expandBy = Math.Min (widthForMaxSiblingWidthExpand, maxSiblingWidth - cd.Size.Width);
					cd.Size.Width   += expandBy;
					widthForMaxSiblingWidthExpand -= expandBy;
				}
				if (cd.Widget.ExpandToAvailableWidth == true)
				{
					int expandBy = (int)(widthForAvailableWidthExpand / countAvailableWidthExpand);
					cd.Size.Width += expandBy;
					widthForAvailableWidthExpand -= expandBy;
					countAvailableWidthExpand--;
				}
				if (cd.Widget.ExpandToAvailableHeight == true)
				{
					if (cd.Size.Height < availableSize.Height - borderAndFrame - borderAndFrame)
						cd.Size.Height = availableSize.Height - borderAndFrame - borderAndFrame;
				}

				GeometryManagerAccess.SetAssignedGeometry (cd.Widget, childPosition, cd.Size);
				childPosition.X = childPosition.X + cd.Size.Width + _horzSpacing;
			}
		}
		
		/// <summary> Calcualate layout after a position or size change acting like a VBox. </summary>
		/// <param name="assignedPosition"> The position of the top left top corner assigned by the window manager (for shell widgets) or geometry management (by non-shell widgets). <see cref="TPoint"/> </param>
		/// <param name="availableSize"> The available size. <see cref="TIntSize"/> </param>
		/// <remarks> This method is defined at XrwRectObj for compatibility, but must be implemented for composite widgets only. </remarks>
		private void CalculateChildLayoutVBox (TPoint assignedPosition, TSize availableSize)
		{
			int borderAndFrame = _borderWidth + _frameWidth;
			TPoint childPosition = new TPoint (assignedPosition.X + borderAndFrame,
			                                   assignedPosition.Y + borderAndFrame);
			
			List<ChildData> childData = new List<ChildData>();
			int countMaxSiblingHeightExpand = 0;
			int countAvailableHeightExpand  = 0;
			int childrenPreferredHeight     = 0;
			int maxSiblingHeight            = 0;
			int unexpandedMaxSiblingHeight  = 0;

			for (int counter = 0; counter < _children.Count; counter++)
			{
				TSize preferredSize  = _children[counter].PreferredSize ();
				childData.Add (new ChildData (_children[counter], preferredSize));
				
				if (_children[counter].ExpandToMaxSiblingHeight == true)
				{
					countMaxSiblingHeightExpand++;
					unexpandedMaxSiblingHeight += preferredSize.Height;
				}
				if (_children[counter].ExpandToAvailableHeight == true)
					countAvailableHeightExpand++;
				
				childrenPreferredHeight += preferredSize.Height;
				if (counter > 0)
					childrenPreferredHeight += _vertSpacing;
				
				if (maxSiblingHeight < preferredSize.Height)
					maxSiblingHeight = preferredSize.Height;
			}
			
			int heightForAvailableHeightExpand  = (availableSize.Height > childrenPreferredHeight && countAvailableHeightExpand > 0 ?
			                                       (availableSize.Height - childrenPreferredHeight) / countAvailableHeightExpand - _frameWidth - _frameWidth :
			                                       (availableSize.Height - childrenPreferredHeight) - _frameWidth - _frameWidth);
			int heightForMaxSiblingHeightExpand = Math.Min (heightForAvailableHeightExpand, maxSiblingHeight * countMaxSiblingHeightExpand - unexpandedMaxSiblingHeight);
			heightForAvailableHeightExpand -= heightForMaxSiblingHeightExpand;
			
			if (countAvailableHeightExpand == 0 && heightForAvailableHeightExpand > 0)
			{
				childPosition.Y = childPosition.Y + (int)(heightForAvailableHeightExpand * _childAlign);
			}
			
			for (int counter = 0; counter < childData.Count; counter++)
			{
				ChildData cd = childData[counter];
				
				if (cd.Widget.ExpandToMaxSiblingHeight == true)
				{
					int expandBy = Math.Min (heightForMaxSiblingHeightExpand, maxSiblingHeight - cd.Size.Height);
					cd.Size.Height   += expandBy;
					heightForMaxSiblingHeightExpand -= expandBy;
				}
				if (cd.Widget.ExpandToAvailableHeight == true)
				{
					int expandBy = (int)(heightForAvailableHeightExpand / countAvailableHeightExpand);
					cd.Size.Height += expandBy;
					heightForAvailableHeightExpand -= expandBy;
					countAvailableHeightExpand--;
				}
				if (cd.Widget.ExpandToAvailableWidth == true)
				{
					if (cd.Size.Width < availableSize.Width - borderAndFrame - borderAndFrame)
						cd.Size.Width = availableSize.Width - borderAndFrame - borderAndFrame;
				}

				GeometryManagerAccess.SetAssignedGeometry (cd.Widget, childPosition, cd.Size);
				childPosition.Y = childPosition.Y + cd.Size.Height + _vertSpacing;
			}
		}
		
        #endregion

		#region XrwComposite overrides
		
        #endregion
		
		#region Methods
		
        #endregion
		
	}

}