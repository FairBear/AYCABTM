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

	static class KEY
	{
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
		public static readonly float WEIGHT = 10;
		
		public static readonly bool ISCOMBO = false;
	}
}
