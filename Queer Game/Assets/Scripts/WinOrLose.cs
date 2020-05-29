using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinOrLose : MonoBehaviour
{
    [SerializeField] GameObject uiScreen;
    public static GameObject myUiScreen;

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

    public void ReplayLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
