using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Package/New Dialogue")]
public class DialogueData : ScriptableObject
{
    [Header("NPC名字")]
    public string npcName;

    [Header("对话内容（一行一句）")]
    [TextArea] public string[] sentences;
}