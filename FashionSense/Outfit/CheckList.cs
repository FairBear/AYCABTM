using System.Collections.Generic;
using System.Text.RegularExpressions;
using UniRx;

namespace FashionSense.Outfit.CheckList
{
	class _
	{
		public static readonly char S = '.'; // Separator
	}

	public class Navi
	{
		public bool flag;
		public HashSet<Tuple<int, string[], bool>> paths;

		public Navi(bool flag = true, HashSet<Tuple<int, string[], bool>> paths = null)
		{
			this.flag = flag;
			this.paths = paths ?? new HashSet<Tuple<int, string[], bool>>();
		}

		public Navi A(string name, string key, int id)
		{
			var flag = -1;
			var paths = new HashSet<Tuple<int, string[], bool>>();

			foreach (var path in this.paths)
				if (Regex.Match(path.Item2[path.Item1], $@"^{name}").Success)
				{
					var match = Regex.Match(path.Item2[path.Item1], @"\[.+\]$");
					var next_path = new Tuple<int, string[], bool>(path.Item1 + 1, path.Item2, path.Item3);

					if (match.Success)
					{
						var compare = Util.String.Trim(match.Value);

						if (compare == key || compare == id.ToString())
							if (path.Item1 < path.Item2.Length - 1)
								paths.Add(next_path);
							else
								flag = path.Item3 ? 1 : 0;
					}
					else if (path.Item1 < path.Item2.Length - 1)
						paths.Add(next_path);
					else if (flag == -1)
						flag = path.Item3 ? 1 : 0;
				}
				else
					paths.Add(path);

			return new Navi(flag == -1 ? this.flag : flag == 1 ? true : false, paths);
		}

		public Navi A(string name, int id)
		{
			var flag = -1;
			var paths = new HashSet<Tuple<int, string[], bool>>();

			foreach (var path in this.paths)
				if (Regex.Match(path.Item2[path.Item1], $@"^{name}").Success)
				{
					var match = Regex.Match(path.Item2[path.Item1], @"\[.+\]$");
					var compare = Util.String.Trim(match.Value);
					var next_path = new Tuple<int, string[], bool>(path.Item1 + 1, path.Item2, path.Item3);

					if (match.Success)
					{
						if (compare == id.ToString())
							if (path.Item1 < path.Item2.Length - 1)
								paths.Add(next_path);
							else
								flag = path.Item3 ? 1 : 0;
					}
					else if (path.Item1 < path.Item2.Length - 1)
						paths.Add(next_path);
					else if (flag == -1)
						flag = path.Item3 ? 1 : 0;
				}
				else
					paths.Add(path);

			return new Navi(flag == -1 ? this.flag : flag == 1 ? true : false, paths);
		}

		public Navi A(string name)
		{
			var flag = this.flag;
			var paths = new HashSet<Tuple<int, string[], bool>>();

			foreach (var path in this.paths)
				if (name == path.Item2[path.Item1])
					if (path.Item1 < path.Item2.Length - 1)
						paths.Add(new Tuple<int, string[], bool>(path.Item1 + 1, path.Item2, path.Item3));
					else
						flag = path.Item3;
				else
					paths.Add(path);

			return new Navi(flag, paths);
		}

		public bool[] B(string name, string[] keys)
		{
			var flag = this.flag;
			var flags = Util.Array.Replace(new int[keys.Length], i => -1);

			foreach (var path in paths)
				if (Regex.Match(path.Item2[path.Item1], $@"^{name}").Success && path.Item1 >= path.Item2.Length - 1)
				{
					var match = Regex.Match(path.Item2[path.Item1], @"\[.+\]$");

					if (match.Success)
					{
						for (int i = 0; i < keys.Length; i++)
							if (keys[i] == match.Value ||
								match.Value == i.ToString())
							{
								flags[i] = path.Item3 ? 1 : 0;
								break;
							}
					}
					else
						flag = path.Item3;
				}

			return Util.Array.Replace(new bool[keys.Length], i => flags[i] == 1 ? true : flags[i] == 0 ? false : flag);
		}

		public bool[] B(string name, int keys)
		{
			var flag = this.flag;
			var flags = Util.Array.Replace(new int[keys], i => -1);

			foreach (var path in paths)
				if (Regex.Match(path.Item2[path.Item1], $@"^{name}").Success && path.Item1 >= path.Item2.Length - 1)
				{
					var match = Regex.Match(path.Item2[path.Item1], @"\[.+\]$");

					if (match.Success)
					{
						if (int.TryParse(match.Value, out var id) && id >= 0 && id < keys)
							flags[id] = path.Item3 ? 1 : 0;
					}
					else
						flag = path.Item3;
				}

			return Util.Array.Replace(new bool[keys], i => flags[i] == 1 ? true : flags[i] == 0 ? false : flag);
		}

		public bool B(string name)
		{
			foreach (var path in paths)
				if (path.Item2[path.Item1] == name && path.Item1 >= path.Item2.Length - 1)
					return path.Item3;

			return flag;
		}

		public static int B(Match match, string key, int id, bool value, int defaultFlag)
		{
			var flag = value ? 1 : 0;

			if (match.Success)
			{
				var compare = Util.String.Trim(match.Value);

				if (compare == key || compare == id.ToString())
					return flag;
			}
			else if (defaultFlag == -1)
				return flag;

			return defaultFlag;
		}

		public static int B(Match match, int id, bool value, int defaultFlag)
		{
			var flag = value ? 1 : 0;

			if (match.Success)
			{
				if (Util.String.Trim(match.Value) == id.ToString())
					return flag;
			}
			else if (defaultFlag == -1)
				return flag;

			return defaultFlag;
		}
	}

	public class Color
	{
		public readonly bool r, g, b, a;

		public Color(bool r, bool g, bool b, bool a)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}

		public static Color FromINI(Navi navi)
		{
			return new Color(
				navi.B("r"),
				navi.B("g"),
				navi.B("b"),
				navi.B("a")
			);
		}
	}

	public class Vector2
	{
		public readonly bool x, y;

		public Vector2(bool x, bool y)
		{
			this.x = x;
			this.y = y;
		}

		public static Vector2 FromINI(Navi navi)
		{
			return new Vector2(
				navi.B("x"),
				navi.B("y")
			);
		}
	}

	public class Vector3
	{
		public readonly bool x, y, z;

		public Vector3(bool x, bool y, bool z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public static Vector3 FromINI(Navi navi)
		{
			return new Vector3(
				navi.B("x"),
				navi.B("y"),
				navi.B("z")
			);
		}
	}

	public class Vector4
	{
		public readonly bool w, x, y, z;

		public Vector4(bool w, bool x, bool y, bool z)
		{
			this.w = w;
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public static Vector4 FromINI(Navi navi)
		{
			return new Vector4(
				navi.B("w"),
				navi.B("x"),
				navi.B("y"),
				navi.B("z")
			);
		}
	}

	public class ChaFileMakeup
	{
		public readonly Vector4[] paintLayout;
		public readonly Color[] paintColor;
		public readonly Color cheekColor, eyeshadowColor, lipColor;
		public readonly bool cheekId, eyeshadowId, lipId, paintId;

		public ChaFileMakeup(Vector4[] paintLayout, Color[] paintColor,
							 Color cheekColor, Color eyeshadowColor,
							 Color lipColor, bool cheekId,
							 bool eyeshadowId, bool lipId,
							 bool paintId)
		{
			this.paintLayout = paintLayout;
			this.paintColor = paintColor;
			this.cheekColor = cheekColor;
			this.eyeshadowColor = eyeshadowColor;
			this.lipColor = lipColor;
			this.cheekId = cheekId;
			this.eyeshadowId = eyeshadowId;
			this.lipId = lipId;
			this.paintId = paintId;
		}

		public static ChaFileMakeup FromINI(Navi navi, global::ChaFileMakeup dummy)
		{
			return new ChaFileMakeup(
				Util.Array.Replace(
					new Vector4[dummy.paintLayout.Length],
					i => Vector4.FromINI(navi.A("paintlayout", i))
				),
				Util.Array.Replace(
					new Color[dummy.paintColor.Length],
					i => Color.FromINI(navi.A("paintcolor", i))
				),
				Color.FromINI(navi.A("cheekcolor")),
				Color.FromINI(navi.A("eyeshadowcolor")),
				Color.FromINI(navi.A("lipcolor")),
				navi.B("cheekid"),
				navi.B("eyeshadowid"),
				navi.B("lipid"),
				navi.B("paintid")
			);
		}
	}

	public class ChaFileHair
	{
		public class PartsInfo
		{
			public readonly Color[] acsColor;
			public readonly Color baseColor, endColor, outlineColor, startColor;
			public readonly Vector3 pos, rot, scl;
			public readonly bool id, length;

			public PartsInfo(Color[] acsColor, Color baseColor,
							 Color endColor, Color outlineColor,
							 Color startColor, Vector3 pos,
							 Vector3 rot, Vector3 scl,
							 bool id, bool length)
			{
				this.acsColor = acsColor;
				this.baseColor = baseColor;
				this.endColor = endColor;
				this.outlineColor = outlineColor;
				this.startColor = startColor;
				this.pos = pos;
				this.rot = rot;
				this.scl = scl;
				this.id = id;
				this.length = length;
			}

			public static PartsInfo FromINI(Navi navi, global::ChaFileHair.PartsInfo dummy)
			{
				return new PartsInfo(
					Util.Array.Replace(
						new Color[dummy.acsColor.Length],
						i => Color.FromINI(navi.A("acscolor", i))
					),
					Color.FromINI(navi.A("basecolor")),
					Color.FromINI(navi.A("endcolor")),
					Color.FromINI(navi.A("outlinecolor")),
					Color.FromINI(navi.A("startcolor")),
					Vector3.FromINI(navi.A("pos")),
					Vector3.FromINI(navi.A("rot")),
					Vector3.FromINI(navi.A("scl")),
					navi.B("id"),
					navi.B("length")
				);
			}
		}

		public readonly PartsInfo[] parts;
		public readonly bool glossId, kind;

		public ChaFileHair(PartsInfo[] parts, bool glossId, bool kind)
		{
			this.glossId = glossId;
			this.kind = kind;
			this.parts = parts;
		}

		public static ChaFileHair FromINI(Navi navi, global::ChaFileHair dummy)
		{
			return new ChaFileHair(
				Util.Array.Replace(
					new PartsInfo[dummy.parts.Length],
					i => PartsInfo.FromINI(navi.A("parts", i), dummy.parts[i])
				),
				navi.B("glossid"),
				navi.B("kind")
			);
		}
	}

	public class ChaFileFace
	{
		public class PupilInfo
		{
			public readonly Color baseColor, subColor;
			public readonly bool gradBlend, gradMaskId, gradOffsetY, gradScale, id;

			public PupilInfo(Color baseColor, Color subColor,
							 bool gradBlend, bool gradMaskId,
							 bool gradOffsetY, bool gradScale,
							 bool id)
			{
				this.baseColor = baseColor;
				this.subColor = subColor;
				this.gradBlend = gradBlend;
				this.gradMaskId = gradMaskId;
				this.gradOffsetY = gradOffsetY;
				this.gradScale = gradScale;
				this.id = id;
			}

			public static PupilInfo FromINI(Navi navi)
			{
				return new PupilInfo(
					Color.FromINI(navi.A("basecolor")),
					Color.FromINI(navi.A("subcolor")),
					navi.B("gradblend"),
					navi.B("gradmaskid"),
					navi.B("gradoffsety"),
					navi.B("gradscale"),
					navi.B("id")
				);
			}
		}

		public readonly ChaFileMakeup baseMakeup;
		public readonly Color
			eyebrowColor, eyelineColor, hlDownColor, hlUpColor, lipLineColor, moleColor, whiteBaseColor,
			whiteSubColor;
		public readonly Vector4 moleLayout;
		public readonly PupilInfo[] pupil;
		public readonly bool[] shapeValueFace;
		public readonly bool
			cheekGlossPower, detailId, detailPower, doubleTooth, eyebrowId, eyelineDownId, eyelineUpId,
			eyelineUpWeight, foregroundEyebrow, foregroundEyes, headId, hlDownId, hlDownY, hlUpId, hlUpY,
			lipGlossPower, lipLineId, moleId, noseId, pupilHeight, pupilWidth, pupilX, pupilY, skinId,
			whiteId;

		public ChaFileFace(PupilInfo[] pupil, ChaFileMakeup baseMakeup,
						   Color eyebrowColor, Color eyelineColor,
						   Color hlDownColor, Color hlUpColor,
						   Color lipLineColor, Color moleColor,
						   Color whiteBaseColor, Color whiteSubColor,
						   Vector4 moleLayout, bool[] shapeValueFace,
						   bool cheekGlossPower, bool detailId,
						   bool detailPower,  bool doubleTooth,
						   bool eyebrowId, bool eyelineDownId,
						   bool eyelineUpId, bool eyelineUpWeight,
						   bool foregroundEyebrow, bool foregroundEyes,
						   bool headId, bool hlDownId,
						   bool hlDownY, bool hlUpId,
						   bool hlUpY, bool lipGlossPower,
						   bool lipLineId, bool moleId,
						   bool noseId, bool pupilHeight,
						   bool pupilWidth, bool pupilX,
						   bool pupilY, bool skinId,
						   bool whiteId)
		{
			this.baseMakeup = baseMakeup; this.eyebrowColor = eyebrowColor;
			this.eyelineColor = eyelineColor; this.hlDownColor = hlDownColor;
			this.hlUpColor = hlUpColor; this.lipLineColor = lipLineColor;
			this.moleColor = moleColor; this.whiteBaseColor = whiteBaseColor;
			this.whiteSubColor = whiteSubColor; this.moleLayout = moleLayout;
			this.shapeValueFace = shapeValueFace; this.pupil = pupil;
			this.cheekGlossPower = cheekGlossPower; this.detailId = detailId;
			this.detailPower = detailPower; this.doubleTooth = doubleTooth;
			this.eyebrowId = eyebrowId; this.eyelineDownId = eyelineDownId;
			this.eyelineUpId = eyelineUpId; this.eyelineUpWeight = eyelineUpWeight;
			this.foregroundEyebrow = foregroundEyebrow; this.foregroundEyes = foregroundEyes;
			this.headId = headId; this.hlDownId = hlDownId;
			this.hlDownY = hlDownY; this.hlUpId = hlUpId;
			this.hlUpY = hlUpY; this.lipGlossPower = lipGlossPower;
			this.lipLineId = lipLineId; this.moleId = moleId;
			this.noseId = noseId; this.pupilHeight = pupilHeight;
			this.pupilWidth = pupilWidth; this.pupilX = pupilX;
			this.pupilY = pupilY; this.skinId = skinId;
			this.whiteId = whiteId;
		}

		public static ChaFileFace FromINI(Navi navi, global::ChaFileFace dummy)
		{
			return new ChaFileFace(
				Util.Array.Replace(
					new PupilInfo[dummy.pupil.Length],
					i => PupilInfo.FromINI(navi.A("pupil", i))
				),
				ChaFileMakeup.FromINI(navi.A("basemakeup"), dummy.baseMakeup),
				Color.FromINI(navi.A("eyebrowcolor")),
				Color.FromINI(navi.A("eyelinecolor")),
				Color.FromINI(navi.A("hldowncolor")),
				Color.FromINI(navi.A("hlupcolor")),
				Color.FromINI(navi.A("liplinecolor")),
				Color.FromINI(navi.A("molecolor")),
				Color.FromINI(navi.A("whitebasecolor")),
				Color.FromINI(navi.A("whitesubcolor")),
				Vector4.FromINI(navi.A("molelayout")),
				navi.B("shapevalueface", dummy.shapeValueFace.Length),
				navi.B("cheekglosspower"),
				navi.B("detailid"),
				navi.B("detailpower"),
				navi.B("doubletooth"),
				navi.B("eyebrowid"),
				navi.B("eyelinedownid"),
				navi.B("eyelineupid"),
				navi.B("eyelineupweight"),
				navi.B("foregroundeyebrow"),
				navi.B("foregroundeyes"),
				navi.B("headid"),
				navi.B("hldownid"),
				navi.B("hldowny"),
				navi.B("hlupid"),
				navi.B("hlupy"),
				navi.B("lipglosspower"),
				navi.B("liplineid"),
				navi.B("moleid"),
				navi.B("noseid"),
				navi.B("pupilheight"),
				navi.B("pupilwidth"),
				navi.B("pupilx"),
				navi.B("pupily"),
				navi.B("skinid"),
				navi.B("whiteid")
			);
		}
	}

	public class ChaFileBody
	{
		public readonly Vector4[] paintLayout;
		public readonly Color[] paintColor;
		public readonly Color
			nailColor, nipColor, skinMainColor, skinSubColor, sunburnColor, underhairColor;
		public readonly bool[] shapeValueBody;
		public readonly bool
			aerolaSize, bustSoftness, bustWeight, detailId, detailPower, drawAddLine, nailGlossPower,
			nipGlossPower, nipId, paintId, paintLayoutId, skinGlossPower, skinId, sunburnId, underhairId;

		public ChaFileBody(Vector4[] paintLayout, Color[] paintColor,
						   Color nailColor, Color nipColor,
						   Color skinMainColor, Color skinSubColor,
						   Color sunburnColor, Color underhairColor,
						   bool[] shapeValueBody, bool aerolaSize,
						   bool bustSoftness, bool bustWeight,
						   bool detailId, bool detailPower,
						   bool drawAddLine, bool nailGlossPower,
						   bool nipGlossPower, bool nipId,
						   bool paintId, bool paintLayoutId,
						   bool skinGlossPower, bool skinId,
						   bool sunburnId, bool underhairId)
		{
			this.paintLayout = paintLayout; this.paintColor = paintColor;
			this.nailColor = nailColor; this.nipColor = nipColor;
			this.skinMainColor = skinMainColor; this.skinSubColor = skinSubColor;
			this.sunburnColor = sunburnColor; this.underhairColor = underhairColor;
			this.shapeValueBody = shapeValueBody; this.aerolaSize = aerolaSize;
			this.bustSoftness = bustSoftness; this.bustWeight = bustWeight;
			this.detailId = detailId; this.detailPower = detailPower;
			this.drawAddLine = drawAddLine; this.nailGlossPower = nailGlossPower;
			this.nipGlossPower = nipGlossPower; this.nipId = nipId;
			this.paintId = paintId; this.paintLayoutId = paintLayoutId;
			this.skinGlossPower = skinGlossPower; this.skinId = skinId;
			this.sunburnId = sunburnId; this.underhairId = underhairId;
		}

		public static ChaFileBody FromINI(Navi navi, global::ChaFileBody dummy)
		{
			return new ChaFileBody(
				Util.Array.Replace(
					new Vector4[dummy.paintLayout.Length],
					i => Vector4.FromINI(navi.A("paintlayout", i))
				),
				Util.Array.Replace(
					new Color[dummy.paintColor.Length],
					i => Color.FromINI(navi.A("paintcolor", i))
				),
				Color.FromINI(navi.A("nailcolor")),
				Color.FromINI(navi.A("nipcolor")),
				Color.FromINI(navi.A("skinmaincolor")),
				Color.FromINI(navi.A("skinsubcolor")),
				Color.FromINI(navi.A("sunburncolor")),
				Color.FromINI(navi.A("underhaircolor")),
				navi.B("shapevaluebody", dummy.shapeValueBody.Length),
				navi.B("aerolasize"),
				navi.B("bustsoftness"),
				navi.B("bustweight"),
				navi.B("detailid"),
				navi.B("detailpower"),
				navi.B("drawaddline"),
				navi.B("nailglosspower"),
				navi.B("nipglosspower"),
				navi.B("nipid"),
				navi.B("paintid"),
				navi.B("paintlayoutid"),
				navi.B("skinglosspower"),
				navi.B("skinid"),
				navi.B("sunburnid"),
				navi.B("underhairid")
			);
		}
	}

	public class ChaFileCustom
	{
		public readonly ChaFileBody body;
		public readonly ChaFileFace face;
		public readonly ChaFileHair hair;

		public ChaFileCustom(ChaFileBody body, ChaFileFace face, ChaFileHair hair)
		{
			this.body = body;
			this.face = face;
			this.hair = hair;
		}

		public static ChaFileCustom FromINI(Navi navi, global::ChaFileCustom dummy)
		{
			return new ChaFileCustom(
				ChaFileBody.FromINI(navi.A("body"), dummy.body),
				ChaFileFace.FromINI(navi.A("face"), dummy.face),
				ChaFileHair.FromINI(navi.A("hair"), dummy.hair)
			);
		}
	}

	public class ChaFileAccessory
	{
		public class PartsInfo
		{
			public readonly Vector3[] addMove;
			public readonly Color[] color;
			public readonly bool hideCategory, id, parentKey, partsOfHead, type;

			public PartsInfo(Vector3[] addMove, Color[] color,
							 bool hideCategory, bool id,
							 bool parentKey, bool partsOfHead,
							 bool type)
			{
				this.addMove = addMove; this.color = color;
				this.hideCategory = hideCategory; this.id = id;
				this.parentKey = parentKey; this.partsOfHead = partsOfHead;
				this.type = type;
			}

			public static PartsInfo FromINI(Navi navi, global::ChaFileAccessory.PartsInfo dummy)
			{
				return new PartsInfo(
					Util.Array.Replace(
						new Vector3[dummy.addMove.Length],
						i => Vector3.FromINI(navi.A("addmove", i))
					),
					Util.Array.Replace(
						new Color[dummy.color.Length],
						i => Color.FromINI(navi.A("color", i))
					),
					navi.B("hidecategory"),
					navi.B("id"),
					navi.B("parentkey"),
					navi.B("partsofhead"),
					navi.B("type")
				);
			}
		}

		public readonly PartsInfo[] parts;
		public readonly bool overwrite;

		public ChaFileAccessory(PartsInfo[] parts, bool overwrite)
		{
			this.parts = parts;
			this.overwrite = overwrite;
		}

		public static ChaFileAccessory FromINI(Navi navi, global::ChaFileAccessory dummy)
		{
			return new ChaFileAccessory(
				Util.Array.Replace(
					new PartsInfo[dummy.parts.Length],
					i => PartsInfo.FromINI(navi.A("parts", i), dummy.parts[i])
				),
				navi.B("overwrite")
			);
		}
	}

	public class ChaFileClothes
	{
		public class PartsInfo
		{
			public class ColorInfo
			{
				public readonly Color baseColor, patternColor;
				public readonly Vector2 tiling;
				public readonly bool pattern;

				public ColorInfo(Color baseColor, Color patternColor,
								 Vector2 tiling, bool pattern)
				{
					this.baseColor = baseColor;
					this.patternColor = patternColor;
					this.tiling = tiling;
					this.pattern = pattern;
				}

				public static ColorInfo FromINI(Navi navi)
				{
					return new ColorInfo(
						Color.FromINI(navi.A("basecolor")),
						Color.FromINI(navi.A("patterncolor")),
						Vector2.FromINI(navi.A("tiling")),
						navi.B("pattern")
					);
				}
			}

			public readonly ColorInfo[] colorInfo;
			public readonly bool emblemeId, id;

			public PartsInfo(ColorInfo[] colorInfo, bool emblemeId, bool id)
			{
				this.colorInfo = colorInfo;
				this.emblemeId = emblemeId;
				this.id = id;
			}

			public static PartsInfo FromINI(Navi navi, global::ChaFileClothes.PartsInfo dummy)
			{
				return new PartsInfo(
					Util.Array.Replace(
						new ColorInfo[dummy.colorInfo.Length],
						i => ColorInfo.FromINI(navi.A("colorinfo", i))
					),
					navi.B("emblemeid"),
					navi.B("id")
				);
			}
		}

		public static readonly string[] PARTS_KEY =
		{
			"top",
			"bottom",
			"bra",
			"panties",
			"gloves",
			"pantyhose",
			"legwear",
			"inshoe",
			"outshoe"
		};

		public static readonly string[] SUBPARTSID_KEY =
		{
			"subshirt",
			"subjacketcollar",
			"subdecoration"
		};

		public readonly PartsInfo[] parts;
		public readonly bool[] hideBraOpt, hideShortsOpt, subPartsId;
		public readonly bool overwrite;

		public ChaFileClothes(PartsInfo[] parts, bool[] hideBraOpt,
							  bool[] hideShortsOpt, bool[] subPartsId,
							  bool overwrite)
		{
			this.parts = parts;
			this.hideBraOpt = hideBraOpt;
			this.hideShortsOpt = hideShortsOpt;
			this.subPartsId = subPartsId;
			this.overwrite = overwrite;
		}

		public static ChaFileClothes FromINI(Navi navi, global::ChaFileClothes dummy)
		{
			return new ChaFileClothes(
				Util.Array.Replace(
					new PartsInfo[dummy.parts.Length],
					i => PartsInfo.FromINI(navi.A("parts", PARTS_KEY[i], i), dummy.parts[i])
				),
				navi.B("hidebraopt", dummy.hideBraOpt.Length),
				navi.B("hideshortsopt", dummy.hideBraOpt.Length),
				navi.B("subpartsid", SUBPARTSID_KEY),
				navi.B("overwrite")
			);
		}
	}

	public class ChaFileCoordinate
	{
		public readonly ChaFileAccessory accessory;
		public readonly ChaFileClothes clothes;

		public ChaFileCoordinate(ChaFileAccessory accessory, ChaFileClothes clothes)
		{
			this.accessory = accessory;
			this.clothes = clothes;
		}

		public static ChaFileCoordinate FromINI(Navi navi, global::ChaFileCoordinate dummy)
		{
			return new ChaFileCoordinate(
				ChaFileAccessory.FromINI(navi.A("accessory"), dummy.accessory),
				ChaFileClothes.FromINI(navi.A("clothes"), dummy.clothes)
			);
		}
	}

	public class ChaFile
	{
		public static readonly string[] COORDINATE_KEY =
		{
			"school",
			"goinghome",
			"pe",
			"swimsuit",
			"club",
			"casual",
			"sleep"
		};

		public readonly ChaFileCoordinate[] coordinate;
		public readonly ChaFileCustom custom;
		public readonly bool schoolUniformAllCoordinates;

		public ChaFile(ChaFileCoordinate[] coordinate, ChaFileCustom custom, bool schoolUniformAllCoordinates)
		{
			this.coordinate = coordinate;
			this.custom = custom;
			this.schoolUniformAllCoordinates = schoolUniformAllCoordinates;
		}

		public static ChaFile FromINI(INI config, string section, string outfit_section, bool defaultValue = true)
		{
			var dummy = SimpleSingleton<ChaFileControl>.Instance;
			var navi = new Navi(defaultValue);
			var dump = new HashSet<string>();

			Util.Array.Loop(config.Keys(outfit_section), key =>
			{
				dump.Add(key);
				navi.paths.Add(new Tuple<int, string[], bool>(0, key.Split(_.S), config.GetBool(outfit_section, key, true)));
			});

			Util.Array.Loop(config.Keys(section), key =>
			{
				if (!dump.Contains(key))
					navi.paths.Add(new Tuple<int, string[], bool>(0, key.Split(_.S), config.GetBool(section, key, true)));
			});

			return new ChaFile(
				Util.Array.Replace(
					new ChaFileCoordinate[dummy.coordinate.Length],
					i => ChaFileCoordinate.FromINI(navi.A("coordinate", COORDINATE_KEY[i], i), dummy.coordinate[i])
				),
				ChaFileCustom.FromINI(navi.A("custom"), dummy.custom),
				navi.B("schooluniformallcoordinates")
			);
		}
	}
}
