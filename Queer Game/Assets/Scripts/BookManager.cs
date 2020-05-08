using UnityEngine;
using HutongGames.PlayMaker;
using UnityEngine.UI;
using System.Collections.Generic;

public class BookManager : MonoBehaviour
{
    public Color celebrityColor;
    public Color cultureColor;
    public Color shadeColor;

    [UnityEngine.Tooltip("The tutorial FSM if this is a tutorial level")]
    public PlayMakerFSM tutorialFsm;

    public static BookManager manager;

    public static MemeExplainer infoImage; //Used by Verses script
    [SerializeField] MemeExplainer setInfoImage; //Used to set the value of infoImage through the inspector

    [UnityEngine.Tooltip("The gameobject that all the 'pages' are attached to")]
    public GameObject pagesHolder;

    [UnityEngine.Tooltip("The gameobject that defines the 'up position' of the book")]
    [SerializeField] GameObject bookUpPos;

    PlayMakerFSM[] pages; //The pages where the "Memes" are
    Button[] pageButtons; //The buttons on each page in pages

    [UnityEngine.Tooltip("This represents the game cards and should be filled up manually with card prefabs")]
    [SerializeField] GameObject[] cards;

    //The cards that the deck in this level holds
    List<GameObject> cardDeck = new List<GameObject>();

    [UnityEngine.Tooltip("Fill this up with the gameobjects that represent where the cards will show up on screen")]
    [SerializeField] Transform[] cardPositions;

    public List<Transform> oldPositions; //This array contains the positioons of cards that have been Destroyed (used)

    private void Awake()
    {
        manager = this;
        FillDeck();
        DrawNewCards();
    }

    //Fills the card deck with 8 celebrity cards, 8 culture cards and 4 shade cards
    void FillDeck()
    {
        int celeb = 0;
        int culture = 0;
        int shade = 0;

        while(cardDeck.Count < 20)
        {
            var card = cards[Random.Range(0, cards.Length)];
            var cardKind = card.GetComponent<Verses>().KindOfCard;
            if (celeb < 8 && cardKind == Verses.CardKind.Celebrity)
            {
                cardDeck.Add(card);
                celeb++;
            }
            if (culture < 8 && cardKind == Verses.CardKind.Culture)
            {
                cardDeck.Add(card);
                culture++;
            }
            if (shade < 4 && cardKind == Verses.CardKind.Shade)
            {
                cardDeck.Add(card);
                shade++;
            }

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        infoImage = setInfoImage;
    }

    public void DrawNewCards()
    {
        BoxCollider2D[] inHand = null; //Using BoxCollider2D only because it's the only component that isn't repreated more than once in the card prefab and it's children. Otherwise, that would cause problems
        int handCards = 0;
        if (pagesHolder.transform.childCount > 0)
        {
            inHand = pagesHolder.GetComponentsInChildren<BoxCollider2D>();
            handCards = inHand.Length;
        }
        int toDraw = cardPositions.Length - handCards;
        for (int i = 0; i < toDraw; i++)
        {
            GameObject currentCard = Instantiate(cardDeck[Random.Range(0, cardDeck.Count)], pagesHolder.transform);
            cardDeck.Remove(currentCard); //Removes the card from the deck so it doesn't spawn again
            if (toDraw == cardPositions.Length)
                currentCard.transform.position = cardPositions[i].position;
            else
                currentCard.transform.position = oldPositions[i].transform.position;
            currentCard.GetComponent<Verses>().myPosition = cardPositions[i].gameObject;
        }

        //The following sets up the newly generated cards to work with their FSM components
        pages = pagesHolder.GetComponentsInChildren<PlayMakerFSM>();
        foreach (PlayMakerFSM page in pages)
        {
            page.SendEvent("myStart");
            page.FsmVariables.GetFsmGameObject("mySelf").CastVariable = new FsmGameObject(page.gameObject);
            page.FsmVariables.GetFsmGameObject("book position").CastVariable = new FsmGameObject(bookUpPos);
            page.FsmVariables.GetFsmGameObject("book").CastVariable = new FsmGameObject(gameObject);
        }
    }

    /// <summary>
    /// Used by Tutorial FSM to move the next tutorial objective
    /// </summary>
    /// <param name="fsm"></param>
    public void MoveNext(PlayMakerFSM fsm)
    {
        fsm.SendEvent("Next");
    }

    //Used by this gameobject to reactivate pages/cards gameobjects
    public void ScatterPages()
    {
        foreach (PlayMakerFSM page in pages)
        {
            if(page != null)
                page.gameObject.SetActive(true);
            page.SendEvent("Move outside book");
        }
    }

    //Used by FSM on pages. Sets the pages' gameobjects to enabled/disabled
    public void FlipPagesActiveState(GameObject myObject) 
    {
        myObject.gameObject.SetActive(!myObject.gameObject.activeSelf);
    }

    
}
