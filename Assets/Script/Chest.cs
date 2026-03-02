using UnityEngine;
using UnityEngine.InputSystem;

public class Chest : MonoBehaviour
{
    public LayerMask playerLayer;
    private bool isPlayerInRange = false;
    private bool isChestOpened = false;
    private Animator chestAnimator;
    public Player player;

    void Start() 
    {
        chestAnimator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (isPlayerInRange && !isChestOpened &&
            player != null &&
            player.input.Player.Action.WasPressedThisFrame())
        {
            PlayChestOpenAnimation();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            isPlayerInRange = true;
            Debug.Log("按下E打开宝箱");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            isPlayerInRange = false;
        }
    }

    private void PlayChestOpenAnimation()
    {
        isChestOpened = true;

        if (chestAnimator != null)
        {
            chestAnimator.SetBool("IsTriggered", true);
            Debug.Log("宝箱打开动画已播放");
        }
        else
        {
            Debug.LogWarning("未找到宝箱的Animator组件！");
        }
    }
}