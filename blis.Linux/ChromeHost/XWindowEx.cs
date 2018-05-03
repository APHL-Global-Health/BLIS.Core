// --------------------------------------------------------------------------------------------------------------------
//<license>
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// </license>
// <note>
// blis project is licensed under MIT License. CefGlue may have additional licensing.
// This is a port from CefGlue.WindowsForms sample of CefGlue. Mostly provided as-is. 
// For more info: https://bitbucket.org/xilium/xilium.cefglue/wiki/Home
// </note>
// --------------------------------------------------------------------------------------------------------------------

#region Using
using XSharp;
using System;
using System.Drawing;
using System.Threading;
#endregion

namespace blis.Linux
{
	public class XWindowEx : XWindow
	{
		#region Variables
		private XEvent xevents;
		private bool xeventsRunner;
		#endregion

		#region Events
		public event ShapeHandler ShapeHandlerEvent;
		public event KeyPressHandler KeyPressHandlerEvent;
		public event KeyReleaseHandler KeyReleaseHandlerEvent;
		public event ButtonPressHandler ButtonPressHandlerEvent;
		public event ButtonReleaseHandler ButtonReleaseHandlerEvent;
		public event ExposeHandler ExposeHandlerEvent;
		public event EnterNotifyHandler EnterNotifyHandlerEvent;
		public event LeaveNotifyHandler LeaveNotifyHandlerEvent;
		public event MotionNotifyHandler MotionNotifyHandlerEvent;
		public event FocusInHandler FocusInHandlerEvent;
		public event FocusOutHandler FocusOutHandlerEvent;
		public event KeymapNotifyHandler KeymapNotifyHandlerEvent;
		public event GraphicsExposeHandler GraphicsExposeHandlerEvent;
		public event NoExposeHandler NoExposeHandlerEvent;
		public event VisibilityNotifyHandler VisibilityNotifyHandlerEvent;
		public event CreateNotifyHandler CreateNotifyHandlerEvent;
		public event DestroyNotifyHandler DestroyNotifyHandlerEvent;
		public event UnmapNotifyHandler UnmapNotifyHandlerEvent;
		public event MapNotifyHandler MapNotifyHandlerEvent;
		public event MapRequestHandler MapRequestHandlerEvent;
		public event ReparentNotifyHandler ReparentNotifyHandlerEvent;
		public event ConfigureNotifyHandler ConfigureNotifyHandlerEvent;
		public event ConfigureRequestHandler ConfigureRequestHandlerEvent;
		public event GravityNotifyHandler GravityNotifyHandlerEvent;
		public event ResizeRequestHandler ResizeRequestHandlerEvent;
		public event CirculateNotifyHandler CirculateNotifyHandlerEvent;
		public event CirculateRequestHandler CirculateRequestHandlerEvent;
		public event PropertyNotifyHandler PropertyNotifyHandlerEvent;
		public event SelectionClearHandler SelectionClearHandlerEvent;
		public event SelectionRequestHandler SelectionRequestHandlerEvent;
		public event SelectionNotifyHandler SelectionNotifyHandlerEvent;
		public event ColormapNotifyHandler ColormapNotifyHandlerEvent;
		public event ClientMessageHandler ClientMessageHandlerEvent;
		public event MappingNotifyHandler MappingNotifyHandlerEvent;
		#endregion

		#region Properties
		public XScreen Screen { get; private set; }
		public XPointer Pointer { get; private set; }
		public XGC GC { get; private set; }
		public Thread WindowsThread { get; private set; }
		#endregion

		#region Constructor
		public XWindowEx()
			: base(new XDisplay(IntPtr.Zero))
		{
			Initialize();
		}

		public XWindowEx(XDisplay dpy)
			: base(dpy)

		{
			Initialize();
		}

		public XWindowEx(XDisplay dpy, IntPtr handle)
			: base(dpy, handle)
		{
			Initialize();
		}

		public XWindowEx(XDisplay dpy, Rectangle geom)
			: base(dpy, geom)
		{
			Initialize();
		}

		public XWindowEx(XDisplay dpy, Rectangle geom, int border_width, int border_color, int background_color)
			: base(dpy, geom, border_width, border_color, background_color)
		{
			Initialize();
		}

		public XWindowEx(XDisplay dpy, XWindow parent, Rectangle geom, int border_width, int border_color, int background_color)
			: base(dpy, parent, geom, border_width, border_color, background_color)
		{
			Initialize();
		}
		#endregion

		#region Initialize
		private void Initialize()
		{
			Name = "blis";
			xeventsRunner = true;
			SelectInput(XEventMask.KeyPressMask
						| XEventMask.VisibilityChangeMask
						| XEventMask.KeyReleaseMask
						| XEventMask.ButtonPressMask
						| XEventMask.ButtonReleaseMask
						| XEventMask.EnterWindowMask
						| XEventMask.LeaveWindowMask
						| XEventMask.PointerMotionMask
						| XEventMask.Button1MotionMask
						| XEventMask.Button2MotionMask
						| XEventMask.Button3MotionMask
						| XEventMask.Button4MotionMask
						| XEventMask.Button5MotionMask
						| XEventMask.ButtonMotionMask
						| XEventMask.ExposureMask
						| XEventMask.StructureNotifyMask
						| XEventMask.FocusChangeMask
						| XEventMask.PropertyChangeMask
						| XEventMask.ColormapChangeMask
					   //| XEventMask.PointerMotionHintMask 
					   //| XEventMask.KeymapStateMask 
					   //| XEventMask.SubstructureNotifyMask 
					   //| XEventMask.SubstructureRedirectMask 
					   //| XEventMask.ResizeRedirectMask
					   );

			Screen = new XScreen(Display);
			Pointer = new XPointer(Display);
			GC = new XGC(Display);

			xevents = new XEvent(Display);
			xevents.ShapeHandlerEvent += (xevent, window) =>
			{
				if (ShapeHandlerEvent != null) ShapeHandlerEvent(xevent, window);
			};
			xevents.KeyPressHandlerEvent += (xevent, window, root, subwindow) =>
			{
				if (KeyPressHandlerEvent != null) KeyPressHandlerEvent(xevent, window, root, subwindow);
			};
			xevents.KeyReleaseHandlerEvent += (xevent, window, root, subwindow) =>
			{
				if (KeyReleaseHandlerEvent != null) KeyReleaseHandlerEvent(xevent, window, root, subwindow);
			};
			xevents.ButtonPressHandlerEvent += (xevent, window, root, subwindow) =>
			{
				if (ButtonPressHandlerEvent != null) ButtonPressHandlerEvent(xevent, window, root, subwindow);
			};
			xevents.ButtonReleaseHandlerEvent += (xevent, window, root, subwindow) =>
			{
				if (ButtonReleaseHandlerEvent != null) ButtonReleaseHandlerEvent(xevent, window, root, subwindow);
			};
			xevents.ExposeHandlerEvent += (xevent, window) =>
			{
				if (ExposeHandlerEvent != null) ExposeHandlerEvent(xevent, window);
			};
			xevents.EnterNotifyHandlerEvent += (xevent, window, root, subwindow) =>
			{
				if (EnterNotifyHandlerEvent != null) EnterNotifyHandlerEvent(xevent, window, root, subwindow);
			};
			xevents.LeaveNotifyHandlerEvent += (xevent, window, root, subwindow) =>
			{
				if (LeaveNotifyHandlerEvent != null) LeaveNotifyHandlerEvent(xevent, window, root, subwindow);
			};
			xevents.MotionNotifyHandlerEvent += (xevent, window, root, subwindow) =>
			{
				if (MotionNotifyHandlerEvent != null) MotionNotifyHandlerEvent(xevent, window, root, subwindow);
			};
			xevents.FocusInHandlerEvent += (xevent, window) =>
			{
				if (FocusInHandlerEvent != null) FocusInHandlerEvent(xevent, window);
			};
			xevents.FocusOutHandlerEvent += (xevent, window) =>
			{
				if (FocusOutHandlerEvent != null) FocusOutHandlerEvent(xevent, window);
			};
			xevents.KeymapNotifyHandlerEvent += (xevent, window) =>
			{
				if (KeymapNotifyHandlerEvent != null) KeymapNotifyHandlerEvent(xevent, window);
			};
			xevents.GraphicsExposeHandlerEvent += (xevent, window) =>
			{
				if (GraphicsExposeHandlerEvent != null) GraphicsExposeHandlerEvent(xevent, window);
			};
			xevents.NoExposeHandlerEvent += (xevent, window) =>
			{
				if (NoExposeHandlerEvent != null) NoExposeHandlerEvent(xevent, window);
			};
			xevents.VisibilityNotifyHandlerEvent += (xevent, window) =>
			{
				if (VisibilityNotifyHandlerEvent != null) VisibilityNotifyHandlerEvent(xevent, window);
			};
			xevents.CreateNotifyHandlerEvent += (xevent, window, root) =>
			{
				if (CreateNotifyHandlerEvent != null) CreateNotifyHandlerEvent(xevent, window, root);
			};
			xevents.DestroyNotifyHandlerEvent += (xevent, window) =>
			{
				if (DestroyNotifyHandlerEvent != null) DestroyNotifyHandlerEvent(xevent, window);
			};
			xevents.UnmapNotifyHandlerEvent += (xevent, window) =>
			{
				if (UnmapNotifyHandlerEvent != null) UnmapNotifyHandlerEvent(xevent, window);
			};
			xevents.MapNotifyHandlerEvent += (xevent, window) =>
			{
				if (MapNotifyHandlerEvent != null) MapNotifyHandlerEvent(xevent, window);
			};
			xevents.MapRequestHandlerEvent += (xevent, window, root) =>
			{
				if (MapRequestHandlerEvent != null) MapRequestHandlerEvent(xevent, window, root);
			};
			xevents.ReparentNotifyHandlerEvent += (xevent, window, root) =>
			{
				if (ReparentNotifyHandlerEvent != null) ReparentNotifyHandlerEvent(xevent, window, root);
			};
			xevents.ConfigureNotifyHandlerEvent += (xevent, window) =>
			{
				if (ConfigureNotifyHandlerEvent != null) ConfigureNotifyHandlerEvent(xevent, window);
			};
			xevents.ConfigureRequestHandlerEvent += (xevent, window) =>
			{
				if (ConfigureRequestHandlerEvent != null) ConfigureRequestHandlerEvent(xevent, window);
			};
			xevents.GravityNotifyHandlerEvent += (xevent, window) =>
			{
				if (GravityNotifyHandlerEvent != null) GravityNotifyHandlerEvent(xevent, window);
			};
			xevents.ResizeRequestHandlerEvent += (xevent, window) =>
			{
				if (ResizeRequestHandlerEvent != null) ResizeRequestHandlerEvent(xevent, window);
			};
			xevents.CirculateNotifyHandlerEvent += (xevent, window) =>
			{
				if (CirculateNotifyHandlerEvent != null) CirculateNotifyHandlerEvent(xevent, window);
			};
			xevents.CirculateRequestHandlerEvent += (xevent, window, root) =>
			{
				if (CirculateRequestHandlerEvent != null) CirculateRequestHandlerEvent(xevent, window, root);
			};
			xevents.PropertyNotifyHandlerEvent += (xevent, window) =>
			{
				if (PropertyNotifyHandlerEvent != null) PropertyNotifyHandlerEvent(xevent, window);
			};
			xevents.SelectionClearHandlerEvent += (xevent, window) =>
			{
				if (SelectionClearHandlerEvent != null) SelectionClearHandlerEvent(xevent, window);
			};
			xevents.SelectionRequestHandlerEvent += (xevent, window, root, subwindow) =>
			{
				if (SelectionRequestHandlerEvent != null) SelectionRequestHandlerEvent(xevent, window, root, subwindow);
			};
			xevents.SelectionNotifyHandlerEvent += (xevent, window, root) =>
			{
				if (SelectionNotifyHandlerEvent != null) SelectionNotifyHandlerEvent(xevent, window, root);
			};
			xevents.ColormapNotifyHandlerEvent += (xevent, window) =>
			{
				if (ColormapNotifyHandlerEvent != null) ColormapNotifyHandlerEvent(xevent, window);
			};
			xevents.ClientMessageHandlerEvent += (xevent, window) =>
			{
				if (ClientMessageHandlerEvent != null) ClientMessageHandlerEvent(xevent, window);
			};
			xevents.MappingNotifyHandlerEvent += (xevent, window) =>
			{
				if (MappingNotifyHandlerEvent != null) MappingNotifyHandlerEvent(xevent, window);
			};

			Map();

			WindowsThread = new Thread(() => { while (xeventsRunner) xevents.Refresh(); });
			WindowsThread.Start();

			OnInitialized();
		}
		#endregion

		#region OnInitialized
		public virtual void OnInitialized()
		{
		}
		#endregion

		#region Show | All
		public void Show()
		{

		}

		public void ShowAll()
		{
		}
		#endregion

		#region DestroyEx
		public void DestroyEx()
		{
			xeventsRunner = false;

			GC.Dispose();
			this.Dispose();
			xevents.Dispose();
			Screen.Dispose();
			Display.Dispose();

			if (WindowsThread != null)
			{
				if (WindowsThread.IsAlive)
				{
					try { WindowsThread.Join(1000); }
					catch { WindowsThread.Abort(); }
					WindowsThread = null;
				}
			}
		}
		#endregion

		#region ResizeEx
		public int ResizeEx(IntPtr handle, Size size)
		{
			return XResizeWindow(Display.Handle, handle, size.Width, size.Height);
		}
		#endregion
	}
}
