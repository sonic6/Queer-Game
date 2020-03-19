using UnityEngine.UI;
using UnityEngine;

public class WinOrLose : MonoBehaviour
{
    [SerializeField] GameObject uiScreen;
    private static GameObject myUiScreen;

    private void Start()
    {
        myUiScreen = uiScreen;
    }

    public static void LoseGame()
    {
        Time.timeScale = 0;
        myUiScreen.GetComponentInChildren<Text>().text = "You Lose";
        myUiScreen.SetActive(true);
    }

    public static void WinGame()
    {
        Time.timeScale = 0;
        myUiScreen.GetComponentInChildren<Text>().text = "You Win!";
        myUiScreen.SetActive(true);
    }
}
