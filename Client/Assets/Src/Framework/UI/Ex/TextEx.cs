using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Text.RegularExpressions;

namespace Framework
{
	using Pool;
    public class TextEx : Text
    {
		[Serializable]
		public class HrefClickEvent : UnityEvent<string> { }


		[Serializable]
		public class TypeWriterChangeEvent : UnityEvent<float, string> { }


		[Serializable]
		public class TypeWriterCompleteEvent : UnityEvent<string> { }

		[SerializeField]
		/// <summary>
		/// 超链接事件监听
		/// </summary>
		private HrefClickEvent m_onHrefClick = new HrefClickEvent();

		[SerializeField]
		/// <summary>
		/// 打字机更新
		/// </summary>
		private TypeWriterChangeEvent m_onTypeWriterChange = new TypeWriterChangeEvent();

		[SerializeField]
		/// <summary>
		/// 打字机完成
		/// </summary>
		private TypeWriterCompleteEvent m_onTypeWriterComplete = new TypeWriterCompleteEvent();

		[SerializeField]
		[Tooltip("是否开启顶点优化")]
		/// <summary>
		/// 顶点优化
		/// </summary>
		private bool m_vertOptimize = false;

		[SerializeField]
		[Tooltip("打字机逐字间隔时间")]
		/// <summary>
		/// 打字机逐字间隔时间
		/// </summary>
		private float m_writeIntervalTime = 0.2f;

		[SerializeField]
		[Tooltip("图片大小")]
		/// <summary>
		/// 图片大小
		/// </summary>
		private int m_imgSize = 16;


		[SerializeField]
		[Tooltip("图片表情配置")]
		/// <summary>
		/// 图片大小
		/// </summary>
		private MarkImg m_markImg = null;

		/// <summary>
		/// 多语言ID
		/// </summary>
		[SerializeField]
		private string m_languageId = string.Empty;

		/// <summary>
		/// 切换语种是否自动刷新
		/// </summary>
		[SerializeField]
		private bool m_autoRefresh = true;

#if UNITY_EDITOR
		[SerializeField]
		[Tooltip("编辑器下查看标记区域")]
		/// <summary>
		/// 编辑器下查看标记区域
		/// </summary>
		private bool m_showMarkBounds = false;
#endif

		/// <summary>
		/// 是否置灰
		/// </summary>
		[SerializeField]
		private bool m_grayColor = false;

		/// <summary>
		/// 格式化数据池
		/// </summary>
		private static Queue<FormatData> m_pool = new Queue<FormatData>(256);

		/// <summary>
		/// 当前文本格式化数据
		/// </summary>
		private List<FormatData> m_formatData = new List<FormatData>();

		/// <summary>
		/// 是否更新标记
		/// </summary>
		private bool m_updateMark = false;

		private class FormatData
		{
			public string languageId;
			public object data;

			public void SetLanguageId(string languageId)
			{
				this.languageId = languageId;
				this.data = null;
			}

			public void SetData(object data)
			{
				this.languageId = string.Empty;
				this.data = data;
			}
		}

		/// <summary>
		/// 目前自定义标记的类型
		/// </summary>
		public enum MarkType
		{
			U,
			Url,
			Img,
		}

		/// <summary>
		/// 标记
		/// </summary>
		public sealed class Mark
		{
			/// <summary>
			/// 匹配规则
			/// </summary>
			private static Regex g_regex = new Regex("(<u[\\s]*(color=#[A-Za-z0-9]+)*[\\s]*(>[\\s\\S]*?<)/u>|<url[\\s]*(color=#[A-Za-z0-9]+)*[\\s]*(href=\"[^>]*\")*[\\s]*(>[\\s\\S]*?<)/url>|<img[\\s]*(width=[0-9]+)*[\\s]*(name=\"[^>]*\")+[\\s]*(>[\\s\\S]?<)/img>)");

			/// <summary>
			/// 标记池
			/// </summary>
			private static Queue<Mark> g_pool = new Queue<Mark>(32);

			/// <summary>
			/// 开始索引
			/// </summary>
			private int m_startIndex = 0;

			/// <summary>
			/// 结束索引
			/// </summary>
			private int m_endIndex = 0;

			/// <summary>
			/// 颜色
			/// </summary>
			private Color m_color = Color.white;

			/// <summary>
			/// 标记区域
			/// </summary>
			private List<Bounds> m_bounds = null;

			/// <summary>
			/// 获取一个标记
			/// </summary>
			/// <returns></returns>
			private static Mark GetFromPool()
			{
				return g_pool.Count > 0 ? g_pool.Dequeue() : new Mark();
			}

			/// <summary>
			/// 返还到池
			/// </summary>
			/// <param name="list"></param>
			public static void GetToPool(List<Mark> data)
			{
				for (int i = 0; i < data.Count; ++i)
				{
					data[i].colorString = null;
					data[i].href = null;

					g_pool.Enqueue(data[i]);
				}
				data.Clear();
			}

			/// <summary>
			/// 开始索引
			/// </summary>
			public int startIndex
			{
				get { return m_startIndex; }
				set { m_startIndex = value; }
			}

			/// <summary>
			/// 结束索引
			/// </summary>
			public int endIndex
			{
				get { return m_endIndex; }
				set { m_endIndex = value; }
			}

			/// <summary>
			/// 颜色
			/// </summary>
			public Color color
			{
				get { return m_color; }
				set { m_color = value; }
			}

			/// <summary>
			/// 标记区域
			/// </summary>
			public List<Bounds> bounds
			{
				get { return m_bounds; }
				set { m_bounds = value; }
			}

			/// <summary>
			/// 标记类型
			/// </summary>
			public MarkType markType
			{
				get; set;
			}

			/// <summary>
			/// 颜色字符串
			/// </summary>
			public string colorString
			{
				get; set;
			}

			/// <summary>
			/// 超链接网址
			/// </summary>
			public string href
			{
				get; set;
			}

			/// <summary>
			/// 标记图大小
			/// </summary>
			public int imgSize
			{
				get; set;
			}

			/// <summary>
			/// 标记图名
			/// </summary>
			public string imgName
			{
				get; set;
			}

			/// <summary>
			/// 图片表情索引
			/// </summary>
			public int imgIndex
			{
				get; set;
			}

			/// <summary>
			/// 图片表情播放时间
			/// </summary>
			public float imgPlayTime
			{
				get; set;
			}

			/// <summary>
			/// 图片表情数据
			/// </summary>
			public MarkImg.Img img
			{
				get; set;
			}

			/// <summary>
			/// 构造
			/// </summary>
			public Mark()
			{
				m_bounds = new List<Bounds>();
			}

			/// <summary>
			/// 设置标记类型
			/// </summary>
			/// <param name="value"></param>
			private void SetMarkType(string value)
			{
				if (value.StartsWith("<u ") || value.StartsWith("<u>"))
				{
					markType = MarkType.U;
				}
				else if (value.StartsWith("<url ") || value.StartsWith("<url>"))
				{
					markType = MarkType.Url;
				}
				else if (value.StartsWith("<img ") || value.StartsWith("<img>"))
				{
					markType = MarkType.Img;
				}
			}

			/// <summary>
			/// 查找标记
			/// </summary>
			/// <param name="value"></param>
			/// <param name="data"></param>
			/// <returns></returns>
			public static string FindMark(string value, List<Mark> data, int fontSize, float preferredWidth, int imgSize = 0)
			{
				GetToPool(data);

				int startIndex = 0;
				string temp = string.Empty;
				var m = g_regex.Match(value, startIndex);
				while (m.Success)
				{
					Mark mark = GetFromPool();
					data.Add(mark);
					mark.SetMarkType(m.Value);
					mark.imgSize = 0;
					foreach (Group g in m.Groups)
					{
						if (g.Success)
						{
							if (g.Value.StartsWith("color=#"))
							{
								mark.colorString = g.Value.Substring(6);
							}
							else if (g.Value.StartsWith("href="))
							{
								mark.href = g.Value.Substring(6, g.Value.Length - 7);
							}
							else if (g.Value.StartsWith("name="))
							{
								mark.imgName = g.Value.Substring(6, g.Value.Length - 7);
							}
							else if (g.Value.StartsWith("width="))
							{
								int width = fontSize;
								if (int.TryParse(g.Value.Substring(6), out width))
								{
									width = Mathf.CeilToInt(width / preferredWidth * fontSize);
								}
								mark.imgSize = width;
							}
							else if (g.Value.StartsWith(">") && g.Value.EndsWith("<"))
							{
								if (mark.markType == MarkType.Img)
								{
									if (imgSize > 0)
									{
										mark.imgSize = imgSize;
									}
									else if (mark.imgSize == 0)
									{
										mark.imgSize = fontSize;
									}
									temp = string.Format("<size={0}>　</size>", mark.imgSize);
									mark.startIndex = m.Index;
									mark.endIndex = m.Index + temp.Length;

									value = value.Substring(0, m.Index) + temp + value.Substring(m.Index + m.Value.Length);
								}
								else
								{
									temp = g.Value.Substring(1, g.Value.Length - 2);
									mark.startIndex = m.Index;
									mark.endIndex = m.Index + temp.Length;

									value = value.Substring(0, m.Index) + temp + value.Substring(m.Index + m.Value.Length);
								}
							}
						}
					}
					startIndex = mark.endIndex;
					m = g_regex.Match(value, startIndex);
				}

				return value;
			}
		}

		/// <summary>
		/// 打字机
		/// </summary>
		public sealed class Typewriter
		{
			/// <summary>
			/// 匹配规则
			/// </summary>
			private static Regex g_regex = new Regex("(<u[\\s\\S]*(>[\\s\\S]*?<)/u>|<url[\\s\\S]*(>[\\s\\S]*?<)/url>|<img[\\s\\S]*(>[\\s\\S]*?<)/img>|<color=[\\s\\S]*(>[\\s\\S]*?<)/color>|<size=[0-9]*(>[\\s\\S]*?<)/size>|<b(>[\\s\\S]*?<)/b>|<i(>[\\s\\S]*?<)/i>)");

			/// <summary>
			/// 标记池
			/// </summary>
			private static Queue<Typewriter> g_pool = new Queue<Typewriter>(32);

			/// <summary>
			/// 开始索引
			/// </summary>
			private int m_startIndex = 0;

			/// <summary>
			/// 结束索引
			/// </summary>
			private int m_endIndex = 0;

			/// <summary>
			/// 格式
			/// </summary>
			private string m_format = string.Empty;

			/// <summary>
			/// 内容
			/// </summary>
			private string m_content = string.Empty;

			/// <summary>
			/// 获取一个标记
			/// </summary>
			/// <returns></returns>
			private static Typewriter GetFromPool()
			{
				return g_pool.Count > 0 ? g_pool.Dequeue() : new Typewriter();
			}

			/// <summary>
			/// 返还到池
			/// </summary>
			/// <param name="list"></param>
			private static void GetToPool(List<Typewriter> data)
			{
				for (int i = 0; i < data.Count; ++i)
				{
					data[i].format = string.Empty;
					data[i].content = string.Empty;

					g_pool.Enqueue(data[i]);
				}
				data.Clear();
			}

			/// <summary>
			/// 开始索引
			/// </summary>
			public int startIndex
			{
				get { return m_startIndex; }
				set { m_startIndex = value; }
			}

			/// <summary>
			/// 结束索引
			/// </summary>
			public int endIndex
			{
				get { return m_endIndex; }
				set { m_endIndex = value; }
			}

			/// <summary>
			/// 格式
			/// </summary>
			public string format
			{
				get { return m_format; }
				set { m_format = value; }
			}

			/// <summary>
			/// 内容
			/// </summary>
			public string content
			{
				get { return m_content; }
				set { m_content = value; }
			}

			/// <summary>
			/// 查找标记
			/// </summary>
			/// <param name="value"></param>
			/// <param name="data"></param>
			/// <returns></returns>
			public static string Find(string value, List<Typewriter> data)
			{
				GetToPool(data);

				int startIndex = 0;
				Typewriter typewriter = null;
				string temp = string.Empty;
				var m = g_regex.Match(value, startIndex);
				while (m.Success)
				{
					// 无标记内容添加
					if (startIndex < m.Index)
					{
						typewriter = GetFromPool();
						data.Add(typewriter);

						typewriter.content = value.Substring(startIndex, m.Index - startIndex);
						typewriter.format = "{0}";

						typewriter.startIndex = temp.Length;
						temp += typewriter.content;
						typewriter.endIndex = temp.Length;
					}

					typewriter = GetFromPool();
					data.Add(typewriter);
					foreach (Group g in m.Groups)
					{
						if (g.Success)
						{
							if (g.Value.StartsWith(">") && g.Value.EndsWith("<"))
							{
								typewriter.content = g.Value.Substring(1, g.Value.Length - 2);
								typewriter.format = m.Value.Substring(0, g.Index - m.Index + 1) + "{0}" + m.Value.Substring(g.Index - m.Index + g.Value.Length - 1);

								typewriter.startIndex = temp.Length;
								temp += typewriter.content;
								typewriter.endIndex = temp.Length;
							}
						}
					}
					startIndex = m.Index + m.Value.Length;
					m = g_regex.Match(value, startIndex);
				}
				if (startIndex < value.Length)
				{
					typewriter = GetFromPool();
					data.Add(typewriter);

					typewriter.content = value.Substring(startIndex, value.Length - startIndex);
					typewriter.format = "{0}";

					typewriter.startIndex = temp.Length;
					temp += typewriter.content;
					typewriter.endIndex = temp.Length;
				}

				return temp;
			}
		}


		/// <summary>
		/// 缓存顶点
		/// </summary>
		private readonly UIVertex[] m_TempVerts = new UIVertex[4];

		/// <summary>
		/// 标记列表
		/// </summary>
		private List<Mark> m_markList = new List<Mark>();

		/// <summary>
		/// 打字机列表
		/// </summary>
		private List<Typewriter> m_typewriterList = new List<Typewriter>();

		/// <summary>
		/// 缓存的文本
		/// </summary>
		private string m_cacheText = string.Empty;

		/// <summary>
		/// 超链接事件
		/// </summary>
		public HrefClickEvent onHrefClick
		{
			get
			{
				return m_onHrefClick;
			}
		}

		/// <summary>
		/// 打字机变化
		/// </summary>
		public TypeWriterChangeEvent onTypeWriterChange
		{
			get
			{
				return m_onTypeWriterChange;
			}
		}

		/// <summary>
		/// 打字机完成
		/// </summary>
		public TypeWriterCompleteEvent onTypeWriterComplete
		{
			get
			{
				return m_onTypeWriterComplete;
			}
		}

		/// <summary>
		/// 打字机内容
		/// </summary>
		public string typewriterText
		{
			set
			{
				StopCoroutine("StartTypewriter");
				StartCoroutine("StartTypewriter", value);
			}
		}

		/// <summary>
		/// 打字机间隔时间
		/// </summary>
		public float writeIntervalTime
		{
			get
			{
				return m_writeIntervalTime;
			}
			set
			{
				m_writeIntervalTime = value;
			}
		}

		/// <summary>
		/// 图片大小
		/// </summary>
		public int imgSize
		{
			get
			{
				return m_imgSize;
			}
			set
			{
				m_imgSize = value;
			}
		}

		private void Awake()
		{
			base.Awake();
			if (Application.isPlaying && Lua.hasInstance)
			{
				Show();
			}
			if (m_grayColor)
			{
				SetGray(m_grayColor);
			}
		}

		/// <summary>
		/// 为True设置置灰效果
		/// </summary>
		/// <param name="enable"></param>
		/// <param name="power"></param>
		public void SetGray(bool enable, float power = 1f)
		{
			if (!enable && material.name != "UI/UIEffect")
			{
				return;
			}
			UIEffectIns.SetGrayEffect(this, enable, power);
		}

		/// <summary>
		/// 逐字
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		private IEnumerator StartTypewriter(string value)
		{
			string temp = Typewriter.Find(value, m_typewriterList);
			int Cnt = temp.Length;
			int num = 0;

			text = string.Empty;
			while (num++ < Cnt)
			{
				yield return new WaitForSeconds(m_writeIntervalTime);
				temp = string.Empty;
				foreach (var data in m_typewriterList)
				{
					if (data.endIndex < num)
					{
						temp += string.Format(data.format, data.content);
					}
					else
					{
						temp += string.Format(data.format, data.content.Substring(0, num - data.startIndex));
						break;
					}
				}
				text = temp;
				if (m_onTypeWriterChange != null)
				{
					m_onTypeWriterChange.Invoke((float)num / Cnt, temp);
				}
			}
			if (m_onTypeWriterComplete != null)
			{
				m_onTypeWriterComplete.Invoke(temp);
			}
		}

		/// <summary>
		/// 是否是同一行
		/// </summary>
		/// <param name="lines"></param>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <returns></returns>
		private bool IsSameLine(UILineInfo[] lines, ref Vector3 startPos, ref Vector3 endPos)
		{
			foreach (var line in lines)
			{
				if (startPos.y <= line.topY && startPos.y >= line.topY - line.height)
				{
					return endPos.y <= line.topY && endPos.y >= line.topY - line.height;
				}
			}
			return false;
		}

		/// <summary>
		/// 得到指定位置行信息
		/// </summary>
		/// <param name="lines"></param>
		/// <param name="v"></param>
		/// <returns></returns>
		private UILineInfo GetLineInfo(UILineInfo[] lines, ref Vector3 pos)
		{
			for (int i = 1; i < lines.Length; ++i)
			{
				if (pos.y <= lines[i].topY && pos.y >= lines[i].topY - lines[i].height)
				{
					return lines[i];
				}
			}
			return lines[0];
		}

		/// <summary>
		/// 添加标记区域
		/// </summary>
		/// <param name="mark"></param>
		/// <param name="lines"></param>
		/// <param name="startPos"></param>
		/// <param name="endPos"></param>
		private void AddBoundsToMark(Mark mark, UILineInfo[] lines, ref UIVertex start, ref UIVertex end)
		{
			UILineInfo line = GetLineInfo(lines, ref start.position);
			Vector3 boundsCenter = new Vector2((start.position.x + end.position.x) * 0.5f, line.topY - line.height * 0.5f);
			Vector3 boundsSize = new Vector2(end.position.x - start.position.x, line.height);
			mark.bounds.Add(new Bounds(boundsCenter, boundsSize));
			mark.color = start.color;
#if UNITY_EDITOR
			if (m_showMarkBounds)
			{
				var c = gameObject.AddComponent<BoxCollider2D>();
				c.offset = boundsCenter;
				c.size = boundsSize;
			}
#endif
		}

		/// <summary>
		/// 填充mesh
		/// </summary>
		/// <param name="toFill"></param>
		protected override void OnPopulateMesh(VertexHelper toFill)
		{
			if (font == null)
			{
				return;
			}
			m_DisableFontTextureRebuiltCallback = true;

			Vector2 size = rectTransform.rect.size;
			TextGenerationSettings generationSettings = GetGenerationSettings(size);
			if (supportRichText)
			{
				m_cacheText = Mark.FindMark(text, m_markList, fontSize, cachedTextGeneratorForLayout.GetPreferredWidth("　", generationSettings) / pixelsPerUnit, m_imgSize);
			}
			else
			{
				Mark.GetToPool(m_markList);
				m_cacheText = text;
			}
			cachedTextGenerator.PopulateWithErrors(m_cacheText, generationSettings, gameObject);
			IList<UIVertex> verts = cachedTextGenerator.verts;
			float num = 1f / pixelsPerUnit;
			int num2 = verts.Count - 4;
			Vector2 vector = Vector2.zero;
			if (verts.Count > 0)
			{
				vector = new Vector2(verts[0].position.x, verts[0].position.y) * num;
			}
			vector = PixelAdjustPoint(vector) - vector;
			toFill.Clear();
			if (vector != Vector2.zero)
			{
				for (int i = 0; i < num2; i++)
				{
					int num3 = i & 3;
					m_TempVerts[num3] = verts[i];
					m_TempVerts[num3].position *= num;
					m_TempVerts[num3].position.x += vector.x;
					m_TempVerts[num3].position.y += vector.y;
					if (num3 == 3)
					{
						toFill.AddUIVertexQuad(m_TempVerts);
					}
				}
			}
			else
			{
				for (int i = 0; i < num2; i++)
				{
					int num4 = i & 3;
					m_TempVerts[num4] = verts[i];
					m_TempVerts[num4].position *= num;
					if (num4 == 3)
					{
						toFill.AddUIVertexQuad(m_TempVerts);
					}
				}
			}

			// 如果标记弄脏了，需要处理
			if (supportRichText)
			{
				// 计算区域
				UIVertex start = UIVertex.simpleVert, end = UIVertex.simpleVert, temp = UIVertex.simpleVert;
#if UNITY_EDITOR
				var colliders = GetComponents<BoxCollider2D>();
				foreach (var c in colliders)
				{
					DestroyImmediate(c);
				}
#endif
				var lines = cachedTextGenerator.GetLinesArray();
				for (int i = 0; i < lines.Length; ++i)
				{
					lines[i].topY *= num;
					lines[i].height = Mathf.CeilToInt(num * lines[i].height);
				}

				foreach (var mark in m_markList)
				{
					mark.bounds.Clear();
					//文本过长，显示区域截断处理
					if (mark.startIndex * 4 >= toFill.currentVertCount)
					{
						break;
					}
					toFill.PopulateUIVertex(ref start, mark.startIndex * 4);
					for (int i = mark.startIndex; i < mark.endIndex; ++i)
					{
						//文本过长，显示区域截断处理
						if (i * 4 >= toFill.currentVertCount)
						{
							toFill.PopulateUIVertex(ref end, (i - 1) * 4 + 1);
							AddBoundsToMark(mark, lines, ref start, ref end);
							break;
						}
						toFill.PopulateUIVertex(ref temp, i * 4);
						//换行了
						if (!IsSameLine(lines, ref start.position, ref temp.position) && i > 0)
						{
							toFill.PopulateUIVertex(ref end, (i - 1) * 4 + 1);
							AddBoundsToMark(mark, lines, ref start, ref end);
							start = temp;
						}
						//结束了
						if (i + 1 == mark.endIndex)
						{
							toFill.PopulateUIVertex(ref end, i * 4 + 1);
							AddBoundsToMark(mark, lines, ref start, ref end);
						}
					}
				}

				// 顶点优化
				if (m_vertOptimize)
				{
					List<UIVertex> cacheVerts = new List<UIVertex>();
					toFill.GetUIVertexStream(cacheVerts);
					toFill.Clear();

					for (int i = 0; i < cacheVerts.Count;)
					{
						m_TempVerts[0] = cacheVerts[i];
						m_TempVerts[1] = cacheVerts[i + 1];
						m_TempVerts[2] = cacheVerts[i + 2];
						m_TempVerts[3] = cacheVerts[i + 4];
						if (InvalidVertexQuad())
						{
							toFill.AddUIVertexQuad(m_TempVerts);
						}
						i += 6;
					}
				}
			}

			m_DisableFontTextureRebuiltCallback = false;
			m_updateMark = true;
		}

		/// <summary>
		/// 有效四顶点判定
		/// </summary>
		/// <returns></returns>
		private bool InvalidVertexQuad()
		{
			return Mathf.Abs(m_TempVerts[0].position.x - m_TempVerts[1].position.x) > 1f && Mathf.Abs(m_TempVerts[0].position.y - m_TempVerts[3].position.y) > 1f;
		}

		/// <summary>
		/// 填充标记
		/// </summary>
		private void OnPopulateMark()
		{
			if (supportRichText && m_markList != null && m_markList.Count > 0)
			{
				RectTransform tf = transform.Find("TextMark") as RectTransform;
				if (tf == null)
				{
					GameObject go = new GameObject("TextMark");
					tf = go.AddComponent<RectTransform>();
					tf.parent = transform;
					tf.localPosition = Vector3.zero;
					tf.localScale = Vector3.one;
					//设定Content中心点、描点、位置
					tf.pivot = rectTransform.pivot;
					tf.anchorMin = Vector2.zero;
					tf.anchorMax = Vector2.one;
					tf.anchoredPosition = Vector3.zero;
					tf.offsetMin = Vector2.zero;
					tf.offsetMax = Vector2.zero;
				}
				tf.gameObject.SetActive(true);
				var textMark = tf.gameObject.GetComponent<TextMark>();
				if (textMark == null)
				{
					textMark = tf.gameObject.AddComponent<TextMark>();
					textMark.Init(m_markImg);
				}
				textMark.SetMarkInfo(m_markList, m_onHrefClick, raycastTarget);
			}
			else
			{
				RectTransform tf = transform.Find("TextMark") as RectTransform;
				if (tf != null)
				{
					tf.gameObject.SetActive(false);
				}
			}
		}

        private void LateUpdate()
        {
			if (m_updateMark)
			{
				OnPopulateMark();
				m_updateMark = false;
			}
		}

#if UNITY_EDITOR
		void OnDrawGizmos()
		{
			if (!Application.isPlaying)
			{
				if (m_updateMark)
				{
					OnPopulateMark();
					m_updateMark = false;
				}
			}
		}
#endif

#if UNITY_EDITOR
		//[ContextMenu("OnHrefClick")]
		//public void OnHrefClickTest()
		//{
		//	text = "<url href=\"onClick\">我是超链接标记</url><u>我是下划线标记</u>";

		//	onHrefClick.AddListener((s) =>
		//	{
		//		Debug.LogError(s);
		//	});
		//}

		//[ContextMenu("打字机")]
		//public void TypewriterTest()
		//{
		//	List<Typewriter> data = new List<Typewriter>();
		//	string temp = "0123<i>i</i><b>b</b>123456<size=22>size</size><color=red>color</color><u>u</u><url>url</url>456";
		//	Typewriter.Find(temp, data);
		//	onTypeWriterChange.AddListener((f, s) =>
		//	{
		//		Debug.LogError(string.Format("打字机进度：{0:F2}, {1}", f, s));
		//	});
		//	onTypeWriterComplete.AddListener((s) =>
		//	{
		//		Debug.LogError("打字机完成：" + s);
		//	});
		//	typewriterText = temp;
		//}
#endif

		/// <summary>
		/// 刷新文本
		/// </summary>
		public void RefreshText()
		{
			Show();
		}

		/// <summary>
		/// 设置语言Id
		/// </summary>
		/// <param name="languageId"></param>
		/// <returns></returns>
		public TextEx SetLanguageId(string languageId)
		{
			m_languageId = languageId;
			for (int i = 0; i < m_formatData.Count; ++i)
			{
				m_pool.Enqueue(m_formatData[i]);
			}
			m_formatData.Clear();
			return this;
		}

		/// <summary>
		/// 添加格式化的语言Id
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public TextEx AddFormatLangId(params string[] args)
		{
			FormatData formatData = null;
			for (int i = 0; i < args.Length; ++i)
			{
				if (m_pool.Count > 0)
				{
					formatData = m_pool.Dequeue();
				}
				else
				{
					formatData = new FormatData();
				}
				formatData.SetLanguageId(args[i]);
				m_formatData.Add(formatData);
			}
			return this;
		}

		/// <summary>
		/// 添加格式化内容
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public TextEx AddFormatContent(params object[] args)
		{
			FormatData formatData = null;
			for (int i = 0; i < args.Length; ++i)
			{
				if (null != args[i])
				{
					if (m_pool.Count > 0)
					{
						formatData = m_pool.Dequeue();
					}
					else
					{
						formatData = new FormatData();
					}
					formatData.SetData(args[i]);
					m_formatData.Add(formatData);
				}
			}
			return this;
		}

		/// <summary>
		/// 显示
		/// </summary>
		public void Show(object txt = null)
		{
			UI.UIManager.instance.onLoadTargetFont(this);

			if (!m_languageId.Equals("0"))
            {
				if (null != txt)
				{
					this.text = txt.ToString();
				}
				if (!m_languageId.Equals(string.Empty))
				{
					string language = Lua.instance.GetLanguage(m_languageId);
					object[] args = new object[m_formatData.Count];
					for (int i = 0; i < m_formatData.Count; ++i)
					{
						if (null == m_formatData[i].data)
						{
							args[i] = Lua.instance.GetLanguage(m_formatData[i].languageId);
						}
						else
						{
							args[i] = m_formatData[i].data;
						}
					}
					this.text = string.Format(language, args).Replace("/n", "\n");
				}
			}
		}
	}
}