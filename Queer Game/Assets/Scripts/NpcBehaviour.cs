﻿using UnityEngine;
using UnityEngine.AI;

public class NpcBehaviour : MonoBehaviour
{
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

    private void FixedUpdate()
    {
        if(isFollower)
            aiAgent.destination = player.transform.position;
    }
}

public class TalkTrigger : MonoBehaviour
{
    //This class is used to identify the object it's attached to

    
}
