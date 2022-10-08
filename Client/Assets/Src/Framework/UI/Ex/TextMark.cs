using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.Sprites;

namespace Framework
{
	public class TextMark : MaskableGraphic, IPointerClickHandler
	{
		/// <summary>
		/// 缓存下划线、超链接顶点
		/// </summary>
		private readonly UIVertex[] m_TempVerts = new UIVertex[4];

		/// <summary>
		/// 缓存图标表情顶点
		/// </summary>
		private readonly UIVertex[] m_TempVertsImg = new UIVertex[4];

		/// <summary>
		/// 标记信息
		/// </summary>
		private List<TextEx.Mark> m_markList = null;

		/// <summary>
		/// 标记信息
		/// </summary>
		private List<TextEx.Mark> m_imgList = null;

		/// <summary>
		/// 超链接事件
		/// </summary>
		private TextEx.HrefClickEvent m_onHrefClick = null;

		/// <summary>
		/// 是否标记更新
		/// </summary>
		private bool m_imgUpdate = false;

		[SerializeField]
		[Tooltip("图片表情数据")]
		/// <summary>
		/// 图片表情数据
		/// </summary>
		private MarkImg m_markImg;

		[SerializeField]
		[Tooltip("下划线使用精灵对象")]
		/// <summary>
		/// 下划线使用精灵对象
		/// </summary>
		private Sprite m_Sprite;

		/// <summary>
		/// 得到下划线对象
		/// </summary>
		private Sprite activeSprite
		{
			get
			{
				return m_Sprite;
			}
		}

		/// <summary>
		/// 主纹理
		/// </summary>
		public override Texture mainTexture
		{
			get
			{
				if (activeSprite == null)
				{
					if (material != null && material.mainTexture != null)
					{
						return material.mainTexture;
					}
					return Graphic.s_WhiteTexture;
				}
				return activeSprite.texture;
			}
		}

		/// <summary>
		/// 时间
		/// </summary>
		public float realtimeSinceStartup
		{
			get
			{
				return Time.realtimeSinceStartup;
			}
		}

		/// <summary>
		/// 构造
		/// </summary>
		protected TextMark()
		{
			base.useLegacyMeshGeneration = false;
		}

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="markImg"></param>
		public void Init(MarkImg markImg)
		{
			m_markImg = markImg;
			if (m_markImg != null)
			{
				m_Sprite = markImg.Get().mainSprite;
			}
		}

		/// <summary>
		/// 设置标记信息
		/// </summary>
		/// <param name="markList"></param>
		public void SetMarkInfo(List<TextEx.Mark> markList, TextEx.HrefClickEvent onHrefClick, bool raycast)
		{
			m_markList = markList;
			m_onHrefClick = onHrefClick;
			raycastTarget = raycast;

			if (m_imgList == null)
			{
				m_imgList = new List<TextEx.Mark>();
			}
			else
			{
				m_imgList.Clear();
			}
			for (int i = 0; i < m_markList.Count; ++i)
			{
				if (m_markList[i].markType == TextEx.MarkType.Img)
				{
					m_markList[i].imgIndex = 0;
					m_markList[i].imgPlayTime = realtimeSinceStartup;
					m_markList[i].img = m_markImg.Get(m_markList[i].imgName);
					m_imgList.Add(m_markList[i]);
				}
			}

			SetAllDirty();
		}

		/// <summary>
		/// 得到绘制的尺寸
		/// </summary>
		/// <param name="sprite"></param>
		/// <param name="center"></param>
		/// <param name="extents"></param>
		/// <returns></returns>
		private Vector4 GetDrawingDimensions(Sprite sprite, Vector3 center, Vector2 extents)
		{
			float halfY = extents.y;
			if (sprite != null)
			{
				halfY = extents.x * sprite.rect.height / sprite.rect.width;
			}

			return new Vector4(center.x - extents.x, center.y - halfY, center.x + extents.x, center.y + halfY);
		}

		protected void Update()
		{
			if (m_imgList != null)
			{
				m_imgUpdate = false;
				for (int i = 0; i < m_imgList.Count; ++i)
				{
					if (m_imgList[i].img.loop || m_imgList[i].imgIndex < m_imgList[i].img.sprite.Count - 1)
					{
						if (realtimeSinceStartup > m_imgList[i].imgPlayTime + m_imgList[i].img.intervalTime)
						{
							m_imgList[i].imgIndex += 1;
							m_imgList[i].imgPlayTime = realtimeSinceStartup;

							if (m_imgList[i].imgIndex == m_imgList[i].img.sprite.Count - 1)
							{
								m_imgList[i].imgPlayTime += m_imgList[i].img.loopIntervalTime;
							}
							else if (m_imgList[i].imgIndex == m_imgList[i].img.sprite.Count)
							{
								m_imgList[i].imgIndex = 0;
							}
							m_imgUpdate = true;
						}
					}
				}
				if (m_imgUpdate)
				{
					SetAllDirty();
				}
			}
		}

		/// <summary>
		/// 填充网格
		/// </summary>
		/// <param name="vh"></param>
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
			if (m_markList == null || m_markList.Count == 0) return;

			//添加下划线顶点
			Vector4 underlineVector = (activeSprite != null) ? DataUtility.GetOuterUV(activeSprite) : Vector4.zero;
			m_TempVerts[0].uv0 = new Vector2(underlineVector.x, underlineVector.w);
			m_TempVerts[1].uv0 = new Vector2(underlineVector.z, underlineVector.w);
			m_TempVerts[2].uv0 = new Vector2(underlineVector.z, underlineVector.y);
			m_TempVerts[3].uv0 = new Vector2(underlineVector.x, underlineVector.y);

			// 精灵图
			Color color = this.color;
			m_TempVertsImg[0].color = color;
			m_TempVertsImg[1].color = color;
			m_TempVertsImg[2].color = color;
			m_TempVertsImg[3].color = color;

			foreach (var mark in m_markList)
			{
				switch (mark.markType)
				{
					case TextEx.MarkType.U:
					case TextEx.MarkType.Url:
						{
							if (!string.IsNullOrEmpty(mark.colorString))
							{
								Color c;
								if (ColorUtility.TryParseHtmlString(mark.colorString, out c))
								{
									mark.color = c;
								}
							}
							m_TempVerts[0].color = mark.color;
							m_TempVerts[1].color = mark.color;
							m_TempVerts[2].color = mark.color;
							m_TempVerts[3].color = mark.color;
							foreach (var bound in mark.bounds)
							{
								m_TempVerts[0].position = new Vector3(bound.min.x, bound.min.y + bound.size.y * 0.1f);
								m_TempVerts[1].position = new Vector3(bound.max.x, bound.min.y + bound.size.y * 0.1f);
								m_TempVerts[2].position = new Vector3(bound.max.x, bound.min.y + 1f);
								m_TempVerts[3].position = new Vector3(bound.min.x, bound.min.y + 1f);
								vh.AddUIVertexQuad(m_TempVerts);
							}
						}
						break;
					case TextEx.MarkType.Img:
						{
							foreach (var bound in mark.bounds)
							{
								TrySetMarkImg(ref vh, mark, bound);
							}
						}
						break;
				}
			}
		}

		/// <summary>
		/// 点击事件检测是否点击到超链接文本
		/// </summary>
		/// <param name="eventData"></param>
		public void OnPointerClick(PointerEventData eventData)
		{
			Vector2 pos;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out pos);

			foreach (var mark in m_markList)
			{
				if (mark.markType == TextEx.MarkType.Url && mark.href != null)
				{
					foreach (var bound in mark.bounds)
					{
						if (bound.Contains(pos))
						{
							m_onHrefClick.Invoke(mark.href);
							return;
						}
					}
				}
			}
		}

		/// <summary>
		/// 更新材质球
		/// </summary>
		protected override void UpdateMaterial()
		{
			base.UpdateMaterial();
			if (activeSprite == null)
			{
				base.canvasRenderer.SetAlphaTexture(null);
				return;
			}
			Texture2D associatedAlphaSplitTexture = activeSprite.associatedAlphaSplitTexture;
			if (associatedAlphaSplitTexture != null)
			{
				base.canvasRenderer.SetAlphaTexture(associatedAlphaSplitTexture);
			}
		}

		/// <summary>
		/// 尝试设置图片表情
		/// </summary>
		/// <param name="vh"></param>
		/// <param name="name"></param>
		/// <param name="bound"></param>
		private void TrySetMarkImg(ref VertexHelper vh, TextEx.Mark mark, Bounds bound)
		{
			Sprite curSprite = mark.img.sprite[mark.imgIndex];

			if (curSprite != null)
			{
				Vector4 drawingDimensions = GetDrawingDimensions(curSprite, bound.center, bound.extents);
				Vector4 vector = DataUtility.GetOuterUV(curSprite);

				m_TempVertsImg[0].position = new Vector3(drawingDimensions.x, drawingDimensions.w);
				m_TempVertsImg[0].uv0 = new Vector2(vector.x, vector.w);
				m_TempVertsImg[1].position = new Vector3(drawingDimensions.z, drawingDimensions.w);
				m_TempVertsImg[1].uv0 = new Vector2(vector.z, vector.w);
				m_TempVertsImg[2].position = new Vector3(drawingDimensions.z, drawingDimensions.y);
				m_TempVertsImg[2].uv0 = new Vector2(vector.z, vector.y);
				m_TempVertsImg[3].position = new Vector3(drawingDimensions.x, drawingDimensions.y);
				m_TempVertsImg[3].uv0 = new Vector2(vector.x, vector.y);
				vh.AddUIVertexQuad(m_TempVertsImg);
			}
		}
	}
}
