using AudioSlider;
using Extensions;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Utils
{
    [RequireComponent(typeof(Button))]
    public abstract class UIController : MonoBehaviour
    {
        [SerializeField] private bool _animated = true;
    
        [Inject] private IAudioPlayer _audioPlayer;
    
        private Button _button;
        private Image _image;

        public Image Image => _image;
        public Button Button => _button;

        public abstract void HandleListener();

        protected virtual void Awake()
            => Initialize();
    
        protected virtual void OnEnable()
            => _button.onClick.AddListener(OnClick);
    
        protected virtual void OnDisable()
            => _button.onClick.RemoveListener(OnClick);

        private void OnClick()
        {
            _audioPlayer.Play("audio_click");
            if (!_animated)
            {
                HandleListener();
                return;
            }
            _image.Enlarge(HandleListener, .25f, 1.25f);
        }

        private void Initialize()
        {
            _button = GetComponent<Button>();
            _image = GetComponent<Image>();
        
            _button.transition = Selectable.Transition.None;
        }
    }
}
