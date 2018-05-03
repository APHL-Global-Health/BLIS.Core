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

namespace X11
{
	/// <summary> The orientation, a collection can have. </summary>
	public enum TOrientation
	{
		/// <summary> The collection is oriented horizontally. </summary>
		Horizontal,
		
		/// <summary> The collection is oriented vertically. </summary>
		Vertical
	}
	
	/// <summary> The shadow type to apply a 3D look. </summary>
	public enum TFrameType
	{
		/// <summary> No 3D effect at all. </summary>
		None,
		
		/// <summary> The frame appears in raised 3D effect. </summary>
		Raised,
		
		/// <summary> The frame appears in sunken 3D effect. </summary>
		Sunken,
		
		/// <summary> The frame appears in chiseled 3D effect. </summary>
		Chiseled,
		
		/// <summary> The frame appears in ledged 3D effect. </summary>
		Ledged
	}
	
	/// <summary> Platform specific integer point structure. </summary>
	public struct TPoint
	{
		/// <summary> The x-coordinate. </summary>
		public int X;
		
		/// <summary> The x-coordinate. </summary>
		public int Y;
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="x"> The x-coordinate. <see cref="System.Int32"/> </param>
		/// <param name="y"> The y-coordinate. <see cref="System.Int32"/> </param>
		public TPoint (int x, int y)
		{
			X = x;
			Y = y;
		}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="x"> The x-coordinate. <see cref="TInt"/> </param>
		/// <param name="y"> The y-coordinate. <see cref="TInt"/> </param>
		public TPoint (TInt x, TInt y)
		{
			X = (int)x;
			Y = (int)y;
		}
		
		/// <summary> Add the indicated size. </summary>
		/// <param name="summand"> The size to add. <see cref="TSize"/> </param>
		public void Add (TSize summand)
		{
			X = X + summand.Width;
			Y = Y + summand.Height;
		}
		
		/// <summary> Substract the indicated size. </summary>
		/// <param name="subtrahend"> The size to substract. <see cref="TSize"/> </param>
		public void Substract (TSize subtrahend)
		{
			X = X - subtrahend.Width;
			Y = Y - subtrahend.Height;
		}
	}
	
	/// <summary> Platform specific integer size structure. </summary>
	public struct TSize
	{
		/// <summary> The width. </summary>
		public int Width;
		
		/// <summary> The height. </summary>
		public int Height;
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="width"> The width. <see cref="System.Int32"/> </param>
		/// <param name="height"> The height. <see cref="System.Int32"/> </param>
		public TSize (int width, int height)
		{
			Width = width;
			Height = height;
		}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="width"> The width. <see cref="TInt"/> </param>
		/// <param name="height"> The height. <see cref="TInt"/> </param>
		public TSize (TInt width, TInt height)
		{
			Width = (int)width;
			Height = (int)height;
		}
		
		/// <summary> Add the indicated size. </summary>
		/// <param name="width"> The width to add. <see cref="System.Int32"/> </param>
		/// <param name="height"> The height to add. <see cref="System.Int32"/> </param>
		public void Add (int width, int height)
		{
			Width  = Width + width;
			Height = Height + height;
		}
		
		/// <summary> Add the indicated size. </summary>
		/// <param name="summand"> The size to add. <see cref="TSize"/> </param>
		public void Add (TSize summand)
		{
			Width  = Width + summand.Width;
			Height = Height + summand.Height;
		}
		
		/// <summary> Substract the indicated size. </summary>
		/// <param name="subtrahend"> The size to substract. <see cref="TSize"/> </param>
		public void Substract (TSize subtrahend)
		{
			Width  = Width - subtrahend.Width;
			Height = Height - subtrahend.Height;
		}

	}
	
	/// <summary> Platform specific integer rectangle structure. </summary>
	public struct TRectangle
	{
		/// <summary> The x-coordinate. </summary>
		public int X;
		
		/// <summary> The x-coordinate. </summary>
		public int Y;

		/// <summary> The width. </summary>
		public int Width;
		
		/// <summary> The height. </summary>
		public int Height;
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="x"> The x-coordinate. <see cref="TInt"/> </param>
		/// <param name="y"> The y-coordinate. <see cref="TInt"/> </param>
		/// <param name="width"> The width. <see cref="TInt"/> </param>
		/// <param name="height"> The height. <see cref="TInt"/> </param>
		public TRectangle(int x, int y, int width, int height)
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;
		}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="position"> The top left position. <see cref="TPoint"/> </param>
		/// <param name="width"> The width. <see cref="System.Int32"/> </param>
		/// <param name="height"> The height. <see cref="System.Int32"/> </param>
		public TRectangle(TPoint position, int width, int height)
		{
			X = position.X;
			Y = position.Y;
			Width = width;
			Height = height;
		}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="x"> The x-coordinate. <see cref="System.Int32"/> </param>
		/// <param name="y"> The y-coordinate. <see cref="System.Int32"/> </param>
		/// <param name="size"> The size. <see cref="TSize"/> </param>
		public TRectangle(int x, int y, TSize size)
		{
			X = x;
			Y = y;
			Width = size.Width;
			Height = size.Height;
		}
		
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="position"> The top left position. <see cref="TPoint"/> </param>
		/// <param name="size"> The size. <see cref="TSize"/> </param>
		public TRectangle(TPoint position, TSize size)
		{
			X = position.X;
			Y = position.Y;
			Width = size.Width;
			Height = size.Height;
		}

		/// <summary> Get the top coordinate. </summary>
		public int Top
		{
			get { return Y;	}
		}
		
		/// <summary> Get the left coordinate. </summary>
		public int Left
		{
			get { return X;	}
		}
		
		/// <summary> Get the bottom coordinate. </summary>
		public int Bottom
		{
			get { return Y + Height;	}
		}
		
		/// <summary> Get the right coordinate. </summary>
		public int Right
		{
			get { return X + Width;	}
		}
		
		/// <summary> Get the top left position. </summary>
		public TPoint Position
		{
			get { return new TPoint (X, Y);	}
		}
		
		/// <summary> Get the size. </summary>
		public TSize Size
		{
			get { return new TSize (Width, Height);	}
		}
		
		/// <summary> Inflate the rectangle consistent in x and y direction. </summary>
		/// <param name="leftRight"> The value to inflate the left and right boundary. <see cref="System.Int32"/> </param>
		/// <param name="topBottom"> The value to inflate the top and bottom boundary. <see cref="System.Int32"/> </param>
		public TRectangle Inflate (int leftRight, int topBottom)
		{
			X -= leftRight;
			Width += leftRight * 2;
			Y -= topBottom;
			Height += topBottom * 2;
			
			return this;
		}
	}

}

