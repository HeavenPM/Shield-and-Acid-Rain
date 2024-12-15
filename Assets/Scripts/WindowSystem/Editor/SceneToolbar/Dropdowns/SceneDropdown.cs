using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.SceneManagement;

[EditorToolbarElement(ID, typeof(SceneView))]
public class SceneDropdown : PrefabDropdown<SceneAsset>
{
    public const string ID = WindowsToolbar.OVERLAY_ID + "/scene-dropdown";
        
    protected override string Tooltip => "Switch scene.";
    protected override string PrefabPostfix => "Scene";
    protected override string AssetType => "Scene";
    protected override Color ActiveColor => new (100f / 255f, 0f, 140f / 255f);
        
    public SceneDropdown()
         => Init();
    
    protected override void SetView()
    {
        var content =
            EditorGUIUtility.TrTextContentWithIcon
            (
                SceneManager.GetActiveScene().name, 
                Tooltip,
                "d_SceneAsset Icon"
            );
            
        text = content.text;
        tooltip = content.tooltip;
        icon = content.image as Texture2D;
        style.backgroundColor = ActiveColor;
    }

    protected override void OnPrefabStageOpened(PrefabStage obj) { }

    protected override void OnProjectChanged()
    {
        text = SceneManager.GetActiveScene().name;
    }

    protected override void OnSceneOpened(Scene scene, OpenSceneMode mode)
    {
        text = scene.name;
    }
        
    protected override void OnDropdownItemSelected(string itemName, string path)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
        }
    }
}