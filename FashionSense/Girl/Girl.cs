namespace FashionSense
{
	public class Girl
	{
		public readonly SaveData.CharaData data;
		public readonly ChaFileCoordinate[] coordinate;
		public readonly ChaFileCustom custom;
		public readonly System.Version loadVersion;
		public bool justChanged;
		public bool Initialized { get; private set; }
		public ChaFile ChaFile { get; private set; }
		private ActionGame.Chara.Base CharaBase { get; set; }

		public Girl(SaveData.CharaData data)
		{
			this.data = data;
			justChanged = false;
			Initialized = false;

			var charFile = data.charFile;

			coordinate = Util.Array.Replace(new ChaFileCoordinate[charFile.coordinate.Length], i =>
			{
				var v = new ChaFileCoordinate();
				v.LoadBytes(charFile.coordinate[i].SaveBytes(), charFile.loadVersion);
				return v;
			});
			custom = new ChaFileCustom();
			custom.LoadBytes(charFile.custom.SaveBytes(), charFile.loadVersion);
			loadVersion = charFile.loadVersion;
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

			ChaFileCoordinate[] coordinate;
			ChaFileCustom custom;

			if (!reset)
			{
				var dummy = Outfit.Closet.Roll(this);

				if (dummy == null)
					return false;

				coordinate = Util.Array.Replace(new ChaFileCoordinate[dummy.coordinate.Length], i =>
				{
					var v = new ChaFileCoordinate();
					v.LoadBytes(dummy.coordinate[i].SaveBytes(), dummy.loadVersion);
					return v;
				});
				custom = new ChaFileCustom();
				custom.LoadBytes(dummy.custom.SaveBytes(), dummy.loadVersion);
			}
			else
			{
				coordinate = this.coordinate;
				custom = this.custom;
			}

			ChaFile.coordinate = coordinate;
			ChaFile.custom = custom;

			// Reload Overworld Model

			//data.chaCtrl.Reload();
			data.chaCtrl.ChangeHair(true);
			CharaBase.ChangeNowCoordinate();

			return true;
		}
	}
}
