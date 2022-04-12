using UnityEngine;

public class LevelController : MonoBehaviour
{
    public void EnableKey(Key key)
    {
        key.gameObject.SetActive(true);
    }
}
