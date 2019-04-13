using System.Collections.Generic;

namespace FashionSense.Outfit
{
	public class Outfit
	{
		public readonly ChaFileControl ChaFile;
		public float Weight { get; private set; }
		public readonly bool[]
			CoordinatesFlag,
			ClothesFlag,
			SubClothesFlag,
			AccessoriesFlag,
			ReplaceAccessoriesFlag,
			ReplaceClothesFlag;
		public readonly bool SchoolUniformAllCoordinates;
		public readonly HashSet<string>
			blackList,
			whiteList;

		public Outfit(ChaFileControl ChaFile,
					  float Weight,
					  bool[] CoordinatesFlag,
					  bool[] ClothesFlag,
					  bool[] SubClothesFlag,
					  bool[] AccessoriesFlag,
					  bool[] ReplaceAccessoriesFlag,
					  bool[] ReplaceClothesFlag,
					  bool SchoolUniformAllCoordinates,
					  HashSet<string> blackList,
					  HashSet<string> whiteList)
		{
			this.ChaFile = ChaFile;
			this.Weight = Weight;
			this.CoordinatesFlag = CoordinatesFlag;
			this.ClothesFlag = ClothesFlag;
			this.SubClothesFlag = SubClothesFlag;
			this.AccessoriesFlag = AccessoriesFlag;
			this.ReplaceAccessoriesFlag = ReplaceAccessoriesFlag;
			this.ReplaceClothesFlag = ReplaceClothesFlag;
			this.SchoolUniformAllCoordinates = SchoolUniformAllCoordinates;
			this.blackList = blackList;
			this.whiteList = whiteList;
		}

		public void Apply(int kind, ChaFileClothes clothes)
		{
			var parts = clothes.parts;
			var subParts = clothes.subPartsId;

			// Clothes
			for (int i = 0; i < parts.Length; i++)
				if (ClothesFlag[i] && (ReplaceClothesFlag[kind] || parts[i].id != 0))
					parts[i] = ChaFile.coordinate[kind].clothes.parts[i];

			// Sub Clothes
			for (int i = 0; i < subParts.Length; i++)
				if (SubClothesFlag[i] && (ReplaceClothesFlag[kind] || subParts[i] != 0))
					subParts[i] = ChaFile.coordinate[kind].clothes.subPartsId[i];
		}

		public void Apply(int kind, ChaFileAccessory accessories)
		{
			var parts0 = accessories.parts;
			var parts1 = ChaFile.coordinate[kind].accessory.parts;

			if (!AccessoriesFlag[kind])
				return;
			
			int len = parts0.Length;

			if (ReplaceAccessoriesFlag[kind])
				for (int i = 0; i < len; i++)
					parts0[i] = parts1[i];
			else
			{
				int k = 0;

				for (int i = 0; i < len && k < len; i++)
				{
					if (parts0[i].id == 0)
					{
						while (k < len && parts1[k].id == 0)
							k++;

						if (k >= len)
							break;

						parts0[i] = parts1[k];
						k++;
					}
				}
			}
		}

		public void Apply(int kind, ChaFileCoordinate coordinate)
		{
			Apply(kind, coordinate.clothes);
			Apply(kind, coordinate.accessory);
		}

		public void Apply(int kind, ChaFileControl ChaFile)
		{
			if (CoordinatesFlag[kind])
				Apply(SchoolUniformAllCoordinates ? 0 : kind, ChaFile.coordinate[kind]);
		}

		public void Apply(ChaFileControl ChaFile)
		{
			for (int i = 0; i < KEY.COORDINATE.Length; i++)
				Apply(i, ChaFile);
		}
	}
}
