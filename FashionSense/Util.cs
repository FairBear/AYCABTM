using BepInEx;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FashionSense
{
	static class Util
	{
		public static readonly Type[] NUMBERS = {
			typeof(byte),
			typeof(sbyte),
			typeof(ushort),
			typeof(uint),
			typeof(ulong),
			typeof(short),
			typeof(int),
			typeof(long),
			typeof(decimal),
			typeof(double),
			typeof(float)
		};

		public static class String
		{
			public static bool Parse(string value, out HashSet<string> result)
			{
				result = null;

				if (value == null || value.Length == 0)
					return false;

				if (value.StartsWith("[") && value.EndsWith("]"))
					value = value.Substring(1, value.Length - 2);

				result = new HashSet<string>();

				foreach (var item in value.Split(','))
				{
					Parse(item.Trim(), out string result1);
					result.Add(result1);
				}

				return true;
			}

			public static bool Parse(string value, out List<string> result)
			{
				result = null;

				if (value == null || value.Length == 0)
					return false;

				if (value.StartsWith("[") && value.EndsWith("]"))
					value = value.Substring(1, value.Length - 2);

				result = new List<string>();

				foreach (var item in value.Split(','))
				{
					Parse(item.Trim(), out string result1);
					result.Add(result1);
				}

				return true;
			}

			public static bool Parse(string value, out string result)
			{
				result = value;

				if (value == null)
					return false;

				if (value.StartsWith("\"") && value.EndsWith("\"") ||
					value.StartsWith("'") && value.EndsWith("'") ||
					value.StartsWith("`") && value.EndsWith("`"))
					result = value.Substring(1, value.Length - 2);

				return true;
			}

			public static bool Parse(string value, out bool result)
			{
				result = false;

				if (value == null)
					return false;

				value = value.Trim();

				if (bool.TryParse(value, out result))
					return true;
				else if (float.TryParse(value, out var result1))
				{
					result = result1 != 0;
					return true;
				}

				return false;
			}
		}
	}
}
