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

    //private void Start()
    //{
    //    player = FindObjectOfType<PlayerMovement>();
    //}
    
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

        myNpc.FollowPlayer();
    }
}
