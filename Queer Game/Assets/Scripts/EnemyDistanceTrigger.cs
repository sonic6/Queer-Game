using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDistanceTrigger : MonoBehaviour
{
    public static List<GameObject> pollutedNpcs;
    float ogTriggerRad;

    private void Start()
    {
        pollutedNpcs = new List<GameObject>();
        ogTriggerRad = GetComponent<SphereCollider>().radius;
    }
    
    public IEnumerator ExpandTrigger()
    {
        while(EnemyController.currentTarget == null && (FollowerCounter.npcCount - FollowerCounter.required < FollowerCounter.pollutedCount) == false)
        {
            GetComponent<SphereCollider>().radius++;
            yield return null;
        }
        yield break;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<NpcBehaviour>() && pollutedNpcs.Contains(other.gameObject) == false)
        {
            EnemyController.currentTarget = other.gameObject;
            transform.parent.GetComponent<NavMeshAgent>().destination = other.transform.position;
            GetComponent<SphereCollider>().radius = ogTriggerRad;
        }
    }
}
