using System;
using TMPro;
using UnityEngine;

namespace Extensions
{
    public static class TMProExtensions
    {
        public static void ResizeToFitText(this TextMeshProUGUI textComponent)
        {
            var rectTransform = textComponent.GetComponent<RectTransform>();

            if (rectTransform == null) throw new Exception("RectTransform not found!");
        
            textComponent.ForceMeshUpdate();
            var preferredSize = textComponent.GetPreferredValues();

            rectTransform.sizeDelta = preferredSize;
        }
    }
}