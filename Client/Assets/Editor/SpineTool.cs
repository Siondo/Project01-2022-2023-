using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using Spine.Unity;
using Spine.Unity.Editor;
using UnityEditor.Animations;
using System.Collections;
using System.Collections.Generic;

namespace Framework
{
    public class SpineTool
    {
        /// <summary>
        /// 更新配置
        /// </summary>
        [MenuItem("Tools/Update/Spine")]
        private static void UpdateSpine()
        {
            string[] assetGUIDs = Selection.assetGUIDs;
            if (assetGUIDs.Length == 1)
            {
                string currentDirectory = Directory.GetCurrentDirectory();
                string path = AssetDatabase.GUIDToAssetPath(assetGUIDs[0]);
                path = currentDirectory + "/" + path;
                currentDirectory = PathUtil.GetPath(currentDirectory);
                string[] paths = Directory.GetFiles(path, "*_Atlas.asset", SearchOption.AllDirectories);
                for (int i = 0; i < paths.Length; ++i)
                {
                    path = PathUtil.GetPath(paths[i]);
                    path = path.Replace(currentDirectory + "/", "");
                    string skeletonPath = path.Replace("_Atlas.asset", "_SkeletonData.asset");
                    SkeletonDataAsset skeletonData = AssetDatabase.LoadAssetAtPath<SkeletonDataAsset>(skeletonPath);
                    if (skeletonData == null)
                    {
                        skeletonData = ScriptableObject.CreateInstance<SkeletonDataAsset>();

                        string jsonPath = path.Replace("_Atlas.asset", ".json.bytes");
                        skeletonData.skeletonJSON = AssetDatabase.LoadAssetAtPath<TextAsset>(jsonPath);
                        if (skeletonData.skeletonJSON == null)
                        {
                            jsonPath = path.Replace("_Atlas.asset", ".json");
                            skeletonData.skeletonJSON = AssetDatabase.LoadAssetAtPath<TextAsset>(jsonPath);
                        }

                        string atlasPath = path;
                        var spineAtlasAsset = AssetDatabase.LoadAssetAtPath<SpineAtlasAsset>(atlasPath);
                        skeletonData.atlasAssets = new SpineAtlasAsset[] { spineAtlasAsset };

                        AssetDatabase.CreateAsset(skeletonData, skeletonPath);
                        AssetDatabase.Refresh();
                    }
                    skeletonData.scale = 1;

                    string controllerPath = path.Replace("_Atlas.asset", "_Controller.controller");
                    AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(controllerPath);
                    if (controller == null)
                    {
                        SkeletonBaker.GenerateMecanimAnimationClips(skeletonData);
                        AssetDatabase.Refresh();
                        controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(controllerPath);
                    }
                    if (controller != null)
                    {
                        var clips = AssetDatabase.LoadAllAssetsAtPath(controllerPath);
                        var stateMachine = controller.layers[0].stateMachine;
                        var states = stateMachine.states;
                        foreach (var animatorState in states)
                        {
                            stateMachine.RemoveState(animatorState.state);
                        }

                        Dictionary<string, AnimationClip> animationClips = new Dictionary<string, AnimationClip>();
                        Dictionary<string, AnimatorState> animationStates = new Dictionary<string, AnimatorState>();
                        foreach (var value in clips)
                        {
                            AnimationClip clip = value as AnimationClip;
                            if (clip != null)
                            {
                                animationClips.Add(clip.name, clip);
                                AnimatorState animationState = controller.AddMotion(clip);
                                animationStates.Add(clip.name, animationState);
                            }
                        }

                        //attack hit idle siwang yidong
                        stateMachine.defaultState = animationStates["idle"];
                        var stateTransition = animationStates["attack"].AddTransition(animationStates["idle"]);
                        stateTransition.hasExitTime = true;
                        stateTransition = animationStates["hit"].AddTransition(animationStates["siwang"]);
                        stateTransition.hasExitTime = true;

                        Action<AnimationClip> func = (clip) => {
                            AnimationClipSettings clipSetting = AnimationUtility.GetAnimationClipSettings(clip);
                            clipSetting.loopTime = true;
                            AnimationUtility.SetAnimationClipSettings(clip, clipSetting);
                        };
                        func(animationClips["idle"]);
                        func(animationClips["yidong"]);

                        string modelName = path.Replace("_Atlas.asset", "");
                        modelName = Path.GetFileName(modelName);

                        string childName = string.Format("Spine Mecanim ({0})", modelName);
                        GameObject child = new GameObject(childName);
                        SkeletonMecanim skeletonMecanim = child.AddComponent<SkeletonMecanim>();
                        skeletonMecanim.skeletonDataAsset = skeletonData;
                        Animator animator = child.GetComponent<Animator>();
                        animator.runtimeAnimatorController = controller;

                        string[] p = path.Split('/');
                        string directory = string.Empty;
                        for (int pos = 0; pos < p.Length; ++pos)
                        {
                            if (p[pos] == "Model")
                            {
                                directory = p[pos + 1];
                                break;
                            }
                        }
                        directory = string.Format("Assets/Res/Model/Prefab/{0}/{1}.prefab", directory, modelName);
                        UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath<GameObject>(directory);
                        GameObject go = null;
                        if (null != obj)
                        {
                            go = GameObject.Instantiate(obj) as GameObject;
                        }
                        if (null == go)
                        {
                            go = new GameObject(modelName);
                            go.AddComponent<RectTransform>();
                        }
                        else
                        {
                            Transform childTf = go.transform.Find(childName);
                            if (null != childTf)
                            {
                                GameObject.DestroyImmediate(childTf.gameObject);
                            }
                        }
                        child.transform.SetParent(go.transform, true);

                        Transform attackNode = go.transform.Find("AttackNode");
                        if (null == attackNode)
                        {
                            GameObject node = new GameObject("AttackNode");
                            attackNode = node.AddComponent<RectTransform>();
                            attackNode.localPosition = new Vector3(0, 128, 0);
                            attackNode.SetParent(go.transform, true);
                        }

                        bool success = false;
                        PrefabUtility.SaveAsPrefabAsset(go, directory, out success);
                        if (success)
                        {
                            Debug.Log(string.Format("Make Prefab SUCCESSFUL! [{0}]\n{1}", directory, path));
                        }
                        else
                        {
                            Debug.LogError(string.Format("Make Prefab FAILED! [{0}]\n{1}", directory, path));
                        }
                        GameObject.DestroyImmediate(go);
                    }
                }
                AssetDatabase.Refresh();
            }
        }
    }
}
