using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public List<GameObject> npcs; //For some reason causes the code not to work when private
    private NavMeshAgent me;
    private bool pollute; //When this bool is active, this enemy can start affecting an NPC
    private float subtraction;

    [Tooltip("The time it takes for this Enemy to pollute an NPC")]
    [SerializeField] float timeToPollute;
    
    void Start()
    {
        me = GetComponent<NavMeshAgent>();
        FindNpcs();
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
        if(npcs.Count != 0)
            me.SetDestination(npcs[0].transform.position);
        if (pollute)
            PolluteNpc(npcs[0]);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.gameObject == npcs[0].gameObject)
        {
            pollute = true;
            subtraction = npcs[0].GetComponentInChildren<Canvas>().transform.localScale.x / timeToPollute;
        }
    }

    private void PolluteNpc(GameObject npc)
    {
        Transform canvasSize = npc.GetComponentInChildren<Canvas>().transform;
        canvasSize.localScale = new Vector3(canvasSize.localScale.x - subtraction * Time.deltaTime, canvasSize.localScale.y, canvasSize.localScale.z);
        if (canvasSize.localScale.x <= 0)
        {
            npcs.Remove(npcs[0]);
            pollute = false;
        }
    }
}
