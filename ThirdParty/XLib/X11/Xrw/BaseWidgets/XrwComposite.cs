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
	
	/// <summary> The windowless base class for composite widgets, managing an arbitary number of child widgets. </summary>
	/// <remarks> The member attribute Window contains the window pointer of the *** parent ***. </remarks>
	public class XrwComposite : XrwCore
	{
		
        // ###############################################################################
        // ### I N N E R   C L A S S E S
        // ###############################################################################

		#region Inner classes
		
		/// <summary> Helper class for size calculation *** in one run ** (without doble call of PreferredSize ()). </summary>
		protected struct ChildData
		{
			public XrwRectObj	Widget;
			
			public TSize		Size;
			
			public ChildData (XrwRectObj widget, TSize size)
			{
				Widget = widget;
				Size = size;
			}
		}
		
		#endregion

        // ###############################################################################
        // ### C O N S T A N T S
        // ###############################################################################

        #region Constants

        /// <summary> The class name constant. </summary>
        public new const string		CLASS_NAME = "XrwCmposite";
		
        #endregion

		// ###############################################################################
        // ### A T T R I B U T E S
        // ###############################################################################

		#region Attributes
		
		/// <summary> The list of registered child widgets. </summary>
		protected List<XrwRectObj>	_children					= new List<XrwRectObj> ();
		
        #endregion
		
        // ###############################################################################
        // ### C O N S T R U C T I O N   A N D   I N I T I A L I Z A T I O N
        // ###############################################################################

        #region Construction
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window for X11 calls. This widget has no X11 *** own *** window. <see cref="IntPtr"/> </param>
		public XrwComposite (IntPtr display, X11.TInt screenNumber, IntPtr parentWindow)
			: base (display, screenNumber, parentWindow)
		{	
			_borderWidth = XrwTheme.CompositeBorderWidth;
		}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window for X11 calls. This widget has no X11 *** own *** window. <see cref="IntPtr"/> </param>
		/// <param name="fixedSize"> The fixed size, that ist to use (if set) rather than the calcualated size. Passed as reference to avoid structure copy constructor calls. <see cref="TSize"/> </param>
		public XrwComposite (IntPtr display, X11.TInt screenNumber, IntPtr parentWindow, ref TSize fixedSize)
			: base (display, screenNumber, parentWindow, ref fixedSize)
		{	
			_borderWidth = XrwTheme.CompositeBorderWidth;
		}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window for X11 calls. This widget has no X11 *** own *** window. <see cref="IntPtr"/> </param>
		/// <param name="assignedPosition"> The position of the top left top corner assigned by the window manager (for shell widgets) or geometry management (by non-shell widgets). Passed as reference to avoid structure copy constructor calls. <see cref="TPoint"/> </param>
		/// <param name="fixedSize"> The fixed size, that ist to use (if set) rather than the calcualated size. Passed as reference to avoid structure copy constructor calls. <see cref="TSize"/> </param>
		public XrwComposite (IntPtr display, X11.TInt screenNumber, IntPtr parentWindow, ref TPoint assignedPosition, ref TSize fixedSize)
			: base (display, screenNumber, parentWindow, ref assignedPosition, ref fixedSize)
		{	
			_borderWidth = XrwTheme.CompositeBorderWidth;
		}
		
		#endregion
		
        // ###############################################################################
        // ### D E S T R U C T I O N
        // ###############################################################################

        #region Destruction
		
		/// <summary> IDisposable implementation. </summary>
		public override void Dispose ()
		{
			Console.WriteLine (CLASS_NAME + "::Dispose ()");
			
			this.DisposeByParent ();
		}

		/// <summary> Dispose by parent. </summary>
		public override void DisposeByParent ()
		{	
			for (int cntChildren = _children.Count - 1; cntChildren >= 0; cntChildren--)
			{
				_children[cntChildren].Dispose();
				_children.RemoveAt (cntChildren);
			}
			
			base.DisposeByParent ();
		}
		
		#endregion
		
        // ###############################################################################
        // ### P R O P E R T I E S
        // ###############################################################################

		#region Properties
		
		/// <summary> Get the list of registered child widgets. </summary>
		public List<XrwRectObj>	Children
		{
			get { return _children; }
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
			
			foreach (XrwRectObj child in _children)
			{
				result.AddRange (child.AllWindowedWidgets());
			}

			return result;
		}
		
		#endregion
		
		#region Methods
		
		/// <summary> Check wether the indicated widget is a child. </summary>
		/// <param name="child"> The widget to test. <see cref="XrwRectObj"/> </param>
		/// <returns> True, if indicated widget is a child, or false otherwise. <see cref="System.Boolean"/> </returns>
		public virtual bool HasChild (XrwRectObj child)
		{
			return _children.Contains (child);
		}
		
		/// <summary> Add a widget to the list of children. </summary>
		/// <param name="child"> The widget to add. <see cref="XrwRectObj"/> </param>
		public virtual void AddChild (XrwRectObj child)
		{
			if (child == null)
				return;
				
			_children.Add (child);
			child._parent = this;
		}
		
		/// <summary> Remove a widget from the list of children. </summary>
		/// <param name="child"> The widget to remove. <see cref="XrwRectObj"/> </param>
		public virtual void RemoveChild (XrwRectObj child)
		{
			if (child == null)
				return;
			
			_children.Remove (child);
			child._parent = null;
		}
		
		/// <summary> Insert a widget into the list of children. </summary>
		/// <param name="index"> The preferred index to insert at. <see cref="System.Int32"/> </param>
		/// <param name="child"> The widget to insert. <see cref="XrwRectObj"/> </param>
		public virtual void InsertChild (int index, XrwRectObj child)
		{
			if (child == null)
				return;
			
			if (index < 0 || index > _children.Count)
				index = _children.Count;

			_children.Insert (index, child);
			child._parent = null;
		}
		
		#endregion
		
		#region Event handler
		
		/// <summary> Handle the ExposeEvent. </summary>
		/// <param name="e"> The event data. <see cref="XrwExposeEvent"/> </param>
		/// <remarks> Set XrwExposeEvent. Set result to nonzero to stop further event processing. </remarks>
		/// <remarks> This metod is called from applications message loop for windowed widgets only. </remarks>
		public override void OnExpose (XrwExposeEvent e)
		{
			if (_shown == false)
				return;
			
			base.OnExpose(e);
			
			foreach (XrwRectObj child in _children)
			{
				if (!(child.HasOwnWindow))
					child.OnExpose(e);
			}

			e.Result = 1; // TRUE
		}
		
		#endregion
		
	}

}

