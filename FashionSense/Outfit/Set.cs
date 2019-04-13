using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace FashionSense.Outfit
{
	public class Set
	{
		private HashSet<Tuple<string, float, HashSet<Outfit>>> cache; // Combinations of lists for each girl.
		private HashSet<Outfit> Values;
		public Set Next = null;
		public float Weight { get; private set; } // How likely will this be chosen.
		public float MaxWeight { get; private set; } = 0; // Total weight of all the values.
		public readonly HashSet<string>
			blackList, // List of girls that can't wear this.
			whiteList; // List of girls that can only wear this.

		public Set(float Weight = 0, HashSet<string> blackList = null, HashSet<string> whiteList = null)
		{
			cache = new HashSet<Tuple<string, float, HashSet<Outfit>>>();
			Values = new HashSet<Outfit>();
			this.Weight = Weight;
			this.blackList = blackList;
			this.whiteList = whiteList;
		}

		public static Set FromINI(INI config)
		{
			var Weight = config.GetFloat("general", "weight", DEFAULT.WEIGHT);

			if (Weight <= 0)
				return null;

			return new Set(
				Weight,
				config.GetStringHashSet("general", "blacklist"),
				config.GetStringHashSet("general", "whitelist")
			);
		}

		public void Add(Outfit outfit)
		{
			if (outfit == null)
				return;
			
			MaxWeight += outfit.Weight;
			Values.Add(outfit);
		}

		public void Clear()
		{
			MaxWeight = 0;
			Values.Clear();
			Flush();
		}

		public bool IsEmpty(string subject = null)
		{
			Filter(out var Values, out var MaxWeight, subject);

			return MaxWeight <= 0;
		}

		private void Filter(out HashSet<Outfit> Values, out float MaxWeight, string subject = null)
		{
			Values = this.Values;
			MaxWeight = this.MaxWeight;

			if (Values.Count == 0 || subject == null)
				return;

			var tuple = cache.FirstOrDefault(v => v.Item1 == subject);

			if (tuple.Item1 != null)
			{
				Values = tuple.Item3;
				MaxWeight = tuple.Item2;
				return;
			}

			WeightDist.Filter(
				this.Values,
				Closet.FilterFunc<Outfit>(
					subject,
					v => v.Weight,
					v => v.blackList,
					v => v.whiteList
				),
				subject,
				out Values,
				out MaxWeight,
				blackList,
				whiteList
			);

			cache.Add(new Tuple<string, float, HashSet<Outfit>>(subject, MaxWeight, Values));
		}

		public List<Outfit> Roll(string subject = null, List<Outfit> list = null)
		{
			if (list == null)
				list = new List<Outfit>();
			
			Filter(out var Values, out var MaxWeight, subject);
			
			if (MaxWeight > 0)
				list.Add(WeightDist.Roll(Values, v => v.Weight, MaxWeight));
			
			if (Next != null)
				return Next.Roll(subject, list);
			
			return list;
		}

		public void Flush()
		{
			cache.Clear();
		}
	}
}
