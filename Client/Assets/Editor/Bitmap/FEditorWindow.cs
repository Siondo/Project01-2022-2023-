using UnityEditor;
using UnityEngine;

namespace FFramework.UnityEditor
{
	public abstract class FEditorWindow : EditorWindow
	{
		private object obj;
		public EState State = EState.OnAwake;

		public enum EState : byte
		{
			OnAwake,
			OnBegin,
			OnEnd,
		}

		protected virtual void Awake()
		{
		}

		protected virtual void OnBegin()
		{
		}

		protected virtual void OnEnd()
		{
		}
		private void OnDestroy()
		{
			State = EState.OnEnd;
			OnEnd();
		}

		protected virtual void Update()
		{
			if (State == EState.OnAwake || obj == null)
			{
				obj = new object();
				OnBegin();
				State = EState.OnBegin;
			}

		}

		protected virtual void OnGUI()
		{
			var e = Event.current;
			if (e.type == EventType.KeyDown)
			{
				OnHotkey(e.keyCode, e.control, e.alt, e.shift);
			}

			if (State == EState.OnAwake || obj == null)
			{
				obj = new object();
				OnBegin();
				State = EState.OnBegin;
			}
			GUILayout.BeginHorizontal("Toolbar", GUILayout.Width(Screen.width));
			OnDrawMenus();
			GUILayout.EndHorizontal();
			OnDrawContent();
		}

		protected abstract void OnDrawMenus();

		protected abstract void OnDrawContent();

		protected virtual void OnHotkey(KeyCode key, bool isCtrl, bool isAlt, bool isShift)
		{ }




	}
}
