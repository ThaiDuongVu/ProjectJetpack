using UnityEngine;

public class AudioEffect : MonoBehaviour
{
    public AudioVariant variant;
    private AudioSource _audioSource;

    #region Unity Event

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    #endregion

    public void Play()
    {
        _audioSource.Play();
    }

    public void Stop()
    {
        _audioSource.Stop();
    }
}
