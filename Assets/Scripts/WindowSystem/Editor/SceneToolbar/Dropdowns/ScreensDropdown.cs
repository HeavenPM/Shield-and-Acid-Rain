using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;
using WindowSystem.Runtime.Scripts;

[EditorToolbarElement(ID, typeof(SceneView))]
public class ScreensDropdown : PrefabDropdown<Window>
{
    public const string ID = WindowsToolbar.OVERLAY_ID + "/screens-dropdown";
        
    protected override string Tooltip => "Select screen";
    protected override string PrefabPostfix => "Screen";
    protected override string AssetType => "prefab";
    protected override Color ActiveColor => new (12f / 255f, 105f / 255f, 0f, 1f);
}