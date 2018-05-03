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
	
	/// <summary> The "Roma Widget Set" minimal theming. </summary>
	public static class XrwTheme
	{
		private struct Graphic
		{
			public IntPtr     Display;
			public TInt       ScreenNumber;
			public X11Graphic.StockIcon Stock;
			public X11Graphic Image;
			
			public Graphic (IntPtr display, TInt screenNumber, X11Graphic.StockIcon stock, X11Graphic image)
			{
				Display = display;
				ScreenNumber = screenNumber;
				Stock = stock;
				Image = image;
			}
		}
		
		private class GraphicList : List<Graphic>, IDisposable
		{
		
        	// ---------------------------------------------------------------------------
        	// --- C O N S T A N T S
        	// ---------------------------------------------------------------------------

	        #region Constants
	
	        /// <summary> The class name constant. </summary>
	        public const String CLASS_NAME = "GraphicList";
			
	        #endregion

        	// ---------------------------------------------------------------------------
        	// --- A T T R I B U T E S
        	// ---------------------------------------------------------------------------

	        #region Attributes
			
			 // Track whether Dispose has been called.
	        private bool	_disposed = false;
			
	        #endregion

        	// ---------------------------------------------------------------------------
        	// --- M E T H O D S
        	// ---------------------------------------------------------------------------

	        #region Methods
	
			public X11Graphic Graphic (IntPtr display, TInt screenNumber, X11Graphic.StockIcon stock)
			{
				for (int count = 0; count < this.Count; count++)
				{
					Graphic gr1 = this[count];
					if (gr1.Stock == stock && gr1.Display == display && gr1.ScreenNumber == screenNumber)
					{
						return gr1.Image;
					}
				}
				X11Graphic image = new X11Graphic (display, screenNumber, stock);
				if (image == null)
					return null;
				
				Graphic gr2 = new Graphic (display, screenNumber, stock, image);
				this.Add (gr2);
				return image;
			}
			
	        #endregion

        	// ---------------------------------------------------------------------------
        	// --- D E S T R U C T I O N
        	// ---------------------------------------------------------------------------

	        #region Destruction
			
			/// <summary> Implement IDisposable. </summary>
	        /// <remarks> Do not make this method virtual. A derived class should not be able to override this method. </remarks>
	        public void Dispose()
	        {
				if (XrwApplicationSettings.VERBOSE_OUTPUT_TO_CONSOLE)
					Console.WriteLine (CLASS_NAME + ":: Dispose ()");
			
	            Dispose(true);
	            // This object will be cleaned up by the Dispose method.
	            // Therefore, you should call GC.SupressFinalize to
	            // take this object off the finalization queue
	            // and prevent finalization code for this object
	            // from executing a second time.
	            GC.SuppressFinalize(this);
	        }
		
			/// <summary> Execute disposing for user calls and GC calls. </summary>
			/// <param name="disposing"> Determine whether called by user (true) or GC (false). <see cref="System.Boolean"/> </param>
	        protected virtual void Dispose(bool disposing)
	        {
	            // Check to see if Dispose has already been called.
	            if(_disposed)
	            {
	                // If disposing equals true, dispose all managed and unmanaged resources.
	                if(disposing)
	                {
	                    // Dispose managed resources.
	                    ;
	                }
	
	                // Call the appropriate methods to clean up unmanaged resources here.
	                // If disposing is false, only the following code is executed.
					for (int count = 0; count < this.Count; count++)
					{
						Graphic gr = this[count];
						if (gr.Image != null)
						{
							gr.Image.Dispose ();
							gr.Image = null;
						}
					}
	
	                // Note disposing has been done.
	                _disposed = true;
	            }
	        }
		
		
	        /// <summary> Finalize. </summary>
	        /// <remarks> This destructor will run only if the Dispose method does not get called. It gives the class
	        /// the opportunity to finalize. Do not provide destructors in types derived from this class. </remarks>
			~GraphicList()
	        {
	            // Do not re-create Dispose clean-up code here.
	            // Calling Dispose(false) is optimal in terms of readability and maintainability.
	            Dispose(false);
	        }
			
			#endregion
		}
		

        // ###############################################################################
        // ### A T T R I B U T E S
        // ###############################################################################

        #region Private attributes

		private static GraphicList Graphics					= null;
		
        #endregion
		
        #region Public attributes

		/// <summary> Highlight color for focusable widgets (e. g. CrwCommand, XreToggle, ...) on *** mouse over ***. </summary>
		public static string     FocusedColor				= X11ColorNames.Gray75;
		
		/// <summary> General widget background color. </summary>
		public static string     BackGroundColor			= X11ColorNames.Gray80;
		
		/// <summary> Dark shadow color for 3D frame effects. </summary>
		public static string     DarkShadowColor			= X11ColorNames.Gray60;
		
		/// <summary> Light shadow color for 3D frame effects. </summary>
		public static string     LightShadowColor			= X11ColorNames.Gray92;
		
		/// <summary> General widget border color (surrounding the frame, if any, and the client area). </summary>
		public static string     OuterBorderColor			= X11ColorNames.Gray40;
		
		/// <summary> General text color. </summary>
		public static string     TextColor					= X11ColorNames.Black;
		
		/// <summary> General border width for simple widgets. </summary>
		public static int        BorderWidth				= 0;
		
		public static int        ShellBorderWidth			= 0;
		public static int        CompositeBorderWidth		= 0;
		
		/// <summary> General frame type for widgets without user interaction. </summary>
		public static TFrameType NonInteractingFrameType	= TFrameType.None;
		
		/// <summary> General border width for widgets without user interaction. </summary>
		public static int        NonInteractingFrameWidth	= 0;
		
		/// <summary> General frame type for widgets with user interaction. </summary>
		public static TFrameType InteractingFrameType		= TFrameType.Raised;
		
		/// <summary> General border width for widgets with user interaction. </summary>
		public static int        InteractingFrameWidth		= 2;
		
		
        #endregion
		
        // ###############################################################################
        // ### M E T H O D S
        // ###############################################################################
		
		#region Methods
		
		public static X11Graphic GetGraphic (IntPtr display, TInt screenNumber, X11Graphic.StockIcon stock)
		{
			if (Graphics == null)
				Graphics = new GraphicList ();
			return Graphics.Graphic (display, screenNumber, stock);
		}
		
		public static void FreeGraphic ()
		{
			if (Graphics != null)
				Graphics.Dispose ();
			Graphics = null;
		}
		#endregion		
		
	}
}
