using UnityEngine;

[CreateAssetMenu(fileName = "LaserConfig", menuName = "Combat/LaserConfig")]
public class LaserConfig : ScriptableObject
{
    [Header("Combat")]
    [Tooltip("造成的物理伤害")]
    public float damage = 30f;

    [Header("Lifetime")]
    [Tooltip("导弹存在时间（秒），到时自动销毁）")]
    public float lifeTime = 8f;

    private void OnValidate()
    {
        if (damage < 0f) damage = 0f;
        if (lifeTime < 0f) lifeTime = 0f;
    }
}
