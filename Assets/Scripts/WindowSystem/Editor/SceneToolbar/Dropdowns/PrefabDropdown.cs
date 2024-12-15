using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class PrefabDropdown<T> : BaseDropdown<T>
    {
        private readonly Color _inActiveColor = new (90f / 255f, 90f / 255f, 90f / 255f, 1f);

        protected PrefabDropdown()
            => Init();
        
        protected override void OnPrefabStageOpened(PrefabStage obj)
        {
            Debug.Log($"OnPrefabStageOpened {obj.prefabContentsRoot.name} {PrefabPostfix}");
            text = GetTitle(obj);
            SetStateColor(obj);
        }
        
        protected override void SetView()
        {
            var prefabOnStage = PrefabStageUtility.GetCurrentPrefabStage();
            
            var content =
                EditorGUIUtility.TrTextContentWithIcon(GetTitle(prefabOnStage), Tooltip,
                    "d_SceneAsset Icon");
            
            SetStateColor(prefabOnStage);
            
            text = content.text;
            tooltip = content.tooltip;
            icon = content.image as Texture2D;    
            
            ElementAt(1).style.paddingLeft = 5;
            ElementAt(1).style.paddingRight = 5;
        }

        protected override void OnSceneOpened(Scene scene, OpenSceneMode mode)
        {
            var prefabOnStage = PrefabStageUtility.GetCurrentPrefabStage();
            text = GetTitle(prefabOnStage);
            SetStateColor(prefabOnStage);
        }

        protected override void OnProjectChanged()
        {
            var prefabOnStage = PrefabStageUtility.GetCurrentPrefabStage();
            text = GetTitle(prefabOnStage);
            SetStateColor(prefabOnStage);
        }
        
        private string GetTitle(PrefabStage prefabOnStage)
        {
            return IsPrefab(prefabOnStage) ? prefabOnStage.prefabContentsRoot.name : $"Select {PrefabPostfix}";
        }
        
        private bool IsPrefab(PrefabStage prefabOnStage)
        {
            if(prefabOnStage == null) return false;
            
            var root = prefabOnStage.prefabContentsRoot;
            return root.name.EndsWith(PrefabPostfix);
        }
        
        private void SetStateColor(PrefabStage prefabOnStage)
        {
            style.backgroundColor = IsPrefab(prefabOnStage) ? ActiveColor : _inActiveColor;
        }
    }