using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using QueerGame;

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
    public List<GameObject> cardDeck = new List<GameObject>();

    [UnityEngine.Tooltip("Fill this up with the gameobjects that represent where the cards will show up on screen")]
    [SerializeField] Transform[] cardPositions;

    bool stopDrawingCards = false;
    public static Vector3 cardScale;
    /*public List<Transform> oldPositions;*/ //This array contains the positioons of cards that have been Destroyed (used)

    private void Awake()
    {
        NewSceneBegan();
        Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;

        foreach (Transform pos in cardPositions)
        {
            pos.gameObject.AddComponent<CardPosition>();
        }

        manager = this;
        FillDeck();
        DrawNewCards();
    }

    void NewSceneBegan()
    {
        Time.timeScale = 1;
    }
    

    //Fills the card deck with 8 celebrity cards, 8 culture cards and 4 shade cards
    void FillDeck()
    {
        int celeb = 0;
        int culture = 0;
        int shade = 0;

        do
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
            
        } while (cardDeck.Count < 20);

        QueerFunctions.ConvertToSceneRefrence(cardDeck);


    }

    // Start is called before the first frame update
    void Start()
    {
        infoImage = setInfoImage;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ClickedBook();
        }
    }

    public void DrawNewCards()
    {
        if (stopDrawingCards == false)
        {
            int handCards = 0;

            foreach (Transform pos in cardPositions)
            {
                if (pos.GetComponent<CardPosition>().myCard != null)
                    handCards++;
            }


            int toDraw = cardPositions.Length - handCards;
            for (int i = 0; i < toDraw; i++)
            {
                GameObject currentCard = cardDeck[Random.Range(0, cardDeck.Count)];
                currentCard.transform.SetParent(pagesHolder.transform);
                currentCard.transform.localScale = cardScale;
                InfoDealer.cardsInHand.Add(currentCard);
                cardDeck.Remove(currentCard); //Removes the card from the deck so it doesn't spawn again
                
                currentCard.transform.position = transform.position;
                
                foreach (Transform pos in cardPositions) //Find the first empty position and take it
                {
                    if (pos.GetComponent<CardPosition>().myCard == null)
                    {
                        pos.GetComponent<CardPosition>().myCard = currentCard;
                        break;
                    }
                }
                foreach(Transform pos in cardPositions)
                {
                    if (pos.GetComponent<CardPosition>().myCard == currentCard)
                    {
                        currentCard.GetComponent<Verses>().myPosition = pos.gameObject;
                        break;
                    }
                }

                currentCard.gameObject.SetActive(false);
                if (cardDeck.Count == 0)
                    stopDrawingCards = true;
            }
        }
    }
    

    public void ClickedBook()
    {
        DrawNewCards();
        if (InfoDealer.cardsInHand[0] == true)
        {
            GameObject card = InfoDealer.cardsInHand[0]; //No need to check all activestates of cards in hand, just one is enough for this

            if (card.activeSelf == false)
            {
                StartCoroutine(QueerFunctions.OpenCloseBook(InfoDealer.cardsInHand, "yes"));
            }
            else if (card.activeSelf == true)
            {
                StartCoroutine(QueerFunctions.OpenCloseBook(InfoDealer.cardsInHand, "no"));
            }
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

    //Used by FSM on pages. Sets the pages' gameobjects to enabled/disabled
    public void FlipPagesActiveState(GameObject myObject) 
    {
        myObject.gameObject.SetActive(!myObject.gameObject.activeSelf);
    }

    
}
