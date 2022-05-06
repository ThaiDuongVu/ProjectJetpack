using UnityEngine;

public class AmbientMusic : MonoBehaviour
{
    public AudioSource AudioSource { get; private set; }

    #region Unity Event

    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    #endregion

    public void Play()
    {
        AudioSource.Play();
    }

    public void Stop()
    {
        AudioSource.Stop();
    }
}
