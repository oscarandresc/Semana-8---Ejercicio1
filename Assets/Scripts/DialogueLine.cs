using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[System.Serializable]
public class DialogueLine
{
    public string speakerName;
    public string localKey;
    public AssetReferenceSprite portraitSprite;
    public AssetReferenceGameObject portraitModel;
    public List<DialogueChoice> choices;
}