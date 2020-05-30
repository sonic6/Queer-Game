using System.Collections;
using System.Collections.Generic;
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
                    CameraAnimator.SetTrigger("turn");
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
}
