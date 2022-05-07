using UnityEngine;

public class AudioController : MonoBehaviour
{
    #region Singleton

    private static AudioController _audioControllerInstance;

    public static AudioController Instance
    {
        get
        {
            if (_audioControllerInstance == null) _audioControllerInstance = FindObjectOfType<AudioController>();
            return _audioControllerInstance;
        }
    }

    #endregion

    private AudioEffect[] _audioEffectPrefabs;
    private AudioEffect[] _audioEffects;

    #region Unity Event

    private void Awake()
    {
        _audioEffectPrefabs = Resources.LoadAll<AudioEffect>("Audio");
        _audioEffects = new AudioEffect[_audioEffectPrefabs.Length];

        for (var i = 0; i < _audioEffectPrefabs.Length; i++)
        {
            _audioEffects[i] = Instantiate(_audioEffectPrefabs[i], Vector2.zero, Quaternion.identity);
            _audioEffects[i].transform.parent = transform;
        }
    }

    #endregion

    public void Play(AudioVariant variant)
    {
        if (PlayerPrefs.GetInt("Effects", 0) == 1) return;

        foreach (var effect in _audioEffects)
        {
            if (effect.variant != variant) continue;
            
            effect.Play();
            return;
        }
    }

    public void Stop(AudioVariant variant)
    {
        if (PlayerPrefs.GetInt("Effects", 0) == 1) return;
        
        foreach (var effect in _audioEffects)
        {
            if (effect.variant != variant) continue;
            
            effect.Stop();
            return;
        }
    }
}
