using UnityEngine;
using HutongGames.PlayMaker;
using UnityEngine.UI;

public class BookManager : MonoBehaviour
{
    public static MemeExplainer infoImage; //Used by Verses script
    [SerializeField] MemeExplainer setInfoImage; //Used to set the value of infoImage through the inspector

    [UnityEngine.Tooltip("The gameobject that all the 'pages' are attached to")]
    [SerializeField] GameObject pagesHolder;

    [UnityEngine.Tooltip("The gameobject that defines the 'up position' of the book")]
    [SerializeField] GameObject bookUpPos;

    PlayMakerFSM[] pages; //The pages where the "Memes" are
    Button[] pageButtons; //The buttons on each page in pages

    

    // Start is called before the first frame update
    void Start()
    {
        infoImage = setInfoImage;
        pages = pagesHolder.GetComponentsInChildren<PlayMakerFSM>();
        pageButtons = new Button[pages.Length];
        //int i = 0;
        foreach (PlayMakerFSM page in pages)
        {
            page.SendEvent("myStart");
            page.FsmVariables.GetFsmGameObject("mySelf").CastVariable = new FsmGameObject(page.gameObject);
            page.FsmVariables.GetFsmGameObject("book position").CastVariable = new FsmGameObject(bookUpPos);
            page.FsmVariables.GetFsmGameObject("book").CastVariable = new FsmGameObject(gameObject);
            //pageButtons[i] = page.GetComponentInChildren<Button>();
            //pageButtons[i].gameObject.SetActive(false);
            //i++;
        }
    }

    //Used by this gameobject to reactivate pages/cards gameobjects
    public void ScatterPages()
    {
        foreach (PlayMakerFSM page in pages)
        {
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
