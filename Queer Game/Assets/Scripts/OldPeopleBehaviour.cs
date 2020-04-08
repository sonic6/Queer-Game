using UnityEngine;
using UnityEngine.AI;

public class OldPeopleBehaviour : NpcBehaviour
{
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

        else if (aiAgent.velocity.magnitude > 0)
        {
            DisableAllParameters("walk");
            if (myAnimator.GetBool("walk") != true)
                myAnimator.SetBool("walk", true);
            aiAgent.speed = walkingSpeed;
        }

    }

    void DisableAllParameters(string paramName) //Disables all parameters but paramName
    {
        if (paramName != "idle")
            myAnimator.SetBool("idle", false);
        if (paramName != "walk")
            myAnimator.SetBool("walk", false);
    }

    

    private void FixedUpdate()
    {
        HandleAnimations();
        if (isFollower)
            aiAgent.destination = player.transform.position;
    }
}
