using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Portal : MonoBehaviour
{
    public string[] ids;

    public bool IsOpen { get; set; }
    private Animator _animator;
    private static readonly int OpenAnimationTrigger = Animator.StringToHash("open");

    private InputPrompt _inputPrompt;

    public UnityEvent onEntered;

    #region Unity Event

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _inputPrompt = GetComponentInChildren<InputPrompt>();
    }

    private void Start()
    {
        _inputPrompt.gameObject.SetActive(false);
    }

    #endregion

    public void Unlock(List<string> keyIds)
    {
        foreach (var id in ids)
        {
            if (!keyIds.Contains(id))
            {
                GameController.Instance.SendUIMessage("Missing key(s) to unlock portal");
                return;
            }
        }

        Open();
    }

    public void Open()
    {
        IsOpen = true;
        _animator.SetTrigger(OpenAnimationTrigger);
        _inputPrompt.gameObject.SetActive(false);
        _inputPrompt.gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) _inputPrompt.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) _inputPrompt.gameObject.SetActive(false);
    }
}
