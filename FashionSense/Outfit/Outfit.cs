using System.Collections.Generic;

namespace FashionSense.Outfit
{
	public class Outfit
	{
		public readonly ChaFileCoordinate[] coordinate;
		public readonly ChaFileCustom custom;
		public float Weight { get; private set; }
		public readonly CheckList.ChaFile checkList;
		public readonly HashSet<string> blackList, whiteList;

		public Outfit(ChaFileControl ChaFile,
					  float Weight,
					  CheckList.ChaFile checkList,
					  HashSet<string> blackList,
					  HashSet<string> whiteList)
		{
			if (ChaFile != null)
			{
				custom = new ChaFileCustom();
				custom.LoadBytes(ChaFile.custom.SaveBytes(), ChaFile.loadVersion);

				coordinate = Util.Array.Replace(new ChaFileCoordinate[ChaFile.coordinate.Length], i =>
				{
					var v = new ChaFileCoordinate();
					v.LoadBytes(ChaFile.coordinate[i].SaveBytes(), ChaFile.loadVersion);

					return v;
				});
			}
			else
			{
				custom = null;
				coordinate = new ChaFileCoordinate[ChaFile.coordinate.Length];
			}

			this.Weight = Weight;
			this.checkList = checkList;
			this.blackList = blackList;
			this.whiteList = whiteList;
		}

		public UnityEngine.Color Apply(UnityEngine.Color a, UnityEngine.Color b, CheckList.Color flag)
		{
			return new UnityEngine.Color(
				flag.r ? b.r : a.r,
				flag.g ? b.g : a.g,
				flag.b ? b.b : a.b,
				flag.a ? b.a : a.a
			);
		}
		public UnityEngine.Vector2 Apply(UnityEngine.Vector2 a, UnityEngine.Vector2 b, CheckList.Vector2 flag)
		{
			return new UnityEngine.Vector2(
				flag.x ? b.x : a.x,
				flag.y ? b.y : a.y
			);
		}

		public UnityEngine.Vector3 Apply(UnityEngine.Vector3 a, UnityEngine.Vector3 b, CheckList.Vector3 flag)
		{
			return new UnityEngine.Vector3(
				flag.x ? b.x : a.x,
				flag.y ? b.y : a.y,
				flag.z ? b.z : a.z
			);
		}

		public UnityEngine.Vector4 Apply(UnityEngine.Vector4 a, UnityEngine.Vector4 b, CheckList.Vector4 flag)
		{
			return new UnityEngine.Vector4(
				flag.x ? b.x : a.x,
				flag.y ? b.y : a.y,
				flag.z ? b.z : a.z,
				flag.w ? b.w : a.w
			);
		}

		public void Apply(ChaFileMakeup a, ChaFileMakeup b, CheckList.ChaFileMakeup flag)
		{
			a.cheekColor = Apply(a.cheekColor, b.cheekColor, flag.cheekColor);

			if (flag.cheekId)
				a.cheekId = b.cheekId;

			a.eyeshadowColor = Apply(a.eyeshadowColor, b.eyeshadowColor, flag.eyeshadowColor);

			if (flag.eyeshadowId)
				a.eyeshadowId = b.eyeshadowId;

			a.lipColor = Apply(a.lipColor, b.lipColor, flag.lipColor);

			if (flag.lipId)
				a.lipId = b.lipId;

			Util.Array.Loop(a.paintColor, i => a.paintColor[i] = Apply(a.paintColor[i], b.paintColor[i], flag.paintColor[i]));

			if (flag.paintId)
				a.paintId = b.paintId;

			Util.Array.Loop(a.paintLayout, i => a.paintLayout[i] = Apply(a.paintLayout[i], b.paintLayout[i], flag.paintLayout[i]));
		}

		public void Apply(ChaFileHair.PartsInfo a, ChaFileHair.PartsInfo b, CheckList.ChaFileHair.PartsInfo flag)
		{
			Util.Array.Loop(a.acsColor, i => a.acsColor[i] = Apply(a.acsColor[i], b.acsColor[i], flag.acsColor[i]));

			a.baseColor = Apply(a.baseColor, b.baseColor, flag.baseColor);

			a.endColor = Apply(a.endColor, b.endColor, flag.endColor);

			if (flag.id)
				a.id = b.id;

			if (flag.length)
				a.length = b.length;

			a.outlineColor = Apply(a.outlineColor, b.outlineColor, flag.outlineColor);

			a.pos = Apply(a.pos, b.pos, flag.pos);

			a.rot = Apply(a.rot, b.rot, flag.rot);

			a.scl = Apply(a.scl, b.scl, flag.scl);

			a.startColor = Apply(a.startColor, b.startColor, flag.startColor);
		}

		public void Apply(ChaFileHair a, ChaFileHair b, CheckList.ChaFileHair flag)
		{
			if (flag.glossId)
				a.glossId = b.glossId;

			if (flag.kind)
				a.kind = b.kind;

			Util.Array.Loop(a.parts, i => Apply(a.parts[i], b.parts[i], flag.parts[i]));
		}

		public void Apply(ChaFileFace.PupilInfo a, ChaFileFace.PupilInfo b, CheckList.ChaFileFace.PupilInfo flag)
		{
			a.baseColor = Apply(a.baseColor, b.baseColor, flag.baseColor);

			if (flag.gradBlend)
				a.gradBlend = b.gradBlend;

			if (flag.gradMaskId)
				a.gradMaskId = b.gradMaskId;

			if (flag.gradOffsetY)
				a.gradOffsetY = b.gradOffsetY;

			if (flag.gradScale)
				a.gradScale = b.gradScale;

			if (flag.id)
				a.id = b.id;

			a.subColor = Apply(a.subColor, b.subColor, flag.subColor);
		}

		public void Apply(ChaFileFace a, ChaFileFace b, CheckList.ChaFileFace flag)
		{
			Apply(a.baseMakeup, b.baseMakeup, flag.baseMakeup);

			if (flag.cheekGlossPower)
				a.cheekGlossPower = b.cheekGlossPower;

			if (flag.detailId)
				a.detailId = b.detailId;

			if (flag.detailPower)
				a.detailPower = b.detailPower;

			if (flag.doubleTooth)
				a.doubleTooth = b.doubleTooth;

			a.eyebrowColor = Apply(a.eyebrowColor, b.eyebrowColor, flag.eyebrowColor);

			if (flag.eyebrowId)
				a.eyebrowId = b.eyebrowId;

			a.eyelineColor = Apply(a.eyelineColor, b.eyelineColor, flag.eyelineColor);

			if (flag.eyelineDownId)
				a.eyelineDownId = b.eyelineDownId;

			if (flag.eyelineUpId)
				a.eyelineUpId = b.eyelineUpId;

			if (flag.eyelineUpWeight)
				a.eyelineUpWeight = b.eyelineUpWeight;

			if (flag.foregroundEyebrow)
				a.foregroundEyebrow = b.foregroundEyebrow;

			if (flag.foregroundEyes)
				a.foregroundEyes = b.foregroundEyes;

			if (flag.headId)
				a.headId = b.headId;

			a.hlDownColor = Apply(a.hlDownColor, b.hlDownColor, flag.hlDownColor);

			if (flag.hlDownId)
				a.hlDownId = b.hlDownId;

			if (flag.hlDownY)
				a.hlDownY = b.hlDownY;

			a.hlUpColor = Apply(a.hlUpColor, b.hlUpColor, flag.hlUpColor);

			if (flag.hlUpId)
				a.hlUpId = b.hlUpId;

			if (flag.hlUpY)
				a.hlUpY = b.hlUpY;

			if (flag.lipGlossPower)
				a.lipGlossPower = b.lipGlossPower;

			a.lipLineColor = Apply(a.lipLineColor, b.lipLineColor, flag.lipLineColor);

			if (flag.lipLineId)
				a.lipLineId = b.lipLineId;

			a.moleColor = Apply(a.moleColor, b.moleColor, flag.moleColor);

			if (flag.moleId)
				a.moleId = b.moleId;

			a.moleLayout = Apply(a.moleLayout, b.moleLayout, flag.moleLayout);

			if (flag.noseId)
				a.noseId = b.noseId;

			Util.Array.Loop(a.pupil, i => Apply(a.pupil[i], b.pupil[i], flag.pupil[i]));

			if (flag.pupilHeight)
				a.pupilHeight = b.pupilHeight;

			if (flag.pupilWidth)
				a.pupilWidth = b.pupilWidth;

			if (flag.pupilX)
				a.pupilX = b.pupilX;

			if (flag.pupilY)
				a.pupilY = b.pupilY;

			Util.Array.Loop(a.shapeValueFace, i =>
			{
				if (flag.shapeValueFace[i])
					a.shapeValueFace[i] = b.shapeValueFace[i];
			});

			if (flag.skinId)
				a.skinId = b.skinId;

			a.whiteBaseColor = Apply(a.whiteBaseColor, b.whiteBaseColor, flag.whiteBaseColor);

			if (flag.whiteId)
				a.whiteId = b.whiteId;

			a.whiteSubColor = Apply(a.whiteSubColor, b.whiteSubColor, flag.whiteSubColor);
		}

		public void Apply(ChaFileBody a, ChaFileBody b, CheckList.ChaFileBody flag)
		{
			if (flag.aerolaSize)
				a.areolaSize = b.areolaSize;

			if (flag.bustSoftness)
				a.bustSoftness = b.bustSoftness;

			if (flag.bustWeight)
				a.bustWeight = b.bustWeight;

			if (flag.detailId)
				a.detailId = b.detailId;

			if (flag.detailPower)
				a.detailPower = b.detailPower;

			if (flag.drawAddLine)
				a.drawAddLine = b.drawAddLine;

			a.nailColor = Apply(a.nailColor, b.nailColor, flag.nailColor);

			if (flag.nailGlossPower)
				a.nailGlossPower = b.nailGlossPower;
			
			a.nipColor = Apply(a.nipColor, b.nipColor, flag.nipColor);

			if (flag.nipGlossPower)
				a.nipGlossPower = b.nipGlossPower;

			if (flag.nipId)
				a.nipId = b.nipId;

			Util.Array.Loop(a.paintColor, i => a.paintColor[i] = Apply(a.paintColor[i], b.paintColor[i], flag.paintColor[i]));

			if (flag.paintId)
				a.paintId = b.paintId;

			Util.Array.Loop(a.paintLayout, i => a.paintLayout[i] = Apply(a.paintLayout[i], b.paintLayout[i], flag.paintLayout[i]));

			if (flag.paintLayoutId)
				a.paintLayoutId = b.paintLayoutId;

			Util.Array.Loop(a.shapeValueBody, i =>
			{
				if (flag.shapeValueBody[i])
					a.shapeValueBody[i] = b.shapeValueBody[i];
			});

			if (flag.skinGlossPower)
				a.skinGlossPower = b.skinGlossPower;

			if (flag.skinId)
				a.skinId = b.skinId;

			a.skinMainColor = Apply(a.skinMainColor, b.skinMainColor, flag.skinMainColor);

			a.skinSubColor = Apply(a.skinSubColor, b.skinSubColor, flag.skinSubColor);

			a.sunburnColor = Apply(a.sunburnColor, b.sunburnColor, flag.sunburnColor);

			if (flag.sunburnId)
				a.sunburnId = b.sunburnId;

			a.underhairColor = Apply(a.underhairColor, b.underhairColor, flag.underhairColor);

			if (flag.underhairId)
				a.underhairId = b.underhairId;
		}

		public void Apply(ChaFileCustom a, ChaFileCustom b, CheckList.ChaFileCustom flag)
		{
			Apply(a.body, b.body, flag.body);

			Apply(a.face, b.face, flag.face);

			Apply(a.hair, b.hair, flag.hair);
		}

		public void Apply(ChaFileAccessory.PartsInfo a, ChaFileAccessory.PartsInfo b, CheckList.ChaFileAccessory.PartsInfo flag)
		{
			var addMove_length = a.addMove.GetLength(1);
			Util.Array.Loop(a.addMove.GetLength(0), x =>
				Util.Array.Loop(addMove_length, y =>
					a.addMove[x, y] = Apply(a.addMove[x, y], b.addMove[x, y], flag.addMove[x * addMove_length + y])
				)
			);

			Util.Array.Loop(a.color, i => a.color[i] = Apply(a.color[i], b.color[i], flag.color[i]));

			if (flag.hideCategory)
				a.hideCategory = b.hideCategory;

			if (flag.id)
				a.id = b.id;

			if (flag.parentKey)
				a.parentKey = b.parentKey;

			if (flag.partsOfHead)
				a.partsOfHead = b.partsOfHead;

			if (flag.type)
				a.type = b.type;
		}

		public void Apply(ChaFileAccessory a, ChaFileAccessory b, CheckList.ChaFileAccessory flag)
		{
			if (flag.overwrite)
			{
				Util.Array.Loop(a.parts, i => Apply(a.parts[i], b.parts[i], flag.parts[i]));
				return;
			}

			var y = 0;
			for (int x = 0; x < a.parts.Length && y < b.parts.Length; x++)
				if (a.parts[x].id == 0)
				{
					while (y < b.parts.Length && b.parts[y].id == 0)
						y++;

					if (y >= b.parts.Length)
						return;

					Apply(a.parts[x], b.parts[y], flag.parts[y]);
					y++;
				}
		}

		public void Apply(ChaFileClothes.PartsInfo.ColorInfo a, ChaFileClothes.PartsInfo.ColorInfo b, CheckList.ChaFileClothes.PartsInfo.ColorInfo flag)
		{
			a.baseColor = Apply(a.baseColor, b.baseColor, flag.baseColor);

			if (flag.pattern)
				a.pattern = b.pattern;

			a.patternColor = Apply(a.patternColor, b.patternColor, flag.patternColor);

			a.tiling = Apply(a.tiling, b.tiling, flag.tiling);
		}

		public void Apply(ChaFileClothes.PartsInfo a, ChaFileClothes.PartsInfo b, CheckList.ChaFileClothes.PartsInfo flag)
		{
			Util.Array.Loop(a.colorInfo, i => Apply(a.colorInfo[i], b.colorInfo[i], flag.colorInfo[i]));
			
			if (flag.emblemeId)
				a.emblemeId = b.emblemeId;

			if (flag.id)
				a.id = b.id;
		}

		public void Apply(ChaFileClothes a, ChaFileClothes b, CheckList.ChaFileClothes flag)
		{
			Util.Array.Loop(a.hideBraOpt, i =>
			{
				if (flag.hideBraOpt[i])
					a.hideBraOpt[i] = b.hideBraOpt[i];
			});

			Util.Array.Loop(a.hideShortsOpt, i =>
			{
				if (flag.hideShortsOpt[i])
					a.hideShortsOpt[i] = b.hideShortsOpt[i];
			});

			Util.Array.Loop(a.parts, i => {
				if (flag.overwrite || b.parts[i].id != 0)
					Apply(a.parts[i], b.parts[i], flag.parts[i]);
			});

			Util.Array.Loop(a.subPartsId, i =>
			{
				if (flag.subPartsId[i])
					a.subPartsId[i] = b.subPartsId[i];
			});
		}

		public void Apply(ChaFileCoordinate a, ChaFileCoordinate b, CheckList.ChaFileCoordinate flag)
		{
			Apply(a.accessory, b.accessory, flag.accessory);

			Apply(a.clothes, b.clothes, flag.clothes);
		}

		public void Apply(ChaFileControl ChaFile, int kind)
		{
			if (checkList == null)
				return;

			if (ChaFile.coordinate[kind] != null)
				Apply(ChaFile.coordinate[kind], coordinate[checkList.schoolUniformAllCoordinates ? 0 : kind], checkList.coordinate[kind]);

			if (ChaFile.custom != null)
				Apply(ChaFile.custom, custom, checkList.custom);
		}
	}
}
