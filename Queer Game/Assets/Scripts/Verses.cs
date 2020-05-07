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
    
    public CardKind KindOfCard;

    private Image myImage;
    [Tooltip("The title of this card which will be shown in bigger text in the when the player clicks the info button")]
    [SerializeField] string myTitle;
    [SerializeField] Text myStrength;
    public Text myExtraPoints;

    public int strength; //The main strength of this card
    [HideInInspector] public static int extraStrength = 0; //The strength that's added per follower. Equals 0 on start
    public static NpcBehaviour myNpc;
    public static GroupTool myGroup;
    public static EnemyController myEnemy;

    [HideInInspector] public GameObject myPosition;

    public static List<GameObject> usedCards = new List<GameObject>(); //This list is to identify the cards that have just been used and aren't deleted yet

    [TextArea(5,20)]
    [SerializeField] private string description;

    private void Awake()
    {
        myStrength.text = strength.ToString();
    }

    private void Start()
    {
        GetImageComponentFromChildren();
        CardColor();
    }

    void CardColor()
    {
        if (KindOfCard == CardKind.Celebrity)
            GetComponent<Image>().color = BookManager.manager.celebrityColor;
        else if (KindOfCard == CardKind.Culture)
            GetComponent<Image>().color = BookManager.manager.cultureColor;
        else if (KindOfCard == CardKind.Shade)
            GetComponent<Image>().color = BookManager.manager.shadeColor;
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
            myNpc.cultureUsed += strength;
        else if (myNpc != null && KindOfCard == CardKind.Celebrity)
            myNpc.celebrityUsed += strength;
        

        if (myNpc != null && myNpc.isFollower == false) //If this script has identified an NPC and it's not already a follower
            myNpc.FollowPlayer(true);
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

    /// <summary>
    /// This function adds extra strength points to the play cards. "Number" is the amount of points to be added
    /// </summary>
    public void AddExtraStrengthUi()
    {
        myExtraPoints.text = "+" + extraStrength.ToString();
        strength = strength + extraStrength;
    }

    
}
