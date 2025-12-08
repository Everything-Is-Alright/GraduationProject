using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class VFXAutoController : MonoBehaviour
{
    [SerializeField]private bool autoDestroy = true;
    [SerializeField] private float destroyDelay = 1;

    private void Start()
    {
        if(autoDestroy)
        {
            Destroy(gameObject, destroyDelay);
        }
    }
}
