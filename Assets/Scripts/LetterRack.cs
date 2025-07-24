using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LetterRack : MonoBehaviour
{
    [SerializeField] private RectTransform slotTemplate;
    private List<Slot> slots = new();

    public WordDictionaryManager wordManager;
    private string clueWord;

    private void Start()
    {
        if (wordManager == null)
        {
            wordManager = FindObjectOfType<WordDictionaryManager>();
            if (wordManager == null)
            {
                Debug.LogError("WordDictionaryManager not found in scene!");
                return;
            }
        }

        clueWord = wordManager.targetClueWord;

        if (string.IsNullOrEmpty(clueWord))
        {
            Debug.LogError("Clue word is null. Make sure WordDictionaryManager.SelectRandomClue() was called.");
            return;
        }

        int wordLength = clueWord.Length;

        for (int i = 0; i < wordLength; i++)
        {
            var slot = Instantiate(slotTemplate, transform);
            slot.gameObject.SetActive(true);

            var text = slot.GetComponentInChildren<TextMeshProUGUI>();
            slots.Add(new Slot(text));

            int slotIndex = i;
            var button = slot.gameObject.AddComponent<UnityEngine.UI.Button>();
            button.onClick.AddListener(() => {
                PopLetter(slotIndex);
                TryCheckConstructedWord();
            });

            var drag = slot.gameObject.AddComponent<DraggableSlot>();
            drag.slotIndex = slotIndex;
            drag.rack = this;
        }
    }

    public void SetLetter(int i, char c, Letter source = null)
    {
        slots[i].Set(c, source);
        TryCheckConstructedWord();
    }

    public bool AddCollectedLetter(char c, Letter source)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].Letter == '\0')
            {
                slots[i].Set(c, source);
                TryCheckConstructedWord();
                return true;
            }
        }
        return false;
    }

    public void PopLetter(int i)
    {
        if (i < 0 || i >= slots.Count) return;

        slots[i].ReturnSource();

        for (int j = i; j < slots.Count - 1; j++)
        {
            slots[j].CopyFrom(slots[j + 1]);
        }

        slots[slots.Count - 1].Clear();
    }

    public void Swap(int i, int j)
    {
        if (i >= 0 && j >= 0 && i < slots.Count && j < slots.Count)
        {
            var tempLetter = slots[i].Letter;
            var tempSource = slots[i].Source;

            slots[i].Set(slots[j].Letter, slots[j].Source);
            slots[j].Set(tempLetter, tempSource);

            TryCheckConstructedWord();
        }
    }

    private string GetConstructedWord()
    {
        foreach (var slot in slots)
        {
            if (slot.Letter == '\0') return null;
        }

        string constructedWord = "";
        foreach (var slot in slots)
        {
            constructedWord += slot.Letter;
        }
        return constructedWord;
    }

    private void TryCheckConstructedWord()
    {
        string constructed = GetConstructedWord();
        if (constructed != null)
        {
            if (wordManager.ValidateClueWord(constructed, out string targetName))
            {
                Debug.Log($"✅ Correct! Word: {constructed} matches the clue for {targetName}");
            }
            else
            {
                Debug.Log($"❌ Incorrect! Word: {constructed} does not match the clue.");
            }
        }
    }

    private class Slot
    {
        public char Letter { get; private set; }
        public Letter Source { get; private set; }
        private TextMeshProUGUI label;

        public Slot(TextMeshProUGUI l)
        {
            label = l;
            Clear();
        }

        public void Set(char c, Letter source = null)
        {
            Letter = c;
            Source = source;
            label.text = c == '\0' ? "" : c.ToString();
        }

        public void Clear()
        {
            Letter = '\0';
            Source = null;
            label.text = "";
        }

        public void CopyFrom(Slot other)
        {
            Set(other.Letter, other.Source);
        }

        public void ReturnSource()
        {
            if (Source != null)
                Source.ReturnToWorld();
        }
    }
}
