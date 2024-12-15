using AudioSlider;
using DG.Tweening;
using Gameplay.Character;
using UnityEngine;
using Zenject;

namespace Gameplay.Boosters
{
    public class BoosterPickup : MonoBehaviour
    {
        [SerializeField] private BoosterType _boosterType;
        
        [Inject] private BoostersService _boostersService;
        [Inject] private IAudioPlayer _audioPlayer;

        private bool _isRaised;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isRaised) return;
            if (!other.TryGetComponent(out CharacterFacade _)) return;

            _isRaised = true;
            _boostersService.PickUpBooster(_boosterType);
            _audioPlayer.Play("audio_pickup");

            var sequence = DOTween.Sequence();

            sequence.Append(transform.DOScale(1.5f * transform.localScale, .5f));
            sequence.Append(transform.DOScale(.1f * transform.localScale, .5f));
            sequence.OnComplete(() => Destroy(gameObject));
        }
    }
}