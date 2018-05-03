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
	
	/// <summary> The windowless notebook widget, arranging multiple pages containing one child on each page. </summary>
	/// <remarks> The member attribute Window contains the window pointer of the *** parent ***. </remarks>
	public class XrwNotebook : XrwBox
	{

        // ###############################################################################
        // ### C O N S T A N T S
        // ###############################################################################

        #region Constants

        /// <summary> The class name constant. </summary>
        public new const string		CLASS_NAME = "XrwNotebook";
	
        #endregion

		// ###############################################################################
        // ### A T T R I B U T E S
        // ###############################################################################

		#region Attributes
		
		/// <summary> The box widget, that contains the tabs. </summary>
		private XrwRadioBox			_tabBox		= null;
		
		/// <summary> The box widget, that contains the pages. </summary>
		private XrwStack			_pageStack	= null;
		
        #endregion

		// ###############################################################################
        // ### E V E N T S
        // ###############################################################################

		#region Events
		
		/// <summary> Register selection changed event handler. </summary>
		public event SelectionChangedDelegate SelectionChanged;
		
        #endregion
		
        // ###############################################################################
        // ### C O N S T R U C T I O N   A N D   I N I T I A L I Z A T I O N
        // ###############################################################################

        #region Construction

		/// <summary> Hidden initializing constructor. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window for X11 calls. This widget has no X11 *** own *** window. <see cref="IntPtr"/> </param>
		protected XrwNotebook (IntPtr display, X11.TInt screenNumber, IntPtr parentWindow)
			: base (display, screenNumber, parentWindow)
		{
		}
		
		/// <summary> Static constructor for a horizontally oriented radio box. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window for X11 calls. This widget has no X11 *** own *** window. <see cref="IntPtr"/> </param>
		public static XrwNotebook NewTopTabedNotebook (IntPtr display, X11.TInt screenNumber, IntPtr parentWindow)
		{
			XrwNotebook b = new XrwNotebook (display, screenNumber, parentWindow);
			b._orientation = TOrientation.Vertical;
			b.ExpandToAvailableHeight = false;
			b.ExpandToAvailableWidth  = true;
			b.VertSpacing = 0;

			b._tabBox  = XrwRadioBox.NewHRadioBox (display, screenNumber, parentWindow);
			b._tabBox.BorderWidth = 0;
			b._tabBox.ChildAlign = 0.0F;
			b._tabBox.FrameType = TFrameType.None;
			b._tabBox.FrameWidth = XrwTheme.NonInteractingFrameWidth;
			b._tabBox.HorzSpacing = 0;
			b._tabBox.VertSpacing = 0;
			b.BaseAddChild (b._tabBox);

			XrwSimple tail = new XrwSimple (display, screenNumber, parentWindow);
			tail.ExpandToAvailableWidth = true;
			tail.ExpandToAvailableHeight = true;
			tail.FrameWidth   = 2;
			tail.FrameTypeExt = TFrameTypeExt.UnsunkenTopTabTail;
			b._tabBox.AddChild (tail);
			
			b._pageStack = new XrwStack (display, screenNumber, parentWindow);
			b._pageStack.BorderWidth = 0;
			b._pageStack.FrameTypeExt = Xrw.TFrameTypeExt.RaisedBottomTab;
			b._pageStack.FrameWidth = 2;
			b._pageStack.VertSpacing = 0;
			b._pageStack.ExpandToAvailableWidth = true;
			b.BaseAddChild (b._pageStack);
			
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
			_tabBox.DisposeByParent ();
			_pageStack.DisposeByParent ();
			
			base.DisposeByParent ();
		}
		
		#endregion
		
        // ###############################################################################
        // ### P R O P E R T I E S
        // ###############################################################################

		#region Properties
		
		/// <summary> Get the number of pages. </summary>
		public int CountPages
		{	get { return _pageStack.Children.Count;	}
		}
		
        #endregion
		
        // ###############################################################################
        // ### M E T H O D S
        // ###############################################################################

		#region XrwRectObj overrides
		
		/// <summary> Show the widget and map the window (if any) including all children. </summary>
		public override void Show()
		{
			_shown = true;
			
			foreach (XrwRectObj child in _children)
			{
				child.Show();
			}
		}
		
		/// <summary> Hide the widget and unmap the window (if any) including all children. </summary>
		public override void Hide ()
		{
			_shown = false;
			
			foreach (XrwRectObj child in _children)
			{
				child.Hide();
			}
		}

        #endregion

		#region XrwComposite overrides
		
		/// <summary> Provide original base class method AddChild(). </summary>
		/// <param name="child"> The widget to add. <see cref="XrwRectObj"/> </param>
		protected void BaseAddChild (XrwRectObj child)
		{
			base.AddChild (child);
		}
			
		/// <summary> Add a widget, acting as page, to the list of children. </summary>
		/// <param name="child"> The widget to add. <see cref="XrwRectObj"/> </param>
		public override void AddChild (XrwRectObj child)
		{
			if (child == null)
			    return;
			
			XrwRadio tab = new XrwRadio (_display, _screenNumber, _window, "__" + (_tabBox.Children.Count).ToString() + "__", null, false, null, false);
			_tabBox.InsertChild (_tabBox.Children.Count - 1, tab);
			if (_tabBox.Children.Count == 2)
			{
				tab.Pressed = true;
			}
			tab.SwitchedOn += HandleSelectionChange;
			
			for (int count = 0; count < _tabBox.Children.Count; count++)
			{
				if ((_tabBox.Children[count] is XrwVisibleRectObj) && count <  _tabBox.Children.Count - 1)
					(_tabBox.Children[count] as XrwVisibleRectObj).FrameTypeExt = TFrameTypeExt.RaisedTopTab;
				if ((_tabBox.Children[count] is XrwVisibleRectObj) && count == _tabBox.Children.Count - 1)
					(_tabBox.Children[count] as XrwVisibleRectObj).FrameTypeExt = TFrameTypeExt.UnsunkenTopTabTail;
			}
			_pageStack.AddChild (child);
		}
		
        #endregion

		#region Methods
		
		/// <summary> Get the indicated page widget. </summary>
		/// <param name="index"> The index of the requested page widget. <see cref="System.Int32"/> </param>
		/// <returns> The requested page widget. <see cref="XrwRectObj"/> </returns>
		/// <remarks> Throws IndexOutOfRangeException. </remarks>
		public XrwRectObj PageWidget (int index)
		{
			if (index < 0 && index >= _pageStack.Children.Count)
				throw new IndexOutOfRangeException ();
			else
				return _pageStack.Children[index];
		}
		
		/// <summary> Get the indicated tab widget. </summary>
		/// <param name="index"> The index of the requested tab widget. <see cref="System.Int32"/> </param>
		/// <returns> The requested tab widget. <see cref="XrwRectObj"/> </returns>
		/// <remarks> Throws IndexOutOfRangeException. </remarks>
		public XrwRectObj TabWidget  (int index)
		{
			if (index < 0 && index >= _tabBox.Children.Count - 1)
				throw new IndexOutOfRangeException ();
			else
				return _tabBox.Children[index];
		}
		
		#endregion
		
		#region Event handler
		
		/// <summary> Handle the switched off event. </summary>
		/// <param name="selected"> The child widget, the SelectionChange event sets visible. <see cref="XrwRectObj"/> </param>
		public virtual void OnSelectionChanged (XrwRectObj selected)
		{
			SelectionChangedDelegate selectionChanged = SelectionChanged;
			if (selectionChanged != null)
				selectionChanged (this, selected);
		}

		/// <summary> Handle the SelectionChange event. </summary>
		/// <param name="selected"> The child widget, the SelectionChange event sets visible. <see cref="XrwRectObj"/> </param>
		protected void HandleSelectionChange (XrwRectObj selected)
		{
			int index = 0;
			for (int count = 0; count < _tabBox.Children.Count; count++)
			{
				if (_tabBox.Children[count] == selected)
					break;
				index++;
			}
			
			if (_tabBox.Children[index] == selected)
			{
				if (index <= _pageStack.Children.Count)
				{
					_pageStack.ShowChild (_pageStack.Children[index]);
					//_pageStack.CalculateChildLayout (_pageStack.AssignedPosition, _pageStack.AssignedSize);
					
					OnSelectionChanged (selected);
				}
			}
		}
		
        #endregion

	}
}

