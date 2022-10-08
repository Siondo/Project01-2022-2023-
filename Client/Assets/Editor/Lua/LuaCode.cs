using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class LuaCode
{
    //lua中要使用到C#库的配置，比如C#标准库，或者Unity API，第三方库等。
    [XLua.LuaCallCSharp]
    public static List<Type> LuaCallCSharp = new List<Type>()
    {
        typeof(WWWForm),
        typeof(SystemInfo),
        typeof(UnityEngine.Networking.UnityWebRequest),
        typeof(AudioSource),
        typeof(UnityEngine.UI.ScrollRect.MovementType),
        typeof(Vector2),
        typeof(Vector3),
        typeof(UnityEngine.EventSystems.EventTrigger),
        typeof(UnityEngine.EventSystems.EventTriggerType),

        typeof(Spine.AnimationState),
        typeof(DG.Tweening.ShortcutExtensions),
        typeof(DG.Tweening.DOTween),
        typeof(DG.Tweening.Tweener),
        typeof(DG.Tweening.Tween),
        typeof(DG.Tweening.Tweener),
        typeof(DG.Tweening.Sequence),
        typeof(DG.Tweening.TweenSettingsExtensions),
        typeof(DG.Tweening.DOTweenModuleUI),
        typeof(DG.Tweening.TweenExtensions),
        typeof(Framework.Debugger),
        typeof(Framework.IO.Manifest),
        typeof(Framework.IO.ManifestConfig),
        typeof(Framework.IO.ManifestMappingConfig),
        typeof(Framework.UnityAsset.AsyncAsset),
        typeof(Framework.UI.UISpineSortingOrder),
        typeof(App),
        typeof(SDKManager),
        typeof(LuaHelper),
        typeof(Framework.Util),
        typeof(Framework.PathUtil),
        typeof(Action<bool, Framework.UnityAsset.AsyncAsset>),
        typeof(Action<bool, float>),
        typeof(Action<float>),
        typeof(Action<int, GameObject>),
        typeof(GameTween),
        typeof(ScrollPool),
        typeof(ScrollPoolGrid),
        typeof(ScrollPoolHorizontal),
        typeof(ScrollPoolVertical),
        typeof(UnityEngine.UI.LayoutElement),
        typeof(UnityEngine.UI.Outline),
        typeof(Rigidbody),
        typeof(Screen),
        typeof(UnityEngine.UI.Shadow),
        typeof(SkeletonGraphic),
        typeof(SkeletonAnimation),
    };

    //C#静态调用Lua的配置（包括事件的原型），仅可以配delegate，interface
    [XLua.CSharpCallLua]
    public static List<Type> CSharpCallLua = new List<Type>()
    {
        typeof(Action),
        typeof(Action<float>),
        typeof(Action<bool, Framework.UnityAsset.AsyncAsset>),
        typeof(Action<GameObject, int>),
        typeof(Action<GameObject>),
        typeof(Action<int, GameObject>),
        typeof(Action<int, string>),
    };
}
