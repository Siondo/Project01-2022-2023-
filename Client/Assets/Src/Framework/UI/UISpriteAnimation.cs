using System;
using UnityEngine;
using UnityEngine.UI;

public class UISpriteAnimation : MonoBehaviour
{
	public int m_fps = 5;
	public bool m_loop = false;
	public bool m_snap = true;

	[SerializeField]
	private bool m_restart = false;
	[SerializeField]
    private Image m_image;
    [SerializeField]
    private Sprite[] m_sprites;

	private bool m_active = false;
	private float m_delta;
	private int m_count;
	private int m_cur;
	private Action m_callback;
    // Start is called before the first frame update
    private void Start()
    {
		if (m_loop)
		{
			Play(null);
		}
	}

	[ContextMenu("Exe")]
	public void Exe()
	{
		Play(null);
	}

    public void OnEnable()
    {
		if (m_restart)
		{
			Play(null);
		}
	}

    public void Play(Action act)
	{
		ResetImage();
		m_callback = act;
		m_count = m_sprites.Length;
		m_active = true;
		m_image.gameObject.SetActive(m_active);
	}

    public void ResetImage()
    {
		m_active = false;
		m_cur = 0;
		m_image.sprite = m_sprites[m_cur];
		if (m_snap)
		{
			m_image.SetNativeSize();
		}
	}

    // Update is called once per frame
    void Update()
    {
		if (m_active && m_count > 1 && Application.isPlaying && m_fps > 0f)
		{
			m_delta += Time.deltaTime;
			float rate = 1f / m_fps;
			if (rate < m_delta)
			{
				m_delta = (rate > 0f) ? m_delta - rate : 0f;
				if (++m_cur >= m_count)
				{
					m_cur = 0;
					m_active = m_loop;
				}

				if (m_active)
				{
					m_image.sprite = m_sprites[m_cur];
					if (m_snap)
					{
						m_image.SetNativeSize();
					}
				}
				else
                {
					m_image.gameObject.SetActive(m_active);
					m_callback?.Invoke();
				}
			}
		}
	}
}
