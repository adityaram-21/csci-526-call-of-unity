using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ClueMapping
{
    public string objectName;
    public string objectTagName;
    public List<string> cluesWordList;
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
    public TextAsset clueJsonFile;

    [HideInInspector]
    public ClueMappings clueMappings;

    public string targetObjectName { get; private set; }
    public string targetObjectTagName { get; private set; }
    public string targetClueWord { get; private set; }

    void Awake()
    {
        // Load the clue mappings from the JSON file
        clueMappings = JsonUtility.FromJson<ClueMappings>(clueJsonFile.text);
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
        targetObjectTagName = selectedObjectMapping.objectTagName;

        if (selectedObjectMapping.cluesWordList.Count > 0)
        {
            int randomClueIndex = Random.Range(0, selectedObjectMapping.cluesWordList.Count);
            targetClueWord = selectedObjectMapping.cluesWordList[randomClueIndex].ToUpper();
        }
        else
        {
            targetClueWord = null;
            Debug.LogError($"Object {selectedObjectMapping.objectName} has no clue words! Check your JSON.");
        }
    }

    public bool IsClueWordValid(string playerWord, out string targetTag, out string targetName)
    {
        targetTag = this.targetObjectTagName;
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
