using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CipherManager : MonoBehaviour
{
    public Dictionary<char, int> letterToNumberMap;

    void Awake()
    {
        GenerateNewCipher();
    }

    void GenerateNewCipher()
    {
        letterToNumberMap = new Dictionary<char, int>();
        List<int> shuffleNumbers = Enumerable.Range(0, 26).OrderBy(x => Random.value).ToList();
        for (int i = 0; i < 26; i++)
        {
            char letter = (char)('A' + i);
            letterToNumberMap[letter] = shuffleNumbers[i];
        }
    }

    public string EncodeWord(string word)
    {
        word = word.ToUpper();
        List<string> encodedNumbers = new List<string>();
        foreach (char letter in word)
        {
            if (letterToNumberMap.TryGetValue(letter, out int value))
                encodedNumbers.Add(value.ToString());
        }
        return string.Join("-", encodedNumbers);
    }
}
