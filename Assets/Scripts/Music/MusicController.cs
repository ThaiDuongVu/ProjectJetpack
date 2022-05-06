using System.Collections;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    #region Singleton

    private static MusicController _musicControllerInstance;

    public static MusicController Instance
    {
        get
        {
            if (_musicControllerInstance == null) _musicControllerInstance = FindObjectOfType<MusicController>();
            return _musicControllerInstance;
        }
    }

    #endregion

    private AmbientMusic[] _ambientMusicPrefabs;
    private AmbientMusic[] _ambientMusicSources;
    private AmbientMusic _currentAmbientMusic;

    #region Unity Event

    private void Awake()
    {
        _ambientMusicPrefabs = Resources.LoadAll<AmbientMusic>("Music/Ambient");
        _ambientMusicSources = new AmbientMusic[_ambientMusicPrefabs.Length];

        for (var i = 0; i < _ambientMusicPrefabs.Length; i++)
        {
            _ambientMusicSources[i] = Instantiate(_ambientMusicPrefabs[i], Vector2.zero, Quaternion.identity);
            _ambientMusicSources[i].transform.parent = transform;
        }
    }

    private void Start()
    {
        PlayAmbient();
    }

    #endregion

    private void PlayAmbient()
    {
        if (PlayerPrefs.GetInt("Music", 0) == 1) return;
        _currentAmbientMusic?.Stop();

        _currentAmbientMusic = _ambientMusicSources[Random.Range(0, _ambientMusicSources.Length)];
        _currentAmbientMusic.Play();

        Invoke(nameof(PlayAmbient), _currentAmbientMusic.AudioSource.clip.length);
    }
}
