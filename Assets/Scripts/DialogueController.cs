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

    int currentLine = 0;

    void Start()
    {
        ShowLine();
    }

    public void NextLine()
    {
        currentLine++;

        if (currentLine >= convo.lines.Count)
        {
            Debug.Log("End of conversation");
            return;
        }

        ShowLine();
    }

    void ShowLine()
    {
        DialogueLine line = convo.lines[currentLine];
        speakerText.text = line.speakerName;
        string text = LocalizationSettings.StringDatabase.GetLocalizedString(convo.tableName,line.localKey); //fetch localized text from the specified table
        dialogueText.text = text; //apply
        Sprite portrait = line.portrait.LoadAssetAsync<Sprite>().WaitForCompletion(); //fetch sprite
        portraitImage.sprite = portrait; //apply
    }
}