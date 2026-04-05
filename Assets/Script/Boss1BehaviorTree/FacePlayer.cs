using UnityEngine;

/// <summary>
/// 2D标准 玩家朝向脚本
/// 适配导弹系统：使用localScale.x翻转，无旋转冲突
/// </summary>
[RequireComponent(typeof(Transform))]
public class FacePlayer : MonoBehaviour
{
    [Header("锁定目标")]
    public Transform target;
    [Header("玩家标签")]
    public string targetTag = "Player";
    [Header("朝向阈值（防止抖动）")]
    public float threshold = 0.01f;

    /// <summary>
    /// 公开朝向属性，给导弹生成器调用（1=右，-1=左）
    /// </summary>
    public int Facing => facingRight ? 1 : -1;

    private bool facingRight;

    void Start()
    {
        // 自动寻找玩家
        if (target == null)
        {
            GameObject player = GameObject.FindWithTag(targetTag);
            if (player != null) target = player.transform;
        }

        // 初始化朝向
        facingRight = transform.localScale.x > 0f;
    }

    void Update()
    {
        if (target == null) return;

        // 计算玩家与Boss的水平距离
        float xDiff = target.position.x - transform.position.x;

        // 玩家在右侧 + 当前朝左 → 翻转
        if (xDiff > threshold && !facingRight) Flip();
        // 玩家在左侧 + 当前朝右 → 翻转
        else if (xDiff < -threshold && facingRight) Flip();
    }

    /// <summary>
    /// 2D标准翻转：修改X缩放（兼容所有导弹/动画系统）
    /// </summary>
    private void Flip()
    {
        // 禁用旋转！改用缩放翻转
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        facingRight = !facingRight;
    }
}