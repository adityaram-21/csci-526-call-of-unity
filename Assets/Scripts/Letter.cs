using UnityEngine;

public class Letter : MonoBehaviour
{
    public char letterValue;

    public TextMesh letterText; // Assign in prefab (UI text or sprite text)

    public void SetLetter(char c)
    {
        letterValue = c;
        letterText.text = c.ToString();
    }
}