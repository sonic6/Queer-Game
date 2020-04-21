using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Verses : MonoBehaviour
{
    public enum CardKind
    {
        Celebrity,
        Culture,
        Shade
    };

    //private PlayerMovement player;
    public CardKind KindOfCard;

    private Image myImage;
    [Tooltip("The title of this card which will be shown in bigger text in the when the player clicks the info button")]
    [SerializeField] string myTitle;
   
    [SerializeField] int strength;
    public static NpcBehaviour myNpc;
    public static EnemyController myEnemy;

    private void Start()
    {
        GetImageComponentFromChildren();
    }

    void GetImageComponentFromChildren()
    {
        Image[] images = GetComponentsInChildren<Image>();
        foreach(Image image in images)
        {
            if (image.tag == "myImage")
                myImage = image;
        }
    }

    public void UseButton()
    {
        if (myNpc != null && KindOfCard == CardKind.Culture)
            myNpc.argumentUsed += strength;
        else if (myNpc != null && KindOfCard == CardKind.Celebrity)
            myNpc.materialUsed += strength;
        else if(myEnemy != null && KindOfCard == CardKind.Shade)
        {
            myEnemy.GetComponent<NavMeshAgent>().speed = 0;
        }

        if(myNpc != null && myNpc.isFollower == false) //If this script has identified an NPC and it's not already a follower
            myNpc.FollowPlayer();
    }

    public void UseInfoButton()
    {
        BookManager.infoImage.gameObject.SetActive(!BookManager.infoImage.gameObject.activeSelf);
        BookManager.infoImage.image.material.mainTexture = myImage.mainTexture;
        BookManager.infoImage.title.text = myTitle;
    }
}
