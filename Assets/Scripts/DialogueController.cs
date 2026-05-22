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
    GameObject currentModel;

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
        string text = LocalizationSettings.StringDatabase.GetLocalizedString(convo.tableName,line.localKey);
        dialogueText.text = text;

        
        if (currentModel != null) // delete previous model
        {
            Destroy(currentModel);
        }

        portraitImage.gameObject.SetActive(false); // hide spirte by default
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
    }
}