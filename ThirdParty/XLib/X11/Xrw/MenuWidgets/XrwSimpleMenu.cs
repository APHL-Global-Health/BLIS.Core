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

	/// <summary> The windowed base class for popup menus. Glues the windowless menu entries together to one menu. </summary>
	/// <remarks> The member attribute Window contains the *** own *** window pointer. </remarks>
	public class XrwSimpleMenu : XrwOverrideShell
	{

        // ###############################################################################
        // ### C O N S T A N T S
        // ###############################################################################

        #region Constants

        /// <summary> The class name constant. </summary>
        public new const string		CLASS_NAME = "XrwSimpleMenu";
		
        #endregion

		// ###############################################################################
        // ### A T T R I B U T E S
        // ###############################################################################

		#region Attributes
		
		/// <summary> Vertical spacing between two neighbouring children. </summary>
		/// <remarks> Default value is 2. Value ranges from 0 to 400. </remarks>
		protected int				_vertSpacing					= 2;
		
        #endregion
		
        // ###############################################################################
        // ### C O N S T R U C T I O N   A N D   I N I T I A L I Z A T I O N
        // ###############################################################################

        #region Construction
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="parent"> The parent (associated top-level) shell (a menu shell can't be a top-level shell. <see cref="IntPtr"/> </param>
		public XrwSimpleMenu (XrwApplicationShell parent)
			: base (parent)
		{
			InitializeSimpleMenuResources ();
		}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="parent"> The parent (associated top-level) shell (a menu shell can't be a top-level shell. <see cref="IntPtr"/> </param>
		/// <param name="assignedPosition"> The position of the top left top corner assigned by the window manager (for shell widgets) or geometry management (by non-shell widgets). Passed as reference to avoid structure copy constructor calls. <see cref="TPoint"/> </param>
		public XrwSimpleMenu (XrwApplicationShell parent, ref TPoint assignedPosition)
			: base (parent, ref assignedPosition)
		{
			InitializeSimpleMenuResources ();
		}
		
		/// <summary> Initialize local ressources for all constructors. </summary>
		public void InitializeSimpleMenuResources ()
		{
			_vertSpacing	= 0;
			_borderWidth	= 1;
			_frameType		= TFrameTypeExt.None;
			
			FocusIn			+= HandleFocusInDefault;
			FocusOut		+= HandleFocusOutDefault;
			Motion			+= HandleMotionDefault;
			ButtonPress		+= HandleButtonPressDefault;
			ButtonRelease	+= HandleButtonReleaseDefault;
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
				Console.WriteLine (CLASS_NAME + ":: Dispose ()");
			
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
		
		/// <summary> Get or set the vertical spacing between two neighbouring children. </summary>
		/// <remarks> Default value is 2. Value ranges from 0 to 400. </remarks>
		public int VertSpacing
		{
			get	{	return _vertSpacing;	}
			set	{	_vertSpacing = Math.Min (400, Math.Max (0, value));	}
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
			TSize preferredSize = new TSize (_borderWidth * 2, _borderWidth * 2);
			
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
		
		/// <summary> Show the widget and map the window (if any). </summary>
		public override void Show()
		{
			for (int countChildren = 0; countChildren < _children.Count; countChildren++)
			{
				XrwSme sme = _children[countChildren] as XrwSme;
				if (sme != null)
				{
					sme.Show ();
					sme.Focused = false;
				}
			}
			
			base.Show();
		}
		
		/// <summary> Hide the widget and unmap the window (if any). </summary>
		public override void Hide()
		{
			for (int countChildren = 0; countChildren < _children.Count; countChildren++)
			{
				XrwSme sme = _children[countChildren] as XrwSme;
				if (sme != null)
				{
					sme.Hide ();
				}
			}
			
			base.Hide ();
		}
		
		#endregion
		
		#region Methods
		
		/// <summary> Calcualate layout after a position or size change. </summary>
		/// <remarks> This method is defined at XrwRectObj for compatibility, but must be implemented for composite widgets only. </remarks>
		public void CalculateChildLayout ()
		{
			TPoint childPosition = new TPoint (_borderWidth, _borderWidth);
			TSize  requestedSize = new TSize  (0, 0);
			
			List<ChildData> childData = new List<ChildData>();

			for (int counter = 0; counter < _children.Count; counter++)
			{
				TSize preferredSize  = _children[counter].PreferredSize ();
				childData.Add (new ChildData (_children[counter], preferredSize));
				
				if (requestedSize.Width < preferredSize.Width)
					requestedSize.Width = preferredSize.Width;
				requestedSize.Height += preferredSize.Height;
				if (counter > 0)
					requestedSize.Height += _vertSpacing;
			}
			requestedSize.Width  = Math.Max (MIN_WIDTH,  requestedSize.Width);
			requestedSize.Height = Math.Max (MIN_HEIGHT, requestedSize.Height);
				
			for (int counter = 0; counter < childData.Count; counter++)
			{
				ChildData cd = childData[counter];
				
				cd.Widget._assignedPosition.X = childPosition.X;
				cd.Widget._assignedPosition.Y = childPosition.Y;
				
				cd.Widget._assignedSize.Width  = requestedSize.Width;
				cd.Widget._assignedSize.Height = cd.Size.Height;
				
				childPosition.Y = childPosition.Y + cd.Size.Height + _vertSpacing;
			}
			this._assignedSize.Width  = requestedSize.Width  + 2 * _borderWidth;
			this._assignedSize.Height = requestedSize.Height + 2 * _borderWidth;
		}
		
		#endregion
		
		#region Event handler

		/// <summary> Handle the Motion event. </summary>
		/// <param name="source"> The widget, the Motion event is assigned to. <see cref="XrwRectObj"/> </param>
		/// <param name="e"> The event data. <see cref="XMotionEvent"/> </param>
		/// <remarks> Set XrwMotionEvent. Set result to nonzero to stop further event processing. </remarks>
		void HandleMotionDefault (XrwRectObj source, XrwMotionEvent e)
		{
			// Determine pranslated position.
			TPoint translatedPosition = new TPoint (0, 0);
			// Anywhere on the way from GNOME 2.3.0 to 3.6.2 the values x_root and y_root habe been fixed.
			if (e.Event.x_root > e.Event.x && e.Event.y_root > e.Event.y)
			{
				translatedPosition.X = e.Event.x_root;
				translatedPosition.Y = e.Event.y_root;
			}
			else
			{
				translatedPosition.X = e.Event.x;
				translatedPosition.Y = e.Event.y;
			}
			
			translatedPosition.X -= _assignedPosition.X;
			translatedPosition.Y -= _assignedPosition.Y;
				
			X11lib.XWindowAttributes menuShellAttributes = new X11lib.XWindowAttributes();
			if (this.GetWindowAttributes (ref menuShellAttributes) == true)
			{
				translatedPosition.X -= (int)menuShellAttributes.x;
				translatedPosition.Y -= (int)menuShellAttributes.y;
			}
			else
			{
				Console.WriteLine (CLASS_NAME + "::HandleButtonPress () ERROR: Unable to determine window attributes.");
			}
				
			for (int countChildren = 0; countChildren < _children.Count; countChildren++)
			{
				XrwSme sme = _children[countChildren] as XrwSme;
				if (sme != null)
				{
					if (translatedPosition.X > sme._assignedPosition.X && translatedPosition.Y > sme._assignedPosition.Y &&
					    translatedPosition.X < sme._assignedPosition.X + sme._assignedSize.Width && translatedPosition.Y < sme._assignedPosition.Y + sme._assignedSize.Height)
					{
						if (sme.Focused != true)
						{
							sme.Focused = true;
							XrwObject.SendExposeEvent (_display, sme.Window, Parent.Window);
						}
					}
					else
					{
						if (sme.Focused != false)
						{
							sme.Focused = false;
							XrwObject.SendExposeEvent (_display, sme.Window, Parent.Window);
						}
					}
				}
			}
			e.Result = 1;
		}

		/// <summary> Handle the FocusIn event. </summary>
		/// <param name="source"> The widget, the FocusIn event is assigned to. <see cref="XrwRectObj"/> </param>
		/// <param name="e"> The event data. <see cref="XawFocusChangeEvent"/> </param>
		/// <remarks> Set XrwFocusChangeEvent. Set result to nonzero to stop further event processing. </remarks>
		void HandleFocusInDefault (XrwRectObj source, XrwFocusChangeEvent e)
		{
			e.Result = 1;
		}

		/// <summary> Handle the FocusOut event. </summary>
		/// <param name="source"> The widget, the FocusOut event is assigned to. <see cref="XrwRectObj"/> </param>
		/// <param name="e"> The event data. <see cref="XawFocusChangeEvent"/> </param>
		/// <remarks> Set XrwFocusChangeEvent. Set result to nonzero to stop further event processing. </remarks>
		void HandleFocusOutDefault (XrwRectObj source, XrwFocusChangeEvent e)
		{
			XrwApplicationShell appShell = ApplicationShell;
			if (appShell != null)
			{
				appShell.RemoveChild (this);
				_parent = appShell;
			}
			else
				Console.WriteLine (CLASS_NAME + "::HandleFocusOut () ERROR. Can not investigate application shell.");
			
			if (this._shown)
				this.Hide ();
			e.Result = 1;
		}

		/// <summary> Handle the ButtonPress event. </summary>
		/// <param name="source"> The widget, the ButtonPress event is assigned to. <see cref="XrwRectObj"/> </param>
		/// <param name="e"> The event data. <see cref="XawButtonEvent"/> </param>
		/// <remarks> Set XawButtonEvent. Set result to nonzero to stop further event processing. </remarks>
		void HandleButtonPressDefault (XrwRectObj source, XrwButtonEvent e)
		{
			// Determine pranslated position.
			TPoint translatedPosition = new TPoint (0, 0);
			// Anywhere on the way from GNOME 2.3.0 to 3.6.2 the values x_root and y_root habe been fixed.
			if (e.Event.x_root > e.Event.x && e.Event.y_root > e.Event.y)
			{
				translatedPosition.X = e.Event.x_root;
				translatedPosition.Y = e.Event.y_root;
			}
			else
			{
				translatedPosition.X = e.Event.x;
				translatedPosition.Y = e.Event.y;
			}
			
			translatedPosition.X -= _assignedPosition.X;
			translatedPosition.Y -= _assignedPosition.Y;
				
			X11lib.XWindowAttributes menuShellAttributes = new X11lib.XWindowAttributes();
			if (this.GetWindowAttributes (ref menuShellAttributes) == true)
			{
				translatedPosition.X -= (int)menuShellAttributes.x;
				translatedPosition.Y -= (int)menuShellAttributes.y;
			}
			else
			{
				Console.WriteLine (CLASS_NAME + "::HandleButtonPress () ERROR: Unable to determine window attributes.");
			}
			
			for (int countChildren = 0; countChildren < _children.Count; countChildren++)
			{
				XrwSme sme = _children[countChildren] as XrwSme;
				if (sme != null)
				{
					if (translatedPosition.X > sme._assignedPosition.X && translatedPosition.Y > sme._assignedPosition.Y &&
					    translatedPosition.X < sme._assignedPosition.X + sme._assignedSize.Width && translatedPosition.Y < sme._assignedPosition.Y + sme._assignedSize.Height)
					{
						sme.OnButtonPress (e);
						e.Result = 1;
					}
				}
			}
		}

		/// <summary> Handle the ButtonRelease event. </summary>
		/// <param name="source"> The widget, the ButtonRelease event is assigned to. <see cref="XrwRectObj"/> </param>
		/// <param name="e"> The event data. <see cref="XawButtonEvent"/> </param>
		/// <remarks> Set XawButtonEvent. Set result to nonzero to stop further event processing. </remarks>
		void HandleButtonReleaseDefault (XrwRectObj source, XrwButtonEvent e)
		{
			// Determine pranslated position.
			TPoint translatedPosition = new TPoint (0, 0);
			// Anywhere on the way from GNOME 2.3.0 to 3.6.2 the values x_root and y_root habe been fixed.
			if (e.Event.x_root > e.Event.x && e.Event.y_root > e.Event.y)
			{
				translatedPosition.X = e.Event.x_root;
				translatedPosition.Y = e.Event.y_root;
			}
			else
			{
				translatedPosition.X = e.Event.x;
				translatedPosition.Y = e.Event.y;
			}
			
			translatedPosition.X -= _assignedPosition.X;
			translatedPosition.Y -= _assignedPosition.Y;
				
			X11lib.XWindowAttributes menuShellAttributes = new X11lib.XWindowAttributes();
			if (this.GetWindowAttributes (ref menuShellAttributes) == true)
			{
				translatedPosition.X -= (int)menuShellAttributes.x;
				translatedPosition.Y -= (int)menuShellAttributes.y;
			}
			else
			{
				Console.WriteLine (CLASS_NAME + "::HandleButtonRelease () ERROR: Unable to determine window attributes.");
			}
			
			for (int countChildren = 0; countChildren < _children.Count; countChildren++)
			{
				XrwSme sme = _children[countChildren] as XrwSme;
				if (sme != null)
				{
					if (translatedPosition.X > sme._assignedPosition.X && translatedPosition.Y > sme._assignedPosition.Y &&
					    translatedPosition.X < sme._assignedPosition.X + sme._assignedSize.Width && translatedPosition.Y < sme._assignedPosition.Y + sme._assignedSize.Height)
					{
						sme.OnButtonRelease (e);
						this.Hide ();
						e.Result = 1;
					}
				}
			}
		}
		
		#endregion

	}
}

