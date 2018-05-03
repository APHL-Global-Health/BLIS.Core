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
using System.Drawing;
using System.Runtime.InteropServices;

namespace X11
{
	
	/// <summary> The windowless base class to provide (color and transparent) bitmap graphic convenient. </summary>
	public class X11Graphic : IDisposable
	{

		/// <summary> Predefined images for icons. </summary>
		public enum StockIcon
		{
			/// <summary> No bitmap. </summary>
			None,
			
			/// <summary> Attention icon, 16 x 16 pixel @ true color. </summary>
			Attention16TrueColor,
			
			/// <summary> Attention icon, 32 x 32 pixel @ true color. </summary>
			Attention32TrueColor,
			
			/// <summary> Attention icon, 48 x 48 pixel @ true color. </summary>
			Attention48TrueColor,
			
			/// <summary> Cancel icon, 16 x 16 pixel @ true color. </summary>
			Cancel16TrueColor,
			
			/// <summary> Cancel icon, 32 x 32 pixel @ true color. </summary>
			Cancel32TrueColor,
			
			/// <summary> Cancel icon, 48 x 48 pixel @ true color. </summary>
			Cancel48TrueColor,
			
			/// <summary> Error icon, 16 x 16 pixel @ true color. </summary>
			Error16TrueColor,
			
			/// <summary> Error icon, 32 x 32 pixel @ true color. </summary>
			Error32TrueColor,
			
			/// <summary> Error icon, 48 x 48 pixel @ true color. </summary>
			Error48TrueColor,
			
			/// <summary> Information icon, 16 x 16 pixel @ true color. </summary>
			Information16TrueColor,
			
			/// <summary> Information icon, 32 x 32 pixel @ true color. </summary>
			Information32TrueColor,
			
			/// <summary> Information icon, 48 x 48 pixel @ true color. </summary>
			Information48TrueColor,
			
			/// <summary> Question icon, 16 x 16 pixel @ true color. </summary>
			Ok16TrueColor,
			
			/// <summary> Question icon, 16 x 16 pixel @ true color. </summary>
			Question16TrueColor,
			
			/// <summary> Question icon, 32 x 32 pixel @ true color. </summary>
			Question32TrueColor,
			
			/// <summary> Warning icon, 16 x 16 pixel @ true color. </summary>
			Warning16TrueColor,
			
			/// <summary> Warning icon, 32 x 32 pixel @ true color. </summary>
			Warning32TrueColor,
			
			/// <summary> Toggle off icon, 16 x 16 pixel @ true color. </summary>
			ToggleOff16TrueColor,
			
			/// <summary> Toggle on icon, 16 x 16 pixel @ true color. </summary>
			ToggleOn16TrueColor,
			
			/// <summary> Radio off icon, 16 x 16 pixel @ true color. </summary>
			RadioOff16TrueColor,
			
			/// <summary> Radio on icon, 16 x 16 pixel @ true color. </summary>
			RadioOn16TrueColor
		}
		
        // ###############################################################################
        // ### C O N S T A N T S
        // ###############################################################################

        #region Constants

        /// <summary> The class name constant. </summary>
        public const string	CLASS_NAME = "XrwGraphic";

		/// <summary> Specifies the quantum of a scanline (8, 16, or 32). In other words, the start of one scanline is
		/// separated in client memory from the start of the next scanline by an integer multiple of this many bits. </summary>
		private const TInt      IMAGE_PADDING = (TInt)8; // Scanlines must start on word boundaries.
		
		private const TInt      ASSUME_CONTIGUOUS = (TInt)0;
        #endregion

		// ###############################################################################
        // ### A T T R I B U T E S
        // ###############################################################################

		#region Attributes
		
		/// <summary> The X11 display pointer. </summary>
		protected IntPtr		_display;
		
		/// <summary> The X11 screen number. </summary>
		protected TInt			_screenNumber;
		
		/// <summary> The bitmap width. </summary>
		private int				_width			= 0;
		
		/// <summary> The bitmap height. </summary>
		private int				_height			= 0;
		
		/// <summary> The depth (number of planes) of the graphic - that holds color pixel information. </summary>
		private int 		    _graphicDepth	= 1;
		
		/// <summary> The depth (number of planes) of the clip mask - that holds binary information only. </summary>
		private int 		    _clipDepth		= 1;
		
		/// <summary> The X11 image of the graphic. </summary>
		private IntPtr			_graphicXImage	= IntPtr.Zero;
		
		/// <summary> The X11 image of the transparency mask. </summary>
		private IntPtr			_transpXImage	= IntPtr.Zero;
		
		/// <summary> The X11 pixmap of the transparency mask. </summary>
		private IntPtr			_transpXPixmap	= IntPtr.Zero;
			
        #endregion
		
        // ###############################################################################
        // ### C O N S T R U C T I O N   A N D   I N I T I A L I Z A T I O N
        // ###############################################################################

        #region Construction
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="icon"> The left bitmap to display. <see cref="XrwBitmap.Icon"/> </param>
		public X11Graphic (IntPtr display, TInt screenNumber, StockIcon icon)
		{
			// Check arguments.
			if (display == IntPtr.Zero)
				throw new ArgumentNullException ("display");
			if (icon == X11Graphic.StockIcon.None)
				throw new ArgumentNullException ("icon");
			
			_display = display;
			_screenNumber = screenNumber;

			// Load bitmap.
			Bitmap bmp = null;
			try
			{
				System.IO.Stream bmpStream = null;
				
				if (icon == X11Graphic.StockIcon.Attention16TrueColor)
					bmpStream = new System.IO.MemoryStream (X11Resources.IMAGE_ATTENTION_16_TRUECOLOR, false);
				else if (icon == X11Graphic.StockIcon.Attention32TrueColor)
					bmpStream = new System.IO.MemoryStream (X11Resources.IMAGE_ATTENTION_32_TRUECOLOR, false);
				else if (icon == X11Graphic.StockIcon.Attention48TrueColor)
					bmpStream = new System.IO.MemoryStream (X11Resources.IMAGE_ATTENTION_48_TRUECOLOR, false);
				
				else if (icon == X11Graphic.StockIcon.Cancel16TrueColor)
					bmpStream = new System.IO.MemoryStream (X11Resources.IMAGE_CANCEL_16_TRUECOLOR, false);
				else if (icon == X11Graphic.StockIcon.Cancel32TrueColor)
					bmpStream = new System.IO.MemoryStream (X11Resources.IMAGE_CANCEL_32_TRUECOLOR, false);
				else if (icon == X11Graphic.StockIcon.Cancel48TrueColor)
					bmpStream = new System.IO.MemoryStream (X11Resources.IMAGE_CANCEL_48_TRUECOLOR, false);
				
				else if (icon == X11Graphic.StockIcon.Error16TrueColor)
					bmpStream = new System.IO.MemoryStream (X11Resources.IMAGE_ERROR_16_TRUECOLOR, false);
				else if (icon == X11Graphic.StockIcon.Error32TrueColor)
					bmpStream = new System.IO.MemoryStream (X11Resources.IMAGE_ERROR_32_TRUECOLOR, false);
				else if (icon == X11Graphic.StockIcon.Error48TrueColor)
					bmpStream = new System.IO.MemoryStream (X11Resources.IMAGE_ERROR_48_TRUECOLOR, false);
				
				else if (icon == X11Graphic.StockIcon.Information16TrueColor)
					bmpStream = new System.IO.MemoryStream (X11Resources.IMAGE_INFO_16_TRUECOLOR, false);
				else if (icon == X11Graphic.StockIcon.Information32TrueColor)
					bmpStream = new System.IO.MemoryStream (X11Resources.IMAGE_INFO_32_TRUECOLOR, false);
				else if (icon == X11Graphic.StockIcon.Information48TrueColor)
					bmpStream = new System.IO.MemoryStream (X11Resources.IMAGE_INFO_48_TRUECOLOR, false);
				
				else if (icon == X11Graphic.StockIcon.Ok16TrueColor)
					bmpStream = new System.IO.MemoryStream (X11Resources.IMAGE_OK_16_TRUECOLOR, false);
					
				else if (icon == X11Graphic.StockIcon.Question16TrueColor)
					bmpStream = new System.IO.MemoryStream (X11Resources.IMAGE_QUESTION_16_TRUECOLOR, false);
				else if (icon == X11Graphic.StockIcon.Question32TrueColor)
					bmpStream = new System.IO.MemoryStream (X11Resources.IMAGE_QUESTION_32_TRUECOLOR, false);
				
				else if (icon == X11Graphic.StockIcon.Warning16TrueColor)
					bmpStream = new System.IO.MemoryStream (X11Resources.IMAGE_WARNING_16_TRUECOLOR, false);
				else if (icon == X11Graphic.StockIcon.Warning32TrueColor)
					bmpStream = new System.IO.MemoryStream (X11Resources.IMAGE_WARNING_32_TRUECOLOR, false);

				else if (icon == X11Graphic.StockIcon.ToggleOff16TrueColor)
					bmpStream = new System.IO.MemoryStream (X11Resources.IMAGE_TOGGLEOFF_16_TRUECOLOR, false);
				else if (icon == X11Graphic.StockIcon.ToggleOn16TrueColor)
					bmpStream = new System.IO.MemoryStream (X11Resources.IMAGE_TOGGLEON_16_TRUECOLOR, false);
				else if (icon == X11Graphic.StockIcon.RadioOff16TrueColor)
					bmpStream = new System.IO.MemoryStream (X11Resources.IMAGE_RADIOOFF_16_TRUECOLOR, false);
				else if (icon == X11Graphic.StockIcon.RadioOn16TrueColor)
					bmpStream = new System.IO.MemoryStream (X11Resources.IMAGE_RADIOON_16_TRUECOLOR, false);
				
				bmp = new Bitmap (bmpStream);
				bmpStream.Close ();
			}
			catch (Exception e)
			{
				throw e;
			}
			if (bmp == null)
				throw new OperationCanceledException ("Failed to create bitmap from '" + icon.ToString() + "'.");
			
			InitializeGraphicRessources (bmp);
		}
		
		// Tested: OK // XrwBitmap fi = new XrwBitmap (_display, X11lib.XDefaultVisual(_display, _screenNumber), "32x32.bmp");
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="display">The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="screenNumber"> The appropriate screen number on the host server. <see cref="System.Int32"/> </param>
		/// <param name="filename"> The path of the graphic file. <see cref="System.String"/> </param>
		public X11Graphic (IntPtr display, TInt screenNumber, string filename)
		{
			// Check arguments.
			if (display == IntPtr.Zero)
				throw new ArgumentNullException ("display");
			if (string.IsNullOrEmpty (filename))
				throw new ArgumentNullException ("filename");
			
			_display = display;
			_screenNumber = screenNumber;

			// Load bitmap.
			Bitmap bmp = null;
			try
			{
				bmp = new Bitmap (filename);
			}
			catch (Exception e)
			{
				throw e;
			}
			if (bmp == null)
				throw new OperationCanceledException ("Failed to create bitmap from '" + filename + "'.");
			
			InitializeGraphicRessources (bmp);
		}
		
		/// <summary> Initialize local ressources for all constructors. </summary>
		/// <param name="bmp"> The bitmap associated with this graphic. <see cref="Bitmap"/> </param>
		private void InitializeGraphicRessources (Bitmap bmp)
		{
			IntPtr visual   = X11lib.XDefaultVisual (_display, _screenNumber);
			if (visual == IntPtr.Zero)
				throw new OperationCanceledException ("Failed to investigate default visual.");

			_width        = bmp.Width;
			_height       = bmp.Height;
			
			// Prepare bitmap conversion.
			int		colorPixelBytes		= 4;
			int     maskLineBytes		= (int)((_width + (int)IMAGE_PADDING - 1) / (int)IMAGE_PADDING);
			
			_graphicDepth = (int) X11lib.XDefaultDepth (_display, _screenNumber);
		    if (_graphicDepth > 16)
				colorPixelBytes   = 4;
			else if (_graphicDepth > 8)
				colorPixelBytes   = 2;
			else
				colorPixelBytes   = 1;

			// ### Allocate bitmap conversion data.
			// The bitmap color data. Use a temporary managed byte array for speed.
			byte[]	graphicData			= new byte[_height * _width * colorPixelBytes];
			// The bitmap transparency data. Use a temporary managed byte array for speed.
			byte[]	maskData			= new byte[_height * maskLineBytes];
			// Quick access to current line's color pixel.
			int		graphicPixelIndex	= 0;
			// Quick access to current line's mask pixel.
			int		maskPixelIndex		= 0;
			// Reduce slow calls to bmp.GetPixel (x,y).
			Color   pixelColor			= Color.Black;
			// Determine whether transparency is required.
			bool	transparency		= false;

			// ### Convert bitmap.
			for (int y = 0; y < _height; y++)
			{
				for (int x = 0; x < _width; x++)
				{
					graphicPixelIndex = (y * _width + x) << 2;
					maskPixelIndex    = y * maskLineBytes + (x >> 3);
					pixelColor = bmp.GetPixel (x,y);

					graphicData[graphicPixelIndex + 0] = pixelColor.B; // B
					graphicData[graphicPixelIndex + 1] = pixelColor.G; // G
					graphicData[graphicPixelIndex + 2] = pixelColor.R; // R
					graphicData[graphicPixelIndex + 3] = pixelColor.A; // A
						
					if (pixelColor. B == 255 && pixelColor.G == 255 && pixelColor.R == 255)
					{
						byte summand = (byte)(1<<(x % 8));
						maskData[maskPixelIndex] = (byte)(maskData[maskPixelIndex] + summand);
						transparency               = true;
					}
				}
			}
			
			// ### Create XImage.
			// The bitmap color data.
			IntPtr	graphicDataHandle = Marshal.AllocHGlobal(graphicData.Length);
			// Allocate not movable memory.
			Marshal.Copy (graphicData, 0, graphicDataHandle, graphicData.Length);
			// Client side XImage storage.
			_graphicXImage = X11lib.XCreateImage (_display, visual, (TUint)_graphicDepth,
			                                      X11lib.TImageFormat.ZPixmap, (TInt)0,
			                                      graphicDataHandle, (TUint)_width, (TUint)_height,
			                                      IMAGE_PADDING, ASSUME_CONTIGUOUS);
			if (_graphicXImage == IntPtr.Zero)
				throw new OperationCanceledException ("Image creation for graphic failed.");
			
			if (transparency == true)
			{
				IntPtr	maskDataHandle = Marshal.AllocHGlobal(maskData.Length);				// The bitmap bitmap transparency data.
				Marshal.Copy (maskData, 0, maskDataHandle, maskData.Length);				// Allocate not movable memory.
				// Client side storage.
				_transpXImage = X11lib.XCreateImage (_display, visual, (TUint)_clipDepth, X11lib.TImageFormat.XYBitmap, (TInt)0,
				                                   maskDataHandle, (TUint)_width, (TUint)_height, IMAGE_PADDING, ASSUME_CONTIGUOUS);
				if (_transpXImage == IntPtr.Zero)
					throw new OperationCanceledException ("Image creation for transparency mask failed.");
			}
		}
		
        #endregion
		
        // ###############################################################################
        // ### D E S T R U C T I O N
        // ###############################################################################

        #region Destruction
		
		/// <summary> IDisposable implementation. </summary>
		public void Dispose ()
		{
			// Console.WriteLine (CLASS_NAME + "::Dispose ()");
			
			if (_graphicXImage != IntPtr.Zero)
			{
				// Note: The destroy procedure (_XImage.f.destroy_image), that this macro calls,
				// frees both - the image structure and the data pointed to by the image structure.
				X11lib.XDestroyImage (_graphicXImage);
				_graphicXImage = IntPtr.Zero;
			}
			if (_transpXImage != IntPtr.Zero)
			{
				// Note: The destroy procedure (_XImage.f.destroy_image), that this macro calls,
				// frees both - the image structure and the data pointed to by the image structure.
				X11lib.XDestroyImage (_transpXImage);
				_transpXImage = IntPtr.Zero;
			}
			if (_transpXPixmap != IntPtr.Zero)
			{
				X11lib.XFreePixmap  (_display, _transpXPixmap);
				_transpXPixmap = IntPtr.Zero;
			}
		}
		
		#endregion
		
        // ###############################################################################
        // ### P R O P E R T I E S
        // ###############################################################################

		#region Properties
		
		/// <summary> Get the bitmap width. </summary>
		public int Width
		{
			get { return _width; }
		}
		
		/// <summary> Get the bitmap height. </summary>
		public int Height
		{
			get { return _height; }
		}
		
		/// <summary> Get the X11 graphic image. </summary>
		public IntPtr GraphicImage
		{
			get { return _graphicXImage; }
		}
		
		/// <summary> Get the X11 Mask image. Can be null. </summary>
		public IntPtr MaskImage
		{
			get { return _transpXImage; }
		}
		
		/// <summary> Get the depth (number of planes) of the graphic. </summary>
		public int GraphicDepth
		{
			get { return _graphicDepth; }
		}

		#endregion
		
        // ###############################################################################
        // ### M E T H O D S
        // ###############################################################################

		#region Methods
		
		/// <summary> Draw the image in the indicated window, using the indicated graphics context. </summary>
		/// <param name="window"> The window to draw the pitmap on. <see cref="System.IntPtr"/> </param>
		/// <param name="gc"> The crapchics context to use for drawing. <see cref="System.IntPtr"/> </param>
		/// <param name="destX"> The x coordinate, which is relative to the origin of the window and is the coordinate of the subimage. <see cref="TInt"/> </param>
		/// <param name="destY"> The y coordinate, which is relative to the origin of the window and is the coordinate of the subimage. <see cref="TInt"/> </param>
		private void DrawUnfinished (IntPtr window, IntPtr gc, TInt dstX, TInt dstY)
		{
			// This is an alternative drawing approach, but i didn't get it working.
			IntPtr clipPixmap = X11lib.XCreatePixmap (_display, window, (TUint)_width, (TUint)_height, (TUint)_clipDepth);
			IntPtr clipGc     = X11lib.XCreateGC (_display, clipPixmap, (TUlong)0, IntPtr.Zero);
			X11lib.XPutImage    (_display, clipPixmap, clipGc, _transpXImage, (TInt)0, (TInt)0, (TInt)0, (TInt)0, (TUint)_width, (TUint)_height);
			
			X11lib.XSetFunction (_display, gc, X11lib.XGCFunction.GXand);
			X11lib.XSetBackground (_display, gc, (TPixel)1);
			X11lib.XSetForeground (_display, gc, (TPixel)0);
			
			X11lib.XCopyPlane (_display, clipPixmap, window, gc, (TInt)0, (TInt)0, (TUint)_width, (TUint)_height, (TInt)dstX, (TInt)dstY, (TUlong)1);
		}
		
		/// <summary> Provide a bitmap, containing the intransparent graphic, that can be used independent from this class. </summary>
		/// <param name="display"> The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="window"> The target window to create the pixmap for. <see cref="IntPtr"/> </param>
		/// <returns> The (server side) pixmap (that must be feed) on success, or IntPtr.Zero otherwise. <see cref="IntPtr"/> </returns>
		public IntPtr CreateIndependentGraphicPixmap (IntPtr display, IntPtr window)
		{
			if (_graphicXImage == IntPtr.Zero)
				return IntPtr.Zero;
			
			IntPtr pixmap	= X11lib.XCreatePixmap (display, window, (TUint)_width, (TUint)_height, (TUint)_graphicDepth);
			IntPtr pixmapGc	= X11lib.XCreateGC (display, window, (TUlong)0, IntPtr.Zero);
			if (pixmapGc != IntPtr.Zero)
			{
				X11lib.XPutImage (display, pixmap, pixmapGc, _graphicXImage, (TInt)0, (TInt)0, (TInt)0, (TInt)0, (TUint)_width, (TUint)_height);
				// Console.WriteLine (CLASS_NAME + "::CreateIndependentGraphicPixmap () Delete graphic image GC.");
				X11lib.XFreeGC   (display, pixmapGc);
				pixmapGc = IntPtr.Zero;
			}	
			else
			{
				Console.WriteLine (CLASS_NAME + "::CreateIndependentGraphicPixmap () ERROR: Can not create graphics context for graphic pixmap.");
			}
			return pixmap;
		}
		
		/// <summary> Provide a bitmap, containing the transparency mask, that can be used independent from this class. </summary>
		/// <param name="display"> The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="window"> The target window to create the pixmap for. <see cref="IntPtr"/> </param>
		/// <returns> The (server side) pixmap (that must be feed) on success, or IntPtr.Zero otherwise. <see cref="IntPtr"/> </returns>
		public IntPtr CreateIndependentMaskPixmap (IntPtr display, IntPtr window)
		{
			if (_transpXImage == IntPtr.Zero)
				return IntPtr.Zero;
			
			IntPtr pixmap	= X11lib.XCreatePixmap (display, window, (TUint)_width, (TUint)_height, (TUint)_clipDepth);
			IntPtr pixmapGc	= X11lib.XCreateGC (display, pixmap, (TUlong)0, IntPtr.Zero);
			if (pixmapGc != IntPtr.Zero)
			{
				X11lib.XPutImage (display, pixmap, pixmapGc, _transpXImage, (TInt)0, (TInt)0, (TInt)0, (TInt)0, (TUint)_width, (TUint)_height);
				// Console.WriteLine (CLASS_NAME + "::CreateIndependentMaskPixmap () Delete transparency mask image GC.");
				X11lib.XFreeGC   (display, pixmapGc);
				pixmapGc = IntPtr.Zero;
			}	
			else
			{
				Console.WriteLine (CLASS_NAME + "::CreateIndependentMaskPixmap () ERROR: Can not create graphics context for mask pixmap.");
			}
			return pixmap;
		}
		
		/// <summary> Provide a bitmap, containing the transparent graphic, that can be used independent from this class. </summary>
		/// <param name="display"> The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="window"> The target window to create the pixmap for. <see cref="IntPtr"/> </param>
		/// <param name="backgroundColorPixel"> The background color behind any transparent pixel. <see cref="TPixel"/> </param>
		/// <param name="maskPixmap"> The mask pixmap to distinguish transparent from intransparent pixel. <see cref="IntPtr"/> </param>
		/// <returns> The (server side) pixmap (that must be feed) on success, or IntPtr.Zero otherwise. <see cref="IntPtr"/> </returns>
		public IntPtr CreateIndependentPixmap (IntPtr display, IntPtr window, TPixel backgroundColorPixel, IntPtr maskPixmap)
		{
			if (_graphicXImage == IntPtr.Zero)
				return IntPtr.Zero;
			
			IntPtr pixmap	= X11lib.XCreatePixmap (display, window, (TUint)_width, (TUint)_height, (TUint)_graphicDepth);
			
			// Fill pixmap with background color.
			IntPtr bgGc	= X11lib.XCreateGC (display, window, (TUlong)0, IntPtr.Zero);
			X11lib.XSetForeground (display, bgGc, backgroundColorPixel);
			X11lib.XFillRectangle (display, pixmap, bgGc, (TInt)0, (TInt)0, (TUint)_width, (TUint)_height);
			X11lib.XFreeGC (display, bgGc);
			bgGc = IntPtr.Zero;
			
			// Overlay the image.
			IntPtr pixmapGc	= X11lib.XCreateGC (display, window, (TUlong)0, IntPtr.Zero);
			if (pixmapGc != IntPtr.Zero)
			{
				if (maskPixmap != IntPtr.Zero)
				{
					// Prepare the clipping graphics context.
					IntPtr graphicGc     = X11lib.XCreateGC (display, window, (TUlong)0, IntPtr.Zero);
					if (graphicGc != IntPtr.Zero)
					{
						X11lib.XSetClipMask (display, graphicGc, maskPixmap);
						X11lib.XSetClipOrigin (display, graphicGc, (TInt)0, (TInt)0);
						
						// Draw graphic using the clipping graphics context.
						X11lib.XPutImage (display, pixmap, graphicGc, _graphicXImage, (TInt)0, (TInt)0,
						                  (TInt)0, (TInt)0, (TUint)_width, (TUint)_height);
						
						// Restore previous behaviour and clean up. 
						X11lib.XSetClipMask (display, graphicGc, IntPtr.Zero);
						X11lib.XSetClipOrigin (display, graphicGc, (TInt)0, (TInt)0);
						// Console.WriteLine (CLASS_NAME + "::Draw () Delete clipping image GC.");
						X11lib.XFreeGC (display, graphicGc);
						graphicGc = IntPtr.Zero;
					}
					else
					{
						Console.WriteLine (CLASS_NAME + "::Draw () ERROR: Can not create graphics context for transparency application.");
					}
				}
				else
				{
					X11lib.XPutImage (display, pixmap, pixmapGc, _graphicXImage, (TInt)0, (TInt)0, (TInt)0, (TInt)0, (TUint)_width, (TUint)_height);
					// Console.WriteLine (CLASS_NAME + "::CreateIndependentGraphicPixmap () Delete graphic image GC.");
					X11lib.XFreeGC   (display, pixmapGc);
					pixmapGc = IntPtr.Zero;
				}
			}	
			else
			{
				Console.WriteLine (CLASS_NAME + "::CreateIndependentGraphicPixmap () ERROR: Can not create graphics context for graphic pixmap.");
			}
			return pixmap;
		}
		
		/// <summary> Draw the image in the indicated window, using the indicated graphics context. </summary>
		/// <param name="window"> The window to draw the pitmap on. <see cref="System.IntPtr"/> </param>
		/// <param name="gc"> The crapchics context to use for drawing. <see cref="System.IntPtr"/> </param>
		/// <param name="destX"> The x coordinate, which is relative to the origin of the window and is the coordinate of the subimage. <see cref="TInt"/> </param>
		/// <param name="destY"> The y coordinate, which is relative to the origin of the window and is the coordinate of the subimage. <see cref="TInt"/> </param>
		public void Draw (IntPtr window, IntPtr gc, TInt dstX, TInt dstY)
		{
			// Possible optimization: Keep the graphicGc between the exposure events.
			// Problem: Every graphic allocates a graphics context (limited server resource).
			if (_transpXImage != IntPtr.Zero)
			{
				// Prepare the clip mask (for transparency) that must be a XPixmap.
				if (_transpXPixmap == IntPtr.Zero)
				{
					// Server side storage.
					_transpXPixmap = X11lib.XCreatePixmap (_display, window, (TUint)_width, (TUint)_height, (TUint)_clipDepth);
					IntPtr maskGc = X11lib.XCreateGC (_display, _transpXPixmap, (TUlong)0, IntPtr.Zero);
					if (maskGc != IntPtr.Zero)
					{
						X11lib.XPutImage    (_display, _transpXPixmap, maskGc, _transpXImage, (TInt)0, (TInt)0, (TInt)0, (TInt)0, (TUint)_width, (TUint)_height);
						// Console.WriteLine (CLASS_NAME + "::Draw () Delete transparency mask image GC.");
						X11lib.XFreeGC      (_display, maskGc);
						maskGc = IntPtr.Zero;
					}
					else
					{
						Console.WriteLine (CLASS_NAME + "::Draw () ERROR: Can not create graphics context for mask pixmap.");
					}
				}
				
				// Prepare the clipping graphics context.
				IntPtr graphicGc     = X11lib.XCreateGC (_display, window, (TUlong)0, IntPtr.Zero);
				if (graphicGc != IntPtr.Zero)
				{
					X11lib.XSetClipMask (_display, graphicGc, _transpXPixmap);
					X11lib.XSetClipOrigin (_display, graphicGc, dstX, dstY);
					
					// Draw graphic using the clipping graphics context.
					X11lib.XPutImage (_display, window, graphicGc, _graphicXImage, (TInt)0, (TInt)0, (TInt)dstX, (TInt)dstY, (TUint)_width, (TUint)_height);
					
					// Restore previous behaviour and clean up. 
					X11lib.XSetClipMask (_display, graphicGc, IntPtr.Zero);
					X11lib.XSetClipOrigin (_display, graphicGc, (TInt)0, (TInt)0);
					// Console.WriteLine (CLASS_NAME + "::Draw () Delete clipping image GC.");
					X11lib.XFreeGC (_display, graphicGc);
					graphicGc = IntPtr.Zero;
				}
				else
				{
					Console.WriteLine (CLASS_NAME + "::Draw () ERROR: Can not create graphics context for transparency application.");
				}
			}
			else
				X11lib.XPutImage(_display, window, gc, _graphicXImage, 0, 0, dstX, dstY, (TUint)_width, (TUint)_height);
		}

		#endregion

	}
}

