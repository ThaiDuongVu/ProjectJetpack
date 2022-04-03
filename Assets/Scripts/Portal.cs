using UnityEngine;

public class Portal : MonoBehaviour
{
    public bool IsOpen { get; set; }
    private Animator _animator;
    private static readonly int OpenAnimationTrigger = Animator.StringToHash("open");

    #region Unity Event

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    #endregion

    public void Open()
    {
        IsOpen = true;
        _animator.SetTrigger(OpenAnimationTrigger);
    }
}
