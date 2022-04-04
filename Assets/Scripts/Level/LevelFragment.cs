using UnityEngine;

public class LevelFragment : MonoBehaviour
{
    [SerializeField] private SpriteRenderer referenceBackground;

    #region Unity Event

    private void Start()
    {
        referenceBackground.gameObject.SetActive(false);
    }

    #endregion
}