using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;

[Icon("d_SceneAsset Icon")]
[Overlay(typeof(SceneView), OVERLAY_ID, "ExWindows Toolbar", 
    defaultDisplay = true, defaultDockZone = DockZone.BottomToolbar, 
    defaultDockPosition = DockPosition.Top)]
public class WindowsToolbar : ToolbarOverlay
{
    public const string OVERLAY_ID = "screens-switcher-overlay";

    private WindowsToolbar() : base
    (
        SceneDropdown.ID, 
        ScreensDropdown.ID, 
        PopupsDropdown.ID
    ) { }
}