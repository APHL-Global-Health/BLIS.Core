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
	public static class XrwApplicationSettings
	{
		
		/// <summary> Define whether to write verbose trace/debug output to console. </summary>
		public static bool     VERBOSE_OUTPUT_TO_CONSOLE = true;

	}


	/// <summary> The windowed base class for application shells, that implement the "always the same" code of an X11 application. </summary>
	/// <remarks> The member attribute Window contains the *** own *** window pointer. </remarks>
	public class XrwApplicationShell : XrwWmShell
	{

        // ###############################################################################
        // ### C O N S T A N T S
        // ###############################################################################

        #region Constants

        /// <summary> The class name constant. </summary>
        public new const string	CLASS_NAME = "XrwApplicationShell";
		
        #endregion
		
		// ###############################################################################
        // ### A T T R I B U T E S
        // ###############################################################################

		#region Attributes
		
		/// <summary> The delete message from the windows manager. Closing an app via window
		/// title functionality doesn't generate a window message - it only generates a
		/// window manager message, thot must be routed to the window (message loop). </summary>
		private IntPtr					_wmDeleteMessage ;
		
		/// <summary> The list of associated transient shells, that must be handled by the message loop. </summary>
		private List<XrwTransientShell>	_associatedTransientShells = new List<XrwTransientShell> ();
		
        #endregion

		// ###############################################################################
        // ### E V E N T S
        // ###############################################################################

		#region Events
		
		/// <summary> Register WmShellClose event handler. </summary>
		public WmShellCloseDelegate WmShellClose;

        #endregion
		
        // ###############################################################################
        // ### C O N S T R U C T I O N   A N D   I N I T I A L I Z A T I O N
        // ###############################################################################

        #region Construction
		
		/// <summary> Default constructor. </summary>
		/// <remarks> This constructor skips the initialization of a graphics context. Therefor the graphics context must be initialized later. </remarks>
		public XrwApplicationShell ()
			: base (X11lib.XOpenDisplay(String.Empty)) /* Connect to X server. */
			        
		{
			// The border is managed by the window manager.
			_borderWidth		= 0;
			
			// Expanding is the default for an application.
			_expandToAvailableWidth  = true;
			_expandToAvailableHeight = true;
			
			// Register event handler.
			Expose += HandleExposeDefault;
		}
		
		/// <summary> Initialize ressources for all constructors. </summary>
		public void InitializeApplicationShellResources ()
		{
			/* Get the colors black and white. */
			TPixel black = X11lib.XBlackPixel (_display, _screenNumber);	/* get color black */
			TPixel white = X11lib.XWhitePixel (_display, _screenNumber);	/* get color white */
			
			/* Once the display is initialized, create the window.
			   It will have the foreground white and background black */
		   	_window = X11lib.XCreateSimpleWindow (_display, X11lib.XDefaultRootWindow(_display), (TInt)0, (TInt)0, (TUint)350, (TUint)300, 0, 0, black);
			if (_window == IntPtr.Zero)
			{
				Console.WriteLine (CLASS_NAME + "::InitializeApplicationShellResources () ERROR: Failed to create window.");
				return;
			}

			X11lib.XSelectInput(_display, _window, EventMask.StructureNotifyMask | EventMask.ExposureMask      |
			                    				   EventMask.ButtonPressMask     | EventMask.ButtonReleaseMask |
			                    				   EventMask.KeyPressMask        | EventMask.KeyReleaseMask    |
			                    				   EventMask.FocusChangeMask     | EventMask.EnterWindowMask   |
			                    				   EventMask.LeaveWindowMask     | EventMask.PointerMotionMask |
			                    				   EventMask.SubstructureNotifyMask);

			_hasOwnWindow = true;
			
			/* here is where some properties of the window can be set.
			   The third and fourth items indicate the name which appears
			   at the top of the window and the name of the minimized window
			   respectively. */
			//X11lib.XSetStandardProperties(dis, win, "My Window","HI!", None, NULL, 0, NULL);
			
			/* Hook the closing event from windows manager. */
			_wmDeleteMessage = X11lib.XInternAtom (_display, "WM_DELETE_WINDOW", false);
			if (X11lib.XSetWMProtocols (_display, _window, ref _wmDeleteMessage, (X11.TInt)1) == 0)
			{
				Console.WriteLine (CLASS_NAME + "::InitializeApplicationShellResources () WARNING: Failed to register 'WM_DELETE_WINDOW' event.");
			}

			/* Create the foreground Graphics Context. */
			if (_gc != IntPtr.Zero)
			{
				if (XrwApplicationSettings.VERBOSE_OUTPUT_TO_CONSOLE)
					Console.WriteLine (CLASS_NAME + "::InitializeApplicationShellResources () Replace the foreground GC.");
				X11lib.XFreeGC (_display, _gc);
				_gc = IntPtr.Zero;
			}
		    _gc = X11lib.XCreateGC(_display, _window, 0, IntPtr.Zero);        
			X11lib.XSetForeground (_display, _gc, white);
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
			XrwTheme.FreeGraphic ();
			base.DisposeByParent ();
		}

		#endregion
		
        // ###############################################################################
        // ### M E T H O D S
        // ###############################################################################

		#region XrwRectObj overrides
		
		/// <summary> Show the widget and map the window (if any). </summary>
		/// <remarks> Adds the override shell to her application schell's children, if not already done. </remarks>
		public override void Show()
		{
			_shown = true;
			
		 	X11lib.XMapWindow (_display, _window);
			X11lib.XFlush (_display);
			X11lib.XSetInputFocus (_display, _window, X11lib.TRevertTo.RevertToParent, (TInt)0);
		}
		
		/// <summary> Hide the widget and unmap the window (if any). </summary>
		public override void Hide()
		{
			_shown = false;
			
			X11lib.XUnmapWindow (_display, _window);
	  		X11lib.XFlush (_display);
		}
		
		/// <summary> Get or set outer border width. </summary>
		/// <param name="value"> The outer border width to set. <see cref="System.Int32"/> </param>
		/// <returns> The outer border width. <see cref="System.Int32"/> </returns>
		public override int BorderWidth
		{
			get { return _borderWidth; }
			set { Console.WriteLine (CLASS_NAME + ":: BorderWidth () WARNING: Setting the outer border width is not implemented."); }
		}
		
		/// <summary> Process the ExposeEvent. </summary>
		/// <param name="source"> The widget, the ExposeEvent is assigned to. <see cref="XrwRectObj"/> </param>
		/// <param name="e"> The event data. <see cref="XrwExposeEvent"/> </param>
		/// <remarks> Set XrwExposeEvent. Set result to nonzero to stop further event processing. </remarks>
		void HandleExposeDefault (XrwRectObj source, XrwExposeEvent e)
		{
			foreach (XrwRectObj widget in _children)
				widget.OnExpose (e);
		}
		
		#endregion
		
        #region Methods
		
		/// <summary> Add a transient shell to the child shells. This is necessary to start processing events of transient shell and its children. </summary>
		/// <param name="transientShell"> The transient shell to add to the child shells. <see cref="XrwTransientShell"/> </param>
		/// <remarks> Inserts application modal transient shells at list heat, otherwise at list tail. </remarks>
		public void AddTransientShell (XrwTransientShell transientShell)
		{
			if (transientShell.AppModal == true)
				_associatedTransientShells.Insert (0, transientShell);
			else
				_associatedTransientShells.Add (transientShell);
		}
		
		/// <summary> Remove a transient shell from the child shells. This is necessary to stop processing events of transient shell and its children. </summary>
		/// <param name="transientShell"> The transient shell to remove from the child shells. <see cref="XrwTransientShell"/> </param>
		public void RemoveTransientShell (XrwTransientShell transientShell)
		{
			int cntBefore = _associatedTransientShells.Count;
			
			_associatedTransientShells.Remove (transientShell);
			
			if (XrwApplicationSettings.VERBOSE_OUTPUT_TO_CONSOLE)
				Console.WriteLine (CLASS_NAME + "::XrwApplicationShell (); Reduced associated transient shels from " +
				                   cntBefore.ToString() + " to " + _associatedTransientShells.Count.ToString());

		}
		
		/// <summary> Ensure that current modal transient shell is first in list. </summary>
		private void SortModalTransientShellsFirst()
		{
			for (int cntShells = 0; cntShells < _associatedTransientShells.Count; cntShells++)
			{
				if (cntShells == 0 && _associatedTransientShells[cntShells].AppModal == true)
					return;
				
				if (_associatedTransientShells[cntShells].AppModal == true)
				{
					XrwTransientShell shell = _associatedTransientShells[cntShells];
					_associatedTransientShells.Remove (shell);
					_associatedTransientShells.Insert (0, shell);
					return;
				}
			}
		}
		
		/// <summary> Run the application's message loop. </summary>
		protected void RunMessageLoop ()
		{
			bool proceed = true;
			while(proceed == true)
			{
				try
				{
					proceed = DoEvent();
				}
				catch (Exception e)
				{
					Console.WriteLine (CLASS_NAME + ":: RunMessageLoop () ERROR: " + e.StackTrace);
				}
			}
			return;
		}
		
		/// <summary> Process the topmost event and remove it from the event queue. </summary>
		/// <returns> True if efent processing must contionue, false otherwise. </returns>
		public bool DoEvent()
		{
			// Prevent event processing *** after dispose *** bur *** before destruction ***.
			if (_display == IntPtr.Zero)
				return false;
			
			XEvent xevent = new XEvent ();
			
			// Get the next event and stuff it into our event variable.
			// Note:  only events we set the mask for are detected!
			X11lib.XNextEvent(_display, ref xevent);
			SortModalTransientShellsFirst();
			
// ConfigureNotify: No specialities for modal shells.
			if (xevent.type == XEventName.ConfigureNotify)
			{
				// FIRST: ConfigureNotify event always goes to the application shell.
				if (xevent.ConfigureEvent.window == this._window || xevent.ConfigureEvent.window == IntPtr.Zero)
				{
					XrwConfigureEvent e = new XrwConfigureEvent (ref xevent.ConfigureEvent);
					OnConfigure (e);
					return true;
				}
				
				// SECOND: ConfigureNotify events might go to associated transient shells.
				for (int cntShells = 0; cntShells < _associatedTransientShells.Count; cntShells++)
				{
					XrwTransientShell transientShell = _associatedTransientShells[cntShells];
					if (xevent.ConfigureEvent.window == transientShell.Window)
					{	
						XrwConfigureEvent e = new XrwConfigureEvent (ref xevent.ConfigureEvent);
						transientShell.OnConfigure (e);
					    continue;
					}
					
				}

				// FINALY: ConfigureNotify events go to windowed children, if e.g. XMoveResizeWindow () has been called for them.
				TPoint childConfiguredPosition = new TPoint (xevent.ConfigureEvent.x,     xevent.ConfigureEvent.y);
				TSize  childConfiguredSize     = new TSize  (xevent.ConfigureEvent.width, xevent.ConfigureEvent.height);

				for (int cntChildren = 0; cntChildren < _children.Count; cntChildren++)
				{
					XrwRectObj childWidget = _children[cntChildren];
					List<XrwCore> windowedChildren = childWidget.AllWindowedWidgets();
					for (int cntSubChildren = 0; cntSubChildren <  windowedChildren.Count; cntSubChildren++)
					{
						XrwCore windowedSubChildWidget = windowedChildren[cntSubChildren];
						if (xevent.ConfigureEvent.window == windowedSubChildWidget.Window)
						{
							// Every direct child can occupy the complete size.
							if (windowedSubChildWidget as XrwShell != null)
								GeometryManagerAccess.SetAssignedGeometry (windowedSubChildWidget as XrwRectObj, new TPoint (0, 0), childConfiguredSize);
							else
								GeometryManagerAccess.SetAssignedGeometry (windowedSubChildWidget as XrwRectObj, childConfiguredPosition, childConfiguredSize);
						}
					}
				}
				return true;
			}
// ExposeEvent:  No specialities for modal shells.
			else if (xevent.type == XEventName.Expose && xevent.ExposeEvent.count == 0)
			{
				bool stopProcessing = false;
				
				// FIRST: ExposeEvent events might go to associated transient shells.
				for (int cntShells = 0; cntShells < _associatedTransientShells.Count; cntShells++)
				{
					XrwTransientShell transientShell = _associatedTransientShells[cntShells];
					if (xevent.ExposeEvent.window == transientShell.Window)
					{	
						XrwExposeEvent e = new XrwExposeEvent (ref xevent.ExposeEvent);
						transientShell.OnExpose (e);
						if (e.Result != 0)
						{
							stopProcessing = true;
							break;
						}
					}
					
					List<XrwCore> windowedChildren = transientShell.AllWindowedWidgets();
					for (int cntSubChildren = 0; cntSubChildren < windowedChildren.Count; cntSubChildren++)
					{
						XrwCore childWithWindow = windowedChildren[cntSubChildren];
						if (xevent.ExposeEvent.window == childWithWindow.Window)
						{
							stopProcessing = true;
							XrwExposeEvent e = new XrwExposeEvent (ref xevent.ExposeEvent);
							childWithWindow.OnExpose (e);
							if (e.Result != 0)
							{
								stopProcessing = true;
								break;
							}
						}
					}
					if (stopProcessing == true)
					    continue;
					
				}
				if (stopProcessing == true)
				    return true;

				// SECOND: ExposeEvent events might go to windowed children.
				for (int cntChildren = 0; cntChildren < _children.Count; cntChildren++)
				{
					XrwRectObj childWidget = _children[cntChildren];
					if ((childWidget as XrwOverrideShell) != null && xevent.ExposeEvent.window == (childWidget as XrwOverrideShell).Window) 
					{
						XrwExposeEvent e = new XrwExposeEvent (ref xevent.ExposeEvent);
						childWidget.OnExpose (e);
						if (e.Result != 0)
						{
							stopProcessing = true;
							break;	// Exit (inner) for.
						}
					}
					
					List<XrwCore> windows = childWidget.AllWindowedWidgets();
					for (int cntSubChildren = 0; cntSubChildren < windows.Count; cntSubChildren++)
					{
						XrwCore childWithWindow = windows[cntSubChildren];
						if (xevent.ExposeEvent.window == childWithWindow.Window)
						{
							XrwExposeEvent e = new XrwExposeEvent (ref xevent.ExposeEvent);
							childWithWindow.OnExpose (e);
							if (e.Result != 0)
							{
								stopProcessing = true;
								break;	// Exit (inner) for.
							}
						}
					}
					if (stopProcessing == true)
						break;	// Exit (outer) for.
					
				}

				if (stopProcessing == true)
					return true;
				
				// FINALLY: ExposeEvent events go to the application shell.
				XrwExposeEvent evt = new XrwExposeEvent (ref xevent.ExposeEvent);
				if (xevent.ExposeEvent.window == _window)
					this.OnExpose (evt);
				else
				{
					Console.WriteLine (CLASS_NAME + "::DoEvent () // EXPOSE " +
					                   " can not recognize window " + xevent.ExposeEvent.window.ToString() + ".");
					this.OnExpose (evt);
				}
				return true;
			}
// KeyPress: Prevent processing for non-modal shells, if a modal shell exists.
			else if (xevent.type == XEventName.KeyPress || xevent.type == XEventName.KeyRelease)
			{
				/*
				if (xevent.type == XEventName.FocusIn)
					Console.WriteLine ("KeyPress notify: " + xevent.KeyEvent.window);
				else
					Console.WriteLine ("KeyRelease notify: " + xevent.KeyEvent.window);
				 */
				// KeyEvent events might go to associated transient shells.
				for (int cntShells = 0; cntShells < _associatedTransientShells.Count; cntShells++)
				{
					XrwTransientShell transientShell = _associatedTransientShells[cntShells];
					if (xevent.KeyEvent.window == transientShell.Window)
					{	
						if (xevent.type == XEventName.KeyPress)
						{
							XrwKeyEvent e = new XrwKeyEvent (ref xevent.KeyEvent);
							transientShell.OnKeyPress (e);
							if (e.Result != 0)
							{
								// Transien shell or children might be destroyed now!
								return true; // Immediate exit!!!
							}
						}
						if (xevent.type == XEventName.KeyRelease)
						{
							XrwKeyEvent e = new XrwKeyEvent (ref xevent.KeyEvent);
							transientShell.OnKeyRelease (e);
							if (e.Result != 0)
							{
								// Transien shell or children might be destroyed now!
								return true; // Immediate exit!!!
							}
						}
						break;	// Exit for, speed up if e.Result is == 0.
					}
					
					for (int cntRectObj = 0; cntRectObj < transientShell.Children.Count; cntRectObj++)
					{
						XrwRectObj widget = transientShell.Children[cntRectObj];
						List<XrwCore> windows = widget.AllWindowedWidgets();
						for (int cntSubChildren = 0; cntSubChildren < windows.Count; cntSubChildren++)
						{
							XrwCore childWithWindow = windows[cntSubChildren];
							if (xevent.KeyEvent.window == childWithWindow.Window)
							{
								if (xevent.type == XEventName.KeyPress)
								{
									XrwKeyEvent e = new XrwKeyEvent (ref xevent.KeyEvent);
									childWithWindow.OnKeyPress (e);
									if (e.Result != 0)
									{
										// Transien shell or children might be destroyed now!
										return true; // Immediate exit!!!
									}
								}
								if (xevent.type == XEventName.KeyRelease)
								{
									XrwKeyEvent e = new XrwKeyEvent (ref xevent.KeyEvent);
									childWithWindow.OnKeyRelease (e);
									if (e.Result != 0)
									{
										// Transien shell or children might be destroyed now!
										return true; // Immediate exit!!!
									}
								}
							}
						}
					}
					// Supress key events for other widgets than the current modal shell, even if e.Result == 0.
					if (transientShell.AppModal == true)
						return true;
				}
				bool stopProcessing = false;
				
				// Subsequent KeyEvent events might go to windowed children.
				for (int cntChildren = 0; cntChildren < _children.Count; cntChildren++)
				{
					XrwRectObj widget = _children[cntChildren];
					List<XrwCore> windows = widget.AllWindowedWidgets();
					for (int cntSubChildren = 0; cntSubChildren < windows.Count; cntSubChildren++)
					{
						XrwCore childWithWindow = windows[cntSubChildren];
						if (xevent.KeyEvent.window == childWithWindow.Window)
						{
							if (xevent.type == XEventName.KeyPress)
							{
								XrwKeyEvent e = new XrwKeyEvent (ref xevent.KeyEvent);
								childWithWindow.OnKeyPress (e);
								if (e.Result != 0)
								{
									stopProcessing = true;
									break;
								}
							}
							if (xevent.type == XEventName.KeyRelease)
							{
								XrwKeyEvent e = new XrwKeyEvent (ref xevent.KeyEvent);
								childWithWindow.OnKeyRelease (e);
								if (e.Result != 0)
								{
									stopProcessing = true;
									break;
								}
							}
						}
					}
					if (stopProcessing == true)
						break;
				}

				// Finaly KeyEvent events go to the application shell.
				if (stopProcessing == false)
				{
					if (xevent.type == XEventName.KeyPress)
					{
						XrwKeyEvent e = new XrwKeyEvent (ref xevent.KeyEvent);
						this.OnKeyPress (e);
					}
					if (xevent.type == XEventName.KeyRelease)
					{
						XrwKeyEvent e = new XrwKeyEvent (ref xevent.KeyEvent);
						this.OnKeyRelease (e);
					}
				}
				return true;
			}
// ButtonEvent: Prevent processing for non-modal shells, if a modal shell exists.
			else if (xevent.type == XEventName.ButtonPress || xevent.type == XEventName.ButtonRelease)
			{
				/*
				if (xevent.type == XEventName.FocusIn)
					Console.WriteLine ("ButtonPress notify: " + xevent.ButtonEvent.window);
				else
					Console.WriteLine ("ButtonRelease notify: " + xevent.ButtonEvent.window);
				 */
				// ButtonEvent events might go to associated transient shells.
				for (int cntShells = 0; cntShells < _associatedTransientShells.Count; cntShells++)
				{
					XrwTransientShell transientShell = _associatedTransientShells[cntShells];
					if (xevent.ButtonEvent.window == transientShell.Window)
					{	
						if (xevent.type == XEventName.ButtonPress)
						{
							XrwButtonEvent e = new XrwButtonEvent(ref xevent.ButtonEvent);
							transientShell.OnButtonPress (e);
							if (e.Result != 0)
							{
								// PROBLEM: Transien shell probably disposed!
								// Transien shell or children might be destroyed now!
								return true; // Immediate exit!!!
							}
						}
						if (xevent.type == XEventName.ButtonRelease)
						{
							XrwButtonEvent e = new XrwButtonEvent(ref xevent.ButtonEvent);
							transientShell.OnButtonRelease (e);
							// Transien shell or children might be destroyed now!
							if (e.Result != 0)
							{
								// PROBLEM: Transien shell probably disposed!
								// Transien shell or children might be destroyed now!
								return true; // Immediate exit!!!
							}
						}
						break;	// Exit for, speed up if e.Result == 0.
					}
					
					for (int cntChildren = 0; cntChildren < transientShell.Children.Count; cntChildren++)
					{
						XrwRectObj widget = transientShell.Children[cntChildren];
						List<XrwCore> windows = widget.AllWindowedWidgets();
						for (int cntSubChildren = 0; cntSubChildren < windows.Count; cntSubChildren++)
						{
							XrwCore childWithWindow = windows[cntSubChildren];
							if (xevent.ButtonEvent.window == childWithWindow.Window)
							{
								if (xevent.type == XEventName.ButtonPress)
								{
									XrwButtonEvent e = new XrwButtonEvent(ref xevent.ButtonEvent);
									childWithWindow.OnButtonPress (e);
									if (e.Result != 0)
									{
										// PROBLEM: Transien shell probably disposed!
										// Transien shell or children might be destroyed now!
										return true; // Immediate exit!!!
									}
								}
								if (xevent.type == XEventName.ButtonRelease)
								{
									XrwButtonEvent e = new XrwButtonEvent(ref xevent.ButtonEvent);
									childWithWindow.OnButtonRelease (e);
									if (e.Result != 0)
									{
										// PROBLEM: Transien shell probably disposed!
										// Transien shell or children might be destroyed now!
										return true; // Immediate exit!!!
									}
								}
							}
						}
					}
					// Supress button events for other widgets than the current modal shell, even if e.Result == 0.
					if (transientShell.AppModal == true)
						return true;
				}
				bool stopProcessing = false;
				
				// Subsequent ButtonEvent events might go to windowed children.
				for (int cntChildren = 0; cntChildren < _children.Count; cntChildren++)
				{
					XrwRectObj widget = _children[cntChildren];
					List<XrwCore> windows = widget.AllWindowedWidgets();
					for (int cntSubChildren = 0; cntSubChildren < windows.Count; cntSubChildren++)
					{
						XrwCore childWithWindow = windows[cntSubChildren];
						if (xevent.ButtonEvent.window == childWithWindow.Window)
						{
							if (xevent.type == XEventName.ButtonPress)
							{
								stopProcessing = true;
								XrwButtonEvent e = new XrwButtonEvent(ref xevent.ButtonEvent);
								childWithWindow.OnButtonPress (e);
								if (e.Result != 0)
									break;
							}
							if (xevent.type == XEventName.ButtonRelease)
							{
								stopProcessing = true;
								XrwButtonEvent e = new XrwButtonEvent(ref xevent.ButtonEvent);
								childWithWindow.OnButtonRelease (e);
								if (e.Result != 0)
									break;
							}
						}
					}
					if (stopProcessing == true)
						break;
				}

				// Finaly ButtonEvent events go to the application shell.
				if (stopProcessing == false)
				{
					if (xevent.type == XEventName.ButtonPress)
					{
						XrwButtonEvent e = new XrwButtonEvent(ref xevent.ButtonEvent);
						this.OnButtonPress (e);
					}
					if (xevent.type == XEventName.ButtonRelease)
					{
						XrwButtonEvent e = new XrwButtonEvent(ref xevent.ButtonEvent);
						this.OnButtonRelease (e);
					}
				}
				return true;
			}
// FocusChangeEvent: No specialities for modal shells.
			else if (xevent.type == XEventName.FocusIn || xevent.type == XEventName.FocusOut)
			{
				/*
				if (xevent.type == XEventName.FocusIn)
					Console.WriteLine ("FocusIn notify: " + xevent.CrossingEvent.window);
				else
					Console.WriteLine ("FocusOut notify: " + xevent.CrossingEvent.window);
				 */
				bool stopProcessing = false;
				
				// FocusChangeEvent events might go to associated transient shells.
				for (int cntShells = 0; cntShells < _associatedTransientShells.Count; cntShells++)
				{
					XrwTransientShell transientShell = _associatedTransientShells[cntShells];
					if (xevent.FocusChangeEvent.window == transientShell.Window)
					{	
						if (xevent.type == XEventName.FocusIn)
						{
							stopProcessing = true;
							XrwFocusChangeEvent e = new XrwFocusChangeEvent (ref xevent.FocusChangeEvent);
							transientShell.OnFocusIn (e);
						}
						if (xevent.type == XEventName.FocusOut)
						{
							stopProcessing = true;
							XrwFocusChangeEvent e = new XrwFocusChangeEvent (ref xevent.FocusChangeEvent);
							transientShell.OnFocusOut (e);
						}
						break;
					}
					for (int cntChildren = 0; cntChildren < transientShell.Children.Count; cntChildren++)
					{
						XrwRectObj widget = transientShell.Children[cntChildren];
						List<XrwCore> windows = widget.AllWindowedWidgets();
						for (int cntSubChildren = 0; cntSubChildren < windows.Count; cntSubChildren++)
						{
							XrwCore childWithWindow = windows[cntSubChildren];
							if (xevent.FocusChangeEvent.window == childWithWindow.Window)
							{
								if (xevent.type == XEventName.FocusIn)
								{
									stopProcessing = true;
									XrwFocusChangeEvent e = new XrwFocusChangeEvent (ref xevent.FocusChangeEvent);
									childWithWindow.OnFocusIn (e);
									if (e.Result != 0)
										break;
								}
								if (xevent.type == XEventName.FocusOut)
								{
									stopProcessing = true;
									XrwFocusChangeEvent e = new XrwFocusChangeEvent (ref xevent.FocusChangeEvent);
									childWithWindow.OnFocusOut (e);
									if (e.Result != 0)
										break;
								}
							}
						}
					}
					if (stopProcessing == true)
						break;
				}
				if (stopProcessing == true)
				    return true;

				// Subsequent FocusChangeEvent events might go to windowed children.
				for (int cntChildren = 0; cntChildren < _children.Count; cntChildren++)
				{
					XrwRectObj widget = _children[cntChildren];
					List<XrwCore> windows = widget.AllWindowedWidgets();
					for (int cntSubChildren = 0; cntSubChildren < windows.Count; cntSubChildren++)
					{
						XrwCore childWithWindow = windows[cntSubChildren];
						if (xevent.FocusChangeEvent.window == childWithWindow.Window)
						{
							if (xevent.type == XEventName.FocusIn)
							{
								stopProcessing = true;
								XrwFocusChangeEvent e = new XrwFocusChangeEvent (ref xevent.FocusChangeEvent);
								childWithWindow.OnFocusIn (e);
								if (e.Result != 0)
									break;
							}
							if (xevent.type == XEventName.FocusOut)
							{
								stopProcessing = true;
								XrwFocusChangeEvent e = new XrwFocusChangeEvent (ref xevent.FocusChangeEvent);
								childWithWindow.OnFocusOut (e);
								if (e.Result != 0)
									break;
							}
						}
					}
					if (stopProcessing == true)
						break;
				}

				// Finaly FocusChangeEvent events go to the application shell.
				if (stopProcessing == false)
				{
					if (xevent.type == XEventName.FocusIn)
					{
						XrwFocusChangeEvent e = new XrwFocusChangeEvent (ref xevent.FocusChangeEvent);
						this.OnFocusIn (e);
					}
					if (xevent.type == XEventName.FocusOut)
					{
						XrwFocusChangeEvent e = new XrwFocusChangeEvent (ref xevent.FocusChangeEvent);
						this.OnFocusOut (e);
					}
				}
				return true;
			}
// CrossingEvent: No specialities for modal shells.
			else if (xevent.type == XEventName.EnterNotify || xevent.type == XEventName.LeaveNotify)
			{
				/*
				if (xevent.type == XEventName.EnterNotify)
					Console.WriteLine ("Enter notify: " + xevent.CrossingEvent.window);
				else
					Console.WriteLine ("Leave notify: " + xevent.CrossingEvent.window);
				*/
				bool stopProcessing = false;
				
				// CrossingEvent events might go to associated transient shells.
				for (int cntShells = 0; cntShells < _associatedTransientShells.Count; cntShells++)
				{
					XrwTransientShell transientShell = _associatedTransientShells[cntShells];
					if (xevent.CrossingEvent.window == transientShell.Window)
					{	
						if (xevent.type == XEventName.EnterNotify)
						{
							stopProcessing = true;
							XrwCrossingEvent e = new XrwCrossingEvent (ref xevent.CrossingEvent);
							transientShell.OnEnter (e);
							if (e.Result != 0)
								break;
						}
						if (xevent.type == XEventName.LeaveNotify)
						{
							stopProcessing = true;
							XrwCrossingEvent e = new XrwCrossingEvent (ref xevent.CrossingEvent);
							transientShell.OnLeave (e);
							if (e.Result != 0)
								break;
						}
					}
					for (int cntChildren = 0; cntChildren < transientShell.Children.Count; cntChildren++)
					{
						XrwRectObj widget = transientShell.Children[cntChildren];
						List<XrwCore> windows = widget.AllWindowedWidgets();
						for (int cntSubChildren = 0; cntSubChildren < windows.Count; cntSubChildren++)
						{
							XrwCore childWithWindow = windows[cntSubChildren];
							if (xevent.CrossingEvent.window == childWithWindow.Window)
							{
								if (xevent.type == XEventName.EnterNotify)
								{
									stopProcessing = true;
									XrwCrossingEvent e = new XrwCrossingEvent (ref xevent.CrossingEvent);
									childWithWindow.OnEnter (e);
									if (e.Result != 0)
										break;
								}
								if (xevent.type == XEventName.LeaveNotify)
								{
									stopProcessing = true;
									XrwCrossingEvent e = new XrwCrossingEvent (ref xevent.CrossingEvent);
									childWithWindow.OnLeave (e);
									if (e.Result != 0)
										break;
								}
							}
						}
					}
					if (stopProcessing == true)
						break;
				}
				if (stopProcessing == true)
				    return true;

				// Subsequent CrossingEvent events might go to windowed children.
				for (int cntChildren = 0; cntChildren < _children.Count; cntChildren++)
				{
					XrwRectObj widget = _children[cntChildren];
					List<XrwCore> windows = widget.AllWindowedWidgets();
					for (int cntSubChildren = 0; cntSubChildren < windows.Count; cntSubChildren++)
					{
						XrwCore childWithWindow = windows[cntSubChildren];
						if (xevent.CrossingEvent.window == childWithWindow.Window)
						{
							if (xevent.type == XEventName.EnterNotify)
							{
								stopProcessing = true;
								XrwCrossingEvent e = new XrwCrossingEvent (ref xevent.CrossingEvent);
								childWithWindow.OnEnter (e);
								break;
							}
							if (xevent.type == XEventName.LeaveNotify)
							{
								stopProcessing = true;
								XrwCrossingEvent e = new XrwCrossingEvent (ref xevent.CrossingEvent);
								childWithWindow.OnLeave (e);
								break;
							}
						}
					}
					if (stopProcessing == true)
						break;
				}

				// Finaly CrossingEvent events go to the application shell.
				if (stopProcessing == false)
				{
					if (xevent.type == XEventName.EnterNotify)
					{
						XrwCrossingEvent e = new XrwCrossingEvent (ref xevent.CrossingEvent);
						this.OnEnter (e);
					}
					if (xevent.type == XEventName.LeaveNotify)
					{
						XrwCrossingEvent e = new XrwCrossingEvent (ref xevent.CrossingEvent);
						this.OnLeave (e);
					}
				}
				return true;
			}
// MotionNotify: Prevent processing for non-modal shells, if a modal shell exists.
			else if (xevent.type == XEventName.MotionNotify)
			{
				for (int cntChildren = 0; cntChildren < _children.Count; cntChildren++)
				{
					XrwRectObj widget = _children[cntChildren];
					if (!(widget.HasOwnWindow) || xevent.MotionEvent.window == (widget as XrwCore).Window)
					{
						XrwMotionEvent e = new XrwMotionEvent (ref xevent.MotionEvent);
						widget.OnMotion (e);
					}
				}
			}
			else if (xevent.type == XEventName.CirculateNotify || xevent.type == XEventName.ConfigureNotify ||
			         xevent.type == XEventName.CreateNotify    || xevent.type == XEventName.DestroyNotify   ||
			         xevent.type == XEventName.GravityNotify   || xevent.type == XEventName.MapNotify       ||
			         xevent.type == XEventName.ReparentNotify  || xevent.type == XEventName.UnmapNotify)
			{
				// SubstructureNotify.
			}
			else if (xevent.type == XEventName.DestroyNotify)
			{
				// Child widget destroying.
			}
			else if (xevent.type == XEventName.ClientMessage && xevent.ClientMessageEvent.ptr1 == _wmDeleteMessage)
			{
				// ClientMessageEvent events might go to associated transient shells.
				foreach (XrwTransientShell transientShell in _associatedTransientShells)
				{
					if (xevent.ClientMessageEvent.window == transientShell.Window)
					{	
						transientShell.OnWmClose (new XrwClientMessageEvent (ref xevent.ClientMessageEvent));
						// Transien shell is destroyed now!
						return true; // Immediate exit!!!
					}
				}

				// Subsequent ClientMessageEvent events might go to windowed children.
				if (xevent.ClientMessageEvent.window == this.Window)
					OnClose (new XrwClientMessageEvent (ref xevent.ClientMessageEvent));
			}
			else
			{
				Console.WriteLine (CLASS_NAME + "::DoEvent() Unprocessed event: " + xevent.type.ToString());
			}
			return true;
		}
		
		#endregion
		
        #region Event handler
		
		/// <summary> Handle the ClientMessage event. </summary>
		/// <param name="e"> The event data. <see cref="XawClientMessageEvent"/> </param>
		/// <remarks> Set XawClientMessageEvent. Set result to nonzero to stop further event processing. </remarks>
		public void OnClose (XrwClientMessageEvent e)
		{
			WmShellCloseDelegate wmShellClose = WmShellClose;
			if (wmShellClose != null)
				wmShellClose (this, e);
		}

		/// <summary> Default processing for the Close event. </summary>
		/// <param name="source"> The widget, the Close event is assigned to. <see cref="XrwApplicationShell"/> </param>
		/// <returns> True to continue closing, false otherwise. </returns>
		public bool DefaultClose ()
		{
			if (_display == IntPtr.Zero)
				return true;

			foreach (XrwRectObj widget in _children)
			{
				widget.Hide ();
				widget.Dispose ();
			}
			_children.Clear();
			
			Dispose ();
			
			// Disconnect from X server.
			X11lib.XCloseDisplay (_display);
			_display = IntPtr.Zero;
			
			return true;
		}
		
		#endregion
		
	}
}

