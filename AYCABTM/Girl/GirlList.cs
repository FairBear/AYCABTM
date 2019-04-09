﻿using System.Collections.Generic;
using BepInEx;
using Manager;
using Heroine = SaveData.Heroine;

namespace AYCABTM
{
	public static class GirlList
	{
		public static List<Girl> girls = new List<Girl>();

		public static void Compile(UnityEngine.SceneManagement.Scene scene)
		{
			var game = Game.Instance;

			if (game == null || scene.name != "MyRoom")
				return;

			foreach (Heroine girl in game.HeroineList)
				if (!girl.isTeacher)
					girls.Add(new Girl(girl));
		}

		public static void Decompile(UnityEngine.SceneManagement.Scene scene)
		{
			var count = girls.Count;

			if (scene.name != "MyRoom" || count == 0)
				return;

			// Set everyone's clothes back to normal.
			foreach (Girl girl in girls)
			{
				if (girl.data.charaBase != null)
					girl.data.charaBase.mapNo = 0; // Reset their locations.

				girl.ChangeOutfit(true);
			}

			girls.Clear();
		}

		private static void Update_Girl(Girl girl)
		{
			if (girl.data.charaBase == null)
				return;

			if (!girl.Initialized)
				girl.ChangeOutfit();
			
			if (Root._locations.Contains(girl.data.charaBase.mapNo))
			{
				if (!girl.justChanged)
				{
					girl.justChanged = true;

					girl.ChangeOutfit();
				}
			}
			else if (girl.justChanged)
			{
				girl.justChanged = false;
			}
		}

		public static void Update()
		{
			if (girls.Count == 0)
				return;

			girls.ForEach(Update_Girl);
		}
	}
}
