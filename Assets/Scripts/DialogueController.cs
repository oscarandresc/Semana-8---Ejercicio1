using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Settings;

public class DialogueController : MonoBehaviour
{
    public Image portraitImage;
    public TMP_Text speakerText;
    public TMP_Text dialogueText;
    public Convo convo;
    public Transform modelSpawnPoint;
    public GameObject choiceButtonPrefab;
    public Transform choiceContainer;
    GameObject currentModel;
    List<GameObject> spawnedButtons = new List<GameObject>();
    int currentLine = 0;

    void Start()
    {
        ShowLine();
    }

    public void NextLine()
    {
        DialogueLine line = convo.lines[currentLine];

        if (line.choices.Count > 0) // stop player from skipping choice lines
        {
            return;
        }

        currentLine++;

        if (currentLine >= convo.lines.Count)
        {
            
            if (convo.nextConvo != null) // automatically move to next convo if there is one
            {
                StartConvo(convo.nextConvo);
                return;
            }

            Debug.Log("End of conversation");
            return;
        }

        ShowLine();
    }

    void StartConvo(Convo newConvo)
    {
        convo = newConvo;
        currentLine = 0;
        ShowLine();
    }

    void ShowLine()
    {
        ClearChoices();
        DialogueLine line = convo.lines[currentLine];
        speakerText.text = line.speakerName;
        string text = LocalizationSettings.StringDatabase.GetLocalizedString(convo.tableName, line.localKey);
        dialogueText.text = text;

        if (currentModel != null) // delete previous model
        {
            Destroy(currentModel);
        }

        portraitImage.gameObject.SetActive(false); // hide sprite by default

        if (line.portraitSprite.RuntimeKeyIsValid())// show sprite if there is one
        {
            portraitImage.gameObject.SetActive(true);
            Sprite portrait = line.portraitSprite.LoadAssetAsync<Sprite>().WaitForCompletion();
            portraitImage.sprite = portrait;
        }
        else if (line.portraitModel.RuntimeKeyIsValid())// show model if there is one
        {
            GameObject model = line.portraitModel.LoadAssetAsync<GameObject>().WaitForCompletion();
            currentModel = Instantiate(model, modelSpawnPoint);
        }

        
        foreach (DialogueChoice choice in line.choices) // create choice buttons
        {
            GameObject buttonObj = Instantiate(choiceButtonPrefab, choiceContainer);
            spawnedButtons.Add(buttonObj);
            TMP_Text buttonText = buttonObj.GetComponentInChildren<TMP_Text>();
            buttonText.text = choice.choiceText;
            Button button = buttonObj.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                StartConvo(choice.nextConvo);
            });
        }
    }

    void ClearChoices()
    {
        foreach (GameObject button in spawnedButtons)
        {
            Destroy(button);
        }
        spawnedButtons.Clear();
    }
}