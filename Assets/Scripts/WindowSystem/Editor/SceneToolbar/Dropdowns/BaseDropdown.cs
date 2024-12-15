using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

  public abstract class BaseDropdown<T> : EditorToolbarDropdown
    {
        protected abstract string Tooltip { get; }
        protected abstract string PrefabPostfix { get; }
        protected abstract string AssetType { get; }
        protected abstract Color ActiveColor { get; }
        
        protected void Init()
        {
            SetView();
            
            clicked += ToggleDropdown;
            
            RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
        }

        protected virtual void OnAttachToPanel(AttachToPanelEvent evt)
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            PrefabStage.prefabStageOpened += OnPrefabStageOpened;
            EditorApplication.projectChanged += OnProjectChanged;
            EditorSceneManager.sceneOpened += OnSceneOpened;
        }

        protected virtual void OnDetachFromPanel(DetachFromPanelEvent evt)
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            PrefabStage.prefabStageOpened -= OnPrefabStageOpened;
            EditorApplication.projectChanged -= OnProjectChanged;
            EditorSceneManager.sceneOpened -= OnSceneOpened;
        }

        protected virtual void SetView() { }

        protected virtual void OnPrefabStageOpened(PrefabStage obj) { }

        protected virtual void OnSceneOpened(Scene scene, OpenSceneMode mode) { }

        protected virtual void OnProjectChanged() { }

        private void OnPlayModeStateChanged(PlayModeStateChange stateChange)
        {
            switch (stateChange)
            {
                case PlayModeStateChange.EnteredEditMode:
                    SetEnabled(true);
                    break;
                
                case PlayModeStateChange.EnteredPlayMode:
                    SetEnabled(false);
                    break;
            }
        }
        
        private void ToggleDropdown()
        {
            var menu = new GenericMenu();
            var paths = LoadAssetsPaths();
            
            foreach (var screenPath in paths)
            {
                var screenName = Path.GetFileNameWithoutExtension(screenPath);

                menu.AddItem
                (
                    new GUIContent(screenName), 
                    text == screenName,
                    () => OnDropdownItemSelected(screenName, screenPath)
                );
            }

            menu.DropDown(worldBound);
        }
        
        private List<string> LoadAssetsPaths()
        {
            var paths = new List<string>();
            var guids = AssetDatabase.FindAssets($"{PrefabPostfix} t:{AssetType} ", new[] {"Assets"});
      
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath(path, typeof(T));

                if (asset is T)
                    paths.Add(path);
            }
            
            return paths;
        }
        
        protected virtual void OnDropdownItemSelected(string itemName, string path)
        {
            text = itemName;
            PrefabStageUtility.OpenPrefab(path);
        }
    }