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
    // ###################################################################################
    // ### D E L E G A T E S   F O R   X 1 1  E V E N T S
    // ####################################################################################
				
	/// <summary> Prototype the Expose event. </summary>
	/// <param name="source"> The widget, the Expose event is assigned to. <see cref="XrwRectObj"/> </param>
	/// <param name="e"> The event data. <see cref="XrwExposeEvent"/> </param>
	/// <remarks> Set XrwExposeEvent. Set result to nonzero to stop further event processing. </remarks>
	public delegate void ExposeDelegate (XrwRectObj source, XrwExposeEvent e);

	/// <summary> Prototype the KeyPress event. </summary>
	/// <param name="source"> The widget, the KeyPress event is assigned to. <see cref="XrwRectObj"/> </param>
	/// <param name="e"> The event data. <see cref="XrwKeyEvent"/> </param>
	/// <remarks> Set XrwKeyEvent. Set result to nonzero to stop further event processing. </remarks>
	public delegate void KeyPressDelegate (XrwRectObj source, XrwKeyEvent e);

	/// <summary> Prototype the KeyRelease event. </summary>
	/// <param name="source"> The widget, the KeyRelease event is assigned to. <see cref="XrwRectObj"/> </param>
	/// <param name="e"> The event data. <see cref="XrwKeyEvent"/> </param>
	/// <remarks> Set XrwKeyEvent. Set result to nonzero to stop further event processing. </remarks>
	public delegate void KeyReleaseDelegate (XrwRectObj source, XrwKeyEvent e);

	/// <summary> Prototype the ButtonPress event. </summary>
	/// <param name="source"> The widget, the ButtonPress event is assigned to. <see cref="XrwRectObj"/> </param>
	/// <param name="e"> The event data. <see cref="XawButtonEvent"/> </param>
	/// <remarks> Set XawButtonEvent. Set result to nonzero to stop further event processing. </remarks>
	public delegate void ButtonPressDelegate (XrwRectObj source, XrwButtonEvent e);

	/// <summary> Prototype the ButtonRelease event. </summary>
	/// <param name="source"> The widget, the ButtonRelease event is assigned to. <see cref="XrwRectObj"/> </param>
	/// <param name="e"> The event data. <see cref="XawButtonEvent"/> </param>
	/// <remarks> Set XawButtonEvent. Set result to nonzero to stop further event processing. </remarks>
	public delegate void ButtonReleaseDelegate (XrwRectObj source, XrwButtonEvent e);
	
	/// <summary> Prototype the FocusIn event. </summary>
	/// <param name="source"> The widget, the FocusIn event is assigned to. <see cref="XrwRectObj"/> </param>
	/// <param name="e"> The event data. <see cref="XawFocusChangeEvent"/> </param>
	/// <remarks> Set XrwFocusChangeEvent. Set result to nonzero to stop further event processing. </remarks>
	public delegate void FocusInDelegate (XrwRectObj source, XrwFocusChangeEvent e);
	
	/// <summary> Prototype the FocusOut event. </summary>
	/// <param name="source"> The widget, the FocusOut event is assigned to. <see cref="XrwRectObj"/> </param>
	/// <param name="e"> The event data. <see cref="XawFocusChangeEvent"/> </param>
	/// <remarks> Set XrwFocusChangeEvent. Set result to nonzero to stop further event processing. </remarks>
	public delegate void FocusOutDelegate (XrwRectObj source, XrwFocusChangeEvent e);
	
	/// <summary> Prototype the Enter event. </summary>
	/// <param name="source"> The widget, the Enter event is assigned to. <see cref="XrwRectObj"/> </param>
	/// <param name="e"> The event data. <see cref="XawCrossingEvent"/> </param>
	/// <remarks> Set XawCrossingEvent. Set result to nonzero to stop further event processing. </remarks>
	public delegate void EnterDelegate (XrwRectObj source, XrwCrossingEvent e);
	
	/// <summary> Prototype the Leave event. </summary>
	/// <param name="source"> The widget, the Leave event is assigned to. <see cref="XrwRectObj"/> </param>
	/// <param name="e"> The event data. <see cref="XawCrossingEvent"/> </param>
	/// <remarks> Set XawCrossingEvent. Set result to nonzero to stop further event processing. </remarks>
	public delegate void LeaveDelegate (XrwRectObj source, XrwCrossingEvent e);
	
	/// <summary> Prototype the Motion event. </summary>
	/// <param name="source"> The widget, the Motion event is assigned to. <see cref="XrwRectObj"/> </param>
	/// <param name="e"> The event data. <see cref="XrwMotionEvent"/> </param>
	/// <remarks> Set XrwMotionEvent. Set result to nonzero to stop further event processing. </remarks>
	public delegate void MotionDelegate (XrwRectObj source, XrwMotionEvent e);
		
	/// <summary> Prototype the DialogShellEnd event. </summary>
	/// <param name="source"> The widget, the DialogShellEnd event is assigned to. <see cref="XrwRectObj"/> </param>
	/// <param name="data"> The event data. <see cref="System.Object"/> </param>
	public delegate void DialogShellEndDelegate (XrwRectObj source, object data);
		
	/// <summary> Prototype the ApplicationClose event. </summary>
	/// <param name="source"> The widget, the ApplicationClose event is assigned to. <see cref="XrwRectObj"/> </param>
	/// <param name="e"> The event data. <see cref="XawClientMessageEvent"/> </param>
	/// <remarks> Set XawClientMessageEvent. Set result to nonzero to stop further event processing. </remarks>
	public delegate void WmShellCloseDelegate (XrwRectObj source, XrwClientMessageEvent e);

    // ###################################################################################
    // ### I N T E R N A L   D E L E G A T E S
    // ####################################################################################

	/// <summary> Prototype the SwitchedOff event. </summary>
	/// <param name="source"> The widget, the SwitchedOff event is assigned to. <see cref="XrwRectObj"/> </param>
	public delegate void SwitchedOffDelegate (XrwRectObj source);

	/// <summary> Prototype the SwitchedOn event. </summary>
	/// <param name="source"> The widget, the SwitchedOn event is assigned to. <see cref="XrwRectObj"/> </param>
	public delegate void SwitchedOnDelegate (XrwRectObj source);

	/// <summary> Prototype the SelectionChanged event. </summary>
	/// <param name="source"> The widget, the SelectionChanged event is assigned to. <see cref="XrwRectObj"/> </param>
	/// <param name="selected"> The child widget, the SelectionChange event sets visible. <see cref="XrwRectObj"/> </param>
	public delegate void SelectionChangedDelegate (XrwRectObj source, XrwRectObj selected);

	/// <summary> The windowless fundamental invisible object class. </summary>
	public class XrwObject : IDisposable
	{

        // ###############################################################################
        // ### C O N S T A N T S
        // ###############################################################################

        #region Constants

        /// <summary> The class name constant. </summary>
        public const String CLASS_NAME = "XrwObject";
		
        #endregion

		// ###############################################################################
        // ### A T T R I B U T E S
        // ###############################################################################

		#region Attributes
		
        #endregion
		
        // ###############################################################################
        // ### D E S T R U C T I O N
        // ###############################################################################

        #region Destruction

		/// <summary> IDisposable implementation. </summary>
		public virtual void Dispose ()
		{	
			if (XrwApplicationSettings.VERBOSE_OUTPUT_TO_CONSOLE)
				Console.WriteLine (CLASS_NAME + "::Dispose ()");
			
			DisposeByParent ();
		}

		/// <summary> Dispose by parent. </summary>
		public virtual void DisposeByParent ()
		{	;	}
		
		#endregion
		
        // ###############################################################################
        // ### P R O P E R T I E S
        // ###############################################################################

		#region Properties
		
		/// <summary> Determine whether object has an *** own *** window. </summary>
		public virtual bool HasOwnWindow
		{	get {	return false;	}
		}
		
		#endregion
		
        // ###############################################################################
        // ### M E T H O D S
        // ###############################################################################
		
		#region Methods
		
		#endregion
		
		# region Helper methods
		
		/// <summary> Send an expose event to the indicated window. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="receiverWindow"> The window to adress the expose event to. <see cref="IntPtr"/> </param>
		/// <param name="dispatcherWindow"> The window to send the expose event to. <see cref="IntPtr"/> </param>
		/// <returns> Nonzero on success, or zero otherwise (if the conversion to wire protocol format failed). <see cref="X11.TInt"/> </returns>
        public static TInt SendExposeEvent (IntPtr display, IntPtr receiverWindow, IntPtr dispatcherWindow)
		{

			XEvent       eE         = new XEvent();
			eE.ExposeEvent.count	= 0;
			eE.ExposeEvent.display	= display;
			eE.ExposeEvent.window	= receiverWindow;
			eE.ExposeEvent.type		= XEventName.Expose;
			
			return X11lib.XSendEvent (display, dispatcherWindow, (TBoolean)1, (TLong)EventMask.ExposureMask, ref eE);
		}
		
		#endregion
		
	}
	
}

