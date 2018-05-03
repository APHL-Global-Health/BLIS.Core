// ==================
// The X11 C# wrapper
// ==================

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
using System.Runtime.InteropServices;

// ==========================================================================================================
// Type mapping:
// ==========================================================================================================
// Xlib / X11 / Xt		C#				C / C++
// ----------------------------------------------------------------------------------------------------------
// Widget	    		System.IntPtr;
// XtPointer    		System.IntPtr;
// XFloat      			System.Single; // X11 32 Bit: 4 Bytes:
// XDouble     			System.Double; // X11 32 Bit: 8 Bytes:

namespace X11
{
	// ==========================================================================================================
	// Type mapping:
	// ==========================================================================================================
	// Xlib / X11 / Xt							C / C++
	// ----------------------------------------------------------------------------------------------------------
	public enum TChar		: sbyte		{	};  // X11    32 Bit: 1 Byte:                  -127 to 127
	public enum TUchar		: byte		{	};  // X11    32 Bit: 1 Byte:                     0 to 255
	public enum TWchar		: short		{	};  // X11    32 Bit: 2 Byte:                -32767 to 32767
	public enum TShort		: short		{	};  // X11    32 Bit: 2 Byte:                -32767 to 32767
	public enum TUshort		: ushort	{	};  // X11    32 Bit: 2 Byte:                     0 to 65535
	public enum TInt		: int		{	};  // X11 64/32 Bit: 4 Bytes:       -2.147.483.648 to 2.147.483.647
	public enum TUint		: uint		{	};  // X11 64/32 Bit: 4 Bytes:                    0 to 4294967295
	//public enum TLong		: int		{	};  // X11    32 Bit: 4 Bytes:                    0 to 65535
	public enum TLong		: long		{	};  // X11 64    Bit: 8 Bytes: -9223372036854775807 to 9223372036854775807
	//public enum TUlong		: uint		{	};  // X11    32 Bit: 4 Bytes:                    0 to 4294967295
	public enum TUlong		: ulong		{	};  // X11 64    Bit: 8 Bytes:                    0 to 18446744073709551615
	public enum TLonglong	: long		{	};  // X11 64/32 Bit: 8 Bytes: -9223372036854775807 to 9223372036854775807
	public enum TUlonglong	: ulong		{	};  // X11 64/32 Bit: 8 Bytes:                    0 to 18446744073709551615
	//public enum TFloat	: float		{	};  // X11    32 Bit: 4 Byte:              -1.5E-45 to -3.4E+38 with a precision of 7 decimal digits
	//public enum TDouble	: double	{	};  // X11    32 Bit: 8 Byte:             -5.0E-324 to -1.7E+308 with a precision of 15-16 decimal digits
	public enum TBoolean	: sbyte		{	};  // X11    32 Bit: 1 Byte:                  -127 to 127
	//public enum TPixel	: uint		{	};  // X11    32 Bit: 4 Bytes:                    0 to 4294967295
	public enum TPixel		: ulong		{	};  // X11 64    Bit: 8 Bytes:                    0 to 18446744073709551615
	
	public enum TPixmap		: int		{	};  // X11 64/32 Bit: 4 Bytes:               -32767 to 32767
	
	public enum XtEnum		: byte		{	};  // X11    32 Bit: 1 Byte:                     0 to 255
	public enum XCardinal	: uint		{	};  // X11    32 Bit: 4 Bytes:          -2147483647 to 2147483647
	public enum XDimension	: ushort	{	};  // X11    32 Bit: 2 Byte:                     0 to 65535
	public enum XPosition	: short		{	};  // X11    32 Bit: 2 Byte:                -32767 to 32767
	//public enum XtArgVal	: int		{	};  // X11    32 Bit: 4 Bytes:                    0 to 65535
	public enum XtArgVal	: long		{	};  // X11 64    Bit: 4 Bytes:                    0 to 65535
	//public enum XtVersionType : uint	{	};  // X11    32 Bit: 4 Bytes:                    0 to 4294967295
	public enum XtVersionType : ulong	{	};  // X11 64    Bit: 8 Bytes:                    0 to 18446744073709551615
	
	public enum XrmName		: int		{	};  // X11    32 Bit: 4 Bytes:               -32767 to 32767
	public enum XrmClass	: int		{	};  // X11    32 Bit: 4 Bytes:               -32767 to 32767
	
	//public enum XtValueMask : uint		{	};  // X11    32 Bit: 4 Bytes:                    0 to 4294967295
	public enum XtValueMask : ulong	{	};  // X11 64    Bit: 8 Bytes:                    0 to 18446744073709551615

	
	public class X11lib
	{
		[DllImport("XlibWrapper.so")]
		extern public static void WrapXTest (IntPtr dpy, IntPtr gc);

		[DllImport("XlibWrapper.so")]
		extern public static void WrapXGetGCValues (IntPtr dpy, IntPtr gc, uint flags, ref XGCValues xgcv);
		
		[DllImport("XlibWrapper.so")]
		extern public static void WrapMinimalXaw ();
			
		[Flags]
		public enum GCattributemask : long
		{
			GCFunction						= (1L<<0),
			GCPlaneMask						= (1L<<1),
			GCForeground					= (1L<<2),
			GCBackground					= (1L<<3),
			GCLineWidth						= (1L<<4),
			GCLineStyle						= (1L<<5),
			GCCapStyle						= (1L<<6),
			GCJoinStyle						= (1L<<7),
			GCFillStyle						= (1L<<8),
			GCFillRule						= (1L<<9),
			GCTile							= (1L<<10),
			GCStipple						= (1L<<11),
			GCTileStipXOrigin				= (1L<<12),
			GCTileStipYOrigin				= (1L<<13),
			GCFont							= (1L<<14),
			GCSubwindowMode					= (1L<<15),
			GCGraphicsExposures				= (1L<<16),
			GCClipXOrigin					= (1L<<17),
			GCClipYOrigin					= (1L<<18),
			GCClipMask						= (1L<<19),
			GCDashOffset					= (1L<<20),
			GCDashList						= (1L<<21),
			GCArcMode						= (1L<<22)
		}

		public enum WindowClass : uint
		{
			CopyFromParent					= 0,
			InputOutput						= 1,
			InputOnly						= 2
		}
	
		// Tested: O.K.
		public enum TRevertTo : int
		{
			RevertToNone					= 0,
			RevertToPointerRoot				= 1,
			RevertToParent					= 2
		}
		
		public enum TImageFormat : int
		{
			XYBitmap						= 0,
			XYPixmap						= 1,
			ZPixmap							= 2
		}
		
		// Tested: O.K.
		[Flags]
		public enum WindowAttributeMask : uint
		{
			CWBackPixmap					= (1<<0),
			CWBackPixel						= (1<<1),
			CWBorderPixmap					= (1<<2),
			CWBorderPixel					= (1<<3),
			CWBitGravity					= (1<<4),
			CWWinGravity					= (1<<5),
			CWBackingStore					= (1<<6),
			CWBackingPlanes					= (1<<7),
			CWBackingPixel					= (1<<8),
			CWOverrideRedirect				= (1<<9),
			CWSaveUnder						= (1<<10),
			CWEventMask						= (1<<11),
			CWDontPropagate					= (1<<12),
			CWColormap						= (1<<13),
			CWCursor						= (1<<14)
		}
		
		public struct XWindowAttributes
		{
			public TInt						x, y;					/* location of window */
			public TInt						width, height;			/* width and height of window */
			public TInt						border_width;			/* border width of window */
			public TInt						depth;					/* depth of window */
			public IntPtr					visual;					/* Visual: the associated visual structure */
			public IntPtr					root;					/* Window: root of screen containing window */
			public TInt						cls;					/* InputOutput, InputOnly*/
			public TInt						bit_gravity;			/* one of the bit gravity values */
			public TInt						win_gravity;			/* one of the window gravity values */
			public TInt						backing_store;			/* NotUseful, WhenMapped, Always */
			public TUlong					backing_planes;			/* planes to be preserved if possible */
			public TUlong					backing_pixel;			/* value to be used when restoring planes */
			public TBoolean					save_under;				/* boolean, should bits under be saved? */
			public IntPtr					colormap;				/* Colormap: color map to be associated with window */
			public TBoolean					map_installed;			/* boolean, is color map currently installed*/
			public TInt						map_state;				/* IsUnmapped, IsUnviewable, IsViewable */
			public TLong					all_event_masks;		/* set of events all people have interest in*/
			public TLong					your_event_mask;		/* my event mask */
			public TLong					do_not_propagate_mask;	/* set of events that should not propagate */
			public TBoolean					override_redirect;		/* boolean value for override-redirect */
			public IntPtr					screen;					/* Screen: back pointer to correct screen */
		}
		
		// See: Chapter 4. Window Attributes
		// Tested: O.K.
		public struct XSetWindowAttributes
		{
			public IntPtr					background_pixmap;		/* background, None, or ParentRelative */
			public TPixel					background_pixel;		/* background pixel */ 
			public IntPtr					border_pixmap;			/* border of the window or CopyFromParent */ 
			public TPixel					border_pixel;			/* border pixel value */ 
			public TInt						bit_gravity;			/* one of bit gravity values */ 
			public TInt						win_gravity;			/* one of the window gravity values */
			public TInt						backing_store;			/* NotUseful, WhenMapped, Always */
			public TUlong					backing_planes;			/* planes to be preserved if possible */ 
			public TUlong					backing_pixel;			/* value to use in restoring planes */ 
			public TBoolean					save_under;				/* should bits under be saved? (popups) */
			public TLong					event_mask;				/* set of events that should be saved */
			public TLong					do_not_propagate_mask;	/* set of events that should not propagate */ 
			public TBoolean					override_redirect;		/* boolean value for override_redirect */ 
			public IntPtr					colormap;				/* color map to be associated with window */
			public IntPtr					cursor;					/* cursor to be displayed (or None) */
		
		}
		
		[Flags]
		public enum XGCValueMask :uint
		{
			GCFunction			= (1<<0),
			GCPlaneMask			= (1<<1),
			GCForeground		= (1<<2),
			GCBackground		= (1<<3),
			GCLineWidth			= (1<<4),
			GCLineStyle			= (1<<5),
			GCCapStyle			= (1<<6),
			GCJoinStyle			= (1<<7),
			GCFillStyle			= (1<<8),
			GCFillRule			= (1<<9),
			GCTile				= (1<<10),
			GCStipple			= (1<<11),
			GCTileStipXOrigin	= (1<<12),
			GCTileStipYOrigin	= (1<<13),
			GCFont				= (1<<14),
			GCSubwindowMode		= (1<<15),
			GCGraphicsExposures	= (1<<16),
			GCClipXOrigin		= (1<<17),
			GCClipYOrigin		= (1<<18),
			GCClipMask			= (1<<19),
			GCDashOffset		= (1<<20),
			GCDashList			= (1<<21),
			GCArcMode			= (1<<22),
		}
		
		public enum XGCFunction : int
		{
			GXclear				= 0x0,       /* 0 */
			GXand				= 0x1,       /* src AND dst */
			GXandReverse		= 0x2,       /* src AND NOT dst */
			GXcopy				= 0x3,       /* src */
			GXandInverted		= 0x4,       /* NOT src AND dst */
			GXnoop				= 0x5,       /* dst */
			GXxor				= 0x6,       /* src XOR dst */
			GXor				= 0x7,       /* src OR dst */
			GXnor				= 0x8,       /* NOT src AND NOT dst */
			GXequiv				= 0x9,       /* NOT src XOR dst */
			GXinvert			= 0xa,       /* NOT dst */
			GXorReverse			= 0xb,       /* src OR NOT dst */
			GXcopyInverted		= 0xc,       /* NOT src */
			GXorInverted		= 0xd,       /* NOT src OR dst */
			GXnand				= 0xe,       /* NOT src OR NOT dst */
			GXset				= 0xf        /* 1 */
		}
		
		public struct XGCValues
		{
			public XGCFunction	function;			/* logical operation */
			public TPixel		plane_mask;			/* plane mask */
			public TPixel		foreground;			/* foreground pixel */
			public TPixel		background;			/* background pixel */
			public TInt			line_width;			/* line width (in pixels) */
			public TInt			line_style;			/* LineSolid, LineOnOffDash, LineDoubleDash */
			public TInt			cap_style;			/* CapNotLast, CapButt, CapRound, CapProjecting */
			public TInt			join_style;			/* JoinMiter, JoinRound, JoinBevel */
			public TInt			fill_style;			/* FillSolid, FillTiled, FillStippled FillOpaqueStippled*/
			public TInt			fill_rule;			/* EvenOddRule, WindingRule */
			public TInt			arc_mode;			/* ArcChord, ArcPieSlice */
			public IntPtr		tile;				/* tile pixmap for tiling operations */
			public IntPtr		stipple;			/* stipple 1 plane pixmap for stippling */
			public TInt			ts_x_origin;		/* offset for tile or stipple operations */
			public TInt			ts_y_origin;
			public IntPtr		font;				/* default text font for text operations */
			public TInt			subwindow_mode;		/* ClipByChildren, IncludeInferiors */
			public TBoolean		graphics_exposures;	/* boolean, should exposures be generated */
			public TInt			clip_x_origin;		/* origin for clipping */
			public TInt			clip_y_origin;
			public IntPtr		clip_mask;			/* bitmap clipping; other calls for rects */
			public TInt			dash_offset;		/* patterned/dashed line information */
			public TChar		dashes;
		}
		
		public enum CursorFontShape : uint
		{
			XC_X_cursor				= 0,
			XC_arrow				= 2,
			XC_based_arrow_down		= 4,
			XC_based_arrow_up		= 6,
			XC_boat					= 8,
			XC_bogosity				= 10,
			XC_bottom_left_corner	= 12,
			XC_bottom_right_corner	= 14,
			XC_bottom_side			= 16,
			XC_bottom_tee			= 18,
			XC_box_spiral			= 20,
			XC_center_ptr			= 22,
			XC_circle				= 24,
			XC_clock				= 26,
			XC_coffee_mug			= 28,
			XC_cross				= 30,
			XC_cross_reverse		= 32,
			XC_crosshair			= 34,
			XC_diamond_cross		= 36,
			XC_dot					= 38,
			XC_dot_box_mask			= 40,
			XC_double_arrow			= 42,
			XC_draft_large			= 44,
			XC_draft_small			= 46,
			XC_draped_box			= 48,
			XC_exchange				= 50,
			XC_fleur				= 52,
			XC_gobbler				= 54,
			XC_gumby				= 56,
			XC_hand1				= 58,
			XC_hand2				= 60,
			XC_heart				= 62,
			XC_icon					= 64,
			XC_iron_cross			= 66,
			XC_left_ptr				= 68,
			XC_left_side			= 70,
			XC_left_tee				= 72,
			XC_leftbutton			= 74,
			XC_ll_angle				= 76,
			XC_lr_angle				= 78,
			XC_man					= 80,
			XC_middlebutton			= 82,
			XC_mouse				= 84,
			XC_pencil				= 86,
			XC_pirate				= 88,
			XC_plus					= 90,
			XC_question_arrow		= 92,
			XC_right_ptr			= 94,
			XC_right_side			= 96,
			XC_right_tee			= 98,
			XC_rightbutton			= 100,
			XC_rtl_logo				= 102,
			XC_sailboat				= 104,
			XC_sb_down_arrow		= 106,
			XC_sb_h_double_arrow	= 108,
			XC_sb_left_arrow		= 110,
			XC_sb_right_arrow		= 112,
			XC_sb_up_arrow			= 114,
			XC_sb_v_double_arrow	= 116,
			XC_shuttle				= 118,
			XC_sizing				= 120,
			XC_spider				= 122,
			XC_spraycan				= 124,
			XC_star					= 126,
			XC_target				= 128,
			XC_tcross				= 130,
			XC_top_left_arrow		= 132,
			XC_top_left_corner		= 134,
			XC_top_right_corner		= 136,
			XC_top_side				= 138,
			XC_top_tee				= 140,
			XC_trek					= 142,
			XC_ul_angle				= 144,
			XC_umbrella				= 146,
			XC_ur_angle				= 148,
			XC_watch				= 150,
			XC_xterm				= 152,
		}
		
		
		[StructLayout(LayoutKind.Sequential)]
		public struct XCharStruct
		{
			public X11.TShort lbearing;			/* origin to left edge of raster */
			public X11.TShort rbearing;			/* origin to right edge of raster */
			public X11.TShort width;			/* advance to next char's origin */
			public X11.TShort ascent;			/* baseline to top edge of raster */
			public X11.TShort descent;			/* baseline to bottom edge of raster */
			public X11.TUchar attributes;		/* per char flags (not predefined) */
		}


		[StructLayout(LayoutKind.Sequential)]
		public struct XChar2b /* normal 16 bit characters are two bytes */
		{
		    public X11.TUchar byte1;
		    public X11.TUchar byte2;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct XColor
		{
			public TPixel pixel;				/* pixel value */
			public X11.TUchar red, green, blue;	/* rgb values */
			public X11.TUchar flags;			/* DoRed, DoGreen, DoBlue */	
			public X11.TUchar pad;
			
			public const X11.TUchar DoRed   = (X11.TUchar)1;
			public const X11.TUchar DoGreen = (X11.TUchar)2;
			public const X11.TUchar DoBlue  = (X11.TUchar)4;
		};

		public static XColor NewXColor(TPixel pixel, TUchar r, TUchar g, TUchar b, TUchar flags, TUchar pad)
		{
			XColor result = new XColor();
			result.pixel = pixel;
			result.red = r;
			result.green = g;
			result.blue = b;
			result.flags = flags;
			result.pad = pad;
			
			return result;
		}
		
		/// <summary> Internal memory mapping structure for XClassHint structure. </summary>
		/// <remarks> First structure element is on offset 0. This can be used to free the structute itself. </remarks>
		[StructLayout(LayoutKind.Sequential)]
		private struct _XClassHint
		{
			/// <summary> The application name (might be changed during runtime). </summary>
			/// <remarks> Must be freed separately. </remarks>
			public IntPtr res_name;
			/// <summary> The application class name (should be constant during runtime). </summary>
			/// <remarks> Must be freed separately. </remarks>
			public IntPtr res_class;
			
			public static _XClassHint Zero = new _XClassHint ();
		}
		
		[StructLayout(LayoutKind.Sequential)]
		public struct XClassHint
		{
			[MarshalAs(UnmanagedType.LPStr)] public string res_name;
			[MarshalAs(UnmanagedType.LPStr)] public string res_class;
			
			public static XClassHint Zero = new XClassHint ();
		}
		
		// Tested: O.K.
        [StructLayout(LayoutKind.Sequential)]
		public struct XTextProperty
		{
			public	IntPtr		val;			/* property data */
			public	IntPtr		encoding;		/* type of property */
			public	TInt		format;			/* 8, 16, or 32 */
			public	TUlong		nitems;			/* number of items in value */
		}
		
		// Tested: OK (for flags, icon_pixmap, icon_x, icon_y, icon_mask)
		[StructLayout(LayoutKind.Sequential)]
		public struct XWMHints
		{
			public	XWMHintMask	flags;			/* marks which fields in this structure are defined */
			public	TBoolean	input;			/* does this application rely on the window manager to get keyboard input? */
			public	TInt		initial_state;	/*	WithdrawnState	0,	NormalState	1,	IconicState	3 */
			public	IntPtr		icon_pixmap;	/* Pixmap: pixmap to be used as icon */
			public	IntPtr		icon_window;	/* Window: window to be used as icon */
			public	TInt		icon_x, icon_y;	/* initial position of icon */
			public	IntPtr		icon_mask;		/* Pixmap: pixmap to be used as mask for icon_pixmap */
			public	IntPtr		window_group;	/* XID: id of related window group */
		}
		
		public enum XWMHintMask : int

		{
			InputHint			= (1 << 0),
			StateHint			= (1 << 1),
			IconPixmapHint		= (1 << 2),
			IconWindowHint		= (1 << 3),
			IconPositionHint	= (1 << 4),
			IconMaskHint		= (1 << 5),
			WindowGroupHint		= (1 << 6),
			UrgencyHint			= (1 << 8),
			AllHints	 		= (InputHint|StateHint|IconPixmapHint|IconWindowHint|IconPositionHint|IconMaskHint|WindowGroupHint)
		}
		
	    [CLSCompliant(false)]
	    public enum XKeySym : uint
	    {
	        XK_BackSpace = 0xFF08,
	        XK_Tab = 0xFF09,
	        XK_Clear = 0xFF0B,
	        XK_Return = 0xFF0D,
	        XK_Home = 0xFF50,
	        XK_Left = 0xFF51,
	        XK_Up = 0xFF52,
	        XK_Right = 0xFF53,
	        XK_Down = 0xFF54,
	        XK_Page_Up = 0xFF55,
	        XK_Page_Down = 0xFF56,
	        XK_End = 0xFF57,
	        XK_Begin = 0xFF58,
	        XK_Menu = 0xFF67,
	        XK_Shift_L = 0xFFE1,
	        XK_Shift_R = 0xFFE2,
	        XK_Control_L = 0xFFE3,
	        XK_Control_R = 0xFFE4,
	        XK_Caps_Lock = 0xFFE5,
	        XK_Shift_Lock = 0xFFE6,
	        XK_Meta_L = 0xFFE7,
	        XK_Meta_R = 0xFFE8,
	        XK_Alt_L = 0xFFE9,
	        XK_Alt_R = 0xFFEA,
	        XK_Super_L = 0xFFEB,
	        XK_Super_R = 0xFFEC,
	        XK_Hyper_L = 0xFFED,
	        XK_Hyper_R = 0xFFEE,
	    }

		[StructLayout(LayoutKind.Sequential)]
		public struct XVisualInfo
		{
			public	IntPtr		visual;
			public	TLong		visualid;
			public	TInt		screen;
			public	TInt		depth;
			public	TInt		cls;
			public	TUlong		red_mask;
			public	TUlong		green_mask;
			public	TUlong		blue_mask;
			public	TInt		colormap_size;
			public	TInt		bits_per_rgb;
		}

		// Tested: O.K. on physical machines only.
		/// <summary> Move the pointer by the offsets (dest_x, dest_y) relative to the current position of the pointer. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="src_x11window"> Specify the source window or IntPtr.Zero. <see cref="IntPtr"/> </param>
		/// <param name="dest_x11window"> Specify the destination window or IntPtr.Zero. <see cref="IntPtr"/> </param>
		/// <param name="src_x"> X-coordinate to specify a rectangle in the source window. <see cref="System.Int32"/> </param>
		/// <param name="src_y"> X-coordinate to specify a rectangle in the source window. <see cref="System.Int32"/> </param>
		/// <param name="src_width"> Width to specify a rectangle in the source window. <see cref="System.UInt32"/> </param>
		/// <param name="src_height"> Height to specify a rectangle in the source window. <see cref="System.UInt32"/> </param>
		/// <param name="dest_x"> Specify the x-coordinates within the destination window. <see cref="System.Int32"/> </param>
		/// <param name="dest_y">Specify the y-coordinates within the destination window. <see cref="System.Int32"/> </param>
		/// <remarks> If src_x11window is IntPtr.Zero and dest_x11window is IntPtr.Zero, the pointer moves relative to the current position. </remarks>
		/// <remarks> If src_x11window is IntPtr.Zero and dest_x11window is a window, the pointer moves absolute to the window's origin. </remarks>
		/// <remarks> If src_x11window is a window, the pointer moves only within the src_x11window and if the specified rectangle contains the pointer. </remarks>
		/// <remarks> !!! Requires subsequent call of XFlush to take effect !!! </remarks>
		/// <remarks> !!! Doesn't work an virtual machines based on VM-Ware !!! </remarks>
		[DllImport ("libX11")]
		public extern static void XWarpPointer(IntPtr x11display, IntPtr src_x11window, IntPtr dest_x11window, int src_x, int src_y, uint src_width, uint src_height, int dest_x, int dest_y);

		#region Cursor manitulation
		
		// Usage:
		// IntPtr cursor = Xlib.XCreateFontCursor (xdisplay, (uint)Gdk.CursorType.IronCross);
		// Xlib.XDefineCursor (xdisplay, xwindow, cursor);
		// Xlib.XUndefineCursor (xdisplay, xwindow);

		// Tested: O.K.
		/// <summary> X provides a set of standard cursor shapes in a special font named cursor.The shape argument specifies which glyph of the standard fonts to use. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="cursor_shape"> The glyph of the cursor font to set. <see cref="System.UInt32"/> </param>
		/// <returns> The cursor structure to use with XDefineCursor() on success, or IntPtr.Zero otherwise. <see cref="IntPtr"/> </returns>
		[DllImport ("libX11")]
		public extern static IntPtr XCreateFontCursor (IntPtr x11display, CursorFontShape cursorFontShape);

		// Tested: O.K. on physical machines only.
		/// <summary> Set the indicated cursor. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="x11window"> The window to set the cursor for. <see cref="IntPtr"/> </param>
		/// <param name="cursor"> The cursor to set, or IntPtr.Zero to unset the current cursor. <see cref="IntPtr"/> </param>
		/// <remarks> !!! Requires subsequent call of XFlush to take effect !!! </remarks>
		/// <remarks> !!! Doesn't work reliable an virtual machines based on VM-Ware !!! </remarks>
		[DllImport ("libX11")]
		public extern static void XDefineCursor (IntPtr x11display, IntPtr x11window, IntPtr cursor);

		// Tested: O.K.
		/// <summary> Undoes the effect of a previous XDefineCursor() call for indicated window. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="x11window"> The window to set the cursor for. <see cref="IntPtr"/> </param>
		[DllImport("libX11")]
		extern public static void XUndefineCursor(IntPtr x11display, IntPtr x11window);
		
		#endregion

		#region Window creation and manipulation methods
		
		// Tested: O.K.
		/// <summary> Open a connection to the X server that controls a display. </summary>
		/// <param name="displayName"> Display name syntax is: hostname:number.screen_number; like: dual-headed:0.1; or empty sting for default. <see cref="String"/> </param>
		/// <returns> The display pointer on success, or IntPtr.Zero otherwise. <see cref="IntPtr"/> </returns>
		[DllImport("libX11")]
		extern public static IntPtr XOpenDisplay (String x11displayName);
		
		// Tested: O.K.
		/// <summary> Close a connection to the X server that controls a display. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		[DllImport("libX11")]
		extern public static void XCloseDisplay (IntPtr x11display);

		// Tested: O.K.
		/// <summary> Get the root window. Useful with functions that need a drawable of a particular screen and for creating top-level windows. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The screen number, that specifies the appropriate screen on the host server. <see cref="TInt"/> </param>
		/// <returns> The root window on success, or IntPtr.Zero otherwise. <see cref="IntPtr"/> </returns>
		[DllImport("libX11")]
		extern public static IntPtr XRootWindow (IntPtr x11display, TInt screenNumber);
		
		/// <summary> Get the root window of the specified screen. </summary>
		/// <param name="x11screen"> The connection to an X server. <see cref="System.IntPtr"/> </param>
		/// <returns> The window ID. <see cref="IntPtr"/> </returns>
		[DllImport("libX11")]
		extern public static IntPtr XRootWindowOfScreen (IntPtr x11screen);
			
		// Tested: O.K.
		/// <summary> Destroy the specified window as well as all of its subwindows. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="x11window"> The window to destroy. <see cref="IntPtr"/> </param>
		[DllImport("libX11")]
		extern public static  void XDestroyWindow (IntPtr x11display, IntPtr x11window);
		
		// Tested: O.K.
		/// <summary> Get the default root window. Useful with functions that need a drawable of a particular screen and for creating top-level windows. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <returns> The default root window on success, or IntPtr.Zero otherwise. <see cref="IntPtr"/> </returns>
		[DllImport("libX11")]
		extern public static IntPtr XDefaultRootWindow (IntPtr x11display);
	
		// Tested: O.K.
		/// <summary> Move specified window to the specified x and y coordinates, but do not change the window's size, raise the window, or change the mapping state of the window. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="x11window"> The window to move. <see cref="IntPtr"/> </param>
		/// <param name="x"> The new x coordinate, which defines the new location of the top-left pixel of the window's border or the window itself if it has no border. <see cref="System.Int32"/> </param>
		/// <param name="y"> The new y coordinate, which defines the new location of the top-left pixel of the window's border or the window itself if it has no border. <see cref="System.Int32"/> </param>
		[DllImport("libX11")]
		extern public static void XMoveWindow (IntPtr x11display, IntPtr x11window, TInt x, TInt y);
	
		// Tested: O.K.
		/// <summary> Change the inside dimensions of the specified window, not including its borders. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="x11window"> The window to resize. <see cref="IntPtr"/> </param>
		/// <param name="width"> The new width, which is the interior dimensions of the window after the call completes. <see cref="System.UInt32"/> </param>
		/// <param name="height"> The new heigth, which is the interior dimensions of the window after the call completes. <see cref="System.UInt32"/> </param>
		[DllImport("libX11")]
		extern public static void XResizeWindow (IntPtr x11display, IntPtr x11window, TUint width, TUint height);
	
		// Tested: O.K.
		/// <summary> Change the size and location of the specified window without raising it. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="x11window"> The window to move and resize. <see cref="IntPtr"/> </param>
		/// <param name="x"> The new x coordinate, which defines the new location of the top-left pixel of the window's border or the window itself if it has no border. <see cref="System.Int32"/> </param>
		/// <param name="y"> The new y coordinate, which defines the new location of the top-left pixel of the window's border or the window itself if it has no border. <see cref="System.Int32"/> </param>
		/// <param name="width"> The new width, which is the interior dimensions of the window after the call completes. <see cref="System.UInt32"/> </param>
		/// <param name="height"> The new heigth, which is the interior dimensions of the window after the call completes. <see cref="System.UInt32"/> </param>
		[DllImport("libX11")]
		extern public static void XMoveResizeWindow (IntPtr x11display, IntPtr x11window, TInt x, TInt y, TUint width, TUint height);
		
		// Tested: O.K.
		/// <summary> Map the window and all of its subwindows that have had map requests to a display. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="x11window"> The window to map. <see cref="System.IntPtr"/> </param>
		[DllImport("libX11")]
		extern public static void XMapWindow (IntPtr x11display, IntPtr x11window);
		
		/// <summary> Unmap the specified window and causes the X server to generate an UnmapNotify event. If the specified window is already unmapped,
		/// XUnmapWindow () has no effect. Normal exposure processing on formerly obscured windows is performed. Any child window will no longer be visible
		/// until another map call is made on the parent. In other words, the subwindows are still mapped but are not visible until the parent is mapped.
		/// Unmapping a window will generate Expose events on windows that were formerly obscured by it. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="x11window"> The window to unmap. <see cref="System.IntPtr"/> </param>
		[DllImport("libX11")]
		extern public static void XUnmapWindow (IntPtr x11display, IntPtr x11window);

		// Tested: O.K.
		/// <summary> The XMapRaised() function essentially is similar to XMapWindow() in that it maps the window and all of its subwindows that have
		/// had map requests. However, it also raises the specified window to the top of the stack. For additional information, see XMapWindow(). </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="x11window"> The window to map. <see cref="IntPtr"/> </param>
		[DllImport("libX11")]
		extern public static void XMapRaised(IntPtr x11display, IntPtr x11window);
		
		// Tested: O.K.
		/// <summary> Create an unmapped subwindow for a specified parent window, returns the window ID of the created window, and causes the
		/// X server to generate a CreateNotify event.  The created window is placed on top in the stacking order with respect to siblings. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="x11window"> The parent window. <see cref="IntPtr"/> </param>
		/// <param name="x"> Tthe x coordinate, which is the top-left outside corner of the window's borders and relative to the inside of the parent window's borders. <see cref="X11.TInt"/> </param>
		/// <param name="x"> Tthe y coordinate, which is the top-left outside corner of the window's borders and relative to the inside of the parent window's borders. <see cref="X11.TInt"/> </param>
		/// <param name="width"> The width, which is the created window's inside dimensions and do not include the created window's borders. <see cref="X11.TUint"/> </param>
		/// <param name="height"> The height, which is the created window's inside dimensions and do not include the created window's borders. <see cref="X11.TUint"/> </param>
		/// <param name="outsideBorderWidth"> The outside (surrounding) border width of the created window in pixels. <see cref="X11.TInt"/> </param>
		/// <param name="border"> The border pixel value of the window. <see cref="TPixel"/> </param>
		/// <param name="background"> The background pixel value of the window. <see cref="TPixel"/> </param>
		/// <returns> The window ID of the created window on success, or IntPtr.Zero otherwise. <see cref="IntPtr"/> </returns>
		[DllImport("libX11")]
		extern public static IntPtr XCreateSimpleWindow (IntPtr x11display, IntPtr x11window, TInt x, TInt y, TUint width, TUint height,
		                                                 TUint outsideBorderWidth, TPixel border, TPixel background);
		
		// Tested: O.K.
		/// <summary> Create an unmapped subwindow for a specified parent window, returns the window ID of the created window, and causes the
		/// X server to generate a CreateNotify event.  The created window is placed on top in the stacking order with respect to siblings. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="x11window"> The parent window. <see cref="IntPtr"/> </param>
		/// <param name="x"> Tthe x coordinate, which is the top-left outside corner of the window's borders and relative to the inside of the parent window's borders. <see cref="X11.TInt"/> </param>
		/// <param name="x"> Tthe y coordinate, which is the top-left outside corner of the window's borders and relative to the inside of the parent window's borders. <see cref="X11.TInt"/> </param>
		/// <param name="width"> The width, which is the created window's inside dimensions and do not include the created window's borders. <see cref="X11.TUint"/> </param>
		/// <param name="height"> The height, which is the created window's inside dimensions and do not include the created window's borders. <see cref="X11.TUint"/> </param>
		/// <param name="outsideBorderWidth"> The outside (surrounding) border width of the created window in pixels. <see cref="X11.TInt"/> </param>
		/// <param name="depth"> The window's depth.  A depth of CopyFromParent means the depth is taken from the parent. <see cref="TInt"/> </param>
		/// <param name="cls"> The created window's class.  You can pass InputOutput, InputOnly, or CopyFromParent.
		/// A class of CopyFromParent means the class is taken from the parent. <see cref="TUint"/> </param>
		/// <param name="x11visual"> The visual type.  A visual of CopyFromParent means the visual type is taken from the parent. <see cref="IntPtr"/> </param>
		/// <param name="valueMask"> Specifies which window attributes are defined in the attributes argument.  This mask is the bitwise inclusive OR of the valid
		/// attribute mask bits. If valuemask is zero, the attributes are ignored and are not referenced. <see cref="WindowAttributeMask"/> </param>
		/// <param name="attributes"> The structure from which the values (as specified by the value mask) are to be taken. The value mask should have the
		/// appropriate bits set to indicate which attributes have been set in the structure. <see cref="XSetWindowAttributes"/> </param>
		/// <returns> The window ID of the created window on success, or IntPtr.Zero otherwise. <see cref="IntPtr"/> </returns>
		[DllImport("libX11")]
		extern public static IntPtr XCreateWindow (IntPtr x11display, IntPtr x11window, TInt x, TInt y, TUint width, TUint height, TUint outsideBorderWidth,
		                                           TInt depth, TUint cls, IntPtr x11visual, WindowAttributeMask valueMask, ref XSetWindowAttributes attributes);

		// Tested: O.K.
		/// <summary> Return the atom identifier associated with the specified atom_name string.
		/// If onlyIfExists is False, the atom is created if it does not exist. Therefore, XInternAtom() can return IntPtr.Zero.
		/// If the atom name is not in the Host Portable Character Encoding, the result is implementation dependent.
		/// Uppercase and lowercase matter; the strings ``thing'', ``Thing'', and ``thinG'' all designate different atoms.
		/// The atom will remain defined even after the client's connection closes. It will become undefined only when the last
		/// connection to the X server closes. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="atomName"> The name of the atom to return. <see cref="System.SByte[]"/> </param>
		/// <param name="onlyIfExists"> Specifies a boolean value that indicates whether the atom must be created, if it doesn't exist. <see cref="System.Boolean"/> </param>
		/// <returns> A pointer to the atom on success, or IntPtr.Zero otherwise. <see cref="IntPtr"/> </returns>
		/// <remarks> Define the atomName including terminating NULL like: X11Utils.StringToSByteArray ("WM_DELETE_WINDOW\0") </remarks>
		[DllImport("libX11")]
		extern public static IntPtr XInternAtom(IntPtr x11display, X11.TChar[] atomName, bool onlyIfExists);

		// Tested: O.K.
		/// <summary> Return the atom identifier associated with the specified atomName string.
		/// If onlyIfExists is False, the atom is created if it does not exist. Therefore, XInternAtom() can return IntPtr.Zero.
		/// If the atom name is not in the Host Portable Character Encoding, the result is implementation dependent.
		/// Uppercase and lowercase matter; the strings ``thing'', ``Thing'', and ``thinG'' all designate different atoms.
		/// The atom will remain defined even after the client's connection closes. It will become undefined only when the last
		/// connection to the X server closes. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="atomName"> The name of the atom to return. <see cref="System.String"/> </param>
		/// <param name="onlyIfExists"> Specifies a boolean value that indicates whether the atom must be created, if it doesn't exist. <see cref="System.Boolean"/> </param>
		/// <returns> A pointer to the atom on success, or IntPtr.Zero otherwise. <see cref="IntPtr"/> </returns>
		[DllImport("libX11")]
		extern public static IntPtr XInternAtom(IntPtr x11display, [MarshalAs(UnmanagedType.LPStr)] string atomName, bool onlyIfExists);
		
		// Tested: O.K.
		/// <summary> Replace the WM_PROTOCOLS property on the specified window with the list of atoms specified by the protocols argument.
		/// If the property does not already exist, XSetWMProtocols() sets the WM_PROTOCOLS property on the specified window to the list of
		/// atoms specified by the protocols argument. The property is stored with a type of ATOM and a format of 32.
		/// If it cannot intern the WM_PROTOCOLS atom, XSetWMProtocols() returns a zero status. Otherwise, it returns a nonzero status. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="x11window"> The window to replace the the WM_PROTOCOLS property. <see cref="IntPtr"/> </param>
		/// <param name="protocols"> Thelist of protocols as atom pointer. <see cref="IntPtr"/> </param>
		/// <param name="count"> The number of protocols in the list. <see cref="X11.TInt"/> </param>
		/// <returns> Nonzero on success, or zero otherwise. <see cref="X11.TInt"/> </returns>
		[DllImport("libX11")]
		extern public static TInt XSetWMProtocols(IntPtr x11display, IntPtr x11window, ref IntPtr protocols, TInt count);
		
		/// <summary> Flush the output buffer. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		[DllImport("libX11")]
		extern public static void XFlush (IntPtr x11display);
		
		// Tested: O.K.
		/// <summary> Assign the indicated name to the specified window. A window manager can display the window name in some prominent place,
		/// such as the title bar, to allow users to identify windows easily. Some window managers may display a window's name in the window's
		/// icon, although they are encouraged to use the window's icon name if one is provided by the application. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="x11window"> The window to set the name for. <see cref="IntPtr"/> </param>
		/// <param name="windowName"> The name to set for the window. <see cref="System.SByte[]"/> </param>
		[DllImport("libX11")]
		extern public static void XStoreName(IntPtr x11display, IntPtr x11window, sbyte[] windowName);
		
		/// <summary> Allocate and return a pointer to a XClassHint structure.
		/// Note that the pointer fields in the XClassHint structure are initially set to NULL.
		/// To free the memory allocated to this structure, use XFree(). </summary>
		/// <returns> The pointer to a XClassHint structure on success, or NULL otherwise (insufficient memory). <see cref="IntPtr"/> </returns>
		[DllImport("libX11", EntryPoint = "_XAllocClassHint")]
		extern private static IntPtr _XAllocClassHint ();
		
		// Tested: O.K.
		/// <summary> Allocate and return a pointer to a XClassHint structure.
		/// Note that the pointer fields in the XClassHint structure are initially set to NULL.
		/// To free the memory allocated to this structure, use XFree(). </summary>
		/// <param name="classHint"> The class hint to get. <see cref="XClassHint"/> </param>
		/// <returns> Nonzero on success, or zero otherwise. <see cref="System.Int32"/> </returns>
		public static int XAllocClassHint (out XClassHint classHint)
		{
			// IntPtr classHintPtr = _XAllocClassHint ();
			classHint = new XClassHint ();
			return 1;
		}
		
		// Tested: O.K.
		/// <summary> Free in-memory data that was created by an Xlib function. </summary>
		/// <param name="data"> The data that is to be freed. <see cref="System.IntPtr"/> </param>
		[DllImport("libX11", EntryPoint = "XFree")]
		extern public static void XFree (IntPtr data);
		
		// Tested: O.K.
		/// <summary> Return the class hint of the specified window to the members of the structure.
		/// If the data returned by the server is in the Latin Portable Character Encoding, then the returned
		/// strings are in the Host Portable Character Encoding. Otherwise, the result is implementation dependent.
		/// To free res_name and res_class when finished with the strings, use XFree() on each individually. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="x11window"> The window to get the class hint for. <see cref="System.IntPtr"/> </param>
		/// <param name="classHint"> The class hint to get. <see cref="X11._XClassHint"/> </param>
		/// <returns> Nonzero on success, or zero otherwise. <see cref="X11.TInt"/> </returns>
		[DllImport("libX11", EntryPoint = "XGetClassHint")]
		extern private static TInt _XGetClassHint (IntPtr x11display, IntPtr x11window, ref _XClassHint classHint);

		// Tested: O.K.
		/// <summary> Return a XClassHint structure of the specified window to the members of the structure.
		/// If the data returned by the server is in the Latin Portable Character Encoding, then the returned
		/// strings are in the Host Portable Character Encoding. Otherwise, the result is implementation dependent. </summary
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="x11window"> The window to get the class hint for. <see cref="System.IntPtr"/> </param>
		/// <param name="classHint"> The class hint to get. <see cref="X11.XClassHint"/> </param>
		/// <returns> Nonzero on success, or zero otherwise. <see cref="System.Int32"/> </returns>
		public static int XGetClassHint (IntPtr x11display, IntPtr x11window, out XClassHint classHint)
		{
			_XClassHint _classHint = _XClassHint.Zero;
			
			if (_XGetClassHint (x11display, x11window, ref _classHint) == (TInt)0)
			{
				// Clean up unmanaged memory.
				// --------------------------
				// Typically: _classHint.res_name == IntPtr.Zero
				// Freeing a IntPtr.Zero is not prohibited.
				// Use first member (at offset 0) to free the structure itself.
				XFree (_classHint.res_name);
				
				classHint = XClassHint.Zero;
				return 0;
			}
			else
			{
				classHint = new XClassHint();
				// Marshal data from an unmanaged block of memory to a managed object.
				classHint.res_name  = (string)Marshal.PtrToStringAnsi (_classHint.res_name );
				classHint.res_class = (string)Marshal.PtrToStringAnsi (_classHint.res_class);
				
				// Clean up unmanaged memory.
				// --------------------------
				// Freeing a IntPtr.Zero is not prohibited.
				// First structure member (at offset 0) frees  the structure itself as well.
				XFree (_classHint.res_name);
				XFree (_classHint.res_class);
				
				return 1;
			}
		}
		
		// Tested: O.K.
		/// <summary> Set the class hint for the specified window.
		/// If the strings are not in the Host Portable Character Encoding, the result is implementation dependent.  </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="x11window"> The window to set the class hint for. <see cref="System.IntPtr"/> </param>
		/// <param name="classHint"> The class hint to set. <see cref="X11.XClassHint"/> </param>
		[DllImport("libX11")]
		extern public static void XSetClassHint (IntPtr x11display, IntPtr x11window, [MarshalAs(UnmanagedType.Struct)] ref XClassHint classHints);
		
		// Tested: O.K.
		/// <summary> Get the WM_TRANSIENT_FOR property for the specified window.  </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="x11window"> The window to set the WM_TRANSIENT_FOR property for. <see cref="System.IntPtr"/> </param>
		/// <param name="x11transientWindow"> The WM_TRANSIENT_FOR property of the specified window. <see cref="System.IntPtr"/> </param>
		/// <returns> Nonzero on success, or zero otherwise. <see cref="X11.TInt"/> </returns>
		[DllImport("libX11")]
		extern public static TInt XGetTransientForHint(IntPtr x11display, IntPtr x11window, ref IntPtr x11transientWindow);
			
		// Tested: O.K.
		/// <summary> Set the WM_TRANSIENT_FOR property for the specified window.  </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="x11window"> The window to get the WM_TRANSIENT_FOR property for. <see cref="System.IntPtr"/> </param>
		/// <param name="x11transientWindow"> The WM_TRANSIENT_FOR property of the specified window. <see cref="System.IntPtr"/> </param>
		[DllImport("libX11")]
		extern public static void XSetTransientForHint(IntPtr x11display, IntPtr x11window, IntPtr x11transientWindow);
			
		// Tested: O.K.
		/// <summary> Get the window attributes for the specified window. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="x11window"> The window to get the window attributes for. <see cref="System.IntPtr"/> </param>
		/// <param name="classHint"> The window attributes to get. <see cref="X11.XWindowAttributes"/> </param>
		/// <returns> Nonzero on success, or zero otherwise. <see cref="X11.TInt"/> </returns>
		[DllImport("libX11")]
		extern public static TInt XGetWindowAttributes (IntPtr x11display, IntPtr x11window, [MarshalAs(UnmanagedType.Struct)] ref XWindowAttributes windowAttributes);
		
		// Tested: O.K.
		/// <summary> Changes the input focus and the last-focus-change time.
		/// It has no effect if the specified time is earlier than the current last-focus-change time or is later than the current X server time.
		/// Otherwise, the last-focus-change time is set to the specified time (CurrentTime is replaced by the current X server time).
		/// XSetInputFocus() causes the X server to generate FocusIn and FocusOut events. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="x11window"> The window that shold get the focus, 'PointerRoot', that is 1L, or 'None', that 0L. <see cref="System.IntPtr"/> </param>
		/// <param name="revertTo"> Specifies where the input focus reverts to if the window becomes not viewable. Either RevertToParent, RevertToPointerRoot, or RevertToNone. <see cref="X11lib.TRevertTo"/> </param>
		/// <param name="time"> The time. Either a timestamp or 'CurrentTime', that is 0L. <see cref="TInt"/> </param>
		[DllImport("libX11")]
		extern public static void XSetInputFocus(IntPtr x11display, IntPtr x11window, TRevertTo revertTo, TInt time);
		
		// Tested: O.K. (for one string)
        /// <summary> Set the specified text property to be of type STRING (format 8) with a value representing the concatenation of the specified list of
        /// null-separated character strings. An extra null byte (which is not included in the nitems member) is stored at the end of the val field of
        /// text property. The strings are assumed (without verification) to be in the STRING encoding. To free the storage for the val field, use XFree(). </summary>
        /// <param name="list"> The list of null-terminated character strings to set into the text property. <see cref="TChar[]"/> </param>
        /// <param name="count"> The number of strings to set to the text property. A <see cref="TInt"/> </param>
        /// <param name="textProperty"> The text property to set. <see cref="XTextProperty"/> </param>
		/// <returns> Nonzero on success, or zero otherwise. <see cref="X11.TInt"/> </returns>
		[DllImport("libX11")]
		extern public static TInt XStringListToTextProperty (ref TChar[] list, TInt count, ref XTextProperty textProperty);
		[DllImport("libX11")]
		extern public static TInt XStringListToTextProperty (ref TChar[] list, TInt count, IntPtr p);
		
		// Tested: O.K.
        /// <summary> Set the window manager name of a window (in other words the application window title).
        /// Convenience function calls XSetTextProperty() to set the WM_NAME property. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="x11window"> The window to set the window manager name of a window for. <see cref="System.IntPtr"/> </param>
		/// <param name="textProperty"> The text property holding the window manager name of a window to set. <see cref="XTextProperty"/> </param>
		[DllImport("libX11", EntryPoint = "XSetWMName")]
		extern public static void _XSetWMName(IntPtr x11display, IntPtr x11window, ref XTextProperty textProperty);
		[DllImport("libX11", EntryPoint = "XSetWMName")]
		extern public static void _XSetWMName(IntPtr x11display, IntPtr x11window, IntPtr p);

		// Tested: O.K.
        /// <summary> Set the window manager name of a window (in other words the application window title). </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="x11window"> The window to set the window manager name of a window for. <see cref="System.IntPtr"/> </param>
		/// <param name="windowTitle"> The indow manager name of a window to set. <see cref="System.String"/> </param>
		public static void XSetWMName(IntPtr x11display, IntPtr x11window, string windowTitle)
		{
			X11lib.XTextProperty titleNameProp = new X11lib.XTextProperty ();
			TChar[]              titleNameVal  = X11Utils.StringToSByteArray (windowTitle + "\0");
			
			// PROBLEM: Doesn't work on 64 bit!
			/*NEW*/IntPtr p = Marshal.AllocHGlobal (Marshal.SizeOf (titleNameProp));
			/*REPLACED*///if (X11lib.XStringListToTextProperty (ref titleNameVal, (TInt)1, ref titleNameProp) != (TInt)0)
			if (X11lib.XStringListToTextProperty (ref titleNameVal, (TInt)1, p) != (TInt)0)
			{
				/*NEW*/X11lib._XSetWMName (x11display, x11window, p);
				/*REPLACED*///X11lib._XSetWMName (x11display, x11window, ref titleNameProp);
				/*NEW*/titleNameProp = (X11lib.XTextProperty)Marshal.PtrToStructure (p, typeof(X11lib.XTextProperty));
				X11lib.XFree (titleNameProp.val);
				titleNameProp.val = IntPtr.Zero;
			}
			else
				Console.WriteLine ("X11lib::XSetWMName () ERROR: Can not set window name.");
		}
		
		// Tested: O.K.
        /// <summary> Set the window manager name of a window's icon (in other words the application icon title).
        /// Convenience function calls XSetTextProperty() to set the WM_ICON_NAME property. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="x11window"> The window to set the window manager name of a window's icon for. <see cref="System.IntPtr"/> </param>
		/// <param name="textProperty"> The text property holding the window manager name of a window's icon to set. <see cref="XTextProperty"/> </param>
		[DllImport("libX11", EntryPoint = "XSetWMIconName")]
		extern public static void _XSetWMIconName(IntPtr x11display, IntPtr x11window, ref XTextProperty textProperty);
		[DllImport("libX11", EntryPoint = "XSetWMIconName")]
		extern public static void _XSetWMIconName(IntPtr x11display, IntPtr x11window, IntPtr p);

		// Tested: O.K.
        /// <summary> Set the window manager name of a window's icon (in other words the application icon title). </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="x11window"> The window to set the window manager name of a window's icon for. <see cref="System.IntPtr"/> </param>
		/// <param name="windowTitle"> The indow manager name of a window's icon to set. <see cref="System.String"/> </param>
		public static void XSetWMIconName(IntPtr x11display, IntPtr x11window, string windowIconTitle)
		{
			X11lib.XTextProperty iconNameProp  = new X11lib.XTextProperty ();
			TChar[]              iconNameVal   = X11Utils.StringToSByteArray (windowIconTitle + "\0");

			// PROBLEM: Doesn't work on 64 bit!
			/*NEW*/IntPtr p = Marshal.AllocHGlobal (Marshal.SizeOf (iconNameProp));
			if (X11lib.XStringListToTextProperty (ref iconNameVal, (TInt)1, p) != (TInt)0)
			/*REPLACED*///if (X11lib.XStringListToTextProperty (ref iconNameVal, (TInt)1, ref iconNameProp) != (TInt)0)
			{
				/*NEW*/X11lib._XSetWMIconName (x11display, x11window, p);
				/*REPLACED*///X11lib._XSetWMIconName (x11display, x11window, ref iconNameProp);
				/*NEW*/iconNameProp = (X11lib.XTextProperty)Marshal.PtrToStructure (p, typeof(X11lib.XTextProperty));
				X11lib.XFree (iconNameProp.val);
				iconNameProp.val = IntPtr.Zero;
			}
			else
				Console.WriteLine ("X11lib::XSetWMIconName () ERROR: Can not set window's icon name.");
		}
		
		// Tested: O.K.
		/// <summary> Allocate and return a pointer to a XWMHints structure. Note that all fields in the XWMHints structure are initially set to zero.
		/// If insufficient memory is available, XAllocWMHints() returns NULL. To free the memory allocated to this structure, use XFree(). </summary>
		/// <returns> The pointer to a XWMHints structure on success, or NULL otherwise. <see cref="XWMHints"/> </returns>
		[DllImport ("libX11")]
        extern public static XWMHints XAllocWMHints ();
		
		/// <summary> Read the window manager hints and return NULL if no WM_HINTS property was set on the window or return a pointer to a XWMHints
		/// structure if it succeeds. When finished with the data, free the space used for it by calling XFree(). </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="x11window"> The window to get the window manager hints for. <see cref="System.IntPtr"/> </param>
		/// <returns> The pointer to a XWMHints structure on success, or NULL if no WM_HINTS property was set on the window. <see cref="XWMHints"/> </returns>
		[DllImport ("libX11")]
        extern public static XWMHints XGetWMHints (IntPtr x11display, IntPtr x11window);
		
		// Tested: O.K.
		/// <summary> Set the window manager hints that include icon information and location, the initial state of the window,
		/// and whether the application relies on the window manager to get keyboard input. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="x11window"> The window to set the window manager hints for. <see cref="System.IntPtr"/> </param>
		/// <param name="wmHints"> The pointer to a XWMHints structure contining the values to apply. <see cref="XWMHints"/> </param>
		[DllImport ("libX11")]
        extern public static void XSetWMHints (IntPtr x11display, IntPtr x11window, ref XWMHints wmHints);

		#endregion
		
		#region Font and color
		
		// Tested: O.K.
		/// <summary> Create a font set for the specified display. The font set is bound to the current locale when XCreateFontSet () is called.
		/// The font set may be used in subsequent calls to obtain font and character information and to image text in the locale of the font set. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="baseFontName"> The list of base font names that Xlib uses to load the fonts needed for the locale.
		/// The base font names are a comma-separated list. The string is null-terminated and is assumed to be in the Host Portable Character Encoding; otherwise,
		/// the result is implementation dependent. White space immediately on either side of a separating comma is ignored. <see cref="System.SByte[]"/> </param>
		/// <param name="missingCharSetList"> Returns the missing charsets. <see cref="System.IntPtr"/> </param>
		/// <param name="count"> Returns the number of missing charsets. <see cref="System.Int32"/> </param>
		/// <param name="terminator"> Returns the string drawn for missing charsets. <see cref="System.IntPtr"/> </param>
		/// <returns> The created font set an output context using only default values. <see cref="IntPtr"/> </returns>
        [DllImport ("libX11")]
        extern public static IntPtr XCreateFontSet (IntPtr x11display, sbyte[] baseFontName, out IntPtr list, out int count, IntPtr terminator);

		#endregion Font and color
		
		#region Bitmap
		
		// Tested: O.K.
		/// <summary> Allocate the memory needed for an XImage structure for the specified display *** but *** does not allocate space for the image itself.
		/// Rather, it initializes the structure byte-order, bit-order, and bitmap-unit values from the display and returns a pointer to the XImage structure.
		/// The red, green, and blue mask values are defined for Z format images only and are derived from the Visual structure passed in. Other values
		/// also are passed in. The offset permits the rapid displaying of the image without requiring each scanline to be shifted into position. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="x11visual"> The visual to store the image. <see cref="IntPtr"/> </param>
		/// <param name="depth"> The (color) depth of the image. <see cref="TUint"/> </param>
		/// <param name="imageFormat"> The image format. <see cref="TImageFormat"/></param>
		/// <param name="offset"> The number of pixels to ignore at the beginning of the scanline. <see cref="TInt"/> </param>
		/// <param name="data"> The image data. In other words, a char* / byte* to the image data array <see cref="System.Byte[]"/> </param>
		/// <param name="width"> The width of the image, in pixels. <see cref="TUint"/> </param>
		/// <param name="height"> The height of the image, in pixels. <see cref="TUint"/> </param>
		/// <param name="bitmapPad"> The quantum of a scanline (8, 16, or 32). In other words, the start of one scanline is separated in client memory
		/// from the start of the next scanline by an integer multiple of this many bits. <see cref="TInt"/> </param>
		/// <param name="bytesPerLine"> The number of bytes in the client image between the start of one scanline and the start of the next. <see cref="TInt"/> </param>
		/// <returns> The new XImage on success, or IntPtr.Zero on any error. <see cref="IntPtr"/> </returns>
		/// <remarks>  If a value of zero is passed in bytesPerLine, Xlib assumes that the scanlines are contiguous in memory and calculates the value of bytesPerLine itself. </remarks>
		[DllImport ("libX11")]
        extern public static IntPtr XCreateImage (IntPtr x11display, IntPtr x11visual, TUint depth, TImageFormat imageFormat, TInt offset, [MarshalAs(UnmanagedType.U1)] byte[] data, TUint width, TUint height, TInt bitmapPad, TInt bytesPerLine);
		[DllImport ("libX11")]
        extern public static IntPtr XCreateImage (IntPtr x11display, IntPtr x11visual, TUint depth, TImageFormat imageFormat, TInt offset, IntPtr data, TUint width, TUint height, TInt bitmapPad, TInt bytesPerLine);
		
		// Tested: O.K.
		/// <summary> Deallocate the memory associated with the XImage structure. </summary>
		/// <param name="x11ximage"> The image to destroy. <see cref="IntPtr"/> </param>
		/// <remarks> Note that when the image is created using XCreateImage(), XGetImage(), or XSubImage(), the destroy procedure (_XImage.f.destroy_image), that this macro calls,
		/// frees both the image structure and the data pointed to by the image structure. *** Danger *** Don't delete associated graphic data from managed code! </remarks>
		[DllImport("libX11")]
		extern public static void XDestroyImage(IntPtr x11ximage);

		// Tested: O.K.
		/// <summary> combines an image with a rectangle of the specified drawable.
		/// The section of the image defined by the src_x, src_y, width, and height arguments is drawn on the specified part of the drawable. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="x11drawable"> The drawable, the imag is to put on. <see cref="System.IntPtr"/> </param>
		/// <param name="x11gc"> The crapchics context to use for drawing. <see cref="System.IntPtr"/> </param>
		/// <param name="image"> The image to be combined with the rectangle. <see cref="System.IntPtr"/> </param>
		/// <param name="srcOffsetX"> The offset in X from the left edge of the image. <see cref="TInt"/> </param>
		/// <param name="srOffsetY"> The offset in Y from the top edge of the image. <see cref="TInt"/> </param>
		/// <param name="destX"> The x coordinate, which is relative to the origin of the drawable and is the coordinate of the subimage. <see cref="TInt"/> </param>
		/// <param name="destY"> The y coordinate, which is relative to the origin of the drawable and is the coordinate of the subimage. <see cref="TInt"/> </param>
		/// <param name="width"> The width of the subimage, which define the dimensions of the rectangle. <see cref="TUint"/> </param>
		/// <param name="height"> The height of the subimage, which define the dimensions of the rectangle. <see cref="TUint"/> </param>
		[DllImport("libX11")]
		extern public static void XPutImage (IntPtr x11display, IntPtr x11drawable, IntPtr X11gc, IntPtr image, TInt srcOffsetX, TInt srcOffsetY, TInt destX, TInt destY, TUint width, TUint height);
		
		/// <summary> Create a pixmap of the width, height, and depth you specified and returns a pixmap ID that identifies it.
		/// It is valid to pass an InputOnly window to the drawable argument. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="x11drawable"> The drawable, the pixmap is created on. The server uses the specified drawable to determine on which screen to create the pixmap. <see cref="System.IntPtr"/> </param>
		/// <param name="width"> The width, which define the dimensions of the pixmap. <see cref="TUint"/> </param>
		/// <param name="height"> The height, which define the dimensions of the pixmap. <see cref="TUint"/> </param>
		/// <param name="depth"> The depth of the pixmap. <see cref="TUint"/> </param>
		/// <returns> The pixmap on success, or IntPtr.Zero otherwise. <see cref="IntPtr"/> </returns>
		/// <remarks> The width and height arguments must be nonzero, or a BadValue error results.
		/// The depth argument must be one of the depths supported by the screen of the specified drawable, or a BadValue error results.
		/// The pixmap can be used only on this screen and only with other drawables of the same depth.
		/// The initial contents of the pixmap are undefined. </remarks>
		[DllImport("libX11")]
		extern public static IntPtr XCreatePixmap (IntPtr x11display, IntPtr x11drawable, TUint width, TUint height, TUint depth);
		
		// Tested: O.K.
		/// <summary> Delete the association between the pixmap ID and the pixmap.
		/// Then, the X server frees the pixmap storage when there are no references to it.
		/// The pixmap should never be referenced again. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="x11pixmap"> The pixmap to free. <see cref="IntPtr"/> </param>
		/// <remarks> The XFreePixmap() function first deletes the association between the pixmap ID and the pixmap.
		/// Then, the X server frees the pixmap storage when there are no references to it. The pixmap should never be referenced again. </remarks>
		[DllImport("libX11")]
		extern public static void XFreePixmap(IntPtr x11display, IntPtr x11pixmap);
		
		// Tested: O.K.
		/// <summary> Set (with x11pixmap) or unset (with IntPtr.Zero) a clip mask (transparency mask). </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="x11gc"> The crapchics context to apply the clip mask on. <see cref="System.IntPtr"/> </param>
		/// <param name="x11pixmap"> The clip mask (transparency mask) to apply. <see cref="IntPtr"/> </param>
		[DllImport("libX11")]
		extern public static void XSetClipMask(IntPtr x11display, IntPtr X11gc, IntPtr  x11pixmap);
		
		[DllImport("libX11")]
		extern public static void XSetClipOrigin (IntPtr x11display, IntPtr X11gc, TInt clipOriginX, TInt clipOriginY);
		
		[DllImport("libX11")]
		extern public static void XCopyArea (IntPtr x11display, IntPtr X11drawableSrc, IntPtr X11drawableDest, IntPtr X11gc, TInt srcOffsetX, TInt srcOffsetY, TUint width, TUint height,  TInt dstOffsetX, TInt dstOffsetY);
		
		[DllImport("libX11")]
		extern public static void XCopyPlane (IntPtr x11display, IntPtr X11drawableSrc, IntPtr X11drawableDest, IntPtr X11gc, TInt srcOffsetX, TInt srcOffsetY, TUint width, TUint height,  TInt dstOffsetX, TInt dstOffsetY, TUlong plane);
		
		// Tested: O.K.
		/// <summary> Overwrite the pixel in the named image with the specified pixel value. The input pixel value must be in normalized format (that is,
		/// the least-significant byte of the long is the least-significant byte of the pixel). The image must contain the x and y coordinates. </summary>
		/// <param name="x11image"> The image to overwrite a containing pixel. <see cref="IntPtr"/> </param>
		/// <param name="x"> The logical x coordinate of the pixel to overwrite. <see cref="TInt"/> </param>
		/// <param name="y"> The logical y coordinate of the pixel to overwrite. <see cref="TInt"/> </param>
		/// <param name="pixel"> The pixel value to overwrite. <see cref="TPixel"/> </param>
		[DllImport("libX11")]
		extern public static void XPutPixel(IntPtr x11image, TInt x, TInt y, TPixel pixel);

		// Tested: O.K.
		/// <summary> Allows to include a bitmap file into the program code that was written out by XWriteBitmapFile () without reading in the bitmap file. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="X11drawableSrc"> The drawable that indicates the screen. <see cref="IntPtr"/> </param>
		/// <param name="data"> The data in bitmap format. <see cref="TUchar[]"/> </param>
		/// <param name="width"> The width of the bitmap. <see cref="TUint"/> </param>
		/// <param name="height"> The height of the bitmap. <see cref="TUint"/> </param>
		/// <returns> The server side pixmap on success, or NULL otherwise. <see cref="TPixmap"/> </returns>
		/// <remarks> It is your responsibility to free the bitmap using XFreePixmap() when finished. </remarks>
		[DllImport("libX11")]
		extern public static TPixmap XCreateBitmapFromData (IntPtr x11display, IntPtr X11drawableSrc, TUchar[] data, TUint width, TUint height);
		                       
		[DllImport("libX11")]
		extern public static TPixmap XReadBitmapFile (IntPtr x11display, IntPtr X11drawableSrc, TChar[] filepath,
		                                           ref TUint widthReturn, ref TUint heightReturn, ref IntPtr pixmapReturn,
		                                           ref TInt xHotReturn, ref TInt yHotReturn);
		#endregion Bitmap
		
		#region

		// Tested: O.K.
        [DllImport ("libX11")]
        extern public static void XFreeStringList (IntPtr ptr);

		// Tested: O.K.
        [DllImport ("libX11")]
        extern public static void XFreeFontSet (IntPtr x11display, IntPtr data);
		
		/// <summary> Request that the X server report the events associated with the specified event mask. Initially, X will not report any of these events. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="x11window"> The window whose events you are interested in. <see cref="System.IntPtr"/> </param>
		/// <param name="eventMask"> Specifies the event mask. <see cref="X11lib.EventMask"/> </param>
		[DllImport("libX11")]
		extern public static void XSelectInput (IntPtr x11display, IntPtr x11window, EventMask eventMask);
		
		// Tested: O.K.
		/// <summary> Search the event queue. The first event that matches the specified mask will be removed and copied into the specified eventReturn structure. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="eventMask"> The the event mask, specifying the events to observe. <see cref="X11lib.EventMask"/> </param>
		/// <param name="eventTeturn"> Returns the matched event's associated structure. <see cref="IntPtr"/> </param>
		/// <returns> True, if an event was removed from the event queue and copied into the specified eventReturn structure, false otherwise. <see cref="System.Boolean"/> </returns>
		[DllImport("libX11")]
		extern public static bool XCheckMaskEvent (IntPtr x11display, EventMask eventMask, out IntPtr eventReturn);
		
		// ##########################################################################################################
		// ###   D I S P L A Y ,   S C R E E N   A N D   V I S U A L
		// ##########################################################################################################
		
		#region Display, screen and visual
		
		// Tested: O.K.
		/// <summary> Return the number of available screens. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <returns> The number of available screens. <see cref="System.Int32"/> </returns>
		[DllImport("libX11")]
		extern public static int XScreenCount(IntPtr x11display);

		// Tested: O.K.
		/// <summary> Return the default screen number referenced by the XOpenDisplay() function.
		/// This macro or function should be used to retrieve the screen number in applications that will use only a single screen. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <returns> The default screen number. <see cref="System.Int32"/> </returns>
		[DllImport("libX11")]
		extern public static X11.TInt XDefaultScreen(IntPtr x11display);
		
		// Tested: O.K.
		/// <summary> Return the connection number for the specified display.
		/// On a POSIX-conformant system, this is the file descriptor of the connection. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <returns> The connection number. <see cref="System.Int32"/> </returns>
		[DllImport("libX11")]
		extern public static int XConnectionNumber (IntPtr x11display);
		
		// Tested: O.K.
		/// <summary> Return a pointer to the indicated screen. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The screen number, that specifies the appropriate screen on the host server. <see cref="TInt"/> </param>
		/// <returns> The pointer to the indicated screen on success, or IntPtr.Zero otherwise. <see cref="IntPtr"/> </returns>
		[DllImport("libX11")]
		extern public static IntPtr XScreenOfDisplay (IntPtr x11display, TInt screenNumber);

		// Tested: O.K.
		/// <summary> Return the default visual type for the specified screen. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The screen number, that specifies the appropriate screen on the host server. <see cref="TInt"/> </param>
		/// <returns> The pointer to the default visual type on success, or IntPtr.Zero otherwise. <see cref="IntPtr"/> </returns>
		[DllImport("libX11")]
		extern public static IntPtr XDefaultVisual (IntPtr x11display, TInt screenNumber);
		
		// Tested: O.K.
		/// <summary> Return the depth (number of planes) of the default root window for the specified screen.
		/// Other depths may also be supported on this screen (see .PN XMatchVisualInfo ). </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The screen number, that specifies the appropriate screen on the host server. <see cref="TInt"/> </param>
		/// <returns> The requested default depth. <see cref="TInt"/> </returns>
		[DllImport("libX11")]
		extern public static TInt XDefaultDepth (IntPtr x11display, TInt screenNumber);
		
		#endregion
		
		// ##########################################################################################################
		// ###   C O L O R M A P   A N D   C O L O R
		// ##########################################################################################################
		
		#region Colormap and color
			
		// Tested: O.K.
		/// <summary> Permanently allocated default colormap entry to convey the expected relative intensity of the color. Can be used in implementing a monochrome application. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The screen number, that specifies the appropriate screen on the host server. <see cref="TInt"/> </param>
		/// <returns> The black pixel value. Only black and white pixel are guaranteed for all XServer. <see cref="TPixel"/> </returns>
		[DllImport("libX11")]
		extern public static TPixel XBlackPixel (IntPtr x11display, X11.TInt screenNumber);
		
		// Tested: O.K.
		/// <summary> Permanently allocated default colormap entry to convey the expected relative intensity of the color. Can be used in implementing a monochrome application. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The screen number, that specifies the appropriate screen on the host server. <see cref="TInt"/> </param>
		/// <returns> The white pixel value. Only black and white pixel are guaranteed for all XServer. <see cref="TPixel"/> </returns>
		[DllImport("libX11")]
		extern public static TPixel XWhitePixel (IntPtr x11display, X11.TInt screenNumber);
		
		// Tested: O.K.
		/// <summary> Return the black pixel value of the specified screen. </summary>
		/// <param name="x11screen"> The screen pointer, that specifies  the appropriate screen structure. <see cref="IntPtr"/> </param>
		/// <returns> The black pixel value. Only black and white pixel are guaranteed for all XServer. <see cref="TPixel"/> </returns>
		[DllImport("X11")]
		extern public static TPixel XBlackPixelOfScreen(IntPtr x11screen);

		// Tested: O.K.
		/// <summary> Return the white pixel value of the specified screen. </summary>
		/// <param name="x11screen"> The screen pointer, that specifies  the appropriate screen structure. <see cref="IntPtr"/> </param>
		/// <returns> The white pixel value. Only black and white pixel are guaranteed for all XServer. <see cref="TPixel"/> </returns>
		[DllImport("X11")]
		extern public static TPixel XWhitePixelOfScreen(IntPtr x11screen);
		
		// Tested: O.K.
		/// <summary> Return the default colormap for allocation on the specified screen.
		/// Most routine allocations of color should be made out of this colormap. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The screen number, that specifies the appropriate screen on the host server. <see cref="TInt"/> </param>
		/// <returns> The requested default colormap on success, or IntPtr.Zero otherwise. <see cref="IntPtr"/> </returns>
		[DllImport ("libX11")]
		extern public static IntPtr XDefaultColormap (IntPtr x11display, X11.TInt screenNumber);
		
		// Tested: O.K.
		/// <summary> Look up the string name of a color with respect to the screen associated with the specified colormap. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="x11colormap"> The colormap to use. <see cref="IntPtr"/> </param>
		/// <param name="colorname"> The color's name to look up. <see cref="System.String"/> </param>
		/// <param name="x11color"> The exact color. <see cref="XColor"/> </param>
		/// <returns> The error indication. <see cref="System.Int32"/> </returns>
		[DllImport("libX11")]
		extern public static int XParseColor (IntPtr x11display, IntPtr x11colormap, [MarshalAs(UnmanagedType.LPStr)] string colorname, ref XColor x11color);
		
		[DllImport("libX11")]
		extern public static int XLookupColor (IntPtr x11display, IntPtr x11colormap, [MarshalAs(UnmanagedType.LPStr)] string colorname, ref XColor x11colorExactDef, ref XColor x11colorScrDef);
		
		[DllImport("libX11")]
		extern public static void XQueryColor (IntPtr x11display, IntPtr x11colormap, ref XColor x11color);
		
		// Tested: O.K.
		/// <summary> Allocate a read-only colormap entry corresponding to the closest RGB value supported by the hardware.
		/// XAllocColor() returns the pixel value of the color closest to the specified RGB elements supported by the hardware
		/// and returns the RGB value actually used. The corresponding colormap cell is read-only.
		/// Multiple clients that request the same effective RGB value can be assigned the same read-only entry,
		/// thus allowing entries to be shared. When the last client deallocates a shared cell, it is deallocated.
		/// XAllocColor() does not use or affect the flags in the XColor structure. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="colormap"> The colormap to use for nearest read-only color cell allocation. <see cref="IntPtr"/> </param>
		/// <param name="xcolor"> The color to allocate the nearest read-only color cell for. <see cref="XColor"/> </param>
		/// <returns> Nonzero on success, or zero otherwise. <see cref="System.Int32"/> </returns>
		[DllImport("libX11")]
		extern public static int XAllocColor (IntPtr x11display, IntPtr colormap, ref XColor xcolor);
		
		// Tested: O.K.
		/// <summary> Allocate a read-only colormap entry corresponding to the closest RGB value supported by the hardware.
		/// XAllocParsedColorByName() returns the pixel value of the color closest to the specified RGB elements supported by
		/// the hardware and returns the RGB value actually used. The corresponding colormap cell is read-only.
		/// Multiple clients that request the same effective RGB value can be assigned the same read-only entry,
		/// thus allowing entries to be shared. When the last client deallocates a shared cell, it is deallocated. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="screenNumber"> The screen number, that specifies the appropriate screen on the host server. <see cref="TInt"/> </param>
		/// <param name="colorname"> The color's name to look up. <see cref="System.String"/> </param>
		/// <returns> The cixel value of the requested color on success, or 1 as fallback. <see cref="TPixel"/> </returns>
		public static TPixel XAllocParsedColorByName (IntPtr x11display, X11.TInt screenNumber, [MarshalAs(UnmanagedType.LPStr)] string colorname)
		{
			if (x11display == IntPtr.Zero)
				return (TPixel)1;

			X11lib.XColor tmp = new X11lib.XColor ();
			X11lib.XParseColor (x11display, X11lib.XDefaultColormap (x11display, screenNumber), colorname, ref tmp);
			X11lib.XAllocColor (x11display, X11lib.XDefaultColormap (x11display, screenNumber), ref tmp);
			return tmp.pixel;
		}
		
		/// <summary> Free the cells represented by pixels whose values are in the pixels array. The planes argument should
		/// not have any bits set to 1 in common with any of the pixels. The set of all pixels is produced by ORing
		/// together subsets of the planes argument with the pixels. The request frees all of these pixels that were allocated
		/// by the client (using XAllocColor(), XAllocNamedColor(), XAllocColorCells(), and XAllocColorPlanes()). </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="colormap"> The colormap to use for color cell deallocation. <see cref="IntPtr"/> </param>
		/// <param name="pixels"> The array of pixel values that map to the cells in the specified colormap to free. <see cref="TPixel[]"/> </param>
		/// <param name="nPixels"> The number of colors to free. <see cref="TInt"/> </param>
		/// <param name="planes"> The planes to free. <see cref="TUlong"/> </param>
		[DllImport("libX11")]
		extern public static void XFreeColors (IntPtr x11display, IntPtr colormap, TPixel[] pixels, TInt nPixels, TUlong planes);

		// Tested: O.K.
		/// <summary> Return  the visual information for a visual that matches the specified depth and class for a screen.
		/// Because multiple visuals that match the specified depth and class can exist, the exact visual chosen is undefined. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="screenNumber"> The screen number, that specifies the appropriate screen on the host server. <see cref="TInt"/> </param>
		/// <param name="depth"> The requested depth of the screen. <see cref="TInt"/> </param>
		/// <param name="cls"> The requested class of the screen. <see cref="TInt"/> </param>
		/// <param name="visualInfo"> The matching visual information. <see cref="XVisualInfo"/> </param>
		/// <returns> The requested default depth. <see cref="TInt"/> </returns>
		[DllImport("libX11")]
		extern public static TInt XMatchVisualInfo (IntPtr x11display, TInt screenNumber, TInt depth, TInt cls, ref XVisualInfo visualInfo);

		#endregion
		
		// ##########################################################################################################
		// ###   Drawing
		// ##########################################################################################################
		
		#region Drawing preparation
		
		// Tested: O.K.
		/// <summary> Clear the entire area in the specified window and is equivalent to XClearArea(display, w, 0, 0, 0, 0, False).
		/// If the window has a defined background tile, the rectangle is tiled with a plane-mask of all ones and GXcopy function.
		/// If the window has background None, the contents of the window are not changed.
		/// If you specify a window whose class is InputOnly , a BadMatch error results. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="x11window"> The window to clear. <see cref="IntPtr"/> </param>
		[DllImport("libX11")]
		extern public static void XClearWindow (IntPtr x11display, IntPtr x11window);
			
		// Tested: O.K.
		/// <summary> Create a graphics context.
		/// It can be used with any destination drawable having the same root and depth as the specified drawable. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="x11drawable"> The drawable to draw on. <see cref="IntPtr"/> </param>
		/// <param name="valuemask"> Determine which components in the GC are to be set using the information in the specified values structure.
		/// This argument is the bitwise inclusive OR of zero or more of the valid GC component mask bits. <see cref="System.UInt64"/> </param>
		/// <param name="values"> The values as specified by the valuemask in a XGCValues structure. <see cref="IntPtr"/> </param>
		/// <returns> Thegraphics context on success, or IntPtr.Zero otherwise. <see cref="IntPtr"/> </returns>
		[DllImport("libX11")]
		extern public static IntPtr XCreateGC (IntPtr x11display, IntPtr x11drawable, TUlong valuesMask, IntPtr values);
		[DllImport("libX11")]
		extern public static IntPtr XCreateGC (IntPtr x11display, IntPtr x11drawable, TUlong valuesMask, ref XGCValues values);
		
		// Tested: O.K.
		/// <summary>  Destroy the specified GC as well as all the associated storage. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="x11gc"> The graphics context to destroy. <see cref="IntPtr"/> </param>
		[DllImport("libX11")]
		extern public static void XFreeGC (IntPtr x11display, IntPtr x11gc);
		
		/// <summary> Get the resource ID for the indicated graphics context. </summary>
		/// <param name="x11gc"> The graphics context to get the resource ID for. <see cref="System.IntPtr"/> </param>
		/// <returns> The resource ID for the indicated graphics context. <see cref="System.Int32"/> </returns>
		[DllImport("libX11")]
		extern public static TInt XGContextFromGC (IntPtr x11gc);
		
		// Tested: O.K.
		/// <summary> Set the foreground to the specified graphics context. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="x11gc"> The graphics context. <see cref="System.IntPtr"/> </param>
		/// <param name="foreground"> The foreground to set. <see cref="TPixel"/> </param>
		[DllImport("libX11")]
		extern public static void XSetForeground (IntPtr x11display, IntPtr x11gc, TPixel foreground);

		/// <summary> Set the background to the specified graphics context. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="x11gc"> The graphics context. <see cref="System.IntPtr"/> </param>
		/// <param name="background"> The background to set. <see cref="TPixel"/> </param>
		[DllImport("libX11")]
		extern public static void XSetBackground (IntPtr x11display, IntPtr x11gc, TPixel background);
		
		// Tested: O.K.
		/// <summary> Set the pixel manipulation function of indicated graphics context. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="x11gc"> The graphics context to set the pixel manipulation function for. <see cref="System.IntPtr"/> </param>
		/// <param name="function"> The pixel manipulation function to set. <see cref="XGCFunction"/> </param>
		[DllImport("libX11")]
		extern public static void XSetFunction(IntPtr x11display, IntPtr x11gc, XGCFunction function);

		// Tested: O.K.
		/// <summary> The XGetGCValues() function returns the components specified by valuemask for the specified GC.
		/// If the valuemask contains a valid set of GC mask bits (GCFunction, GCPlaneMask, GCForeground, GCBackground,
		/// GCLineWidth, GCLineStyle, GCCapStyle, GCJoinStyle, GCFillStyle, GCFillRule, GCTile, GCStipple, GCTileStipXOrigin,
		/// GCTileStipYOrigin, GCFont, GCSubwindowMode, GCGraphicsExposures, GCClipXOrigin, GCCLipYOrigin, GCDashOffset,
		/// or GCArcMode) and no error occurs, XGetGCValues() sets the requested components in values and returns nonzero.
		/// Otherwise, it returns a zero status. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="System.IntPtr"/> </param>
		/// <param name="x11gc"> The crapchics context to get the values for. <see cref="System.IntPtr"/> </param>
		/// <param name="valuemask"> The flags, determining which values to get. <see cref="System.UInt32"/> </param>
		/// <param name="values"> The structure containing the investigated values. <see cref="XGCValues"/> </param>
		/// <returns> Nonzero on success, or zero otherwise. <see cref="System.Int32"/> </returns>
		/// <remarks> Note that the clip-mask and dash-list (represented by the GCClipMask and GCDashList bits,
		/// respectively, in the valuemask) cannot be requested. Also note that an invalid resource ID (with one
		/// or more of the three most-significant bits set to 1) will be returned for GCFont, GCTile, and GCStipple
		/// if the component has never been explicitly set by the client. </remarks>
		[DllImport("libX11")]
		extern public static int XGetGCValues(IntPtr x11display, IntPtr x11gc, TUint valuemask, ref XGCValues values);

		// Tested: O.K.
		/// <summary> Change the components specified by valuemask for the specified GC.
		/// The values argument contains the values to be set. The values and restrictions are the same as for XCreateGC().
		/// Changing the clip-mask overrides any previous XSetClipRectangles() request on the context.
		/// Changing the dash-offset or dash-list overrides any previous XSetDashes() request on the context.
		/// The order in which components are verified and altered is server-dependent.
		/// If an error is generated, a subset of the components may have been altered. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="x11gc"> The crapchics context to change the values for. <see cref="IntPtr"/> </param>
		/// <param name="valuemask"> The flags, determining which values to change. <see cref="System.UInt32"/> </param>
		/// <param name="values"> The structure containing the change values. <see cref="XGCValues"/> </param>
		[DllImport("libX11")]
		extern public static void XChangeGC(IntPtr x11display, IntPtr x11gc, X11.TUint valuemask, ref XGCValues values);

		#endregion
		
		#region Shape drawing
		
		// Tested: O.K.
		/// <summary> Draw a line between the specified set of points (x1, y1) and (x2, y2) using the components of the specified GC.
		/// It does not perform joining at coincident endpoints. For any given line, XDrawLine() does not draw a pixel more than once.
		/// If lines intersect, the intersecting pixels are drawn multiple times. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="x11drawable"> The drawable to draw on. <see cref="IntPtr"/> </param>
		/// <param name="x11gc"> The crapchics context to use for drawing. <see cref="IntPtr"/> </param>
		/// <param name="x"> The point x-coordinate. <see cref="System.Int32"/> </param>
		/// <param name="y"> The point y-coordinate. <see cref="System.Int32"/> </param>
		/// <remarks> XDrawPoint() use these GC components: function, plane-mask, foreground, subwindow-mode, clip-x-origin, clip-y-origin
		/// and clip-mask. XDrawPoint() also uses these GC mode-dependent component: foreground. </remarks>
		[DllImport("libX11")]
		extern public static void XDrawPoint (IntPtr x11display, IntPtr x11drawable, IntPtr X11gc, X11.TInt x, X11.TInt y);
		
		// Tested: O.K.
		/// <summary> Draw a line between the specified set of points (x1, y1) and (x2, y2) using the components of the specified GC.
		/// It does not perform joining at coincident endpoints. For any given line, XDrawLine() does not draw a pixel more than once.
		/// If lines intersect, the intersecting pixels are drawn multiple times. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="x11drawable"> The drawable to draw on. <see cref="IntPtr"/> </param>
		/// <param name="x11gc"> The crapchics context to use for drawing. <see cref="IntPtr"/> </param>
		/// <param name="x1"> The start point x-coordinate. <see cref="System.Int32"/> </param>
		/// <param name="y1"> The start point y-coordinate. <see cref="System.Int32"/> </param>
		/// <param name="x2"> The end point x-coordinate. <see cref="System.Int32"/> </param>
		/// <param name="y2"> The end point x-coordinate. <see cref="System.Int32"/> </param>
		/// <remarks> XDrawLine() use these GC components: function, plane-mask, line-width, line-style, cap-style, fill-style, subwindow-mode,
		/// clip-x-origin, clip-y-origin, and clip-mask. XDrawLine() also uses these GC mode-dependent components: foreground, background, tile,
		/// stipple, tile-stipple-x-origin, tile-stipple-y-origin, dash-offset, and dash-list. </remarks>
		[DllImport("libX11")]
		extern public static void XDrawLine (IntPtr x11display, IntPtr x11drawable, IntPtr X11gc, X11.TInt x1, X11.TInt y1, X11.TInt x2, X11.TInt y2);
		
		// Tested: O.K.
		/// <summary> Fill the specified rectangle as if a four-point FillPolygon protocol request.
		/// For any given rectangle, XFillRectangle() does not draw a pixel more than once. If rectangles intersect, the intersecting pixels are drawn multiple times. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="x11drawable"> The drawable to draw on. <see cref="IntPtr"/> </param>
		/// <param name="x11gc"> The crapchics context to use for drawing. <see cref="IntPtr"/> </param>
		/// <param name="x"> The left top corner x-coordinate. <see cref="SX11.TInt"/> </param>
		/// <param name="y"> The left top corner y-coordinate. <see cref="SX11.TInt"/> </param>
		/// <param name="width"> The rectangle width. <see cref="X11.TUint"/> </param>
		/// <param name="height"> The rectangle height. <see cref="X11.TUint"/> </param>
		/// <remarks> The function uses these GC components: function, plane-mask, fill-style, subwindow-mode, clip-x-origin, clip-y-origin, and clip-mask.
		/// It also uses these GC mode-dependent components: foreground, background, tile, stipple, tile-stipple-x-origin, and tile-stipple-y-origin. </remarks>
		[DllImport("libX11")]
		extern public static void XFillRectangle (IntPtr x11display, IntPtr x11drawable, IntPtr x11gc, X11.TInt x, X11.TInt y, X11.TUint width, X11.TUint height);
		
		// Tested: O.K.
		/// <summary> Fill region closed by the infinitely thin path described by the specified arc and, depending on the arc-mode specified in the GC,
		/// one (ArcChord) or two (ArcPieSlice) line segments. For ArcChord , the single line segment joining the endpoints of the arc is used.
		/// For ArcPieSlice , the two line segments joining the endpoints of the arc with the center point are used. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="x11drawable"> The drawable to draw on. <see cref="IntPtr"/> </param>
		/// <param name="x11gc"> The crapchics context to use for drawing. <see cref="IntPtr"/> </param>
		/// <param name="x"> The left top bounding rectangle x-coordinate. <see cref="SX11.TInt"/> </param>
		/// <param name="y"> The left top bounding rectangle y-coordinate. <see cref="SX11.TInt"/> </param>
		/// <param name="width"> The bounding rectangle width. <see cref="X11.TUint"/> </param>
		/// <param name="height"> The bounding rectangle height. <see cref="X11.TUint"/> </param>
		/// <param name="angle1"> The start of the arc relative to the three-o'clock position from the center, in units of degrees * 64. <see cref="X11.TInt"/> </param>
		/// <param name="angle2"> The the path and extent of the arc relative to the start of the arc, in units of degrees * 64.  <see cref="X11.TInt"/> </param>
		/// <remarks> The function uses these GC components: : function, plane-mask, fill-style, arc-mode, subwindow-mode, clip-x-origin, clip-y-origin, and clip-mask.
		/// It also uses these GC mode-dependent components: foreground, background, tile, stipple, tile-stipple-x-origin, and tile-stipple-y-origin. </remarks>
		[DllImport("libX11")]
		extern public static void XFillArc (IntPtr x11display, IntPtr x11drawable, IntPtr x11gc, X11.TInt x, X11.TInt y, X11.TUint width, X11.TUint height, X11.TInt angle1, X11.TInt angle2);
		
		#endregion

		#region String drawing

		// Tested: O.K.
		/// <summary> Return the bounding box of the specified 8-bit character string in the specified font or the font contained in the specified GC.
		/// This function queries the X server and, therefore, suffer the round-trip overhead that is avoided by XTextExtents().
		/// The function returns a XCharStruct structure, whose members are set to the values as follows. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="resourceID"> Specifies either the font ID or the graphics context ID that contains the font. <see cref="X11.TInt"/> </param>
		/// <param name="text"> The text to measure. <see cref="System.SByte[]"/> </param>
		/// <param name="length"> The length of the text to measure. <see cref="System.Int32"/> </param>
		/// <param name="direction"> The direction hint (FontLeftToRight or FontRightToLeft). <see cref="X11.TInt"/> </param>
		/// <param name="fontAscent"> The font ascent. <see cref="X11.TInt"/> </param>
		/// <param name="fontDescent"> The font descent. <see cref="X11.TInt"/> </param>
		/// <param name="overall"> The overall size stored in the specified XCharStruct structure.
		/// The ascent member is set to the maximum of the ascent metrics of all characters in the string.
		/// The descent member is set to the maximum of the descent metrics.
		/// The width member is set to the sum of the character-width metrics of all characters in the string.
		/// For each character in the string, let W be the sum of the character-width metrics of all characters preceding it in the string.
		/// Let L be the left-side-bearing metric of the character plus W. Let R be the right-side-bearing metric of the character plus W.
		/// The lbearing member is set to the minimum L of all characters in the string. The rbearing member is set to the maximum R. <see cref="XCharStruct"/> </param>
		/// <remarks> Characters with all zero metrics are ignored.
		/// If the font has no defined default_char, the undefined characters in the string are also ignored. </remarks>
		[DllImport("libX11")]
		extern public static void XQueryTextExtents (IntPtr x11display, X11.TInt resourceID, X11.TChar[] text, X11.TInt length, ref X11.TInt direction, ref X11.TInt fontAscent, ref X11.TInt fontDescent, ref XCharStruct overall);
			
		/// <summary> Return the bounding box of the specified 8-bit character string in the specified font or the font contained in the specified GC.
		/// This function queries the X server and, therefore, suffer the round-trip overhead that is avoided by XTextExtents().
		/// The function returns a XCharStruct structure, whose members are set to the values as follows. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="resourceID"> Specifies either the font ID or the graphics context ID that contains the font. <see cref="X11.TInt"/> </param>
		/// <param name="text"> The text to measure. <see cref="System.SByte[]"/> </param>
		/// <param name="length"> The length of the text to measure. <see cref="System.Int32"/> </param>
		/// <param name="direction"> The direction hint (FontLeftToRight or FontRightToLeft). <see cref="X11.TInt"/> </param>
		/// <param name="fontAscent"> The font ascent. <see cref="X11.TInt"/> </param>
		/// <param name="fontDescent"> The font descent. <see cref="X11.TInt"/> </param>
		/// <param name="overall"> The overall size stored in the specified XCharStruct structure.
		/// The ascent member is set to the maximum of the ascent metrics of all characters in the string.
		/// The descent member is set to the maximum of the descent metrics.
		/// The width member is set to the sum of the character-width metrics of all characters in the string.
		/// For each character in the string, let W be the sum of the character-width metrics of all characters preceding it in the string.
		/// Let L be the left-side-bearing metric of the character plus W. Let R be the right-side-bearing metric of the character plus W.
		/// The lbearing member is set to the minimum L of all characters in the string. The rbearing member is set to the maximum R. <see cref="XCharStruct"/> </param>
		/// <remarks> Characters with all zero metrics are ignored.
		/// If the font has no defined default_char, the undefined characters in the string are also ignored.
		/// For fonts defined with linear indexing rather than 2-byte matrix indexing, each XChar2b structure is interpreted as a 16-bit number
		/// with byte1 as the most-significant byte. If the font has no defined default character, undefined characters in the string are taken
		/// to have all zero metrics.</remarks>
		[DllImport("libX11")]
		extern public static void XQueryTextExtents16 (IntPtr x11display, X11.TInt resourceID, XChar2b[] text, X11.TInt length, ref X11.TInt direction, ref X11.TInt fontAscent, ref X11.TInt fontDescent, ref XCharStruct overall);
			
		// Tested: O.K.
		/// <summary> Draw a string without cleaning the background.
		/// Each character image, as defined by the font in the GC, is treated as an additional mask for a fill operation on the drawable.
		/// The drawable is modified only where the font character has a bit set to 1. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="x11drawable"> The drawable to draw on. <see cref="IntPtr"/> </param>
		/// <param name="x11gc"> The crapchics context to use for drawing. <see cref="IntPtr"/> </param>
		/// <param name="x"> The left top corner x-coordinate. <see cref="X11.TInt"/> </param>
		/// <param name="y"> The left top corner y-coordinate. <see cref="X11.TInt"/> </param>
		/// <param name="text"> The text to draw. <see cref="System.SByte[]"/> </param>
		/// <param name="length"> The length of the text to draw. <see cref="X11.TInt"/> </param>
		/// <remarks> The function uses these GC components: function, plane-mask, fill-style, font, subwindow-mode, clip-x-origin, clip-y-origin, and clip-mask.
		/// It also use these GC mode-dependent components: foreground, background, tile, stipple, tile-stipple-x-origin, and tile-stipple-y-origin.</remarks>
		[DllImport("libX11")]
		extern public static void XDrawString (IntPtr x11display, IntPtr x11drawable, IntPtr x11gc, X11.TInt x, X11.TInt y, X11.TChar[] text, X11.TInt length);
		
		// Tested: O.K.
		/// <summary> Draw a string without cleaning the background.
		/// Each character image, as defined by the font in the GC, is treated as an additional mask for a fill operation on the drawable.
		/// The drawable is modified only where the font character has a bit set to 1. </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="x11drawable"> The drawable to draw on. <see cref="IntPtr"/> </param>
		/// <param name="x11gc"> The crapchics context to use for drawing. <see cref="IntPtr"/> </param>
		/// <param name="x"> The left top corner x-coordinate. <see cref="X11.TInt"/> </param>
		/// <param name="y"> The left top corner y-coordinate. <see cref="X11.TInt"/> </param>
		/// <param name="text"> The text to draw. <see cref="System.SByte[]"/> </param>
		/// <param name="length"> The length of the text to draw. <see cref="X11.TInt"/> </param>
		/// <remarks> The function uses these GC components: function, plane-mask, fill-style, font, subwindow-mode, clip-x-origin, clip-y-origin, and clip-mask.
		/// It also use these GC mode-dependent components: foreground, background, tile, stipple, tile-stipple-x-origin, and tile-stipple-y-origin.</remarks>
		[DllImport("libX11")]
		extern public static void XDrawString16 (IntPtr x11display, IntPtr x11drawable, IntPtr x11gc, X11.TInt x, X11.TInt y, XChar2b[] text, X11.TInt length);
		
		// Tested: O.K.
        [DllImport ("libX11")]
        extern public static void XmbDrawString   (IntPtr x11display, IntPtr x11drawable, IntPtr fontSet, IntPtr x11gc, int x, int y, sbyte[] text, int length);

        [DllImport ("libX11")]
        extern public static void XwcDrawString   (IntPtr x11display, IntPtr x11drawable, IntPtr fontSet, IntPtr x11gc, int x, int y, X11.TWchar[] text, int length);

		// Tested: O.K.
        [DllImport ("libX11")]
        extern public static void Xutf8DrawString (IntPtr x11display, IntPtr x11drawable, IntPtr fontSet, IntPtr x11gc, int x, int y, sbyte[] text, int length);
		
		#endregion

		// Applies changes, bot not sure if it's O.K.
		// Usage:
		// Xlib.XRecolorCursor(xdisplay, cursor, ref wht, ref gry);
		[DllImport ("libX11")]
		internal extern static void XRecolorCursor (IntPtr x11display, IntPtr cursor, ref XColor foregroundColor, ref XColor backgroundColor);
		
		// Tested: O.K.
		/// <summary> Copies the first event from the event queue into the specified XEvent structure and then removes it from the queue.
		/// If the event queue is empty, XNextEvent() flushes the output buffer and blocks until an event is received.  </summary>
		/// <param name="x11display"> The display pointer, that specifies the connection to the X server. <see cref="IntPtr"/> </param>
		/// <param name="x11event"> Returns the next event in the queue. <see cref="XEvent"/> </param>
		[DllImport("libX11")]
		public extern static void XNextEvent(IntPtr x11display, ref XEvent x11event);
		
		[DllImport("libX11")]
		public extern static bool XWindowEvent(IntPtr x11display, IntPtr x11window, EventMask event_mask, ref XEvent x11event);
		
		[DllImport("libX11")]
		public extern static bool XCheckWindowEvent(IntPtr x11display, IntPtr x11window, EventMask event_mask, ref XEvent x11event);
		
		[DllImport("libX11")]
		public extern static bool XCheckTypedWindowEvent(IntPtr x11display, IntPtr x11window, XEventName event_type, ref XEvent x11event);

		[DllImport("libX11", EntryPoint = "XFilterEvent")]
		public extern static bool XFilterEvent(ref XEvent xevent, IntPtr x11window);

		[DllImport("libX11")]
		public extern static void XPeekEvent(IntPtr display, ref XEvent x11event);
		
		// Tested: O.K.
		/// <returns> Nonzero on success, or zero otherwise (if the conversion to wire protocol format failed). <see cref="X11.TInt"/> </returns>
        [DllImport("libX11")]
		extern public static TInt XSendEvent(IntPtr x11display, IntPtr x11window, TBoolean propagate, TLong eventMask, ref XEvent eventSend);

		#endregion
		
	}
}

