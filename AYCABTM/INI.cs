using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using BepInEx;

namespace AYCABTM
{
	class INI
	{
		private Dictionary<string, Dictionary<string, string>> list;
		private readonly bool caseSensitive;

		public INI(string path = null, bool caseSensitive = true)
		{
			this.caseSensitive = caseSensitive;
			list = new Dictionary<string, Dictionary<string, string>>();

			if (path != null)
				Load(path);
		}

		public Type TypeOf(string section, string key)
		{
			if (!caseSensitive)
			{
				section = section.ToLower();
				key = key.ToLower();
			}

			if (!list.ContainsKey(section))
				return null;
			
			var dict = list[section];

			if (!dict.ContainsKey(key))
				return null;

			return dict[key].GetType();
		}

		public bool Has(string section)
		{
			if (!caseSensitive)
				section = section.ToLower();

			return list.ContainsKey(section);
		}

		public bool Has(string section, string key)
		{
			if (!caseSensitive)
			{
				section = section.ToLower();
				key = key.ToLower();
			}

			if (!list.ContainsKey(section))
				return false;

			return list[section].ContainsKey(key);
		}

		public bool Add(string section)
		{
			if (!caseSensitive)
				section = section.ToLower();

			if (list.ContainsKey(section))
				return false;

			list[section] = new Dictionary<string, string>();

			return true;
		}

		public bool Add(string section, string key, string value = null, bool auto = true)
		{
			if (!caseSensitive)
			{
				section = section.ToLower();
				key = key.ToLower();
			}

			if (!list.ContainsKey(section))
				if (auto)
					Add(section);
				else
					return false;
			
			list[section][key] = value;

			return true;
		}

		public bool Remove(string section)
		{
			if (!caseSensitive)
				section = section.ToLower();

			return list.Remove(section);
		}

		public bool Remove(string section, string key)
		{
			if (!caseSensitive)
			{
				section = section.ToLower();
				key = key.ToLower();
			}

			if (!list.ContainsKey(section))
				return false;

			return list[section].Remove(key);
		}

		public string Get(string section, string key)
		{
			if (!caseSensitive)
			{
				section = section.ToLower();
				key = key.ToLower();
			}

			if (!list.ContainsKey(section))
				return null;

			var dict = list[section];

			if (!dict.ContainsKey(key))
				return null;

			list[section].TryGetValue(key, out string value);

			return value;
		}

		public string GetString(string section, string key, string defaultValue = "")
		{
			var value = Get(section, key);

			if (value == null)
				return defaultValue;

			Util.String.Parse(value, out string result);

			return result;
		}

		public List<string> GetStringList(string section, string key, List<string> defaultValue = null)
		{
			var value = Get(section, key);

			if (value == null)
				return defaultValue;

			if (value.StartsWith("[") && value.EndsWith("]"))
				value = value.Substring(1, value.Length - 2);

			var list = new List<string>();
			var items = value.Split(',');

			foreach (var item in value.Split(','))
			{
				item.Trim();

				Util.String.Parse(item, out string result);
				list.Add(result);
			}

			return list;
		}

		public List<bool> GetBoolList(string section, string key, List<bool> defaultValue = null)
		{
			var value = Get(section, key);

			if (value == null)
				return defaultValue;

			if (value.StartsWith("[") && value.EndsWith("]"))
				value = value.Substring(1, value.Length - 2);

			var list = new List<bool>();
			var items = value.Split(',');

			foreach (var item in value.Split(','))
			{
				item.Trim();

				Util.String.Parse(item, out bool result);
				list.Add(result);
			}

			return list;
		}

		public int GetInt(string section, string key, int defaultValue = 0)
		{
			return int.TryParse(Get(section, key), out int value) ? value : defaultValue;
		}

		public float GetFloat(string section, string key, float defaultValue = 0)
		{
			return float.TryParse(Get(section, key), out float value) ? value : defaultValue;
		}

		public bool GetBool(string section, string key, bool defaultValue = false)
		{
			var value = Get(section, key);

			if (Util.String.Parse(value, out bool result))
				return result;

			return defaultValue;
		}

		public string[] Sections()
		{
			return list.Keys.ToArray();
		}

		public string[] Keys(string section)
		{
			if (!caseSensitive)
				section = section.ToLower();

			if (!list.ContainsKey(section))
				return null;

			list.TryGetValue(section, out var result);

			return result.Keys.ToArray();
		}

		public void Save(string path)
		{
			var writer = new StreamWriter(File.Create(path));

			foreach (var pair0 in list)
			{
				writer.WriteLine($"[{pair0.Key}]");

				foreach (var pair1 in pair0.Value)
				{
					writer.WriteLine($"{pair1.Key}={pair1.Value}");
				}
			}
		}

		public void Load(string path, bool replace = true)
		{
			if (!File.Exists(path))
				return;

			if (replace)
				list.Clear();

			Dictionary<string, string> section = null;

			foreach (string line in File.ReadAllLines(path))
			{
				line.Trim();


				// Section

				if (line.StartsWith("[") && line.EndsWith("]"))
				{
					var name = line.Substring(1, line.Length - 2).Trim();

					if (!caseSensitive)
						name = name.ToLower();

					if (list.ContainsKey(name))
						list.TryGetValue(name, out section);
					else
						list.Add(name, section = new Dictionary<string, string>());

					continue;
				}


				// Irrelevant

				if (line.Length == 0 || // Empty
					line.StartsWith(";") || line.StartsWith("#") || // Comment
					section == null) // No section
					continue;


				// Key-Value Pair

				var i = line.IndexOf('=');

				if (i == -1)
					continue;

				var key = line.Substring(0, i);

				if (!caseSensitive)
					key = key.ToLower();

				section.Add(key.Trim(), line.Substring(i + 1).Trim());
			}
		}
	}
}
