using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public Stat maxHp;
    public StatMajor major;

    
    public float GetMaxHealth()
    {
        if (maxHp == null)
        {
            Debug.LogError($"【EntityStats】maxHp 未赋值！挂载对象：{gameObject.name}", this);
            return 0; // 返回默认值避免崩溃
        }

        // 2. 校验 major 是否为空
        if (major == null)
        {
            Debug.LogError($"【EntityStats】major 未赋值！挂载对象：{gameObject.name}", this);
            return maxHp.GetValue(); // 仅返回基础血量，避免崩溃
        }

        // 3. 校验 major.vitality 是否为空
        if (major.vitality == null)
        {
            Debug.LogError($"【EntityStats】major.vitality 未赋值！挂载对象：{gameObject.name}", this);
            return maxHp.GetValue(); // 仅返回基础血量
        }

        float baseHp = maxHp.GetValue();
        float bonusHp = major.vitality.GetValue() * 5;
        //Debug.Log(baseHp + " " + bonusHp);
        return baseHp + bonusHp;
    }
}
