using System;
using System.Collections.Generic;
using System.Linq;
using Logger = BepInEx.Logger;

namespace AYCABTM.Outfit
{
	public class Part<T>
	{
		private Dictionary<string, KeyValuePair<float, List<Part<T>>>> cache; // Combinations of lists for each girl.
		private List<Part<T>> list; // List of parts.
		public float ListWeight { get; private set; } = 0; // Total weight of the list.
		public T Value { get; private set; }
		public readonly string name; // Name of this part. Doesn't need to be unique.
		public float Weight { get; private set; } // How likely will this be chosen.
		public readonly List<string> blackList; // List of girls that can't wear this.
		public readonly List<string> whiteList; // List of girls that can only wear this.

		public Part(string name = "", float Weight = 0, List<string> blackList = null, List<string> whiteList = null)
		{
			Flush(false);
			list = new List<Part<T>>();
			this.name = name.ToLower();
			this.Weight = Weight;
			this.blackList = blackList;
			this.whiteList = whiteList;
		}

		public Part(T Value, string name = "", float Weight = 0, List<string> blackList = null, List<string>  whiteList = null)
			: this(name, Weight, blackList, whiteList)
		{
			this.Value = Value;
		}

		public void Add(Part<T> part)
		{
			list.Add(part);
			ListWeight += part.Weight;
		}

		public void Add(Part<T>[] parts)
		{
			foreach (var part in parts)
				Add(part);
		}

		public void Flush(bool recursive = true)
		{
			cache = new Dictionary<string, KeyValuePair<float, List<Part<T>>>>();

			if (recursive)
				foreach (var part in list)
					part.Flush(true);
		}

		public bool Roll(out Part<T> result, string subject = null)
		{
			result = this;

			// There's nothing in here, give 'this' instead.
			if (this.list.Count == 0)
				return true;
			
			var list = this.list;
			var ListWeight = this.ListWeight;

			// Subject was given. Try to filter out the list.
			if (subject != null)
			{
				if (cache.ContainsKey(subject))
				{
					cache.TryGetValue(subject, out KeyValuePair<float, List<Part<T>>> pair);
					list = pair.Value;
					ListWeight = pair.Key;
				}
				else
				{
					list = new List<Part<T>>();
					ListWeight = 0;

					foreach (var part in this.list)
					{
						if (part.whiteList != null && !part.whiteList.Contains(subject) ||
							part.blackList != null && part.blackList.Contains(subject))
							continue;

						ListWeight += part.Weight;
						list.Add(part);
					}

					cache.Add(subject, new KeyValuePair<float, List<Part<T>>>(ListWeight, list));
				}
			}
			
			// There's only 1 item.
			if (list.Count == 1)
				return list.FirstOrDefault().Roll(out result, subject);
			
			// Roll weighted random distribution.
			var rand = new Random().NextDouble() * ListWeight;
			float n = 0;

			foreach (var part in list)
				if (rand <= (n += part.Weight))
				{
					var res = part.Roll(out result, subject);
					string test = result != null ? result.name : "";

					return res;
				}

			return false;
		}
	}
}
