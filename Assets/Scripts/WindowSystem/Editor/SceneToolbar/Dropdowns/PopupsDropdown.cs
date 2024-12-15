using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;
using WindowSystem.Runtime.Scripts;

[EditorToolbarElement(ID, typeof(SceneView))]
public class PopupsDropdown: PrefabDropdown<Window>
{
    public const string ID = WindowsToolbar.OVERLAY_ID + "/popups-dropdown";
        
    protected override string Tooltip => "Select popup";
    protected override string PrefabPostfix => "Popup";
    protected override string AssetType => "prefab";
    protected override Color ActiveColor => Color.blue;
}