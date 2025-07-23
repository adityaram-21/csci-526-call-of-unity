using System.Collections.Generic;
using Mono.Cecil.Cil;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ClueMapping
{
    public string objectName;
    //public string objectTagName;
    public List<string> clueWords;
}

[System.Serializable]
public class ClueMappings
{
    public List<ClueMapping> clueMappingsList;

    public ClueMappings()
    {
        clueMappingsList = new List<ClueMapping>();
    }
}

public class WordDictionaryManager : MonoBehaviour
{
    [HideInInspector]
    public ClueMappings clueMappings;
    public CipherManager cipherManager;
    public TMP_Text clueText;

    public string targetObjectName { get; private set; }
    public string targetObjectTagName { get; private set; }
    public string targetClueWord { get; private set; }
    public string cipherWord { get; private set; }

    void Awake()
    {
        TextAsset clueJsonFile = Resources.Load<TextAsset>("Clues/clues");
        // Load the clue mappings from the JSON file
        if (clueJsonFile == null)
        {
            Debug.LogError("clues.json not found in Resources/Clues folder! Check your path.");
            return;
        }
        clueMappings = JsonUtility.FromJson<ClueMappings>(clueJsonFile.text);

        cipherManager = GetComponent<CipherManager>();
    }

    public void SelectRandomClue()
    {
        if (clueMappings.clueMappingsList.Count == 0)
        {
            Debug.LogWarning("No clue mappings available.");
            return;
        }

        // Select a random clue mapping
        int randomIndex = Random.Range(0, clueMappings.clueMappingsList.Count);
        ClueMapping selectedObjectMapping = clueMappings.clueMappingsList[randomIndex];

        // Set the target object name, tag name, and clue word
        targetObjectName = selectedObjectMapping.objectName;
        //targetObjectTagName = selectedObjectMapping.objectTagName;

        if (selectedObjectMapping.clueWords.Count > 0)
        {
            int randomClueIndex = Random.Range(0, selectedObjectMapping.clueWords.Count);
            targetClueWord = selectedObjectMapping.clueWords[randomClueIndex].ToUpper();
            cipherWord = cipherManager.EncodeWord(targetClueWord);
        }
        else
        {
            targetClueWord = null;
            cipherWord = null;
            Debug.LogError($"Object {selectedObjectMapping.objectName} has no clue words! Check your JSON.");
        }

        clueText.text = $"Code Word: {cipherWord ?? "No clue available"}";

        Debug.Log($"Selected clue: {targetClueWord} for object: {targetObjectName}");
        Debug.Log($"Ciphered clue: {cipherWord}");
    }

    public bool ValidateClueWord(string playerWord,out string targetName) //out string targetTag, )
    {
        //targetTag = this.targetObjectTagName;
        targetName = this.targetObjectName;

        if (string.IsNullOrEmpty(targetClueWord))
        {
            Debug.LogWarning("No clue word set! Did you forget to call SelectRandomClue?");
            return false;
        }

        // Check if the player's word matches the target clue word
        return playerWord.ToUpper() == targetClueWord;
    }
}
