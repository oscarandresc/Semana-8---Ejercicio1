using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Conversation")]
public class Convo : ScriptableObject
{
    public string tableName;
    public List<DialogueLine> lines;
}