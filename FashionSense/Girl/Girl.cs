using BepInEx;

namespace FashionSense
{
	public class Girl
	{
		public SaveData.CharaData data;
		public ChaFileCoordinate[] coordinate;
		public bool justChanged;
		public bool Initialized { get; private set; }
		public ChaFile ChaFile { get; private set; }
		private ActionGame.Chara.Base CharaBase { get; set; }

		public Girl(SaveData.CharaData data)
		{
			this.data = data;
			justChanged = false;
			Initialized = false;

			var coordinate = data.charFile.coordinate;
			var len = coordinate.Length;
			
			this.coordinate = new ChaFileCoordinate[len];

			for (int i = 0; i < len; i++)
				this.coordinate[i] = coordinate[i];
		}

		public bool ChangeOutfit(bool reset = false)
		{
			if (data.charFile == null ||
				data.chaCtrl == null ||
				data.chaCtrl.chaFile == null ||
				data.charaBase == null)
				return false;

			if (ChaFile == null)
			{
				ChaFile = data.chaCtrl.chaFile;
				Initialized = true;
			}

			//foreach (var pair in KKABMX.Core.Utilities.GetDictDst(KKABMX.Core.Utilities.GetSibBody(data.chaCtrl)))
			//	Logger.Log(BepInEx.Logging.LogLevel.Message, $"{pair.Key}-{pair.Value}");

			if (CharaBase == null)
				CharaBase = data.charaBase;

			ChaFileCoordinate[] coordinate = null;

			if (!reset)
			{
				var dummy = Outfit.Closet.Roll(this);

				if (dummy == null)
					return false;

				coordinate = dummy.coordinate;
			}
			else
				coordinate = this.coordinate;

			ChaFile.coordinate = coordinate;

			// Reload Overworld Model
			CharaBase.ChangeNowCoordinate();

			return true;
		}
	}
}
