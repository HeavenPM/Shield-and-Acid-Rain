using Extensions;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(TextMeshProUGUI))]
public class CustomTMProEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var textComponent = (TextMeshProUGUI)target;

        if (GUILayout.Button("Resize to Fit Text"))
            ResizeToFitText(textComponent);
        
        if (GUILayout.Button("Set Text Parameters"))
            SetTextParameters(textComponent);
    }

    private void ResizeToFitText(TextMeshProUGUI textComponent)
    {
        var rectTransform = textComponent.GetComponent<RectTransform>();

        textComponent.ResizeToFitText();

        EditorUtility.SetDirty(textComponent);
        EditorUtility.SetDirty(rectTransform);
        EditorSceneManager.MarkSceneDirty(textComponent.gameObject.scene);
    }

    private void SetTextParameters(TextMeshProUGUI textComponent)
    {
        textComponent.enableAutoSizing = true;
        textComponent.fontSizeMin = 10f;
        textComponent.fontSizeMax = 150f;
        textComponent.alignment = TextAlignmentOptions.Center;
        textComponent.enableWordWrapping = false;

        EditorSceneManager.MarkSceneDirty(textComponent.gameObject.scene);
    }
}