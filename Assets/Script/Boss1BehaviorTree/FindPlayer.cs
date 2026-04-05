using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
public class FindPlayer : Conditional
{
   private GameObject player;

    public override void OnStart()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public override TaskStatus OnUpdate()
    {
        if (player != null)
        {
            SharedGameObject sharePlayer = player;
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}
