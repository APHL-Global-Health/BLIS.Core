using System;
using System.Collections.Generic;
using System.Drawing;

using Utils;

// Simplify X11lib application programming by the "Roma Widget Set".
using Xrw;

// New features:
// - Clean differentiation between border and frame.
// - New frame types "Chiseled" and "Ledged".
// - Maintenance of XrwExposeEvent.Result within all OnExpose calls.
// - XrwOverrideShell now bases on an independent window instead in an XrwApplicationShell sub-window.
// - New widgets XrwToggle and XrwRadio, XrwRadioBox, XrwStack and XrwNotebook.

/*
IDisposable (windowless)
 +--XrwObject (windowless)
     +---XrwRectObj (windowless)
          +---XrwVisibleRectObj (windowless)
               +---XrwCore (windowless)
                    +---XrwComposite (windowless)
                    |    +---XrwConstraint (windowless)
                    |    |    +---XrwBox (windowless)
                    |    |    |    +---XrwNotebook (windowless)
                    |    |    |    +---XrwRadioBox (windowless)
                    |    |    +---XrwStack (windowless)
                    |    +---XrwShell
                    |         +---XrwOverrideShell (windowed)
                    |         |    +---XrwSimpleMenu (windowed)
                    |         +---XrwWmShell
                    |              +---XrwApplicationShell (windowed)
                    |              +---XrwTransientShell (windowed)
                    |                   +---XrwDialogShell (windowed)
                    |                        +---XrwMessageBox (windowed)
                    +---XrwLabel (windowless)
                    |    +---XrwCommand (windowed)
                    |    |    +---XrwMenuButton (windowed)
                    |    +---XrwSme (windowless)
                    +---XrwToggle (windowed)
                    |    +---XrwRadio (windowed)
                    +---XrwPanel (windowless)
                    +---XrwSimple (windowed)
*/
namespace X11
{
	public class X11Window : XrwApplicationShell
	{

        // ###############################################################################
        // ### C O N S T A N T S
        // ###############################################################################

        #region Constants

        /// <summary> The class name constant. </summary>
		new const string CLASS_NAME = "X11Window";
		
		/// <summary> The file path to the application icon. </summary>
		const string APPICON_FILEPATH = "attention.bmp";
        
		#endregion

		// ###############################################################################
        // ### A T T R I B U T E S
        // ###############################################################################

		#region Attributes
		
		/// <summary> The status label widget. Managed by the application shell. </summary>
		XrwLabel		_labelStatus = null;
		
		/// <summary> The file menu. Self-managed. Must be disposed explicit. </summary>
		XrwSimpleMenu	_fileMenuShell = null;
		
        #endregion
		
        // ###############################################################################
        // ### M E T H O D S
        // ###############################################################################

		#region Methods
		
		public static void Main ()
		{
			X11Window appWindow = new X11Window();
			
			appWindow.Run ();
		}
		
		public void Run ()
		{
			InitializeApplicationShellResources ();
			
			X11.TPixel pixel = X11lib.XAllocParsedColorByName (_display, _screenNumber, "plum");
			X11lib.XSetForeground (_display, _gc, pixel);
			
			// Clear the window and bring it on top of the other windows.
			X11lib.XClearWindow (_display, _window);
			X11lib.XMapRaised   (_display, _window);
			
			XrwBox vboxMain = XrwBox.NewVBox (_display, _screenNumber, _window);
			vboxMain.BorderColor = vboxMain.BackgroundColor;
			vboxMain.VertSpacing = 0;
			vboxMain.FrameWidth  = 2;
			vboxMain.Show ();
			
			XrwBox hboxFileRibbon = XrwBox.NewHBox (_display, _screenNumber, _window);
			hboxFileRibbon.BorderWidth = 0;
			hboxFileRibbon.FrameType = XrwTheme.NonInteractingFrameType;
			hboxFileRibbon.ChildAlign = 0.0F;
			hboxFileRibbon.ExpandToAvailableHeight = true;
			
			// ----
			
			TPoint origin = new TPoint (20, 20);
			_fileMenuShell = new XrwSimpleMenu (this, ref origin);
			
			X11Graphic menuEntryGraphic = XrwTheme.GetGraphic ( _display,_screenNumber, X11Graphic.StockIcon.Information16TrueColor);
			XrwSme menuEntry1 = new XrwSme (_fileMenuShell.Display, _fileMenuShell.Screen, _fileMenuShell.Window, "File menu entry 1", menuEntryGraphic, true, null, false);
			menuEntry1.ButtonRelease += HandleMenuEntry1ButtonRelease;
			_fileMenuShell.AddChild (menuEntry1);
			XrwSme menuEntry2 = new XrwSme (_fileMenuShell.Display, _fileMenuShell.Screen, _fileMenuShell.Window, "File menu entry 2", menuEntryGraphic, true, null, false);
			menuEntry2.ButtonRelease += HandleMenuEntry2ButtonRelease;
			_fileMenuShell.AddChild (menuEntry2);

			_fileMenuShell.CalculateChildLayout ();
			_fileMenuShell.SetFixedWidth  (_fileMenuShell.AssignedSize.Width);
			_fileMenuShell.SetFixedHeight (_fileMenuShell.AssignedSize.Height);

			// ----
			
			XrwMenuButton commandFileMenu = new XrwMenuButton (_display, _screenNumber, _window, "File");
			commandFileMenu.FrameType = XrwTheme.NonInteractingFrameType;
			commandFileMenu.FrameWidth = XrwTheme.NonInteractingFrameWidth;
			commandFileMenu.ExpandToAvailableHeight = true;
			commandFileMenu.Menu = _fileMenuShell;
			hboxFileRibbon.AddChild (commandFileMenu);

			X11Graphic cbw1Graphic = XrwTheme.GetGraphic ( _display,_screenNumber, X11Graphic.StockIcon.Error16TrueColor);
			XrwCommand cbw1 = new XrwCommand (_display, _screenNumber, _window, "Close menu", cbw1Graphic, true, null, false);
			cbw1.FrameType  = XrwTheme.NonInteractingFrameType;
			cbw1.FrameWidth = XrwTheme.NonInteractingFrameWidth;
			cbw1.ExpandToAvailableHeight = true;
			cbw1.ButtonPress += HandleCloseMenuButtonPress;
			hboxFileRibbon.AddChild (cbw1);
			
			X11Graphic cbw2Graphic = XrwTheme.GetGraphic ( _display,_screenNumber, X11Graphic.StockIcon.Question16TrueColor);
			XrwCommand cbw2 = new XrwCommand (_display, _screenNumber, _window, "Message box", cbw2Graphic, true, null, false);
			cbw2.FrameType  = XrwTheme.NonInteractingFrameType;
			cbw2.FrameWidth = XrwTheme.NonInteractingFrameWidth;
			cbw2.ExpandToAvailableHeight = true;
			cbw2.ButtonRelease += HandleMessageBoxButtonRelease;
			hboxFileRibbon.AddChild (cbw2);

			X11Graphic cbw3Graphic = XrwTheme.GetGraphic ( _display,_screenNumber, X11Graphic.StockIcon.Warning16TrueColor);
			XrwCommand cbw3 = new XrwCommand (_display, _screenNumber, _window, "Close app", cbw3Graphic, true, null, false);
			cbw3.FrameType  = XrwTheme.NonInteractingFrameType;
			cbw3.FrameWidth = XrwTheme.NonInteractingFrameWidth;
			cbw3.ExpandToAvailableHeight = true;
			cbw3.ButtonRelease += HandleCloseButtonRelease;
			hboxFileRibbon.AddChild (cbw3);
			
			// ----
			
			XrwBox hboxToggleRibbon = XrwBox.NewHBox (_display, _screenNumber, _window);
			hboxToggleRibbon.BorderWidth = 0;
			hboxToggleRibbon.ChildAlign = 0.0F;
			hboxToggleRibbon.VertSpacing = 0;
			
			X11Graphic toggleOffGraphic = XrwTheme.GetGraphic ( _display,_screenNumber, X11Graphic.StockIcon.ToggleOff16TrueColor);
			X11Graphic toggleOnGraphic = XrwTheme.GetGraphic ( _display,_screenNumber, X11Graphic.StockIcon.ToggleOn16TrueColor);
			XrwToggle toggle1 = new XrwToggle (_display, _screenNumber, _window, "Toggle test 1", toggleOffGraphic, true, toggleOnGraphic, true);
			toggle1.ExpandToAvailableWidth = false;
			toggle1.FrameWidth = XrwTheme.InteractingFrameWidth;
			hboxToggleRibbon.AddChild (toggle1);
			
			XrwToggle toggle2 = new XrwToggle (_display, _screenNumber, _window, "Toggle test 2", toggleOffGraphic, true, toggleOnGraphic, true);
			toggle2.ExpandToAvailableWidth = false;
			toggle2.FrameWidth = XrwTheme.InteractingFrameWidth;
			hboxToggleRibbon.AddChild (toggle2);
			
			XrwToggle toggle3 = new XrwToggle (_display, _screenNumber, _window, "Toggle test 3", toggleOffGraphic, true, toggleOnGraphic, true);
			toggle3.ExpandToAvailableWidth = false;
			toggle3.FrameWidth = XrwTheme.InteractingFrameWidth;
			hboxToggleRibbon.AddChild (toggle3);

			// ----

			XrwRadioBox hboxRadioRibbon = XrwRadioBox.NewHRadioBox (_display, _screenNumber, _window);
			hboxRadioRibbon.BorderWidth = 0;
			hboxRadioRibbon.ChildAlign = 0.0F;
			hboxRadioRibbon.VertSpacing = 0;

			X11Graphic radioOffGraphic = XrwTheme.GetGraphic ( _display,_screenNumber, X11Graphic.StockIcon.RadioOff16TrueColor);
			X11Graphic radioOnGraphic = XrwTheme.GetGraphic ( _display,_screenNumber, X11Graphic.StockIcon.RadioOn16TrueColor);
			XrwRadio radio1 = new XrwRadio (_display, _screenNumber, _window, "Radio test 1", radioOffGraphic, true, radioOnGraphic, true);
			radio1.ExpandToAvailableWidth = false;
			radio1.FrameType  = XrwTheme.InteractingFrameType;
			radio1.FrameWidth = XrwTheme.InteractingFrameWidth;
			hboxRadioRibbon.AddChild (radio1);
			
			XrwRadio radio2 = new XrwRadio (_display, _screenNumber, _window, "Radio test 2", radioOffGraphic, true, radioOnGraphic, true);
			radio2.ExpandToAvailableWidth = false;
			radio2.FrameType  = XrwTheme.InteractingFrameType;
			radio2.FrameWidth = XrwTheme.InteractingFrameWidth;
			hboxRadioRibbon.AddChild (radio2);
			
			XrwRadio radio3 = new XrwRadio (_display, _screenNumber, _window, "Radio test 3", radioOffGraphic, true, radioOnGraphic, true);
			radio3.ExpandToAvailableWidth = false;
			radio3.FrameType  = XrwTheme.InteractingFrameType;
			radio3.FrameWidth = XrwTheme.InteractingFrameWidth;
			hboxRadioRibbon.AddChild (radio3);
			
			// ----

			XrwNotebook ribbonBar = XrwNotebook.NewTopTabedNotebook (_display, _screenNumber, _window);
			ribbonBar.FrameType = TFrameType.Sunken;
			ribbonBar.FrameWidth = 1;
			ribbonBar.AddChild (hboxFileRibbon);
			(ribbonBar.TabWidget (ribbonBar.CountPages - 1) as XrwRadio).Label = "File";
			
			ribbonBar.AddChild (hboxRadioRibbon);
			(ribbonBar.TabWidget (ribbonBar.CountPages - 1) as XrwRadio).Label = "Radio test";

			ribbonBar.AddChild (hboxToggleRibbon);
			(ribbonBar.TabWidget (ribbonBar.CountPages - 1) as XrwRadio).Label = "Toggle test";
			vboxMain.AddChild (ribbonBar);
			ribbonBar.Show ();
			
			/*
			// A panel is windowless! All expose events are routed to the parent widget.
			XrwPanel panel1   = new XrwPanel   (_display, _screenNumber, _window);
			panel1.FrameType = TFrameType.Sunken;
			panel1.FrameWidth = XrwTheme.FrameWidth;
			vboxMain.AddChild (panel1);
			panel1.Show ();
			*/

			// Simple is windowed! No expose events are routed to the parent widget.
			XrwSimple simple1 = new XrwSimple (_display, _screenNumber, _window);
			simple1.FrameType = TFrameType.Sunken;
			simple1.FrameWidth = 1; // XrwTheme.FrameWidth;
			simple1.ExpandToAvailableHeight = true;
			simple1.ExpandToAvailableWidth  = true;
			vboxMain.AddChild (simple1);
			simple1.Show ();
			
			X11Graphic labelGraphic = XrwTheme.GetGraphic ( _display,_screenNumber, X11Graphic.StockIcon.Information16TrueColor);
			_labelStatus = new XrwLabel   (_display, _screenNumber, _window, "Hallo App!", labelGraphic, true, labelGraphic, true);
			_labelStatus.FrameType = TFrameType.Sunken;
			_labelStatus.FrameWidth = 1; // XrwTheme.FrameWidth;
			_labelStatus.HorzTextAlign = 0.0F;
			_labelStatus.BorderWidth = 0;
			_labelStatus.VertTextAlign = 0.5F;
			_labelStatus.ExpandToAvailableWidth = true;
			vboxMain.AddChild (_labelStatus);
			_labelStatus.Show ();
			
			this.AddChild (vboxMain);
			
			// Register close event.
			this.WmShellClose += HandleApplicationClose;

			ApplicationFramework.WriteStatusText += HandleWriteStatus;
			
			X11lib.XSetWMName(_display, _window, "Hallo X11 from C#!");
			X11lib.XSetWMIconName (_display, _window, "X11 from C#");
			ApplicationFramework.SetWmShellIcon (this, APPICON_FILEPATH);

			Show ();
			RunMessageLoop();
		}
		
		#endregion
		
		#region Event handler

		/// <summary> Handle the ButtonPress event. </summary>
		/// <param name="e"> The event data. <see cref="XawButtonEvent"/> </param>
		/// <remarks> Set XawButtonEvent. Set result to nonzero to stop further event processing. </remarks>
		void HandleMenuEntry1ButtonRelease (XrwRectObj source, XrwButtonEvent e)
		{
			ApplicationFramework.WriteStatus ("MenuEntryButton_1");
			e.Result = 1;
		}

		/// <summary> Handle the ButtonRelease event. </summary>
		/// <param name="e"> The event data. <see cref="XawButtonEvent"/> </param>
		/// <remarks> Set XawButtonEvent. Set result to nonzero to stop further event processing. </remarks>
		void HandleMenuEntry2ButtonRelease (XrwRectObj source, XrwButtonEvent e)
		{
			ApplicationFramework.WriteStatus ("MenuEntryButton_2");
			e.Result = 1;
		}

		/// <summary> Handle the WriteStatus event. </summary>
		/// <param name="message"> The message to write to the status field. <see cref="System.String"/> </param>
		void HandleWriteStatus (string message)
		{
			_labelStatus.Label = message;
			
			// Ensure exposing.
			if (XrwObject.SendExposeEvent (_display, _labelStatus.Window, this.Window) == 0)
			{
				Console.WriteLine (CLASS_NAME + "::HandleWriteStatus () ERROR: Can not send expose event.");
			}
		}

		/// <summary> Handle the ButtonPress event. </summary>
		/// <param name="source"> The widget, the ButtonPress event is assigned to. <see cref="XrwRectObj"/> </param>
		/// <param name="e"> The event data. <see cref="XawButtonEvent"/> </param>
		/// <remarks> Set XawButtonEvent. Set result to nonzero to stop further event processing. </remarks>
		void HandleCloseMenuButtonPress (XrwRectObj source, XrwButtonEvent e)
		{
			X11lib.XSetInputFocus (_display, _window, X11lib.TRevertTo.RevertToParent, (TInt)0);			
			e.Result = 1;
		}
			
		/// <summary> Handle the ButtonPress event. </summary>
		/// <param name="source"> The widget, the ButtonPress event is assigned to. <see cref="XrwRectObj"/> </param>
		/// <param name="e"> The event data. <see cref="XawButtonEvent"/> </param>
		/// <remarks> Set XawButtonEvent. Set result to nonzero to stop further event processing. </remarks>
		void HandleMessageBoxButtonRelease (XrwRectObj source, XrwButtonEvent e)
		{
			if ((source is XrwCommand) && !(source as XrwCommand).Focused)
				return;
			
			XrwMessageBox messageBox = new XrwMessageBox (this, "Hallo an alle X11\nund Mono Develop\nFans da draußen!", "Mono Develop", X11Graphic.StockIcon.Information48TrueColor);
			ApplicationFramework.SetWmShellIcon (messageBox, APPICON_FILEPATH);
			
			this.AddTransientShell (messageBox);
			XrwDialogShell.Result result = messageBox.Run ();
			
			if (result == XrwDialogShell.Result.OK)
			{
				Console.WriteLine (CLASS_NAME + "::HandleMessageBoxButtonRelease () Message box closed with: OK");
				ApplicationFramework.WriteStatus ("Message box closed with: OK");
			}
			else
			{
				Console.WriteLine (CLASS_NAME + "::HandleMessageBoxButtonRelease () Message box closed with: Cancel");
				ApplicationFramework.WriteStatus ("Message box closed with: Cancel");
			}
			
			e.Result = 1;
		}
		
		/// <summary> Handle the ButtonPress event. </summary>
		/// <param name="source"> The widget, the ButtonPress event is assigned to. <see cref="XrwRectObj"/> </param>
		/// <param name="e"> The event data. <see cref="XawButtonEvent"/> </param>
		/// <remarks> Set XawButtonEvent. Set result to nonzero to stop further event processing. </remarks>
		void HandleCloseButtonRelease (XrwRectObj source, XrwButtonEvent e)
		{
			if ((source is XrwCommand) && !(source as XrwCommand).Focused)
				return;
			
			this.DefaultClose();
			e.Result = 1;
		}

		/// <summary> Prototype the ApplicationClose event. </summary>
		/// <param name="source"> The widget, the ApplicationClose event is assigned to. <see cref="XrwRectObj"/> </param>
		/// <param name="e"> The event data. <see cref="XawClientMessageEvent"/> </param>
		/// <remarks> Set XawClientMessageEvent. Set result to nonzero to stop further event processing. </remarks>
		void HandleApplicationClose (XrwRectObj source, XrwClientMessageEvent e)
		{
			this.RemoveChild (_fileMenuShell);
			_fileMenuShell.Dispose ();
			
			this.DefaultClose();
			e.Result = 1;
		}
		
		#endregion

	}
}