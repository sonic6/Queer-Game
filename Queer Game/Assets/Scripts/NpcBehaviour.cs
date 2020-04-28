using UnityEngine;
using UnityEngine.AI;
using QueerGame;
using System.Collections.Generic;
using System.Collections;

public class NpcBehaviour : MonoBehaviour
{
    /////////Several other scripts inherit variables from this script////////
    
    [Tooltip("The walking speed of this gameobjects ai agent")]
    public float walkingSpeed;

    private bool looping = true; //Used to start looping the FollowPlayerPosition coroutine
    
    [HideInInspector] public Animator myAnimator;
    [HideInInspector] public GameObject player; //This is the player gameobject

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

    [HideInInspector] public NavMeshAgent aiAgent;
    


    private void OnMouseDown()
    {
        if (isFollower && Verses.extraStrength != 0)
        {
            print("off to work");
            RemoveExtraStrengthUi();
            RecruitOthers();
        }
    }

    public void RemoveExtraStrengthUi()
    {
        Verses.extraStrength--;
        Verses[] cards = BookManager.manager.pagesHolder.GetComponentsInChildren<Verses>();

        foreach (Verses card in cards)
        {
            card.myExtraPoints.text = "+" + Verses.extraStrength.ToString();
            if (Verses.extraStrength == 0)
                card.myExtraPoints.text = "";
            card.strength--;
        }
    }

    void RecruitOthers()
    {
        
        List<NpcBehaviour> npcs = QueerFunctions.FindAvailableNpcs();
        Transform closest = npcs[0].transform;
        foreach (NpcBehaviour npc in npcs)
        {
            float myDistance = QueerFunctions.FindDistanceBetweenVectors(npc.transform.position, transform.position);
            float closesDistance = QueerFunctions.FindDistanceBetweenVectors(closest.transform.position, transform.position);
            if (myDistance < closesDistance)
                closest = npc.transform;
        }

        looping = false;
        aiAgent.SetDestination(closest.transform.position);
    }

    IEnumerator FollowPlayerPosition() //Will work as an update to find the player's location but, can be stopped when needed
    {
        while(looping)
        {
            aiAgent.SetDestination(player.transform.position);
            yield return new WaitForFixedUpdate();
        }
        yield return null;

    }

    public void FollowPlayer() //Gets called in "Verses" script
    {
        if (materialRequired <= materialUsed && argumentRequired <= argumentUsed)
        {
            isFollower = true;
            FollowerCounter.AddFollower();
            transform.GetChild(0).GetComponent<SphereCollider>().enabled = false;
            aiAgent.stoppingDistance = 2; //The distance this npc will keep from the player while following
            

            int nr = Verses.usedCards.Count;

            for (int i = 0; i < nr; i++) //Destroys the card gameobjects held in the static variable usedCards in Verses
            {
                GameObject currentCard = Verses.usedCards[i];
                BookManager.manager.oldPositions.Add(currentCard.GetComponent<Verses>().myPosition.transform);
                Destroy(currentCard);
            }

            Verses.usedCards.RemoveRange(0, Verses.usedCards.Count);

            BookManager.manager.DrawNewCards();

            Verses[] cardsInHand = BookManager.manager.pagesHolder.GetComponentsInChildren<Verses>();
            Verses.extraStrength++;
            foreach (Verses card in cardsInHand)
            {
                card.AddExtraStrengthUi();
            }

            StartCoroutine(FollowPlayerPosition());

        }
    }

    public Vector2 CalculateDistance()
    {
        float distanceX;
        float distanceZ;
        distanceX = Mathf.Abs(player.transform.position.x - gameObject.transform.position.x);
        distanceZ = Mathf.Abs(player.transform.position.z - gameObject.transform.position.z);
        Vector2 vec = new Vector2(distanceX, distanceZ);
        return vec;
    }
}

public class TalkTrigger : MonoBehaviour
{
    //This class is used to identify the object it's attached to

    
}
