using System.Collections.Generic;

namespace AYCABTM.Outfit
{
	class Outfit
	{
		private static Dictionary<string, ChaFileControl> cache = new Dictionary<string, ChaFileControl>();

		public readonly ChaFileControl ChaFile;
		public readonly bool[] CoordinatesFlag,
			ClothesFlag,
			SubClothesFlag,
			AccessoriesFlag,
			ReplaceAccessoriesFlag,
			ReplaceClothesFlag;
		public readonly bool SchoolUniformAllCoordinates;

		public Outfit(ChaFileControl ChaFile,
					  bool[] CoordinatesFlag,
					  bool[] ClothesFlag,
					  bool[] SubClothesFlag,
					  bool[] AccessoriesFlag,
					  bool[] ReplaceAccessoriesFlag,
					  bool[] ReplaceClothesFlag,
					  bool SchoolUniformAllCoordinates)
		{
			this.ChaFile = ChaFile;
			this.CoordinatesFlag = CoordinatesFlag;
			this.ClothesFlag = ClothesFlag;
			this.SubClothesFlag = SubClothesFlag;
			this.AccessoriesFlag = AccessoriesFlag;
			this.ReplaceAccessoriesFlag = ReplaceAccessoriesFlag;
			this.ReplaceClothesFlag = ReplaceClothesFlag;
			this.SchoolUniformAllCoordinates = SchoolUniformAllCoordinates;
		}

		public static ChaFileAccessory.PartsInfo Copy(ChaFileAccessory.PartsInfo part0)
		{
			var part1 = new ChaFileAccessory.PartsInfo
			{
				addMove = part0.addMove,
				color = part0.color,
				hideCategory = part0.hideCategory,
				id = part0.id,
				parentKey = part0.parentKey,
				partsOfHead = part0.partsOfHead,
				type = part0.type
			};

			return part1;
		}

		public static ChaFileClothes.PartsInfo Copy(ChaFileClothes.PartsInfo part0)
		{
			var part1 = new ChaFileClothes.PartsInfo
			{
				colorInfo = part0.colorInfo,
				emblemeId = part0.emblemeId,
				id = part0.id
			};

			return part1;
		}

		public static ChaFileAccessory Copy(ChaFileAccessory accessory0)
		{
			var accessory1 = new ChaFileAccessory();

			for (int i = 0; i < accessory0.parts.Length; i++)
				accessory1.parts[i] = Copy(accessory0.parts[i]);

			return accessory1;
		}

		public static ChaFileClothes Copy(ChaFileClothes clothes0)
		{
			var clothes1 = new ChaFileClothes();

			for (int i = 0; i < clothes0.parts.Length; i++)
				clothes1.parts[i] = Copy(clothes0.parts[i]);

			return clothes1;
		}

		public static ChaFileCoordinate Copy(ChaFileCoordinate coordinate0)
		{
			var coordinate1 = new ChaFileCoordinate
			{
				accessory = Copy(coordinate0.accessory),
				clothes = Copy(coordinate0.clothes)
			};

			return coordinate1;
		}

		public static ChaFileCoordinate[] Copy(ChaFileCoordinate[] coordinates0)
		{
			var coordinates1 = new ChaFileCoordinate[coordinates0.Length];

			for (int i = 0; i < coordinates0.Length; i++)
				coordinates1[i] = Copy(coordinates0[i]);

			return coordinates1;
		}

		public void Apply(int kind, ChaFileAccessory accessory0)
		{
			if (!AccessoriesFlag[kind])
				return;

			var accessory1 = ChaFile.coordinate[kind].accessory;
			int len = accessory0.parts.Length;
			var parts = new ChaFileAccessory.PartsInfo[len];

			if (ReplaceAccessoriesFlag[kind])
			{
				for (int i = 0; i < len; i++)
					parts[i] = accessory1.parts[i];

				accessory0.parts = parts;
			}
			else
			{
				int n = 0;

				for (int i = 0; i < len; i++)
				{
					var part = accessory0.parts[i];

					if (part.id != 0)
					{
						parts[n] = part;
						n++;
					}
				}

				for (int i = 0; i < accessory0.parts.Length && n < len; i++)
				{
					var part = accessory1.parts[i];

					if (part.id != 0)
					{
						parts[n] = part;
						n++;
					}
				}

				accessory0.parts = parts;
			}
		}

		public void Apply(int kind, ChaFileClothes clothes0)
		{
			var clothes1 = ChaFile.coordinate[kind].clothes;
			int partsLen = ClothesFlag.Length;
			int subPartsLen = SubClothesFlag.Length;
			var parts = new ChaFileClothes.PartsInfo[partsLen];
			var subParts = new int[subPartsLen];

			// Clothes
			for (int i = 0; i < partsLen; i++)
			{
				var part0 = clothes0.parts[i];
				var part1 = clothes1.parts[i];

				if (ClothesFlag[i])
					if (ReplaceClothesFlag[kind] || part1.id != 0)
						parts[i] = part1;
					else
						parts[i] = part0;
			}

			clothes0.parts = parts;


			// Sub Clothes
			for (int i = 0; i < subPartsLen; i++)
			{
				var subPart0 = clothes0.subPartsId[i];
				var subPart1 = clothes1.subPartsId[i];

				if (SubClothesFlag[i])
					if (ReplaceClothesFlag[kind] || subPart1 != 0)
						subParts[i] = subPart1;
					else
						subParts[i] = subPart0;
				else
					subParts[i] = subPart0;
			}

			clothes0.subPartsId = subParts;
		}

		public void Apply(int kind, ChaFileCoordinate coordinate)
		{
			Apply(kind, coordinate.accessory);
			Apply(kind, coordinate.clothes);
		}

		public void Apply(ChaFileControl ChaFile)
		{
			var coordinates = ChaFile.coordinate;

			for (int i = 0; i < CoordinatesFlag.Length; i++)
			{
				if (!CoordinatesFlag[i])
					continue;

				if (SchoolUniformAllCoordinates)
					Apply(0, coordinates[i]);
				else
					Apply(i, coordinates[i]);
			}
		}
		
		public static ChaFileControl Create(Outfit[] outfits)
		{
			var ChaFile = new ChaFileControl();
			string hash = "";

			// See if this was already done before.
			foreach(Outfit outfit in outfits)
				hash += outfit.GetHashCode() + ":";

			if (cache.TryGetValue(hash, out var value))
				return value;
			

			// Combine the outfits.
			foreach (Outfit outfit in outfits)
				outfit.Apply(ChaFile);

			cache.Add(hash, ChaFile);

			return ChaFile;
		}

		public static void Flush()
		{
			cache.Clear();
		}
	}
}
