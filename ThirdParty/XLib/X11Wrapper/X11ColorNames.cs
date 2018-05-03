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
	
	public static class X11ColorNames
	{
		public static string	AliceBlue				= "AliceBlue";				//#F0F8FF
		public static string	AntiqueWhite			= "AntiqueWhite";			//#FAEBD7
		public static string	Aqua					= "Aqua";					//#00FFFF
		public static string	Aquamarine				= "Aquamarine";				//#7FFFD4
		public static string	Azure					= "Azure";					//#F0FFFF
		public static string	Beige					= "Beige";					//#F5F5DC
		public static string	Bisque					= "Bisque";					//#FFE4C4
		public static string	Black					= "Black";					//#000000
		public static string	BlanchedAlmond			= "BlanchedAlmond";			//#FFEBCD
		public static string	Blue					= "Blue";					//#0000FF
		public static string	BlueViolet				= "BlueViolet";				//#8A2BE2
		public static string	Brown					= "Brown";					//#A52A2A
		public static string	BurlyWood				= "BurlyWood";				//#DEB887
		public static string	CadetBlue				= "CadetBlue";				//#5F9EA0
		public static string	Chartreuse				= "Chartreuse";				//#7FFF00
		public static string	Chocolate				= "Chocolate";				//#D2691E
		public static string	Coral					= "Coral";					//#FF7F50
		public static string	CornflowerBlue			= "CornflowerBlue";			//#6495ED
		public static string	Cornsilk				= "Cornsilk";				//#FFF8DC
		public static string	Crimson					= "Crimson";				//#DC143C
		public static string	Cyan					= "Cyan";					//#00FFFF
		public static string	DarkBlue				= "DarkBlue";				//#00008B
		public static string	DarkCyan				= "DarkCyan";				//#008B8B
		public static string	DarkGoldenrod			= "DarkGoldenrod";			//#B8860B
		public static string	DarkGray				= "DarkGray";				//#A9A9A9
		public static string	DarkGreen				= "DarkGreen";				//#006400
		public static string	DarkKhaki				= "DarkKhaki";				//#BDB76B
		public static string	DarkMagenta				= "DarkMagenta";			//#8B008B
		public static string	DarkOliveGreen			= "DarkOliveGreen";			//#556B2F
		public static string	DarkOrange				= "DarkOrange";				//#FF8C00
		public static string	DarkOrchid				= "DarkOrchid";				//#9932CC
		public static string	DarkRed					= "DarkRed";				//#8B0000
		public static string	DarkSalmon				= "DarkSalmon";				//#E9967A
		public static string	DarkSeaGreen			= "DarkSeaGreen";			//#8FBC8F
		public static string	DarkSlateBlue			= "DarkSlateBlue";			//#483D8B
		public static string	DarkSlateGray			= "DarkSlateGray";			//#2F4F4F
		public static string	DarkTurquoise			= "DarkTurquoise";			//#00CED1
		public static string	DarkViolet				= "DarkViolet";				//#9400D3
		public static string	DeepPink				= "DeepPink";				//#FF1493
		public static string	DeepSkyBlue				= "DeepSkyBlue";			//#00BFFF
		public static string	DimGray					= "DimGray";				//#696969
		public static string	DodgerBlue				= "DodgerBlue";				//#1E90FF
		public static string	FireBrick				= "FireBrick";				//#B22222
		public static string	FloralWhite				= "FloralWhite";			//#FFFAF0
		public static string	ForestGreen				= "ForestGreen";			//#228B22
		public static string	Fuchsia					= "Fuchsia";				//#FF00FF
		public static string	Gainsboro				= "Gainsboro";				//#DCDCDC
		public static string	GhostWhite				= "GhostWhite";				//#F8F8FF
		public static string	Gold					= "Gold";					//#FFD700
		public static string	Goldenrod				= "Goldenrod";				//#DAA520
		public static string	Gray					= "Gray";					//#808080
		public static string	Green					= "Green";					//#008000
		public static string	GreenYellow				= "GreenYellow";			//#ADFF2F
		public static string	Honeydew				= "Honeydew";				//#F0FFF0
		public static string	HotPink					= "HotPink";				//#FF69B4
		public static string	IndianRed				= "IndianRed";				//#CD5C5C
		public static string	Indigo					= "Indigo";					//#4B0082
		public static string	Ivory					= "Ivory";					//#FFFFF0
		public static string	Khaki					= "Khaki";					//#F0E68C
		public static string	Lavender				= "Lavender";				//#E6E6FA
		public static string	LavenderBlush			= "LavenderBlush";			//#FFF0F5
		public static string	LawnGreen				= "LawnGreen";				//#7CFC00
		public static string	LemonChiffon			= "LemonChiffon";			//#FFFACD
		public static string	LightBlue				= "LightBlue";				//#ADD8E6
		public static string	LightCoral				= "LightCoral";				//#F08080
		public static string	LightCyan				= "LightCyan";				//#E0FFFF
		public static string	LightGoldenrodYellow	= "LightGoldenrodYellow";	//#FAFAD2
		public static string	LightGreen				= "LightGreen";				//#90EE90
		public static string	LightGrey				= "LightGrey";				//#D3D3D3
		public static string	LightPink				= "LightPink";				//#FFB6C1
		public static string	LightSalmon				= "LightSalmon";			//#FFA07A
		public static string	LightSeaGreen			= "LightSeaGreen";			//#20B2AA
		public static string	LightSkyBlue			= "LightSkyBlue";			//#87CEFA
		public static string	LightSlateGray			= "LightSlateGray";			//#778899
		public static string	LightSteelBlue			= "LightSteelBlue";			//#B0C4DE
		public static string	LightYellow				= "LightYellow";			//#FFFFE0
		public static string	Lime					= "Lime";					//#00FF00
		public static string	LimeGreen				= "LimeGreen";				//#32CD32
		public static string	Linen					= "Linen";					//#FAF0E6
		public static string	Magenta					= "Magenta";				//#FF00FF
		public static string	Maroon					= "Maroon";					//#800000
		public static string	MediumAquamarine		= "MediumAquamarine";		//#66CDAA
		public static string	MediumBlue				= "MediumBlue";				//#0000CD
		public static string	MediumOrchid			= "MediumOrchid";			//#BA55D3
		public static string	MediumPurple			= "MediumPurple";			//#9370DB
		public static string	MediumSeaGreen			= "MediumSeaGreen";			//#3CB371
		public static string	MediumSlateBlue			= "MediumSlateBlue";		//#7B68EE
		public static string	MediumSpringGreen		= "MediumSpringGreen";		//#00FA9A
		public static string	MediumTurquoise			= "MediumTurquoise";		//#48D1CC
		public static string	MediumVioletRed			= "MediumVioletRed";		//#C71585
		public static string	MidnightBlue			= "MidnightBlue";			//#191970
		public static string	MintCream				= "MintCream";				//#F5FFFA
		public static string	MistyRose				= "MistyRose";				//#FFE4E1
		public static string	Moccasin				= "Moccasin";				//#FFE4B5
		public static string	NavajoWhite				= "NavajoWhite";			//#FFDEAD
		public static string	Navy					= "Navy";					//#000080
		public static string	OldLace					= "OldLace";				//#FDF5E6
		public static string	Olive					= "Olive";					//#808000
		public static string	OliveDrab				= "OliveDrab";				//#6B8E23
		public static string	Orange					= "Orange";					//#FFA500
		public static string	OrangeRed				= "OrangeRed";				//#FF4500
		public static string	Orchid					= "Orchid";					//#DA70D6
		public static string	PaleGoldenrod			= "PaleGoldenrod";			//#EEE8AA
		public static string	PaleGreen				= "PaleGreen";				//#98FB98
		public static string	PaleTurquoise			= "PaleTurquoise";			//#AFEEEE
		public static string	PaleVioletRed			= "PaleVioletRed";			//#DB7093
		public static string	PapayaWhip				= "PapayaWhip";				//#FFEFD5
		public static string	PeachPuff				= "PeachPuff";				//#FFDAB9
		public static string	Peru					= "Peru";					//#CD853F
		public static string	Pink					= "Pink";					//#FFC0CB
		public static string	Plum					= "Plum";					//#DDA0DD
		public static string	PowderBlue				= "PowderBlue";				//#B0E0E6
		public static string	Purple					= "Purple";					//#800080
		public static string	Red						= "Red";					//#FF0000
		public static string	RosyBrown				= "RosyBrown";				//#BC8F8F
		public static string	RoyalBlue				= "RoyalBlue";				//#4169E1
		public static string	SaddleBrown				= "SaddleBrown";			//#8B4513
		public static string	Salmon					= "Salmon";					//#FA8072
		public static string	SandyBrown				= "SandyBrown";				//#F4A460
		public static string	SeaGreen				= "SeaGreen";				//#2E8B57
		public static string	Seashell				= "Seashell";				//#FFF5EE
		public static string	Sienna					= "Sienna";					//#A0522D
		public static string	Silver					= "Silver";					//#C0C0C0
		public static string	SkyBlue					= "SkyBlue";				//#87CEEB
		public static string	SlateBlue				= "SlateBlue";				//#6A5ACD
		public static string	SlateGray				= "SlateGray";				//#708090
		public static string	Snow					= "Snow";					//#FFFAFA
		public static string	SpringGreen				= "SpringGreen";			//#00FF7F
		public static string	SteelBlue				= "SteelBlue";				//#4682B4
		public static string	Tan						= "Tan";					//#D2B48C
		public static string	Teal					= "Teal";					//#008080
		public static string	Thistle					= "Thistle";				//#D8BFD8
		public static string	Tomato					= "Tomato";					//#FF6347
		public static string	Turquoise				= "Turquoise";				//#40E0D0
		public static string	Violet					= "Violet";					//#EE82EE
		public static string	Wheat					= "Wheat";					//#F5DEB3
		public static string	White					= "White";					//#FFFFFF
		public static string	WhiteSmoke				= "WhiteSmoke";				//#F5F5F5
		public static string	Yellow					= "Yellow";					//#FFFF00
		public static string	YellowGreen				= "YellowGreen";			//#9ACD32
		
		public static string	Gray0 					= "Gray0";					//#000000 
		public static string	Gray1 					= "Gray1";					//#030303 
		public static string	Gray2 					= "Gray2";					//#050505 
		public static string	Gray3 					= "Gray3";					//#080808 
		public static string	Gray4 					= "Gray4";					//#0a0a0a 
		public static string	Gray5 					= "Gray5";					//#0d0d0d 
		public static string	Gray6 					= "Gray6";					//#0f0f0f 
		public static string	Gray7 					= "Gray7";					//#121212 
		public static string	Gray8 					= "Gray8";					//#141414 
		public static string	Gray9 					= "Gray9";					//#171717 
		public static string	Gray10 					= "Gray10";					//#1A1A1A 
		public static string	Gray11 					= "Gray11";					//#1c1c1c 
		public static string	Gray12 					= "Gray12";					//#1f1f1f 
		public static string	Gray13 					= "Gray13";					//#212121 
		public static string	Gray14 					= "Gray14";					//#242424 
		public static string	Gray15 					= "Gray15";					//#262626 
		public static string	Gray16 					= "Gray16";					//#292929 
		public static string	Gray17 					= "Gray17";					//#2b2b2b 
		public static string	Gray18 					= "Gray18";					//#2e2e2e 
		public static string	Gray19 					= "Gray19";					//#303030 
		public static string	Gray20 					= "Gray20";					//#333333 
		public static string	Gray21 					= "Gray21";					//#363636 
		public static string	Gray22 					= "Gray22";					//#383838 
		public static string	Gray23 					= "Gray23";					//#3b3b3b 
		public static string	Gray24 					= "Gray24";					//#3d3d3d 
		public static string	Gray25 					= "Gray25";					//#404040 
		public static string	Gray26 					= "Gray26";					//#424242 
		public static string	Gray27 					= "Gray27";					//#454545 
		public static string	Gray28 					= "Gray28";					//#474747 
		public static string	Gray29 					= "Gray29";					//#4a4a4a 
		public static string	Gray30 					= "Gray30";					//#4d4d4d 
		public static string	Gray31 					= "Gray31";					//#4f4f4f 
		public static string	Gray32 					= "Gray32";					//#525252 
		public static string	Gray33 					= "Gray33";					//#545454 
		public static string	Gray34 					= "Gray34";					//#575757 
		public static string	Gray35 					= "Gray35";					//#595959 
		public static string	Gray36 					= "Gray36";					//#5c5c5c 
		public static string	Gray37 					= "Gray37";					//#5e5e5e 
		public static string	Gray38 					= "Gray38";					//#616161 
		public static string	Gray39 					= "Gray39";					//#636363 
		public static string	Gray40 					= "Gray40";					//#666666 
		public static string	Gray41 					= "Gray41";					//#696969 
		public static string	Gray42 					= "Gray42";					//#6b6b6b 
		public static string	Gray43 					= "Gray43";					//#6e6e6e 
		public static string	Gray44 					= "Gray44";					//#707070 
		public static string	Gray45 					= "Gray45";					//#737373 
		public static string	Gray46 					= "Gray46";					//#757575 
		public static string	Gray47 					= "Gray47";					//#787878 
		public static string	Gray48 					= "Gray48";					//#7a7a7a 
		public static string	Gray49 					= "Gray49";					//#7d7d7d 
		public static string	Gray50 					= "Gray50";					//#7f7f7f 
		public static string	Gray51 					= "Gray51";					//#828282 
		public static string	Gray52 					= "Gray52";					//#858585 
		public static string	Gray53 					= "Gray53";					//#878787 
		public static string	Gray54 					= "Gray54";					//#8a8a8a 
		public static string	Gray55 					= "Gray55";					//#8c8c8c 
		public static string	Gray56 					= "Gray56";					//#8f8f8f 
		public static string	Gray57 					= "Gray57";					//#919191 
		public static string	Gray58 					= "Gray58";					//#949494 
		public static string	Gray59 					= "Gray59";					//#969696 
		public static string	Gray60 					= "Gray60";					//#999999 
		public static string	Gray61 					= "Gray61";					//#9c9c9c 
		public static string	Gray62 					= "Gray62";					//#9e9e9e 
		public static string	Gray63 					= "Gray63";					//#A1A1A1 
		public static string	Gray64 					= "Gray64";					//#a3a3a3 
		public static string	Gray65 					= "Gray65";					//#a6a6a6 
		public static string	Gray66 					= "Gray66";					//#a8a8a8 
		public static string	Gray67 					= "Gray67";					//#ababab 
		public static string	Gray68 					= "Gray68";					//#adadad 
		public static string	Gray69 					= "Gray69";					//#b0b0b0 
		public static string	Gray70 					= "Gray70";					//#b3b3b3 
		public static string	Gray71 					= "Gray71";					//#b5b5b5 
		public static string	Gray72 					= "Gray72";					//#b8b8b8 
		public static string	Gray73 					= "Gray73";					//#bababa 
		public static string	Gray74 					= "Gray74";					//#bdbdbd 
		public static string	Gray75 					= "Gray75";					//#bfbfbf 
		public static string	Gray76 					= "Gray76";					//#c2c2c2 
		public static string	Gray77 					= "Gray77";					//#c4c4c4 
		public static string	Gray78 					= "Gray78";					//#c7c7c7 
		public static string	Gray79 					= "Gray79";					//#c9c9c9 
		public static string	Gray80 					= "Gray80";					//#cccccc 
		public static string	Gray81 					= "Gray81";					//#cfcfcf 
		public static string	Gray82 					= "Gray82";					//#d1d1d1 
		public static string	Gray83 					= "Gray83";					//#d4d4d4 
		public static string	Gray84 					= "Gray84";					//#d6d6d6 
		public static string	Gray85 					= "Gray85";					//#d9d9d9 
		public static string	Gray86 					= "Gray86";					//#dbdbdb 
		public static string	Gray87 					= "Gray87";					//#dedede 
		public static string	Gray88 					= "Gray88";					//#e0e0e0 
		public static string	Gray89 					= "Gray89";					//#e3e3e3 
		public static string	Gray90 					= "Gray90";					//#e5e5e5 
		public static string	Gray91 					= "Gray91";					//#e8e8e8 
		public static string	Gray92 					= "Gray92";					//#ebebeb 
		public static string	Gray93 					= "Gray93";					//#ededed 
		public static string	Gray94 					= "Gray94";					//#f0f0f0 
		public static string	Gray95 					= "Gray95";					//#f2f2f2 
		public static string	Gray96 					= "Gray96";					//#f5f5f5 
		public static string	Gray97 					= "Gray97";					//#f7f7f7 
		public static string	Gray98 					= "Gray98";					//#fafafa 
		public static string	Gray99 					= "Gray99";					//#fcfcfc 
		public static string	Gray100 				= "Gray100";				//#ffffff 
		
	}

}

