using UnityEngine;

public class ObjectClickChecker : MonoBehaviour
{
    public GameObject winPopup;
    public GameObject losePopup;

    private WordDictionaryManager clueManager;

    void Start()
    {
        clueManager = FindObjectOfType<WordDictionaryManager>();
        if (clueManager == null)
        {
            Debug.LogError("WordDictionaryManager not found in scene!");
        }
    }

    void OnMouseDown()
    {
        //Prevent clicking before solving the clue
        if (!LetterRack.clueSolved)
        {
            Debug.Log("❌ You must solve the clue first before clicking objects.");
            return;
        }

        if (clueManager == null || string.IsNullOrEmpty(clueManager.targetObjectName))
        {
            Debug.LogWarning("ClueManager or target object not set.");
            return;
        }

        string clickedName = gameObject.name.Trim().ToLower();
        string targetName = clueManager.targetObjectName.Trim().ToLower();

        Debug.Log($"Clicked Object: {clickedName}, Target Object: {targetName}");

        if (clickedName == targetName)
        {
            Debug.Log("✅ Correct object clicked! You win.");
            if (winPopup != null)
            {
                winPopup.SetActive(true);
                Time.timeScale = 0f;  //Freeze game
            }
        }
        else
        {
            Debug.Log("❌ Wrong object clicked. Game Over.");
            if (losePopup != null)
            {
                losePopup.SetActive(true);
                Time.timeScale = 0f;  //Freeze game
            }
        }
    }
}
