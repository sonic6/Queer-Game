using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public Camera myCam;
    public NavMeshAgent agent;
    [SerializeField] Vector3 cameraPosition;
    bool cameraUpdate = true;

    float ogSpeed;


    private void Start()
    {
        StartCoroutine(CameraUpdate());
        ogSpeed = agent.speed;
    }

    float clickTime = 0;
    // Update is called once per frame
    void Update()
    {
        MouseDragMovement();

        if (Input.GetMouseButtonDown(0))
        {
            clickTime = Time.realtimeSinceStartup;
            Ray ray = myCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            

            if(Physics.Raycast(ray, out hit))
            {
                if (EventSystem.current.IsPointerOverGameObject(-1)) //If the mouse pointer is over a UI
                    return;
                else if (hit.collider.gameObject.GetComponent<NpcBehaviour>() && hit.collider.gameObject.GetComponent<NpcBehaviour>().isFollower)
                    return; //If the NPC that was clicked on is a follower, do not go to towards them. Other scripts will handle what happens
                else
                {
                    agent.SetDestination(hit.point);
                }
            }
            
        }

        
    }

    private void MouseDragMovement()
    {
        
        Ray ray = myCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);

        

        if (Input.GetMouseButton(0))
        {
            agent.ResetPath();
            transform.LookAt(hit.point, Vector3.up);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            //Vector3 playerPosToScrn = myCam.WorldToScreenPoint(transform.position);
            //transform.eulerAngles = new Vector3(0,Vector2.SignedAngle(playerPosToScrn, Input.mousePosition),0);
            //print(Vector2.Angle(playerPosToScrn, Input.mousePosition));
            agent.Move(transform.forward * Time.deltaTime * agent.speed);
        }
    }

    //Used in tutorial to stop camera from following player
    public void FlipCamera()
    {
        cameraUpdate = !cameraUpdate;
    }

    IEnumerator CameraUpdate()
    {
        while (cameraUpdate)
        {
            myCam.transform.position = new Vector3(transform.position.x + cameraPosition.x, transform.position.y + cameraPosition.y, transform.position.z + cameraPosition.z);
            yield return new WaitForEndOfFrame();
        } 
        yield break;
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

        else if (other.GetComponent<GroupTool>()) //if the NPCs are in a group
        {
            Verses.myGroup = other.GetComponent<GroupTool>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<TalkTrigger>())
        {
            Verses.myNpc = null;
            //foreach(GameObject card in Verses.usedCards) //If the player leaves before recruiting a follower. reactivate the used cards
            //{
            //    card.SetActive(true);
            //}
        }
        if (other.GetComponent<GroupTool>())
        {
            Verses.myGroup = null;
            //foreach (GameObject card in Verses.usedCards) //If the player leaves before recruiting a follower. reactivate the used cards
            //{
            //    card.SetActive(true);
            //}
        }
    }

    //Helps stop player in tutorial
    public void MovementSpeed(bool move)
    {
        if (!move)
            agent.speed = 0;
        else
            agent.speed = ogSpeed;
    }
}
