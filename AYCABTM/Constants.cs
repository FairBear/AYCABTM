using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AYCABTM
{
	enum Place
	{
		bathroom1f = 14,
		bathroom2f = 15,
		bathroom3f = 16,
		bathroomm = 18,
		lockerroom = 46,
		shawerroom = 45 // This is how Illusion spelled it.
	}

	enum Coordinate
	{
		school,
		goinghome,
		pe,
		swimsuit,
		club,
		casual,
		sleep
	}

	enum Clothes
	{
		top,
		bottom,
		bra,
		panties,
		gloves,
		pantyhose,
		legwear,
		inshoe,
		outshoe
	}

	enum SubClothes
	{
		subshirt,
		subjacketcollar,
		subdecoration
	}

	static class KEY
	{
		public static readonly string[] COORDINATE =
		{
			"coordinate_school",
			"coordinate_goinghome",
			"coordinate_pe",
			"coordinate_swimsuit",
			"coordinate_club",
			"coordinate_casual",
			"coordinate_sleep"
		};

		public static readonly string[] CLOTHES =
		{
			"clothes_top",
			"clothes_bottom",
			"clothes_bra",
			"clothes_panties",
			"clothes_gloves",
			"clothes_pantyhose",
			"clothes_legwear",
			"clothes_inshoe",
			"clothes_outshoe"
		};

		public static readonly string[] SUBCLOTHES =
		{
			"subclothes_subshirt",
			"subclothes_subjacketcollar",
			"subclothes_subdecoration"
		};

		public static readonly string[] ACCESSORIES =
		{
			"accessories_school",
			"accessories_goinghome",
			"accessories_pe",
			"accessories_swimsuit",
			"accessories_club",
			"accessories_casual",
			"accessories_sleep"
		};

		public static readonly string[] REPLACEACCESSORIES =
		{
			"replaceaccessories_school",
			"replaceaccessories_goinghome",
			"replaceaccessories_pe",
			"replaceaccessories_swimsuit",
			"replaceaccessories_club",
			"replaceaccessories_casual",
			"replaceaccessories_sleep"
		};

		public static readonly string[] REPLACECLOTHES =
		{
			"replaceclothes_school",
			"replaceclothes_goinghome",
			"replaceclothes_pe",
			"replaceclothes_swimsuit",
			"replaceclothes_club",
			"replaceclothes_casual",
			"replaceclothes_sleep"
		};
	}

	static class DEFAULT
	{
		// How likely will this outfit be chosen.
		public static readonly float WEIGHT = 10f;
		// If all the other coordinates will use the 'SchoolUniform' coordinate.
		// This means 'GoHome', 'PE', 'Swimsuit', and other coordinates will use the 'SchoolUniform' coordinate,
		// as if they we're copy-pasted.
		public static readonly bool SCHOOLUNIFORMALLCOORDINATES = false;

		// If the coordinate is used. All 'false' means this outfit is completely ignored.
		public static readonly bool[] COORDINATE =
		{
			true,
			true,
			true,
			true,
			true,
			true,
			true
		};

		// If the clothing part is used.
		public static readonly bool[] CLOTHES =
{
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true
		};

		// If the sub-clothing part is used.
		public static readonly bool[] SUBCLOTHES =
		{
			true,
			true,
			true
		};

		// If accessories of a given coordinate are used.
		public static readonly bool[] ACCESSORIES =
		{
			true,
			true,
			true,
			true,
			true,
			true,
			true
		};

		// If accessories of a given coordinate should overwrite everything.
		// If not, all accessories will be compiled to up to 20, favoring the first outfit's accessories when combining.
		public static readonly bool[] REPLACEACCESSORIES =
		{
			true,
			true,
			true,
			true,
			true,
			true,
			true
		};

		// If clothes with an id of 0 (nothing equipped) should be 'equipped'.
		// This means that if there was something equipped in that slot, it will be removed.
		public static readonly bool[] REPLACECLOTHES =
		{
			true,
			true,
			true,
			true,
			true,
			true,
			true
		};
	}
}
