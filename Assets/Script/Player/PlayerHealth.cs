using UnityEngine;

public class PlayerHealth : EntityHealth
{
    private Player player;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<Player>();
    }

    protected override void Die()
    {
        base.Die();
        Entity<Player>.instance.EntityDeath();
    }
}
