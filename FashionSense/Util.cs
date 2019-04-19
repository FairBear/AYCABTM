using System;
using System.Collections.Generic;

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

		public static class Array
		{
			public static void Loop(int length, Action<int> act)
			{
				for (int i = 0; i < length; i++)
					act(i);
			}

			public static T[] Loop<T>(T[] arr, Action<int> act)
			{
				for (int i = 0; i < arr.Length; i++)
					act(i);

				return arr;
			}

			public static T[] Loop<T>(T[] arr, Action<T> act)
			{
				for (int i = 0; i < arr.Length; i++)
					act(arr[i]);

				return arr;
			}

			public static T[] Loop<T>(T[] arr, Action<int, T> act)
			{
				for (int i = 0; i < arr.Length; i++)
					act(i, arr[i]);

				return arr;
			}

			public static T[] Replace<T>(T[] arr, Func<int, T> func)
			{
				for (int i = 0; i < arr.Length; i++)
					arr[i] = func(i);

				return arr;
			}
		}

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

				if (float.TryParse(value, out var result1))
				{
					result = result1 != 0;
					return true;
				}

				return false;
			}

			public static string Trim(string value, int a = 1, int b = -1)
			{
				if (value.Length == 0)
					return value;

				b = b < 0 ? a : b;

				if (value.Length < a + b)
					return "";

				return value.Substring(a, value.Length - b - a);
			}
		}
	}
}
