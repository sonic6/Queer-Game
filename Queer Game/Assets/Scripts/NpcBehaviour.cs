using UnityEngine;
using UnityEngine.AI;

public class NpcBehaviour : MonoBehaviour
{
    [Tooltip("The walking speed of this gameobjects ai agent")]
    [SerializeField] float walkingSpeed;

    [Tooltip("The running speed of this gameobjects ai agent")]
    [SerializeField] float runningSpeed;

    [Tooltip("The allowed distance between this ai agent and their destination before they start running towards it")]
    [SerializeField] float allowedDistance;

    private Animator myAnimator; 
    private GameObject player; //This is the player gameobject

    [HideInInspector] public bool convertedByEnemy = false; //If this bool is true it means the player shouldn' be able to convert this npc to their side

    [HideInInspector] public bool isFollower = false; //When the value of this bool is true, it means this NPC is a follower

    [Tooltip("This is how much material points this npc requires")]
    public int materialRequired;
    [Tooltip("This is how much argument points this npc requires")]
    public int argumentRequired;

    //This is how much material points the player has used for this npc
    [HideInInspector] public int materialUsed;
    //This is how much argument points the player has used for this npc
    [HideInInspector] public int argumentUsed;

    private NavMeshAgent aiAgent;

    private void Awake()
    {
        myAnimator = GetComponentInChildren<Animator>();
        transform.GetChild(0).gameObject.AddComponent<TalkTrigger>();
        player = FindObjectOfType<PlayerMovement>().gameObject;
        aiAgent = GetComponent<NavMeshAgent>();
    }

    public void FollowPlayer()
    {
        if (materialRequired <= materialUsed && argumentRequired <= argumentUsed)
        {
            isFollower = true;
            FollowerCounter.AddFollower();
            transform.GetChild(0).GetComponent<SphereCollider>().enabled = false;
            aiAgent.stoppingDistance = 2; //The distance this npc will keep from the player while following
        }
    }

    private void HandleAnimations()
    {
        Vector2 vector = CalculateDistance();

        if (aiAgent.velocity.magnitude == 0)
        {
            DisableAllParameters();
            myAnimator.SetBool("idle", true);
        }

        if (aiAgent.velocity.magnitude > 0 && (vector.x < allowedDistance && vector.y < allowedDistance))
        {
            DisableAllParameters();
            myAnimator.SetBool("walk", true);
            aiAgent.speed = walkingSpeed;
        }
        
        else if (aiAgent.velocity.magnitude > 0 && (vector.x > allowedDistance && vector.y > allowedDistance))
        {
            DisableAllParameters();
            myAnimator.SetBool("run", true);
            aiAgent.speed = runningSpeed;
        }

        else if(aiAgent.velocity.magnitude > 0 && (vector.x > allowedDistance*1.5f && vector.y > allowedDistance*1.5f))
        {
            DisableAllParameters();
            myAnimator.SetBool("fastRun", true);
            aiAgent.speed = runningSpeed*1.3f;
        }
    }

    void DisableAllParameters()
    {
        myAnimator.SetBool("idle", false);
        myAnimator.SetBool("walk", false);
        myAnimator.SetBool("run", false);
    }

    private Vector2 CalculateDistance()
    {
        float distanceX;
        float distanceZ;
        distanceX = Mathf.Abs(player.transform.position.x - gameObject.transform.position.x);
        distanceZ = Mathf.Abs(player.transform.position.z - gameObject.transform.position.z);
        Vector2 vec = new Vector2(distanceX, distanceZ);
        return vec;
    }

    private void FixedUpdate()
    {
        HandleAnimations();
        if(isFollower)
            aiAgent.destination = player.transform.position;
    }
}

public class TalkTrigger : MonoBehaviour
{
    //This class is used to identify the object it's attached to

    
}
