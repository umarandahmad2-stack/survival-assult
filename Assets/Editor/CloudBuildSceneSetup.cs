using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class CloudBuildSceneSetup
{
    const string ScenePath = "Assets/Scenes/CloudBuildScene.unity";
    static CloudBuildSceneSetup()
    {
        EditorApplication.delayCall += EnsureScene;
    }

    static void EnsureScene()
    {
        if (EditorApplication.isCompiling || EditorApplication.isUpdating) return;
        if (!Directory.Exists("Assets/Scenes")) Directory.CreateDirectory("Assets/Scenes");
        if (!File.Exists(ScenePath))
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            var root = new GameObject("Game Bootstrap");
            root.AddComponent<CloudBuildBootstrap>();
            EditorSceneManager.SaveScene(scene, ScenePath);
        }

        var scenes = new System.Collections.Generic.List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        if (!scenes.Exists(s => s.path == ScenePath))
        {
            scenes.Add(new EditorBuildSettingsScene(ScenePath, true));
            EditorBuildSettings.scenes = scenes.ToArray();
        }
    }
}