using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LetterRack : MonoBehaviour
{
    [SerializeField] private RectTransform slotTemplate;

    private List<string> wordList = new List<string>
    {
        "KEY", "LOCK", "PUZZLE"
    };

    private List<Slot> slots = new();

    private void Awake()
    {
        int longestWordLength = GetLongestWordLength();

        for (int i = 0; i < longestWordLength; i++)
        {
            var slot = Instantiate(slotTemplate, transform);
            slot.gameObject.SetActive(true);

            var text = slot.GetComponentInChildren<TextMeshProUGUI>();
            slots.Add(new Slot(text));

            int slotIndex = i;
            var button = slot.gameObject.AddComponent<UnityEngine.UI.Button>();
            button.onClick.AddListener(() => PopLetter(slotIndex));

            var drag = slot.gameObject.AddComponent<DraggableSlot>();
            drag.slotIndex = slotIndex;
            drag.rack = this;
        }
    }

    private int GetLongestWordLength()
    {
        int max = 0;
        foreach (var word in wordList)
            if (word.Length > max)
                max = word.Length;
        return max;
    }

    public void SetLetter(int i, char c, Letter source = null) => slots[i].Set(c, source);

    public bool AddCollectedLetter(char c, Letter source)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].Letter == '\0')
            {
                slots[i].Set(c, source);
                return true;
            }
        }
        return false; // rack is full
    }

    public void PopLetter(int i)
    {
        if (i < 0 || i >= slots.Count) return;

        // return to world
        slots[i].ReturnSource();

        // shift all right letters left
        for (int j = i; j < slots.Count - 1; j++)
        {
            slots[j].CopyFrom(slots[j + 1]);
        }

        // clear last
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
