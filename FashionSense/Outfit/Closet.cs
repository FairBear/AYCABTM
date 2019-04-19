using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UniRx;

namespace FashionSense.Outfit
{
	static class Closet
	{
		private static readonly HashSet<Tuple<string, float, HashSet<Set>>> cache = new HashSet<Tuple<string, float, HashSet<Set>>>();
		// Contains all the sets.
		private static readonly HashSet<Set> SetList = new HashSet<Set>();
		// Contains only the combo sets.
		private static readonly HashSet<Set> ComboSetList = new HashSet<Set>();
		// The original outfits of all the girls in the classroom.
		private static Set OriginalList;
		// The INI configuration file for the original outfits.
		private static INI OriginalConfig;
		private static float SetMaxWeight = 0;

		private static ChaFileControl LoadFile(string path)
		{
			if (Path.GetExtension(path) != ".png")
				return null;

			var ChaFile = SimpleSingleton<ChaFileControl>.Instance;
			ChaFile.LoadFileLimited(path, parameter: false);

			return ChaFile;
		}

		private static HashSet<string> GetStringHashSet(INI config, string rule_section, string outfit_section, string key, HashSet<string> defaultValue = null)
		{
			return config.GetStringHashSet(
				outfit_section,
				key,
				config.GetStringHashSet(
					rule_section,
					key,
					defaultValue
				)
			);
		}

		private static float GetFloat(INI config, string rule_section, string outfit_section, string key, float defaultValue)
		{
			return config.GetFloat(
				outfit_section,
				key,
				config.GetFloat(
					rule_section,
					key,
					defaultValue
				)
			);
		}

		// For processing black lists and white lists.
		public static Func<T, float> FilterFunc<T>(string subject, Func<T, float> weightFunc, Func<T, HashSet<string>> blackListFunc, Func<T, HashSet<string>> whiteListFunc)
		{
			return v =>
			{
				var blackList = blackListFunc(v);
				var whiteList = whiteListFunc(v);

				if (blackList != null && blackList.Contains(subject) ||
					whiteList != null && !whiteList.Contains(subject))
					return 0;

				return weightFunc(v);
			};
		}

		private static Outfit GenerateOutfit(ChaFileControl ChaFile, INI config, string name, bool original = false)
		{
			name = $"rule_{name}";
			var weight = GetFloat(config, "rule", name, "weight", DEFAULT.WEIGHT);

			if (weight <= 0)
				return null;

			var blackList = original ?
				null :
				GetStringHashSet(config, "rule", name, "blacklist");
			var whiteList = original ?
				new HashSet<string> { name } : // Original outfits are strictly for the girl who owns it.
				GetStringHashSet(config, "rule", name, "whitelist");

			return new Outfit(
				original ? null : ChaFile,
				weight,
				original ? null : CheckList.ChaFile.FromINI(config, "rule", name),
				blackList,
				whiteList
			);
		}

		private static Set Compile_Set(string path, INI config)
		{
			var set = Set.FromINI(config);

			if (set == null)
				return null;


			// Loopy

			foreach (string file_path in Directory.GetFiles(path))
			{
				var ChaFile = LoadFile(file_path);

				if (ChaFile == null)
					continue;

				var outfit = GenerateOutfit(ChaFile, config, Path.GetFileNameWithoutExtension(file_path).ToLower());

				if (outfit == null)
					continue;

				set.Add(outfit);
			}

			if (set.MaxWeight == 0)
				return null;

			return set;
		}

		private static Set Compile_Combo_Parse(INI config, string path)
		{
			return Compile_Set(path, new INI(path + "\\config.ini", false).Combine(config, new string[] { "general" }));
		}

		private static void Compile_Combo(string root_path, INI root_config)
		{
			var root_set = Set.FromINI(root_config);

			if (root_set == null)
				return;

			var previous = root_set;

			List<string> order = null;
			var order_str = root_config.Get("general", "comboorder");

			if (order_str != null)
				Util.String.Parse(order_str.ToLower(), out order);

			if (order != null)
				foreach (var name in order)
				{
					var path = $"{root_path}\\{name}";

					if (!Directory.Exists(path))
						continue;

					var set = Compile_Combo_Parse(root_config, path);

					if (set != null)
					{
						previous.Next = set;
						previous = set;
					}
				}

			foreach (string path in Directory.GetDirectories(root_path))
			{
				// Check if already parsed.
				if (order != null && order.Contains(Path.GetFileName(path).ToLower()))
					continue;
				
				var set = Compile_Combo_Parse(root_config, path);

				if (set != null)
				{
					previous.Next = set;
					previous = set;
				}
			}

			SetMaxWeight += root_set.Weight;
			SetList.Add(root_set);
			ComboSetList.Add(root_set);
		}

		public static void Compile()
		{
			string root_path = null;

			foreach (var name in KEY.NAMESPACES)
			{
				root_path = Path.GetFullPath(UserData.Path + name);

				if (Directory.Exists(root_path))
					break;
			}

			if (!Directory.Exists(root_path))
				return;

			cache.Clear();


			// Setup original outfits.

			OriginalConfig = new INI(root_path + "\\original.ini", false);
			OriginalList = new Set(
				OriginalConfig.GetFloat("general", "weight", DEFAULT.WEIGHT),
				OriginalConfig.GetStringHashSet("general", "blacklist"),
				OriginalConfig.GetStringHashSet("general", "whitelist")
			);
			
			if (OriginalList != null && OriginalList.Weight > 0)
			{
				SetMaxWeight += OriginalList.Weight;
				SetList.Add(OriginalList);
			}


			// Get all the folders.

			foreach (string path in Directory.GetDirectories(root_path))
			{
				var config = new INI(path + "\\config.ini", false);

				if (!config.GetBool("general", "iscombo", DEFAULT.ISCOMBO))
				{
					var set = Compile_Set(path, config);

					if (set == null)
						continue;

					SetMaxWeight += set.Weight;
					SetList.Add(set);
				}
				else
					Compile_Combo(path, config);
			}
		}

		public static void Apply(ChaFileControl ChaFile, int kind, List<Outfit> outfits)
		{
			if (outfits != null)
				foreach (var outfit in outfits)
					outfit.Apply(ChaFile, kind);
		}

		public static Set Roll(HashSet<Set> list, float maxWeight, string subject = null)
		{
			if (subject == null)
				return WeightDist.Roll(list, v => v.Weight, maxWeight);

			var tuple = cache.FirstOrDefault(v => v.Item1 == subject);

			if (tuple.Item1 != null)
				return WeightDist.Roll(tuple.Item3, v => v.Weight, tuple.Item2);

			WeightDist.Filter(
				list,
				FilterFunc<Set>(
					subject,
					v => v.IsEmpty(subject) && v != OriginalList ? 0 : v.Weight,
					v => v.blackList,
					v => v.whiteList
				),
				subject,
				out var resultList,
				out var resultMaxWeight
			);

			cache.Add(new Tuple<string, float, HashSet<Set>>(subject, resultMaxWeight, resultList));

			return WeightDist.Roll(resultList, v => v.Weight, resultMaxWeight);
		}

		public static ChaFileControl Roll(ChaFileControl ChaFile, string subject = null)
		{
			if (SetMaxWeight <= 0)
				return null;

			// Roll for a random set.
			var set = Roll(SetList, SetMaxWeight, subject);

			// There's nothing available in the list.
			if (set == null)
				return null;

			if (set == OriginalList)
				return ChaFile;

			var outfits = set.Roll(subject);
			Apply(ChaFile, 0, outfits);

			if (set.Next == null)
				// Apply all the coordinates of the set if not a combo-type.
				for (int i = 1; i < ChaFile.coordinate.Length; i++)
					Apply(ChaFile, i, outfits);
			else
				// Only roll outfits within the current folder.
				for (int i = 1; i < ChaFile.coordinate.Length; i++)
					Apply(ChaFile, i, set.Roll(subject));

			return ChaFile;
		}

		public static ChaFileControl Roll(Girl girl)
		{
			if (girl.ChaFile == null)
				return null;
			
			var ChaFile = SimpleSingleton<ChaFileControl>.Instance;
			ChaFile.CopyCoordinate(girl.coordinate);
			ChaFile.CopyCustom(girl.custom);

			//return Roll(ChaFile);
			return Roll(ChaFile, girl.data.Name);
		}
	}
}
