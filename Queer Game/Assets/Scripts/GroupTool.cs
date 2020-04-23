using System.Collections.Generic;
using UnityEngine;

public class GroupTool : MonoBehaviour
{
    public List<NpcBehaviour> npcs; //The NPC members of this group

    [SerializeField]int argumentsRequired;
    [SerializeField]int materialsRequired;

    [HideInInspector] public int materialUsed;
    [HideInInspector] public int argumentUsed;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<Rigidbody>();
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().isTrigger = true;

        foreach(NpcBehaviour npc in npcs)
        {
            argumentsRequired += npc.argumentRequired;
            materialsRequired += npc.materialRequired;
            npc.GetComponentInChildren<SphereCollider>().enabled = false; //Disable the individual sphere colliders for the members in the group
        }
        argumentsRequired = argumentsRequired - (npcs.Count * 2);
        materialsRequired = materialsRequired - (npcs.Count * 2);

    }

    public void GroupFollowPlayer() //Gets called in "Verses" script
    {
        if (materialsRequired <= materialUsed && argumentsRequired <= argumentUsed)
        {
            foreach(NpcBehaviour npc in npcs)
            {
                npc.isFollower = true;
                FollowerCounter.AddFollower(); //Because this is in a foreach loop. It will correctly add the number of followers in the group
                npc.aiAgent.stoppingDistance = 2; //The distance each npc will keep from the player while following
                Verses.myGroup = null;
                Destroy(gameObject); //Destroys the gameobject attached to this GroupTool
            }

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
            Verses.extraStrength += npcs.Count;
            foreach (Verses card in cardsInHand)
            {
                card.AddExtraStrengthUi();
            }
        }
    }
}
