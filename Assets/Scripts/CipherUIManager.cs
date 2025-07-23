using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CipherUIManager : MonoBehaviour
{
    public CipherManager cipherManager;
    public string currentWord = "BOOK";

    public TextMeshProUGUI codewordText;
    public Button showMappingButton;
    public GameObject mappingPanel;

    public Transform letterRow;
    public Transform numberRow;
    public GameObject textElementPrefab;

    void Start()
    {
        string encoded = cipherManager.EncodeWord(currentWord);
        codewordText.text = "CODEWORD: " + encoded;

        mappingPanel.SetActive(false);
        showMappingButton.onClick.AddListener(ToggleMappingPanel);

        PopulateMappingTable();
    }

    void ToggleMappingPanel()
    {
        mappingPanel.SetActive(!mappingPanel.activeSelf);
    }

    void PopulateMappingTable()
    {
        foreach (Transform child in letterRow) Destroy(child.gameObject);
        foreach (Transform child in numberRow) Destroy(child.gameObject);

        foreach (var pair in cipherManager.letterToNumberMap.OrderBy(p => p.Key))
        {
            GameObject letter = Instantiate(textElementPrefab, letterRow);
            letter.GetComponent<TextMeshProUGUI>().text = pair.Key.ToString();

            GameObject number = Instantiate(textElementPrefab, numberRow);
            number.GetComponent<TextMeshProUGUI>().text = pair.Value.ToString();
        }
    }
}
