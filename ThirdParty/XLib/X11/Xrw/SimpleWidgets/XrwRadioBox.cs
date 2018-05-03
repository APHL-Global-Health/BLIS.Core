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
	
	/// <summary> The windowless constraint widget, arranging the child widgets horizontally. </summary>
	/// <remarks> The member attribute Window contains the window pointer of the *** parent ***. </remarks>
	public class XrwRadioBox : XrwBox
	{

        // ###############################################################################
        // ### C O N S T A N T S
        // ###############################################################################

        #region Constants

        /// <summary> The class name constant. </summary>
        public new const string	CLASS_NAME = "XrwRadioBox";
		
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
		protected XrwRadioBox (IntPtr display, X11.TInt screenNumber, IntPtr parentWindow)
			: base (display, screenNumber, parentWindow)
		{
		}
		
		/// <summary> Static constructor for a horizontally oriented radio box. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window for X11 calls. This widget has no X11 *** own *** window. <see cref="IntPtr"/> </param>
		public static XrwRadioBox NewHRadioBox (IntPtr display, X11.TInt screenNumber, IntPtr parentWindow)
		{
			XrwRadioBox b = new XrwRadioBox (display, screenNumber, parentWindow);
			return b;
		}
		
		/// <summary> Static constructor for a horizontally oriented radio box. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window for X11 calls. This widget has no X11 *** own *** window. <see cref="IntPtr"/> </param>
		public static XrwRadioBox NewVRadioBox (IntPtr display, X11.TInt screenNumber, IntPtr parentWindow)
		{
			XrwRadioBox b = new XrwRadioBox (display, screenNumber, parentWindow);
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
			
        #endregion
		
        // ###############################################################################
        // ### M E T H O D S
        // ###############################################################################

		#region XrwRectObj overrides
			
        #endregion

		#region XrwComposite overrides

		/// <summary> Add a widget to the list of children. </summary>
		/// <param name="child"> The widget to add. <see cref="XrwRectObj"/> </param>
		public override void AddChild (XrwRectObj child)
		{
			this.InsertChild (-1, child);
		}

		/// <summary> Insert a widget into the list of children. </summary>
		/// <param name="index"> The preferred index to insert at. <see cref="System.Int32"/> </param>
		/// <param name="child"> The widget to add. <see cref="XrwRectObj"/> </param>
		public override void InsertChild (int index, XrwRectObj child)
		{
			if (!(child is XrwRadio))
			{
				if (XrwApplicationSettings.VERBOSE_OUTPUT_TO_CONSOLE)
					Console.WriteLine (CLASS_NAME + "::AddChild (); Parameter 'child' must be of type XrwRadio!");
			}
			
			if (child is XrwRadio)
			{
				if (_children.Count == 0)
					(child as XrwRadio).Pressed = true;
				else
					(child as XrwRadio).Pressed = false;
			}

			base.InsertChild (index, child);

			if (child is XrwRadio)
			{
				(child as XrwRadio).SwitchedOn += HandleChildSwitchedOn;
			}
		}
		
		/// <summary> Remove a widget from the list of children. </summary>
		/// <param name="child"> The widget to remove. <see cref="XrwRectObj"/> </param>
		public override void RemoveChild (XrwRectObj child)
		{
			if (!(child is XrwRadio))
			{
				if (XrwApplicationSettings.VERBOSE_OUTPUT_TO_CONSOLE)
					Console.WriteLine (CLASS_NAME + "::RemoveChild (); Parameter 'child' must be of type XrwRadio!");
				    throw new ArgumentException ("Parameter 'child' must be of type XrwRadio!", "child");
			}
			
			throw new NotImplementedException ();
		}
		
        #endregion
		
		#region Methods
		
		#endregion
		
		#region Event handler

		/// <summary> Handle the SwitchedOn event. </summary>
		/// <param name="source"> The widget, the SwitchedOn event is assigned to. <see cref="XrwRectObj"/> </param>
		protected void HandleChildSwitchedOn (XrwRectObj source)
		{
			for (int count = 0; count < _children.Count; count++)
			{
				if (_children[count] == source)
					continue;
				if (_children[count] is XrwRadio)
					(_children[count] as XrwRadio).Pressed = false;
			}
		}
		
        #endregion
		
	}
}
