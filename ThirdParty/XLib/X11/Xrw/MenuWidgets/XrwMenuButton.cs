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

	/// <summary> The command button widget, that pops up a simple menu. </summary>
	/// <remarks> The member attribute Window contains the window pointer of the *** own *** window. </remarks>
	public class XrwMenuButton : XrwCommand
	{

        // ###############################################################################
        // ### C O N S T A N T S
        // ###############################################################################

        #region Constants

        /// <summary> The class name constant. </summary>
        public new const string	CLASS_NAME = "XrwMenuButton";
		
        #endregion

		// ###############################################################################
        // ### A T T R I B U T E S
        // ###############################################################################

		#region Attributes
		
		/// <summary> The associated menu. </summary>
		private XrwSimpleMenu		_menu = null;

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
		public XrwMenuButton (IntPtr display, X11.TInt screen, IntPtr parentWindow, string label)
			: base (display, screen, parentWindow, label)
		{
			InitializeMenuButtonRessources ();
		}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window that will be the parent for an X11 *** own ** window. <see cref="IntPtr"/> </param>
		/// <param name="fixedSize"> The fixed size, that ist to use (if set) rather than the calcualated size. <see cref="TIntSize"/> </param>
		/// <param name="label"> The label to display. <see cref="System.String"/> </param>
		public XrwMenuButton (IntPtr display, X11.TInt screen, IntPtr parentWindow, ref TSize fixedSize, string label)
			: base (display, screen, parentWindow, ref fixedSize, label)
		{
			InitializeMenuButtonRessources ();
		}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="parentWindow"> The X11 *** parent *** window that will be the parent for an X11 *** own ** window. <see cref="IntPtr"/> </param>
		/// <param name="assignedPosition"> The position of the top left top corner assigned by the window manager (for shell widgets) or geometry management (by non-shell widgets). <see cref="TPoint"/> </param>
		/// <param name="fixedSize"> The fixed size, that ist to use (if set) rather than the calcualated size. <see cref="TIntSize"/> </param>
		/// <param name="label"> The label to display. <see cref="System.String"/> </param>
		public XrwMenuButton (IntPtr display, X11.TInt screenNumber, IntPtr parentWindow, ref TPoint assignedPosition, ref TSize fixedSize, string label)
			: base (display, screenNumber, parentWindow, ref assignedPosition, ref fixedSize, label)
		{	
			InitializeMenuButtonRessources ();
		}
		
		/// <summary> Initialize local ressources for all constructors. </summary>
		private void InitializeMenuButtonRessources ()
		{
			ButtonPress += HandleButtonPressDefault;
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
		
		/// <summary> Get or set the associated menu. </summary>
		public XrwSimpleMenu Menu
		{
			get	{	return _menu;	}
			set	{	_menu = value;	}
		}

		#endregion
	
        // ###############################################################################
        // ### M E T H O D S
        // ###############################################################################
		
		#region Methods
		
		#endregion
		
		#region Event handler

		/// <summary> Handle the ButtonPress event. </summary>
		/// <param name="source"> The widget, the ButtonPress event is assigned to. <see cref="XrwRectObj"/> </param>
		/// <param name="e"> The event data. <see cref="XawButtonEvent"/> </param>
		/// <remarks> Set XawButtonEvent. Set result to nonzero to stop further event processing. </remarks>
		private new void HandleButtonPressDefault (XrwRectObj source, XrwButtonEvent e)
		{
			if (_menu != null)
			{
				// Set position and size.
				TPoint position = new TPoint (20, 20);
				X11lib.XWindowAttributes menuButtonAttributes = new X11lib.XWindowAttributes();
				if (this.GetWindowAttributes (ref menuButtonAttributes) == true)
				{
					position.X = (int)menuButtonAttributes.x;
					position.Y = (int)menuButtonAttributes.y + (int)menuButtonAttributes.height;
				}
				else
				{
					Console.WriteLine (CLASS_NAME + "::HandleButtonPress () ERROR: Unable to determine window attributes.");
				}
				
				XrwObject parent = this.Parent;
				while (parent != null && (parent is XrwVisibleRectObj))
				{
					if (parent is XrwWmShell)
					{
						position.X += (parent as XrwWmShell).AssignedPosition.X;
						position.Y += (parent as XrwWmShell).AssignedPosition.Y;
					}
					else if (parent.HasOwnWindow)
					{
						if ((parent as XrwVisibleRectObj).GetWindowAttributes (ref menuButtonAttributes) == true)
						{
							position.X += (int)menuButtonAttributes.x;
							position.Y += (int)menuButtonAttributes.y;
						}
						else
						{
							Console.WriteLine (CLASS_NAME + "::HandleButtonPress () ERROR: Unable to determine parent window attributes.");
						}
					}
					else
					{
						// Windowless widgets positions must not be added!
					}
					parent = (parent as XrwVisibleRectObj).Parent;
				}
				
				TSize size = _menu.AssignedSize;
				_menu.MoveResize (ref position, ref size);
				
				// Register menu shell to her application shell for integrating into their event loop.
				XrwApplicationShell appShell = _menu.ApplicationShell;
				if (appShell != null)
				{
					if (!appShell.HasChild (_menu))
						appShell.AddChild (_menu);
				}
				else
					Console.WriteLine (CLASS_NAME + "::HandleButtonPress () ERROR. Can not investigate menu's application shell.");

				_menu.Show ();
			}
			else
			{
				Console.WriteLine (CLASS_NAME + "::HandleButtonPress () ERROR: No menu registered.");
			}
			e.Result = 1;
		}
		
		#endregion

	}
}

