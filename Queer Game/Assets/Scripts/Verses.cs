using UnityEngine;

public class Verses : MonoBehaviour
{
    public enum VerseKind
    {
        Material,
        Argument,
        Finisher,
        Shade
    };

    //private PlayerMovement player;
    public VerseKind KindOfVerse;

    [SerializeField] int strength;
    public static NpcBehaviour myNpc;
    
    
    public void UseButton()
    {
        if (myNpc != null && KindOfVerse == VerseKind.Argument)
            myNpc.argumentUsed += strength;
        else if (myNpc != null && KindOfVerse == VerseKind.Material)
            myNpc.materialUsed += strength;
        else if(myNpc != null && KindOfVerse == VerseKind.Finisher)
        {
            //Do stuff
        }
        else if(myNpc != null && KindOfVerse == VerseKind.Shade)
        {
            //Do stuff
        }

        if(myNpc != null && myNpc.isFollower == false) //If this script has identified an NPC and it's not already a follower
            myNpc.FollowPlayer();
    }
}
