using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Camera myCam;
    public NavMeshAgent agent;
    
    

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = myCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            

            if(Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<TalkTrigger>())
        {
            Verses.myNpc = other.transform.parent.GetComponent<NpcBehaviour>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<TalkTrigger>())
        {
            Verses.myNpc = null;
        }
    }
}
