using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public static bool MoveActiveState = true;
    public Transform cameraPivot;
    public Camera myCam;
    public NavMeshAgent agent;
    [SerializeField] Vector3 cameraPosition;
    bool cameraUpdate = true;

    float ogSpeed;
    float startTime = 0; //Is used to make sure that the player is infact holding down the left mouse button for longer than 0.1 seconds


    private void Start()
    {
        StartCoroutine(CameraUpdate());
        ogSpeed = agent.speed;
        myCam.gameObject.AddComponent<CameraCollision>();
    }
    
    void MouseKeyboardMove(bool active)
    {
        if(active)
        {
            agent.ResetPath();
            cameraPivot.position = transform.position;
            agent.Move(cameraPivot.forward * agent.speed * Input.GetAxis("Vertical") / 20);
            agent.Move(transform.right * agent.speed * Input.GetAxis("Horizontal") / 20);
            cameraPivot.eulerAngles = new Vector3(0, Input.mousePosition.x, 0);
            transform.eulerAngles = cameraPivot.eulerAngles;
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
            MouseKeyboardMove(MoveActiveState);
            yield return new WaitForEndOfFrame();
        } 
        yield break;
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<TalkTrigger>() && other.transform.parent.GetComponent<NpcBehaviour>().convertedByEnemy == false)
        {
            Verses.myNpc = other.transform.parent.GetComponent<NpcBehaviour>();
            AudioSource source = Verses.myNpc.GetComponent<AudioSource>();
            AudioClip currentClip = Verses.myNpc.GetComponent<NpcBehaviour>().triggerSounds[Random.Range(0, Verses.myNpc.GetComponent<NpcBehaviour>().triggerSounds.Length)];
            source.clip = currentClip;
            source.Play();
        }

        //This else statement is just for debugging
        else if (other.GetComponent<TalkTrigger>() && other.transform.parent.GetComponent<NpcBehaviour>().convertedByEnemy == true)
            print("This Npc can't be converted anymore");

        else if (other.GetComponent<GroupTool>() && other.GetComponent<GroupTool>().convertedByEnemy == false) //if the NPCs are in a group
        {
            Verses.myGroup = other.GetComponent<GroupTool>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<TalkTrigger>())
        {
            Verses.myNpc = null;
        }
        if (other.GetComponent<GroupTool>())
        {
            Verses.myGroup = null;
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

public class CameraCollision : MonoBehaviour
{
    public float minDistance = 1;
    public float maxDistance = 6;
    public float smooth = 10;
    Vector3 camDir;
    float distance;

    private void Awake()
    {
        camDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }

    private void Update()
    {
        Vector3 newCamPos = transform.parent.TransformPoint(camDir * maxDistance);
        RaycastHit hit;

        if (Physics.Linecast(transform.parent.position, newCamPos, out hit) && hit.collider.gameObject.GetComponent<NpcBehaviour>() == null && hit.collider.gameObject.GetComponent<EnemyController>() == null && hit.collider.isTrigger == false)
            distance = Mathf.Clamp(hit.distance * .9f, minDistance, maxDistance);
        else
            distance = maxDistance;
        transform.localPosition = Vector3.Lerp(transform.localPosition, camDir * distance, Time.deltaTime * smooth);
    }
}
