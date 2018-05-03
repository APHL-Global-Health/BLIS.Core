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

namespace X11
{
	public static class X11Utils
	{
		
	    [StructLayout(LayoutKind.Explicit)]
		private struct IntPtrToByteArray
		{
	        [FieldOffset(0)]
			public IntPtr ptr;
	        [FieldOffset(0)]
			public byte[] str;
		}
		
		public static X11.TChar[] StringToSByteArray (string text)
		{
			if (string.IsNullOrEmpty (text))
				return new X11.TChar[0];
			
			X11.TChar[] result = new X11.TChar[text.Length];
			for (int charIndex = 0;  charIndex < text.Length; charIndex++)
				result[charIndex] = (X11.TChar)text[charIndex];
			
			return result;
		}
		
		public static string SByteArrayToString (X11.TChar[] text)
		{
			if (text.Length == 0)
				return "";
			
			string result = "";
			for (int charIndex = 0; charIndex < text.Length; charIndex++)
				result += (char)text[charIndex];
			
			return result;
		}
		
		public static X11lib.XChar2b[] StringToXChar2bArray (string text)
		{
			if (string.IsNullOrEmpty (text))
				return new X11lib.XChar2b[0];
			
			char[]  buffer = text.ToCharArray();
			X11lib.XChar2b[] result = new X11lib.XChar2b[text.Length];
			for (int charIndex = 0;  charIndex < buffer.Length; charIndex++)
			{
				result[charIndex].byte1 = (X11.TUchar)0;
				result[charIndex].byte2 = (X11.TUchar)buffer[charIndex];
			}
			return result;
		}

	}
}

