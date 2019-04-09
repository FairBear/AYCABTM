using System;
using System.Collections.Generic;
using System.Linq;

namespace AYCABTM
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

			public static bool Parse(string value, out int result)
			{
				result = 0;

				if (value == null)
					return false;

				int.TryParse(value.Trim(), out result);

				return true;
			}

			public static bool Parse(string value, out bool result)
			{
				result = false;

				if (value == null)
					return false;

				value.Trim();

				if (bool.TryParse(value, out result))
					return true;
				else if (float.TryParse(value, out var result1))
				{
					result = result1 != 0;
					return true;
				}

				return false;
			}

			public static string Parse(object value)
			{
				var type = value.GetType();

				if (type.IsArray) // Array
				{
					if (!(value is IEnumerable<object> list))
						return null;

					string result = "[";

					foreach (var item in list)
						result += Parse(item);

					return result + "]";
				}
				
				if (type == typeof(string) || NUMBERS.Contains(type)) // Number & String
					return value.ToString();

				return null;
			}
		}
	}
}
