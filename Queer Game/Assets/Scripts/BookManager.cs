using UnityEngine;
using HutongGames.PlayMaker;
using UnityEngine.UI;

public class BookManager : MonoBehaviour
{
    [UnityEngine.Tooltip("The gameobject that all the 'pages' are attached to")]
    [SerializeField] GameObject pagesHolder;

    [UnityEngine.Tooltip("The gameobject that defines the 'up position' of the book")]
    [SerializeField] GameObject bookUpPos;

    PlayMakerFSM[] pages; //The pages where the "Memes" are
    Button[] pageButtons; //The buttons on each page in pages

    // Start is called before the first frame update
    void Start()
    {
        pages = pagesHolder.GetComponentsInChildren<PlayMakerFSM>();
        pageButtons = new Button[pages.Length];
        int i = 0;
        foreach (PlayMakerFSM page in pages)
        {
            page.SendEvent("myStart");
            page.FsmVariables.GetFsmGameObject("myButton").CastVariable = new FsmGameObject(page.gameObject.GetComponentInChildren<Button>().gameObject);
            page.FsmVariables.GetFsmGameObject("book position").CastVariable = new FsmGameObject(bookUpPos);
            page.FsmVariables.GetFsmGameObject("book").CastVariable = new FsmGameObject(gameObject);
            pageButtons[i] = page.GetComponentInChildren<Button>();
            pageButtons[i].gameObject.SetActive(false);
            i++;
        }
    }

    //Used by FSM on pages. Sets the pages' buttons' gameobjects to enabled/disabled
    public void FlipPagesActiveState(GameObject button) 
    {
        button.gameObject.SetActive(!button.gameObject.activeSelf);
    }

    
}
