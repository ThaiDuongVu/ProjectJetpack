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

    private const float MusicDefaultVolume = 0.2f;

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
        PlayMusic();
        StartCoroutine(CheckMusicVolume());
    }

    #endregion

    private void PlayMusic()
    {
        if (_currentAmbientMusic) _currentAmbientMusic.Stop();

        _currentAmbientMusic = _ambientMusicSources[Random.Range(0, _ambientMusicSources.Length)];
        _currentAmbientMusic.Play();

        Invoke(nameof(PlayMusic), _currentAmbientMusic.AudioSource.clip.length);
    }

    public void StopAmbient()
    {
        if (_currentAmbientMusic) _currentAmbientMusic.Stop();
        CancelInvoke();
    }

    private IEnumerator CheckMusicVolume()
    {
        if (_currentAmbientMusic) _currentAmbientMusic.AudioSource.volume = PlayerPrefs.GetInt("Music", 0) == 0 ? MusicDefaultVolume : 0f;
        yield return new WaitForSeconds(1f);
        StartCoroutine(CheckMusicVolume());
    }
}
