using UnityEngine;

public class Chest : MonoBehaviour, IDamgable
{
    public void TakeDamage(float damage, Transform damageDealer)
    {
        GetComponentInChildren<Animator>().SetBool("IsAttacked", true);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
