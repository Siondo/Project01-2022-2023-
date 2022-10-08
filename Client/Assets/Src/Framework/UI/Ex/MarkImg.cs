using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkImg : ScriptableObject
{
	[System.Serializable]
	public class Img
	{
		[SerializeField]
		/// <summary>
		/// 图片组名
		/// </summary>
		private string m_name;

		[SerializeField]
		/// <summary>
		/// 图标切换间隔时间
		/// </summary>
		private float m_intervalTime = 0.05F;

		[SerializeField]
		/// <summary>
		/// 是否开启循环
		/// </summary>
		private bool m_loop = false;

		[SerializeField]
		/// <summary>
		/// 循环播放间隔时间
		/// </summary>
		private float m_loopIntervalTime = 0.5F;

		[SerializeField]
		/// <summary>
		/// 精灵组
		/// </summary>
		private List<Sprite> m_sprite;

		/// <summary>
		/// 图片组名
		/// </summary>
		public string name => m_name;

		/// <summary>
		/// 图标切换间隔时间
		/// </summary>
		public float intervalTime => m_intervalTime;

		/// <summary>
		/// 循环播放间隔时间
		/// </summary>
		public bool loop => m_loop;

		/// <summary>
		/// 循环播放间隔时间
		/// </summary>
		public float loopIntervalTime => m_loopIntervalTime;

		/// <summary>
		/// 精灵组
		/// </summary>
		public List<Sprite> sprite => m_sprite;

		/// <summary>
		/// 得到第一个精灵
		/// </summary>
		public Sprite mainSprite => m_sprite[0];
	}

	[SerializeField]
	/// <summary>
	/// 图片表情配置
	/// </summary>
	private Img[] m_config;

	/// <summary>
	/// 字典数据
	/// </summary>
	private Dictionary<string, Img> m_data = null;

	/// <summary>
	/// 得到标记信息
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	public Img Get(string name = "_")
	{
		if (null == m_data)
		{
			m_data = new Dictionary<string, Img>();
			for (int i = 0; i < m_config.Length; ++i)
			{
				m_data.Add(m_config[i].name, m_config[i]);
			}
		}
		return m_data.ContainsKey(name) ? m_data[name] : null;
	}
}
