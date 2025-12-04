using UnityEngine;

public class Chest : MonoBehaviour, IDamgable
{
    public void TakeDamage(float damage, Transform damageDealer)
    {
        GetComponentInChildren<Animator>().SetBool("IsAttacked", true);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
