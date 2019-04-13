namespace FashionSense
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
		public static readonly string[] RESETCLOTHES =
		{
			"resetclothes_school",
			"resetclothes_goinghome",
			"resetclothes_pe",
			"resetclothes_swimsuit",
			"resetclothes_club",
			"resetclothes_casual",
			"resetclothes_sleep"
		};

		public static readonly string[] RESETACCESSORIES =
		{
			"resetaccessories_school",
			"resetaccessories_goinghome",
			"resetaccessories_pe",
			"resetaccessories_swimsuit",
			"resetaccessories_club",
			"resetaccessories_casual",
			"resetaccessories_sleep"
		};

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

		// Names that this mod identifies with.
		// This is used to find where the folder for the clothes are.
		public static readonly string[] NAMESPACES =
		{
			"fashionsense",
			"aycabtm" // This was the name of this mod before.
		};
	}

	static class DEFAULT
	{
		// -== THINGS TO KNOW ==-
		//
		// Coordinate
		// - The outfit a girl wears at a given scenario.
		// - Example; SchoolUniform, GoingHome, PE, Swimsuit, etc.
		//
		// Character Card
		// - This is the saved image when you save a girl in the character editor.
		// - This contains pretty much everything about the girl, except the stats they get when they are in-game.
		//
		// Coordinate Card
		// - Outfit cards. Contains clothes and accessories only.
		//
		// Outfit
		// - Clothes, accessories, body, hair, and makeup of a character card.
		// - If using a coordinate card, it will only contain the clothes and accessories.
		//
		// Combo-Type
		// - Allows mixing of different outfits to create an entirely new outfit.
		// - These are done by creating multiple folders within another folder.
		// - Each of those folders inside have character or coordinate cards.
		// - Example;
		// - `UserData\aycabtm\Combo Set 1\Bras\bra0.png`
		// - `UserData\aycabtm\Combo Set 1\Bras\bra1.png`
		// - `UserData\aycabtm\Combo Set 1\Panties\panties0.png`
		// - `UserData\aycabtm\Combo Set 1\Panties\panties1.png`
		// - This will choose a random bra and a random pair of panties and combine those to a single outfit.

		// How likely will this outfit be chosen.
		// The higher the value, the higher the chance.
		// Setting to 0 disables the outfit.
		public static readonly float WEIGHT = 10;

		// If the outfit is a 'combo-type'.
		// A 'combo-type' mix-and-matches outfits.
		// This is done in conjuction with the 'COMBOORDER' array.
		// 'COMBOORDER' helps identify which outfit will be applied first.
		public static readonly bool ISCOMBO = false;

		// If all the other coordinates will use the 'SchoolUniform' coordinate.
		// This means 'GoHome', 'PE', 'Swimsuit', and other coordinates will use the 'SchoolUniform' coordinate,
		// as if they we're copy-pasted.
		public static readonly bool SCHOOLUNIFORMALLCOORDINATES = false;

		// This will clear the girl's clothes first before applying the new outfit.
		// Each boolean corresponds to it's respective coordinate.
		// This is ALWAYS TRUE if the outfit is NOT a combo-type.
		public static readonly bool RESETCLOTHESALL = false;
		public static readonly bool[] RESETCLOTHES =
		{
			false,
			false,
			false,
			false,
			false,
			false,
			false
		};

		// This will clear the girl's accessories first before applying the new outfit.
		// Each boolean corresponds to it's respective coordinate.
		// This is ALWAYS TRUE if the outfit is NOT a combo-type.
		public static readonly bool RESETACCESSORIESALL = false;
		public static readonly bool[] RESETACCESSORIES =
		{
			false,
			false,
			false,
			false,
			false,
			false,
			false
		};

		// If the coordinate is used. All 'false' means this outfit is completely ignored.
		// If some is set to 'false' and the outfit is not a combo-type, it will use the original outfit.
		public static readonly bool COORDINATEALL = true;
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
		// Outfit must be a combo-type.
		public static readonly bool CLOTHESALL = true;
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
		// Outfit must be a combo-type.
		public static readonly bool SUBCLOTHESALL = true;
		public static readonly bool[] SUBCLOTHES =
		{
			true,
			true,
			true
		};

		// If accessories of a given coordinate are used.
		// Outfit must be a combo-type.
		public static readonly bool ACCESSORIESALL = true;
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
		// Outfit must be a combo-type.
		public static readonly bool REPLACEACCESSORIESALL = true;
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
		// Each boolean corresponds to it's respective coordinate.
		// Outfit must be a combo-type.
		public static readonly bool REPLACECLOTHESALL = true;
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
