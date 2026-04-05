using UnityEngine;

[CreateAssetMenu(fileName = "Missile2Config", menuName = "Combat/Missile2Config")]
public class Missile2Config : ScriptableObject
{
    [Header("Movement")]
    [Tooltip("水平速度（正值），实际移动方向由发射时传入的朝向决定")]
    public float verticalSpeed = 2f;

    [Header("Combat")]
    [Tooltip("造成的物理伤害")]
    public float damage = 10f;

    [Header("Lifetime")]
    [Tooltip("导弹存在时间（秒），到时自动销毁）")]
    public float lifeTime = 2f;

    private void OnValidate()
    {
        if (damage < 0f) damage = 0f;
        if (lifeTime < 0f) lifeTime = 0f;
    }
}
