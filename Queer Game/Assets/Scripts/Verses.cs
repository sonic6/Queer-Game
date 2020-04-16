using UnityEngine;

public class Verses : MonoBehaviour
{
    public enum VerseKind
    {
        Celebrity,
        Culture,
        Shade
    };

    //private PlayerMovement player;
    public VerseKind KindOfVerse;

   
    [SerializeField] int strength;
    public static NpcBehaviour myNpc;
    

    public void UseButton()
    {
        if (myNpc != null && KindOfVerse == VerseKind.Culture)
            myNpc.argumentUsed += strength;
        else if (myNpc != null && KindOfVerse == VerseKind.Celebrity)
            myNpc.materialUsed += strength;
        else if(myNpc != null && KindOfVerse == VerseKind.Shade)
        {
            //Do stuff
        }

        if(myNpc != null && myNpc.isFollower == false) //If this script has identified an NPC and it's not already a follower
            myNpc.FollowPlayer();
    }

    public void UseInfoButton()
    {
        BookManager.infoImage.SetActive(!BookManager.infoImage.activeSelf);
    }
}
