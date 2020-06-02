using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Tooltip("This image will be the first thing the player sees in the main menu. After pressing a button, it fades away")]
    [SerializeField] Image startImage;

    [SerializeField] TMP_Text myText;

    Animator CameraAnimator;
    float skyRotation = 0;

    //Becomes true on the very first button or mouse click the player clicks
    bool fadeStart = false;

    public Image[] Buttons;
    public Text[] buttonTexts;
    public Image[] levelButtons;
    public Image loadingScreen;
    public GameObject loadscrnTxt;

    float loadingOgColor;

    private void Awake()
    {
        loadingOgColor = loadingScreen.color.a;
        loadingScreen.color = Color.clear;
        FadeAllButtons(Buttons, 0, 0, buttonTexts);
        EnableButtons(Buttons, false);

        FadeAllButtons(levelButtons, 0, 0);
        EnableButtons(levelButtons, false);
    }

    private void Start()
    {
        CameraAnimator = Camera.main.GetComponent<Animator>();
        StartCoroutine(FadeInOut());
    }

    void Update()
    {
        if(startImage.gameObject.activeSelf)
        {
            if (Input.anyKeyDown)
            {
                fadeStart = true;
            }
            
            if (fadeStart)
            {
                myText.gameObject.SetActive(false);
                Color main = startImage.color;
                startImage.color = new Color(main.r, main.g, main.b, main.a - 1 * Time.deltaTime);
                if (startImage.color.a <= 0)
                {
                    startImage.gameObject.SetActive(false);
                    EnableButtons(Buttons, true);
                    FadeAllButtons(Buttons,255, 1, buttonTexts);
                    
                }
            }
        }
        
        RenderSettings.skybox.SetFloat("_Rotation", skyRotation);
        skyRotation = .1f + skyRotation;

    }

    IEnumerator FadeInOut()
    {
        while(myText.gameObject.activeSelf)
        {
            if (myText.gameObject.activeSelf == false)
                break;

            var og = myText.color.a;
            while (myText.color.a > 0)
            {
                myText.color = new Color(myText.color.r, myText.color.g, myText.color.b, myText.color.a - 1 * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            while(myText.color.a < og)
            {
                myText.color = new Color(myText.color.r, myText.color.g, myText.color.b, myText.color.a + 1 * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            
        }
        yield break;
    }
    
    void FadeAllButtons(Image[] Buttons, float alpha, float time, Text[] buttonTexts = null)
    {
        foreach(Image button in Buttons)
        {
            button.CrossFadeAlpha(alpha, time, false);
        }
        if (buttonTexts != null)
        {
            foreach (Text txt in buttonTexts)
            {
                txt.CrossFadeAlpha(alpha, time, false);
            }
        }
    }

    void EnableButtons(Image[] Buttons, bool myBool)
    {
        foreach (Image button in Buttons)
        {
            button.gameObject.SetActive(myBool);
        }
    }

    //Quits the game
    public void QuitGame()
    {
        Application.Quit();
    }

    //Starts the process of introducing the player to Boyle and the game 
    public void NewGame()
    {
        StartCoroutine(Happen());
        IEnumerator Happen()
        {
            CameraAnimator.SetTrigger("turn");
            FadeAllButtons(Buttons, 0, 1, buttonTexts);
            yield return new WaitForSeconds(1);
            EnableButtons(Buttons, false);
        }
    }

    //Enables the level select menu
    public void LevelSelect()
    {
        StartCoroutine(Happen());
        IEnumerator Happen()
        {
            //Hide menu buttons
            FadeAllButtons(Buttons, 0, 1, buttonTexts);
            yield return new WaitForSeconds(1);
            EnableButtons(Buttons, false);

            //Show level buttons
            EnableButtons(levelButtons, true);
            FadeAllButtons(levelButtons, 255, 1);
        }
    }

    //Starts a unity scene after a fade in of the loading screen
    public void LaunchLevel(int sceneNumber)
    {
        StartCoroutine(Happen());
        IEnumerator Happen()
        {
            loadingScreen.gameObject.SetActive(true);
            loadscrnTxt.SetActive(true);
            while (loadingScreen.color.a < loadingOgColor)
            {
                loadingScreen.color = new Color(loadingScreen.color.r, loadingScreen.color.g, loadingScreen.color.b, loadingScreen.color.a + 1 * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            SceneManager.LoadScene(sceneNumber);
        }
        
    }

    //Broadcasts event to all FSM components in scene
    public void ClickedTextBox()
    {
        PlayMakerFSM.BroadcastEvent("clicked");
    }
}
