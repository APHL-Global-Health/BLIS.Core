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

using X11;

namespace Xrw
{
	/// <summary> Wrap the XCrossingEvent to 1) avoid meaningless structure copy constructor calls and 2) provide a return value. </summary>
	public class XrwCrossingEvent
	{
		/// <summary> The underlaying XCrossingEvent. </summary>
		public XCrossingEvent Event;
		
		/// <summary> The event processing result. </summary>
		/// <remarks> Nonzero values for stop further prosessing this event, zero value for continue processing this event. </remarks>
		public int Result = 0;
		
		/// <summary> Initializing constructor. </summary>
		/// <param name="evt"> The event to wrap. <see cref="XCrossingEvent"/> </param>
		public XrwCrossingEvent (ref XCrossingEvent evt)
		{
			Event = evt;
		}
	}
}

