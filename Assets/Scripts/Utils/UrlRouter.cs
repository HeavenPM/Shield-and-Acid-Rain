using System;
using UnityEngine;

namespace Utils
{
    public class UrlRouter : UIController
    {
        [SerializeField] private string _url;
    
        public override void HandleListener()
        {
            if (string.IsNullOrEmpty(_url)) throw new Exception("---- UrlRouter OnClick: URL is empty");
        
            Application.OpenURL(_url);
        }
    }
}
