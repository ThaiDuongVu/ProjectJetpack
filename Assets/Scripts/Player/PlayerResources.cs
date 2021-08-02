using UnityEngine;
using TMPro;

public class PlayerResources : MonoBehaviour
{
    public TMP_Text tokenText;
    private int currentTokens;
    public int Tokens
    {
        get => currentTokens;
        set
        {
            currentTokens = value;
            tokenText.text = value.ToString();
        }
    }

    private float currentHealth;
    public float Health
    {
        get => currentHealth;
        set
        {
            currentHealth = value;
        }
    }
}
