using UnityEngine;
using UnityEngine.AI;

public class WomanBehaviour : NpcBehaviour
{
    [Tooltip("The allowed distance between this ai agent and their destination before they start running towards it")]
    [SerializeField] float allowedDistance;

    [Tooltip("The running speed of this gameobjects ai agent")]
    [SerializeField] float runningSpeed;

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
            if (isFollower == true)
            {
                DisableAllParameters("idle");
                if (myAnimator.GetBool("idle") != true)
                    myAnimator.SetBool("idle", true);
            }

            else if (isFollower == false && (vector.x <= allowedDistance && vector.y <= allowedDistance))
            {
                DisableAllParameters("talk");
                if (myAnimator.GetBool("talk") != true)
                    myAnimator.SetBool("talk", true);
            }

            else if (isFollower == false && (vector.x > allowedDistance || vector.y > allowedDistance))
            {
                DisableAllParameters("idle");
                if (myAnimator.GetBool("idle") != true)
                    myAnimator.SetBool("idle", true);
            }
        }

        

        else if (aiAgent.velocity.magnitude > 0 && (vector.x <= allowedDistance && vector.y <= allowedDistance))
        {
            DisableAllParameters("walk");
            if (myAnimator.GetBool("walk") != true)
                myAnimator.SetBool("walk", true);
            aiAgent.speed = walkingSpeed;
        }

        else if (aiAgent.velocity.magnitude > 0 && (vector.x > allowedDistance || vector.y > allowedDistance))
        {
            DisableAllParameters("run");
            if (myAnimator.GetBool("run") != true)
                myAnimator.SetBool("run", true);
            aiAgent.speed = runningSpeed;
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
        if (paramName != "talk")
            myAnimator.SetBool("talk", false);
    }



    private void FixedUpdate()
    {
        HandleAnimations();
        if (isFollower)
            aiAgent.destination = player.transform.position;
    }
}
