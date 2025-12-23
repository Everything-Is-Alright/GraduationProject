using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public Stat maxHp;
    public StatMajor major;
    public StatOffense offense;
    public StatDefense defense;
    
    //生命值
    public float GetMaxHealth()
    {
        float baseHp = maxHp.GetValue();
        float bonusHp = major.vitality.GetValue() * 5;
        return baseHp + bonusHp;
    }

    //物理攻击力
    public float GetPhysicalDamage()
    {
        float baseDamage = offense.damage.GetValue();
        float bonusDamage = major.strength.GetValue();
        float totalBaseDamage = baseDamage + bonusDamage;

        float baseCritChance = offense.critChance.GetValue();
        float bonusCritChance = major.agility.GetValue() * .3f;
        float critChance = baseCritChance + bonusCritChance;

        float baseCritPower = offense.critPower.GetValue();
        float bonusCritPower = major.strength.GetValue() * .5f;
        float critPower = (baseCritPower + bonusCritPower) / 100;
        
        bool isCrit = Random.Range(0, 100) < critChance;
        float finalDamage = isCrit ? totalBaseDamage * critPower : totalBaseDamage;

        return finalDamage;
    }

    //防御力
    public float GetArmorMitigation()
    {
        float baseArmor = defense.armor.GetValue();
        float bonusArmor = major.vitality.GetValue();
        float totalArmor = baseArmor + bonusArmor;

        float mitigation = totalArmor / (totalArmor + 100);
        float mitigationCap = .85f;

        float finalMitigation = Mathf.Clamp(mitigation, 0, mitigationCap);
        return finalMitigation;
    }

    //魔法攻击
    public float GetMagicDamage()
    {
        float magicDamage = offense.magicDamage.GetValue();

        float bonusMagicDamage = major.intelligence.GetValue();

        float finalDamage = magicDamage + bonusMagicDamage;
        
        if(finalDamage <=0)
        {
            return 0;
        }

        return finalDamage;
    }
}
