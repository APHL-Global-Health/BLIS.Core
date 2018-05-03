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
	
	/// <summary> The windowless stack widget, holding multiple child widgets but show only one of them. </summary>
	/// <remarks> The member attribute Window contains the window pointer of the *** parent ***. </remarks>
	public class XrwStack : XrwConstraint
	{

        // ###############################################################################
        // ### C O N S T A N T S
        // ###############################################################################

        #region Constants

        /// <summary> The class name constant. </summary>
        public new const string	CLASS_NAME = "XrwStack";
		
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
		
		/// <summary> Hidden initializing constructor. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window for X11 calls. This widget has no X11 *** own *** window. <see cref="IntPtr"/> </param>
		public XrwStack (IntPtr display, X11.TInt screenNumber, IntPtr parentWindow)
			: base (display, screenNumber, parentWindow)
		{
			_borderColorPixel     = X11lib.XAllocParsedColorByName (_display, _screenNumber, XrwTheme.OuterBorderColor);
			_backgroundColorPixel = X11lib.XAllocParsedColorByName (_display, _screenNumber, XrwTheme.BackGroundColor);
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
		
		/// <summary> Get the currently visible child. </summary>
		/// <returns> The currently visible child, if any, or null otherwise. <see cref="System.Boolean"/> </returns>
		public XrwRectObj ShownChild
		{
			get
			{
				for (int count = 0; count < _children.Count; count++)
				{
					if (_children[count].Shown == true)
						return _children[count];
				}
				return null;
			}
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
			TSize preferredSize = new TSize (0, 0);
			
			for (int counter = 0; counter < _children.Count; counter++)
			{
				TSize childPreferredSize = _children[counter].PreferredSize ();
				if ((int)childPreferredSize.Width > preferredSize.Width)
					preferredSize.Width = childPreferredSize.Width;
				if ((int)childPreferredSize.Height > preferredSize.Height)
					preferredSize.Height = childPreferredSize.Height;
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
			int borderAndFrame = _borderWidth + _frameWidth;
			TPoint childPosition = new TPoint (assignedPosition.X + borderAndFrame,
			                                   assignedPosition.Y + borderAndFrame);

			for (int counter = 0; counter < _children.Count; counter++)
			{
				TSize preferredSize  = _children[counter].PreferredSize ();
				
				if (_children[counter].ExpandToAvailableWidth == true)
				{
					if (preferredSize.Width < availableSize.Width - borderAndFrame - borderAndFrame)
						preferredSize.Width = availableSize.Width - borderAndFrame - borderAndFrame;
				}
				if (_children[counter].ExpandToAvailableHeight == true)
				{
					if (preferredSize.Height < availableSize.Height - borderAndFrame - borderAndFrame)
						preferredSize.Height = availableSize.Height - borderAndFrame - borderAndFrame;
				}

				GeometryManagerAccess.SetAssignedGeometry (_children[counter], childPosition, preferredSize);
			}
		}
		
		/// <summary> Show the widget and map the window (if any) including all children. </summary>
		public override void Show()
		{
			_shown = true;
		}
		
		/// <summary> Hide the widget and unmap the window (if any) including all children. </summary>
		public override void Hide()
		{
			_shown = false;
		}
		
        #endregion

		#region XrwComposite overrides
		
		/// <summary> Add a widget to the list of children. </summary>
		/// <param name="child"> The widget to add. <see cref="XrwRectObj"/> </param>
		public override void AddChild (XrwRectObj child)
		{
			if (child == null)
				return;
			
			if (_children.Count == 0)
			{
				child.Show();
			}
			else if (child.Shown)
			{
				for (int count = 0; count < _children.Count; count++)
				{
					_children[count].Hide ();
				}
			}
			else
				child.Hide();
				
			_children.Add (child);
			child._parent = this;
		}
		
		#endregion
		
		#region Methods
		
		/// <summary> Show indicated child and hide formerly shown child. </summary>
		/// <param name="child"> The child to show. <see cref="XrwRectObj"/> </param>
		public void ShowChild (XrwRectObj child)
		{
			if (child == null)
				return;
			
			if (child == this.ShownChild)
				return;
			
			if (!_children.Contains (child))
				return;
			
			ShownChild.Hide();
			child.Show();
		}
		
        #endregion

	}
}

