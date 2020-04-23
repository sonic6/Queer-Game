using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections.Generic;

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
    [SerializeField] Text myStrength;
   
    [SerializeField] int strength;
    public static NpcBehaviour myNpc;
    public static GroupTool myGroup;
    public static EnemyController myEnemy;

    [HideInInspector] public GameObject myPosition;

    public static List<GameObject> usedCards = new List<GameObject>(); //This list is to identify the cards that have just been used and aren't deleted yet

    [TextArea(5,20)]
    [SerializeField] private string description;

    private void Start()
    {
        GetImageComponentFromChildren();
        myStrength.text = strength.ToString();
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
        DisableCard();
        IndividualNpcs();
        Enemy();
        GroupNpcs();
        
    }

    void IndividualNpcs()
    {
        if (myNpc != null && KindOfCard == CardKind.Culture)
            myNpc.argumentUsed += strength;
        else if (myNpc != null && KindOfCard == CardKind.Celebrity)
            myNpc.materialUsed += strength;
        

        if (myNpc != null && myNpc.isFollower == false) //If this script has identified an NPC and it's not already a follower
            myNpc.FollowPlayer();
    }

    void GroupNpcs()
    {
        if (myGroup != null && KindOfCard == CardKind.Culture)
            myGroup.argumentUsed += strength;
        else if (myGroup != null && KindOfCard == CardKind.Celebrity)
            myGroup.materialUsed += strength;

        if (myGroup != null)
            myGroup.GroupFollowPlayer(); //Check if the group can follow the player and make them follow if so
        
    }

    void Enemy()
    {
        if (myEnemy != null && KindOfCard == CardKind.Shade)
        {
            myEnemy.GetComponent<NavMeshAgent>().speed = 0;
        }
    }

    void DisableCard()
    {
        usedCards.Add(gameObject);
        gameObject.SetActive(false);
    }

    public void UseInfoButton()
    {
        BookManager.infoImage.gameObject.SetActive(!BookManager.infoImage.gameObject.activeSelf);
        BookManager.infoImage.image.material.mainTexture = myImage.mainTexture;
        BookManager.infoImage.title.text = myTitle;
        BookManager.infoImage.description.text = description;
    }
}
