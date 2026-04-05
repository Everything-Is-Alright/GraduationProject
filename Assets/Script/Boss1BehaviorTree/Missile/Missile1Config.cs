using UnityEngine;

[CreateAssetMenu(fileName = "Missile1Config", menuName = "Combat/Missile1Config")]
public class Missile1Config : ScriptableObject
{
    [Header("Movement")]
    [Tooltip("水平速度（正值），实际移动方向由发射时传入的朝向决定")]
    public float horizontalSpeed = 5f;

    [Header("Combat")]
    [Tooltip("造成的物理伤害")]
    public float damage = 10f;

    [Header("Lifetime")]
    [Tooltip("导弹存在时间（秒），到时自动销毁）")]
    public float lifeTime = 3f;

    private void OnValidate()
    {
        if (horizontalSpeed < 0f) horizontalSpeed = -horizontalSpeed;
        if (damage < 0f) damage = 0f;
        if (lifeTime < 0f) lifeTime = 0f;
    }
}
