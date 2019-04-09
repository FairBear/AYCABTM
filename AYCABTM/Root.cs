using BepInEx;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Logger = BepInEx.Logger;

namespace AYCABTM
{
	[BepInPlugin(GUID, PluginName, Version)]
	public class Root : BaseUnityPlugin
	{
		public const string GUID = "com.fairbear.bepinex.aycabtm";
		public const string PluginName = "AllYourClothesAreBelongToMe";
		public const string PluginNameInternal = "AYCABTM";
		public const string Version = "1.0";

		public static List<int> _locations { get; set; }

		void SceneManager_sceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
		{
			// Decompile the girls.
			GirlList.Decompile(scene);
		}

		void SceneManager_sceneUnloaded(Scene scene)
		{
			// Compile the girls.
			GirlList.Compile(scene);
		}

		void Update()
		{
			GirlList.Update();
		}

		void Start()
		{
			// locations

			_locations = new List<int>();
			
			foreach (int i in Enum.GetValues(typeof(Place)))
				_locations.Add(i);

			Outfit.Closet.Compile();
		}

		void Main()
		{
			SceneManager.sceneLoaded += SceneManager_sceneLoaded;
			SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
		}
	}
}
