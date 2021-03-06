using BepInEx;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace FashionSense
{
	// Formely `All Your Clothes Are Belong To Me (AYCABTM)`.
	[BepInPlugin(GUID, PluginName, Version)]
	public class Root : BaseUnityPlugin
	{
		public const string GUID = "com.fairbear.bepinex.fashionsense";
		public const string PluginName = "Fashion Sense";
		public const string PluginNameInternal = "FashionSense";
		public const string Version = "2.1";

		public static HashSet<int> Locations { get; set; }

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

			Locations = new HashSet<int>();
			
			foreach (int i in Enum.GetValues(typeof(Place)))
				Locations.Add(i);

			Outfit.Closet.Compile();
		}

		void Main()
		{
			SceneManager.sceneLoaded += SceneManager_sceneLoaded;
			SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
		}
	}
}
