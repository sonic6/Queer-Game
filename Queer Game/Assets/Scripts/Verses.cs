﻿using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.EventSystems;

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
    public static int extraStrength = 0; //The strength that's added per follower. Equals 0 on start
    public static NpcBehaviour myNpc;
    public static GroupTool myGroup;
    public static EnemyController myEnemy;

    [HideInInspector] public GameObject myPosition;

    /*public static List<GameObject> usedCards = new List<GameObject>();*/ //This list is to identify the cards that have just been used and aren't deleted yet

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

    private void OnMouseDown()
    {
        UseButton();
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
        IndividualNpcs();
        Enemy();
        GroupNpcs();


        DestroyCard();
        
    }

    void IndividualNpcs()
    {
        TMP_Text celeb = null;
        TMP_Text culture = null;

        if (myNpc != null && KindOfCard == CardKind.Culture)
        {
            myNpc.cultureUsed += strength;
            foreach(TMP_Text text in myNpc.GetComponentsInChildren<TMP_Text>())
            {
                if (text.gameObject.tag == "culture")
                    culture = text;
            }

            int value = int.Parse(culture.text);
            if (value > strength /*myNpc.cultureUsed*/)
                culture.text = (value - strength /*myNpc.cultureUsed*/).ToString();
            else
                culture.text = "0";
        }
        else if (myNpc != null && KindOfCard == CardKind.Celebrity)
        {
            myNpc.celebrityUsed += strength;
            foreach (TMP_Text text in myNpc.GetComponentsInChildren<TMP_Text>())
            {
                if (text.gameObject.tag == "celeb")
                    celeb = text;
            }

            int value = int.Parse(celeb.text);
            if (value > strength /*myNpc.celebrityUsed*/)
                celeb.text = (value - strength /*myNpc.celebrityUsed*/).ToString();
            else
                celeb.text = "0";
        }
        

        if (myNpc != null && myNpc.isFollower == false) //If this script has identified an NPC and it's not already a follower
            myNpc.FollowPlayer(true);
    }

    void GroupNpcs()
    {
        TMP_Text celeb = null;
        TMP_Text culture = null;

        if (myGroup != null && KindOfCard == CardKind.Culture)
        {
            myGroup.cultureUsed += strength;
            foreach (TMP_Text text in myGroup.GetComponentsInChildren<TMP_Text>())
            {
                if (text.gameObject.tag == "culture")
                    culture = text;
            }

            int value = int.Parse(culture.text);
            if (value > strength /*myGroup.cultureUsed*/)
                culture.text = (value - strength /*myGroup.cultureUsed*/).ToString();
            else
                culture.text = "0";
        }
        else if (myGroup != null && KindOfCard == CardKind.Celebrity)
        {
            myGroup.celebrityUsed += strength;
            foreach (TMP_Text text in myGroup.GetComponentsInChildren<TMP_Text>())
            {
                if (text.gameObject.tag == "celeb")
                    celeb = text;
            }

            int value = int.Parse(celeb.text);
            if (value > strength /*myGroup.celebrityUsed*/)
                celeb.text = (value - strength /*myGroup.celebrityUsed*/).ToString();
            else
                celeb.text = "0";
        }

        if (myGroup != null)
            myGroup.GroupFollowPlayer(); //Check if the group can follow the player and make them follow if so
        
    }

    void Enemy()
    {
        if (myEnemy != null && KindOfCard == CardKind.Shade)
        {
            myEnemy.Stop(strength);
        }
    }

    void DestroyCard()
    {
        if (QueerGame.QueerFunctions.MoveTowardsIsRunning == false) //This bool must be checked so that no cards can be used while they are moving on screen which would cause errors
        {
            myPosition.GetComponent<CardPosition>().myCard = null; //This card breaks its connection with the position it was in
                                                                   //BookManager.manager.cardDeck.Remove(gameObject);
            InfoDealer.cardsInHand.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    public void UseInfoButton()
    {
        BookManager.infoImage.gameObject.SetActive(!BookManager.infoImage.gameObject.activeSelf);
        BookManager.infoImage.image.material.mainTexture = myImage.mainTexture;
        BookManager.infoImage.title.text = myTitle;
        BookManager.infoImage.description.text = description;
    }

    /// <summary>
    /// This function adds extra strength points to the play cards. 
    /// </summary>
    public void AddExtraStrengthUi()
    {
        
        myExtraPoints.text = "+" + extraStrength.ToString();
        strength = strength + extraStrength;
    }

    
}

public class CardPosition : MonoBehaviour
{
    public GameObject myCard;
}
