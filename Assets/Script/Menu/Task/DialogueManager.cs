using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("TMP UI 绑定")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI contentText;

    public bool isDialogueActive = false;
    private string[] currentSentences;
    private int currentIndex;

    void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
    }

    // 开始对话（可重复调用）
    public void StartDialogue(DialogueData data)
    {
        if (data == null || isDialogueActive) return;

        isDialogueActive = true;
        dialoguePanel.SetActive(true);
        DisablePlayerMovement(); // 对话期间禁止移动

        npcNameText.text = data.npcName;
        currentSentences = data.sentences;
        currentIndex = 0;
        ShowCurrentSentence();
    }

    private void ShowCurrentSentence()
    {
        contentText.text = currentSentences[currentIndex];
    }

    public void NextSentence()
    {
        if (!isDialogueActive) return;

        currentIndex++;
        if (currentIndex >= currentSentences.Length)
        {
            EndDialogue(); // 只隐藏，不销毁
            return;
        }
        ShowCurrentSentence();
    }

    // 结束对话：仅隐藏，可再次按E打开
    private void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
        EnablePlayerMovement(); // 恢复移动
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            NextSentence();
        }
    }

    private void DisablePlayerMovement()
    {
        Player player = FindObjectOfType<Player>();
        if (player != null) player.SetMovementEnabled(false);
    }

    private void EnablePlayerMovement()
    {
        Player player = FindObjectOfType<Player>();
        if (player != null) player.SetMovementEnabled(true);
    }
}