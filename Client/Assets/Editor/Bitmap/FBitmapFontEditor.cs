using FFramework.UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using Font = UnityEngine.Font;

public class FBitmapFontEditor : FEditorWindow
{
	private Texture2D mMainTex;
	private CharacterInfo[] mChtInfos;

	[MenuItem("Font/位图字体生成工具[Bitmap]", priority = 300)]
	public static void Open()
	{
		GetWindow<FBitmapFontEditor>();
	}

	protected override void OnDrawMenus()
	{
		if (GUILayout.Button("Choose Images Folder (c)", "ToolbarButton"))
		{
			OnClickOpenImages();
		}

		if (GUILayout.Button("Export (e)", "ToolbarButton"))
		{
			OnClickExport();
		}

	}

	protected override void OnHotkey(KeyCode key, bool isCtrl, bool isAlt, bool isShift)
	{
		base.OnHotkey(key, isCtrl, isAlt, isShift);
		switch (key)
		{
			case KeyCode.E:
				OnClickExport();
				Repaint();
				break;
			case KeyCode.C:
				OnClickOpenImages();
				Repaint();
				break;
		}
	}

	protected override void OnDrawContent()
	{
		if (mMainTex != null && mChtInfos != null)
		{
			GUILayout.Box(mMainTex, GUILayout.Width(Screen.width), GUILayout.Height(Screen.height));
		}
	}


	private void OnClickOpenImages()
	{
		mMainTex = null;
		string dir = EditorUtility.OpenFolderPanel("", Application.dataPath.Replace("/Unity/Assets",""), "");
		if (string.IsNullOrEmpty(dir))
		{
			return;
		}
		var files = Directory.GetFiles(dir, "*", SearchOption.AllDirectories);
		List<Texture2D> tiles = new List<Texture2D>();
		foreach (var item in files)
		{
			if (!(item.EndsWith(".png") || item.EndsWith(".jpg") || item.EndsWith(".tga")))
			{
				continue;
			}
            Texture2D texture = new Texture2D(64, 64);
		    texture.LoadImage(File.ReadAllBytes(item));
            texture.name = Path.GetFileNameWithoutExtension(item);
            tiles.Add(texture);
		}
		mMainTex = new Texture2D(64, 64, TextureFormat.ARGB32, false, false)
		{
			name = Path.GetFileName(dir)
		};
		var rects = mMainTex.PackTextures(tiles.ToArray(), 1, 1024);
		int texW = mMainTex.width;
		int texH = mMainTex.height;

		mChtInfos = new CharacterInfo[rects.Length];
		for (int i = 0; i < rects.Length; i++)
		{
			Rect r = rects[i];
		    mChtInfos[i] = new CharacterInfo();
            mChtInfos[i].glyphHeight = texH;
		    mChtInfos[i].glyphWidth = texW;
		    mChtInfos[i].index = Encoding.ASCII.GetBytes(tiles[i].name)[0];
		    mChtInfos[i].uvTopLeft = r.position;
		    mChtInfos[i].uvTopRight = new Vector2(r.x + r.width, r.y);
		    mChtInfos[i].uvBottomLeft = new Vector2(r.x, r.y + r.height);
		    mChtInfos[i].uvBottomRight = new Vector2(r.x + r.width, r.y + r.height);
			mChtInfos[i].minX = 0;
			mChtInfos[i].minY = (int)(r.height * texH * 0.5f);
			mChtInfos[i].maxX = (int)(r.width * texW);
			mChtInfos[i].maxY = (int)(r.height * texH * -0.5f); ;
			mChtInfos[i].advance = mChtInfos[i].maxX;
		}

	}


	private void OnClickExport()
	{
		if (mMainTex == null)
		{
			return;
		}
		string fontPath = EditorUtility.SaveFilePanelInProject("", mMainTex.name, "fontsettings", "save font files");
		if (string.IsNullOrEmpty(fontPath))
		{
			return;
		}

		string name = Path.GetFileNameWithoutExtension(fontPath);

		var texPath = fontPath.Replace("fontsettings", "png");
		File.WriteAllBytes(texPath, mMainTex.EncodeToPNG());
		AssetDatabase.Refresh();
		if (!File.Exists(texPath))
		{
			var texSettings = (TextureImporter)AssetImporter.GetAtPath(texPath);
			texSettings.mipmapEnabled = false;
			texSettings.alphaIsTransparency = true;
			texSettings.wrapMode = TextureWrapMode.Clamp;
		}

		var matPath = fontPath.Replace("fontsettings", "mat");
		if (!File.Exists(matPath))
		{
			var mat = new Material(Shader.Find("GUI/Text Shader"))
			{
				mainTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(texPath)
			};
			AssetDatabase.CreateAsset(mat, matPath);
		}

        Font font;
        if (!File.Exists(fontPath))
        {
            font = new Font(name)
            {
                material = AssetDatabase.LoadAssetAtPath<Material>(matPath),
            };
            AssetDatabase.CreateAsset(font, fontPath);
        }
        font = AssetDatabase.LoadAssetAtPath<Font>(fontPath);
        font.characterInfo = mChtInfos;
        EditorUtility.SetDirty(font);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }




}
