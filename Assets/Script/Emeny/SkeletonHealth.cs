using UnityEngine;

public class SkeletonHealth : EntityHealth
{
    private Skeleton skeleton;

    private void Update()
    {
        skeleton = GetComponent<Skeleton>();
    }

    public override void TakeDamage(float damage, Transform damageDealer)
    {
        if(damageDealer.CompareTag("Player"))
        {
            skeleton.TryEnterBattleState(damageDealer);
        }
        base.TakeDamage(damage, damageDealer);
    }
}