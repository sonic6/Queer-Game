using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using HutongGames.PlayMaker;

public class PlayerMovement : MonoBehaviour
{
    public Camera myCam;
    public NavMeshAgent agent;
    [SerializeField] Vector3 cameraPosition;
    

    // Update is called once per frame
    void Update()
    {
        CameraFollow();

        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = myCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            

            if(Physics.Raycast(ray, out hit))
            {
                if (EventSystem.current.IsPointerOverGameObject(-1)) //If the mouse pointer is over a UI
                    return;
                else
                {
                    agent.SetDestination(hit.point);
                }
            }
        }
    }

    void CameraFollow()
    {
        myCam.transform.position = new Vector3(transform.position.x + cameraPosition.x, cameraPosition.y, transform.position.z + cameraPosition.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<TalkTrigger>() && other.transform.parent.GetComponent<NpcBehaviour>().convertedByEnemy == false)
        {
            Verses.myNpc = other.transform.parent.GetComponent<NpcBehaviour>();
        }

        //This else statement is just for debugging
        else if (other.GetComponent<TalkTrigger>() && other.transform.parent.GetComponent<NpcBehaviour>().convertedByEnemy == true)
            print("This Npc can't be converted anymore");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<TalkTrigger>())
        {
            Verses.myNpc = null;
        }
    }
}
