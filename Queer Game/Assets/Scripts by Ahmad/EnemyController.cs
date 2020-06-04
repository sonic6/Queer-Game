﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using TMPro;

public class EnemyController : MonoBehaviour
{
    public static EnemyController mainEnemy;
    List<NpcBehaviour> myNpcGroup = new List<NpcBehaviour>();
    public static GameObject currentTarget;
    public List<GameObject> npcs; //For some reason causes the code not to work when private
    private NavMeshAgent me;
    private float subtraction;
    [HideInInspector] public bool caughtByPlayer = false; //Becomes true when the player is in the enemy's trigger box

    [Tooltip("The time it takes for this Enemy to pollute an NPC")]
    [SerializeField] float timeToPollute;

    private Animator myAnimator;
    bool foundGroup = false;

    float ogSpeed; //The speed of this ai agent

    public ParticleSystem fire;
    public AudioClip grunt;
    public AudioClip fireNoise;
    
    void Start()
    {
        mainEnemy = this;
        me = GetComponent<NavMeshAgent>();
        ogSpeed = me.speed;
        myAnimator = GetComponentInChildren<Animator>();
        FindNpcs();
        //EnemyDistanceTrigger.open = true;
        StartCoroutine(GetComponentInChildren<EnemyDistanceTrigger>().ExpandTrigger());
    }

    void FindNpcs()
    {
        NpcBehaviour[] tempArray;
        tempArray = FindObjectsOfType<NpcBehaviour>();

        for (int i = 0; i < tempArray.Length; i++)
        {

            if (tempArray[i].isFollower == false)
            {
                npcs.Add(tempArray[i].gameObject);
            }
        }
    }
    
    void Update()
    {
        Animate();
    }

    public void FindNewTarget()
    {
        if (npcs.Count != 0)
        {
            currentTarget = null;
            StartCoroutine(GetComponentInChildren<EnemyDistanceTrigger>().ExpandTrigger());
            //me.SetDestination(currentTarget.transform.position);
        }
    }

    public void WaitForPlayerAttack(Transform player)
    {
        me.SetDestination(transform.position);
        Vector3 velocity = new Vector3();
        StartCoroutine(FacePlayer());
        IEnumerator FacePlayer() //Keeps looking at player until they leave, meaning that caughtByPlayer = false
        {
            while (caughtByPlayer)
            {
                Vector3 ogRotation = transform.eulerAngles;
                transform.LookAt(player.position);
                Vector3 targetRotation = transform.eulerAngles;
                transform.eulerAngles = ogRotation;

                transform.eulerAngles = Vector3.SmoothDamp(transform.eulerAngles, targetRotation, ref velocity, .5f);
                yield return new WaitForEndOfFrame();
            }
            yield break;
        }
    }

    

    void Animate()
    {
        if(me.velocity.magnitude == 0)
        {
            myAnimator.SetBool("walk", false);
            myAnimator.SetBool("idle", true);
        }
        else
        {
            myAnimator.SetBool("idle", false);
            myAnimator.SetBool("walk", true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<TalkTrigger>() && other.transform.parent.gameObject == currentTarget.gameObject)
        {
            Canvas drainThis = currentTarget.GetComponent<NpcBehaviour>().drainCanvas;
            drainThis.gameObject.SetActive(true);
            subtraction = drainThis.transform.localScale.x / timeToPollute;
            StartCoroutine(PolluteNpc(currentTarget));
        }

        else if (other.gameObject.GetComponent<GroupTool>())
        {
            foundGroup = true;
            myNpcGroup = other.GetComponent<GroupTool>().npcs;
            foreach (NpcBehaviour npc in myNpcGroup)
            {
                npc.GetComponent<NpcBehaviour>().drainCanvas.gameObject.SetActive(true);
                if (npc.gameObject == currentTarget)
                {
                    Canvas drainThis = currentTarget.GetComponent<NpcBehaviour>().drainCanvas;
                    drainThis.gameObject.SetActive(true);
                    subtraction = drainThis.transform.localScale.x / timeToPollute;
                    StartCoroutine(PolluteGroup());
                }
            }
        }
        

    }

    IEnumerator PolluteGroup()
    {
        int amountConverted = 0;
        subtraction = currentTarget.GetComponentInChildren<Canvas>().transform.localScale.x / timeToPollute;
        while (foundGroup == true)
        {
            foreach (NpcBehaviour npc in myNpcGroup)
            {
                if (npc.gameObject == currentTarget)
                {
                    
                    Transform canvasSize = npc.GetComponentInChildren<Canvas>().transform;
                    if (npc.GetComponent<NpcBehaviour>().convertedByEnemy == false)
                    {
                        canvasSize.localScale = new Vector3(canvasSize.localScale.x - subtraction * Time.deltaTime, canvasSize.localScale.y, canvasSize.localScale.z);
                        if (canvasSize.localScale.x <= 0)
                        {
                            npc.GetComponent<NpcBehaviour>().horns.GetComponent<MeshRenderer>().enabled = true;
                            npc.GetComponent<NpcBehaviour>().convertedByEnemy = true;
                            currentTarget = null;
                            npcs.Remove(currentTarget);
                            FollowerCounter.CheckNonPollutedNpcs();
                            EnemyDistanceTrigger.pollutedNpcs.Add(npc.gameObject);
                            StartCoroutine(GetComponentInChildren<EnemyDistanceTrigger>().ExpandTrigger());

                            amountConverted++;
                            canvasSize.gameObject.SetActive(false);
                        }
                    }
                }
                
            }
            

            if (/*currentTarget == myNpcGroup[myNpcGroup.Count - 1]*/ amountConverted == myNpcGroup.Count)
            {
                foundGroup = false;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        myNpcGroup[0].partOfGroup.convertedByEnemy = true;
        myNpcGroup[0].partOfGroup.GetComponentInChildren<TMP_Text>().transform.parent.gameObject.SetActive(false);
    }

    IEnumerator PolluteNpc(GameObject npc)
    {
        Transform canvasSize = npc.GetComponentInChildren<Canvas>().transform;
        while (npc.GetComponent<NpcBehaviour>().convertedByEnemy == false)
        {
            canvasSize.localScale = new Vector3(canvasSize.localScale.x - subtraction * Time.deltaTime, canvasSize.localScale.y, canvasSize.localScale.z);
            if (canvasSize.localScale.x <= 0)
            {
                npc.GetComponent<NpcBehaviour>().horns.GetComponent<MeshRenderer>().enabled = true;
                npc.GetComponent<NpcBehaviour>().convertedByEnemy = true;
                npc.GetComponent<NpcBehaviour>().pointsCanvas.SetActive(false);
                currentTarget = null;
                npcs.Remove(currentTarget);
                FollowerCounter.CheckNonPollutedNpcs();
                EnemyDistanceTrigger.pollutedNpcs.Add(npc.gameObject);
                //StartCoroutine(GetComponentInChildren<EnemyDistanceTrigger>().ExpandTrigger());
            }
            if (npc.GetComponent<NpcBehaviour>().isFollower)
            {

                currentTarget = null;
                npcs.Remove(currentTarget);
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        Destroy(canvasSize.gameObject);
        StartCoroutine(GetComponentInChildren<EnemyDistanceTrigger>().ExpandTrigger());
        yield break;
    }
    public void Stop(float time)
    {
        StartCoroutine(BeStoppedByPlayer());
        IEnumerator BeStoppedByPlayer()
        {
            me.speed = 0;
            fire.Play();
            AudioSource audio = GetComponent<AudioSource>();
            audio.loop = false;
            audio.clip = grunt;
            audio.Play();
            yield return new WaitForSeconds(audio.clip.length);
            audio.clip = fireNoise;
            audio.loop = true;
            audio.Play();
            yield return new WaitForSeconds(time * 10);
            fire.Stop();
            audio.Stop();
            me.speed = ogSpeed;
        }
    }

    private void PolluteNextInGroup()
    {
        subtraction = currentTarget.GetComponentInChildren<Canvas>().transform.localScale.x / timeToPollute;
    }

    //Helps stop N-Amy in tutorial
    public void MovementSpeed(bool move)
    {
        if (!move)
            me.speed = 0;
        else
            me.speed = ogSpeed;
    }
}
