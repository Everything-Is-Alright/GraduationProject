using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public Stat maxHp;
    public StatMajor major;

    public float GetMaxHealth()
    {
        float baseHp = maxHp.GetValue();
        float bonusHp = major.vitality.GetValue() * 5;
        //Debug.Log(baseHp + " " + bonusHp);
        return baseHp + bonusHp;
    }
}
