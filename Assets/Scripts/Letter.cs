using UnityEngine;
using TMPro;

public class Letter : MonoBehaviour
{
    public char letterValue;

    public TMP_Text letterText; // Assign in prefab (UI text or sprite text)

    public void SetLetter(char c)
    {
        letterValue = c;
        letterText.text = c.ToString();
    }
}