using BepInEx;

namespace AYCABTM
{
	public class Girl
	{
		public SaveData.CharaData data;
		public ChaFileCoordinate[] coordinate;
		public bool justChanged;
		public bool Initialized { get; private set; }

		private ChaFile _chaFile { get; set; }
		private ActionGame.Chara.Base _charaBase { get; set; }

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

			if (_chaFile == null)
			{
				_chaFile = data.chaCtrl.chaFile;
				Initialized = true;
			}

			if (_charaBase == null)
				_charaBase = data.charaBase;

			ChaFileCoordinate[] coordinate = null;

			if (!reset)
			{
				var ChaFile = Outfit.Closet.Roll(_chaFile);

				if (ChaFile == null)
					return false;
				
				coordinate = ChaFile.coordinate;
			}
			else
				coordinate = this.coordinate;

			for (int i = 0; i < coordinate.Length; i++)
				_chaFile.coordinate[i] = coordinate[i];

			// Reload Overworld Model
			_charaBase.ChangeNowCoordinate();

			return true;
		}
	}
}
