namespace AudioSlider
{
    public interface IAudioPlayer
    {
        float SoundVolume { get; }

        float MusicVolume { get; }

        void Play(string id, bool onRepeat = false);

        void SetSoundVolume(float volume);

        void SetMusicVolume(float volume);

        void Toggle(string type);
    }
}