using UnityEngine;
using Utils;
using Zenject;

namespace AudioSlider
{
    public class AudioPlayer : MonoBehaviour, IAudioPlayer
    {
        [Inject] private ResourcesLoader _resources;
    
        private const string SoundVolumeKey = "SoundVolumeKey";
        private const string MusicVolumeKey= "MusicVolumeKey";

        private AudioSource _musicSource;

        public float SoundVolume { get; private set; }
        public float MusicVolume { get; private set; }
    
        private void Awake()
        {
            SoundVolume = PlayerPrefs.GetFloat(SoundVolumeKey, 1f);
            MusicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
        }

        private void Start()
            => Play("audio_lobby", true);

        public void Play(string id, bool onRepeat = false)
        {
            var clip = _resources.Get<AudioClip>(id);
        
            var volume = onRepeat ? MusicVolume : SoundVolume;

            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.loop = onRepeat;
            audioSource.Play();

            if (!onRepeat) Destroy(audioSource, clip.length);
            else _musicSource = audioSource;
        }

        public void SetSoundVolume(float volume)
        {
            SoundVolume = Mathf.Clamp01(volume);
            PlayerPrefs.SetFloat(SoundVolumeKey, SoundVolume);

            foreach (var audioSource in GetComponents<AudioSource>())
            {
                if (audioSource != _musicSource)
                    audioSource.volume = SoundVolume;
            }
        }

        public void SetMusicVolume(float volume)
        {
            MusicVolume = Mathf.Clamp01(volume);
            PlayerPrefs.SetFloat(MusicVolumeKey, MusicVolume);

            _musicSource.volume = MusicVolume;
        }

        public void Toggle(string type)
        {
            if (type == "Music")
            {
                SetMusicVolume(MusicVolume > 0f ? 0f : 1f);
                return;
            }
        
            SetSoundVolume(SoundVolume > 0f ? 0f : 1f);
        }

    }
}