using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public Stat maxHp;
    public Stat vitality;

    public float GetMaxHealth()
    {
        float baseHp = maxHp.GetValue();
        if(baseHp == null)
        {
            Debug.Log("baseHpÎª¿Õ£¡");
        }
        float bonusHp = vitality.GetValue() * 5;
        //Debug.Log(baseHp + " " + bonusHp);
        return baseHp + bonusHp;
    }
}
