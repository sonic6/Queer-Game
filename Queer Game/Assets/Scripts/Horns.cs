using UnityEngine;
public class Horns : MonoBehaviour
{
    private void Start()
    {
        gameObject.GetComponentInParent<NpcBehaviour>().horns = gameObject;
    }
}
