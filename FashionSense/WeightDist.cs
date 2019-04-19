using System;
using System.Collections.Generic;
using System.Linq;

namespace FashionSense
{
	// Random Weighted Distribution
	public static class WeightDist
	{
		private static readonly Random random = new Random();

		public static T Roll<T>(HashSet<T> list, Func<T, float> weight, float maxWeight = 1)
		{
			if (list.Count == 1)
				return list.FirstOrDefault();

			var rand = random.NextDouble() * maxWeight;
			float n = 0;

			foreach (T item in list)
				if (rand < (n += weight(item)))
					return item;

			return list.LastOrDefault();
		}

		public static void Filter<T, F>(HashSet<T> list, Func<T, float> weight, F subject, out HashSet<T> resultList, out float resultMaxWeight, HashSet<F> blackList = null, HashSet<F> whiteList = null)
		{
			resultList = new HashSet<T>();
			resultMaxWeight = 0;

			foreach (T item in list)
			{
				if (blackList != null && blackList.Contains(subject) ||
					whiteList != null && !whiteList.Contains(subject))
					continue;

				var n = weight(item);

				if (n <= 0)
					continue;

				resultMaxWeight += n;
				resultList.Add(item);
			}
		}
	}
}
