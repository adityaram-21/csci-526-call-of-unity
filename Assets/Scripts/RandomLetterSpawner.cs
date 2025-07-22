using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class RandomLetterSpawner : MonoBehaviour
{
    [Header("References")]
    public Grid grid;
    public Tilemap groundTilemap;
    public Tilemap wallTilemap;
    public Tilemap decorationTilemap;
    public WordDictionaryManager wordDictionaryManager;

    [Header("Spawning")]
    public GameObject letterPrefab;
    public Transform letterParent;
    public int extraBogusLetters = 3;

    private List<Vector3> validSpawnPoints = new List<Vector3>();
    private string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    void Awake()
    {
        if (wordDictionaryManager == null)
        {
            wordDictionaryManager = GetComponent<WordDictionaryManager>();
        }
    }

    void Start()
    {
        // Ensure references are assigned
        if (wordDictionaryManager == null || letterPrefab == null)
        {
            Debug.LogError("Missing references on GridLetterSpawner!");
            return;
        }

        GenerateValidSpawnPoints();

        wordDictionaryManager.SelectRandomClue();  // Pick a clue for this level

        SpawnLettersForClue();
    }

    void GenerateValidSpawnPoints()
    {
        validSpawnPoints.Clear();

        BoundsInt bounds = groundTilemap.cellBounds;

        for (int x = bounds.xMin; x <= bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y <= bounds.yMax; y++)
            {
                Vector3Int cell = new Vector3Int(x, y, 0);

                if (!groundTilemap.HasTile(cell))
                    continue;

                if (wallTilemap.HasTile(cell) || decorationTilemap.HasTile(cell))
                    continue;

                // Center of cell
                Vector3 worldPos = grid.GetCellCenterWorld(cell);
                validSpawnPoints.Add(worldPos);
            }
        }

        if (validSpawnPoints.Count == 0)
        {
            Debug.LogWarning("No valid spawn points found in grid!");
        }
    }

    void SpawnLettersForClue()
    {
        List<char> lettersToSpawn = new List<char>();

        string clue = wordDictionaryManager.targetClueWord;
        if (string.IsNullOrEmpty(clue))
        {
            Debug.LogWarning("No clue word selected in WordDictionaryManager!");
            return;
        }

        // Add the clue letters
        foreach (char c in clue)
        {
            lettersToSpawn.Add(c);
        }

        // Add bogus letters
        for (int i = 0; i < extraBogusLetters; i++)
        {
            char bogus = alphabet[Random.Range(0, alphabet.Length)];
            lettersToSpawn.Add(bogus);
        }

        // Shuffle the letters
        for (int i = 0; i < lettersToSpawn.Count; i++)
        {
            int randIndex = Random.Range(i, lettersToSpawn.Count);
            (lettersToSpawn[i], lettersToSpawn[randIndex]) = (lettersToSpawn[randIndex], lettersToSpawn[i]);
        }

        // Spawn letters at random valid points
        for (int i = 0; i < lettersToSpawn.Count; i++)
        {
            if (validSpawnPoints.Count == 0)
            {
                Debug.LogWarning("Ran out of spawn points for letters!");
                break;
            }

            int spawnIndex = Random.Range(0, validSpawnPoints.Count);
            Vector3 spawnPos = validSpawnPoints[spawnIndex];
            validSpawnPoints.RemoveAt(spawnIndex);  // Prevent reuse of same spot

            GameObject letterObj = Instantiate(letterPrefab, letterParent);
            letterObj.transform.position = spawnPos; // Set world position manually
            letterObj.GetComponent<Letter>().SetLetter(lettersToSpawn[i]);
        }
    }
}