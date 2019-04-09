using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Logger = BepInEx.Logger;

namespace AYCABTM.Outfit
{
	static class Closet
	{
		private static ChaFileControl dummy = SimpleSingleton<ChaFileControl>.Instance;
		private static Part<Outfit> closetList = new Part<Outfit>();

		private static ChaFileControl LoadFile(string path)
		{
			if (Path.GetExtension(path) != ".png")
				return null;

			var ChaFile = new ChaFileControl();

			ChaFile.LoadFileLimited(path);
			
			return ChaFile;
		}

		private static bool[] Get(INI config, string section, string[] key, bool[] defaultValue)
		{
			var list = new bool[defaultValue.Length];

			for (int i = 0; i < list.Length; i++)
				list[i] = config.GetBool(section, key[i], defaultValue[i]);

			return list;
		}

		private static Part<Outfit> Compile_Set(string path, INI config)
		{
			Part<Outfit> parts = new Part<Outfit>(
				Path.GetDirectoryName(path),
				config.GetFloat("general", "weight", DEFAULT.WEIGHT),
				config.GetStringList("general", "blacklist"),
				config.GetStringList("general", "whitelist")
			);

			// Default Rule Configuration

			var rule_weight = config.GetFloat("rule", "weight", DEFAULT.WEIGHT);


			// Loopy

			foreach (string file_path in Directory.GetFiles(path))
			{
				var ChaFile = LoadFile(file_path);

				if (ChaFile == null)
					continue;


				// Outfit Configuration

				var name = Path.GetFileNameWithoutExtension(file_path).ToLower();
				var section = "rule_" + name;
				var weight = config.GetFloat(section, "weight", rule_weight);


				// Test if able for distribution.

				if (weight <= 0)
					continue;


				// Build outfit.

				var outfit = new Outfit(
					ChaFile,
					Get(config, section, KEY.COORDINATE, DEFAULT.COORDINATE),
					//Get(config, section, KEY.CLOTHES, DEFAULT.CLOTHES),
					//Get(config, section, KEY.SUBCLOTHES, DEFAULT.SUBCLOTHES),
					//Get(config, section, KEY.ACCESSORIES, DEFAULT.ACCESSORIES),
					//Get(config, section, KEY.REPLACEACCESSORIES, DEFAULT.REPLACEACCESSORIES),
					//Get(config, section, KEY.REPLACECLOTHES, DEFAULT.REPLACECLOTHES),
					//DEFAULT.COORDINATE,
					DEFAULT.CLOTHES,
					DEFAULT.SUBCLOTHES,
					DEFAULT.ACCESSORIES,
					DEFAULT.REPLACEACCESSORIES,
					DEFAULT.REPLACECLOTHES,
					config.GetBool(section, "schooluniformallcoordinates", DEFAULT.SCHOOLUNIFORMALLCOORDINATES)
				);

				parts.Add(new Part<Outfit>(
					outfit,
					name,
					weight,
					config.GetStringList(section, "blacklist"),
					config.GetStringList(section, "whitelist")
				));
			}

			if (parts.ListWeight == 0)
				return null;

			return parts;
		}

		private static void Compile_Combo(string path, INI config)
		{

		}

		public static void Compile()
		{
			var root_path = Path.GetFullPath(UserData.Path + Root.PluginNameInternal);

			if (!Directory.Exists(root_path))
				return;
			
			foreach (string path in Directory.GetDirectories(root_path))
			{
				var config = new INI(path + "\\config.ini", false);

				if (config.GetBool("general", "iscombo"))
				{
					Compile_Combo(path, config);
				}
				else
				{
					var set = Compile_Set(path, config);

					if (set != null)
						closetList.Add(set);
				}
			}
		}

		public static ChaFileControl Roll(ChaFile ChaFile)
		{
			if (!closetList.Roll(out var result))
				return null;

			return Outfit.Create(new Outfit[] { result.Value });
		}
	}
}
