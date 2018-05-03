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

using Utils;

namespace Xrw
{
	/// <summary> The "Roma Widget Set" ressources. </summary>
	internal class XrwResources
	{
        
		/// <summary> Keep the current thread's current UI culture property for all resource lookups. </summary>
        private static System.Globalization.CultureInfo resourceCulture = null;

		/// <summary> Get current thread's current UI culture LCID. </summary>
		private static int Lcid
		{	get
			{
				if (resourceCulture != null)
					return resourceCulture.LCID;
				else
					return System.Globalization.CultureInfo.CurrentUICulture.LCID;
			}
		}
		
        /// <summary> Get or set the current thread's current UI culture property for all resource lookups. </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
		
		#region Texts
			
		internal static string TXT_BUTTON_CANCEL
		{	get	{	switch (Lcid)
					{	case 1031:
							return "Abbrechen";
						default:
							return "Cancel";
					}
				}
		}
			
		internal static string TXT_BUTTON_OK
		{	get	{	switch (Lcid)
					{	case 1031:
							return "OK";
						default:
							return "OK";
					}
				}
		}
		
		#endregion

		#region Images
		
		#endregion

	}
        
}