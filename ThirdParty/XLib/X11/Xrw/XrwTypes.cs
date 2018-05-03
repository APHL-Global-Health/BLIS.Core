// =====================
// The "Roma Widget Set"
// =====================

/*
 * Created by Mono Develop 2.4.1.
 * User: PloetzS
 * Date: November 2013
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
	
	/// <summary> Provide a text line with geometry to make text processing fast and easy. </summary>
	public class Line
	{
		/// <summary> The line's text. </summary>
		public X11.TChar[]		Text;
		
		/// <summary> The line's width in pixel. </summary>
		public int				WidthInPixel;
		
		/// <summary> The line's height in pixel. </summary>
		public int				HeightInPixel;
		
		/// <summary> Hidden default constructor. </summary>
		private Line ()
		{	;	}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="text"> The line's text. <see cref="TChar[]"/> </param>
		/// <param name="widthInPixel"> The line's width in pixel. <see cref="System.Int32"/> </param>
		/// <param name="heightInPixel"> The line's height in pixel. <see cref="System.Int32"/> </param>
		public Line (TChar[] text, int widthInPixel, int heightInPixel)
		{
			Text = text;
			WidthInPixel = widthInPixel;
			HeightInPixel = heightInPixel;
		}
	}
	
	/// <summary> Provide a multi line to processed a label line by line fast and easy. </summary>
	public class Multiline : List<Line>
	{
		/// <summary> Hidden default constructor. </summary>
		private Multiline ()
		{	;	}
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="visibleRectObj">The rect object to initialize the lines for. <see cref="XrwVisibleRectObj"/> </param>
		/// <param name="text"> The label text. A <see cref="System.String"/> </param>
		public Multiline (XrwVisibleRectObj visibleRectObj, string text)
		{
			if (visibleRectObj == null)
				throw new ArgumentNullException ("labelWidget");

			text = text.Replace ("\r\n", "\n");
			if (string.IsNullOrEmpty (text))
			{
				TChar[] lineText = {};
				this.Add (new Line (lineText, 0, 0));
				return;
			}
			
			string [] lines = text.Split ('\n');
			for (int countLines = 0; countLines < lines.Length; countLines++)
			{
				TChar[] lineText = X11Utils.StringToSByteArray (lines[countLines]);
				TSize textMeasure   = visibleRectObj.MeasureTextLine (visibleRectObj.Display, visibleRectObj.GC, lineText);
				this.Add (new Line (lineText, textMeasure.Width, textMeasure.Height));
			}
		}
		
		/// <summary> Get the measured text size in pixel. </summary>
		/// <param name="summand"> The size to add. <see cref="TSize"/> </param>
		/// <returns> The measured text size in pixel. <see cref="TSize"/> </returns>
		public TSize Measure(TSize summand)
		{
			TSize textMeasure = new TSize (0, 0);
			
			for (int cntLines = 0; cntLines < this.Count; cntLines++)
			{
				textMeasure.Width   = Math.Max (textMeasure.Width, this[cntLines].WidthInPixel);
				textMeasure.Height += this[cntLines].HeightInPixel;
			}
			textMeasure.Add(summand);
			return textMeasure;
		}
		
		/// <summary> Get the measured text size in pixel. </summary>
		/// <param name="startLineIndex"> The zero based index of the first line to measure. <see cref="System.Int32"/> </param>
		/// <param name="endLineIndex"> The zero based index of the last line to measure. <see cref="System.Int32"/> </param>
		/// <returns> The measured text size in pixel. <see cref="TSize"/> </returns>
		public TSize Measure(int startLineIndex, int endLineIndex)
		{
			TSize textMeasure = new TSize (0, 0);
			
			if (startLineIndex >= this.Count || endLineIndex < startLineIndex)
				return textMeasure;
			
			for (int cntLines = startLineIndex; cntLines <= endLineIndex; cntLines++)
			{
				textMeasure.Width   = Math.Max (textMeasure.Width, this[cntLines].WidthInPixel);
				textMeasure.Height += this[cntLines].HeightInPixel;
			}
			return textMeasure;
		}
		
		/// <summary> Return the number of complete drawable lines, that fit into indicated available height. </summary>
		/// <param name="availableHeight"> The available height in pixel. <see cref="System.Int32"/> </param>
		/// <returns> The number of lines that completely fit into indicated available height. <see cref="System.Int32"/> </returns>
		public int CompleteDrawableLines (int availableHeight)
		{
			int textHeight = 0;
			
			for (int cntLines = 0; cntLines < this.Count; cntLines++)
			{
				textHeight += this[cntLines].HeightInPixel;
				if (textHeight > availableHeight)
					return cntLines;
			}
			return this.Count;
		}
	}
}

