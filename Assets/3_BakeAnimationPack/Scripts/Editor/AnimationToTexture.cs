using System.Diagnostics.Contracts;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class AnimationToTexture : EditorWindow
{
    [MenuItem("CONTEXT/SkinnedMeshRenderer/Bake animation")]
    private static void Open(MenuCommand command)
    {
        var window = GetWindow<AnimationToTexture>();
        window.context = (SkinnedMeshRenderer)command.context;
        window.ShowModalUtility();
    }

    private SkinnedMeshRenderer context;
    private AnimationClip clip;
    private static int frameRate = 24;

    private void CreateGUI()
    {
        var frameCountField = new IntegerField("Frame Rate")
        {
            value = frameRate
        };
        frameCountField.RegisterValueChangedCallback(OnFrameRateChanged);

        var clipField = new ObjectField("Clip")
        {
            objectType = typeof(AnimationClip),
            allowSceneObjects = false
        };
        clipField.RegisterValueChangedCallback(OnClipChanged);

        rootVisualElement.Add(frameCountField);
        rootVisualElement.Add(clipField);
        rootVisualElement.Add(new Button(CreateAnimationTexture) { text = "Record" });
    }

    private void OnFrameRateChanged(ChangeEvent<int> evt)
    {
        frameRate = Mathf.Clamp(evt.newValue, 1, 60);
    }

    private void OnClipChanged(ChangeEvent<Object> evt)
    {
        clip = evt.newValue as AnimationClip;
    }

    private void CreateAnimationTexture()
    {
        Close();

        Contract.Assert(clip != null, "Animation clip not defined");

        var duration = clip.length;
        var frameCount = Mathf.Max((int)(duration * frameRate), 1);
        var vertexCount = context.sharedMesh.vertexCount;

        var texture = new Texture2D(
            frameCount,
            vertexCount * 2,
            TextureFormat.RGBAHalf,
            false,
            false
        );
        texture.wrapMode = TextureWrapMode.Clamp;

        var targetGameObject = context.GetComponentInParent<Animation>().gameObject;
        BakeAnimation(targetGameObject, frameCount, duration, texture);
        CreateTextureAsset(texture);
    }

    private void BakeAnimation(GameObject targetGameObject, int frameCount, float duration, Texture2D texture)
    {
        var mesh = new Mesh();

        var lastFrameIndex = frameCount - 1;
        for (int frameIndex = 0; frameIndex < frameCount; frameIndex++)
        {
            clip.SampleAnimation(targetGameObject, (float)frameIndex / lastFrameIndex * duration);
            context.BakeMesh(mesh);

            var vertices = mesh.vertices;
            var normals = mesh.normals;

            for (int i = 0; i < vertices.Length; i++)
            {
                var position = vertices[i];
                var normal = normals[i];
                var positionColor = new Color(position.x, position.y, position.z);
                var normalColor = new Color(normal.x, normal.y, normal.z);
                texture.SetPixel(frameIndex, i * 2, positionColor);
                texture.SetPixel(frameIndex, i * 2 + 1, normalColor);
            }
        }
    }

    private void CreateTextureAsset(Texture2D texture)
    {
        var path = EditorUtility.SaveFilePanelInProject("Save animation texture", "Animation", "asset",
            "Select animation asset path");
        if (string.IsNullOrEmpty(path))
        {
            DestroyImmediate(texture);
            return;
        }

        AssetDatabase.CreateAsset(texture, path);
        AssetDatabase.Refresh();
    }
}