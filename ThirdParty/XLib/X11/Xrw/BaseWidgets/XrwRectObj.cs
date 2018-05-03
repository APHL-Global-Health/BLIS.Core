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
	
	/// <summary> The windowless fundamental invisible object class with geometry. </summary>
	public class XrwRectObj : XrwObject
	{

        // ###############################################################################
        // ### C O N S T A N T S
        // ###############################################################################

        #region Constants

        /// <summary> The class name constant. </summary>
        public new const string	CLASS_NAME = "XrwRectObj";
		
		/// <summary> The minimum width of this rect object, that can not be underrun. </summary>
		public const int		MIN_WIDTH  = 10;
		
		/// <summary> The minimum width of this rect object, that can not be underrun. </summary>
		public const int		MIN_HEIGHT = 10;
		
        #endregion

		// ###############################################################################
        // ### A T T R I B U T E S
        // ###############################################################################

		#region Attributes
		
		/// <summary> The parent widget, that contains this widget. </summary>
		/// <remarks> The parent pointer will be set by parent widget's Add () or Remove () methods. </remarks>
		internal XrwObject		_parent						= null;
		
		/// <summary> The position of the top left top corner assigned by the window manager (for shell widgets) or geometry management (by non-shell widgets). </summary>
		internal TPoint      	_assignedPosition			= new TPoint (0, 0);
		
		/// <summary> The size assigned by the window manager (for shell widgets) or geometry management (by non-shell widgets). </summary>
		internal  TSize			_assignedSize				= new TSize (-1, -1);
		
		/// <summary> The fixed size, that ist to use (if set) rather than the calcualated size. </summary>
		/// <remarks> A negative value for width or height indicates that the respective dimension
		/// is not fixed, but to use as calculated by PreferredSize(). </remarks>
		protected TSize			_fixedSize					= new TSize (-1, -1);
		
		/// <summary> The border width of the outside border. </summary>
		protected int			_borderWidth				= XrwTheme.BorderWidth;
		
		/// <summary> The border color of the outside border. </summary>
		protected TPixel		_borderColorPixel			= (TPixel)0;
		
		/// <summary> Identicate whether the widget can expand to the sibling's max width. </summary>
		protected bool			_expandToMaxSiblingWidth	= false;
		
		/// <summary> Identicate whether the widget can expand width. </summary>
		protected bool			_expandToAvailableWidth		= false;
		
		/// <summary> Identicate whether the widget can expand to the sibling's max height. </summary>
		protected bool			_expandToMaxSiblingHeight	= false;
		
		/// <summary> Identicate whether the widget can expand height. </summary>
		protected bool			_expandToAvailableHeight	= false;
		
		/// <summary> The visibility state. </summary>
		protected bool			_shown						= false;
		
        #endregion

		// ###############################################################################
        // ### E V E N T S
        // ###############################################################################

		#region Events
		
		/// <summary> Register expose event handler. </summary>
		public event ExposeDelegate Expose;
		
		/// <summary> Register KeyPress event handler. </summary>
		public event KeyPressDelegate KeyPress;
		
		/// <summary> Register KeyRelease event handler. </summary>
		public event KeyReleaseDelegate KeyRelease;
		
		/// <summary> Register ButtonPress event handler. </summary>
		public event ButtonPressDelegate ButtonPress;
		
		/// <summary> Register KeyPress event handler. </summary>
		public event ButtonReleaseDelegate ButtonRelease;
		
		/// <summary> Register FocusIn event handler. </summary>
		public event FocusInDelegate FocusIn;
		
		/// <summary> Register FocusOut event handler. </summary>
		public event FocusOutDelegate FocusOut;
		
		/// <summary> Register Enter event handler. </summary>
		public event EnterDelegate Enter;
		
		/// <summary> Register Leave event handler. </summary>
		public event LeaveDelegate Leave;
		
		/// <summary> Register Motion event handler. </summary>
		public event MotionDelegate Motion;

        #endregion

        // ###############################################################################
        // ### C O N S T R U C T I O N   A N D   I N I T I A L I Z A T I O N
        // ###############################################################################

        #region Construction
		
		/// <summary> Default constructor. </summary>
		public XrwRectObj () { ; }

		/// <summary> Initializing constructor. </summary>
		/// <param name="assignedPosition"> The position of the top left top corner assigned by the window manager (for shell widgets) or geometry management (by non-shell widgets). Passed as reference to avoid structure copy constructor calls. <see cref="TPoint"/> </param>
		public XrwRectObj (ref TPoint assignedPosition)
		{
			if (assignedPosition.X < 0)
			{
				Console.WriteLine (CLASS_NAME + "::Ctr () WARNING: The assignedPosition x must be greater or equal 0.");
				assignedPosition.X = 0;
			}
			if (assignedPosition.Y < 0)
			{	
				Console.WriteLine (CLASS_NAME + "::Ctr () WARNING: The assignedPosition y must be greater or equal 0.");
				assignedPosition.X = 0;
			}
			
			_assignedPosition = assignedPosition;
		}

		/// <summary> Initializing constructor. </summary>
		/// <param name="fixedSize"> The fixed size, that ist to use (if set) rather than the calcualated size. Passed as reference to avoid structure copy constructor calls. <see cref="TSize"/> </param>
		public XrwRectObj (ref TSize fixedSize)
		{
			if (fixedSize.Width < MIN_WIDTH)
			{
				Console.WriteLine (CLASS_NAME + "::Ctr () WARNING: The width must be greater or equal MIN_WIDTH.");
				fixedSize.Width = MIN_WIDTH;
			}
			if (fixedSize.Height < MIN_HEIGHT)
			{	
				Console.WriteLine (CLASS_NAME + "::Ctr () WARNING: The height must be greater or equal MIN_HEIGHT.");
				fixedSize.Height = MIN_HEIGHT;
			}
			
			_fixedSize = fixedSize;
		}

		/// <summary> Initializing constructor. </summary>
		/// <param name="assignedPosition"> The position of the top left top corner assigned by the window manager (for shell widgets) or geometry management (by non-shell widgets). Passed as reference to avoid structure copy constructor calls. <see cref="TPoint"/> </param>
		/// <param name="fixedSize"> The fixed size, that ist to use (if set) rather than the calcualated size. Passed as reference to avoid structure copy constructor calls. <see cref="TSize"/> </param>
		public XrwRectObj (ref TPoint assignedPosition, ref TSize fixedSize)
		{
			if (fixedSize.Width < MIN_WIDTH)
			{
				Console.WriteLine (CLASS_NAME + "::Ctr () WARNING: The width must be greater or equal MIN_WIDTH.");
				fixedSize.Width = MIN_WIDTH;
			}
			if (fixedSize.Height < MIN_HEIGHT)
			{	
				Console.WriteLine (CLASS_NAME + "::Ctr () WARNING: The height must be greater or equal MIN_HEIGHT.");
				fixedSize.Height = MIN_HEIGHT;
			}
			
			_assignedPosition = assignedPosition;
			_fixedSize = fixedSize;
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
		
		/// <summary> Get the parent widget. </summary>
		public virtual XrwObject Parent
		{	get	{ return _parent;	}	}
			
		/// <summary> Get the application shell this widget is registered for. </summary>
		public XrwApplicationShell ApplicationShell
		{
			get
			{
				if (_parent as XrwApplicationShell != null)
					return _parent as XrwApplicationShell;
				else if (_parent as XrwRectObj != null)
					return (_parent as XrwRectObj).ApplicationShell;
				else
					return null;
			}
		}
		
		/// <summary> Get whether the width is fixed. </summary>
		/// <returns> The fixed width flag. <see cref="System.Boolean"/> </returns>
		/// <remarks> True if the width is fix, or false otherwise. </remarks>
		public bool IsFixedWidth
		{
			get { return (_fixedSize.Width < 0 ? false : true); }
		}
		
		/// <summary> Get whether the height is fixed. </summary>
		/// <returns> The fixed height flag. <see cref="System.Boolean"/> </returns>
		/// <remarks> True if the height is fix, or false otherwise. </remarks>
		public bool IsFixedHeight
		{
			get { return (_fixedSize.Height < 0 ? false : true); }
		}
		
		/// <summary> Get the fixed size. </summary>
		/// <returns> The fixed size. <see cref="TSize"/> </returns>
		/// <remarks> A negative value indicates that the respective dimension is not tixed (dynamically calculated by PreferredWidth ()). </remarks>
		public TSize FixedSize
		{
			get { return _fixedSize; }
		}
		
		/// <summary> Get the position assigned by the geometry management. </summary>
		/// <returns> The position assigned by the geometry management. <see cref="TSize"/> </returns>
		public TPoint AssignedPosition
		{
			get { return _assignedPosition; }
		}
		
		/// <summary> Get the size assigned by the geometry management. </summary>
		/// <returns> The size assigned by the geometry management. <see cref="TSize"/> </returns>
		/// <remarks> A negative value for width or height indicates that the respective dimension
		/// is not yet initialized by the geometry management. </remarks>
		public TSize AssignedSize
		{
			get { return _assignedSize; }
		}
		
		/// <summary> Get or set the X11 border pixel do use for border drawing. </summary>
		public TPixel BorderColor
		{
			get	{	return _borderColorPixel;	}
			set {	_borderColorPixel = value;	}
		}
		
		/// <summary> Get or set outer border width. </summary>
		/// <param name="value"> The outer border width to set. <see cref="System.Int32"/> </param>
		/// <returns> The outer border width. <see cref="System.Int32"/> </returns>
		public virtual int BorderWidth
		{
			get { return _borderWidth; }
			set
			{
				_borderWidth = (Math.Max (0, value));
				if (value < 0)
					Console.WriteLine (CLASS_NAME + "::BorderWidth () WARNING: The border width must be greater or equal 0.");
			}
		}
		
		/// <summary> Get or set the identicator whether the widget can expand width. </summary>
		/// <returns> True, if width can be expanded, or false otherwise. <see cref="System.Boolean"/> </returns>
		/// <remarks> Be careful with changes on the default setting! </remarks>
		/// <remarks> MaxSiblingWidth expansion has priority over AvailableWidth expansion. </remarks>
		public bool ExpandToMaxSiblingWidth
		{
			get	{	return _expandToMaxSiblingWidth;	}
			set	{	_expandToMaxSiblingWidth = value;	}
		}
		
		/// <summary> Get or set the identicator whether the widget can expand width. </summary>
		/// <returns> True, if width can be expanded, or false otherwise. <see cref="System.Boolean"/> </returns>
		/// <remarks> Be careful with changes on the default setting! </remarks>
		/// <remarks> AvailableWidth expansion is subordinated to MaxSiblingWidth expansion. </remarks>
		public bool ExpandToAvailableWidth
		{
			get	{	return _expandToAvailableWidth;	}
			set	{	_expandToAvailableWidth = value;	}
		}
		
		/// <summary> Get or set the identicator whether the widget can expand height. </summary>
		/// <returns> True, if height can be expanded, or false otherwise. <see cref="System.Boolean"/> </returns>
		/// <remarks> Be careful with changes on the default setting! </remarks>
		/// <remarks> MaxSiblingHeight expansion has priority over AvailableHeight expansion. </remarks>
		public bool ExpandToMaxSiblingHeight
		{
			get	{	return _expandToMaxSiblingHeight;	}
			set	{	_expandToMaxSiblingHeight = value;	}
		}
		
		/// <summary> Get or set the identicator whether the widget can expand height. </summary>
		/// <returns> True, if height can be expanded, or false otherwise. <see cref="System.Boolean"/> </returns>
		/// <remarks> Be careful with changes on the default setting! </remarks>
		/// <remarks> AvailableHeight expansion is subordinated to MaxSiblingHeight expansion. </remarks>
		public bool ExpandToAvailableHeight
		{
			get	{	return _expandToAvailableHeight;	}
			set	{	_expandToAvailableHeight = value;	}
		}
		
		/// <summary> Get the visibility state. </summary>
		/// <returns> True, if object is shown/visible. <see cref="System.Boolean"/> </returns>
		public bool Shown
		{	get { return _shown; }
		}
		
		#endregion
		
        // ###############################################################################
        // ### M E T H O D S
        // ###############################################################################

		#region Methods
		
		/// <summary> Set the fixed width. A negative value indicates that the width is dynamically calculated by PreferredWidth(). </summary>
		/// <param name="fixedWidth"> The fixed width to set. <see cref="System.Int32"/> </param>
		/// <returns> True on success, false otherwise. <see cref="System.Boolean"/> </returns>
		public bool SetFixedWidth (int fixedWidth)
		{
			if (fixedWidth >= MIN_WIDTH)
			{
				_fixedSize.Width = fixedWidth;
				return true;
			}
			else
			{
				Console.WriteLine (CLASS_NAME + "::BorderWidth () WARNING: The fixed width must be greater or equal MIN_WIDTH.");
				return false;
			}
		}
		
		/// <summary> Set the fixed height. A negative value indicates that the height is dynamically calculated by PreferredHeight(). </summary>
		/// <param name="fixedHeight"> The fixed height to set. <see cref="System.Int32"/> </param>
		/// <returns> True on success, false otherwise. <see cref="System.Boolean"/> </returns>
		public bool SetFixedHeight (int fixedHeight)
		{
			if (fixedHeight >= MIN_HEIGHT)
			{
				_fixedSize.Height = fixedHeight;
				return true;
			}
			else
			{
				Console.WriteLine (CLASS_NAME + "::BorderWidth () WARNING: The fixed height must be greater or equal MIN_HEIGHT.");
				return false;
			}
		}
		
		/// <summary> Calculate the preferred size. </summary>
		/// <returns> The preferred size. <see cref="TSize"/> </returns>
		public virtual TSize PreferredSize ()
		{
			TSize preferredSize = new TSize (MIN_WIDTH + _borderWidth * 2, MIN_HEIGHT + _borderWidth * 2);
			
			if (_fixedSize.Width >= 0 && _fixedSize.Width > MIN_WIDTH)
				preferredSize.Width = _fixedSize.Width + _borderWidth * 2;
			
			if (_fixedSize.Height >= 0 && _fixedSize.Height > MIN_HEIGHT)
				preferredSize.Height = _fixedSize.Height + _borderWidth * 2;
			
			return preferredSize;
		}

		/// <summary> Calculate the client area. </summary>
		/// <returns> The client area. <see cref="TRectangle"/> </returns>
		/// <remarks> The client area os the preferred size without outer border. </remarks>
		public virtual TRectangle ClientArea ()
		{
			return new TRectangle (_assignedPosition.X + _borderWidth, _assignedPosition.Y + _borderWidth,
			                       _assignedSize.Width -  _borderWidth * 2,
			                       _assignedSize.Height - _borderWidth * 2);
		}
		
		/// <summary> Get a list of all windowed widget based on this widget - including the own and all windowed children. </summary>
		/// <returns> The list of all window based widgets on this widget. <see cref="List<IWindowedWidget>"/> </returns>
		public virtual List<XrwCore> AllWindowedWidgets()
		{
			return new List<XrwCore> ();
		}
		
		/// <summary> Show the widget and map the window (if any). </summary>
		public virtual void Show ()
		{
			_shown = true;
		}
		
		/// <summary> Hide the widget and unmap the window (if any). </summary>
		public virtual void Hide ()
		{
			_shown = false;
		}
		
		/// <summary> Calcualate layout after a position or size change. </summary>
		/// <param name="assignedPosition"> The position of the top left top corner assigned by the window manager (for shell widgets) or geometry management (by non-shell widgets). <see cref="TPoint"/> </param>
		/// <param name="availableSize"> The available size. <see cref="TSize"/> </param>
		/// <remarks> This method is defined at XrwRectObj for compatibility, but must be implemented for composite widgets only. </remarks>
		public virtual void CalculateChildLayout (TPoint assignedPosition, TSize availableSize)
		{	;	}
		
        #endregion

		#region Event handler
		
		/// <summary> Handle the ExposeEvent. </summary>
		/// <param name="e"> The event data. <see cref="XrwExposeEvent"/> </param>
		/// <remarks> Set XrwExposeEvent. Set result to nonzero to stop further event processing. </remarks>
		/// <remarks> This metod is called from applications message loop for windowed widgets only. </remarks>
		public virtual void OnExpose (XrwExposeEvent e)
		{
			if (_shown == false)
				return;
			
			ExposeDelegate expose = Expose;
			if (expose != null)
				expose (this, e);
		}
		
		/// <summary> Handle the KeyPress event. </summary>
		/// <param name="e"> The event data. <see cref="XrwKeyEvent"/> </param>
		/// <remarks> Set XrwKeyEvent. Set result to nonzero to stop further event processing. </remarks>
		public virtual void OnKeyPress (XrwKeyEvent e)
		{
			KeyPressDelegate keyPress = KeyPress;
			if (keyPress != null)
				keyPress (this, e);
		}
		
		/// <summary> Handle the KeyRelease event. </summary>
		/// <param name="e"> The event data. <see cref="XrwKeyEvent"/> </param>
		/// <remarks> Set XrwKeyEvent. Set result to nonzero to stop further event processing. </remarks>
		public virtual void OnKeyRelease (XrwKeyEvent e)
		{
			KeyReleaseDelegate keyRelease = KeyRelease;
			if (keyRelease != null)
				keyRelease (this, e);
		}
		
		/// <summary> Handle the ButtonPress event. </summary>
		/// <param name="e"> The event data. <see cref="XButtonEvent"/> </param>
		/// <remarks> Set XawButtonEvent. Set result to nonzero to stop further event processing. </remarks>
		internal virtual void OnButtonPress (XrwButtonEvent e)
		{
			ButtonPressDelegate buttonPress = ButtonPress;
			if (buttonPress != null)
				buttonPress (this, e);
		}
		
		/// <summary> Handle the ButtonRelease event. </summary>
		/// <param name="e"> The event data. <see cref="XawButtonEvent"/> </param>
		/// <remarks> Set XawButtonEvent. Set result to nonzero to stop further event processing. </remarks>
		internal virtual void OnButtonRelease (XrwButtonEvent e)
		{
			ButtonReleaseDelegate buttonRelease = ButtonRelease;
			if (buttonRelease != null)
				buttonRelease (this, e);
		}
		
		/// <summary> Handle the FocusIn event. </summary>
		/// <param name="e"> The event data. <see cref="XawFocusChangeEvent"/> </param>
		/// <remarks> Set XawFocusChangeEvent. Set result to nonzero to stop further event processing. </remarks>
		internal virtual void OnFocusIn (XrwFocusChangeEvent e)
		{
			FocusInDelegate focusIn = FocusIn;
			if (focusIn != null)
				focusIn (this, e);
		}
		
		/// <summary> Handle the FocusOut event. </summary>
		/// <param name="e"> The event data. <see cref="XawFocusChangeEvent"/> </param>
		/// <remarks> Set XawFocusChangeEvent. Set result to nonzero to stop further event processing. </remarks>
		internal virtual void OnFocusOut (XrwFocusChangeEvent e)
		{
			FocusOutDelegate focusOut = FocusOut;
			if (focusOut != null)
				focusOut (this, e);
		}
		
		/// <summary> Handle the Enter event. </summary>
		/// <param name="e"> The event data. <see cref="XawCrossingEvent"/> </param>
		/// <remarks> Set XawCrossingEvent. Set result to nonzero to stop further event processing. </remarks>
		internal virtual void OnEnter (XrwCrossingEvent e)
		{
			EnterDelegate enter = Enter;
			if (enter != null)
				enter (this, e);
		}
		
		/// <summary> Handle the Leave event. </summary>
		/// <param name="e"> The event data. <see cref="XawCrossingEvent"/> </param>
		/// <remarks> Set XawCrossingEvent. Set result to nonzero to stop further event processing. </remarks>
		internal virtual void OnLeave (XrwCrossingEvent e)
		{
			LeaveDelegate leave = Leave;
			if (leave != null)
				leave (this, e);
		}
		
		/// <summary> Handle the Motion event. </summary>
		/// <param name="e"> The event data. <see cref="XrwMotionEvent"/> </param>
		/// <remarks> Set XrwCrossingEvent. Set result to nonzero to stop further event processing. </remarks>
		internal virtual void OnMotion(XrwMotionEvent e)
		{
			MotionDelegate motion = Motion;
			if (motion != null)
				motion (this, e);
		}
			
        #endregion
		
	}
	
	/// <summary> Make assigned size setter available for geometry management. </summary>
	public static class GeometryManagerAccess
	{
		
		/// <summary> Set the assigned position and size, calculated by the geometry management. </summary>
		/// <param name="rectObj"> The rectangle object to set the assigned position and size for. <see cref="XrwRectObj"/> </param>
		/// <param name="position"> The assigned position to set. <see cref="TPoint"/> </param>
		/// <param name="size"> The assigned position to set. <see cref="TSize"/> </param>
		public static void SetAssignedGeometry (XrwRectObj rectObj, TPoint position, TSize size)
		{
			if (rectObj == null)
			{
				Console.WriteLine ("GeometryManagerAccess::SetAssignedGeometry () ERROR: Argument null: rectObj");
				return;
			}
			
			// Fixed size always has priority!
			if (rectObj.IsFixedWidth == true)
				size.Width  = rectObj.FixedSize.Width;
			if (rectObj.IsFixedWidth == true)
				size.Height = rectObj.FixedSize.Height;
			rectObj._assignedSize = size;
			
			if (rectObj.HasOwnWindow && (rectObj is XrwVisibleRectObj))
			{
				// Transfer the geometry to the underlaying window.
				XrwVisibleRectObj visibleRect = rectObj as XrwVisibleRectObj;
				if (visibleRect.Display != IntPtr.Zero && visibleRect.Window != IntPtr.Zero)
				{
					// This call causes a ConfigureNotify event.
					X11lib.XMoveResizeWindow (visibleRect.Display, visibleRect.Window, (TInt)position.X, (TInt)position.Y, (TUint)size.Width, (TUint)size.Height);
					visibleRect._assignedPosition = new TPoint (0, 0);
				}
				else
					throw new AccessViolationException ("XrwVisibleRectObj or inherited widget without display or window.");
			}
			else
				rectObj._assignedPosition = position;
			
			// Child should be prevented from overwriting parent's border.
			size.Width  -= 2 * rectObj.BorderWidth;
			size.Height -= 2 * rectObj.BorderWidth;
			
			rectObj.CalculateChildLayout (position, size);
		}

	}

}

