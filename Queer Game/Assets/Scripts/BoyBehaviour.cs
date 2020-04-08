using UnityEngine;
using UnityEngine.AI;

public class BoyBehaviour : NpcBehaviour
{
    [Tooltip("The allowed distance between this ai agent and their destination before they start running towards it")]
    [SerializeField] float allowedDistance;

    [Tooltip("The running speed of this gameobjects ai agent")]
    [SerializeField] float runningSpeed;

    [Tooltip("The fast running speed of this gameobjects ai agent (only relevant id this NPC is 'Boy')")]
    [SerializeField] float fastRunSpeed;

    private void Awake()
    {
        myAnimator = GetComponentInChildren<Animator>();
        transform.GetChild(0).gameObject.AddComponent<TalkTrigger>();
        player = FindObjectOfType<PlayerMovement>().gameObject;
        aiAgent = GetComponent<NavMeshAgent>();
    }
    
    public void HandleAnimations()
    {
        Vector2 vector = CalculateDistance();

        if (aiAgent.velocity.magnitude == 0)
        {
            DisableAllParameters("idle");
            if (myAnimator.GetBool("idle") != true)
                myAnimator.SetBool("idle", true);
        }

        if (aiAgent.velocity.magnitude > 0 && (vector.x <= allowedDistance && vector.y <= allowedDistance))
        {
            DisableAllParameters("walk");
            if (myAnimator.GetBool("walk") != true)
                myAnimator.SetBool("walk", true);
            aiAgent.speed = walkingSpeed;
        }

        if (aiAgent.velocity.magnitude > 0 && ((vector.x > allowedDistance && vector.x < allowedDistance * 1.5f) || (vector.y > allowedDistance && vector.y < allowedDistance * 1.5f)))
        {
            DisableAllParameters("run");
            if (myAnimator.GetBool("run") != true)
                myAnimator.SetBool("run", true);
            aiAgent.speed = runningSpeed;
        }

        else if (aiAgent.velocity.magnitude > 0 && (vector.x >= allowedDistance * 2 || vector.y >= allowedDistance * 2)) 
        {
            DisableAllParameters("fastRun");
            if (myAnimator.GetBool("fastRun") != true)
                myAnimator.SetBool("fastRun", true);
            aiAgent.speed = fastRunSpeed;
        }
    }

    void DisableAllParameters(string paramName) //Disables all parameters but paramName
    {
        if (paramName != "idle")
            myAnimator.SetBool("idle", false);
        if (paramName != "walk")
            myAnimator.SetBool("walk", false);
        if (paramName != "run")
            myAnimator.SetBool("run", false);
        if (paramName != "fastRun")
            myAnimator.SetBool("fastRun", false);
    }
    

    private void FixedUpdate()
    {
        HandleAnimations();
        if (isFollower)
            aiAgent.destination = player.transform.position;
    }
}
