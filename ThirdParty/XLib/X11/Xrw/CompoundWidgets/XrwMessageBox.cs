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

using Utils;

namespace Xrw
{
	/// <summary> The windowed universal modal popup message box, that interact with the window manager. </summary>
	/// <remarks> The member attribute Window contains the *** own *** window pointer. </remarks>
	public class XrwMessageBox : XrwDialogShell
	{
		
        // ###############################################################################
        // ### C O N S T A N T S
        // ###############################################################################

        #region Constants

        /// <summary> The class name constant. </summary>
        public new const string		CLASS_NAME = "XrwMessageBox";
		
        #endregion

		// ###############################################################################
        // ### A T T R I B U T E S
        // ###############################################################################

		#region Attributes
		
		/// <summary> The message to display. </summary>
		private string					_message = "";
		
		/// <summary> The message box title. </summary>
		private string					_title = "";
		
		/// <summary> The message icon to display. </summary>
		private X11Graphic.StockIcon	_image = X11Graphic.StockIcon.None;
		
		/// <summary> The dialog shell result. </summary>
		private XrwDialogShell.Result	_result = XrwDialogShell.Result.None;
		
        #endregion
		
        // ###############################################################################
        // ### C O N S T R U C T I O N   A N D   I N I T I A L I Z A T I O N
        // ###############################################################################

        #region Construction
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="parent"> The parent (associated top-level) shell (an override shell can't be a top-level shell. <see cref="IntPtr"/> </param>
		/// <param name="message"> The message to display. <see cref="System.String"/> </param>
		/// <param name="title"> The title to display. <see cref="System.String"/> </param>
		/// <param name="icon"> The icon to display. <see cref="XrwBitmap.Icon"/> </param>
		public XrwMessageBox (XrwApplicationShell parent, string message, string title, X11Graphic.StockIcon icon)
			: base (parent)
		{
			_message	= message;
			_title		= title;
			_image		= icon;
			_appModal	= true;
			
			InitializeMessageBoxResources();
		}
		
		/// <summary> Initialize local ressources for all constructors. </summary>
		public void InitializeMessageBoxResources ()
		{
			XrwBox vboxMain = XrwBox.NewVBox (_display, _screenNumber, _window);
			vboxMain.BorderWidth = 0;
			vboxMain.VertSpacing = 0;
			AddChild (vboxMain);
			vboxMain.Show ();
			
			XrwBox hboxContentArea = XrwBox.NewHBox (_display, _screenNumber, _window);
			hboxContentArea.ExpandToAvailableHeight = true;
			vboxMain.AddChild (hboxContentArea);
			hboxContentArea.Show ();
			
			X11Graphic graphic = null;
			if (_image != X11Graphic.StockIcon.None)
				graphic = new X11Graphic (_display, _screenNumber, _image);
			XrwLabel _labelMessage = new XrwLabel   (_display, _screenNumber, _window, _message, graphic, false, null, false);
			_labelMessage.FrameType = TFrameType.Sunken;
			_labelMessage.FrameWidth = XrwTheme.InteractingFrameWidth;
			_labelMessage.HorzTextAlign = 0.0F;
			_labelMessage.ExpandToAvailableHeight = true;
			hboxContentArea.AddChild (_labelMessage);
			_labelMessage.Show ();

			XrwBox hboxActionArea = XrwBox.NewHBox (_display, _screenNumber, _window);
			hboxActionArea.BorderWidth = 2;
			hboxActionArea.ChildAlign = 1.0F;
			hboxActionArea.BorderColor = hboxActionArea.BackgroundColor;
			vboxMain.AddChild (hboxActionArea);
			hboxActionArea.Show ();
			
			X11Graphic cancelGraphic = XrwTheme.GetGraphic ( _display,_screenNumber, X11Graphic.StockIcon.Cancel16TrueColor);
			XrwCommand cbCancel = new XrwCommand (_display, _screenNumber, _window, "Cancel", cancelGraphic, true, null, false);
			cbCancel.ExpandToMaxSiblingWidth = true;
			cbCancel.HorzTextAlign = 0.5F;
			cbCancel.ButtonRelease += HandleCbCancelButtonRelease;
			hboxActionArea.AddChild (cbCancel);
			cbCancel.Show ();
			
			X11Graphic okGraphic = XrwTheme.GetGraphic ( _display,_screenNumber, X11Graphic.StockIcon.Ok16TrueColor);
			XrwCommand cbOk = new XrwCommand (_display, _screenNumber, _window, "OK", okGraphic, true, null, false);
			cbOk.ExpandToMaxSiblingWidth = true;
			cbOk.HorzTextAlign = 0.5F;
			cbOk.ButtonRelease += HandleCbOkButtonRelease;
			hboxActionArea.AddChild (cbOk);
			cbOk.Show ();

			CalculateChildLayout();
			_assignedSize = vboxMain.AssignedSize;
			_assignedSize.Width  += 2* _borderWidth;
			_assignedSize.Height += 2* _borderWidth;

			TPoint position = new TPoint (0, 0);
			MoveResize (ref position, ref _assignedSize);
			
			X11lib.XSetWMName (_display, _window, _title);
		}
		
        #endregion
		
        // ###############################################################################
        // ### D E S T R U C T I O N
        // ###############################################################################

        #region Destruction
		
        #endregion
		
        // ###############################################################################
        // ### P R O P E R T I E S
        // ###############################################################################
		
		#region Properties
		
 		#endregion
		
        // ###############################################################################
        // ### M E T H O D S
        // ###############################################################################
		
		#region Methods
		
		/// <summary> Run the message box modal. </summary>
		/// <returns> The pressed button. <see cref="XrwDialogShell.Result"/> </returns>
		/// <remarks> To use the message box non-modal register at least event handler to WmShellClose and to DialogShellEnd and call Show(). </remarks>
		public XrwDialogShell.Result Run ()
		{
			this.Show();
			
			// Take over the event loop processing.
			while (_result == XrwDialogShell.Result.None)
				ApplicationShell.DoEvent ();

			return _result;
		}
		
 		#endregion

        #region XrwTransientShell event handler override

		/// <summary> Handle the ClientMessage event. </summary>
		/// <param name="e"> The event data. <see cref="XrwClientMessageEvent"/> </param>
		/// <remarks> Set XawClientMessageEvent. Set result to nonzero to stop further event processing. </remarks>
		public override void OnWmClose (XrwClientMessageEvent e)
		{
			base.OnWmClose (e);
			this.DefaultClose ();

			// Stop event processing here!
			e.Result = 1;
			
			_result = XrwDialogShell.Result.Cancel;
			this.OnEnd (_result);
		}
		
		#endregion
		
        #region Event handler
		
		/// <summary> Handle the ButtonPress event. </summary>
		/// <param name="source"> The widget, the ButtonPress event is assigned to. <see cref="XrwRectObj"/> </param>
		/// <param name="e"> The event data. <see cref="XawButtonEvent"/> </param>
		/// <remarks> Set XawButtonEvent. Set result to nonzero to stop further event processing. </remarks>
		void HandleCbOkButtonRelease (XrwRectObj source, XrwButtonEvent e)
		{
			this.DefaultClose ();

			// Stop event processing here!
			e.Result = 1;
			
			_result = XrwDialogShell.Result.OK;
			this.OnEnd (_result);
		}

		/// <summary> Handle the ButtonPress event. </summary>
		/// <param name="source"> The widget, the ButtonPress event is assigned to. <see cref="XrwRectObj"/> </param>
		/// <param name="e"> The event data. <see cref="XawButtonEvent"/> </param>
		/// <remarks> Set XawButtonEvent. Set result to nonzero to stop further event processing. </remarks>
		void HandleCbCancelButtonRelease (XrwRectObj source, XrwButtonEvent e)
		{
			this.DefaultClose ();

			// Stop event processing here!
			e.Result = 1;
			
			_result = XrwDialogShell.Result.Cancel;
			this.OnEnd (_result);
		}
		
        #endregion
	}
}

