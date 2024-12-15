using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace AudioSlider
{
    [RequireComponent(typeof(Image))]
    public class VolumeSetter : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private AudioType _audioType;

        [Inject] private IAudioPlayer _audioPlayer;

        private RectTransform _rectTransform;
        private Vector2 _startPosition;
        private float _currentVolume;

        private const float OffsetX = 145f;

        private void Awake()
            => _rectTransform = GetComponent<RectTransform>();

        private void OnEnable()
            => SetVisual();

        private void SetVisual()
        {
            _currentVolume = _audioType == AudioType.Music ? _audioPlayer.MusicVolume : _audioPlayer.SoundVolume;
            UpdateVisual(_currentVolume);
        }

        public void OnBeginDrag(PointerEventData eventData)
            => _startPosition = _rectTransform.anchoredPosition;

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 localMousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform.parent as RectTransform, eventData.position, eventData.pressEventCamera, out localMousePosition);
            var deltaX = localMousePosition.x - _startPosition.x;

            var newPositionX = Mathf.Clamp(_startPosition.x + deltaX, -OffsetX, OffsetX);
            _rectTransform.anchoredPosition = new Vector2(newPositionX, _rectTransform.anchoredPosition.y);

            var normalizedVolume = Mathf.InverseLerp(-OffsetX, OffsetX, newPositionX);
            UpdateVolume(normalizedVolume);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Vector2 localMousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform.parent as RectTransform, eventData.position, eventData.pressEventCamera, out localMousePosition);
            var deltaX = localMousePosition.x - _startPosition.x;

            var newPositionX = Mathf.Clamp(_startPosition.x + deltaX, -OffsetX, OffsetX);
            _rectTransform.anchoredPosition = new Vector2(newPositionX, _rectTransform.anchoredPosition.y);

            var normalizedVolume = Mathf.InverseLerp(-OffsetX, OffsetX, newPositionX);
            UpdateVolume(normalizedVolume);
        }

        private void UpdateVolume(float volume)
        {
            if (_audioType == AudioType.Music)
            {
                _audioPlayer.SetMusicVolume(volume);
                return;
            }
            _audioPlayer.SetSoundVolume(volume);
        }

        private void UpdateVisual(float volume)
        {
            var positionX = Mathf.Lerp(-OffsetX, OffsetX, volume);
            _rectTransform.anchoredPosition = new Vector2(positionX, _rectTransform.anchoredPosition.y);
        }
    }
}
